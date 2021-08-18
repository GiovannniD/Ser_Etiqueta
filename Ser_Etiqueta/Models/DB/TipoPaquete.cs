using System;
using System.Collections.Generic;

#nullable disable

namespace Ser_Etiqueta.Models.DB
{
    public partial class TipoPaquete
    {
        public TipoPaquete()
        {
         //   OrdenTrabajoDetalles = new HashSet<OrdenTrabajoDetalle>();
        }

        public int IdTipoPaquete { get; set; }
        public string DesTipoPaquete { get; set; }
        public int KeyPaquete { get; set; }
        public int KeyClasificacion { get; set; }

      //  public virtual ICollection<OrdenTrabajoDetalle> OrdenTrabajoDetalles { get; set; }
    }
}
