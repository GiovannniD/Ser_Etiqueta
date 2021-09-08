using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
//using SelectPdf;
using Ser_Etiqueta.Models.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
            var ordenes = _context.OrdenTrabajoDetalles.Where(p => p.IdOrdenTrabajo == id);
            OrdenTrabajoDetalle model = new OrdenTrabajoDetalle();
           

            PdfDocument document = new PdfDocument();
            document.Info.Title = "Table Example";


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

            // Row elements
            int el1_width = 80;
                int el2_width = 80;
            int el3_width = 120;
            int el4_width = 80;
            int el5_width = 150;

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


            int offSetX_1 = el1_width;
            int offSetX_2 = el1_width + el2_width;
            int offSetX_3 = el1_width + el2_width+el3_width;
            int offSetX_4 = el1_width + el2_width + el3_width + el4_width;
            int offSetX_5 = el1_width + el2_width + el3_width + el4_width + el5_width;


            XSolidBrush rect_style1 = new XSolidBrush(XColors.LightGray);
                XSolidBrush rect_style2 = new XSolidBrush(XColors.DarkGreen);
                XSolidBrush rect_style3 = new XSolidBrush(XColors.Red);
                int i = -1;
                int a = 500;
                XImage xfoto = XImage.FromFile(_env.WebRootPath + @"\logo_SER.jpg");
                graph.DrawImage(xfoto, 12, 10, 120, 50);
                graph.DrawString("Remision: #00001" , font2, XBrushes.Black, new XPoint(24, 92));
            foreach (var item in ordenes)
                {
                  //  model.Codigo = item.Codigo;
                    i++;
                    a++;
                double dist_Y = lineHeight * (i + 1);
                    double dist_Y2 = dist_Y - 2;
                    a.ToString().PadLeft(8, '0');
a.ToString("00000000");
a.ToString("D4");
                 
                    // header della G
                    if (i == 0)
                    {
                        graph.DrawRectangle(rect_style2, marginLeft, marginTop, pdfPage.Width - 2 * marginLeft, rect_height);

                        tf.DrawString("Municipio", fontParagraph, XBrushes.White,
                                      new XRect(marginLeft, marginTop, el1_width, el_height), format);

                        tf.DrawString("Cliente", fontParagraph, XBrushes.White,
                                      new XRect(marginLeft + offSetX_1 + interLine_X_1, marginTop, el2_width, el_height), format);

                        tf.DrawString("Nombre Comercial", fontParagraph, XBrushes.White,
                                      new XRect(marginLeft + offSetX_2 + 2 * interLine_X_2, marginTop, el1_width, el_height), format);


                    tf.DrawString("Nombre del Cliente", fontParagraph, XBrushes.White,
                                  new XRect(marginLeft + offSetX_3 + 2 * interLine_X_3, marginTop, el3_width, el_height), format);

                    tf.DrawString("Factura", fontParagraph, XBrushes.White,
              new XRect(marginLeft + offSetX_4 + 2 * interLine_X_4, marginTop, el1_width, el_height), format);

                    tf.DrawString("Direccion", fontParagraph, XBrushes.White,
                new XRect(marginLeft + offSetX_5 + 2 * interLine_X_5, marginTop, el5_width, el_height), format);

                    // stampo il primo elemento insieme all'header
                    graph.DrawRectangle(rect_style1, marginLeft, dist_Y2 + marginTop, el1_width, rect_height);
                        tf.DrawString("" + item.Factura, fontParagraph, XBrushes.Black,
                                      new XRect(marginLeft, dist_Y + marginTop, el1_width, el_height), format);

                        //ELEMENT 2 - BIG 380
                        graph.DrawRectangle(rect_style1, marginLeft + offSetX_1 + interLine_X_1, dist_Y2 + marginTop, el1_width, rect_height);
                        tf.DrawString(
                            i.ToString(),
                            fontParagraph,
                            XBrushes.Black,
                            new XRect(marginLeft + offSetX_1 + interLine_X_1, dist_Y + marginTop, el1_width, el_height),
                            format);


                        //ELEMENT 3 - SMALL 80

                        graph.DrawRectangle(rect_style1, marginLeft + offSetX_2 + interLine_X_2, dist_Y2 + marginTop, el3_width, rect_height);
                        tf.DrawString(
                            $"{a:00000000}",
                            fontParagraph,
                            XBrushes.Black,
                            new XRect(marginLeft + offSetX_2 + 2 * interLine_X_2, dist_Y + marginTop, el3_width, el_height),
                            format);

                    graph.DrawRectangle(rect_style1, marginLeft + offSetX_3 + interLine_X_3, dist_Y2 + marginTop, el4_width, rect_height);
                    tf.DrawString(
                        $"{a:00000000}",
                        fontParagraph,
                        XBrushes.Black,
                        new XRect(marginLeft + offSetX_3 + 2 * interLine_X_3, dist_Y + marginTop, el4_width, el_height),
                        format);


                }
                    else
                    {

                        //if (i % 2 == 1)
                        //{
                        //  graph.DrawRectangle(TextBackgroundBrush, marginLeft, lineY - 2 + marginTop, pdfPage.Width - marginLeft - marginRight, lineHeight - 2);
                        //}

                        //ELEMENT 1 - SMALL 80
                        graph.DrawRectangle(rect_style1, marginLeft, marginTop + dist_Y2, el1_width, rect_height);
                        tf.DrawString(

                            ""+item.Factura,
                            fontParagraph,
                            XBrushes.Black,
                            new XRect(marginLeft, marginTop + dist_Y, el1_width, el_height),
                            format);

                        //ELEMENT 2 - BIG 380
                        graph.DrawRectangle(rect_style1, marginLeft + offSetX_1 + interLine_X_1, dist_Y2 + marginTop, el2_width, rect_height);
                        tf.DrawString(
                           item.Codigo,
                            fontParagraph,
                            XBrushes.Black,
                            new XRect(marginLeft + offSetX_1 + interLine_X_1, marginTop + dist_Y, el2_width, el_height),
                            format);


                    //ELEMENT 3 - SMALL 80

                    graph.DrawRectangle(rect_style1, marginLeft + offSetX_2 + interLine_X_2, dist_Y2 + marginTop, el3_width, rect_height);
                    tf.DrawString(
                        $"{a:00000000}",
                        fontParagraph,
                        XBrushes.Black,
                        new XRect(marginLeft + offSetX_2 + 2 * interLine_X_2, dist_Y + marginTop, el3_width, el_height),
                        format);

                }

                }





            MemoryStream stream = new MemoryStream();
            document.Save(stream, false);
            return File(stream, "application/pdf", "Etiqueta.pdf");
        }
    }
}
