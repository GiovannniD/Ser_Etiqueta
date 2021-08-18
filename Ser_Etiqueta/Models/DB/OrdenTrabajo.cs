using System;
using System.Collections.Generic;

#nullable disable

namespace Ser_Etiqueta.Models.DB
{
    public partial class OrdenTrabajo
    {
        public OrdenTrabajo()
        {
        //    OrdenTrabajoDetalles = new HashSet<OrdenTrabajoDetalle>();
        }

        public int IdOrdenTrabajo { get; set; }
        public int? IdEmpresa { get; set; }
        public DateTime? FechaCreacion { get; set; }

        public int? estado { get; set; }
        public bool? Generada { get; set; }

      // public virtual Cliente IdClienteNavigation { get; set; }
       // public virtual ClientesDireccion IdDireccionClienteNavigation { get; set; }
        //public virtual Ordene IdOrdenesNavigation { get; set; }
       //public virtual ICollection<OrdenTrabajoDetalle> OrdenTrabajoDetalles { get; set; }
    }
}
