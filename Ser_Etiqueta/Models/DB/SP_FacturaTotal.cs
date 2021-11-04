using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Ser_Etiqueta.Models.DB
{
    public partial class SP_FacturaTotal
    {

        public int KeyFacturaDetalle { get; set; }
        public int KeyFactura { get; set; }
        public double Subtotal { get; set; }
        public double IVA { get; set; }
        public double Total { get; set; }
      

    }
}
