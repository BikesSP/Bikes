using ClientService.Application.Auth.Model;
using ClientService.Domain.Wrappers;
using FluentValidation;
using MediatR;

namespace ClientService.Application.Auth.Command
{
    public class LoginWithGoogleRequestValidator : AbstractValidator<LoginWithGoogleRequest>
    {
        public LoginWithGoogleRequestValidator()
        {
            RuleFor(model => model.IdToken)
                .NotEmpty();
        }
    }

    public class LoginWithGoogleRequest : IRequest<Response<TokenResponse?>>
    {
        public string IdToken { get; set; } = default!;
    }

}
