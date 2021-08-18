using System;
using System.Collections.Generic;

#nullable disable

namespace Ser_Etiqueta.Models.DB
{
    public partial class Rol
    {
        public Rol()
        {
            RolUsuarios = new HashSet<RolUsuario>();
        }

        public int IdRol { get; set; }
        public string NombreRol { get; set; }
        public bool? Activo { get; set; }

        public virtual ICollection<RolUsuario> RolUsuarios { get; set; }
    }
}
