using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ser_Etiqueta.Models.DB
{
    public partial class InsertOrdenDetalle
    {
        public int IdOtdetalle { get; set; }
        public int? IdOrdenTrabajo { get; set; }

        public int? idCliente { get; set; }
        public string? Codigo { get; set; }
        public int? Factura { get; set; }
        public int? idMunicipio { get; set; }
        public int? IdTipoPaquete { get; set; }

        public string? direccion { get; set; }
        public int? CantidadBulto { get; set; }
        public double? Peso { get; set; }
        public double? PestoTotal { get; set; }


    }
}
