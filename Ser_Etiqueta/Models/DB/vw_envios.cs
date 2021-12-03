using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Ser_Etiqueta.Models.DB
{
    public partial class vw_envios
    {


        public int n_registro { get; set; }
        public string Codigo { get; set; }

        public string Factura { get; set; }

        public string Paquete { get; set; }

        public string  codigoCliente { get; set; }
        public string nombreComercial { get; set; }
        public string Direccion { get; set; }
        public string Contacto { get; set; }
        public string Municipio { get; set; }
        public decimal peso { get; set; }
        public int cantidadBulto { get; set; }
        public DateTime fechaCreacion { get; set; }

        public int idEmpresa { get; set; }



    }
}
