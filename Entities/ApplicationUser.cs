using Microsoft.AspNetCore.Identity;

namespace DotnetJwtAuth.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
    }
}