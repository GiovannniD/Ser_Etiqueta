using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ser_Etiqueta.Models.DB
{
    public class SP_CRUD_Clientes
    {
        public int IdCliente { get; set; }
        public int? IdEmpresa { get; set; }
        public int? IdSucursal { get; set; }

     
        public string Codigo { get; set; }


        public string NombreCliente { get; set; }

        public string NombreComercial { get; set; }
        public string Contacto { get; set; }
        public string Cargo { get; set; }
        public string Email { get; set; }


        public string Telefono { get; set; }

        public string Movil { get; set; }

        public string Direccion { get; set; }
        public int KeyDepartamento { get; set; }
        public int KeyMunicipio { get; set; }
        public bool? Activo { get; set; }

        public string DescripcionDep { get; set; }
        public string DescripcionMun { get; set; }

        
    }
}
