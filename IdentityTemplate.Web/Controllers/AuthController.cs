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
using Microsoft.AspNetCore.Authorization;

namespace IdentityTemplate.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager,
            SignInManager<AppUser> signInManager)
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
            ModelState.AddModelErrorList(identityResult.Errors.Select(x => x.Description).ToList());

            return View();
        }


        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            var x = returnUrl;            
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string returnUrl2 = HttpContext.Request.Query["returnUrl"];
            returnUrl = returnUrl ?? Url.Action("Index","Member");

            /*
             * Önce db'den user'ı bulmak lazım. ardından o user ile giriş yapılması gerekiyor.
             */

            var loginUser = await _userManager.FindByEmailAsync(model.Email);

            if (loginUser == null)
            {
                ModelState.AddModelError(string.Empty, "Email veya şifre yanlış");
            }

            // eğer lockoutOnFailure özelliğini true yaparsak kitleme mekanizmasını açmış oluruz. 
            // yani kullanıcı 3 kez art arta yanlış girerse kitlesin tarzı bir mekanizmayı aktif yapmış oluruz. 
            var result = await _signInManager.PasswordSignInAsync(loginUser, model.Password, model.rememberMe, true);

            if (result.Succeeded)
            {
                
                return Redirect(returnUrl);
            }

            if (result.IsLockedOut)
            {
                // eğer kilitlendi ise kullanıcı : 
                ModelState.AddModelErrorList(new List<string>() { $"Yanlış Şifre Girişinden Dolayı Hesabınız Kilitlenmiştir. {await _userManager.GetLockoutEndDateAsync(loginUser)} tarihine kadar giriş yapamazsınız." });
                return View();
            }

            ModelState.AddModelErrorList(new List<string>()
            {
                $"Email veya şifre yanlış. Başarısız Giriş Sayısı ={await _userManager.GetAccessFailedCountAsync(loginUser)} "
            });

            return View();
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
           await  _signInManager.SignOutAsync();
           return Redirect("/Home/Index");
        }


        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async  Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
           
            // şifre yenileme işlemleri e posta ile yapılmaktadır.
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            
            if (user == null)
            {
                ModelState.AddModelError(string.Empty , "Bu email adresine sahip kullanıcı bulunamadı");
                return View();
            }
            
            // şifre sıfırlama işlemleri için bir adet token üretmemiz gerekmektedir.
            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var passwordResetLink = Url.Action("ResetPassword", "Auth",
                new{ userId = user.Id, Token = passwordResetToken });
            
            
            // localhost:7006?userId = 20&toke=sdasdgds şeklinde bir url oluşacaktır. 
            
            // Email Servis gerekli. Email servisi ile bir email gönderme işlemi yapacağız. 


            // security stamp değeri => kullanıcının kritik bilgilerini değiştirirken güncellenmesi gereken alandır. 
            await _userManager.UpdateSecurityStampAsync(user); // şeklinde her kritik bilgi değişimde update edilmesi gerekmektedi.r
            // identity cookie'si içerisinde bu değer vardır. 
            // 30 dakikada bir güncellenir. 
            // bunu 30 dakika sonra cookie ile db eşleşmeşse kullanıcıyı logout yaparız. 
            TempData["message"] = "Şifre Yenileme linki e posta adresinize gönderilmiştir.";
            
            return Redirect("/Auth/Login");
        }
    }
}