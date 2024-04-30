using FluentValidation;
using Restaurant.BusinessLogic.CommonFunc;
using Restaurant.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BusinessLogic.Implementation.Users
{
    public class RegisterValidator : AbstractValidator<RegisterModel>
    {
        private readonly UnitOfWork _unitOfWork;
        public RegisterValidator(UnitOfWork unitOfWork)
        {
            RuleFor(r => r.Email)
                .NotEmpty().WithMessage("Please enter your email address")
                .Must(NotAlreadyExistEmail).WithMessage("Email already exists")
                .Length(5, 40).WithMessage("Length between 5 and 40 characters");

            RuleFor(r => r.FirstName)
                .NotEmpty().WithMessage("Please enter yout first name.")
                .MinimumLength(3).WithMessage("Your first name should be at least 3 characters long.")
                .MaximumLength(40).WithMessage("Your first name cannot be longer than 40 characters.")
                .Must(ValidationFunctions.ContainsOnlyLetters).WithMessage("Please enter only letters.");

            RuleFor(r => r.LastName)
                .NotEmpty().WithMessage("Please enter your last name.")
                .MinimumLength(3).WithMessage("Your last name should be at least 3 characters long.")
                .MaximumLength(40).WithMessage("Your last name cannot be longer than 40 characters.")
                .Must(ValidationFunctions.ContainsOnlyLetters).WithMessage("Please enter only letters.");

            RuleFor(r => r.Phone)
                .Length(10, 12).WithMessage("Length between 10 and 12 characters")
                .NotEmpty().WithMessage("Please enter yout phone number")
                .Must(NotAlreadyExistPhone).WithMessage("Phone number already exists")
                .Must(ValidationFunctions.ContainsOnlyNumbersWithOptionalPlus).WithMessage("Enter valid a format");

            RuleFor(r => r.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Must(ValidationFunctions.ContainSpecialCharacters).WithMessage("Password must contain at least one special character.")
                .Must(ValidationFunctions.ContainUppercaseLetter).WithMessage("Password must contain at least one uppercase letter.")
                .Must(ValidationFunctions.ContainNumber).WithMessage("Password must contain at least one number.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm your password.")
                .Equal(x => x.Password).WithMessage("Passwords do not match");

            RuleFor(r => r.Birthdate)
                .NotEmpty().WithMessage("Please enter your birth date");

            _unitOfWork = unitOfWork;
        }

        public bool NotAlreadyExistEmail(string email)
        {
            var usersWithTheSameMail = !_unitOfWork.Users.Get().Any(u => u.Email == email);
            return usersWithTheSameMail;
        }

        public bool NotAlreadyExistPhone(string phone)
        {
            var usersWithTheSamePhone = !_unitOfWork.Users.Get().Any(u => u.Phone == phone);
            return usersWithTheSamePhone;
        }
    }
}
