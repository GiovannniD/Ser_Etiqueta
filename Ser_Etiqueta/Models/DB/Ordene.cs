using System;
using System.Collections.Generic;

#nullable disable

namespace Ser_Etiqueta.Models.DB
{
    public partial class Ordene
    {
        public Ordene()
        {
           // OrdenTrabajos = new HashSet<OrdenTrabajo>();
        }

        public int IdOrdenes { get; set; }
        public int? IdEmpresa { get; set; }
        public int? IdSucursal { get; set; }
        public string DescripcionOrden { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int? IdUsuario { get; set; }
        public bool? Generado { get; set; }

        public virtual Empresa IdEmpresaNavigation { get; set; }
        public virtual Sucursale IdSucursalNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
      //  public virtual ICollection<OrdenTrabajo> OrdenTrabajos { get; set; }
    }
}
