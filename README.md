# AppMvcCorrespondencia (.NET 8 + ASP.NET Core MVC + EF Core 8 + SQL Server 2019)


## Requisitos previos
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (`8.0.x`)
- SQL Server 2019 (instancia local, Docker o SQL Server Express / LocalDB)

## Configuración e Instalación
1. **Base de datos:**
   - Puede ejecutar el script original `DBCorrespondencia.sql` en SQL Server 2019 **o** dejar que `DbInitializer.SeedAsync(context)` cree automáticamente las tablas y cargue los usuarios de prueba al iniciar la aplicación (`EnsureCreatedAsync()`).
2. **Cadena de conexión:**
   - Ajuste `ConnectionStrings:DefaultConnection` en `appsettings.json` (producción/Docker) o `appsettings.Development.json` (desarrollo local).
3. **Compilar y ejecutar:**
   ```bash
   dotnet restore
   dotnet build
   dotnet run
   ```
4. **Credenciales iniciales (Datos Semilla):**
   - **Administrador:** `admin` / `123`
   - **Usuarios operativos:** `ana` / `123`, `paco` / `123`, `luis` / `123`
