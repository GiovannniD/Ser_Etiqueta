using System;
using System.Collections.Generic;

#nullable disable

namespace Ser_Etiqueta.Models.DB
{
    public partial class UsuariosEmpresa
    {
        public int IdUsuarioEmpresa { get; set; }
        public int? IdEmpresa { get; set; }
        public int? IdSucursal { get; set; }
        public int? IdUsuario { get; set; }
        public bool? Activo { get; set; }

        public virtual Empresa IdEmpresaNavigation { get; set; }
        public virtual Sucursale IdSucursalNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
