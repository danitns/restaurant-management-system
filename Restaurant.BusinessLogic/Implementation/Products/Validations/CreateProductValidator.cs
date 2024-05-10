using FluentValidation;
using Restaurant.BusinessLogic.CommonFunc;
using Restaurant.BusinessLogic.Implementation.Products.Models;
using Restaurant.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BusinessLogic.Implementation.Products.Validations;

public class CreateProductValidator : AbstractValidator<CreateProductModel>
{
    private readonly UnitOfWork _unitOfWork;
    public CreateProductValidator(UnitOfWork unitOfWork)
    {
        RuleFor(r => r.Name)
            .NotEmpty().WithMessage("Please enter product name")
            .Must(NotAlreadyExistName).WithMessage("Product name already exists")
            .Length(3, 40).WithMessage("Length between 3 and 40 characters")
            .Must(ValidationFunctions.ContainsOnlyLetters).WithMessage("Please enter only letters");    

        RuleFor(r => r.Price)
            .NotEmpty().WithMessage("Please enter product price")
            .GreaterThan(0).WithMessage("Price must be greater than 0");

        _unitOfWork = unitOfWork;
    }

    public bool NotAlreadyExistName(string name)
    {
        var productsWithTheSameName = !_unitOfWork.Products.Get().Any(p => p.Name == name);
        return productsWithTheSameName;
    }

    
}
