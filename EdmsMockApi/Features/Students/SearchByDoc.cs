using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EdmsMockApi.Dtos.DataProfiles;
using EdmsMockApi.Extensions;
using EdmsMockApi.Helpers;
using EdmsMockApi.Infrastructure.ModelBinders;
using EdmsMockApi.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceReference;

namespace EdmsMockApi.Features.Students
{
    public class SearchByDoc
    {
        [ModelBinder(typeof(ParametersModelBinder<Query>))]
        public class Query : BaseSearchDto, IRequest<IList<DataProfileDto>>
        {
            /// <summary>
            /// Profile id
            /// </summary>
            [Required]
            [JsonProperty("profile_id")]
            public int ProfileId { get; set; }

            /// <summary>
            /// Document id
            /// </summary>
            [Required]
            [JsonProperty("doc_id")]
            public long DocId { get; set; }
        }

        public class Handler : IRequestHandler<Query, IList<DataProfileDto>>
        {
            private readonly IDocufloSdkService _docufloSdkService;
            private readonly IDtoHelper _dtoHelper;

            public Handler(IDocufloSdkService docufloSdkService, IDtoHelper dtoHelper)
            {
                _docufloSdkService = docufloSdkService;
                _dtoHelper = dtoHelper;
            }

            public async Task<IList<DataProfileDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (request.ProfileId <= 0)
                    return null;

                var profile = await _docufloSdkService.GetSearchByDocId(new SearchByDocIDRequestBody
                {
                    profileID = request.ProfileId,
                    docID = request.DocId
                });

                var dataProfiles = profile.AsQueryable().GetSearchQuery(request.Query, request.Page, request.Limit, request.Order);

                IList<DataProfileDto> dataProfilesAsDto = dataProfiles.Select(data => _dtoHelper.PrepareDataProfileDto(data)).ToList();

                return dataProfilesAsDto;
            }
        }
    }
}