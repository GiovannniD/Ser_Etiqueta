using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Ser_Etiqueta.Models.DB
{
    public partial class Empresa
    {
        public Empresa()
        {
            Clientes = new HashSet<Cliente>();
            Impresoras = new HashSet<Impresora>();
            LogoEmpresas = new HashSet<LogoEmpresa>();
            ModulosEmpresas = new HashSet<ModulosEmpresa>();
            Ordenes = new HashSet<Ordene>();
            Sucursales = new HashSet<Sucursale>();
            UsuariosEmpresas = new HashSet<UsuariosEmpresa>();
        }


        public int IdEmpresa { get; set; }

        [Required(ErrorMessage = "El codigo Sersa es obligatorio")]
        public int IdSersa { get; set; }

        [Required(ErrorMessage = "El titulo es Obligatorio")]
        public string NombreEmpresa { get; set; }

        [Required(ErrorMessage = "El Nombre comercial es obligatorio")]
        public string NombreComercial { get; set; }
        public DateTime FechaIngreso { get; set; }
        public bool TieneSucursal { get; set; }
        public int? IdDepartamento { get; set; }
        public int? IdMunicipio { get; set; }

        [Required(ErrorMessage = "la serie del codigo de etiqueta es obligatorio")]
        public string SerieCodigoEtiqueta { get; set; } 
        public bool? Activo { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Solo numeros")]
        public int? UltimoConsecutivo { get; set; }

        public virtual ICollection<Cliente> Clientes { get; set; }
        public virtual ICollection<Impresora> Impresoras { get; set; }
        public virtual ICollection<LogoEmpresa> LogoEmpresas { get; set; }
        public virtual ICollection<ModulosEmpresa> ModulosEmpresas { get; set; }
        public virtual ICollection<Ordene> Ordenes { get; set; }
        public virtual ICollection<Sucursale> Sucursales { get; set; }
        public virtual ICollection<UsuariosEmpresa> UsuariosEmpresas { get; set; }
    }
}
