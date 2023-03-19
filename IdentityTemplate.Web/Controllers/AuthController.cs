using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityTemplate.Web.Areas.Admin.Models;
using IdentityTemplate.Web.Extensions;
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
        private readonly SignInManager<AppUser> _signInManager;

        public AuthController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
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

            // foreach (var res in identityResult.Errors)
            // {
            //     ModelState.AddModelError(string.Empty, res.Description);
            // }
            ModelState.AddModelErrorList(identityResult.Errors.Select(x=> x.Description).ToList());

            return View();
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            returnUrl = returnUrl ?? Url.Action("Index");
            
            /*
             * Önce db'den user'ı bulmak lazım. ardından o user ile giriş yapılması gerekiyor.
             */

            var loginUser = await _userManager.FindByEmailAsync(model.Email);

            if (loginUser == null)
            {
                ModelState.AddModelError(string.Empty , "Email veya şifre yanlış");
                
            }

            // eğer lockoutOnFailure özelliğini true yaparsak kitleme mekanizmasını açmış oluruz. 
            // yani kullanıcı 3 kez art arta yanlış girerse kitlesin tarzı bir mekanizmayı aktif yapmış oluruz. 
            var result = await _signInManager.PasswordSignInAsync(loginUser, model.Password, model.rememberMe,true);

            if (result.Succeeded)
            {
                string url = Url.Action("Index", "Home");
                return Redirect(url);
            }

           ModelState.AddModelErrorList(new List<string>(){"Email veya şifre yanlış"});
            
            return View();
        }
    }
}