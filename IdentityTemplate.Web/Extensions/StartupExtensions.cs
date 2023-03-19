using IdentityTemplate.Web.CustomValidations;
using IdentityTemplate.Web.Localizations;
using IdentityTemplate.Web.Models;

namespace IdentityTemplate.Web.Extensions;

public static class StartupExtensions
{
    public static void AddCustomIdentityDependencies(this IServiceCollection services)
    {// identity için program.cs içerisidne yapılan işlemleri burada tanımlıyoruz.
        services.AddIdentity<AppUser, AppRole>(opt =>
        {// identity ile ilgili configurations'ları buradan yapabilirsiniz
    
            // user ile ilgili configurationlar
            opt.User.RequireUniqueEmail = true;
            opt.User.AllowedUserNameCharacters = "abcdefghijklmnoprstuvyzxq_"; // türkçe karakterleri aradan çıkarttık.
            
            // password ile ilgili configurations'lar
            opt.Password.RequiredLength = 7;
            opt.Password.RequireDigit = false;
            opt.Password.RequireLowercase = false;
            opt.Password.RequireUppercase = false;
            opt.Password.RequireNonAlphanumeric = false;

           
            opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10); // 3 başarısız girişten sonra 10 dakika kitlenilsin.
            opt.Lockout.MaxFailedAccessAttempts = 3; // 3 girişten sonra kitleyebiliriz bu şekilde.

        }).AddPasswordValidator<PasswordValidatior>() // custom password validator
            .AddUserValidator<UserValidator>()
            .AddErrorDescriber<LocalizationIdentityErrorDescription>() // hataları türkçeleştirme yapabiliriz bu şekilde. 
            .AddEntityFrameworkStores<IdentityTemplateDbContext>();


    }
}