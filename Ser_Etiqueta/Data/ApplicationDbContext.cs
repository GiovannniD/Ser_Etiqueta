using Microsoft.EntityFrameworkCore;
using Ser_Etiqueta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ser_Etiqueta.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
         
        }

        public DbSet<Libro> Libro { get; set; }
    }
}
