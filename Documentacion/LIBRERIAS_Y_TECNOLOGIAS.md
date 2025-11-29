# Librerías y Tecnologías - Sistema de Alarma Médica

## Índice
1. [Tecnologías Base](#tecnologías-base)
2. [Librerías por Proyecto](#librerías-por-proyecto)
3. [Descripción Detallada de Librerías](#descripción-detallada-de-librerías)
4. [Versiones y Compatibilidad](#versiones-y-compatibilidad)
5. [Justificación de Elecciones](#justificación-de-elecciones)

---

## Tecnologías Base

### Framework y Lenguaje
- **.NET 6.0** (LTS - Long Term Support)
  - Soporte hasta noviembre 2024
  - Mejoras de rendimiento sobre .NET 5
  - Unificación de .NET Framework y .NET Core

- **C# 10**
  - Características modernas: record types, pattern matching, global usings
  - Null-safety con nullable reference types

### Runtime
- **ASP.NET Core 6.0**
  - Framework web de alto rendimiento
  - Multiplataforma (Windows, Linux, macOS)
  - Modular y ligero

### Base de Datos
- **SQL Server**
  - Base de datos relacional de Microsoft
  - Integración nativa con .NET
  - Herramientas robustas de administración

---

## Librerías por Proyecto

### 1. Dominio

| Librería | Versión | Propósito |
|----------|---------|-----------|
| AutoMapper | 13.0.1 | Mapeo objeto-objeto (Entidades ↔ DTOs) |
| Microsoft.EntityFrameworkCore | 6.0.5 | Abstracciones de EF Core para interfaces |
| Newtonsoft.Json | 13.0.3 | Serialización/deserialización JSON |

**Archivo**: `Dominio/Dominio.csproj`

```xml
<ItemGroup>
  <PackageReference Include="AutoMapper" Version="13.0.1" />
  <PackageReference Include="Microsoft.EntityFrameworkCore" Version="6.0.5" />
  <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
</ItemGroup>
```

---

### 2. Infraestructura

| Librería | Versión | Propósito |
|----------|---------|-----------|
| Microsoft.EntityFrameworkCore | 6.0.5 | ORM para acceso a datos |
| Microsoft.EntityFrameworkCore.SqlServer | 6.0.5 | Proveedor de SQL Server |
| Microsoft.EntityFrameworkCore.Tools | 6.0.5 | Herramientas CLI (migraciones) |
| Microsoft.EntityFrameworkCore.Design | 6.0.5 | Diseño de migraciones |
| Microsoft.Extensions.Configuration | 6.0.1 | Lectura de configuración |
| Microsoft.Extensions.Configuration.Json | 6.0.0 | Configuración desde JSON |
| Microsoft.Extensions.Options.ConfigurationExtensions | 6.0.0 | Binding de opciones |

**Archivo**: `Infraestructura/Infraestructura.csproj`

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.EntityFrameworkCore" Version="6.0.5" />
  <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="6.0.5" />
  <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="6.0.5">
    <PrivateAssets>all</PrivateAssets>
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
  </PackageReference>
  <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="6.0.5">
    <PrivateAssets>all</PrivateAssets>
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
  </PackageReference>
  <PackageReference Include="Microsoft.Extensions.Configuration" Version="6.0.1" />
  <PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="6.0.0" />
  <PackageReference Include="Microsoft.Extensions.Options.ConfigurationExtensions" Version="6.0.0" />
</ItemGroup>
```

---

### 3. Aplicacion

| Librería | Versión | Propósito |
|----------|---------|-----------|
| Microsoft.Extensions.DependencyInjection.Abstractions | 6.0.0 | Inyección de dependencias |
| Microsoft.Extensions.Http | 6.0.1 | HttpClientFactory |
| Microsoft.EntityFrameworkCore.Design | 6.0.5 | Soporte de migraciones |

**Archivo**: `Aplicacion/Aplicacion.csproj`

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="6.0.5">
    <PrivateAssets>all</PrivateAssets>
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
  </PackageReference>
  <PackageReference Include="Microsoft.Extensions.DependencyInjection.Abstractions" Version="6.0.0" />
  <PackageReference Include="Microsoft.Extensions.Http" Version="6.0.1" />
</ItemGroup>
```

---

### 4. Presentacion (Web MVC)

| Librería | Versión | Propósito |
|----------|---------|-----------|
| FluentValidation.AspNetCore | 11.3.0 | Validación de modelos |
| Microsoft.AspNetCore.Authentication.Google | 6.0.0 | OAuth con Google |
| Microsoft.EntityFrameworkCore.Design | 6.0.5 | Soporte de migraciones |
| Microsoft.VisualStudio.Web.CodeGeneration.Design | 6.0.18 | Scaffolding de vistas/controladores |

**Archivo**: `Presentacion/Presentacion.csproj`

```xml
<ItemGroup>
  <PackageReference Include="FluentValidation.AspNetCore" Version="11.3.0" />
  <PackageReference Include="Microsoft.AspNetCore.Authentication.Google" Version="6.0.0" />
  <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="6.0.5">
    <PrivateAssets>all</PrivateAssets>
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
  </PackageReference>
  <PackageReference Include="Microsoft.VisualStudio.Web.CodeGeneration.Design" Version="6.0.18" />
</ItemGroup>
```

---

### 5. PresentacionApi (Web API)

| Librería | Versión | Propósito |
|----------|---------|-----------|
| Swashbuckle.AspNetCore | 6.5.0 | Documentación Swagger/OpenAPI |
| Microsoft.EntityFrameworkCore.Design | 6.0.5 | Soporte de migraciones |

**Archivo**: `PresentacionApi/PresentacionApi.csproj`

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="6.0.5">
    <PrivateAssets>all</PrivateAssets>
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
  </PackageReference>
  <PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />
</ItemGroup>
```

---

### 6. DefaultDI

Este proyecto no tiene dependencias externas adicionales, solo .NET 6.0 base.

```xml
<PropertyGroup>
  <TargetFramework>net6.0</TargetFramework>
</PropertyGroup>
```

---

## Descripción Detallada de Librerías

### AutoMapper (13.0.1)

**Propósito**: Simplificar el mapeo entre objetos de diferentes tipos (Entidades ↔ DTOs).

**Uso en el proyecto**:
- Configuración en `Dominio/Application/Mappings/AutoMapperProfile.cs`
- Mapeos definidos:
  ```csharp
  CreateMap<Paciente, PacienteDto>().ReverseMap();
  CreateMap<Medico, MedicoDto>().ReverseMap();
  CreateMap<OrdenMedica, OrdenMedicaDto>().ReverseMap();
  // etc.
  ```

**Registro**:
```csharp
services.AddAutoMapper(typeof(AutoMapperProfile));
```

**Ventajas**:
- Reduce código boilerplate
- Configuración centralizada
- Soporta mapeos complejos y condicionales

**Documentación**: https://automapper.org/

---

### Entity Framework Core (6.0.5)

**Componentes**:
1. **Microsoft.EntityFrameworkCore**: Núcleo del ORM
2. **Microsoft.EntityFrameworkCore.SqlServer**: Proveedor para SQL Server
3. **Microsoft.EntityFrameworkCore.Tools**: CLI para migraciones
4. **Microsoft.EntityFrameworkCore.Design**: Tiempo de diseño

**Uso en el proyecto**:
- `AplicacionBDContexto`: DbContext principal
- Code-First approach: Entidades definen el esquema
- Migraciones para control de versiones de BD

**Comandos útiles**:
```bash
# Crear migración
dotnet ef migrations add NombreMigracion --project Infraestructura

# Aplicar migraciones
dotnet ef database update --project Infraestructura

# Revertir migración
dotnet ef database update MigracionAnterior --project Infraestructura

# Eliminar última migración
dotnet ef migrations remove --project Infraestructura
```

**Características utilizadas**:
- Navegación entre entidades
- Lazy loading / Eager loading (Include)
- Transacciones
- Fluent API para configuración

**Documentación**: https://docs.microsoft.com/ef/core/

---

### Newtonsoft.Json (13.0.3)

**Propósito**: Serialización/deserialización JSON de alto rendimiento.

**Uso en el proyecto**:
- Respuestas de API CIMA
- Serialización de objetos complejos
- Configuración de propiedades JSON

**Ejemplo**:
```csharp
var json = JsonConvert.SerializeObject(objeto);
var objeto = JsonConvert.DeserializeObject<Tipo>(json);
```

**Ventajas**:
- Muy maduro y estable
- Ampliamente utilizado en .NET
- Soporta configuración avanzada

**Alternativa**: System.Text.Json (incluido en .NET 6) - podría considerarse para nuevas implementaciones.

**Documentación**: https://www.newtonsoft.com/json

---

### FluentValidation.AspNetCore (11.3.0)

**Propósito**: Validación de modelos con sintaxis fluida y expresiva.

**Uso en el proyecto**:
- Validadores en `Presentacion/Tools/Validators/`
- Ejemplo: `PacienteValidator`, `MedicoValidator`, `OrdenMedicaValidator`

**Ejemplo de uso**:
```csharp
public class PacienteValidator : AbstractValidator<PacienteViewModel>
{
    public PacienteValidator()
    {
        RuleFor(p => p.Documento)
            .NotEmpty().WithMessage("El documento es obligatorio")
            .Length(7, 8).WithMessage("El documento debe tener 7 u 8 dígitos");

        RuleFor(p => p.Email)
            .EmailAddress().WithMessage("Email inválido")
            .When(p => !string.IsNullOrEmpty(p.Email));
    }
}
```

**Registro**:
```csharp
builder.Services.AddControllersWithViews()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());
```

**Ventajas**:
- Validaciones complejas y reutilizables
- Mensajes personalizables
- Validación condicional
- Integración con ASP.NET Core

**Documentación**: https://docs.fluentvalidation.net/

---

### Microsoft.AspNetCore.Authentication.Google (6.0.0)

**Propósito**: Autenticación OAuth 2.0 con Google.

**Uso en el proyecto**:
- Configuración en `Presentacion/Program.cs`
- Permite inicio de sesión con cuenta de Google

**Configuración**:
```csharp
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
})
.AddCookie("Cookies")
.AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    googleOptions.CallbackPath = "/signin-google";
    googleOptions.Scope.Add("openid");
    googleOptions.Scope.Add("profile");
    googleOptions.Scope.Add("email");
});
```

**Flujo**:
1. Usuario hace clic en "Iniciar sesión con Google"
2. Redirección a Google OAuth
3. Usuario autoriza
4. Callback a `/signin-google`
5. Creación de usuario si es nuevo
6. Establecimiento de sesión

**Requisitos**:
- ClientId y ClientSecret de Google Cloud Console
- Configuración de URIs de redirección autorizadas

**Documentación**: https://docs.microsoft.com/aspnet/core/security/authentication/social/google-logins

---

### Swashbuckle.AspNetCore (6.5.0)

**Propósito**: Generación automática de documentación OpenAPI (Swagger) para APIs.

**Uso en el proyecto**:
- Configuración en `PresentacionApi/Program.cs`
- UI de Swagger disponible en `/swagger`

**Configuración**:
```csharp
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

