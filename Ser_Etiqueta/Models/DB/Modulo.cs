using System;
using System.Collections.Generic;

#nullable disable

namespace Ser_Etiqueta.Models.DB
{
    public partial class Modulo
    {
        public Modulo()
        {
            ModulosEmpresas = new HashSet<ModulosEmpresa>();
        }

        public int IdModulo { get; set; }
        public string NombreModulo { get; set; }
        public bool? Activo { get; set; }

        public virtual ICollection<ModulosEmpresa> ModulosEmpresas { get; set; }
    }
}
