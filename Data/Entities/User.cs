using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store_webAPI.Data.Entities
{
    [Table("User")]
    public class User
    {
        public enum UserRole { BUYER = 1, SELLER };

        [Key]
        [Column("Id")]
        public Guid Id { get; set; }

        [Required]
        [Column("Name")]
        public string Name { get; set; }

        [Required]
        [Column("Email")]
        public string Email { get; set; }

        [Required]
        [Column("PasswordHash")]
        public string PasswordHash { get; set; }

        [Required]
        [Column("Role")]
        public UserRole Role { get; set; }

        public virtual IEnumerable<Product> Products { get; set; }

        public User(Guid Id, string Name, string Email, string PasswordHash, UserRole Role)
        {
            this.Id = Id;
            this.Name = Name;
            this.Email = Email;
            this.PasswordHash = PasswordHash;
            this.Role = Role;
        }
    }
}
