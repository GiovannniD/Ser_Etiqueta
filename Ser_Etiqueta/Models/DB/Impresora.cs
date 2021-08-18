using System;
using System.Collections.Generic;

#nullable disable

namespace Ser_Etiqueta.Models.DB
{
    public partial class Impresora
    {
        public int IdImpresora { get; set; }
        public int? IdEmpresa { get; set; }
        public int? IdSucursal { get; set; }
        public string NombreImpresora { get; set; }
        public string ModeloImpresora { get; set; }
        public DateTime FechaIngreso { get; set; }
        public bool? Activo { get; set; }

        public virtual Empresa IdEmpresaNavigation { get; set; }
        public virtual Sucursale IdSucursalNavigation { get; set; }
    }
}
