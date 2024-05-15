using FluentValidation;
using Restaurant.BusinessLogic.CommonFunc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BusinessLogic.Implementation.Restaurants
{
	public  class CreateRestaurantValidator : AbstractValidator<CreateRestaurantModel>
	{
        public CreateRestaurantValidator()
        {
            RuleFor(r => r.Address).NotEmpty().WithMessage("Please enter the address");

			RuleFor(r => r.Name)
				.NotEmpty().WithMessage("Please enter restaurant's name.")
				.MinimumLength(3).WithMessage("Name should be at least 3 characters long.")
				.MaximumLength(100).WithMessage("Name cannot be longer than 100 characters.");

			RuleFor(r => r.CityId)
				.NotEmpty().WithMessage("Please select a city");
		}
    }
}
