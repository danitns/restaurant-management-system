using FluentValidation;
using Restaurant.BusinessLogic.Implementation.Restaurants.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BusinessLogic.Implementation.Restaurants.Validations
{
    public class CreateBenefitValidator: AbstractValidator<CreateBenefitModel>
    {
        public CreateBenefitValidator()
        {
            RuleFor(t => t.Name).NotEmpty().WithMessage("Please enter name");
            RuleFor(t => t.Value).NotEmpty().WithMessage("Please enter discount value");
            RuleFor(t => t.Details).NotEmpty().WithMessage("Please enter details");
        }
    }
}
