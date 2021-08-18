using System;
using System.Collections.Generic;

#nullable disable

namespace Ser_Etiqueta.Models.DB
{
    public partial class ModulosEmpresa
    {
        public int IdModuloEmpresa { get; set; }
        public int? IdModulo { get; set; }
        public int? IdEmpresa { get; set; }
        public int? IdSucursal { get; set; }
        public bool? Activo { get; set; }

        public virtual Empresa IdEmpresaNavigation { get; set; }
        public virtual Modulo IdModuloNavigation { get; set; }
        public virtual Sucursale IdSucursalNavigation { get; set; }
    }
}
