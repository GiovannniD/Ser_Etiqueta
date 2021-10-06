using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ser_Etiqueta.Areas.Identity.Data;
using Ser_Etiqueta.Models;
using Ser_Etiqueta.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ser_Etiqueta.Controllers
{
    [Authorize]
    [Authorize(Roles = "SuperAdmin")]
    public class UserRolesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SERETIQUETASContext _context;
        public UserRolesController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SERETIQUETASContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            this._context = context;
        }

        public IActionResult EditUser()
        {
           return RedirectToAction("Index");
        }
            public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UserRolesViewModel>();
            foreach (ApplicationUser user in users)
            {
                var thisViewModel = new UserRolesViewModel();
                thisViewModel.UserId = user.Id;
                thisViewModel.Email = user.Email;
                thisViewModel.FirstName = user.FirstName;
                thisViewModel.LastName = user.LastName;
                thisViewModel.Empresa = Empresa(user.idEmpresa);
                thisViewModel.Sucursal = Sucursal(user.idSucursal);
                thisViewModel.Roles = await GetUserRoles(user);
                userRolesViewModel.Add(thisViewModel);
            }
            return View(userRolesViewModel);
        }
        private async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
          
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

        private  string Empresa(int idEmpresa)
        {
            string NombreEmpresa = "";
            var item = _context.Empresas.Where(p => p.IdEmpresa == idEmpresa).AsNoTracking();
            foreach (var emp in item) {
                NombreEmpresa = emp.NombreEmpresa;
            };
            return NombreEmpresa;
        }

        private string Sucursal(int idSucursal)
        {
            string NombreSucursal = "";
            var item = _context.Sucursales.Where(p => p.IdSucursal == idSucursal).AsNoTracking();
            foreach (var emp in item)
            {
                NombreSucursal = emp.NombreSucursal;
            };
            return NombreSucursal;
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Manage(string userId)
        {
            ViewBag.userId = userId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            ViewBag.UserName = user.UserName;
            var model = new List<ManageUserRolesViewModel>();
            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new ManageUserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                model.Add(userRolesViewModel);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Manage(List<ManageUserRolesViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View();
            }
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "No se pueden eliminar los roles actuales");
                return View(model);
            }
            result = await _userManager.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "no se puede agregar el rol seleccionado ");
                return View(model);
            }
            return RedirectToAction("Index");
        }
    }
}
