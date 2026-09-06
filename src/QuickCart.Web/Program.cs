using QuickCart.Catalog.Infrastructure;
using QuickCart.Ordering.Application;
using QuickCart.Ordering.Application.Abstractions.Catalog;
using QuickCart.Ordering.Infrastructure;
using QuickCart.Web.Adapters.Ordering;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddOrderingApplication();
builder.Services.AddOrderingInfrastructure(builder.Configuration);
builder.Services.AddCatalogInfrastructure(builder.Configuration);

builder.Services.AddScoped<IProductCatalog, CatalogProductAdapter>();
builder.Services.AddSingleton(TimeProvider.System);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
