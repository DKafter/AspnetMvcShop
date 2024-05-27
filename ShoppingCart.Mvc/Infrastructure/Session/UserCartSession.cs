using Newtonsoft.Json;
using ShoppingCart.Data.Data;
using ShoppingCart.Data.Models.Cart;
using ShoppingCart.Data.Models.Product;
using ShoppingCart.Data.Services.Cart;
using ShoppingCart.Mvc.Infrastructure.Middleware;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ShoppingCart.Mvc.Infrastructure.Session;

public class UserCartSession : UserCartService
{
    private readonly ILogger _logger;
    private readonly IHttpContextAccessor _httpContext;
    private readonly string _userSessionKey;

    public UserCartSession(StoreDbContext storeDbContext, ILogger<IUserCartService> logger, IHttpContextAccessor httpContext) : base(storeDbContext, logger)
    {
        _logger = logger;
        _httpContext = httpContext;
        _userSessionKey = UserCartSessionMiddleware.UserSessionKey;
    }

    public override async Task AddUserCartAsync(Guid? userID, ProductModel? productModel)
    {
        var sessionID = _httpContext.HttpContext?.Session.GetString(_userSessionKey);
        userID ??= JsonConvert.DeserializeObject<Guid>(sessionID);
        
        await base.AddUserCartAsync(userID, productModel);
    }

    public override Task RemoveCartByIDAsync(Guid? userID, Guid? productID)
    {
        var sessionID = _httpContext.HttpContext?.Session.GetString(_userSessionKey);
        userID ??= JsonConvert.DeserializeObject<Guid>(sessionID);
        
        return base.RemoveCartByIDAsync(userID, productID);
    }

    public override Task ClearCartAsync()
    {
        return base.ClearCartAsync();
    }

    public override async Task<IEnumerable<CartModel>> GetAllCartAsync(Guid? userID)
    {
        var sessionID = _httpContext.HttpContext?.Session.GetString(_userSessionKey);
        userID ??= JsonConvert.DeserializeObject<Guid>(sessionID);
        
        return await base.GetAllCartAsync(userID);
    }

    public override Task UpdateQuantityByIDAsync(Guid? userID, Guid? productID, int quantity = 1)
    {
        var sessionID = _httpContext.HttpContext?.Session.GetString(_userSessionKey);
        userID ??= JsonConvert.DeserializeObject<Guid>(sessionID);
        
        return base.UpdateQuantityByIDAsync(userID, productID, quantity);
    }
}