using System.ComponentModel.DataAnnotations;

namespace TrainingFPTCo.Validation
{
    public class AllowExtensionFile : ValidationAttribute
    {
        private readonly string[] _extension;
        public AllowExtensionFile(string[] extension)
        {
            _extension = extension;
        }

        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_extension.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }
            return ValidationResult.Success;
        }

        private string GetErrorMessage()
        {
            return "This photo extension is not allowed";
        }
    }
}
