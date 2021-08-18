using System;
using System.Collections.Generic;

#nullable disable

namespace Ser_Etiqueta.Models.DB
{
    public partial class ClientesDireccion
    {
        public ClientesDireccion()
        {
         //   OrdenTrabajos = new HashSet<OrdenTrabajo>();
        }

        public int IdDireccionCliente { get; set; }
        public int? IdCliente { get; set; }
        public int? IdDepartamento { get; set; }
        public int? IdMunicipio { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public bool? Activo { get; set; }

        public virtual Cliente IdClienteNavigation { get; set; }
      //  public virtual ICollection<OrdenTrabajo> OrdenTrabajos { get; set; }
    }
}
