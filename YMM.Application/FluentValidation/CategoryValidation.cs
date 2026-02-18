using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Dto.Category;

namespace YMM.Application.FluentValidation
{
    public class CategoryValidation:AbstractValidator<CreatCategoryDto>
    {
        public CategoryValidation()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("the category name is required")
              .MinimumLength(3).WithMessage("The category must at least 3 character");
            RuleFor(x => x.Description).NotEmpty().MaximumLength(250);
        }
    }
}
