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
            .Must((request, name) => NotAlreadyExistName(request.RestaurantId, name)).WithMessage("Product name already exists")
            .Length(3, 255).WithMessage("Length between 3 and 255 characters")
            .Must(ValidationFunctions.ContainsOnlyLetters).WithMessage("Please enter only letters");    

        RuleFor(r => r.Price)
            .NotEmpty().WithMessage("Please enter product price")
            .GreaterThan(0).WithMessage("Price must be greater than 0")
            .LessThan(1000000).WithMessage("It's too expensive");

        RuleFor(r => r.SubcategoryId)
            .GreaterThan(0).WithMessage("Please select an individual category")
            .LessThanOrEqualTo(14).WithMessage("Invalid Option");

        _unitOfWork = unitOfWork;
    }

    public bool NotAlreadyExistName(Guid restaurantId, string name)
    {
        var productsWithTheSameName = !_unitOfWork.Products.Get().Any(p => p.Name == name && p.RestaurantId == restaurantId);
        return productsWithTheSameName;
    }

    
}
