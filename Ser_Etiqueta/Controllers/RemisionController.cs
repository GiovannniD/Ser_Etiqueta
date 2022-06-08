using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PdfSharp.Drawing;
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
using PdfSharp.Drawing.Layout;
//using SelectPdf;


namespace Ser_Etiqueta.Controllers
{
    public class RemisionController : Controller
    {
        private readonly SERETIQUETASContext _context;

        IWebHostEnvironment _env;
        public RemisionController(SERETIQUETASContext context,
            IWebHostEnvironment env)
        {
            this._context = context;
            _env = env;
        }
        [Authorize]
        [HttpGet]
        public IActionResult Remision(int? id)
        {
            int IdEmpresaOrden=0;
            
            int IdSucursalOrden=0;
            string fecha="";
            int countFinal = 0;
            var OrdenTrabajo = _context.OrdenTrabajos.Where(p => p.IdOrdenTrabajo == id).AsNoTracking();
            foreach (var item in OrdenTrabajo)
            {
                IdEmpresaOrden = Convert.ToInt32(item.IdEmpresa);
                IdSucursalOrden = Convert.ToInt32(item.IdSucursal);
                fecha = item.FechaCreacion.ToString();
            }
                var ordenes = _context.OrdenTrabajoDetalles.Where(p => p.IdOrdenTrabajo == id).AsNoTracking();
            var count = _context.OrdenTrabajoDetalles
            .Where(o => o.IdOrdenTrabajo == id).Count();
            OrdenTrabajoDetalle model = new OrdenTrabajoDetalle();
            countFinal = Convert.ToInt32(count)+1;
           

            PdfDocument document = new PdfDocument();
            document.Info.Title = "Remision de envios";


                // Page Options
                PdfPage pdfPage = document.AddPage();

            
            pdfPage.Width = XUnit.FromMillimeter(215.9); 
                pdfPage.Height = XUnit.FromMillimeter(279.4);
            pdfPage.Orientation = PdfSharp.PageOrientation.Landscape;
            pdfPage.Rotate = 0;
            
            // Get an XGraphics object for drawing
            XGraphics graph = XGraphics.FromPdfPage(pdfPage);

            // Text format
            XStringFormat format = new XStringFormat();
                format.LineAlignment = XLineAlignment.Near;
                format.Alignment = XStringAlignment.Near;
                var tf = new XTextFormatter(graph);

                XFont fontParagraph = new XFont("Verdana", 8, XFontStyle.Regular);
                XFont font2 = new XFont("Verdana", 12, XFontStyle.Bold);
            XFont font3 = new XFont("Verdana", 8, XFontStyle.Bold);

            // Row elements
            int el1_width = 80;
                int el2_width = 80;
            int el3_width = 135;
            int el4_width = 80;
            int el5_width = 80;
            int el6_width = 150;

            // page structure options
            double lineHeight = 20;
                int marginLeft = 20;
                int marginTop = 100;

                int el_height = 30;
                int rect_height = 17;

                int interLine_X_1 = 2;
                int interLine_X_2 = 2 * interLine_X_1;
                 int interLine_X_3 = 2 * interLine_X_2;
                int interLine_X_4 = 2 * interLine_X_3;
                int interLine_X_5 = 2 * interLine_X_4;
            int interLine_X_6 = 2 * interLine_X_5;


            int offSetX_1 = el1_width;
            int offSetX_2 = el1_width + el2_width;
            int offSetX_3 = el1_width + el2_width+el3_width;
            int offSetX_4 = el1_width + el2_width + el3_width + el4_width;
            int offSetX_5 = el1_width + el2_width + el3_width + el4_width + el5_width;
            int offSetX_6 = el1_width + el2_width + el3_width + el4_width + el5_width+ el6_width;


            XSolidBrush rect_style1 = new XSolidBrush(XColors.LightGray);
            XSolidBrush rect_style0 = new XSolidBrush(XColors.White);
            XSolidBrush rect_style2 = new XSolidBrush(XColors.DarkGreen);
                XSolidBrush rect_style3 = new XSolidBrush(XColors.Red);
                int i = -1;
                int a = 500;
                int c = 1;
                int total = 0;
                XImage xfoto = XImage.FromFile(_env.WebRootPath + @"\logo_SER.jpg");
                graph.DrawImage(xfoto, 12, 10, 120, 50);

            var logo = _context.LogoEmpresas.Where(p => p.IdEmpresa == IdEmpresaOrden).AsNoTracking();
            if (logo != null)
            {
                foreach (var logos in logo)
                {
                    Stream stream3 = new System.IO.MemoryStream(logos.logoEmpresa);
                    XImage xfoto3 = XImage.FromStream(stream3);
                    graph.DrawImage(xfoto3, 580 , 10, 120, 50);
                }
            }
            /* XPen lineRed = new XPen(XColors.Red, 2);

             graph.DrawLine(lineRed, 100, 100, 50, 50);*/
            double dist_Y = 0;
            double dist_Y2 =0;
            foreach (var item in ordenes)


                {
                  //  model.Codigo = item.Codigo;
                    i++;
                    a++;
                count--;
                dist_Y = lineHeight * (i + 1);
                dist_Y2 = dist_Y - 2;
                a.ToString().PadLeft(8, '0');
a.ToString("00000000");
a.ToString("D4");
                 
                    // header della G
                    if (i == 0)
                    {
                    item.IdOrdenTrabajo.ToString().PadLeft(8, '0');
          
                    graph.DrawString("Remision: #"+ $"{item.IdOrdenTrabajo:00000000}", font2, XBrushes.Black, new XPoint(24, 92));
                    var empresa = _context.Empresas.Where(p => p.IdEmpresa== IdEmpresaOrden).AsNoTracking();
                    foreach (var emp in empresa)
                    {
                        graph.DrawString(emp.NombreComercial, font2, XBrushes.Black, new XPoint(300, 30));
                    }

                    graph.DrawString("Fecha de creacion: "+fecha, fontParagraph, XBrushes.Black, new XPoint(600, 10));
                    graph.DrawRectangle(rect_style2, marginLeft, marginTop, pdfPage.Width - 2 * marginLeft, rect_height);

                        tf.DrawString("Destino", fontParagraph, XBrushes.White,
                                      new XRect(marginLeft, marginTop, el1_width, el_height), format);

                        tf.DrawString("Cliente", fontParagraph, XBrushes.White,
                                      new XRect(marginLeft + offSetX_1 + interLine_X_1, marginTop, el2_width, el_height), format);

                        tf.DrawString("Nombre Comercial", fontParagraph, XBrushes.White,
                                      new XRect(marginLeft + offSetX_2 + 2 * interLine_X_2, marginTop, el1_width, el_height), format);


                    tf.DrawString("Nombre del Cliente", fontParagraph, XBrushes.White,
                                  new XRect(marginLeft + offSetX_3 + 2 * interLine_X_3, marginTop, el3_width, el_height), format);

                    tf.DrawString("Factura", fontParagraph, XBrushes.White,
              new XRect(marginLeft + offSetX_4 + 2 * interLine_X_4, marginTop, el4_width, el_height), format);

                    tf.DrawString("Tipo de paquete", fontParagraph, XBrushes.White,
                new XRect(marginLeft + offSetX_5 + 2 * interLine_X_5-30, marginTop, el5_width, el_height), format);

                    tf.DrawString("Direccion", fontParagraph, XBrushes.White,
          new XRect(marginLeft + offSetX_6 + 2 * interLine_X_6-100, marginTop, el6_width, el_height), format);
                    // stampo il primo elemento insieme all'header
                    var mun = _context.Municipios.Where(p => p.KeyMunicipio == item.idMunicipio).AsNoTracking();
                    foreach (var destino in mun)
                    {
                        graph.DrawRectangle(rect_style1, marginLeft, dist_Y2 + marginTop, el1_width + 5, rect_height + 10);
                        tf.DrawString(destino.DescripcionMun, fontParagraph, XBrushes.Black,
                                      new XRect(marginLeft, dist_Y + marginTop, el1_width, el_height + 10), format);
                    }

                    //ELEMENT 2 - BIG 380
                    graph.DrawRectangle(rect_style1, marginLeft + offSetX_1 + interLine_X_1, dist_Y2 + marginTop, el1_width+5, rect_height + 10);
                        tf.DrawString(
                          item.Codigo,
                            fontParagraph,
                            XBrushes.Black,
                            new XRect(marginLeft + offSetX_1 + interLine_X_1, dist_Y + marginTop, el1_width, el_height + 10),
                            format);


                    //ELEMENT 3 - SMALL 80
                    var nombreCliente = _context.Clientes.Where(p => p.IdCliente == item.idCliente).AsNoTracking();
                    foreach (var cliente in nombreCliente) 
                    {
                        graph.DrawRectangle(rect_style1, marginLeft + offSetX_2 + interLine_X_2, dist_Y2 + marginTop, el3_width + 10, rect_height + 10);
                        tf.DrawString(
                           cliente.NombreComercial,
                            fontParagraph,
                            XBrushes.Black,
                            new XRect(marginLeft + offSetX_2 + 2 * interLine_X_2, dist_Y + marginTop, el3_width, el_height + 10),
                            format);

                        graph.DrawRectangle(rect_style1, marginLeft + offSetX_3 + interLine_X_3, dist_Y2 + marginTop, el4_width + 10, rect_height + 10);
                        tf.DrawString(
                             cliente.NombreCliente,
                            fontParagraph,
                            XBrushes.Black,
                            new XRect(marginLeft + offSetX_3 + 2 * interLine_X_3, dist_Y + marginTop, el4_width, el_height + 10),
                            format);
                    }
                       

                   


                    graph.DrawRectangle(rect_style1, marginLeft + offSetX_4 + interLine_X_4, dist_Y2 + marginTop, el4_width+5, rect_height+10);
                    tf.DrawString(
                        ""+item.Factura,
                        fontParagraph,
                        XBrushes.Black,
                        new XRect(marginLeft + offSetX_4 + 2 * interLine_X_4, dist_Y + marginTop, el4_width, el_height+10),
                        format);

                    var tipoPaquete= _context.TipoPaquetes.Where(p => p.IdTipoPaquete == item.IdTipoPaquete).AsNoTracking();
                    foreach (var paquete in tipoPaquete)
                    {
                        graph.DrawRectangle(rect_style1, marginLeft + offSetX_5 + interLine_X_5 - 11.5, dist_Y2 + marginTop, el5_width, rect_height + 10);
                        tf.DrawString("" +
                           paquete.DesTipoPaquete,
                            fontParagraph,
                            XBrushes.Black,
                            new XRect(marginLeft + offSetX_5 + 1 * interLine_X_5, dist_Y + marginTop, el5_width, el_height),
                            format);
                    }
                    graph.DrawRectangle(rect_style1, marginLeft + offSetX_6 + interLine_X_6 - 115, dist_Y2 + marginTop, el6_width + 49 ,rect_height + 10);
                    tf.DrawString(""+
                       item.direccion,
                        fontParagraph,
                        XBrushes.Black,
                        new XRect(marginLeft + offSetX_6 +2 * interLine_X_6-175, dist_Y + marginTop, el6_width, el_height),
                        format);


                }
                    else
                    {

                    //if (i % 2 == 1)
                    //{
                    //  graph.DrawRectangle(TextBackgroundBrush, marginLeft, lineY - 2 + marginTop, pdfPage.Width - marginLeft - marginRight, lineHeight - 2);
                    //}

                    //ELEMENT 1 - SMALL 80
                    var mun = _context.Municipios.Where(p => p.KeyMunicipio == item.idMunicipio).AsNoTracking();
                    foreach (var destino in mun)
                    {
                        graph.DrawRectangle(rect_style1, marginLeft, dist_Y2 + marginTop, el1_width + 5, rect_height + 10);
                        tf.DrawString("" + destino.DescripcionMun, fontParagraph, XBrushes.Black,
                                      new XRect(marginLeft, dist_Y + marginTop, el1_width, el_height + 10), format);
                    }

                    //ELEMENT 2 - BIG 380
                    graph.DrawRectangle(rect_style1, marginLeft + offSetX_1 + interLine_X_1, dist_Y2 + marginTop, el1_width + 5, rect_height + 10);
                    tf.DrawString(
                     "" + item.Codigo,
                        fontParagraph,
                        XBrushes.Black,
                        new XRect(marginLeft + offSetX_1 + interLine_X_1, dist_Y + marginTop, el1_width, el_height + 10),
                        format);


                    //ELEMENT 3 - SMALL 80
                    var nombreCliente = _context.Clientes.Where(p => p.IdCliente == item.idCliente).AsNoTracking();
                    foreach (var cliente in nombreCliente)
                    {
                        graph.DrawRectangle(rect_style1, marginLeft + offSetX_2 + interLine_X_2, dist_Y2 + marginTop, el3_width + 10, rect_height + 10);
                        tf.DrawString(
                        "" + cliente.NombreComercial,
                            fontParagraph,
                            XBrushes.Black,
                            new XRect(marginLeft + offSetX_2 + 2 * interLine_X_2, dist_Y + marginTop, el3_width, el_height + 10),
                            format);

                        graph.DrawRectangle(rect_style1, marginLeft + offSetX_3 + interLine_X_3, dist_Y2 + marginTop, el4_width + 10, rect_height + 10);
                        tf.DrawString(
                       "" + cliente.NombreCliente,
                            fontParagraph,
                            XBrushes.Black,
                            new XRect(marginLeft + offSetX_3 + 2 * interLine_X_3, dist_Y + marginTop, el4_width, el_height + 10),
                            format);
                    }





                    graph.DrawRectangle(rect_style1, marginLeft + offSetX_4 + interLine_X_4, dist_Y2 + marginTop, el4_width + 5, rect_height + 10);
                    tf.DrawString(
                        "" + item.Factura,
                        fontParagraph,
                        XBrushes.Black,
                        new XRect(marginLeft + offSetX_4 + 2 * interLine_X_4, dist_Y + marginTop, el4_width, el_height + 10),
                        format);

                    var tipoPaquete = _context.TipoPaquetes.Where(p => p.IdTipoPaquete == item.IdTipoPaquete).AsNoTracking();
                    foreach (var paquete in tipoPaquete)
                    {
                        graph.DrawRectangle(rect_style1, marginLeft + offSetX_5 + interLine_X_5 - 11.5, dist_Y2 + marginTop, el5_width, rect_height + 10);
                        tf.DrawString(
                       "" + paquete.DesTipoPaquete,
                            fontParagraph,
                            XBrushes.Black,
                            new XRect(marginLeft + offSetX_5 + 1 * interLine_X_5, dist_Y + marginTop, el5_width, el_height),
                            format);
                    }
                    graph.DrawRectangle(rect_style1, marginLeft + offSetX_6 + interLine_X_6 - 115, dist_Y2 + marginTop, el6_width + 49, rect_height + 10);
                    tf.DrawString(
                      "" + item.direccion,
                        fontParagraph,
                        XBrushes.Black,
                        new XRect(marginLeft + offSetX_6 + 2 * interLine_X_6 - 175, dist_Y + marginTop, el6_width, el_height),
                        format);
                    c++;
                    if (c > 15)
                    {
                        pdfPage = document.AddPage();
                        pdfPage.Width = XUnit.FromMillimeter(215.9);
                        pdfPage.Height = XUnit.FromMillimeter(279.4);
                        pdfPage.Orientation = PdfSharp.PageOrientation.Landscape;
                        pdfPage.Rotate = 0;
                        graph = XGraphics.FromPdfPage(pdfPage);
                        format.LineAlignment = XLineAlignment.Near;
                        format.Alignment = XStringAlignment.Near;
                        tf = new XTextFormatter(graph);
                        xfoto = XImage.FromFile(_env.WebRootPath + @"\logo_SER.jpg");
                        graph.DrawImage(xfoto, 12, 10, 120, 50);
                        var logo2 = _context.LogoEmpresas.Where(p => p.IdEmpresa == IdEmpresaOrden).AsNoTracking();
                        if (logo2 != null)
                        {
                            foreach (var logos in logo)
                            {
                                Stream stream3 = new System.IO.MemoryStream(logos.logoEmpresa);
                                XImage xfoto3 = XImage.FromStream(stream3);
                                graph.DrawImage(xfoto3, 580, 10, 120, 50);
                            }
                        }
                        // graph.DrawString("Remision: #00001", font2, XBrushes.Black, new XPoint(24, 92));
                        c = 1;
                        i = -1;
                    }

                }


            }
            if (countFinal <= 15 && count == 0)
            {
                dist_Y = lineHeight * countFinal;
                dist_Y2 = dist_Y - 2;
                graph.DrawRectangle(rect_style0, marginLeft, dist_Y2 + marginTop + 10, el1_width + 5, rect_height + 10);
                tf.DrawString("Total de Paquetes: " + countFinal, font3, XBrushes.Black,
                              new XRect(marginLeft, dist_Y + marginTop + 10, el1_width + 50, el_height + 10), format);

                XImage xfoto3 = XImage.FromFile(_env.WebRootPath + @"\LineaFirma.png");
                graph.DrawImage(xfoto3, 150, 500, 150, 10);
                graph.DrawString("Entregado por:", font3, XBrushes.Black, new XPoint(180, 520));

                graph.DrawImage(xfoto3, 480, 500, 150, 10);
                graph.DrawString("Recibido por:", font3, XBrushes.Black, new XPoint(510, 520));
            }
            else {
                dist_Y = lineHeight * (c+1);
                dist_Y2 = dist_Y - 2;
                graph.DrawRectangle(rect_style0, marginLeft, dist_Y2 + marginTop + 10, el1_width + 5, rect_height + 10);
                tf.DrawString("Total de Paquetes: " + countFinal, font3, XBrushes.Black,
                              new XRect(marginLeft, dist_Y + marginTop + 10, el1_width + 50, el_height + 10), format);

                XImage xfoto3 = XImage.FromFile(_env.WebRootPath + @"\LineaFirma.png");
                graph.DrawImage(xfoto3, 150, 500, 150, 10);
                graph.DrawString("Entregado por:", font3, XBrushes.Black, new XPoint(180, 520));

                graph.DrawImage(xfoto3, 480, 500, 150, 10);
                graph.DrawString("Recibido por:", font3, XBrushes.Black, new XPoint(510, 520));
            }





            MemoryStream stream = new MemoryStream();
            document.Save(stream, false);
            return File(stream, "application/pdf", "Etiqueta.pdf");
        }
    }
}