**Acceso**:
- URL: `https://localhost:7xxx/swagger`
- Permite probar endpoints directamente desde el navegador

**Ventajas**:
- Documentación automática
- Interfaz interactiva para testing
- Generación de especificación OpenAPI
- Cliente HTTP integrado

**Documentación**: https://github.com/domaindrivendev/Swashbuckle.AspNetCore

---

### Microsoft.Extensions.Http (6.0.1)

**Propósito**: HttpClientFactory para gestión eficiente de HttpClient.

**Uso en el proyecto**:
- Configuración en `Aplicacion/AddIoC.cs`
- Cliente HTTP para API CIMA

**Configuración**:
```csharp
services.AddHttpClient<ICimaHttpClient, CimaHttpClient>(client =>
{
    client.BaseAddress = new Uri("https://cima.aemps.es/cima/rest/");
    client.Timeout = TimeSpan.FromSeconds(30);
});
```

**Ventajas**:
- Evita agotamiento de sockets
- Gestión automática de ciclo de vida
- Configuración centralizada
- Soporte para políticas de retry (con Polly)

**Documentación**: https://docs.microsoft.com/aspnet/core/fundamentals/http-requests

---

### Microsoft.VisualStudio.Web.CodeGeneration.Design (6.0.18)

**Propósito**: Scaffolding de controladores, vistas y áreas en ASP.NET Core.

