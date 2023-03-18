using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityTemplate.Web.Models;
using IdentityTemplate.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mapster;

namespace IdentityTemplate.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public AuthController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel vm)
        {
            // eğer model doğru değil ise direkt return ederiz. 
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var identityResult = await _userManager.CreateAsync(new AppUser()
                {
                    Id = Guid.NewGuid().ToString(), Email = vm.Email, UserName = vm.Username, PhoneNumber = vm.Phone
                },
                vm.Password);

            if (identityResult.Succeeded)
            {
                TempData["message"] = "Kullanıcı oluşturma başarılı. Artık Giriş yapabilirsiniz.";
                //ViewBag.message = "Üyelik işlemi başarılı.";
                return RedirectToAction("Login");
            }
            // eğer başarısız ise birden fazla hata mesajı olabilir. bu sebeple bu hataları alabiliriz.
            // burada mvc'nin modelstate özelliğinden yararlanıcağız. 

            foreach (var res in identityResult.Errors)
            {
                ModelState.AddModelError(string.Empty, res.Description);
            }

            return View();
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            return RedirectToAction("Index");
        }
    }
}