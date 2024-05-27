using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingCart.Data.Data;
using ShoppingCart.Data.Models.Cart;
using ShoppingCart.Data.Models.Product;
using ShoppingCart.Data.Models.User;

namespace ShoppingCart.Data.Services.Cart;

public class UserCartService : IUserCartService
{
    private readonly ILogger _logger;
    private StoreDbContext StoreDbContext { get; set; }

    public UserCartService(StoreDbContext storeDbContext, ILogger<IUserCartService> logger)
    {
        _logger = logger;
        StoreDbContext = storeDbContext;
        LoadContextAsync(storeDbContext);
    }

    private async void LoadContextAsync(StoreDbContext storeDbContext)
    {
        if (await storeDbContext.Roles.AnyAsync())
        {
            await storeDbContext.Roles.LoadAsync();
        }

        if (await storeDbContext.Users.AnyAsync())
        {
            await storeDbContext.Users.LoadAsync();
        }

        if (await storeDbContext.Carts.AnyAsync())
        {
            await storeDbContext.Carts.LoadAsync();
        }
    }

    public virtual async Task AddUserCartAsync(Guid? userID, ProductModel? product)
    {
        try
        {
            var productModel = await StoreDbContext.Products
                .FindAsync(product.ProductID);

            if (productModel == null)
            {
                _logger.LogInformation($"Product with ID {product.ProductID} not found.");
                return;
            }

            var userModel = await StoreDbContext.Users
                .Include(u => u.CartModels)
                .FirstOrDefaultAsync(u => u.UserID == userID);

            if (userModel == null)
            {
                userModel = new UserModel
                {
                    UserID = userID ?? Guid.NewGuid(),
                    CartModels = new HashSet<CartModel>()
                };
                StoreDbContext.Users.Add(userModel);
            }

            var cartModel = userModel.CartModels
                .FirstOrDefault(c => c.ProductID == productModel.ProductID);

            if (cartModel == null)
            {
                cartModel = new CartModel
                {
                    CartID = Guid.NewGuid(),
                    UserID = userModel.UserID,
                    ProductModel = productModel,
                    ProductID = productModel.ProductID,
                    Quantity = 1
                };
                userModel.CartModels.Add(cartModel);
            }
            else
            {
                cartModel.Quantity += 1;
            }

            var roleModel = await StoreDbContext.Roles
                .FirstOrDefaultAsync(r => r.UserID == userModel.UserID);

            if (roleModel == null)
            {
                roleModel = new RoleModel
                {
                    RoleID = Guid.NewGuid(),
                    RoleType = RoleEnum.User,
                    UserID = userModel.UserID,
                    Created = DateTime.Now,
                    Updated = DateTime.Now
                };
                StoreDbContext.Roles.Add(roleModel);
            }
            else
            {
                roleModel.Updated = DateTime.Now;
                StoreDbContext.Roles.Update(roleModel);
            }

            await StoreDbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogInformation($"An error occurred: {e.Message}");
        }
    }

    public virtual async Task RemoveCartByIDAsync(Guid? userID, Guid? productID)
    {
        try
        {
            if (userID == null || productID == null)
            {
                return;
            }

            var userWithCarts = await StoreDbContext.Users
                .Include(u => u.CartModels)
                .FirstOrDefaultAsync(u => u.UserID == userID);

            if (userWithCarts == null)
            {
                _logger.LogInformation($"User with ID {userID} not found.");
                return;
            }

            var cartToRemove = userWithCarts.CartModels
                .FirstOrDefault(c => c.ProductID == productID);

            if (cartToRemove != null)
            {
                StoreDbContext.Carts.Remove(cartToRemove);
                await StoreDbContext.SaveChangesAsync();
            }
            else
            {
                _logger.LogInformation($"Cart with ProductID {productID} not found for User {userID}.");
            }
        }
        catch (Exception e)
        {
            _logger.LogInformation($"An error occurred while removing the cart: {e.Message}");
        }
    }

    public virtual async Task ClearCartAsync()
    {
    }

    public virtual async Task<IEnumerable<CartModel>> GetAllCartAsync(Guid? userID)
    {
        if (userID == null)
        {
            return Enumerable.Empty<CartModel>();
        }

        var userWithCarts = await StoreDbContext.Users
            .Include(u => u.CartModels)
            .ThenInclude(p => p.ProductModel)
            .FirstOrDefaultAsync(u => u.UserID == userID);

        TotalPrice =
            Task.FromResult(Convert.ToDecimal(userWithCarts?.CartModels.Sum(p => p.Quantity * p.ProductModel.Price)));

        return userWithCarts?.CartModels ?? Enumerable.Empty<CartModel>();
    }

    public virtual async Task UpdateQuantityByIDAsync(Guid? userID, Guid? productID, int quantity = 1)
    {
        try
        {
            if (userID == null || productID == null)
            {
                return;
            }

            var userWithCarts = await StoreDbContext.Users
                .Include(u => u.CartModels)
                .ThenInclude(p => p.ProductModel)
                .FirstOrDefaultAsync(u => u.UserID == userID);

            var cart = userWithCarts?.CartModels.FirstOrDefault(c => c.ProductID == productID && c.UserID == userID);
            if (cart != null)
            {
                cart.Quantity = quantity;
                StoreDbContext?.Carts.Update(cart);
            }

            await StoreDbContext?.SaveChangesAsync()!;
        }
        catch (Exception e)
        {
            _logger.LogInformation($"An error occurred: {e.Message}");
        }
    }

    public Task<decimal> TotalPrice { get; private set; }
}