**Uso**:
- Generación rápida de CRUD
- Plantillas predefinidas

**Comandos útiles**:
```bash
# Scaffold controlador con vistas
dotnet aspnet-codegenerator controller -name ProductController -m Product -dc ApplicationDbContext --relativeFolderPath Controllers --useDefaultLayout

# Scaffold área
dotnet aspnet-codegenerator area Admin
```

**Ventajas**:
- Acelera desarrollo inicial
- Código consistente
- Buenas prácticas por defecto

**Documentación**: https://docs.microsoft.com/aspnet/core/fundamentals/tools/dotnet-aspnet-codegenerator

---

## Versiones y Compatibilidad

### Matriz de Compatibilidad

| Librería | Versión Actual | Versión .NET | Última Versión Estable |
|----------|----------------|--------------|------------------------|
| .NET | 6.0 | - | 8.0 (LTS) |
| AutoMapper | 13.0.1 | .NET 6+ | 13.0.x |
| Entity Framework Core | 6.0.5 | .NET 6 | 8.0.x |
| FluentValidation | 11.3.0 | .NET 6+ | 11.9.x |
| Newtonsoft.Json | 13.0.3 | .NET Standard 2.0+ | 13.0.3 |
| Swashbuckle | 6.5.0 | .NET 6+ | 6.5.x |

