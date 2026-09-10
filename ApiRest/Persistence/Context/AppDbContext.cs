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

            entity.HasKey(e => e.id_usuario);

            entity.Property(e => e.id_usuario)
                .HasColumnName("id_usuario")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.nombre_completo)
                .HasColumnName("nombre_completo")
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.email)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(e => e.password_hash)
                .HasColumnName("password_hash")
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

            entity.HasIndex(e => e.email)
                .IsUnique()
                .HasDatabaseName("IX_usuarios_email");
        });
    }
}