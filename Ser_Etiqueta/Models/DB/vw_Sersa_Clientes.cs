using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Ser_Etiqueta.Models.DB
{
    public partial class vw_Sersa_Clientes
    {
       

        public int KeyCliente { get; set; }

        public string DescripcionCliente { get; set; }
        public string  Telefono { get; set; }
        public string Contacto { get; set; }

        public string Direccion { get; set; }

        public string RUC { get; set; }

        public string NombreComercial { get; set; }




        //  public virtual ICollection<OrdenTrabajo> OrdenTrabajos { get; set; }
    }
}
