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
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(model => model.Email)
                .NotEmpty();
            RuleFor(model => model.Password)
                .NotEmpty();
        }
    }

    public class LoginRequest : IRequest<Response<TokenResponse?>>
    {
        public string Email { get; set; } = default!;

        public string Password { get; set; } = default!;
    }

}
