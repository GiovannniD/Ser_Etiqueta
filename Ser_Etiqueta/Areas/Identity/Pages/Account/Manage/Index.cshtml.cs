using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Ser_Etiqueta.Areas.Identity.Data;
using Ser_Etiqueta.Models.DB;

namespace Ser_Etiqueta.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly SERETIQUETASContext _context;
        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            SERETIQUETASContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this._context = context;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }
        [TempData]
        public string UserNameChangeLimitMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Nombres")]
            public string FirstName { get; set; }
            [Required]
            [Display(Name = "Apellidos")]
            public string LastName { get; set; }
            [Required]
            [Display(Name = "Usuario")]
            public string Username { get; set; }
            [Phone]
            [Display(Name = "Numero de telefono")]
            public string PhoneNumber { get; set; }
            [Display(Name = "Foto de perfil")]
            public byte[] ProfilePicture { get; set; }

            public int idEmpresa { get; set; }

            public int idSucursal { get; set; }
        }
        public List<SP_CRUD_EMPRESAS> p { get; set; }
        public List<Sucursale> listSucursales { get; set; }
        public int Empresa { get; set; }
        public int SelectedSucursal { get; set; }
        [Authorize]
        [Authorize(Roles = "SuperAdmin,Admin,Moderator,Basic")]
        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            var firstName = user.FirstName;
            var lastName = user.LastName;
            var profilePicture = user.ProfilePicture;
             var idEmpresa = user.idEmpresa;
             Empresa= user.idEmpresa;
            SelectedSucursal = user.idSucursal;
            var idSucursal = user.idSucursal;
            Username = userName;
            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Username = userName,
                FirstName = firstName,
                LastName = lastName,
                ProfilePicture = profilePicture,
                idEmpresa = idEmpresa,
                idSucursal=idSucursal
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            UserNameChangeLimitMessage = $"You can change your username {user.UsernameChangeLimit} more time(s).";
            await LoadAsync(user);
            p = _context.SP_CRUD_EMPRESAS.FromSqlInterpolated($"exec [configuration].[SP_CRUD_Empresas] null,null,null,null,null,null,null,null,null,null,'S',''").ToList();
            listSucursales = _context.Sucursales.Where(p => p.IdEmpresa == Empresa).ToList();
            return Page(); 
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (ModelState.IsValid)
            {
                var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
                if (Input.PhoneNumber != phoneNumber)
                {
                    var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                    if (!setPhoneResult.Succeeded)
                    {
                        StatusMessage = "Unexpected error when trying to set phone number.";
                        return RedirectToPage();
                    }
                }
                var firstName = user.FirstName;
                var lastName = user.LastName;
                var idEmpresa = user.idEmpresa;
                var idSucursal = user.idSucursal;
                if (Input.FirstName != firstName && Input.FirstName!=null)
                {
                    user.FirstName = Input.FirstName;
                    await _userManager.UpdateAsync(user);
                }
                if (Input.LastName != lastName)
                {
                    user.LastName = Input.LastName;
                    await _userManager.UpdateAsync(user);
                }

                if (Input.idEmpresa != idEmpresa)
                {
                    user.idEmpresa = Input.idEmpresa;
                    await _userManager.UpdateAsync(user);
                }

                if (Input.idSucursal != idSucursal)
                {
                    user.idSucursal = Input.idSucursal;
                    await _userManager.UpdateAsync(user);
                }
                if (Request.Form.Files.Count > 0)
                {
                    IFormFile file = Request.Form.Files.FirstOrDefault();
                    using (var dataStream = new MemoryStream())
                    {
                        await file.CopyToAsync(dataStream);
                        user.ProfilePicture = dataStream.ToArray();
                    }
                    await _userManager.UpdateAsync(user);
                }

                if (user.UsernameChangeLimit > 0)
                {
                    if (Input.Username != user.UserName)
                    {
                        var userNameExists = await _userManager.FindByNameAsync(Input.Username);
                        if (userNameExists != null)
                        {
                            StatusMessage = "El usuario ya existe. Ingrese uno diferente.";
                            return RedirectToPage();
                        }
                        var setUserName = await _userManager.SetUserNameAsync(user, Input.Username);
                        if (!setUserName.Succeeded)
                        {
                            StatusMessage = "Unexpected error when trying to set user name.";
                            return RedirectToPage();
                        }
                        else
                        {
                            user.UsernameChangeLimit -= 1;
                            await _userManager.UpdateAsync(user);
                        }
                    }
                }

            }
            else
            {
                await LoadAsync(user);
                return Page();
            }



            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Su perfil has sido actualizado";
            return RedirectToPage();
        }
        public IActionResult Index()
        {
            return Page();
        }
    }
}
