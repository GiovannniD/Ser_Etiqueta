using System;
using System.Collections.Generic;

#nullable disable

namespace Ser_Etiqueta.Models.DB
{
    public partial class Email
    {
        public int IdEmail { get; set; }
        public string Email1 { get; set; }
        public string Pass { get; set; }
        public string Salt { get; set; }
        public bool? Activo { get; set; }
    }
}
