using Microsoft.AspNetCore.Identity;

namespace Quiz.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
