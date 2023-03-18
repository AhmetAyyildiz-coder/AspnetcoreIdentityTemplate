using IdentityTemplate.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityTemplate.Web.CustomValidations;

public class PasswordValidatior : IPasswordValidator<AppUser>
{
    public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string password)
    {
        // bu sınıf ile kendi şifre sağlayıcımızı yazabiliriz.
        
        // örnek senaryo 1 : Password içeirsinde username içermesin diye kural koyabiliriz. ek bir confirmasyon olarak. 

        var errors = new List<IdentityError>();
        if (password.ToLower().Contains(user.UserName.ToLower()))
        {// eğer şifre kullanıcı adı içeriyor ise bu kod bloğuna düşecektir. 
            errors.Add(new IdentityError()
            {
                Code = "PasswordContainUserName", Description = "Şifre Alanı UserName içermemelidir."
            });
            
        }

        if (password.ToLower().StartsWith("1234"))
        {// şifre 1234 ile başlamasın diye bir kural koyabiliriz.
            errors.Add(new IdentityError(){Code = "PasswordNotStartWith1234" , Description = "Şifre ardışık sayı içermemelidir."});
            
        }

        if (errors.Any())
        {// eğer hata yok ise success döneriz.
            return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
        }

        return Task.FromResult(IdentityResult.Success);
    }
}