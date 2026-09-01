using Microsoft.AspNetCore.Identity;

namespace Next_Store.Models
{
    public class AppUser : IdentityUser
    {
        public string Occupation { get; set; }
    }
}
