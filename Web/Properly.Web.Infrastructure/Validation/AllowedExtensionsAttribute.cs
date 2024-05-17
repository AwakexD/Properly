namespace Properly.Web.Infrastructure.Validation
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;

    using Microsoft.AspNetCore.Http;

    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly HashSet<string> extensions;

        public AllowedExtensionsAttribute(string[] extensions)
        {
            this.extensions = new HashSet<string>(extensions.Select(e => e.ToLower()));
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IEnumerable<IFormFile> files)
            {
                foreach (var file in files)
                {
                    if (!IsValidFile(file))
                    {
                        return new ValidationResult(GetErrorMessage());
                    }
                }
            }
            else if (value is IFormFile file)
            {
                if (!IsValidFile(file))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }
            else
            {
                return new ValidationResult("Invalid file format.");
            }

            return ValidationResult.Success;
        }

        private bool IsValidFile(IFormFile file)
        {
            if (file == null)
            {
                return true;
            }

            var extension = Path.GetExtension(file.FileName).ToLower();
            return this.extensions.Contains(extension);
        }

        private string GetErrorMessage()
        {
            return "This photo extension is not allowed!";
        }
    }
}
