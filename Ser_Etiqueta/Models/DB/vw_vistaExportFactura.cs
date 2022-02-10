using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Ser_Etiqueta.Models.DB
{
    public partial class vw_vistaExportFactura
    {

        public int n_registro { get; set; }
        public string NoFactura { get; set; }
        public DateTime FechaElaboracion { get; set; }
        public string NombreCliente { get; set; }
        public string TipoPaquete { get; set; }
        public string DescripcionDetalle { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal IVA { get; set; }
        public decimal Valor { get; set; }
        public string UserName { get; set; }
        public string Estado { get; set; }







        //  public virtual ICollection<OrdenTrabajo> OrdenTrabajos { get; set; }
    }
}
