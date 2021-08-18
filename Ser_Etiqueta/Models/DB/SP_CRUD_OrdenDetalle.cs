using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ser_Etiqueta.Models.DB
{
    public class SP_CRUD_OrdenDetalle
    {
        public int IdOtdetalle { get; set; }
        public int? IdOrdenTrabajo { get; set; }

        public int? idCliente { get; set; }

        public string? nombreCliente { get; set; }

        public string? nombreComercial { get; set; }
        public string? Codigo { get; set; }
        public int? Factura { get; set; }
        public int? idMunicipio { get; set; }
        public int? IdTipoPaquete { get; set; }
        public string? desTipoPaquete { get; set; }

        public string? descripcionMun { get; set; }
        public string? direccion { get; set; }
        public int? CantidadBulto { get; set; }
        public double? Peso { get; set; }
        public double? PestoTotal { get; set; }

        public string? serie { get; set; }

        public int? IdOtCodigo { get; set; }

        public DateTime fechaRegistro { get; set; }

        public string? Telefono { get; set; }

        public string? Movil { get; set; }

        public string CodigoSerie { get; set; }
        public byte[] Imagen { get; set; }
    }
}
