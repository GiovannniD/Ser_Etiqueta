using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

#nullable disable

namespace Ser_Etiqueta.Models.DB
{
    public partial class OrdenTrabajoDetalle
    {
       public OrdenTrabajoDetalle()
        {
        //    OrdenTrabajoCodigos = new HashSet<OrdenTrabajoCodigo>();
        }

        public int IdOtdetalle { get; set; }
        public int? IdOrdenTrabajo { get; set; }

        public int? idCliente { get; set; }
        public string? Codigo { get; set; }
        public int? Factura { get; set; }
        public int? idMunicipio{ get; set; }
        public int? IdTipoPaquete { get; set; }

        public string? direccion { get; set; }
        public int? CantidadBulto { get; set; }
        public double? Peso { get; set; }
        public double? PestoTotal { get; set; }

        public DateTime fechaRegistro { get; set; }


      // public virtual OrdenTrabajo IdOrdenTrabajoNavigation { get; set; }

    
       // public virtual TipoPaquete IdTipoPaqueteNavigation { get; set; }
   
      //  public virtual ICollection<OrdenTrabajoCodigo> OrdenTrabajoCodigos { get; set; }


    }
}
