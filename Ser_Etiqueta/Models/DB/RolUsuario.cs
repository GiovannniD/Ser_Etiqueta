using System;
using System.Collections.Generic;

#nullable disable

namespace Ser_Etiqueta.Models.DB
{
    public partial class RolUsuario
    {
        public int IdRolUsuario { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdRol { get; set; }
        public bool? Activo { get; set; }

        public virtual Rol IdRolNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
