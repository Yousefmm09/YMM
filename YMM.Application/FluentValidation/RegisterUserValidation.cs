using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Dto.Auth;

namespace YMM.Application.FluentValidation
{
    public class RegisterUserValidation:AbstractValidator<RegisterRequestDto>
    {
        public RegisterUserValidation()
        {
            RuleFor(x => x.UserName).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("UserName is Empty")
                .MinimumLength(3)
                .WithMessage("Username must be between 3 and 50 characters");

            RuleFor(x => x.Password).Cascade(CascadeMode.Stop).MinimumLength(6).MaximumLength(100).NotEmpty()
                .WithMessage("Password must be at least 6 characters");
                
            RuleFor(x => x).Must(x => x.ConfirmPassword == x.Password)
                .WithMessage("The ConfirmPassword not match password");

            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is empty")
                .EmailAddress();

            RuleFor(x => x.PhoneNumber).NotNull().WithMessage("Phone number is required");

            RuleFor(x => x.Country).NotNull().MaximumLength(100).NotEmpty().WithMessage("Country is required");

            RuleFor(x => x.Age).NotEmpty().When(x => x.Age <= 18).WithMessage("Your age is least of 18 ");
        }
    }
}
