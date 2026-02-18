using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Dto.Coupon;

namespace YMM.Application.FluentValidation
{
    public class CreateCouponDtoValidator : AbstractValidator<CreateCouponDto>
    {
        public CreateCouponDtoValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Coupon code is required.")
                .MinimumLength(3)
                .MaximumLength(20)
                .Matches("^[A-Z0-9_-]+$")
                .WithMessage("Coupon code must contain only uppercase letters, numbers, hyphen or underscore.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500);

            RuleFor(x => x.DiscountType)
                .NotEmpty()
                .Must(x => x == "Percentage" || x == "Fixed")
                .WithMessage("DiscountType must be either 'Percentage' or 'Fixed'.");

            RuleFor(x => x.DiscountValue)
                .GreaterThan(0)
                .WithMessage("Discount value must be greater than zero.");

            RuleFor(x => x)
                .Must(x =>
                    x.DiscountType != "Percentage" ||
                    (x.DiscountValue > 0 && x.DiscountValue <= 100))
                .WithMessage("Percentage discount must be between 0 and 100.");

            RuleFor(x => x.MinPurchaseAmount)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.MaxDiscountAmount)
                .GreaterThan(0)
                .When(x => x.DiscountType == "Percentage")
                .WithMessage("MaxDiscountAmount is required when discount type is Percentage.");

            RuleFor(x => x.StartDate)
                .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
                .WithMessage("Start date cannot be in the past.");

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .WithMessage("End date must be after start date.");

            RuleFor(x => x.UsageLimit)
                .GreaterThan(0)
                .WithMessage("UsageLimit must be greater than zero.");

            RuleFor(x => x.UsageLimitPerUser)
                .GreaterThan(0)
                .LessThanOrEqualTo(x => x.UsageLimit)
                .WithMessage("UsageLimitPerUser must be less than or equal to UsageLimit.");
        }
    }

}
