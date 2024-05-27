using ShoppingCart.Data.Models.Category;

namespace ShoppingCart.Data.Services.Category;

public interface ICategoryService
{
    public Task<CategoryListModel> GetCategoryAsync();
    public Task<CategoryListModel> GetCategoryFilterByAsync(string? categoryName);
    public Task<IEnumerable<string>>GetAllCategoryNamesAsync();
}