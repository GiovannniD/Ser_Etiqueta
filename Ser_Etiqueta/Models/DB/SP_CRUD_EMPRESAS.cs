using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ser_Etiqueta.Models.DB
{
    public class SP_CRUD_EMPRESAS
    {
        public int IdEmpresa { get; set; }

        public int IdSersa { get; set; }

        public string NombreEmpresa { get; set; }

        public string NombreComercial { get; set; }
        public DateTime FechaIngreso { get; set; }
        public bool TieneSucursal { get; set; }
        public int? IdDepartamento { get; set; }
        public int? IdMunicipio { get; set; }

     
        public string SerieCodigoEtiqueta { get; set; }
        public bool? Activo { get; set; }

  
        public int? UltimoConsecutivo { get; set; }

        public string DescripcionDep { get; set; }

        public string DescripcionMun { get; set; }
    }
}
