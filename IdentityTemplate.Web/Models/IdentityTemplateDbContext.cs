using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityTemplate.Web.Models;


public class IdentityTemplateDbContext : IdentityDbContext<AppUser , AppRole , string>
{
    // programcs içerisinde options geçiceğimizi belirttik. 
    public IdentityTemplateDbContext(DbContextOptions<IdentityTemplateDbContext> opt):base(opt)
    {
    }
    
    // tables
    
    
    
    
    // configurations 
}