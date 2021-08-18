using System;
using System.Collections.Generic;

#nullable disable

namespace Ser_Etiqueta.Models.DB
{
    public partial class Departamento
    {
        public int KeyDepartamento { get; set; }
        public string CodigoDep { get; set; }
        public string DescripcionDep { get; set; }
        public int? KeyRuta { get; set; }
    }
}
