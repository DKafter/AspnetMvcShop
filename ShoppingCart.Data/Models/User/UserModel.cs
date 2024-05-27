using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ShoppingCart.Data.Models.Cart;

namespace ShoppingCart.Data.Models.User;

public class UserModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Key]
    public Guid UserID { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }
    public string? Contact { get; set; }
    public string? Email { get; set; }

    public string? FullName => $"{FirstName} {MiddleName} {LastName}";
    public virtual ICollection<CartModel> CartModels { get; set; }
}