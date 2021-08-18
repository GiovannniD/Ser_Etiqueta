using System;
using System.Collections.Generic;

#nullable disable

namespace Ser_Etiqueta.Models.DB
{
    public partial class Sucursale
    {
        public Sucursale()
        {
            Clientes = new HashSet<Cliente>();
            Impresoras = new HashSet<Impresora>();
            LogoEmpresas = new HashSet<LogoEmpresa>();
            ModulosEmpresas = new HashSet<ModulosEmpresa>();
            Ordenes = new HashSet<Ordene>();
            UsuariosEmpresas = new HashSet<UsuariosEmpresa>();
        }

        public int IdSucursal { get; set; }
        public int? IdEmpresa { get; set; }
        public string NombreSucursal { get; set; }
        public int? IdDepartamento { get; set; }
        public int? IdMunicipio { get; set; }
        public DateTime FechaIngreso { get; set; }
        public bool? Activo { get; set; }

        public virtual Empresa IdEmpresaNavigation { get; set; }
        public virtual ICollection<Cliente> Clientes { get; set; }
        public virtual ICollection<Impresora> Impresoras { get; set; }
        public virtual ICollection<LogoEmpresa> LogoEmpresas { get; set; }
        public virtual ICollection<ModulosEmpresa> ModulosEmpresas { get; set; }
        public virtual ICollection<Ordene> Ordenes { get; set; }
        public virtual ICollection<UsuariosEmpresa> UsuariosEmpresas { get; set; }
    }
}
