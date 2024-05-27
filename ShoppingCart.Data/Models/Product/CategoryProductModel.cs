using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.Data.Models.Product;

public class CategoryProductModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Key]
    public Guid CategoryID { get; set; }
    public string[]? CategoryNames { get; set; } = new string[] {};
    public virtual ICollection<ProductModel> ProductModels { get; set; } = new HashSet<ProductModel>();
}