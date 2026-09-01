using System.ComponentModel.DataAnnotations;

namespace Next_Store.Infrastructure
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // var context = validationContext.GetService<Next_StoreDbContext>();

           // casting and null check(pattern matching)
            if (value is IFormFile file)
            {
                // here we get the extension
                var extension = Path.GetExtension(file.FileName).ToLower();

                // creating an array of allowed extensions
                string[] AllowedExtensions = { "jpg", "png", "jpeg", "gif" };
                // checking if the extension is valid
                bool AllowedFileType = AllowedExtensions.Any(m => extension.EndsWith(m));
                // if false return a custon error message
                if (!AllowedFileType)
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }
         
            return ValidationResult.Success;


        }

        private string GetErrorMessage()
        {
            return "Allowed image types are jpg, png, jpeg gif ";
        }
    }
}
