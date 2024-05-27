using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Data.Models.Product;
using ShoppingCart.Data.Services.Cart;
using ShoppingCart.Mvc.Infrastructure.Session;
using ShoppingCart.Mvc.ViewModels;

namespace ShoppingCart.Mvc.Controllers;
public class CartController : Controller
{
    private readonly IUserCartService _userCartSession;

    public CartController(UserCartSession userCartSession)
    {
        _userCartSession = userCartSession;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var cartModels = await _userCartSession.GetAllCartAsync(null);
        var totalPrice = await _userCartSession.TotalPrice;
        var viewModel = new CartProductViewModel()
        {
            CartModel = cartModels,
            TotalPrice = totalPrice
        };
        return View(viewModel);
    }

    [HttpPost("[controller]/[action]")]
    public async Task<IActionResult> Add([FromForm] ProductModel? productModel)
    {
        await _userCartSession.AddUserCartAsync(null, productModel);

        return RedirectToAction("Index", "Cart");
    }

    [HttpPut("[controller]/[action]/{ProductID}")]
    public async Task<IActionResult> Update([FromRoute] string ProductID, [FromBody] Dictionary<string, int> model)
    {
        int quantity = model["Quantity"];
        
        Guid.TryParse(ProductID, out Guid productGuidID);
        if (productGuidID != Guid.Empty)
        {
            await _userCartSession.UpdateQuantityByIDAsync(null, productGuidID, quantity);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpDelete("[controller]/[action]/{ProductID}")]
    public async Task<IActionResult> Remove([FromRoute] string ProductID)
    {
        Guid.TryParse(ProductID, out Guid productGuidID);
        if (productGuidID != Guid.Empty)
        {
            await _userCartSession.RemoveCartByIDAsync(null, productGuidID);
        }
        
        return RedirectToAction("Index", "Home");
    }
}