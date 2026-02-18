using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Dto.Auth;

namespace YMM.Application.FluentValidation
{
    public class LoginValidation:AbstractValidator<LoginRequestDto>
    {
        public LoginValidation()
        {
            RuleFor(x => x.Email).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("Email is required")
                .EmailAddress();

            RuleFor(x => x.Password).Cascade(CascadeMode.Stop).NotNull().WithMessage("Password is required");
        }
    }
}
