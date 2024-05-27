using ShoppingCart.Data.Models.Cart;
using ShoppingCart.Data.Models.Product;
using ShoppingCart.Data.Models.User;

namespace ShoppingCart.Data.Services.Cart;

public interface IUserCartService
{
    public Task AddUserCartAsync(Guid? userID, ProductModel? cartModel);
    public Task RemoveCartByIDAsync(Guid? userID, Guid? productID);
    public Task ClearCartAsync();
    public Task<IEnumerable<CartModel>> GetAllCartAsync(Guid? userID);
    public Task UpdateQuantityByIDAsync(Guid? userID, Guid? productID, int quantity = 1);
    public Task<decimal> TotalPrice { get; }
}