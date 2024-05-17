using FluentValidation;
using Restaurant.BusinessLogic.CommonFunc;
using Restaurant.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BusinessLogic.Implementation.Reservations.Validations
{
    public class CreateReservationValidator : AbstractValidator<CreateReservationModel>
    {
        public CreateReservationValidator()
        {
            RuleFor(r => r.Phone)
                .Length(10, 12).WithMessage("Length between 10 and 12 characters")
                .NotEmpty().WithMessage("Please enter yout phone number")
                .Must(ValidationFunctions.ContainsOnlyNumbersWithOptionalPlus).WithMessage("Enter valid a format");

        }
    }
}
