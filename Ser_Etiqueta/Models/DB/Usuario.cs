using System;
using System.Collections.Generic;

#nullable disable

namespace Ser_Etiqueta.Models.DB
{
    public partial class Usuario
    {
        public Usuario()
        {
            Ordenes = new HashSet<Ordene>();
            RolUsuarios = new HashSet<RolUsuario>();
            UsuariosEmpresas = new HashSet<UsuariosEmpresa>();
        }

        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public string Pass { get; set; }
        public string Salt { get; set; }
        public bool? Activo { get; set; }




        public virtual ICollection<Ordene> Ordenes { get; set; }
        public virtual ICollection<RolUsuario> RolUsuarios { get; set; }
        public virtual ICollection<UsuariosEmpresa> UsuariosEmpresas { get; set; }
    }
}
