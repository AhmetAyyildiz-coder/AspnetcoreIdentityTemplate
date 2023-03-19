using IdentityTemplate.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityTemplate.Web.CustomValidations;

public class UserValidator :  IUserValidator<AppUser>
{
    public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
    {
        // user hakkındaki confirmasyonları buradan yapabiliriz. 
        var usernameIsNumeric = int.TryParse(user.UserName[0]!.ToString(),out _);
        var errors = new List<IdentityError>();
        if(usernameIsNumeric)
        {// eğer username numara ile başlıyorsa ; 

            errors.Add(new IdentityError(){ Code = "UserNameStartWithNumeric" , Description = "Username numeric ile başlamamalıdır."});
            
        }

        if (errors.Any())
        {
            return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
        }
        
        return Task.FromResult(IdentityResult.Success);
    }
}