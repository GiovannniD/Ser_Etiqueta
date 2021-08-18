using System;
using System.Collections.Generic;

#nullable disable

namespace Ser_Etiqueta.Models.DB
{
    public partial class OrdenTrabajoCodigo
    {
        public int IdOtcodigo { get; set; }
        public int? IdOtdetalle { get; set; }
        public string CodigoSerie { get; set; }
        public string Serie { get; set; }
        public byte[] Imagen { get; set; }

    //   public virtual OrdenTrabajoDetalle IdOtdetalleNavigation { get; set; }
    }
}
