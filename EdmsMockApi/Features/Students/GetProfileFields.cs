using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EdmsMockApi.Data.Repositories;
using EdmsMockApi.Dtos.ProfileFields;
using EdmsMockApi.Entities;
using EdmsMockApi.Helpers;
using MediatR;

namespace EdmsMockApi.Features.Students
{
    public class GetProfileFields
    {
        public class Query : IRequest<IList<ProfileFieldDto>>
        {
            /// <summary>
            /// Profile id
            /// </summary>
            public int ProfileId { get; set; }

            /// <summary>
            /// Comma-separated list of fields to include in the response
            /// </summary>
            public string Fields { get; set; }
        }

        public class Handler : IRequestHandler<Query, IList<ProfileFieldDto>>
        {
            private readonly IRepository<ProfileField> _profileFieldRepository;
            private readonly IDtoHelper _dtoHelper;

            public Handler(IRepository<ProfileField> profileFieldRepository, IDtoHelper dtoHelper)
            {
                _profileFieldRepository = profileFieldRepository;
                _dtoHelper = dtoHelper;
            }

            public Task<IList<ProfileFieldDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var fields = _profileFieldRepository.Table.Where(f => f.Profile.ProfileId == request.ProfileId);
                if (!fields.Any())
                    return Task.FromResult<IList<ProfileFieldDto>>(null);

                IList<ProfileFieldDto> fieldDtos = fields.Select(f => _dtoHelper.PrepareProfileFieldDto(f)).ToList();

                return Task.FromResult(fieldDtos);
            }
        }
    }
}