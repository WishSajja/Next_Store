using System.ComponentModel.DataAnnotations;

namespace Next_Store.Models
{
    public class Login
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password), Required, MinLength(4, ErrorMessage = "Minimum Length is 4 Characters")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
