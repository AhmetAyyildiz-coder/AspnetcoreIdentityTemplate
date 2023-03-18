using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace IdentityTemplate.Web.Models;

// eğer migration eklerken hata alıyorsanız bu kodu aktifleştirebilirsiniz. 


//
// public class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityTemplateDbContext>
// {
//     public IdentityTemplateDbContext CreateDbContext(string[] args)
//     {
//         var optionsbuilder = new DbContextOptionsBuilder<IdentityTemplateDbContext>();
//         var cfmanager = new ConfigurationManager();
//
//         optionsbuilder.UseSqlServer(cfmanager.GetConnectionString("sqlcon"));
//
//         return new IdentityTemplateDbContext(optionsbuilder.Options);
//     }
// }