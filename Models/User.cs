using System.ComponentModel.DataAnnotations;

namespace Next_Store.Models
{
    public class User
    {
        [Required, MinLength(2, ErrorMessage = "Minimum Length is 2 letters")]
        [Display(Name ="Username")]
        public string UserName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password), Required, MinLength(4, ErrorMessage = "Minimum Length is 4 Characters")]
        public string Password { get; set; }
        
       
    }
}
