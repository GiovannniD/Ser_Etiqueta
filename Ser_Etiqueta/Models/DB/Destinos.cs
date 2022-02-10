using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Ser_Etiqueta.Models.DB
{
    public partial class Destinos
    {
       

        public int KeyDestino { get; set; }

        public int KeyDepartamento { get; set; }
        public int KeyMunicipio { get; set; }
        public int  KeyCliente { get; set; }

        public string DescripcionDestino { get; set; }

        public bool Activo { get; set; }

        public int? KeyDivision { get; set; }
        public bool PorDefecto { get; set; }

    }
}
