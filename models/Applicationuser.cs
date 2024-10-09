using Microsoft.AspNetCore.Identity;

namespace department.models
{
    public class Applicationuser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
