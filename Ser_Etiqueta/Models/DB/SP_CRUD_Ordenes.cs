using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ser_Etiqueta.Models.DB
{
    public class SP_CRUD_Ordenes
    {
        public int IdOrdenTrabajo { get; set; }
        public int? IdEmpresa { get; set; }

        public string? nombreComercial { get; set; }
        public DateTime? FechaCreacion { get; set; }

        public int? estado { get; set; }
        public bool? Generada { get; set; }

        public string? estadoDes { get; set; }
    }
}
