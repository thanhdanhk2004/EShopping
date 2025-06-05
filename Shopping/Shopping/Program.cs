using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shopping.Areas.Admin.Reponsitory;
using Shopping.Models;
using Shopping.Reponitory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


//Ket noi den database
builder.Services.AddDbContext<Context>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectedDb"));
});

// Dang ky gui mail
builder.Services.AddTransient<IEmailSender, EmailSender>();

//Cau hinh Identity
builder.Services.AddIdentity<AppUserModel, IdentityRole>()
    .AddEntityFrameworkStores<Context>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
    options.User.RequireUniqueEmail = true;
});


//Dang ky session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IOTimeout = TimeSpan.FromMinutes(15);
    options.Cookie.IsEssential = true;
});



var app = builder.Build();

//Dang ky trang 404
app.UseStatusCodePagesWithRedirects("/Home/Error?statuscode={0}");


//su dung Session
app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();//Xác thực
app.UseAuthorization();//Phân quyền


app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=Product}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "category",
    pattern: "/category/{Slug?}",
    defaults: new {controller = "Category", action = "index" }
);

app.MapControllerRoute(
    name: "brand",
    pattern: "/brand/{Slug?}",
    defaults: new { controller = "Brand", action = "index" }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



//Seeding data
var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<Context>();
SeedData.SeedingData(context);

app.Run();
