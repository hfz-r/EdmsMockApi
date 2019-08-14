using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EdmsMockApi.Converters;
using EdmsMockApi.Data.Repositories;
using EdmsMockApi.Dtos.Profiles;
using EdmsMockApi.Entities;
using EdmsMockApi.Helpers;
using EdmsMockApi.Infrastructure.ModelBinders;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EdmsMockApi.Features.Students
{
    public class GetProfiles
    {
        [ModelBinder(typeof(ParametersModelBinder<Query>))]
        public class Query : IRequest<IList<ProfileDto>>
        {
            public Query()
            {
                Ids = null;
                Limit = Configurations.DefaultLimit;
                Page = Configurations.DefaultPageValue;
                SinceId = Configurations.DefaultSinceId;
                Fields = string.Empty;
            }

            /// <summary>
            /// A comma-separated list of profile ids
            /// </summary>
            [JsonProperty("ids")]
            public List<int> Ids { get; set; }

            /// <summary>
            /// Amount of results (default: 50) (maximum: 250)
            /// </summary>
            [JsonProperty("limit")]
            public int Limit { get; set; }

            /// <summary>
            /// Page to show (default: 1)
            /// </summary>
            [JsonProperty("page")]
            public int Page { get; set; }

            /// <summary>
            /// Restrict results to after the specified ID
            /// </summary>
            [JsonProperty("since_id")]
            public int SinceId { get; set; }

            /// <summary>
            /// Comma-separated list of fields to include in the response
            /// </summary>
            [JsonProperty("fields")]
            public string Fields { get; set; }
        }

        public class Handler : IRequestHandler<Query, IList<ProfileDto>>
        {
            private readonly IRepository<Profile> _profileRepository;
            private readonly IDtoHelper _dtoHelper;

            public Handler(IRepository<Profile> profileRepository, IDtoHelper dtoHelper)
            {
                _profileRepository = profileRepository;
                _dtoHelper = dtoHelper;
            }

            public Task<IList<ProfileDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = GetProfileQuery(request.Ids);

                if (request.SinceId > 0)
                    query = query.Where(profile => profile.Id > request.SinceId);

                IList<Profile> profiles = new ApiList<Profile>(query, request.Page - 1, request.Limit);

                IList<ProfileDto> profilesAsDto = profiles.Select(profile => _dtoHelper.PrepareProfileDto(profile)).ToList();

                return Task.FromResult(profilesAsDto);
            }

            private IQueryable<Profile> GetProfileQuery(ICollection<int> ids = null)
            {
                var query = _profileRepository.Table;

                if (ids != null && ids.Count > 0)
                    query = query.Where(c => ids.Contains(c.ProfileId));

                query = query.OrderBy(profile => profile.ProfileId);

                return query;
            }
        }
    }
}