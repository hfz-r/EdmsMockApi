using EdmsMockApi.Dtos.DataProfiles;
using ServiceReference;

namespace EdmsMockApi.Infrastructure.Mapper.Extensions
{
    public static class DataProfileDtoMappings
    {
        public static DataProfileDto ToDto(this DataProfileResult dataProfile)
        {
            return dataProfile.MapTo<DataProfileResult, DataProfileDto>();
        }

        public static DataProfileResult ToEntity(this DataProfileDto dataProfileDto)
        {
            return dataProfileDto.MapTo<DataProfileDto, DataProfileResult>();
        }
    }
}