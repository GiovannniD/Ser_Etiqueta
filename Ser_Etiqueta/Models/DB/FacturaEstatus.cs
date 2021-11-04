using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Ser_Etiqueta.Models.DB
{
    public partial class FacturaEstatus
    {
       
        public int KeyFacturaEstatus { get; set; }
        public string DescripcionEstatus { get; set; }

    }
}
