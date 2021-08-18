using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ser_Etiqueta.Areas.Identity.Data;
using Ser_Etiqueta.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ser_Etiqueta.Controllers
{
    public class TipoController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly SERETIQUETASContext _context;
        private readonly ILogger<ClienteController> _logger;
        private readonly IEmailSender _emailSender;

        public TipoController(
       UserManager<ApplicationUser> userManager,
       SignInManager<ApplicationUser> signInManager,
       ILogger<ClienteController> logger,
       IEmailSender emailSender,
       SERETIQUETASContext context
       )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            // this._usuarios = usuarios;
            this._context = context;
        }
        public object TipoPaquetes()
        {
            var tipo = _context.TipoPaquetes.Where(p => p.IdTipoPaquete <=9).ToList();
            return tipo;
        }
    }
}
