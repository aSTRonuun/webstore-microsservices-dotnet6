using Microsoft.AspNetCore.Identity;

namespace GeekShopping.IdentityServer.Models
{
    public class AplicationUser : IdentityUser
    {
        private string FirstName { get; set; }
        private string LastName { get; set; }
    }
}
