using System.ComponentModel.DataAnnotations;

namespace Next_Store.Models
{
    public class UserEdit
    {
       
        [Required, EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password), MinLength(4, ErrorMessage = "Minimum Length is 4 Characters")]
        public string Password { get; set; }
        
        public UserEdit() { }
        
        public UserEdit(AppUser appUser)
        {
            Email = appUser.Email;
            Password = appUser.PasswordHash;
        }
    }
}
