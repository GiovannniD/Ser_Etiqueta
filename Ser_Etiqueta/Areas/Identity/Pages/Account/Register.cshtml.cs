using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ser_Etiqueta.Areas.Identity.Data;
using Ser_Etiqueta.Areas.Identity.Pages.Account.Manage;
using Ser_Etiqueta.Models.DB;

namespace Ser_Etiqueta.Areas.Identity.Pages.Account
{

    [Authorize]
    [Authorize(Roles = "SuperAdmin")]
  
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        private readonly SERETIQUETASContext _context;
        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            SERETIQUETASContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            this._context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public List<SP_CRUD_EMPRESAS> p { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Nombres")]
            public string FirstName { get; set; }
            [Required]
            [Display(Name = "Apellidos")]
            public string LastName { get; set; }

            
            public int idEmpresa { get; set; }

            [Required]
            public int idSucursal { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Correo")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "la contraseña debe contener al menos {2} y maximo {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "contraseña")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar contraseña")]
            [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
            public string ConfirmPassword { get; set; }
        }

        public JsonResult obetener()
        {
            return new JsonResult("hola");
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            p = _context.SP_CRUD_EMPRESAS.FromSqlInterpolated($"exec [configuration].[SP_CRUD_Empresas] null,null,null,null,null,null,null,null,null,null,'S',''").ToList();

        }


        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            p = _context.SP_CRUD_EMPRESAS.FromSqlInterpolated($"exec [configuration].[SP_CRUD_Empresas] null,null,null,null,null,null,null,null,null,null,'S',''").ToList();
            if (ModelState.IsValid)
            {
                MailAddress address = new MailAddress(Input.Email);
                string userName = address.User;
                var user = new ApplicationUser
                {
                    UserName = userName,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    idEmpresa=Input.idEmpresa,
                    idSucursal=Input.idSucursal
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {

                    _logger.LogInformation("User created a new account with password.");
                    await _userManager.AddToRoleAsync(user, Enum.Roles.Etiqueta.ToString());
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                     //   await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

   
   

    }
}
