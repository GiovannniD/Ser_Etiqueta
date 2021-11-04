using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Ser_Etiqueta.Models.DB
{
    public partial class vw_Facturas
    {
       

        public int KeyFactura { get; set; }

        public string nombreComercial { get; set; }
        public int KeyFacturaEstatus { get; set; }
        public string NoFactura { get; set; }

        public DateTime FechaElaboracion { get; set; }

        public string UserName { get; set; }



      
      //  public virtual ICollection<OrdenTrabajo> OrdenTrabajos { get; set; }
    }
}
