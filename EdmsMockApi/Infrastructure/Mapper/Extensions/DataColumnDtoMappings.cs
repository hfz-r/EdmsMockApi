using EdmsMockApi.Dtos.DataProfiles;
using ServiceReference;

namespace EdmsMockApi.Infrastructure.Mapper.Extensions
{
    public static class DataColumnDtoMappings
    {
        public static DataColumnDto ToDto(this DataColumn dataColumn)
        {
            return dataColumn.MapTo<DataColumn, DataColumnDto>();
        }

        public static DataColumn ToEntity(this DataColumnDto dataColumnDto)
        {
            return dataColumnDto.MapTo<DataColumnDto, DataColumn>();
        }
    }
}