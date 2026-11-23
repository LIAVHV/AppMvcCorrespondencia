# **Sistema de Gestión de Correspondencia**

## **Descripción**

El Sistema de Gestión de Correspondencia es una aplicación web que permite administrar, enviar y dar seguimiento a documentos de manera sencilla y organizada.

## **Funcionalidades principales**

- **Gestión de documentos:** permite registrar nuevos documentos.
- **Envío de correspondencia:** facilita el envío de documentos entre usuarios.
- **Recepción de documentos:** permite recibir y actualizar el estado de los documentos.
- **Seguimiento:** consulta el historial de movimientos y acciones realizadas.
- **Usuarios:** administra el acceso al sistema según el perfil correspondiente.

## **Objetivo**

Facilitar el control y seguimiento de la correspondencia, manteniendo un registro de los documentos, sus responsables y su estado.

## **Tecnologías**

- ASP.NET Core 8
- C#
- Entity Framework Core
- SQL Server 2019

## **Beneficios**

- Mejor organización de la información.
- Control de los documentos enviados y recibidos.
- Consulta del historial de movimientos.
- Mayor facilidad para dar seguimiento a la correspondencia.

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
