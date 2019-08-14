using System.Linq;
using EdmsMockApi.Dtos.DataProfiles;
using EdmsMockApi.Dtos.ProfileFields;
using EdmsMockApi.Dtos.Profiles;
using EdmsMockApi.Infrastructure.Mapper.Extensions;
using ServiceReference;
using Profile = AutoMapper.Profile;
using ProfileField = EdmsMockApi.Entities.ProfileField;

namespace EdmsMockApi.Infrastructure.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateProfileFieldMap();
            CreateProfileMap();

            CreateDataColumnMap();
            CreateDataProfileMap();
        }

        private new static void CreateMap<TSource, TDestination>()
        {
            AutoMapperApiConfiguration.MapperConfigurationExpression.CreateMap<TSource, TDestination>()
                .IgnoreAllNonExisting();
        }

        private static void CreateProfileFieldMap()
        {
            CreateMap<ProfileField, ProfileFieldDto>();    
        }

        private static void CreateProfileMap()
        {
            AutoMapperApiConfiguration.MapperConfigurationExpression.CreateMap<Entities.Profile, ProfileDto>()
                .IgnoreAllNonExisting()
                .ForMember(x => x.ProfileId, y => y.MapFrom(src => src.ProfileId))
                .ForMember(x => x.ProfileFields, y => y.MapFrom(src => src.ProfileFields.Select(x => x.ToDto())));
        }

        private static void CreateDataColumnMap()
        {
            CreateMap<DataColumn, DataColumnDto>();
        }

        private static void CreateDataProfileMap()
        {
            AutoMapperApiConfiguration.MapperConfigurationExpression.CreateMap<DataProfileResult, DataProfileDto>()
                .IgnoreAllNonExisting()
                .ForMember(x => x.DataColumns, y => y.MapFrom(src => src.Arr_DataValue.Select(x => x.ToDto())));
        }
    }
}