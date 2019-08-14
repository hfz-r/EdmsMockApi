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
using ServiceReference;

namespace EdmsMockApi.Features.Students
{
    public class ProfileSearch
    {
        [ModelBinder(typeof(ParametersModelBinder<Query>))]
        public class Query : BaseSearchDto, IRequest<IList<DataProfileDto>>
        {
            /// <summary>
            /// Profile name
            /// </summary>
            [Required]
            public string ProfileName { get; set; }

            /// <summary>
            /// Column name
            /// </summary>
            [Required]
            public List<string> ColumnNames { get; set; }

            /// <summary>
            /// Column value 
            /// </summary>
            [Required]
            public List<string> ColumnKeywords { get; set; }

            /// <summary>
            /// Error message
            /// </summary>
            public string ErrorMsg { get; set; }
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
                if (string.IsNullOrEmpty(request.ProfileName))
                    return null;

                var profile = await _docufloSdkService.GetProfileSearch(new ProfileSearchRequestBody
                {
                    profile_name = request.ProfileName,
                    column_names = request.ColumnNames.ToArrayOfAnyType(),
                    column_keywords = request.ColumnKeywords.ToArrayOfAnyType(),
                    error_msg = request.ErrorMsg
                });

                var dataProfiles = profile.AsQueryable().GetSearchQuery(request.Query, request.Page, request.Limit, request.Order);

                IList<DataProfileDto> dataProfilesAsDto = dataProfiles.Select(data => _dtoHelper.PrepareDataProfileDto(data)).ToList();

                return dataProfilesAsDto;
            }
        }
    }
}