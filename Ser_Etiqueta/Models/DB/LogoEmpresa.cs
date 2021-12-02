using System;
using System.Collections.Generic;

#nullable disable

namespace Ser_Etiqueta.Models.DB
{
    public partial class LogoEmpresa
    {
        public int IdLogo { get; set; }
        public int IdEmpresa { get; set; }
        public byte[] logoEmpresa { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool? Activo { get; set; }

      /*  public virtual Empresa IdEmpresaNavigation { get; set; }
        public virtual Sucursale IdSucursalNavigation { get; set; }*/
    }
}
