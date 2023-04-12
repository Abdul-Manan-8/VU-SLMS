using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VU_SLMS.Data;
using VU_SLMS.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var MyDBCS = builder.Configuration.GetConnectionString("CS");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<vu_slms_dbContext>(options =>
    options.UseSqlServer(MyDBCS));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(o => {

    o.IdleTimeout = TimeSpan.FromMinutes(300);
});

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}");
app.MapRazorPages();

app.Run();
