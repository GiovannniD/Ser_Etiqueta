using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QRCoder;
using Ser_Etiqueta.Areas.Identity.Data;
using Ser_Etiqueta.Models.DB;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ser_Etiqueta.Controllers
{
    public class FacturaController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly SERETIQUETASContext _context;
        private readonly ILogger<ClienteController> _logger;
        private readonly IEmailSender _emailSender;

       

        public int IdSucursal { get; set; }
        public int IdEmpresa { get; set; }

        public int UltimoConsecutivo { get; set; }

        public string SerieCodigoEtiqueta { get; set; }

        public FacturaController(
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
        [Authorize]
        [Authorize(Roles = "SuperAdmin,Facturacion")]
        public IActionResult Index()
        {
            var users = _userManager.Users.Where(p => p.Id == _userManager.GetUserId(User)).ToList();
            //  String usuario= _userManager.GetUserName(User);

            foreach (ApplicationUser user in users)
            {

                IdEmpresa = user.idEmpresa;
                IdSucursal = user.idSucursal;

                //  IdSucursal = user.idSucursal;

                /*  var thisViewModel = new UserRolesViewModel();
                  thisViewModel.UserId = user.Id;
                  thisViewModel.Email = user.Email;
                  thisViewModel.FirstName = user.FirstName;
                  thisViewModel.LastName = user.LastName;
                  thisViewModel.Roles = await GetUserRoles(user);
                  userRolesViewModel.Add(thisViewModel);*/
            }

            UsuariosInfo rec = new UsuariosInfo
            {
                idEmpresa = IdEmpresa,
                idSucursal = IdSucursal

            };
            return View(rec);
        }
        [HttpPost]
        public IActionResult generarOrden(int idEmpresa,int idSucursal,int noFactura)
        {
            getInfo();
            int count = 0;
            int keyCliente=0;
            int keyMunicipio=0;
            int keyDepartamento = 0;
            string etiqueta = "";
            string direccion = "";
            string codigoCliente = "";
            OrdenTrabajo orden = new OrdenTrabajo()
            {
                IdEmpresa = idEmpresa,
                IdSucursal=idSucursal
            };
                _context.OrdenTrabajos.Add(orden);
               _context.SaveChanges();

            var objFactura = _context.Factura.Where(p => p.KeyFactura == noFactura).AsNoTracking().ToList();
            foreach (var Factura in objFactura) {
                keyCliente = Convert.ToInt32(Factura.KeyCliente);

            }


            var objCliente = _context.Clientes.Where(p => p.IdCliente== keyCliente).AsNoTracking().ToList();
            foreach (var Cliente in objCliente)
            {
                direccion = Cliente.Direccion;
                codigoCliente = Cliente.Codigo;
            }

            var objFacturaDetalle = _context.FacturaDetalle.Where(p => p.KeyFactura == noFactura).AsNoTracking().ToList();
            foreach (var FacturaDetalle in objFacturaDetalle)
            {
               
                var objMunicipio = _context.Municipios.Where(p => p.KeyMunicipio == FacturaDetalle.KeyDestino).AsNoTracking().ToList();
                foreach (var Municipio in objMunicipio) {
                    keyDepartamento = Municipio.KeyDepartamento;
                }
                for (int i = 1; i <= FacturaDetalle.Cantidad; i++)
                {
                    count++;
                    OrdenTrabajoDetalle o = new OrdenTrabajoDetalle
                    {
                         
                        idCliente = keyCliente,
                        idMunicipio = keyMunicipio,
                        IdOrdenTrabajo=orden.IdOrdenTrabajo,
                        IdTipoPaquete = FacturaDetalle.KeyTipoPaquete,
                        direccion = direccion,
                        CantidadBulto = 1,
                        Peso = 0,
                        Factura=noFactura.ToString(),
                        Codigo= codigoCliente
                    };

                    _context.OrdenTrabajoDetalles.Add(o);
                    _context.SaveChanges();


                    Factura factura = new Factura
                    {
                        KeyFactura = noFactura,
                        IsGeneraOT = 1

                    };
                    _context.Entry(factura).Property(p => p.IsGeneraOT).IsModified = true;
                    _context.SaveChanges();
                    UltimoConsecutivo = UltimoConsecutivo + 1;
                    UltimoConsecutivo.ToString().PadLeft(8, '0');
                    UltimoConsecutivo.ToString("00000000");
                    UltimoConsecutivo.ToString("D4");
                    etiqueta = SerieCodigoEtiqueta + "-" + $"{UltimoConsecutivo:00000000}";
                    QRCodeGenerator _qrCode = new QRCodeGenerator();
                    QRCodeData _qrCodeData = _qrCode.CreateQrCode(etiqueta, QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(_qrCodeData);
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);

                    OrdenTrabajoCodigo codigo = new OrdenTrabajoCodigo
                    {

                        IdOtdetalle = o.IdOtdetalle,
                        CodigoSerie = etiqueta,
                        Serie = i + "/" + o.CantidadBulto,
                        Imagen = BitmapToBytesCode(qrCodeImage)

                    };
                    _context.OrdenTrabajoCodigos.Add(codigo);
                    _context.SaveChanges();
                    etiqueta = "";
                }

            }
            Empresa cliente = new Empresa
            {
                IdEmpresa = IdEmpresa,
                UltimoConsecutivo = UltimoConsecutivo


            };
            _context.Attach(cliente);
            _context.Entry(cliente).Property(p => p.UltimoConsecutivo).IsModified = true;
            _context.SaveChanges();


            return Json(count);
           
        }
        private static Byte[] BitmapToBytesCode(Bitmap image)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
        [HttpPost]
        public IActionResult insertFactura(Factura factura)
        {
         
            if (ModelState.IsValid) {

                _context.Factura.Add(factura);
                _context.SaveChanges();
                factura.NoFactura = factura.KeyFactura;
                _context.Entry(factura).Property(p => p.NoFactura).IsModified = true;
                _context.SaveChanges();
                return Json("1");
            }
            return View(factura);
        }


        [HttpPost]
        public IActionResult insertFacturaDetalle(FacturaDetalle factura)
        {

            if (ModelState.IsValid)
            {

                _context.FacturaDetalle.Add(factura);
                _context.SaveChanges();

                var p = _context.SP_FacturaTotal.FromSqlInterpolated($"exec [etiquetas].[SP_FacturaTotal] {factura.KeyFactura},{factura.KeyFacturaDetalle},1");
                return Json(p);
            }
            return Json(ModelState);
        }

        [HttpPost]
        public IActionResult deleteFacturaDetalle(FacturaDetalle factura)
        {

            if (ModelState.IsValid)
            {

                _context.FacturaDetalle.Remove(factura);
                _context.SaveChanges();

                var p = _context.SP_FacturaTotal.FromSqlInterpolated($"exec [etiquetas].[SP_FacturaTotal] {factura.KeyFactura},{factura.KeyFacturaDetalle},2");
                return Json(p);
            }
            return Json(ModelState);
        }

        [HttpPost]
        public IActionResult updateFacturaDetalle(FacturaDetalle factura)
        {

            if (ModelState.IsValid)
            {

                _context.Attach(factura);
                _context.Entry(factura).Property(p => p.PrecioUnitario).IsModified = true;
                _context.Entry(factura).Property(p => p.Cantidad).IsModified = true;
                _context.Entry(factura).Property(p => p.KeyTipoPaquete).IsModified = true;
                _context.SaveChanges();

                var p = _context.SP_FacturaTotal.FromSqlInterpolated($"exec [etiquetas].[SP_FacturaTotal] {factura.KeyFactura},{factura.KeyFacturaDetalle},1");
                return Json(p);
            }
            return Json(ModelState);
        }

        [HttpPost]
        public IActionResult ChangeEstado(Factura factura)
        {

            if (ModelState.IsValid)
            {

                _context.Attach(factura);
                _context.Entry(factura).Property(p => p.KeyFacturaEstatus).IsModified = true;
                _context.SaveChanges();

                return Json("1");
            }
            return Json(ModelState);
        }
        [HttpPost]
        public IActionResult loadFacturaDetalle(FacturaDetalle factura)
        {

            if (ModelState.IsValid)
            {

              var p=  _context.FacturaDetalle.Where(p=>p.KeyFactura==factura.KeyFactura).AsNoTracking().ToList();
                

              
                return Json(p);
            }
            return Json(ModelState);
        }

        [HttpPost]
        public IActionResult getFacturas()
        {

            var facturas=_context.vw_Facturas.AsNoTracking().ToList();


            return Json(facturas);
        }

        [HttpPost]
        public IActionResult GetEstado()
        {

            var facturas = _context.FacturaEstatus.AsNoTracking().ToList();

            
            return Json(facturas);
        }

        [HttpGet]
        public IActionResult FacturaDetalle(int id,string cliente,DateTime fecha,int estado)
        {
            int noFactura = 0;
            string prefijo = "";

            var estadoOT = _context.Factura.Where(p=>p.KeyFactura==id).AsNoTracking().ToList();
            foreach (var item in estadoOT) {
                ViewData["estadoOT"] = item.IsGeneraOT;
               



            }


            if (id == null || id == 0)
            {
                return NotFound();
            }
            getInfo();

            ViewData["estado"] = estado;
            ViewData["cliente"] = cliente;
            ViewData["fecha"] = fecha.ToString("yyyy-MM-dd");


            UsuariosInfo rec = new UsuariosInfo
            {
                idEmpresa = IdEmpresa,
                idSucursal = IdSucursal

            };
            var listSucursales = _context.Sucursales.Where(p => p.IdSucursal == IdSucursal).AsNoTracking().ToList();
            foreach (var item in listSucursales)
            {
                prefijo = item.codigoFactura;
            }

            noFactura = id;
            noFactura.ToString().PadLeft(4, '0');
            noFactura.ToString("00000000");
            noFactura.ToString("D4");

            ViewData["NoFactura"] = prefijo+"-"+$"{id:0000}";
            //ViewData["serieCodigo"] = SerieCodigoEtiqueta;
           // ViewData["consecutivo"] = UltimoConsecutivo;
            if (User.IsInRole("SuperAdmin"))
            {

                return View("FacturaDetalle", rec);
            }

          

            return RedirectToAction("Index");

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
            var codigo = _context.Empresas.Where(p => p.IdEmpresa == IdEmpresa).AsNoTracking().ToList();

            foreach (var emp in codigo)
            {

                SerieCodigoEtiqueta = emp.SerieCodigoEtiqueta;
                UltimoConsecutivo = Convert.ToInt32(emp.UltimoConsecutivo);


            
            }


        }
    }
}
