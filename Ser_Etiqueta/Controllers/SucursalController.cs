using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ser_Etiqueta.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ser_Etiqueta.Controllers
{
    public class SucursalController : Controller
    {
        private readonly SERETIQUETASContext _context;


        public SucursalController(SERETIQUETASContext context)
        {
            this._context = context;
        }
        [HttpPost]
        public object getSucursales(int? idEmpresa)
        {
            var listSucursales = _context.Sucursales.Where(p => p.IdEmpresa == idEmpresa);
            return listSucursales;
        }

        [HttpPost]
        public IActionResult insert(Sucursale sucursal)
        {

            if (ModelState.IsValid)
            {
                _context.Sucursales.Update(sucursal);
                _context.SaveChanges();
                return Json("1");
                //   TempData["Mensaje"] = "El libro se ha editado correctamente";
                //  return Json(ModelState);
            }
            else
            {
                return Json("Ocurrio un Error");
            }
            //  return PartialView("_EmpresaFormPartial", empresa);
        }

        [HttpPost]
        public IActionResult cargarSucursales()
        {

            var p = _context.SP_CRUD_Sucursal.FromSqlInterpolated($"exec [configuration].[SP_CRUD_Sucursales] null,null,null,null,null,null,null,'S',''");
            return Json(p);
        }
        [HttpPost]
        public IActionResult UpdateSucursal(Sucursale sucursal)
        {

            if (ModelState.IsValid)
            {
                _context.Attach(sucursal);
                _context.Entry(sucursal).Property(p => p.NombreSucursal).IsModified = true;
                _context.Entry(sucursal).Property(p => p.IdEmpresa).IsModified = true;
                _context.Entry(sucursal).Property(p => p.IdDepartamento).IsModified = true;
                _context.Entry(sucursal).Property(p => p.IdMunicipio).IsModified = true;
         
                _context.SaveChanges();
                return Json("1");
                //   TempData["Mensaje"] = "El libro se ha editado correctamente";
                //  return Json(ModelState);
            }
            else
            {
                return Json("Ocurrio un Error");
            }
        }

        [HttpPost]
        public IActionResult GetSucursal(int? IdSucursal)
        {
            if (IdSucursal == null || IdSucursal == 0)
            {
                return NotFound();
            }
            //obtener libro
            var sucursal= _context.Sucursales.Find(IdSucursal);
            if (sucursal== null)
            {
                return NotFound();
            }

            return Json(sucursal);
        }

    }
}
