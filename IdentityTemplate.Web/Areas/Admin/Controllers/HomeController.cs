using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityTemplate.Web.Areas.Admin.Models;
using IdentityTemplate.Web.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityTemplate.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public HomeController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var usercount = _userManager.Users.Count();
            var rolecount = _roleManager.Roles.Count();
            
            return View(new AdminPanelViewModel(){ UserCount = usercount, RoleCount = rolecount});
        }

        public async Task<IActionResult> AllUser()
        {
            var users = await _userManager.Users.ToListAsync();
            var userVm = users.Adapt<List<UserViewModel>>();
            return View(userVm);
        }
        
    }
}