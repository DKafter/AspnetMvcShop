using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShoppingCart.Data.Models.Product;
using ShoppingCart.Data.Models.User;

namespace ShoppingCart.Data.Models.Cart;

public class CartModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Key]
    public Guid CartID { get; set; }
    [ForeignKey("UserID")]
    public Guid UserID { get; set; }
    public virtual UserModel User { get; set; }
    public int Quantity { get; set; } = 1;
    [ForeignKey("ProductID")]
    public Guid ProductID { get; set; }
    public virtual ProductModel ProductModel { get; set; }
}