using Microsoft.AspNetCore.Identity;

namespace IdentityTemplate.Web.Localizations;

public class LocalizationIdentityErrorDescription : IdentityErrorDescriber
{
    
    // var olan ingilizce hataları olur da türkçeleştirmek istersek bu şekilde 
    //  IdentityErrorDescriber sınıfından kalıtım alıp override ederiz. 
    
    public override IdentityError DuplicateUserName(string userName)
    {
        return new IdentityError()
            { Code = "DuplicateUserName", Description = "Bu kullanıcı adı zaten sisteme kayıtlıdır. !" };
        //return base.DuplicateUserName(userName);
    }

    public override IdentityError DuplicateEmail(string email)
    {
        return new IdentityError()
        {
            Code = "DuplicateEmail",
            Description = "Bu email zaten sistemde kayıtlıdır.";
        };
        //return base.DuplicateEmail(email);
    }
}