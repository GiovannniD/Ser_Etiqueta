using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Ser_Etiqueta.Models.DB
{
    public partial class FacturaDetalle
    {
        internal int keyDestino;

        public int KeyFacturaDetalle { get; set; }
        public int KeyFactura { get; set; }
        public int? KeyTipoPaquete { get; set; }
        public int? Cantidad { get; set; }
        public string DescripcionDetalle { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int? KeyOrigen { get; set; }
        public int? KeyDestino { get; set; }
        public DateTime CreateDate { get; set; }
        public string Destinatario { get; set; }
       


    }
}
