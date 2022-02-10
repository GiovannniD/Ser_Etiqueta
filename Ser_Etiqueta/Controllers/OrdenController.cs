using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using QRCoder;
using Ser_Etiqueta.Areas.Identity.Data;
using Ser_Etiqueta.Models.DB;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ser_Etiqueta.Controllers
{
    public class OrdenController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly SERETIQUETASContext _context;
        private readonly ILogger<ClienteController> _logger;
        private readonly IEmailSender _emailSender;
        IWebHostEnvironment _env;
        public int IdSucursal { get; set; }
        public int IdEmpresa { get; set; }

        public int UltimoConsecutivo { get; set; }

        public string SerieCodigoEtiqueta { get; set; }

        public OrdenController(
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

        private void getInfo()
        {
            var users = _userManager.Users.Where(p => p.Id == _userManager.GetUserId(User)).ToList();
            //  String usuario= _userManager.GetUserName(User);
            
            foreach (ApplicationUser user in users)
            {

                IdEmpresa = user.idEmpresa;
                IdSucursal = user.idSucursal;


            }
            var codigo = _context.Empresas.Where(p => p.IdEmpresa == IdEmpresa).AsNoTracking().ToList() ;

            foreach (var emp in codigo)
            {

                SerieCodigoEtiqueta = emp.SerieCodigoEtiqueta;
                UltimoConsecutivo = Convert.ToInt32(emp.UltimoConsecutivo);



            }

         
        }

        [HttpPost]
        public IActionResult insert(OrdenTrabajo orden)
        {
            
            if (ModelState.IsValid)
            {
                _context.OrdenTrabajos.Add(orden);
                _context.SaveChanges();
                return Json("1");
                //   TempData["Mensaje"] = "El libro se ha editado correctamente";
                //  return Json(ModelState);
            }
            else
            {
                return Json(orden);

            }
            //  return PartialView("_clienteFormPartial", cliente);

        }

        [HttpPost]
        public IActionResult UpdateEstado(OrdenTrabajo orden)
        {

            if (ModelState.IsValid)
            {
                _context.Attach(orden);
                _context.Entry(orden).Property(p => p.estado).IsModified = true;
                _context.SaveChanges();
                return Json("1");
                //   TempData["Mensaje"] = "El libro se ha editado correctamente";
                //  return Json(ModelState);
            }
            else
            {
                return Json(orden);

            }
            //  return PartialView("_clienteFormPartial", cliente);

        }

        public class Detalle
        {

            public int? IdOrdenTrabajo { get; set; }

            public int? idCliente { get; set; }
            public string? Codigo { get; set; }
            public int? Factura { get; set; }
            public int? idMunicipio { get; set; }
            public string? IdTipoPaquete { get; set; }

            public string? direccion { get; set; }
            public int? CantidadBulto { get; set; }
            public string? Peso { get; set; }

            public int KeyOrigen { get; set; }

            public int keyDestino { get; set; }

        }

        [HttpPost]
        public object insertDetalleOrden(Detalle detalle)
        {
           
            string prefijo = "0000";
            string etiqueta = "";
          
            /*  string[] peso = detalle.Peso.ToString().Split(",", StringSplitOptions.RemoveEmptyEntries);
              string[] tipo = detalle.IdTipoPaquete.ToString().Split(",", StringSplitOptions.RemoveEmptyEntries);*/
            string peso = detalle.Peso.ToString();
            string tipo = detalle.IdTipoPaquete.ToString();
            int idOtd = 0;
            getInfo();
           

            for (int i=1;i<=detalle.CantidadBulto;i++)
            {
                OrdenTrabajoDetalle saveDetalle = new OrdenTrabajoDetalle
                {
                    IdOrdenTrabajo=detalle.IdOrdenTrabajo,
                    idCliente=detalle.idCliente,
                    Codigo=detalle.Codigo,
                    Factura=detalle.Factura.ToString(),
                    idMunicipio=detalle.idMunicipio,
                    IdTipoPaquete = Convert.ToInt16(tipo),
                    // IdTipoPaquete= Convert.ToInt16(tipo[i-1]),
                    direccion =detalle.direccion,
                    CantidadBulto=1,
                    Peso = Convert.ToDecimal(peso),
                    KeyOrigen=detalle.KeyOrigen,
                    keyDestino=detalle.keyDestino
                    //  Peso=Convert.ToInt16(peso[i-1])
                };
                _context.OrdenTrabajoDetalles.Add(saveDetalle);
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
                   
                    IdOtdetalle = saveDetalle.IdOtdetalle,
                    CodigoSerie = etiqueta,
                    Serie=i+"/"+ detalle.CantidadBulto,
                    Imagen = BitmapToBytesCode(qrCodeImage)
                   
                };
                _context.OrdenTrabajoCodigos.Add(codigo);
                _context.SaveChanges();
                etiqueta = "";
            }

            Empresa cliente = new Empresa
            {
                IdEmpresa=IdEmpresa,
                UltimoConsecutivo=UltimoConsecutivo


            };
            _context.Attach(cliente);
            _context.Entry(cliente).Property(p => p.UltimoConsecutivo).IsModified = true;
            _context.SaveChanges();





            List<String> numbers = new List<String>();
                 string code = "E"; //Or "C"
                 for (int i = 0; i <= 2000; i++)
                 {
                     numbers.Add(String.Format("{0}{1:000}", code, i));
                 }



                return Json("1");

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
        public IActionResult cargarOrdenes()
        {
          
            if (User.IsInRole("SuperAdmin"))
            {
                var p = _context.SP_CRUD_Ordenes.FromSqlInterpolated($"exec [etiquetas].[SP_CRUD_OrdenTrabajo] null,null,null,null,null,null,null,null,null,'S',''");
                return Json(p);
            }
            else
            {
                getInfo();
                var p = _context.SP_CRUD_Ordenes.FromSqlInterpolated($"exec [etiquetas].[SP_CRUD_OrdenTrabajo] {IdEmpresa},null,null,null,null,null,null,null,null,'S1',''");
                return Json(p);
            }


        }

        [HttpPost]
        public IActionResult Filtro(string fechaInicio,string fechaFinal)
        {

            if (User.IsInRole("SuperAdmin"))
            {
                var p = _context.SP_CRUD_Ordenes.FromSqlInterpolated($"exec [etiquetas].[SP_CRUD_OrdenTrabajo] null,null,null,null,null,null,null,{fechaInicio},{fechaFinal},'S2',''");
                return Json(p);
            }
            else
            {
                getInfo();
                var p = _context.SP_CRUD_Ordenes.FromSqlInterpolated($"exec [etiquetas].[SP_CRUD_OrdenTrabajo] {IdEmpresa},null,null,null,null,null,null,{fechaInicio},{fechaFinal},'S3',''");
                return Json(p);
            }


        }

        [HttpPost]
        public IActionResult cargarOrdenDetalle(int? idOrdenTrabajo)
        {

            if (User.IsInRole("SuperAdmin"))
            {
                var p = _context.SP_CRUD_OrdenDetalle.FromSqlInterpolated($"exec [etiquetas].[SP_CRUD_OrdenTrabajoDetalle] null,{idOrdenTrabajo},null,null,null,'S',''").AsNoTracking().ToList();
                return Json(p);
            }
            else
            {
                getInfo();
                var p = _context.SP_CRUD_OrdenDetalle.FromSqlInterpolated($"exec [etiquetas].[SP_CRUD_OrdenTrabajoDetalle] {IdEmpresa},{idOrdenTrabajo},null,null,null,'S3',''").AsNoTracking();
                return Json(p);
            }


        }

        [HttpGet]
        public IActionResult DownloadExcelRemision(DateTime fecha1, DateTime fecha2)
        {
            if (User.IsInRole("SuperAdmin"))
            {
                var dep = _context.vw_vistaExportOrden.Where(p => p.fechaCreacion >= fecha1 && p.fechaCreacion <= fecha2).ToList();
                var stream = new MemoryStream();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(dep, true);
                    package.Save();
                }
                stream.Position = 0;
                string ExcelName = $"Etiquetas-{DateTime.Now.ToString("yyyymmddHmmssfff")}.xls";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", ExcelName);
            }
            else
            {
                getInfo();
                var dep = _context.vw_vistaExportOrden.Where(p => p.fechaCreacion >= fecha1 && p.fechaCreacion <= fecha2 && p.IdEmpresa==IdEmpresa).ToList();
                var stream = new MemoryStream();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(dep, true);
                    package.Save();
                }
                stream.Position = 0;
                string ExcelName = $"Etiquetas-{DateTime.Now.ToString("yyyymmddHmmssfff")}.xls";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", ExcelName);
            }

          
              
        }

        [HttpPost]
        public IActionResult vw_Envios(DateTime fecha1, DateTime fecha2)
        {
            if (User.IsInRole("SuperAdmin"))
            {
                var dep = _context.vw_envios.Where(p => p.fechaCreacion >= fecha1 && p.fechaCreacion <= fecha2).ToList();
                return Json(dep);
               
            }
            else
            {
                getInfo();
                var dep = _context.vw_envios.Where(p => p.fechaCreacion >= fecha1 && p.fechaCreacion <= fecha2 && p.idEmpresa== IdEmpresa).ToList();
         
                return Json(dep);
            }



        }
        [HttpPost]
        public IActionResult Importar()
        {

            if (User.IsInRole("SuperAdmin"))
            {
                var p = _context.SP_CRUD_OrdenDetalle.FromSqlInterpolated($"exec [etiquetas].[importarEtiquetas] ").AsNoTracking().ToList();
               
            }

            return Json("1");

        }
        [HttpPost]
        public IActionResult eliminarOrden(OrdenTrabajoDetalle detalle)
        {


           var p = _context.OrdenTrabajoDetalles.Remove(detalle);
            _context.SaveChanges();

            var update = _context.updateSerie.FromSqlInterpolated($"exec [etiquetas].[updateSerie] {detalle.IdOtdetalle},''").AsNoTracking().ToList();
            return Json("1");
   

        }

        [HttpPost]
        public IActionResult eliminarOT(OrdenTrabajo Ot)
        {


            var p = _context.OrdenTrabajos.Remove(Ot);
            _context.SaveChanges();

            return Json("1");


        }
        public IActionResult imprimirEnvio(int id)
        {
            int idOtDetalle = 0;
          //  int idOT = 0;
           // getInfo();
            
           
            var codigo = _context.OrdenTrabajoCodigos.Where(p=>p.IdOtcodigo==id);
          
            int idep = 0;
            PdfDocument document = new PdfDocument();
            XFont font = new XFont("Arial", 8);
            XFont font2 = new XFont("Arial", 8,XFontStyle.Bold);
            XFont font3= new XFont("Arial", 7,XFontStyle.Bold);
            XFont font4 = new XFont("Arial", 8);
            XFont font5 = new XFont("Arial", 8, XFontStyle.Bold);


            PdfPage page = document.AddPage();
            page.Width = XUnit.FromMillimeter(101.6);
            page.Height = XUnit.FromMillimeter(101.6);
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            // Text format
            XStringFormat format = new XStringFormat();
            format.LineAlignment = XLineAlignment.Near;
            format.Alignment = XStringAlignment.Near;
            XGraphics gfx = XGraphics.FromPdfPage(page);
            var tf = new XTextFormatter(gfx);

           
            XImage xfoto = XImage.FromFile(_env.WebRootPath + @"\logo_SER.jpg");
            gfx.DrawImage(xfoto, 5, 0, 120, 50);
            
            byte[] Imagen = null;
            foreach (var item in codigo) {

                

                idOtDetalle = Convert.ToInt16(item.IdOtcodigo);
                Stream stream2 = new System.IO.MemoryStream(item.Imagen);
            XImage xfoto2 = XImage.FromStream(stream2);
               


                gfx.DrawImage(xfoto2, 130, 165, 130, 130);
                gfx.DrawString(item.CodigoSerie, font3, XBrushes.Black, new XPoint(180, 285));

            }
            var detalle = _context.SP_CRUD_OrdenDetalle.FromSqlInterpolated($"exec [etiquetas].[SP_CRUD_OrdenTrabajoDetalle] {idOtDetalle},null,null,null,null,'S2',''").AsNoTracking().ToList();
            foreach (var item in detalle)
            {
                var OT = _context.OrdenTrabajos.Where(p => p.IdOrdenTrabajo == item.IdOrdenTrabajo).AsNoTracking(); 
                foreach(var Orden in OT)
                {
                    var logo = _context.LogoEmpresas.Where(p => p.IdEmpresa == Orden.IdEmpresa).AsNoTracking();
                    if (logo != null) { 
                    foreach (var logos in logo)
                    {
                        Stream stream2 = new System.IO.MemoryStream(logos.logoEmpresa);
                        XImage xfoto2 = XImage.FromStream(stream2);
                        gfx.DrawImage(xfoto2, 145, 0, 120, 50);
                    }
                    }
                }
                
                XSolidBrush rect_style1 = new XSolidBrush(XColors.LightGray);
                // DateTime date = DateTime.ParseExact("", "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //string formattedDate = date.ToString("dd/MM/yyyy");
                gfx.DrawString("Factura:    "+item.Factura, font2, XBrushes.Black, new XPoint(5, 70));
                gfx.DrawString("Fecha: " + item.fechaRegistro.ToShortDateString(), font2, XBrushes.Black, new XPoint(200, 70));
                gfx.DrawString("Cliente:" , font2, XBrushes.Black, new XPoint(5, 90));
                gfx.DrawString(""+item.nombreComercial, font, XBrushes.Black, new XPoint(50, 90));
                   gfx.DrawString("Direccion:", font2, XBrushes.Black, new XPoint(5, 110));


                XRect rect = new XRect(50, 100, 230, 50);
                gfx.DrawRectangle(XBrushes.White, rect);
                tf.DrawString(""+item.direccion,
                font,
               XBrushes.Black,
                new XRect(rect.X + 5, rect.Y, rect.Width - 5, 34), format);
                //gfx.DrawString(item.direccion, font, XBrushes.Black, new XPoint(60, 110));
                gfx.DrawString("Telefono:", font2, XBrushes.Black, new XPoint(5, 135));
                gfx.DrawString(""+item.Telefono, font, XBrushes.Black, new XPoint(60, 135));
                gfx.DrawString("Movil:", font2, XBrushes.Black, new XPoint(160, 135));
                gfx.DrawString(""+item.Movil, font, XBrushes.Black, new XPoint(205, 135));
                var mun = _context.Municipios.Where(p => p.KeyMunicipio == item.idMunicipio).AsNoTracking();
                foreach (var municipios in mun)
                {
                    idep = municipios.KeyDepartamento;
                    gfx.DrawString(municipios.DescripcionMun, font2, XBrushes.Black, new XPoint(160, 175));
                }
                    var dep = _context.Departamentos.Where(p=>p.KeyDepartamento==idep).AsNoTracking();
                foreach (var departamentos in dep)
                {
                    gfx.DrawString(departamentos.DescripcionDep, font2, XBrushes.Black, new XPoint(5, 175));
                }
                   
                gfx.DrawString("Departamento:", font2, XBrushes.Black, new XPoint(5, 155));
                
                gfx.DrawString("Municipio:", font2, XBrushes.Black, new XPoint(160, 155));
                gfx.DrawString("Paquete:", font2, XBrushes.Black, new XPoint(5, 200));
                var tipo= _context.TipoPaquetes.Where(p=>p.IdTipoPaquete==item.IdTipoPaquete).AsNoTracking();
                foreach (var tipos in tipo)
                {
                    gfx.DrawString(tipos.DesTipoPaquete, font, XBrushes.Black, new XPoint(60, 200));
                }
                gfx.DrawString("Bultos:", font2, XBrushes.Black, new XPoint(5, 220));
                gfx.DrawString(item.serie, font, XBrushes.Black, new XPoint(60, 220));
                gfx.DrawString("Peso:", font2, XBrushes.Black, new XPoint(5, 240));
                gfx.DrawString(item.Peso+" kg", font, XBrushes.Black, new XPoint(60, 240));

            }
            // MemoryStream strm = new MemoryStream(jpegdata);
            // Image img = Image.FromFile(_env.WebRootPath + @"\Logo.png");
            //  img.Save(strm, System.Drawing.Imaging.ImageFormat.Png);

            //   XImage xfoto = XImage.FromStream(strm);
            // XImage xfoto = XImage.FromFile(_env.WebRootPath + @"\Logo.png");
            //   gfx.DrawImage(xfoto, 10, 10, 70, 70);

            // gfx.DrawString("Mi primera etiqueta",font,XBrushes.Black,new XPoint(10,90));
            // gfx.DrawString("first line of code", font, XBrushes.Black, new XPoint(10, 20));
            MemoryStream stream = new MemoryStream();
            document.Save(stream, false);
            return File(stream, "application/pdf", "Etiqueta.pdf");

        }
        public IActionResult imprimirAllEnvio(int id)
        {
            int idOtDetalle = 0;
            var codigo = _context.SP_CRUD_OrdenDetalle.FromSqlInterpolated($"exec [etiquetas].[SP_CRUD_OrdenTrabajoDetalle] null,{id},null,null,null,'S3',''").AsNoTracking().ToList();
            var count = _context.OrdenTrabajoDetalles
             .Where(o => o.IdOrdenTrabajo == id)
             .Count();
            int i = 0;
            int idep = 0;
            PdfDocument document = new PdfDocument();
            XFont font = new XFont("Arial", 8);
            XFont font2 = new XFont("Arial", 8, XFontStyle.Bold);
            XFont font3 = new XFont("Arial", 7, XFontStyle.Bold);
            XFont font4 = new XFont("Arial", 8);
            XFont font5 = new XFont("Arial", 8, XFontStyle.Bold);

            XRect rect = new XRect(50, 100, 230, 50);
            XStringFormat format = new XStringFormat();
            PdfPage page = document.AddPage();
            page.Width = XUnit.FromMillimeter(101.6);
            page.Height = XUnit.FromMillimeter(101.6);
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            
            XGraphics gfx = XGraphics.FromPdfPage(page);


            byte[] Imagen = null;
            foreach (var item in codigo)
            {
                var tf = new XTextFormatter(gfx);
                XImage xfoto = XImage.FromFile(_env.WebRootPath + @"\logo_SER.jpg");
                gfx.DrawImage(xfoto, 5, 0, 120, 50);
                //     idOtDetalle = Convert.ToInt16(item.IdOtcodigo);
                Stream stream2 = new System.IO.MemoryStream(item.Imagen);
                XImage xfoto2 = XImage.FromStream(stream2);

                var OT = _context.OrdenTrabajos.Where(p => p.IdOrdenTrabajo == item.IdOrdenTrabajo).AsNoTracking();
                foreach (var Orden in OT)
                {
                    var logo = _context.LogoEmpresas.Where(p => p.IdEmpresa == Orden.IdEmpresa).AsNoTracking();
                    if (logo != null)
                    {
                        foreach (var logos in logo)
                        {
                            Stream stream3 = new System.IO.MemoryStream(logos.logoEmpresa);
                            XImage xfoto3 = XImage.FromStream(stream3);
                            gfx.DrawImage(xfoto3, 145, 0, 120, 50);
                        }
                    }
                }

                gfx.DrawImage(xfoto2, 130, 165, 130, 130);
                gfx.DrawString(item.CodigoSerie, font3, XBrushes.Black, new XPoint(180, 285));
                // DateTime date = DateTime.ParseExact("", "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //string formattedDate = date.ToString("dd/MM/yyyy");
                gfx.DrawString("Factura:    " + item.Factura, font2, XBrushes.Black, new XPoint(5, 70));
                gfx.DrawString("Fecha: " + item.fechaRegistro.ToShortDateString(), font2, XBrushes.Black, new XPoint(200, 70));
                gfx.DrawString("Cliente:", font2, XBrushes.Black, new XPoint(5, 90));
                gfx.DrawString(""+ item.nombreComercial, font, XBrushes.Black, new XPoint(60, 90));
                gfx.DrawString("Direccion:", font2, XBrushes.Black, new XPoint(5, 110));
               
                gfx.DrawRectangle(XBrushes.White, rect);
              /*  tf.DrawString("" + item.direccion,
                font,
               XBrushes.Black,
                rect, format);*/
                tf.DrawString(""+item.direccion, font, XBrushes.Black, rect,format);
                gfx.DrawString("Telefono:", font2, XBrushes.Black, new XPoint(5, 135));
                gfx.DrawString(""+item.Telefono, font, XBrushes.Black, new XPoint(60, 135));
                gfx.DrawString("Movil:", font2, XBrushes.Black, new XPoint(160, 135));
                gfx.DrawString(""+item.Movil, font, XBrushes.Black, new XPoint(205, 135));
                var mun = _context.Municipios.Where(p => p.KeyMunicipio == item.idMunicipio).AsNoTracking();
                foreach (var municipios in mun)
                {
                    idep = municipios.KeyDepartamento;
                    gfx.DrawString(municipios.DescripcionMun, font2, XBrushes.Black, new XPoint(160, 175));
                }
                var dep = _context.Departamentos.Where(p => p.KeyDepartamento == idep).AsNoTracking();
                foreach (var departamentos in dep)
                {
                    gfx.DrawString(departamentos.DescripcionDep, font2, XBrushes.Black, new XPoint(5, 175));
                }

                gfx.DrawString("Departamento:", font2, XBrushes.Black, new XPoint(5, 155));

                gfx.DrawString("Municipio:", font2, XBrushes.Black, new XPoint(160, 155));
                gfx.DrawString("Paquete:", font2, XBrushes.Black, new XPoint(5, 200));
                var tipo = _context.TipoPaquetes.Where(p => p.IdTipoPaquete == item.IdTipoPaquete).AsNoTracking();
                foreach (var tipos in tipo)
                {
                    gfx.DrawString(tipos.DesTipoPaquete, font, XBrushes.Black, new XPoint(60, 200));
                }
                gfx.DrawString("Bultos:", font2, XBrushes.Black, new XPoint(5, 220));
                gfx.DrawString(item.serie, font, XBrushes.Black, new XPoint(60, 220));
                gfx.DrawString("Peso:", font2, XBrushes.Black, new XPoint(5, 240));
                gfx.DrawString(item.Peso + " kg", font, XBrushes.Black, new XPoint(60, 240));
                i++;
                if (i < count) { 
                page = document.AddPage();
                page.Width = XUnit.FromMillimeter(101.6);
                page.Height = XUnit.FromMillimeter(101.6);
                gfx = XGraphics.FromPdfPage(page);
                }
              
            }
          
            // MemoryStream strm = new MemoryStream(jpegdata);
            // Image img = Image.FromFile(_env.WebRootPath + @"\Logo.png");
            //  img.Save(strm, System.Drawing.Imaging.ImageFormat.Png);

            //   XImage xfoto = XImage.FromStream(strm);
            // XImage xfoto = XImage.FromFile(_env.WebRootPath + @"\Logo.png");
            //   gfx.DrawImage(xfoto, 10, 10, 70, 70);

            // gfx.DrawString("Mi primera etiqueta",font,XBrushes.Black,new XPoint(10,90));
            // gfx.DrawString("first line of code", font, XBrushes.Black, new XPoint(10, 20));
            MemoryStream stream = new MemoryStream();
            document.Save(stream, false);
            return File(stream, "application/pdf", "Etiqueta.pdf");

        }
        [HttpGet]
        public IActionResult OrdenDetalle(int? id)
        {
            if (id == null || id == 0) {
                return NotFound();
            }
            getInfo();
            var idSersa = _context.Empresas.Where(p=> p.IdEmpresa==IdEmpresa).AsNoTracking();
            foreach (var item in idSersa)
            {
                ViewData["idSersa"] = item.IdSersa;
            }
            var estado = _context.OrdenTrabajos.Where(p => p.IdOrdenTrabajo == id).AsNoTracking().ToList();

            foreach (var item in estado)
            {

                ViewData["estado"] = item.estado;

            }
            UsuariosInfo rec = new UsuariosInfo
            {
                idEmpresa = IdEmpresa,
                idSucursal = IdSucursal

            };
            ViewData["orden"] = id;
            ViewData["serieCodigo"] = SerieCodigoEtiqueta;
            ViewData["consecutivo"] = UltimoConsecutivo;
            if (User.IsInRole("SuperAdmin"))
            {
               
                return View("OrdenDetalle", rec);
            }
               
            var ordenes = _context.OrdenTrabajos
    .Where(c => c.IdEmpresa == IdEmpresa && c.IdOrdenTrabajo==id)
    .ToList();
            foreach (var item in ordenes)
            {
                var permiso = item.IdEmpresa;
                if (permiso != null) { 
              
                return View("OrdenDetalle", rec);
                }
                
                    
         
            }

            return RedirectToAction("Index");

        }
    }
}
