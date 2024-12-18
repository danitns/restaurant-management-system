using FluentValidation;
using Restaurant.BusinessLogic.CommonFunc;
using Restaurant.BusinessLogic.Implementation.Tables.Models;
using Restaurant.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BusinessLogic.Implementation.Tables.Validations
{
    public class TableValidator : AbstractValidator<TableModel>
    {
        private readonly UnitOfWork _unitOfWork;
        public TableValidator(UnitOfWork unitOfWork)
        {
            RuleFor(t => t.Name)
                .NotEmpty().WithMessage("Please enter the nanme of the table")
                .Length(5, 40).WithMessage("Length between 5 and 40 characters");

            RuleFor(t => t)
                .Must(NotAlreadyExistTable).WithMessage("Table already exists for this restaurant");

            RuleFor(t => t.Seats)
                .NotEmpty().WithMessage("Please enter the number of seats.")
                .InclusiveBetween(1, 20).WithMessage("The number of seats must be between 1 and 20.");

            _unitOfWork = unitOfWork;
        }

        public bool NotAlreadyExistTable(TableModel table)
        {
            var tablesWithTheSameName = !_unitOfWork.Tables.Get().Any(t => t.Name == table.Name && t.RestaurantId == table.RestaurantId);
            return tablesWithTheSameName;
        }

    }
}
