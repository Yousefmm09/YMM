using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Dto.Brand;

namespace YMM.Application.FluentValidation
{
    public class BrandValidation:AbstractValidator<CreatBrandDto>
    {
        public BrandValidation()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("the brand name is required")
                .MinimumLength(3).WithMessage("The brand must at least 3 character");
            RuleFor(x => x.Description).NotEmpty().MaximumLength(250);
        }
    }
}
