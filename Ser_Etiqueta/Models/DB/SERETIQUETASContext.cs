using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Ser_Etiqueta.Models.DB
{
    public partial class SERETIQUETASContext : DbContext
    {
        public SERETIQUETASContext()
        {
        }

        public SERETIQUETASContext(DbContextOptions<SERETIQUETASContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<ClientesDireccion> ClientesDireccions { get; set; }
        public virtual DbSet<Departamento> Departamentos { get; set; }
        public virtual DbSet<Email> Emails { get; set; }
        public virtual DbSet<Empresa> Empresas { get; set; }
        public virtual DbSet<Impresora> Impresoras { get; set; }
        public virtual DbSet<LogoEmpresa> LogoEmpresas { get; set; }
        public virtual DbSet<Modulo> Modulos { get; set; }
        public virtual DbSet<ModulosEmpresa> ModulosEmpresas { get; set; }
        public virtual DbSet<Municipio> Municipios { get; set; }
        public virtual DbSet<OrdenTrabajo> OrdenTrabajos { get; set; }
        public virtual DbSet<OrdenTrabajoCodigo> OrdenTrabajoCodigos { get; set; }
        public virtual DbSet<OrdenTrabajoDetalle> OrdenTrabajoDetalles { get; set; }
        public virtual DbSet<Ordene> Ordenes { get; set; }
        public virtual DbSet<Rol> Rols { get; set; }
        public virtual DbSet<RolUsuario> RolUsuarios { get; set; }
        public virtual DbSet<Sucursale> Sucursales { get; set; }
        public virtual DbSet<TipoPaquete> TipoPaquetes { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<UsuariosEmpresa> UsuariosEmpresas { get; set; }
        public virtual DbSet<SP_CRUD_EMPRESAS> SP_CRUD_EMPRESAS { get; set; }
        public virtual DbSet<SP_CRUD_Sucursal> SP_CRUD_Sucursal { get; set; }

        public virtual DbSet<SP_CRUD_Clientes> SP_CRUD_Clientes { get; set; }

        public virtual DbSet<SP_CRUD_Ordenes> SP_CRUD_Ordenes { get; set; }
        public virtual DbSet<SP_CRUD_OrdenDetalle> SP_CRUD_OrdenDetalle { get; set; }

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

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.IdCliente)
                    .HasName("PK__Clientes__885457EE16D98E94");

                entity.ToTable("Clientes", "etiquetas");

                entity.Property(e => e.IdCliente).HasColumnName("idCliente");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("activo")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IdEmpresa).HasColumnName("idEmpresa");

                entity.Property(e => e.IdSucursal).HasColumnName("idSucursal");

                entity.Property(e => e.Codigo)
                  .HasMaxLength(50)
                    .IsUnicode(false)
                .HasColumnName("Codigo")
            .HasDefaultValueSql("('')");

                entity.Property(e => e.NombreCliente)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("nombreCliente")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.NombreComercial)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("nombreComercial")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Contacto)
    .HasMaxLength(50)
    .IsUnicode(false)
    .HasColumnName("Contacto")
    .HasDefaultValueSql("('')");

                entity.Property(e => e.Cargo)
.HasMaxLength(50)
.IsUnicode(false)
.HasColumnName("Cargo")
.HasDefaultValueSql("('')");

                entity.Property(e => e.Email)
.HasMaxLength(50)
.IsUnicode(false)
.HasColumnName("Email")
.HasDefaultValueSql("('')");

                entity.Property(e => e.Telefono)
.HasMaxLength(50)
.IsUnicode(false)
.HasColumnName("Telefono")
.HasDefaultValueSql("('')");

                entity.Property(e => e.Movil)
.HasMaxLength(50)
.IsUnicode(false)
.HasColumnName("Movil")
.HasDefaultValueSql("('')");

                entity.Property(e => e.Direccion)
            .HasMaxLength(50)
