# Sistema Médico Web - README del Proyecto

## 📋 Descripción del Proyecto

Este es un sistema web médico desarrollado en **ASP.NET Core 8.0** con **Entity Framework Core** para la gestión de datos médicos.

## 🛠️ Tecnologías Utilizadas

- **.NET 8.0**
- **ASP.NET Core MVC**
- **Entity Framework Core 9.0.8**
- **SQL Server**
- **Bootstrap** (para el frontend)
- **jQuery** (para interactividad)

## 📦 Paquetes NuGet de Entity Framework Instalados

### Paquetes Principales (Ya Instalados)

| Paquete | Versión | Descripción |
|---------|---------|-------------|
| `Microsoft.EntityFrameworkCore.SqlServer` | 9.0.8 | Proveedor de SQL Server para Entity Framework Core |
| `Microsoft.EntityFrameworkCore.Design` | 9.0.8 | Herramientas de diseño para EF Core (Migrations, Scaffolding) |
| `Microsoft.EntityFrameworkCore.Tools` | 9.0.8 | Herramientas de línea de comandos para EF Core |

### Comandos para Instalar los Paquetes

Si necesitas reinstalar o verificar los paquetes, ejecuta estos comandos en la **Package Manager Console**:

```powershell
# Paquete principal de EF Core para SQL Server
Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 9.0.8

# Herramientas de diseño (para Migrations y Scaffolding)
Install-Package Microsoft.EntityFrameworkCore.Design -Version 9.0.8

# Herramientas de línea de comandos
Install-Package Microsoft.EntityFrameworkCore.Tools -Version 9.0.8
```

### Comandos CLI (dotnet)

Alternativamente, puedes usar la CLI de .NET:

```bash
# Paquete principal de EF Core para SQL Server
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 9.0.8

# Herramientas de diseño
dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.0.8

# Herramientas de línea de comandos
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 9.0.8
```

## 🗄️ Configuración de Base de Datos

### Cadena de Conexión

La cadena de conexión se encuentra en `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SistemaMedicoDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### Configuración en Program.cs

```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```
