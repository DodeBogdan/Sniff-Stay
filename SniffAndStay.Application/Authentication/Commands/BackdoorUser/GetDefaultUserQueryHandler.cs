using MediatR;
using Microsoft.Extensions.Options;
using SniffAndStay.Application.Authentication.Response;

namespace SniffAndStay.Application.Authentication.Commands.BackdoorUser
{
    public record GetDefaultUserQuery() : IRequest<AuthenticationResponse>;

    public class GetDefaultUserQueryHandler : IRequestHandler<GetDefaultUserQuery, AuthenticationResponse>
    {
        private readonly DefaultUserConfiguration _defaultUserConfiguration;
        private readonly IMediator _mediator;
        public GetDefaultUserQueryHandler(
            IOptions<DefaultUserConfiguration> defaultUserConfiguration,
            IMediator mediator)
        {
            _defaultUserConfiguration = defaultUserConfiguration.Value;
            _mediator = mediator;
        }
        public async Task<AuthenticationResponse> Handle(GetDefaultUserQuery request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new LoginCommand(_defaultUserConfiguration.Email, _defaultUserConfiguration.Password), cancellationToken);
            return result;
        }
    }
}
