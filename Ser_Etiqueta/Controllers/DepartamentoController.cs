using Microsoft.AspNetCore.Mvc;
using Ser_Etiqueta.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ser_Etiqueta.Controllers 
{
   
    public class DepartamentoController : Controller
    {
        private readonly SERETIQUETASContext _context;
        public DepartamentoController(SERETIQUETASContext context)
        {
            this._context = context;
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
                key = 1;
            }

            //   IEnumerable<Municipio> listMunnicipio = _context.Municipios;
            var listMunicipio = _context.Municipios.Where(p => p.KeyDepartamento == key);
            return listMunicipio;
        }


        public object getAllMunicipio()
        {


            //   IEnumerable<Municipio> listMunnicipio = _context.Municipios;
            var listMunicipio = _context.Municipios.ToList();
            return listMunicipio;
        }
    }
}
