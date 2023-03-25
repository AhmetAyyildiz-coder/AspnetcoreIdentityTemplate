using System.Reflection;
using IdentityTemplate.Web.Extensions;
using IdentityTemplate.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// şifre sıfırlama için gerekli token provider options 
builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
{
    // oluşturulan şifre  değiştirme tokneninin ömrünü 2 saatlik olarak ayarladık.
    opt.TokenLifespan = TimeSpan.FromHours(1);
});

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
// ayrıca identity ile ilgili configurations'ları ek bir dosyada yaptık.
builder.Services.AddCustomIdentityDependencies();

builder.Services.ConfigureApplicationCookie(opt =>
{
    var cookiebuilder = new CookieBuilder();

    cookiebuilder.Name = "IdentityUser";
    opt.LoginPath = "/Auth/Login";
    opt.LogoutPath = new PathString("/Home/Index");
   opt.ExpireTimeSpan=TimeSpan.FromDays(30); // cookie süresini 30 gün ayarladık.
   opt.SlidingExpiration = true; // bunu true set etmez isek cookie cookie tekrar yenilenmez.
   // ama bunu true olarak set edersek cookie her seferinde tekrar güncellenir 30 gün boyunca her girişte +30 olarak tekrar 
   // güncellenir cookie süresi.
   
   // oluşturduğumuz cookie'yi customize edip varsayılan identity cookie si yerine güncelleyebiliriz.
   opt.Cookie = cookiebuilder;
});


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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.Run();