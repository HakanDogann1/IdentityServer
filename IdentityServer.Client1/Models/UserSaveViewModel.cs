using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Client1.Models
{
    public class UserSaveViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public int City { get; set; }
    }
}
