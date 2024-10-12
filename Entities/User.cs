using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store_webAPI.Entities
{
    [Table("User")]
    public record User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; init; }

        [Required]
        [Column("Name")]
        public string Name { get; init; }

        [Required]
        [Column("Email")]
        public string Email { get; init; }

        [Required]
        [Column("PasswordHash")]
        public string PasswordHash { get; init; }

        [Required]
        [Column("CreatedDate", TypeName = "Date")]
        public DateTime CreatedDate { get; init; }
    }
}
