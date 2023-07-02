using ClientService.Application.Auth.Model;
using ClientService.Domain.Wrappers;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.Auth.Command
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(model => model.RefreshToken)
                .NotEmpty();
        }
    }

    public class RefreshTokenRequest : IRequest<Response<TokenResponse>>
    {
        public string RefreshToken { get; set; } = default!;
    }

}
