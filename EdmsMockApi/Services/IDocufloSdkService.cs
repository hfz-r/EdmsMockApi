using System.Threading.Tasks;
using ServiceReference;

namespace EdmsMockApi.Services
{
    public interface IDocufloSdkService
    {
        Task<DocufloSDKSoap> Connect();
        Task<DataProfileResult[]> GetProfileSearch(ProfileSearchRequestBody requestBody);
        Task<DataProfileResult[]> GetSearch(SearchRequestBody requestBody);
        Task<DataProfileResult[]> GetSearchByDocId(SearchByDocIDRequestBody requestBody);
        Task<string> Login(LoginRequestBody requestBody);
    }
}