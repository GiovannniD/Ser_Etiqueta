using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using Ser_Etiqueta.Areas.Identity.Data;
using Ser_Etiqueta.Models.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ser_Etiqueta.Controllers
{
    public class ClienteController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly SERETIQUETASContext _context;
        private readonly ILogger<ClienteController> _logger;
        private readonly IEmailSender _emailSender;
        IWebHostEnvironment _env;
        public int IdSucursal { get; set; }
        public int IdEmpresa { get; set; }
        public int? UltimoConsecutivo { get; set; }


        public ClienteController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<ClienteController> logger,
            IEmailSender emailSender,
            SERETIQUETASContext context,
            IWebHostEnvironment env
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
           // this._usuarios = usuarios;
            this._context = context;
            _env = env;
        }

        [Authorize]
        [Authorize(Roles = "SuperAdmin,Etiqueta,Facturacion")]
        public IActionResult Index()
        {
            var users = _userManager.Users.Where(p => p.Id== _userManager.GetUserId(User)).ToList();
          //  String usuario= _userManager.GetUserName(User);
           
           foreach (ApplicationUser user in users)
           {

                IdEmpresa = user.idEmpresa;
                IdSucursal = user.idSucursal;

              
            }
           
            UsuariosInfo rec = new UsuariosInfo
            {
                idEmpresa = IdEmpresa,
                idSucursal = IdSucursal

            };
            return View(rec);
        }

        public IActionResult Form()
        {
            var model = new Cliente { };

            return PartialView("_ClienteFormPartial", model);
        }

        [HttpPost]
        public IActionResult insert(Cliente cliente)
        {

            if (ModelState.IsValid)
            {
                _context.Clientes.Update(cliente);
                _context.SaveChanges();
                return Json("1");
                //   TempData["Mensaje"] = "El libro se ha editado correctamente";
                //  return Json(ModelState);
            }
            else
            {
                return PartialView("_ClienteFormPartial", cliente);

            }
            //  return PartialView("_clienteFormPartial", cliente);
            
        }
        private void getInfo()
        {
            var users = _userManager.Users.Where(p => p.Id == _userManager.GetUserId(User)).ToList();
            //  String usuario= _userManager.GetUserName(User);

            foreach (ApplicationUser user in users)
            {

                IdEmpresa = user.idEmpresa;
                IdSucursal = user.idSucursal;

         
            }
        }
        [HttpPost]
        public IActionResult cargarClientes()
        {

            if (User.IsInRole("SuperAdmin"))
            {
                var p = _context.SP_CRUD_Clientes.FromSqlInterpolated($"exec [etiquetas].[SP_CRUD_Clientes] null,null,null,null,null,null,'S',''");
                return Json(p);
            }
            else
            {
                getInfo();
                var p = _context.SP_CRUD_Clientes.FromSqlInterpolated($"exec [etiquetas].[SP_CRUD_Clientes] null,{IdEmpresa},null,null,null,null,'S2',''");
                return Json(p);
            }
           
            
        }

        [HttpPost]
        public IActionResult GetCliente(int? IdCliente)
        {
            if (IdCliente == null || IdCliente == 0)
            {
                return NotFound();
            }
            //obtener libro
            var cliente = _context.Clientes.Find(IdCliente);
            if (cliente == null)
            {
                return NotFound();
            }

            return Json(cliente);
        }

        [HttpPost]
        public IActionResult GetClienteCodigo(string? Codigo)
        {
            getInfo();
            if (Codigo == null || Codigo == "")
            {
                return NotFound();
            }
            //obtener libro
            var cliente = _context.Clientes.Where(p=>p.Codigo==Codigo && p.IdEmpresa == IdEmpresa && p.IdSucursal == IdSucursal).ToList();
            if (cliente == null)
            {
                return NotFound();
            }

            return Json(cliente);
        }

        [HttpPost]
        public IActionResult GetClienteNombre(string? Nombre)
        {
            if (Nombre == null || Nombre == "")
            {
                return NotFound();
            }
            //obtener libro
            var cliente = _context.Clientes.Where(p => p.NombreCliente == Nombre).ToList();
            if (cliente == null)
            {
                return NotFound();
            }

            return Json(cliente);
        }

        [HttpPost]
        public IActionResult Updatecliente(Cliente cliente)
        {

            if (ModelState.IsValid)
            {

                _context.Attach(cliente);
                _context.Entry(cliente).Property(p => p.NombreCliente).IsModified = true;
                _context.Entry(cliente).Property(p => p.NombreComercial).IsModified = true;
                _context.Entry(cliente).Property(p => p.Telefono).IsModified = true;
                _context.Entry(cliente).Property(p => p.Direccion).IsModified = true;
                _context.Entry(cliente).Property(p => p.Cargo).IsModified = true;
                _context.Entry(cliente).Property(p => p.Email).IsModified = true;
                _context.Entry(cliente).Property(p => p.Contacto).IsModified = true;
                _context.Entry(cliente).Property(p => p.Movil).IsModified = true;
                _context.Entry(cliente).Property(p => p.KeyDepartamento).IsModified = true;
                _context.Entry(cliente).Property(p => p.KeyMunicipio).IsModified = true;
                _context.Entry(cliente).Property(p => p.Codigo).IsModified = true;
                _context.SaveChanges();

                return Json("1");
                //   TempData["Mensaje"] = "El libro se ha editado correctamente";
                //  return Json(ModelState);
            }
            else
            {
                return PartialView("_ClienteFormPartial", cliente);
            }
            //  return PartialView("_clienteFormPartial", cliente);
        }

       

            [HttpPost]
        public async Task<IActionResult> cargarExcel(int Empresa,int Sucursal)
        {


            getInfo();
            IFormFile file = Request.Form.Files[0];
            var list = new List<Cliente>();
            var Departamento = new List<Departamento>();
            int KeyDepartamento = 0;
            int KeyMunicipio = 0;
            using (var stream=new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package= new ExcelPackage(stream)) {
                    ExcelWorksheet hoja = package.Workbook.Worksheets[0];
                    var rowCount = hoja.Dimension.Rows;
                    
                    for (int row = 2; row <= rowCount; row++)
                    {

                  
                         var dep = _context.Departamentos.Where(p =>p.DescripcionDep== (hoja.Cells[row, 10].Value ?? string.Empty).ToString().ToUpper().Trim()).ToList();
                        var mun = _context.Municipios.Where(p => p.DescripcionMun == (hoja.Cells[row, 11].Value ?? string.Empty).ToString().ToUpper().Trim()).ToList();
                        if (dep != null)
                        {
                            foreach (var item in mun)
                            {
                                KeyMunicipio= item.KeyMunicipio;
                            }
                        }
                        else {
                            KeyMunicipio = 0;
                        }

                        if (dep != null)
                        {
                            foreach (var item in dep)
                            {
                                KeyDepartamento = item.KeyDepartamento;
                            }
                        }
                        else
                        {
                            KeyDepartamento = 0;
                        }
                        Cliente cli =new Cliente 
                        {
                            IdEmpresa=Empresa,
                            IdSucursal=Sucursal,
                            Codigo = (hoja.Cells[row, 1].Value ?? string.Empty).ToString().Trim(),
                            NombreCliente= (hoja.Cells[row, 2].Value ?? string.Empty).ToString().Trim(),
                            NombreComercial = (hoja.Cells[row, 3].Value ?? string.Empty).ToString().Trim(),
                            Contacto = (hoja.Cells[row, 4].Value ?? string.Empty).ToString().Trim(),
                            Cargo = (hoja.Cells[row, 5].Value ?? string.Empty).ToString().Trim(),
                            Email = (hoja.Cells[row, 6].Value ?? string.Empty).ToString().Trim(),
                            Telefono= (hoja.Cells[row, 7].Value ?? string.Empty).ToString().Trim(),
                            Movil = (hoja.Cells[row, 8].Value ?? string.Empty).ToString().Trim(),
                            Direccion = (hoja.Cells[row, 9].Value ?? string.Empty).ToString().Trim(),
                            KeyDepartamento = KeyDepartamento,
                            KeyMunicipio =KeyMunicipio
                        };
                        _context.Clientes.Add(cli);
                    }


                    try
                    {
                        _context.SaveChanges();
                        return Json("1");
                    }
                    catch (DbUpdateException e)
                    {
                        //This either returns a error string, or null if it can’t handle that error
                        var error = CheckHandleError(e);
                        if (error != null)
                        {
                            return Json(error); //return the error string
                        }
                        throw; //couldn’t handle that error, so rethrow
                    }
                }
            }
            //return list;
        }

        private object CheckHandleError(DbUpdateException e)
        {
            throw new NotImplementedException();
        }
    }
}
