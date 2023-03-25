using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityTemplate.Web.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}


// Concurrency stamp nedir ? 
/*
 * Eş zamanlılık sorununu çözer. 1 id'li user'ın eğer bilgileri 12.03 saatinde değiştiriliyor ise
 * diğer bir login olunan yerden aynı 1 id'li user'ın değerini 12.04 de güncellemek isterse kullanıcı
 * ilk güncelleyen o an user'ı kitlemiştir o işini bitirmeden güncelleme yapamayız.
 * Ve ilk güncelleyen kişi concurrency stamp değerini de güncellediği için 2. giren kişi herhangi bir update yapamaz.
 * sayfayı yenilemesi yeni dataları ekrana getirmesi gerekmektedir.
 * Concurrency stamp ef core tarafında otomatik olarak güncellenir ve kontrol eder. İdentity bunu da sağlar bize.
 * Identity her güncelleme yaptığımızda bunu güncelliyor.
 */