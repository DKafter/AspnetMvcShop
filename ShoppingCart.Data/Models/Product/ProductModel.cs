using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.Data.Models.Product;

public class ProductModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Key]
    public Guid ProductID { get; set; }

    public string? Name { get; set; } = "";
    public string? Description { get; set; } = "";
    public float? Price { get; set; } = 0f;
    public float? Weight { get; set; } = 0f;
}