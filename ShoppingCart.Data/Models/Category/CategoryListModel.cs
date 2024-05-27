using ShoppingCart.Data.Models.Product;

namespace ShoppingCart.Data.Models.Category;

public class CategoryListModel
{
    public Task<IQueryable<ProductModel>>? ProductModels { get; set; }
    public string? CurrentCategory { get; set; } = string.Empty;
    public Task<IQueryable<CategoryProductModel>>? CategoryProductModels { get; set; }
 }