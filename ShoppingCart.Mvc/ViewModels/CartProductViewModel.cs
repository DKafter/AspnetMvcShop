using ShoppingCart.Data.Models.Cart;

namespace ShoppingCart.Mvc.ViewModels;

public class CartProductViewModel
{
    public IEnumerable<CartModel>? CartModel;
    public decimal? TotalPrice;
}