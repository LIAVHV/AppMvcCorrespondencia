using AppMvcCorrespondencia.Models;
using Microsoft.EntityFrameworkCore;

namespace AppMvcCorrespondencia.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Documentos> Documentos { get; set; }
    public virtual DbSet<Historial> Historial { get; set; }
    public virtual DbSet<Usuarios> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Documentos>(entity =>
        {
            entity.ToTable("Documentos");
            entity.HasKey(e => e.IdDocumento).HasName("PK__Document__E520734703BB3637");

            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Detalle).HasMaxLength(255);
            entity.Property(e => e.Estado).IsRequired().HasMaxLength(20).HasDefaultValue("pendiente");
            entity.Property(e => e.Avance).HasDefaultValue(0);
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.UsuarioActualNavigation)
                .WithMany(p => p.DocumentosUsuarioActualNavigation)
                .HasForeignKey(d => d.UsuarioActual)
                .HasConstraintName("FK__Documento__Usuar__2C3393D0");

            entity.HasOne(d => d.UsuarioAnteriorNavigation)
                .WithMany(p => p.DocumentosUsuarioAnteriorNavigation)
                .HasForeignKey(d => d.UsuarioAnterior)
                .HasConstraintName("FK__Documento__Usuar__2D27B809");

            entity.HasOne(d => d.UsuarioCreadorNavigation)
                .WithMany(p => p.DocumentosUsuarioCreadorNavigation)
                .HasForeignKey(d => d.UsuarioCreador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Documento__Usuar__2B3F6F97");
        });

        modelBuilder.Entity<Historial>(entity =>
        {
            entity.ToTable("Historial");
            entity.HasKey(e => e.IdHistorial).HasName("PK__Historia__9CC7DBB47AA28A84");

            entity.Property(e => e.Accion).HasMaxLength(50);
            entity.Property(e => e.Estado).HasMaxLength(250);
            entity.Property(e => e.Avance).HasDefaultValue(0);
            entity.Property(e => e.FechaMovimiento).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.IdDocumentoNavigation)
                .WithMany(p => p.Historial)
                .HasForeignKey(d => d.IdDocumento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Historial__IdDoc__31EC6D26");

            entity.HasOne(d => d.UsuarioDestinoNavigation)
                .WithMany(p => p.HistorialUsuarioDestinoNavigation)
                .HasForeignKey(d => d.UsuarioDestino)
                .HasConstraintName("FK__Historial__Usuar__33D4B598");

            entity.HasOne(d => d.UsuarioOrigenNavigation)
                .WithMany(p => p.HistorialUsuarioOrigenNavigation)
                .HasForeignKey(d => d.UsuarioOrigen)
                .HasConstraintName("FK__Historial__Usuar__32E0915F");
        });

        modelBuilder.Entity<Usuarios>(entity =>
        {
            entity.ToTable("Usuarios");
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__5B65BF97BDC59FFE");

            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
            entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Rol).IsRequired().HasMaxLength(15);
            entity.Property(e => e.Departamento).IsRequired().HasMaxLength(100);
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(getdate())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
