using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonModules.Domain.Entities
{
    [Table("Product")]
    public class Product
    {
        [Key]
        [Column("Id")]
        public Guid Id { get; set; }

        [Required]
        [Column("Name")]
        public string Name { get; set; }

        [Column("Description")]
        public string? Description { get; set; }

        [Required]
        [Column("Price")]
        public double Price { get; set; }

        [Required]
        [Column("UserCreatedId")]
        public Guid UserCreatedId { get; set; }

        [Required]
        [Column("TimeCreated")]
        public DateTime TimeCreated { get; set; }

        public virtual User UserCreated { get; set; }


        public Product(Guid id, string name, string description, double price, Guid userCreatedId, DateTime timeCreated)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            UserCreatedId = userCreatedId;
            TimeCreated = timeCreated;
        }
    }
}