### Consideraciones de Actualización

**Actualizaciones seguras** (manteniendo .NET 6):
- AutoMapper: Puede actualizarse a 13.0.x
- FluentValidation: Puede actualizarse a 11.9.x
- Entity Framework Core: Se recomienda mantener en 6.0.x para estabilidad

**Actualización a .NET 8** (recomendado para nuevos desarrollos):
- Requiere actualizar Entity Framework Core a 8.0.x
- Compatibilidad total con librerías actuales
- Mejoras de rendimiento significativas

---

## Justificación de Elecciones

### ¿Por qué .NET 6?

**Ventajas**:
- LTS (Long Term Support) con soporte hasta 2024
- Balance entre estabilidad y características modernas
- Amplio ecosistema de librerías
- Rendimiento mejorado sobre versiones anteriores

**Alternativas consideradas**:
- .NET Framework 4.8: Obsoleto, solo Windows
- .NET 5: Sin LTS, ya fuera de soporte
- .NET 7/8: Más modernas, pero requieren migración

---

### ¿Por qué AutoMapper?

**Ventajas**:
- Estándar de la industria
- Reduce código repetitivo
- Fácil mantenimiento

**Alternativas consideradas**:
- Mapeo manual: Más control, pero tedioso
- Mapster: Más rápido, pero menos maduro

---

### ¿Por qué Entity Framework Core?

**Ventajas**:
- ORM oficial de Microsoft
- Excelente integración con .NET
- Migraciones robustas
- LINQ to Entities

**Alternativas consideradas**:
- Dapper: Más rápido, pero menos características
- ADO.NET: Máximo control, mucho más trabajo
- NHibernate: Maduro, pero más complejo

---

### ¿Por qué FluentValidation?

**Ventajas**:
- Sintaxis expresiva
- Separación de responsabilidades
- Reutilización de reglas

**Alternativas consideradas**:
- Data Annotations: Más simple, menos flexible
- Validación manual: Más control, mucho trabajo

---

### ¿Por qué Newtonsoft.Json?

**Ventajas**:
- Muy maduro y estable
- Ampliamente utilizado
- Características avanzadas

**Alternativas consideradas**:
- System.Text.Json: Más rápido, menos características
- ServiceStack.Text: Alto rendimiento, sintaxis diferente

---

## Recursos y Documentación

### Documentación Oficial

- **.NET 6**: https://docs.microsoft.com/dotnet/core/whats-new/dotnet-6
- **ASP.NET Core**: https://docs.microsoft.com/aspnet/core/
- **Entity Framework Core**: https://docs.microsoft.com/ef/core/
- **AutoMapper**: https://docs.automapper.org/
- **FluentValidation**: https://docs.fluentvalidation.net/

### Tutoriales y Recursos

- **Microsoft Learn**: https://learn.microsoft.com/
- **.NET Blog**: https://devblogs.microsoft.com/dotnet/
- **Entity Framework Tutorial**: https://www.entityframeworktutorial.net/

### Gestión de Paquetes

**NuGet.org**: https://www.nuget.org/

**Comandos útiles**:
```bash
# Listar paquetes instalados
dotnet list package

# Buscar actualizaciones
dotnet list package --outdated

# Actualizar paquete
dotnet add package NombrePaquete --version x.x.x

# Restaurar paquetes
dotnet restore
```

---

## Configuración de NuGet

**Archivo**: `nuget.config`

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3" />
  </packageSources>
</configuration>
```

---

## Resumen de Dependencias Externas

### APIs Externas

**API CIMA** (Centro de Información de Medicamentos de la AEMPS):
- URL: https://cima.aemps.es/cima/rest/
- Propósito: Búsqueda de medicamentos autorizados en España
- Autenticación: No requiere
- Cliente: `CimaHttpClient`

**Google OAuth**:
- Propósito: Autenticación de usuarios
- Requiere: ClientId y ClientSecret de Google Cloud Console

---

## Mantenimiento y Actualizaciones

### Política de Actualización

1. **Parches de seguridad**: Aplicar inmediatamente
2. **Versiones menores**: Evaluar trimestralmente
3. **Versiones mayores**: Planificar con anticipación

### Proceso de Actualización

1. Revisar changelog de la librería
2. Actualizar en entorno de desarrollo
3. Ejecutar pruebas
4. Actualizar en staging
5. Validar funcionalidad
6. Desplegar a producción

### Herramientas de Análisis

**Dependabot** (GitHub): Alertas automáticas de vulnerabilidades

**dotnet outdated**: Verificar paquetes desactualizados
```bash
dotnet tool install --global dotnet-outdated-tool
dotnet outdated
```

---

**Última actualización**: 2025-10-20
