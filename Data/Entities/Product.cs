using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store_webAPI.Data.Entities
{
    [Table("Product")]
    public record Product
    {
        [Key]
        [Column("Id")]
        public Guid Id { get; init; }

        [Required]
        [Column("Name")]
        public string Name { get; init; }

        [Column("Description")]
        public string? Description { get; init; }

        [Required]
        [Column("Price")]
        public double Price { get; init; }

        [Column("UserCreatedId")]
        public virtual User UserCreated { get; init; }

        [Required]
        [Column("TimeCreated")]
        public DateTime TimeCreated { get; init; }
    }
}
