using System.Linq;
using EdmsMockApi.Dtos.DataProfiles;
using EdmsMockApi.Dtos.ProfileFields;
using EdmsMockApi.Dtos.Profiles;
using EdmsMockApi.Entities;
using EdmsMockApi.Infrastructure.Mapper.Extensions;
using ServiceReference;
using ProfileField = EdmsMockApi.Entities.ProfileField;

namespace EdmsMockApi.Helpers
{
    public class DtoHelper : IDtoHelper
    {
        public DtoHelper()
        {
        }

        public ProfileDto PrepareProfileDto(Profile profile)
        {
            var profileDto = profile.ToDto();

            profileDto.ProfileFields = profile.ProfileFields.Select(PrepareProfileFieldDto).ToList();

            return profileDto;
        }

        public ProfileFieldDto PrepareProfileFieldDto(ProfileField profileField)
        {
            var profileFieldDto = profileField.ToDto();

            return profileFieldDto;
        }

        public DataProfileDto PrepareDataProfileDto(DataProfileResult dataProfile)
        {
            var dataProfileDto = dataProfile.ToDto();

            dataProfileDto.DataColumns = dataProfile.Arr_DataValue.Select(PrepareDataColumnDto).ToList();

            return dataProfileDto;
        }

        public DataColumnDto PrepareDataColumnDto(DataColumn dataColumn)
        {
            var dataColumnDto = dataColumn.ToDto();

            return dataColumnDto;
        }
    }
}