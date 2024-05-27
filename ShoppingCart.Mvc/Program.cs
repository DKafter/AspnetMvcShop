using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data.Data;
using ShoppingCart.Data.Data.Product;
using ShoppingCart.Data.Models.Category;
using ShoppingCart.Data.Services.Cart;
using ShoppingCart.Data.Services.Category;
using ShoppingCart.Mvc.Infrastructure.Middleware;
using ShoppingCart.Mvc.Infrastructure.Session;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

// Add services to the container.
builder.Services.AddDbContext<StoreDbContext>(
    opt => { opt.UseSqlite(builder.Configuration["ConnectionStrings:DefaultConnection"]); });
builder.Services.AddEntityFrameworkSqlite();
builder.Services.AddScoped<StoreDbContext>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserCartService, UserCartService>();
builder.Services.AddScoped<UserCartSession>();
builder.Services.AddSingleton<CategoryListModel>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(opt =>
{
    opt.IdleTimeout = TimeSpan.FromMinutes(30);
    opt.IOTimeout = TimeSpan.FromMinutes(30);
    opt.Cookie.HttpOnly = true;
    opt.Cookie.IsEssential = true;
    opt.Cookie.MaxAge = TimeSpan.FromDays(1);
});

builder.Services.AddControllersWithViews();

builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseMiddleware<UserCartSessionMiddleware>();

app.UseCors(options => options.AllowAnyOrigin());

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "Home",
    pattern: "{controller}/{action}",
    defaults: new { Controller = "Home", action = "Index" }
);

app.MapControllerRoute(
    name: "productIndex",
    pattern: "{controller}",
    defaults: new { Controller = "Product", action = "Index" }
);

app.MapControllerRoute(
    name: "productIndex",
    pattern: "{controller}/{category}",
    defaults: new { Controller = "Product", action = "Index" }
);

app.MapControllerRoute(
    name: "cartUpdate",
    pattern: "{controller}/{action}/{ProductID}",
    defaults: new { controller = "Cart", action = "Update", HttpMethod.Put }
);

app.MapControllerRoute(
    name: "cartRemove",
    pattern: "{controller}/{action}/{ProductID}",
    defaults: new { controller = "Cart", action = "Remove", HttpMethod.Delete }
);

app.MapControllerRoute(
    name: "cartIndex",
    pattern: "Cart/Index",
    defaults: new { controller = "Cart", action = "Index", HttpMethod.Get }
);

app.MapControllerRoute(
    name: "cartIndex",
    pattern: "Cart",
    defaults: new { controller = "Cart", action = "Index", HttpMethod.Get }
);

app.MapControllerRoute(
    name: "cartAdd",
    pattern: "{controller}/{action}",
    defaults: new { controller = "Cart", action = "Add", HttpMethod.Post }
);

var productContext = app.Services.CreateAsyncScope().ServiceProvider.GetRequiredService<StoreDbContext>();
ProductInitialize.Initialize(productContext);

app.Run();