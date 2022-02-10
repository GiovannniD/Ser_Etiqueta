using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Ser_Etiqueta.Models.DB
{
    public partial class Factura
    {
       

        public int KeyFactura { get; set; }
        public int? KeyCliente { get; set; }
        public int? IdEmpresa { get; set; }
        public int? IdSucursal { get; set; }
        public int KeyTipoFactura { get; set; } = 1;
        public int KeyFacturaEstatus { get; set; }
        public int NoFactura { get; set; }
        public decimal Subtotal { get; set; }
        public decimal IVA { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaElaboracion { get; set; }
        public string UserName { get; set; }

        public DateTime CreateDate { get; set; }

        public int IsGeneraOT { get; set; }

        public int idSersa { get; set; }
        public string Prefijo { get; set; }
        public int? idOrdentrabajo { get; set; }



        //  public virtual ICollection<OrdenTrabajo> OrdenTrabajos { get; set; }
    }
}
