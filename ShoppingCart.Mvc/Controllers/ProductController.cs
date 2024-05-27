using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Data.Models.Category;
using ShoppingCart.Data.Services.Category;

namespace ShoppingCart.Mvc.Controllers;

public class ProductController : Controller
{
    private readonly ICategoryService _categoryService;

    public ProductController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet("{controller}/{categoryName}")]
    [HttpGet("{controller}")]
    public async Task<IActionResult> Index([FromQuery(Name = "category")] string? categoryName)
    {
        if (string.IsNullOrEmpty(categoryName))
        {
            CategoryListModel? categoryList = await _categoryService.GetCategoryAsync();
            return View("Index", categoryList);
        }
        else
        {
            CategoryListModel? categoryList = await _categoryService.GetCategoryFilterByAsync(categoryName);
            return View("Index", categoryList);
        }
    }
}