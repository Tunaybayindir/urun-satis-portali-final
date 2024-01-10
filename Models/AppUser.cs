using Microsoft.AspNetCore.Identity;

namespace UrunSatisPortali.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public string City { get; set; }
    }
}
