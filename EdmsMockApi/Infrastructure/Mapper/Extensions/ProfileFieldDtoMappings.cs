using EdmsMockApi.Dtos.ProfileFields;
using EdmsMockApi.Entities;

namespace EdmsMockApi.Infrastructure.Mapper.Extensions
{
    public static class ProfileFieldDtoMappings
    {
        public static ProfileFieldDto ToDto(this ProfileField profileField)
        {
            return profileField.MapTo<ProfileField, ProfileFieldDto>();
        }

        public static ProfileField ToEntity(this ProfileFieldDto profileFieldDto)
        {
            return profileFieldDto.MapTo<ProfileFieldDto, ProfileField>();
        }
    }
}