.IsUnicode(false)
.HasColumnName("Direccion")
.HasDefaultValueSql("('')");

                entity.Property(e => e.KeyDepartamento).HasColumnName("KeyDepartamento");

                entity.Property(e => e.KeyMunicipio).HasColumnName("KeyMunicipio");

                entity.HasOne(d => d.IdEmpresaNavigation)
                    .WithMany(p => p.Clientes)
                    .HasForeignKey(d => d.IdEmpresa)
                    .HasConstraintName("FK__Clientes__idEmpr__10566F31");

                entity.HasOne(d => d.IdSucursalNavigation)
                    .WithMany(p => p.Clientes)
                    .HasForeignKey(d => d.IdSucursal)
                    .HasConstraintName("FK__Clientes__idSucu__114A936A");
            });

            modelBuilder.Entity<ClientesDireccion>(entity =>
            {
                entity.HasKey(e => e.IdDireccionCliente)
                    .HasName("PK__Clientes__51EA047A34B391CC");

                entity.ToTable("ClientesDireccion", "etiquetas");

                entity.Property(e => e.IdDireccionCliente).HasColumnName("idDireccionCliente");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("activo")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Direccion)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("direccion")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.IdCliente).HasColumnName("idCliente");

                entity.Property(e => e.IdDepartamento).HasColumnName("idDepartamento");

                entity.Property(e => e.IdMunicipio).HasColumnName("idMunicipio");

                entity.Property(e => e.Telefono)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("telefono")
                    .HasDefaultValueSql("('')");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.ClientesDireccions)
                    .HasForeignKey(d => d.IdCliente)
                    .HasConstraintName("FK__ClientesD__idCli__17036CC0");
            });

            modelBuilder.Entity<Departamento>(entity =>
            {
                entity.HasKey(e => e.KeyDepartamento);

                entity.ToTable("Departamento", "etiquetas");

                entity.Property(e => e.CodigoDep).HasMaxLength(2);

                entity.Property(e => e.DescripcionDep)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Email>(entity =>
            {
                entity.HasKey(e => e.IdEmail)
                    .HasName("PK__Email__DF53771024AAA5A8");

                entity.ToTable("Email", "configuration");

                entity.Property(e => e.IdEmail).HasColumnName("idEmail");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("activo")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Email1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Pass)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("pass")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("salt")
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<Empresa>(entity =>
            {
                entity.HasKey(e => e.IdEmpresa)
                    .HasName("PK__Empresas__75D2CED48C905AA4");

                entity.ToTable("Empresas", "configuration");

                entity.Property(e => e.IdEmpresa).HasColumnName("idEmpresa");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("activo")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.FechaIngreso)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaIngreso")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdDepartamento).HasColumnName("idDepartamento");

                entity.Property(e => e.IdMunicipio).HasColumnName("idMunicipio");

                entity.Property(e => e.IdSersa).HasColumnName("idSersa");

                entity.Property(e => e.NombreComercial)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("nombreComercial")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.NombreEmpresa)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("nombreEmpresa")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SerieCodigoEtiqueta)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("serieCodigoEtiqueta");

                entity.Property(e => e.TieneSucursal).HasColumnName("tieneSucursal");

                entity.Property(e => e.UltimoConsecutivo).HasColumnName("ultimoConsecutivo");
            });

            modelBuilder.Entity<Impresora>(entity =>
            {
                entity.HasKey(e => e.IdImpresora)
                    .HasName("PK__Impresor__CDB0284B8FBD4593");

                entity.ToTable("Impresoras", "configuration");

                entity.Property(e => e.IdImpresora).HasColumnName("idImpresora");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("activo")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.FechaIngreso)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaIngreso")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdEmpresa).HasColumnName("idEmpresa");

                entity.Property(e => e.IdSucursal).HasColumnName("idSucursal");

                entity.Property(e => e.ModeloImpresora)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("modeloImpresora")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.NombreImpresora)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("nombreImpresora")
                    .HasDefaultValueSql("('')");

                entity.HasOne(d => d.IdEmpresaNavigation)
                    .WithMany(p => p.Impresoras)
                    .HasForeignKey(d => d.IdEmpresa)
                    .HasConstraintName("FK__Impresora__idEmp__787EE5A0");

                entity.HasOne(d => d.IdSucursalNavigation)
                    .WithMany(p => p.Impresoras)
                    .HasForeignKey(d => d.IdSucursal)
                    .HasConstraintName("FK__Impresora__idSuc__797309D9");
            });

            modelBuilder.Entity<LogoEmpresa>(entity =>
            {
                entity.HasKey(e => e.IdLogo)
                    .HasName("PK__LogoEmpr__1B94750BF6056A1B");

                entity.ToTable("LogoEmpresas", "configuration");

                entity.Property(e => e.IdLogo).HasColumnName("idLogo");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("activo")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaCreacion")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdEmpresa).HasColumnName("idEmpresa");

                entity.Property(e => e.IdSucursal).HasColumnName("idSucursal");

                entity.Property(e => e.LogoEmpresa1)
                    .IsUnicode(false)
                    .HasColumnName("logoEmpresa");

                entity.HasOne(d => d.IdEmpresaNavigation)
                    .WithMany(p => p.LogoEmpresas)
                    .HasForeignKey(d => d.IdEmpresa)
                    .HasConstraintName("FK__LogoEmpre__idEmp__00200768");

                entity.HasOne(d => d.IdSucursalNavigation)
                    .WithMany(p => p.LogoEmpresas)
                    .HasForeignKey(d => d.IdSucursal)
                    .HasConstraintName("FK__LogoEmpre__idSuc__01142BA1");
            });

            modelBuilder.Entity<Modulo>(entity =>
            {
                entity.HasKey(e => e.IdModulo)
                    .HasName("PK__Modulos__3CE613FA93BB4EA8");

                entity.ToTable("Modulos", "configuration");

                entity.Property(e => e.IdModulo).HasColumnName("idModulo");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("activo")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.NombreModulo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombreModulo")
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<ModulosEmpresa>(entity =>
            {
                entity.HasKey(e => e.IdModuloEmpresa)
                    .HasName("PK__ModulosE__930470061A5825B5");

                entity.ToTable("ModulosEmpresa", "configuration");

                entity.Property(e => e.IdModuloEmpresa).HasColumnName("idModuloEmpresa");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("activo")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IdEmpresa).HasColumnName("idEmpresa");

                entity.Property(e => e.IdModulo).HasColumnName("idModulo");

                entity.Property(e => e.IdSucursal).HasColumnName("idSucursal");

                entity.HasOne(d => d.IdEmpresaNavigation)
                    .WithMany(p => p.ModulosEmpresas)
                    .HasForeignKey(d => d.IdEmpresa)
                    .HasConstraintName("FK__ModulosEm__idEmp__74AE54BC");

                entity.HasOne(d => d.IdModuloNavigation)
                    .WithMany(p => p.ModulosEmpresas)
                    .HasForeignKey(d => d.IdModulo)
                    .HasConstraintName("FK__ModulosEm__idMod__73BA3083");

                entity.HasOne(d => d.IdSucursalNavigation)
                    .WithMany(p => p.ModulosEmpresas)
                    .HasForeignKey(d => d.IdSucursal)
                    .HasConstraintName("FK__ModulosEm__idSuc__75A278F5");
            });

            modelBuilder.Entity<Municipio>(entity =>
            {
                entity.HasKey(e => e.KeyMunicipio);

                entity.ToTable("Municipio", "etiquetas");

                entity.Property(e => e.CodigoMun).HasMaxLength(4);

                entity.Property(e => e.CodigoPostal).HasMaxLength(50);

                entity.Property(e => e.DescripcionMun)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<OrdenTrabajo>(entity =>
            {
                entity.HasKey(e => e.IdOrdenTrabajo);
                    

                entity.ToTable("OrdenTrabajo", "etiquetas");

                entity.Property(e => e.IdOrdenTrabajo).HasColumnName("idOrdenTrabajo");

                entity.Property(e => e.IdEmpresa).HasColumnName("idEmpresa");
                /*  entity.Property(e => e.DetallesAdicionales)
                      .HasMaxLength(50)
                      .IsUnicode(false)
                      .HasColumnName("detallesAdicionales")
                      .HasDefaultValueSql("('')");*/
                entity.Property(e => e.estado)
      .HasColumnName("estado")
      .HasDefaultValueSql("(1)");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaCreacion").HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Generada).HasColumnName("generada").HasDefaultValueSql("(1)"); ;

              /*  entity.Property(e => e.IdCliente).HasColumnName("idCliente");

                entity.Property(e => e.IdDireccionCliente).HasColumnName("idDireccionCliente");

                entity.Property(e => e.IdOrdenes).HasColumnName("idOrdenes");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.OrdenTrabajos)
                    .HasForeignKey(d => d.IdCliente)
                    .HasConstraintName("FK__OrdenTrab__idCli__2645B050");

                entity.HasOne(d => d.IdDireccionClienteNavigation)
                    .WithMany(p => p.OrdenTrabajos)
                    .HasForeignKey(d => d.IdDireccionCliente)
                    .HasConstraintName("FK__OrdenTrab__idDir__2739D489");

                entity.HasOne(d => d.IdOrdenesNavigation)
                    .WithMany(p => p.OrdenTrabajos)
                    .HasForeignKey(d => d.IdOrdenes)
                    .HasConstraintName("FK__OrdenTrab__idOrd__25518C17");*/
            });

            modelBuilder.Entity<OrdenTrabajoCodigo>(entity =>
            {
                entity.HasKey(e => e.IdOtcodigo)
                    .HasName("PK__OrdenTra__FA8CD3128231E82B");

                entity.ToTable("OrdenTrabajoCodigo", "etiquetas");

                entity.Property(e => e.IdOtcodigo).HasColumnName("idOTCodigo");

                entity.Property(e => e.Imagen)
                    .HasColumnName("Imagen");

                entity.Property(e => e.CodigoSerie)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("codigoSerie");

                entity.Property(e => e.IdOtdetalle).HasColumnName("idOTDetalle");

                entity.Property(e => e.Serie)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("serie");

              /*  entity.HasOne(d => d.IdOtdetalleNavigation)
                    .WithMany(p => p.OrdenTrabajoCodigos)
                    .HasForeignKey(d => d.IdOtdetalle)
                    .HasConstraintName("FK__OrdenTrab__idOTD__4A18FC72");*/
            });

            modelBuilder.Entity<OrdenTrabajoDetalle>(entity =>
            {
                entity.HasKey(e => e.IdOtdetalle)
                    .HasName("PK__OrdenTra__39F1DE281C7D5577");

                entity.ToTable("OrdenTrabajoDetalle", "etiquetas");

                entity.Property(e => e.IdOtdetalle).HasColumnName("idOTDetalle");

                entity.Property(e => e.CantidadBulto).HasColumnName("cantidadBulto");

                entity.Property(e => e.IdOrdenTrabajo).HasColumnName("idOrdenTrabajo");

                entity.Property(e => e.IdTipoPaquete).HasColumnName("idTipoPaquete");

                entity.Property(e => e.Codigo).HasColumnName("Codigo");
                entity.Property(e => e.idCliente).HasColumnName("idCliente");
                entity.Property(e => e.Factura).HasColumnName("Factura");
                entity.Property(e => e.Codigo).HasColumnName("Codigo");
                entity.Property(e => e.direccion).HasColumnName("direccion");

                entity.Property(e => e.Peso)
                    .HasColumnName("peso")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PestoTotal)
                    .HasColumnName("pestoTotal")
                    .HasComputedColumnSql("([cantidadBulto]*[peso])", false);

                entity.Property(e => e.fechaRegistro)
                    .HasColumnName("fechaRegistro")
                    .HasComputedColumnSql("(getdate())", false);

             /*   entity.HasOne(d => d.IdOrdenTrabajoNavigation)
                    .WithMany(p => p.OrdenTrabajoDetalles)
                    .HasForeignKey(d => d.IdOrdenTrabajo)
                    .HasConstraintName("FK__OrdenTrab__idOrd__2B0A656D");*/

               /* entity.HasOne(d => d.IdTipoPaqueteNavigation)
                    .WithMany(p => p.OrdenTrabajoDetalles)
                    .HasForeignKey(d => d.IdTipoPaquete)
                    .HasConstraintName("FK__OrdenTrab__idTip__2BFE89A6");*/
            });

            modelBuilder.Entity<Ordene>(entity =>
            {
                entity.HasKey(e => e.IdOrdenes)
                    .HasName("PK__Ordenes__25A13BE5A61EC59C");

                entity.ToTable("Ordenes", "etiquetas");

                entity.Property(e => e.IdOrdenes).HasColumnName("idOrdenes");

                entity.Property(e => e.DescripcionOrden)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("descripcionOrden")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaCreacion")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Generado).HasColumnName("generado");

                entity.Property(e => e.IdEmpresa).HasColumnName("idEmpresa");

                entity.Property(e => e.IdSucursal).HasColumnName("idSucursal");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.HasOne(d => d.IdEmpresaNavigation)
                    .WithMany(p => p.Ordenes)
                    .HasForeignKey(d => d.IdEmpresa)
                    .HasConstraintName("FK__Ordenes__idEmpre__1EA48E88");

                entity.HasOne(d => d.IdSucursalNavigation)
                    .WithMany(p => p.Ordenes)
                    .HasForeignKey(d => d.IdSucursal)
                    .HasConstraintName("FK__Ordenes__idSucur__1F98B2C1");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Ordenes)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK__Ordenes__idUsuar__22751F6C");
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.HasKey(e => e.IdRol)
                    .HasName("PK__Rol__3C872F7693ADDEE7");

                entity.ToTable("Rol", "configuration");

                entity.Property(e => e.IdRol).HasColumnName("idRol");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("activo")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.NombreRol)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombreRol")
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<RolUsuario>(entity =>
            {
                entity.HasKey(e => e.IdRolUsuario)
                    .HasName("PK__RolUsuar__B6AD5CB57CC62F73");

                entity.ToTable("RolUsuarios", "configuration");

                entity.Property(e => e.IdRolUsuario).HasColumnName("idRolUsuario");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("activo")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IdRol).HasColumnName("idRol");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.RolUsuarios)
                    .HasForeignKey(d => d.IdRol)
                    .HasConstraintName("FK__RolUsuari__idRol__6D0D32F4");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.RolUsuarios)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK__RolUsuari__idUsu__6C190EBB");
            });

            modelBuilder.Entity<Sucursale>(entity =>
            {
                entity.HasKey(e => e.IdSucursal)
                    .HasName("PK__Sucursal__F707694CBE27FD40");

                entity.ToTable("Sucursales", "configuration");

                entity.Property(e => e.IdSucursal).HasColumnName("idSucursal");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("activo")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.FechaIngreso)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaIngreso")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdDepartamento).HasColumnName("idDepartamento");

                entity.Property(e => e.IdEmpresa).HasColumnName("idEmpresa");

                entity.Property(e => e.IdMunicipio).HasColumnName("idMunicipio");

                entity.Property(e => e.NombreSucursal)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("nombreSucursal")
                    .HasDefaultValueSql("('')");

                entity.HasOne(d => d.IdEmpresaNavigation)
                    .WithMany(p => p.Sucursales)
                    .HasForeignKey(d => d.IdEmpresa)
                    .HasConstraintName("FK__Sucursale__idEmp__571DF1D5");
            });

            modelBuilder.Entity<TipoPaquete>(entity =>
            {
                entity.HasKey(e => e.IdTipoPaquete)
                    .HasName("PK__TipoPaqu__C12AF8CD153237D9");

                entity.ToTable("TipoPaquete", "etiquetas");

                entity.Property(e => e.IdTipoPaquete).HasColumnName("idTipoPaquete");

                entity.Property(e => e.DesTipoPaquete)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("desTipoPaquete");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PK__Usuarios__645723A60323439E");

                entity.ToTable("Usuarios", "configuration");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("activo")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.NombreCompleto)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("nombreCompleto")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.NombreUsuario)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombreUsuario")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Pass)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("pass")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("salt")
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<UsuariosEmpresa>(entity =>
            {
                entity.HasKey(e => e.IdUsuarioEmpresa)
                    .HasName("PK__Usuarios__BD82ACBF96973767");

                entity.ToTable("UsuariosEmpresa", "configuration");

                entity.Property(e => e.IdUsuarioEmpresa).HasColumnName("idUsuarioEmpresa");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("activo")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IdEmpresa).HasColumnName("idEmpresa");

                entity.Property(e => e.IdSucursal).HasColumnName("idSucursal");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.HasOne(d => d.IdEmpresaNavigation)
                    .WithMany(p => p.UsuariosEmpresas)
                    .HasForeignKey(d => d.IdEmpresa)
                    .HasConstraintName("FK__UsuariosE__idEmp__6754599E");

                entity.HasOne(d => d.IdSucursalNavigation)
                    .WithMany(p => p.UsuariosEmpresas)
                    .HasForeignKey(d => d.IdSucursal)
                    .HasConstraintName("FK__UsuariosE__idSuc__68487DD7");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.UsuariosEmpresas)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK__UsuariosE__idUsu__693CA210");
            });

            modelBuilder.Entity<SP_CRUD_EMPRESAS>(entity =>
            {
                entity.HasKey(e => e.IdEmpresa);

            });

            modelBuilder.Entity<SP_CRUD_Sucursal>(entity =>
            {
                entity.HasKey(e => e.IdSucursal);

            });

            modelBuilder.Entity<SP_CRUD_Clientes>(entity =>
            {
                entity.HasKey(e => e.IdCliente);

            });

            modelBuilder.Entity<SP_CRUD_Ordenes>(entity =>
            {
                entity.HasKey(e => e.IdOrdenTrabajo);

            });

            modelBuilder.Entity<SP_CRUD_OrdenDetalle>(entity =>
            {
                entity.HasKey(e => e.IdOtdetalle);

            });
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
