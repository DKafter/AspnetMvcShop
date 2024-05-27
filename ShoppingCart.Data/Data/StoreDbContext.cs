using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingCart.Data.Models.Cart;
using ShoppingCart.Data.Models.Category;
using ShoppingCart.Data.Models.Product;
using ShoppingCart.Data.Models.User;

namespace ShoppingCart.Data.Data;

public sealed class StoreDbContext : DbContext
{
    public DbSet<CategoryProductModel> CategoryProducts { get; set; }
    public DbSet<ProductModel> Products { get; set; }
    public DbSet<UserModel> Users { get; set; }

    public DbSet<RoleModel> Roles { get; set; }
    public DbSet<CartModel> Carts { get; set; }

    public StoreDbContext() : base()
    {
    }

    public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
    {
        CategoryProducts = Set<CategoryProductModel>();
        Products = Set<ProductModel>();
        Users = Set<UserModel>();
        Roles = Set<RoleModel>();
        Carts = Set<CartModel>();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        if (!builder.IsConfigured)
        {
            builder
                .UseLazyLoadingProxies()
                .UseSqlite("Data Source=shopping.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserModel>()
            .HasMany(u => u.CartModels)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserID);
    }
}