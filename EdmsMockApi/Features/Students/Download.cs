using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using EdmsMockApi.Dtos.Profiles;
using EdmsMockApi.Helpers;
using EdmsMockApi.Infrastructure.ModelBinders;
using EdmsMockApi.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceReference;

namespace EdmsMockApi.Features.Students
{
    public class Download
    {
        [ModelBinder(typeof(ParametersModelBinder<Query>))]
        public class Query : IRequest<DownloadDto>
        {
            /// <summary>
            ///Version id
            /// </summary>
            [Required]
            [JsonProperty("ver_id")]
            public int VerId { get; set; }

            /// <summary>
            /// Profile id
            /// </summary>
            [Required]
            [JsonProperty("profile_id")]
            public int ProfileId { get; set; }

            /// <summary>
            /// File type
            /// </summary>
            [JsonProperty("file_type")]
            public Int16 FileType { get; set; }
        }

        public class Handler : IRequestHandler<Query, DownloadDto>
        {
            private readonly IDocufloSdkService _docufloSdkService;
            private readonly IDtoHelper _dtoHelper;

            public Handler(IDocufloSdkService docufloSdkService, IDtoHelper dtoHelper)
            {
                _docufloSdkService = docufloSdkService;
                _dtoHelper = dtoHelper;
            }

            public async Task<DownloadDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var responseBody = await _docufloSdkService.Download(new DownloadRequestBody
                {
                    VerID = request.VerId,
                    DocProfileID = request.ProfileId,
                    FileType = request.FileType
                });

                var downloadDto = _dtoHelper.PrepareDownloadDto(responseBody);
                if (downloadDto.Result != "1")
                    throw new ArgumentNullException(nameof(downloadDto));

                return downloadDto;
            }
        }
    }
}