using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ser_Etiqueta.Models.DB;
using Microsoft.EntityFrameworkCore;
using Ser_Etiqueta.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Ser_Etiqueta.Controllers
{
    [Authorize]
    [Authorize(Roles = "SuperAdmin")]
    public class EmpresaController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly SERETIQUETASContext _context;


        public EmpresaController(SERETIQUETASContext context)
        {
            this._context = context;
        }


        public IActionResult Index()
        {
           
            return View();
        }

        [HttpGet]
        public object getDepartamento()
        {

            IEnumerable<Departamento> listDepartamento = _context.Departamentos;
            return listDepartamento;
        }

        [HttpPost]
        public object getMunicipio(int keyDepto)
        {
            int key = 0;
            key = keyDepto;
            if (keyDepto == null || keyDepto == 0)
            {
                key=1;
            }

            //   IEnumerable<Municipio> listMunnicipio = _context.Municipios;
            var listMunicipio = _context.Municipios.Where(p=>p.KeyDepartamento==key);
            return listMunicipio;
        }

        [HttpPost]
        public IActionResult insert(Empresa empresa)
        {

            if (ModelState.IsValid)
            {
                _context.Empresas.Update(empresa);
                _context.SaveChanges();
                return Json("1");
                //   TempData["Mensaje"] = "El libro se ha editado correctamente";
                //  return Json(ModelState);
            }
            else
            {
                return PartialView("_EmpresaFormPartial", empresa);
            }
          //  return PartialView("_EmpresaFormPartial", empresa);
        }
        [HttpPost]
        public IActionResult UpdateEmpresa(Empresa empresa)
        {

            if (ModelState.IsValid)
            {

                _context.Attach(empresa);
                _context.Entry(empresa).Property(p => p.NombreComercial).IsModified = true;
                _context.Entry(empresa).Property(p => p.NombreEmpresa).IsModified = true;
                _context.Entry(empresa).Property(p => p.SerieCodigoEtiqueta).IsModified = true;
                _context.Entry(empresa).Property(p => p.IdDepartamento).IsModified = true;
                _context.Entry(empresa).Property(p => p.IdMunicipio).IsModified = true;
                _context.Entry(empresa).Property(p => p.IdSersa).IsModified = true;
                _context.Entry(empresa).Property(p => p.TieneSucursal).IsModified = true;
                _context.SaveChanges();

                return Json("1");
                //   TempData["Mensaje"] = "El libro se ha editado correctamente";
                //  return Json(ModelState);
            }
            else
            {
                return PartialView("_EmpresaFormPartial", empresa);
            }
            //  return PartialView("_EmpresaFormPartial", empresa);
        }
        
        [HttpPost]
        public IActionResult cargarEmpresas()
        {

            var p = _context.SP_CRUD_EMPRESAS.FromSqlInterpolated($"exec [configuration].[SP_CRUD_Empresas] null,null,null,null,null,null,null,null,null,null,'S',''");
            return Json(p);
        }

            public IActionResult Form()
        {
            var model = new Empresa { };

            return PartialView("_EmpresaFormPartial", model);
        }

        [HttpPost]
        public IActionResult GetEmpresa(int? IdEmpresa)
        {
            if (IdEmpresa == null || IdEmpresa == 0)
            {
                return NotFound();
            }
            //obtener libro
            var empresa = _context.Empresas.Find(IdEmpresa);
            if (empresa == null)
            {
                return NotFound();
            }

            return Json(empresa);
        }

    }
}
