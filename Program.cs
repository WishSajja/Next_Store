using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Next_Store.Clients;
using Next_Store.Infrastructure;
using Next_Store.Models;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMemoryCache();
builder.Services.AddSession();
// Add services to the container.
builder.Services.AddControllersWithViews();
// register the database service
builder.Services.AddDbContext<Next_StoreDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Next_StoreDbcontext")));
// register the identity functionalities
builder.Services.AddIdentity<AppUser, IdentityRole>(options => {
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
                .AddEntityFrameworkStores<Next_StoreDbContext>()
                .AddDefaultTokenProviders();

//registering seeddata
builder.Services.AddTransient<SeedData>();
builder.Services.AddSingleton(m =>
new PaypalClient(
    builder.Configuration["PayPal:ClientId"],
    builder.Configuration["PayPal:ClientSecret"],
    builder.Configuration["PayPal:Mode"]
    )
);
builder.Services.AddRouting(options => options.LowercaseUrls = true);
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<SeedData>();
    seeder.Initialize();

}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();


app.MapStaticAssets();

app.MapControllerRoute(
    name: "pages",
    pattern: "{controller=Pages}/{action=Page}/{slug}")
    .WithStaticAssets();

app.MapControllerRoute(
     "products",
     "products/{categoryslug}",
     defaults: new { controller = "Products", action = "ProductByCategory" }
     );



// Route configuration for Areas
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Products}/{action=Index}/{id?}")
    .WithStaticAssets();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=home}/{action=Index}/{id?}")
    .WithStaticAssets();





app.Run();
