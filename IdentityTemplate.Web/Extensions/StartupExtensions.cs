using IdentityTemplate.Web.CustomValidations;
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
           
        }).AddPasswordValidator<PasswordValidatior>()
            .AddEntityFrameworkStores<IdentityTemplateDbContext>();


    }
}