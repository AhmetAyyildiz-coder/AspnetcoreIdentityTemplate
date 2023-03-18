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
            var user = vm.Adapt<AppUser>();
            var identityResult =  await  _userManager.CreateAsync(user);
            if (identityResult.Succeeded)
            {
                TempData["message"] = "Kullanıcı oluşturma başarılı. Artık Giriş yapabilirsiniz.";
                return RedirectToAction("Login");
            
                
            }
            return RedirectToAction("SignUp");

        }
        
        
        
    }
}