using EdmsMockApi.Dtos.Profiles;
using EdmsMockApi.Entities;

namespace EdmsMockApi.Infrastructure.Mapper.Extensions
{
    public static class ProfileDtoMappings
    {
        public static ProfileDto ToDto(this Profile profile)
        {
            return profile.MapTo<Profile, ProfileDto>();
        }

        public static Profile ToEntity(this ProfileDto profileDto)
        {
            return profileDto.MapTo<ProfileDto, Profile>();
        }
    }
}