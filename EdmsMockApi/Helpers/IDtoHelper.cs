using EdmsMockApi.Dtos.DataProfiles;
using EdmsMockApi.Dtos.ProfileFields;
using EdmsMockApi.Dtos.Profiles;
using EdmsMockApi.Entities;
using ServiceReference;
using ProfileField = EdmsMockApi.Entities.ProfileField;

namespace EdmsMockApi.Helpers
{
    public interface IDtoHelper
    {
        ProfileDto PrepareProfileDto(Profile profile);

        ProfileFieldDto PrepareProfileFieldDto(ProfileField profileField);

        DataProfileDto PrepareDataProfileDto(DataProfileResult dataProfile);

        DataColumnDto PrepareDataColumnDto(DataColumn dataColumn);
    }
}