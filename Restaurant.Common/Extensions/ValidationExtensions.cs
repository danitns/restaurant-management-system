using Restaurant.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace Restaurant.Common.Extensions
{
    public static class ValidationExtensions
    {
        public static void ThenThrow(this ValidationResult result, object model)
        {
            if (!result.IsValid)
            {
                throw new ValidationErrorException(result, model);
            }
        }
    }
}
