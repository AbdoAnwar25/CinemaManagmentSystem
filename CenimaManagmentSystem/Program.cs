using CenimaManagmentSystem.Data;
using CenimaManagmentSystem.Models;
using CenimaManagmentSystem.Repository;
using CenimaManagmentSystem.Utilities;
using Ecommerce533.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
///

builder.Services.AddControllersWithViews();

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string"
        + "'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)
    );


// register
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;
    //options.Lockout.MaxFailedAccessAttempts =10;
    //options.Password 
    //options.SignIn
}).AddEntityFrameworkStores<ApplicationDbContext>()
   .AddDefaultTokenProviders();


///        

builder.Services.AddScoped<IRepository<Category> , Repository<Category>>();
builder.Services.AddScoped<IRepository<Cinema> , Repository<Cinema>>();
builder.Services.AddScoped<IRepository<Movie> , Repository<Movie>>();
builder.Services.AddScoped<IRepository<Actor> , Repository<Actor>>();
builder.Services.AddScoped<IRepository<ApplicationUserOtp> , Repository<ApplicationUserOtp>>();
builder.Services.AddScoped<IMovieSubImageRepository , MovieSubImageRepository>();
builder.Services.AddTransient<IEmailSender, EmailSender>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this value.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

//// مسار الـ Areas (مثل صفحة الـ Admin والـ Categories)
//app.MapControllerRoute(
//    name: "areas",
//    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

//// المسار الافتراضي لباقي الموقع
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}")
//    .WithStaticAssets();


app.MapControllerRoute(
    name: "default",
    pattern: "{area=Identity}/{controller=Account}/{action=Register}/{id?}")
    .WithStaticAssets();



app.Run();