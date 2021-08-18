using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Ser_Etiqueta.Data
{
    public class ApplicationDbContextEtiqueta : DbContext
    {
        public ApplicationDbContextEtiqueta(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        
    }
}
