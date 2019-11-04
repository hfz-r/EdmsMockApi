using System.Threading.Tasks;
using ServiceReference;

namespace EdmsMockApi.Services
{
    public interface IDocufloSdkService
    {
        Task<DataProfileResult[]> GetProfileSearch(ProfileSearchRequestBody requestBody);
        Task<DataProfileResult[]> GetSearch(SearchRequestBody requestBody);
        Task<DataProfileResult[]> GetSearchByDocId(SearchByDocIDRequestBody requestBody);
        Task<string> Login(LoginRequestBody requestBody);
        Task<string> Export(ExportRequestBody requestBody);
        Task<DownloadResponseBody> Download(DownloadRequestBody requestBody);
    }
}