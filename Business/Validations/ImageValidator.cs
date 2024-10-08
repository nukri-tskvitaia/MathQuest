﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Business.Validations
{
    public sealed class ImageValidatorAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;
        public ImageValidatorAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        private string GetErrorMessage()
        {
            return $"This photo extension is not allowed!";
        }
    }
}
