using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ser_Etiqueta.Models
{
    public class Libro
    {
      
            [Key]
            public int Id { get; set; }

            [Required(ErrorMessage = "El titulo es Obligatorio")]
            [StringLength(50, ErrorMessage = "El {0} debe ser al menos {2} y maximo {1} Caracteres", MinimumLength = 3)]
            public string Titulo { get; set; }

            [Required(ErrorMessage = "La descripcion es Obligatorio")]
            [StringLength(50, ErrorMessage = "El {0} debe ser al menos {2} y maximo {1} Caracteres", MinimumLength = 3)]
            public string Descripcion { get; set; }

            [Required(ErrorMessage = "La fecha es Obligatoria")]
            [DataType(DataType.Date)]
            [Display(Name = "Fecha de Lanzamiento")]
            public DateTime FechaLanzamiento { get; set; }

            [Required(ErrorMessage = "La descripcion es Obligatorio")]
            public string Autor { get; set; }

            [Required(ErrorMessage = "La descripcion es Obligatorio")]
            public int Precio { get; set; }
        }
    
}
