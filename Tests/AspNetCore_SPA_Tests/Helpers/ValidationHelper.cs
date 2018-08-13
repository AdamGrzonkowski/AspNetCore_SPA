using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AspNetCore_SPA_Tests.Helpers
{
    public static class ValidationHelper
    {
        public static bool TryValidateProperty(object model, object propertyValue, string propertyName)
        {
            ValidationContext validationContext = new ValidationContext(model)
            {
                MemberName = propertyName
            };
            var results = new List<ValidationResult>();

            return Validator.TryValidateProperty(propertyValue, validationContext, results);
        }

        public static bool TryValidateObject(object model)
        {
            ValidationContext validationContext = new ValidationContext(model);
            var results = new List<ValidationResult>();

            return Validator.TryValidateObject(model, validationContext, results, true);
        }

        public static void CheckValidationRules(bool isValid, Func<bool> validationRule)
        {
            if (validationRule())
            {
                Assert.IsTrue(isValid);
            }
            else
            {
                Assert.IsFalse(isValid);
            }
        }
    }
}
