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
    public class Search
    {
        [ModelBinder(typeof(ParametersModelBinder<Query>))]
        public class Query : BaseSearchDto, IRequest<IList<DataProfileDto>>
        {
            /// <summary>
            /// Search criteria
            /// </summary>
            [Required]
            [JsonProperty("criteria")]
            public string Criteria { get; set; }
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
                var profile = await _docufloSdkService.GetSearch(new SearchRequestBody
                {
                    strCriteria = request.Criteria
                });

                var dataProfiles = profile.AsQueryable().GetSearchQuery(request.Query, request.Page, request.Limit, request.Order);

                IList<DataProfileDto> dataProfilesAsDto = dataProfiles.Select(data => _dtoHelper.PrepareDataProfileDto(data)).ToList();

                return dataProfilesAsDto;
            }
        }
    }
}