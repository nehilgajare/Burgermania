using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Burgermania.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [MaxLength(10)]
        public string? MobileNumber { get; set; }

        [Required]
        [MaxLength(8)]
        public string? Password { get; set; }

        [Required]
        public string Role { get; set; } = "User";
    }
}
