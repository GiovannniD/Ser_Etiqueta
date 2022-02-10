using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Ser_Etiqueta.Models.DB
{
    public partial class SERSAContext : DbContext
    {
        public SERSAContext()
        {   
        }

        public SERSAContext(DbContextOptions<SERSAContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Destinos> Destinos { get; set; }
       

        /* protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
         {
             if (!optionsBuilder.IsConfigured)
             {
 #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                 optionsBuilder.UseSqlServer("Server=DESKTOP-6J97MO9\\COOTEL ;Database=SERETIQUETAS ;Trusted_Connection=True;user Id=sa;password=1234;");
             }
         }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<Destinos>(entity =>
            {

                entity.ToTable("Destinos", "dbo");

                entity.HasKey(e => e.KeyDestino);
               // entity.Property(e => e.IdCliente).HasColumnName("idCliente");

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
