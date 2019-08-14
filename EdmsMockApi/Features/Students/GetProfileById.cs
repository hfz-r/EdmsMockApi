using System.Threading;
using System.Threading.Tasks;
using EdmsMockApi.Data.Repositories;
using EdmsMockApi.Dtos.Profiles;
using EdmsMockApi.Entities;
using EdmsMockApi.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EdmsMockApi.Features.Students
{
    public class GetProfileById
    {
        public class Query : IRequest<ProfileDto>
        {
            /// <summary>
            /// Profile id
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// Comma-separated list of fields to include in the response
            /// </summary>
            public string Fields { get; set; } 
        }

        public class Handler : IRequestHandler<Query, ProfileDto>
        {
            private readonly IRepository<Profile> _profileRepository;
            private readonly IDtoHelper _dtoHelper;

            public Handler(IRepository<Profile> profileRepository, IDtoHelper dtoHelper)
            {
                _profileRepository = profileRepository;
                _dtoHelper = dtoHelper;
            }

            public async Task<ProfileDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var profile = await _profileRepository.Table.FirstOrDefaultAsync(p => p.ProfileId == request.Id, cancellationToken);
                if (profile == null)
                    return null;

                var profileAsDto = _dtoHelper.PrepareProfileDto(profile);

                return profileAsDto;
            }
        }
    }
}