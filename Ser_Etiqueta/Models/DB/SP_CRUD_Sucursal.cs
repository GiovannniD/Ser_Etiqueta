using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ser_Etiqueta.Models.DB
{
    public class SP_CRUD_Sucursal
    {
        public int IdSucursal { get; set; }
        public int? IdEmpresa { get; set; }
        public string NombreSucursal { get; set; }
        public string nombreEmpresa { get; set; }
        public int? IdDepartamento { get; set; }
        public int? IdMunicipio { get; set; }
        public DateTime FechaIngreso { get; set; }
        public bool? Activo { get; set; }

        public string DescripcionDep { get; set; }

        public string DescripcionMun { get; set; }
    }
}
