using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using EdmsMockApi.Data.Repositories;
using EdmsMockApi.Entities;
using EdmsMockApi.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ServiceReference;

namespace EdmsMockApi.Features.Students
{
    public class Export
    {
        public class Command : IRequest<string>
        {
            /// <summary>
            /// File content in base64 string
            /// </summary>
            [Required]
            [JsonProperty("file_content")]
            public string FileContent { get; set; }

            /// <summary>
            /// File name
            /// </summary>
            [JsonProperty("file_name")]
            public string FileName { get; set; }

            /// <summary>
            /// Exporter current profile identifier
            /// </summary>
            [JsonProperty("profile_id")]
            public int ProfileId { get; set; }

            /// <summary>
            /// Exporter current profile value
            /// </summary>
            [JsonProperty("profile_value")]
            public ArrayOfString ProfileValue { get; set; }

            /// <summary>
            /// EDMS folder name
            /// </summary>
            [JsonProperty("folder_name")]
            public string FolderName { get; set; }

            /// <summary>
            /// Exporter username
            /// </summary>
            [JsonProperty("user_name")]
            public string UserName { get; set; }
        }

        public class Handler : IRequestHandler<Command, string>
        {
            private readonly IDocufloSdkService _docufloSdkService;
            private readonly IRepository<Profile> _profileRepository;

            public Handler(IDocufloSdkService docufloSdkService, IRepository<Profile> profileRepository)
            {
                _docufloSdkService = docufloSdkService;
                _profileRepository = profileRepository;
            }

            public async Task<string> Handle(Command command, CancellationToken cancellationToken)
            {
                var export = await _docufloSdkService.Export(new ExportRequestBody
                {
                    FileContent = !string.IsNullOrEmpty(command.FileContent) ? Convert.FromBase64String(command.FileContent) : throw new ArgumentNullException(nameof(command.FileContent)),
                    strFileName = command.FileName ?? string.Empty,
                    strProfile = (await _profileRepository.Table.FirstOrDefaultAsync(p => p.ProfileId == command.ProfileId, cancellationToken))?.ProfileName ?? string.Empty,
                    strFolderName = command.FolderName ?? string.Empty,
                    arrProfileValue = command.ProfileValue,
                    userID = command.UserName ?? string.Empty,
                });

                return string.IsNullOrEmpty(export) ? null : export;
            }
        }
    }
}