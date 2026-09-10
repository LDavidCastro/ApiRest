namespace ApiRest.Persistence.Context;

using Microsoft.EntityFrameworkCore;
using ApiRest.Domain.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("usuarios");

            entity.HasKey(e => e.id);

            entity.Property(e => e.id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.nombre)
                .HasColumnName("nombre")
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.apellido)
                .HasColumnName("apellido")
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.correo_electronico)
                .HasColumnName("correo_electronico")
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(e => e.contrasena_hash)
                .HasColumnName("contrasena_hash")
                .IsRequired();

            entity.Property(e => e.rol)
                .HasColumnName("rol")
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("usuario");

            entity.Property(e => e.activo)
                .HasColumnName("activo")
                .IsRequired()
                .HasDefaultValue(true);

            entity.Property(e => e.fecha_creacion)
                .HasColumnName("fecha_creacion")
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(e => e.fecha_actualizacion)
                .HasColumnName("fecha_actualizacion");

            entity.Property(e => e.ultimo_acceso)
                .HasColumnName("ultimo_acceso");

            entity.HasIndex(e => e.correo_electronico)
                .IsUnique()
                .HasDatabaseName("IX_usuarios_correo_electronico");
        });
    }
}