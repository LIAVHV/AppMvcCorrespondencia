using AppMvcCorrespondencia.Models;
using Microsoft.EntityFrameworkCore;

namespace AppMvcCorrespondencia.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            await context.Database.EnsureCreatedAsync();

            if (await context.Usuarios.AnyAsync())
            {
                return;
            }

            // Hash correspondiente a password "123" de DBCorrespondencia.sql
            const string defaultPasswordHash = "AQAAAAIAAYagAAAAEKdKAbNa0KyV0bivPIcz6xozKt1AB/RH/ry2RjBxrCNouulezqLIC1L5ciHmW//fhQ==";

            var usuariosSeed = new List<Usuarios>
            {
                new Usuarios
                {
                    Nombre = "Administrador del sistema",
                    Username = "admin",
                    PasswordHash = defaultPasswordHash,
                    Rol = "admin",
                    Departamento = "Administrador del sistema",
                    FechaCreacion = DateTime.Now
                },
                new Usuarios
                {
                    Nombre = "ANA GONZÁLEZ",
                    Username = "ana",
                    PasswordHash = defaultPasswordHash,
                    Rol = "usuario",
                    Departamento = "Recursos Humanos",
                    FechaCreacion = DateTime.Now
                },
                new Usuarios
                {
                    Nombre = "PACO LÓPEZ",
                    Username = "paco",
                    PasswordHash = defaultPasswordHash,
                    Rol = "usuario",
                    Departamento = "Finanzas",
                    FechaCreacion = DateTime.Now
                },
                new Usuarios
                {
                    Nombre = "LUIS ROJAS",
                    Username = "luis",
                    PasswordHash = defaultPasswordHash,
                    Rol = "usuario",
                    Departamento = "Tecnología",
                    FechaCreacion = DateTime.Now
                }
            };

            await context.Usuarios.AddRangeAsync(usuariosSeed);
            await context.SaveChangesAsync();
        }
    }
}
