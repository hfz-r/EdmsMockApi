using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using EdmsMockApi.Data.Repositories;
using EdmsMockApi.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceReference;
using ProfileField = ServiceReference.ProfileField;

namespace EdmsMockApi.Data
{
    public static class DbInitializer
    {
        #region Private methods

        private static async Task<IList<Profile>> SeedProfilesData(ChannelFactory<DocufloSDKSoap> factory, Func<Profiles[], IList<Profile>> mapToEntityFunc)
        {
            var client = factory.CreateChannel();

            var request = new LoadProfilesRequest
            {
                Body = new LoadProfilesRequestBody()
            };

            var profilesResult = (await client.LoadProfilesAsync(request))?.Body?.LoadProfilesResult;
            if (profilesResult == null)
                return null;

            var profiles = mapToEntityFunc(profilesResult);

            if (profiles.Count > 0)
                ((IClientChannel)client).Close();

            return profiles;
        }

        private static void SeedProfileFieldsData(Profile profile, IEnumerable<ProfileField> profileFields, Action<IEnumerable<Entities.ProfileField>> mapToEntityFunc)
        {
            var result = profileFields.Select(pf =>
                new Entities.ProfileField
                {
                    Profile = profile,
                    ColId = pf.colID,
                    ColName = pf.colName,
                    ColDesc = pf.colDesc,
                    ColDataType = pf.colDataType,
                });

            mapToEntityFunc(result);
        }

        private static IDictionary<Profile, ProfileField[]> GetProfileFieldResult(DocufloSDKSoap client, IEnumerable<Profile> profiles)
        {
            var result = new Dictionary<Profile, ProfileField[]>();

            foreach (var profile in profiles)
            {
                var response = client.LoadProfileFieldAsync(new LoadProfileFieldRequest
                {
                    Body = new LoadProfileFieldRequestBody(profile.ProfileId)
                }).GetAwaiter().GetResult();

                result.Add(profile, response?.Body?.LoadProfileFieldResult);
            }

            return result;
        }

        #endregion

        public static void Initialize(IServiceProvider service)
        {
            var configuration = service.GetRequiredService<IConfiguration>();

            //ConfigureWebservice(configuration, service);
        }

        public static void ConfigureWebservice(IConfiguration configuration, IServiceProvider service)
        {
            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress(configuration.GetSection("DocufloSDKEndPoint").Value);

            var factory = new ChannelFactory<DocufloSDKSoap>(binding, endpoint);

            if (GetProfileData(service, factory) && GetProfileFieldData(service, factory))
                return;

            Console.WriteLine("Data already seeded.");
        }

        public static bool GetProfileData(IServiceProvider service, ChannelFactory<DocufloSDKSoap> factory)
        {
            var profileRepository = service.GetRequiredService<IRepository<Profile>>();
            if (profileRepository.Table.Any())
            {
                profileRepository.DeleteAsync(profileRepository.Table).GetAwaiter().GetResult();
            }

             var result = SeedProfilesData(factory, profileResult =>
                profileResult.Select(p =>
                {
                    var profile = new Profile
                    {
                        ProfileId = p.profileID,
                        ProfileName = p.profileName
                    };

                    profileRepository.InsertAsync(profile).GetAwaiter().GetResult();

                    return profile;

                }).ToList()).GetAwaiter().GetResult();

            return result.Any();
        }

        public static bool GetProfileFieldData(IServiceProvider service, ChannelFactory<DocufloSDKSoap> factory)
        {
            var profileFieldRepository = service.GetRequiredService<IRepository<Entities.ProfileField>>();
            if (profileFieldRepository.Table.Any())
            {
                profileFieldRepository.DeleteAsync(profileFieldRepository.Table).GetAwaiter().GetResult();
            }

            var profileRepository = service.GetRequiredService<IRepository<Profile>>();
            if (!profileRepository.Table.Any())
                return GetProfileData(service, factory);

            var client = factory.CreateChannel();

            foreach (var result in GetProfileFieldResult(client, profileRepository.Table.ToList()))
            {
                SeedProfileFieldsData(result.Key, result.Value, fields => profileFieldRepository.InsertAsync(fields).GetAwaiter().GetResult());
            }

            return profileFieldRepository.Table.Any();
        }
    }
}