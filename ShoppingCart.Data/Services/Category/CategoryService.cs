using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data.Data;
using ShoppingCart.Data.Domains.Helpers;
using ShoppingCart.Data.Models.Category;
using ShoppingCart.Data.Models.Product;

namespace ShoppingCart.Data.Services.Category;

public class CategoryService : ICategoryService
{
    private StoreDbContext StoreDbContext { get; set; }

    private IQueryable<CategoryProductModel> CategorieProducts => StoreDbContext?.CategoryProducts ??
                                                                  Enumerable.Empty<CategoryProductModel>()
                                                                      .AsQueryable();
    
    private IQueryable<ProductModel> ProductModels =>
        CategorieProducts?.SelectMany(t => t.ProductModels).OfType<ProductModel>().AsQueryable() ??
        Enumerable
            .Empty<ProductModel>()
            .AsQueryable();

    public CategoryService(StoreDbContext storeDbContext)
    {
        StoreDbContext = storeDbContext;
        
        LoadContextAsync(storeDbContext);
    }
    
    private async void LoadContextAsync(StoreDbContext storeDbContext)
    {
        if (await storeDbContext.CategoryProducts.AnyAsync())
        {
            await StoreDbContext.CategoryProducts.LoadAsync();
        }

        if (await storeDbContext.Products.AnyAsync())
        {
            await StoreDbContext.Products.LoadAsync();
        }
    }

    public Task<CategoryListModel> GetCategoryAsync()
    {
        CategoryListModel categoryListModel = new CategoryListModel()
        {
            ProductModels = Task.FromResult<IQueryable<ProductModel>>(ProductModels),
            CategoryProductModels = Task.FromResult<IQueryable<CategoryProductModel>>(CategorieProducts)
        };
        return Task.FromResult(categoryListModel);
    }

    public async Task<IEnumerable<string>> GetAllCategoryNamesAsync()
    {
        CategoryListModel categoryListModel = await GetCategoryAsync();
        List<string> stringCategories = new List<string>();

        if (categoryListModel.CategoryProductModels != null)
        {
            await (await categoryListModel.CategoryProductModels).ForEachAsync(t =>
            {
                if (t.CategoryNames != null)
                {
                    foreach (var s in t.CategoryNames)
                    {
                        stringCategories.Add(s);
                    }
                }
            });
        }

        return stringCategories;
    }

    public async Task<CategoryListModel> GetCategoryFilterByAsync(string? categoryName)
    {
        CategoryListModel categoryListModel = await GetCategoryAsync();

        if (!string.IsNullOrEmpty(categoryName))
        {
            if (categoryListModel.CategoryProductModels != null)
            {
                IQueryable<CategoryProductModel> categories = await categoryListModel.CategoryProductModels;
                IQueryable<CategoryProductModel>? filteredCategories = categories.Where(c => c.CategoryNames.Contains(categoryName));
                if (filteredCategories != null)
                {
                    categoryListModel.CurrentCategory = categoryName;

                    categoryListModel.CategoryProductModels = Task.FromResult(filteredCategories);
                }
            }
        }

        return categoryListModel;
    }
}