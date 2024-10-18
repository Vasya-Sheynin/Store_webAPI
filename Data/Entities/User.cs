using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store_webAPI.Data.Entities
{
    [Table("User")]
    public record User
    {
        public enum UserRole { BUYER = 1, SELLER };

        [Key]
        [Column("Id")]
        public Guid Id { get; init; }

        [Required]
        [Column("Name")]
        public string Name { get; init; }

        [Required]
        [Column("Email")]
        public string Email { get; init; }

        [Required]
        [Column("PasswordHash")]
        public string Password { get; init; }

        [Required]
        [Column("Role")]
        public UserRole Role { get; init; }

        public User(Guid Id, string Name, string Email, string Password, UserRole Role)
        {
            this.Id = Id;
            this.Name = Name;
            this.Email = Email;
            this.Password = Password;
            this.Role = Role;
        }
    }
}
