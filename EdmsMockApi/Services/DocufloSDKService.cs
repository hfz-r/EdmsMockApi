using System;
using System.ServiceModel;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ServiceReference;

namespace EdmsMockApi.Services
{
    public class DocufloSdkService : IDocufloSdkService
    {
        private readonly IConfiguration _configuration;

        public DocufloSdkService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private async Task<DocufloSDKSoap> Connect()
        {
            var binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = 64000000;
            binding.MaxBufferSize = 64000000;

            var endpoint = new EndpointAddress(_configuration.GetSection("DocufloSDKEndPoint").Value);

            var factory = new ChannelFactory<DocufloSDKSoap>(binding, endpoint);
            var client = factory.CreateChannel();

            return await Task.FromResult(client);
        }

        public async Task<DataProfileResult[]> GetSearch(SearchRequestBody requestBody)
        {
            var request = new SearchRequest
            {
                Body = requestBody
            };

            var client = await Connect();

            var responseResult = (await client.SearchAsync(request))?.Body?.SearchResult;

            if (responseResult?.Length > 0)
                ((IClientChannel)client).Close();

            return responseResult;
        }

        public async Task<DataProfileResult[]> GetProfileSearch(ProfileSearchRequestBody requestBody)
        {
            var request = new ProfileSearchRequest
            {
                Body = requestBody
            };

            var client = await Connect();

            var response = (await client.ProfileSearchAsync(request))?.Body;

            if (!string.IsNullOrEmpty(response?.error_msg))
                throw new Exception(response.error_msg);

            var responseResult = response?.ProfileSearchResult;

            if (responseResult?.Length > 0)
                ((IClientChannel)client).Close();

            return responseResult;
        }

        public async Task<DataProfileResult[]> GetSearchByDocId(SearchByDocIDRequestBody requestBody)
        {
            var request = new SearchByDocIDRequest
            {
                Body = requestBody
            };

            var client = await Connect();

            var responseResult = (await client.SearchByDocIDAsync(request))?.Body?.SearchByDocIDResult;

            if (responseResult?.Length > 0)
                ((IClientChannel)client).Close();

            return responseResult;
        }

        public async Task<string> Login(LoginRequestBody requestBody)
        {
            var request = new LoginRequest
            {
                Body = requestBody
            };

            var client = await Connect();

            var responseResult = (await client.LoginAsync(request))?.Body.LoginResult ??
                                 (await client.LoginAsync(request))?.Body.userID;

            if (!string.IsNullOrEmpty(responseResult))
                ((IClientChannel)client).Close();

            return responseResult;
        }

        public async Task<string> Export(ExportRequestBody requestBody)
        {
            var request = new ExportRequest
            {
                Body = requestBody
            };

            var client = await Connect();

            var responseResult = (await client.ExportAsync(request))?.Body?.ExportResult;

            if (responseResult?.Length > 0)
                ((IClientChannel)client).Close();

            return responseResult;
        }

        public async Task<DownloadResponseBody> Download(DownloadRequestBody requestBody)
        {
            var request = new DownloadRequest
            {
                Body = requestBody
            };

            var client = await Connect();

            var responseResult = (await client.DownloadAsync(request))?.Body;

            if (responseResult != null && !string.IsNullOrEmpty(responseResult.DownloadResult))
                ((IClientChannel)client).Close();

            return responseResult;
        }
    }
}