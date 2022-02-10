using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Ser_Etiqueta.Models.DB
{
    public partial class vw_vistaExportOrden
    {


        public int n_registro { get; set; }
        public int? IdEmpresa { get; set; }

        public string Codigo { get; set; }

        public string serie { get; set; }

        public string Factura { get; set; }
        public string nombreComercial { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string estado { get; set; }
        public string nombreSucursal { get; set; }
        public string nombreCliente { get; set; }
        public string NombreComercialCliente { get; set; }
        public string Departamento { get; set; }
        public string Destino { get; set; }
        public string Direccion { get; set; }
        public int cantidad { get; set; }
        public decimal peso { get; set; }


    }
}
