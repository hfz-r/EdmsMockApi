using EdmsMockApi.Dtos.Profiles;
using ServiceReference;

namespace EdmsMockApi.Infrastructure.Mapper.Extensions
{
    public static class DownloadDtoMappings
    {
        public static DownloadDto ToDto(this DownloadResponseBody responseBody)
        {
            return responseBody.MapTo<DownloadResponseBody, DownloadDto>();
        }

        public static DownloadResponseBody ToEntity(this DownloadDto downloadDto)
        {
            return downloadDto.MapTo<DownloadDto, DownloadResponseBody>();
        }
    }
}