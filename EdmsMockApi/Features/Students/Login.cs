using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using EdmsMockApi.Services;
using MediatR;
using ServiceReference;

namespace EdmsMockApi.Features.Students
{
    public class Login
    {
        public class Query : IRequest<string>
        {
            /// <summary>
            /// E-DMS username 
            /// </summary>
            [Required]
            public string Username { get; set; }

            /// <summary>
            /// E-DMS password
            /// </summary>
            [Required]
            public string UserPassword { get; set; }
        }

        public class Handler : IRequestHandler<Query, string>
        {
            private readonly IDocufloSdkService _docufloSdkService;

            public Handler(IDocufloSdkService docufloSdkService)
            {
                _docufloSdkService = docufloSdkService;
            }

            public async Task<string> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _docufloSdkService.Login(new LoginRequestBody
                {
                    userName = request.Username,
                    userPassword = request.UserPassword
                });

                return string.IsNullOrEmpty(result) ? null : result;
            }
        }
    }
}