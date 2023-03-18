using System.Reflection;
using IdentityTemplate.Web.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// db context implement
builder.Services.AddDbContext<IdentityTemplateDbContext>(opt =>
{
    // connection string opt
    opt.UseSqlServer(builder.Configuration.GetConnectionString("sqlcon") 
        , opts =>
        {// burada da migration ayarı yapmalıyız.
            opts.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
        });
    
});

// ıdentity kütüphanesini servis olarak eklemek gerekiyor.
builder.Services.AddIdentity<AppUser, AppRole>(opt =>
{
    opt.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<IdentityTemplateDbContext>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();