using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using QRCoder;
using Ser_Etiqueta.Areas.Identity.Data;
using Ser_Etiqueta.Models.DB;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ser_Etiqueta.Controllers
{
    public class FacturaController : Controller
    {
        IWebHostEnvironment _env;
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
            int keyDestino = 0;
            string prefijo = "";
            string codigoFactura = noFactura.ToString();

            codigoFactura.ToString().PadLeft(4, '0');

           
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
                prefijo = Factura.Prefijo;

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
                keyDestino = (int)FacturaDetalle.KeyDestino;
                var objMunicipio = _context.vw_Sersa_Destinos.Where(p => p.KeyDestino == FacturaDetalle.KeyDestino).AsNoTracking().ToList();
                foreach (var municipio in objMunicipio)
                {
  
                        keyMunicipio = municipio.KeyMunicipio;
                  

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
                        direccion = FacturaDetalle.Destinatario,
                        CantidadBulto = 1,
                        Peso = 0.00m,
                        Factura=prefijo+"-"+ $"{noFactura:0000}",
                        Codigo= codigoCliente,
                        KeyOrigen = (int)FacturaDetalle.KeyOrigen,
                        keyDestino = (int)keyDestino
                    };

                    _context.OrdenTrabajoDetalles.Add(o);
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
            Factura factura = new Factura
            {
                KeyFactura = noFactura,
                IsGeneraOT = 1,
                idOrdentrabajo=orden.IdOrdenTrabajo

            };

              _context.Entry(factura).Property(p => p.IsGeneraOT).IsModified = true;
            _context.Entry(factura).Property(p => p.idOrdentrabajo).IsModified = true;
            _context.SaveChanges();
            Empresa cliente = new Empresa
            {
                IdEmpresa = IdEmpresa,
                UltimoConsecutivo = UltimoConsecutivo


            };
            _context.Attach(cliente);
            _context.Entry(cliente).Property(p => p.UltimoConsecutivo).IsModified = true;
            _context.SaveChanges();


            return Json(orden);
           
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

           string Prefijo="";
            if (ModelState.IsValid) {
                getInfo();
                var p = _context.vw_Sersa_Clientes.Where(p => p.KeyCliente == factura.KeyCliente).AsNoTracking().ToList();
                var prefijo = _context.Sucursales.Where(p => p.IdSucursal == IdSucursal).AsNoTracking().ToList();
                foreach (var item in prefijo) {
                    Prefijo = item.codigoFactura;
                }

                
                factura.Prefijo = Prefijo;
                _context.Factura.Add(factura);
                _context.SaveChanges();
                factura.NoFactura = factura.KeyFactura;
                _context.Entry(factura).Property(p => p.NoFactura).IsModified = true;
                _context.SaveChanges();

                return Json("1");
                /*    var count = _context.Clientes
                .Where(o => o.Codigo == factura.KeyCliente.ToString())
                .Count();
                    if (count == 0)
                    {
                        foreach (var item in p)
                        {
                            Cliente cliente = new Cliente
                            {
                                Codigo = item.KeyCliente.ToString(),
                                IdEmpresa = IdEmpresa,
                                IdSucursal = IdSucursal,
                                NombreCliente = item.DescripcionCliente,
                                NombreComercial = item.NombreComercial,
                                Telefono = item.Telefono,
                                Contacto = "-",
                                Direccion = item.Direccion,
                                Cargo = "-",
                                KeyDepartamento = 9,
                                KeyMunicipio = 83,
                                Email = "-",
                                Movil = "-"
                            };
                            _context.Clientes.Add(cliente);
                            _context.SaveChanges();
                            factura.KeyCliente = cliente.IdCliente;
                            factura.Prefijo = Prefijo;
                            _context.Factura.Add(factura);
                           _context.SaveChanges();
                            factura.NoFactura = factura.KeyFactura;
                            _context.Entry(factura).Property(p => p.NoFactura).IsModified = true;
                            _context.SaveChanges();

                            return Json("1");
                        }

                    }
                    else
                    {
                        var c = _context.Clientes.Where(p => p.Codigo == factura.KeyCliente.ToString()).AsNoTracking().ToList();
                        foreach (var item in c)
                        {

                            factura.KeyCliente = item.IdCliente;
                            factura.Prefijo = Prefijo;
                            _context.Factura.Add(factura);
                            _context.SaveChanges();
                            factura.NoFactura = factura.KeyFactura;
                            _context.Entry(factura).Property(p => p.NoFactura).IsModified = true;
                            _context.SaveChanges();

                            return Json("1");

                        }

                    }*/


                /* _context.Factura.Add(factura);
                 _context.SaveChanges();
                 factura.NoFactura = factura.KeyFactura;
                 _context.Entry(factura).Property(p => p.NoFactura).IsModified = true;
                 _context.SaveChanges();*/
                //  return View(count);
            }
            return View(factura);
        }

        [HttpGet]
        public IActionResult DownloadExcelFactura(DateTime fecha1, DateTime fecha2)
        {
            if (User.IsInRole("SuperAdmin"))
            {
                var dep = _context.vw_vistaExportFactura.Where(p => p.FechaElaboracion >= fecha1 && p.FechaElaboracion <= fecha2).AsNoTracking().ToList();
                var stream = new MemoryStream();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(dep, true);
                    package.Save();
                }
                stream.Position = 0;
                string ExcelName = $"Facturacion-{DateTime.Now.ToString("yyyymmddHmmssfff")}.xls";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", ExcelName);
            }
            else if(User.IsInRole("Facturacion"))
            {
                getInfo();
                var dep = _context.vw_vistaExportFactura.Where(p => p.FechaElaboracion >= fecha1 && p.FechaElaboracion <= fecha2 && p.UserName== _userManager.GetUserName(User)).AsNoTracking().ToList();
                var stream = new MemoryStream();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(dep, true);
                    package.Save();
                }
               
                stream.Position = 0;
                string ExcelName = $"Facturacion-{DateTime.Now.ToString("yyyymmddHmmssfff")}.xls";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", ExcelName);
            }
            return Json("Ocurrio un error");


        }

        [HttpPost]
        public IActionResult Filtro(string fechaInicio, string fechaFinal)
        {
            DateTime f1 = Convert.ToDateTime(fechaInicio);
            DateTime f2 = Convert.ToDateTime(fechaFinal);
            if (User.IsInRole("SuperAdmin"))
            {
                var facturas = _context.vw_Facturas.Where(p=>p.FechaElaboracion>=f1 && p.FechaElaboracion<=f2).AsNoTracking().ToList();
                return Json(facturas);
            }
            else
            {
                getInfo();
                var facturas = _context.vw_Facturas.Where(p => p.FechaElaboracion >= f1 && p.FechaElaboracion <= f2 && p.UserName == _userManager.GetUserName(User)).AsNoTracking().ToList();
                return Json(facturas);
            }


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
            if (User.IsInRole("SuperAdmin"))
            {
                var facturas = _context.vw_Facturas.AsNoTracking().ToList();
                return Json(facturas);
            }
            else if (User.IsInRole("Facturacion"))
            {
                var facturas = _context.vw_Facturas.Where(p=>p.UserName==_userManager.GetUserName(User)).AsNoTracking().ToList();
                return Json(facturas);
            }
            return Json("Ocurrio un error");
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
            int idCliente=0;
            var estadoOT = _context.Factura.Where(p=>p.KeyFactura==id).AsNoTracking().ToList();
            foreach (var item in estadoOT) {
                ViewData["estadoOT"] = item.IsGeneraOT;
                ViewData["idOrdenTrabajo"] = item.idOrdentrabajo;
                idCliente = Convert.ToInt32(item.KeyCliente);
            }
            //se obtiene el codigo del cliente para extraer los destinos
            var clientes = _context.Clientes.Where(p => p.IdCliente == idCliente).AsNoTracking().ToList();
            foreach (var item in clientes)
            {
                ViewData["idSersa"] = item.Codigo;
            }
                if (id == null || id == 0)
            {
                return NotFound();
            }
            getInfo();
            var idSersa = _context.Empresas.Where(p => p.IdEmpresa == IdEmpresa).AsNoTracking();
            foreach (var item in idSersa)
            {
                ViewData["idSersa"] = item.IdSersa;
            }
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
            if (User.IsInRole("SuperAdmin") || User.IsInRole("Facturacion"))
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

        [HttpGet]
        public IActionResult imprimirFactura(int? id)
        {
            int IdEmpresaOrden = 0;
            int IdSucursalOrden = 0;
            string fecha = "";
            int idSersa = 0;
            int idSucursal = 0;
            decimal subtotal = 0;
            decimal iva = 0;
            decimal total = 0;
            DateTime fechaFactura=new DateTime();
            var facturas = _context.FacturaDetalle.Where(p => p.KeyFactura == id).AsNoTracking();
            var facturasMaster = _context.Factura.Where(p => p.KeyFactura == id).AsNoTracking() ;
            foreach (var item in facturasMaster) {
                idSucursal = Convert.ToInt32(item.IdSucursal);
                subtotal = item.Subtotal;
                iva = item.IVA;
                total = item.Total;
                fechaFactura = item.FechaElaboracion;
                var codigo = _context.Clientes.Where(p => p.IdCliente == item.KeyCliente);
                foreach (var cli in codigo)
                {
                    idSersa = Convert.ToInt32(cli.Codigo);
                    
                }

                }
            OrdenTrabajoDetalle model = new OrdenTrabajoDetalle();


            PdfDocument document = new PdfDocument();
            document.Info.Title = "Factura de envios";


            // Page Options
            PdfPage pdfPage = document.AddPage();

            XSize size = PageSizeConverter.ToSize(PdfSharp.PageSize.A4);
            pdfPage.Width = XUnit.FromMillimeter(215.9);
            pdfPage.Height = XUnit.FromMillimeter(139.7);
            pdfPage.Orientation = PdfSharp.PageOrientation.Portrait;
            pdfPage.Rotate = 0;
           /* if (pdfPage.Orientation == PageOrientation.Landscape)
            {
                pdfPage.Width = size.Height;
                pdfPage.Height = size.Width;
            }
            else
            {
                pdfPage.Width = size.Width;
                pdfPage.Height = size.Height;
            }

            // default unit in points 1 inch = 72 points
            pdfPage.TrimMargins.Top = 5;
            pdfPage.TrimMargins.Right = 5;
            pdfPage.TrimMargins.Bottom = 5;
            pdfPage.TrimMargins.Left = 5;*/

            // Get an XGraphics object for drawing
            XGraphics graph = XGraphics.FromPdfPage(pdfPage);

            // Text format
            XStringFormat format = new XStringFormat();
            format.LineAlignment = XLineAlignment.Near;
            format.Alignment = XStringAlignment.Near;
            var tf = new XTextFormatter(graph);

            XFont fontParagraph = new XFont("Arial", 8, XFontStyle.Bold);
            XFont titulo = new XFont("Arial", 14, XFontStyle.Bold);
            XFont Subtitulo = new XFont("Arial", 12, XFontStyle.Bold);
            XFont info = new XFont("Arial", 9, XFontStyle.Bold);
            XFont font2 = new XFont("Arial", 12, XFontStyle.Bold);

            // Row elements
            int el1_width = 80;
            int el2_width = 80;
            int el3_width = 135;
            int el4_width = 120;
            int el5_width = 80;
            int el6_width = 150;

            // page structure options
            double lineHeight = 16;
            int marginLeft = 20;
            int marginTop = 100;

            int el_height = 20;
            int rect_height = 15;

            int interLine_X_1 = 2;
            int interLine_X_2 = 2 * interLine_X_1;
            int interLine_X_3 = 2 * interLine_X_2;
            int interLine_X_4 = 2 * interLine_X_3;
            int interLine_X_5 = 2 * interLine_X_4;
            int interLine_X_6 = 2 * interLine_X_5;


            int offSetX_1 = el1_width;
            int offSetX_2 = el1_width + el2_width;
            int offSetX_3 = el1_width + el2_width + el3_width;
            int offSetX_4 = el1_width + el2_width + el3_width + el4_width;
            int offSetX_5 = el1_width + el2_width + el3_width + el4_width + el5_width;
            int offSetX_6 = el1_width + el2_width + el3_width + el4_width + el5_width + el6_width;


            XSolidBrush rect_style1 = new XSolidBrush(XColors.White);
            XSolidBrush rect_style2 = new XSolidBrush(XColors.Red);
            XSolidBrush rect_style3 = new XSolidBrush(XColors.Red);
            int i = -1;
            int a = 500;
            int c = 1;
            XImage xfoto = XImage.FromFile(_env.WebRootPath + @"\logo_SER.jpg");
            graph.DrawImage(xfoto, 12, 10, 120, 53);
            XImage firma = XImage.FromFile(_env.WebRootPath + @"\firma.PNG");
            graph.DrawImage(firma, 15, 300,400, 23);
            XImage pie = XImage.FromFile(_env.WebRootPath + @"\pie_factura.PNG");
            graph.DrawImage(pie, 15, 330, 570, 65);
            XImage fechaimg = XImage.FromFile(_env.WebRootPath + @"\fecha.PNG");
            graph.DrawImage(fechaimg, 520, 45, 60, 40);
            XImage totalimg = XImage.FromFile(_env.WebRootPath + @"\total.PNG");
            graph.DrawImage(totalimg, 408, 230, 180, 100);

            graph.DrawString("SERVICIOS DE ENCOMIENDAS RAPIDAS S.A", titulo, XBrushes.Black, new XPoint(140, 25));
            graph.DrawString("contado", titulo, XBrushes.Black, new XPoint(520, 25));
            graph.DrawString(fechaFactura.ToString("dd/MM/yyyy"), info, XBrushes.Black, new XPoint(525, 75));
            graph.DrawString("SERVICIOS DE ENCOMIENDAS RAPIDAS S.A", titulo, XBrushes.Black, new XPoint(140, 25));
            graph.DrawString("RUC:  J0310000013608", Subtitulo, XBrushes.Black, new XPoint(140, 38));
            
            graph.DrawString("Ant. Hospital Militar 3C. al Lago, 20 Vrs Este #30. Managua-Nicaragua", info, XBrushes.Black, new XPoint(140, 48));
            graph.DrawString("Administración: (505) 2222 6356 Operaciones: (505) 2254 5529", info, XBrushes.Black, new XPoint(140, 60));
            graph.DrawString("AUTORIZADO DGI      ASFC 04/0090/06/2016/9", info, XBrushes.Black, new XPoint(140, 72));
            XPen pen = new XPen(XColors.Black, 2);
            /* XPen lineRed = new XPen(XColors.Red, 2);

             graph.DrawLine(lineRed, 100, 100, 50, 50);*/
            foreach (var item in facturas)


            {
                graph.DrawString(subtotal.ToString(), font2, XBrushes.Black, new XPoint(515, 250));
                graph.DrawString("0.00", font2, XBrushes.Black, new XPoint(525, 274));
                graph.DrawString(iva.ToString(), font2, XBrushes.Black, new XPoint(520, 297));
                graph.DrawString(total.ToString(), font2, XBrushes.Black, new XPoint(515, 320));
                //  model.Codigo = item.Codigo;
                i++;
                a++;
                double dist_Y = lineHeight * (i + 1.1);
                double dist_Y2 = dist_Y - 2;
                a.ToString().PadLeft(8, '0');
                a.ToString("00000000");
                a.ToString("D4");
                item.KeyFactura.ToString().PadLeft(3, '0');
                var sucursal = _context.Sucursales.Where(p => p.IdSucursal == idSucursal).AsNoTracking();
                foreach (var suc in sucursal)
                {
                    graph.DrawString("Factura #   "+suc.codigoFactura+"-" + $"{ item.KeyFactura:000}", titulo, XBrushes.Black, new XPoint(450, 39));
                }
                var cliente= _context.vw_Sersa_Clientes.Where(p => p.KeyCliente == idSersa).AsNoTracking();
                foreach (var clientes in cliente)
                {
                    if (clientes.NombreComercial != null)
                    graph.DrawString("Señor(es): "+clientes.NombreComercial+"-RUC:"+clientes.RUC+"", info, XBrushes.Black, new XPoint(18, 95));
                    else
                        graph.DrawString("Señor(es): " + clientes.DescripcionCliente + "-RUC:" + clientes.RUC + "", info, XBrushes.Black, new XPoint(18, 95));
                }
      
                   
                  
                    graph.DrawRectangle(rect_style2, marginLeft, marginTop, pdfPage.Width - 2 * marginLeft, rect_height);

                    tf.DrawString("Cantidad", fontParagraph, XBrushes.White,
                                  new XRect(40, 103, el1_width, el_height), format);

                    tf.DrawString("Tipo Paquete", fontParagraph, XBrushes.White,
                                  new XRect(40 + offSetX_1 + interLine_X_1, 103, el2_width, el_height), format);

                    tf.DrawString("Descripcion Detalle", fontParagraph, XBrushes.White,
                                  new XRect(70 + offSetX_2 + 2 * interLine_X_2, 103, el1_width, el_height), format);


                    tf.DrawString("Precio Unitario", fontParagraph, XBrushes.White,
                                  new XRect(70 + offSetX_3 + 2 * interLine_X_3, 103, el3_width, el_height), format);

                    tf.DrawString("Valor", fontParagraph, XBrushes.White,
              new XRect(95 + offSetX_4 + 2 * interLine_X_4, 103, el4_width, el_height), format);

              
                    // stampo il primo elemento insieme all'header

                        graph.DrawRectangle(rect_style1, marginLeft, dist_Y2 + marginTop, el1_width + 5, rect_height + 5);
                        tf.DrawString(""+item.Cantidad, fontParagraph, XBrushes.Black,
                                      new XRect(50, dist_Y + marginTop, el1_width, el_height + 5), format);


                    //ELEMENT 2 - BIG 380
                    var tipoPaquete = _context.TipoPaquetes.Where(p => p.IdTipoPaquete == item.KeyTipoPaquete).AsNoTracking();
                    foreach (var paquete in tipoPaquete)
                    {
                        graph.DrawRectangle(rect_style1, 40 + offSetX_1 + interLine_X_1, dist_Y2 + marginTop, el1_width + 20, rect_height + 5);
                        tf.DrawString(
                          paquete.DesTipoPaquete,
                            fontParagraph,
                            XBrushes.Black,
                            new XRect(40 + offSetX_1 + interLine_X_1, dist_Y + marginTop, el1_width, el_height + 5),
                            format);

                    }
                    //ELEMENT 3 - SMALL 80
                   
                        graph.DrawRectangle(rect_style1, 60 + offSetX_2 + interLine_X_2, dist_Y2 + marginTop, el3_width + 22, rect_height + 5);
                        tf.DrawString(
                           item.DescripcionDetalle,
                            fontParagraph,
                            XBrushes.Black,
                            new XRect(60 + offSetX_2 + 2 * interLine_X_2, dist_Y + marginTop, el3_width, el_height + 5),
                            format);

                        graph.DrawRectangle(rect_style1, 80 + offSetX_3 + interLine_X_3, dist_Y2 + marginTop, el4_width + 10, rect_height + 5);
                        tf.DrawString(
                            ""+item.PrecioUnitario,
                            fontParagraph,
                            XBrushes.Black,
                            new XRect(90 + offSetX_3 + 2 * interLine_X_3, dist_Y + marginTop, el4_width, el_height + 5),
                            format);
                   





                    graph.DrawRectangle(rect_style1, 36 + offSetX_4 + interLine_X_4, dist_Y2 + marginTop, el4_width + 5, rect_height + 5);
                    tf.DrawString(
                        ""+item.PrecioUnitario*item.Cantidad,
                        fontParagraph,
                        XBrushes.Black,
                        new XRect(90 + offSetX_4 + 2 * interLine_X_4, dist_Y + marginTop, el4_width, el_height + 5),
                        format);

                  


              


            }





            MemoryStream stream = new MemoryStream();
            document.Save(stream, false);
            return File(stream, "application/pdf", "Etiqueta.pdf");
        }
    }
}
