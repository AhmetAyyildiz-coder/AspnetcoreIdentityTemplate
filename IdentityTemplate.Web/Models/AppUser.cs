using Microsoft.AspNetCore.Identity;

namespace IdentityTemplate.Web.Models;

public class AppUser : IdentityUser<string> // buraya string verme sebebimiz id değerinin türünü belirlemektir.
                                            // guid yapıcağımızdan dolayı id değerini string olarak belirledik. 
{
    
}