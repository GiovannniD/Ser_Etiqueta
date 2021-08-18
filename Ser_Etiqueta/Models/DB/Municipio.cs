using System;
using System.Collections.Generic;

#nullable disable

namespace Ser_Etiqueta.Models.DB
{
    public partial class Municipio
    {
        public int KeyMunicipio { get; set; }
        public string CodigoMun { get; set; }
        public int KeyDepartamento { get; set; }
        public string DescripcionMun { get; set; }
        public string CodigoPostal { get; set; }
    }
}
