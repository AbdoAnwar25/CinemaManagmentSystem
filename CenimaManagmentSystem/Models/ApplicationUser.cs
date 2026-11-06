using Microsoft.AspNetCore.Identity;

namespace Ecommerce533.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string? Address { get; set; }
    }
}