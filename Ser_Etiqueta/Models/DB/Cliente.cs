using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Ser_Etiqueta.Models.DB
{
    public partial class Cliente
    {
        public Cliente()
        {
            ClientesDireccions = new HashSet<ClientesDireccion>();
           // OrdenTrabajos = new HashSet<OrdenTrabajo>();
        }

        public int IdCliente { get; set; }
        public int? IdEmpresa { get; set; }
        public int? IdSucursal { get; set; }

        [Required(ErrorMessage = "El codigo Sersa es obligatorio")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "El Nombre del cliente obligatorio")]
        public string NombreCliente { get; set; }

        [Required(ErrorMessage = "El Nombre Comercial es obligatorio")]
        public string NombreComercial { get; set; }
        public string Contacto { get; set; }
        public string Cargo { get; set; }
        [DataType(DataType.EmailAddress, ErrorMessage = "El email no es valido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El Telefono es obligatorio")]
        public string Telefono { get; set; }
        [Required(ErrorMessage = "el Movil es obligatorio")]
        public string Movil { get; set; }
        [Required(ErrorMessage = "La direccion es obligatoria")]
        public string Direccion { get; set; }
        public int KeyDepartamento { get; set; }
        public int KeyMunicipio { get; set; }
        public bool? Activo { get; set; }

        public virtual Empresa IdEmpresaNavigation { get; set; }
        public virtual Sucursale IdSucursalNavigation { get; set; }
        public virtual ICollection<ClientesDireccion> ClientesDireccions { get; set; }
      //  public virtual ICollection<OrdenTrabajo> OrdenTrabajos { get; set; }
    }
}
