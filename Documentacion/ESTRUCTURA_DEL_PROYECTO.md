# Estructura del Proyecto - Sistema de Alarma Médica

## Índice
1. [Organización de la Solución](#organización-de-la-solución)
2. [Estructura por Proyecto](#estructura-por-proyecto)
3. [Convenciones de Nombres](#convenciones-de-nombres)
4. [Organización de Archivos](#organización-de-archivos)

---

## Organización de la Solución

La solución `SistemaAlarmaMedica.sln` está organizada en **carpetas de solución** que agrupan los proyectos por responsabilidad:

```
SistemaAlarmaMedica.sln
│
├── 1.1.PresentacionWeb/          # Carpeta de solución
│   └── Presentacion              # Proyecto ASP.NET Core MVC
│
├── 1.2.PresentacionApi/          # Carpeta de solución
│   └── PresentacionApi           # Proyecto ASP.NET Core Web API
│
├── 2.Dominio/                    # Carpeta de solución
│   └── Dominio                   # Proyecto de lógica de negocio
│
├── 3.Infraestructura/            # Carpeta de solución
│   └── Infraestructura           # Proyecto de acceso a datos
│
├── 4.Aplicacion/                 # Carpeta de solución
│   └── Aplicacion                # Proyecto de configuración
│
└── DefaultDI/                    # Proyecto auxiliar
```

**Numeración**: Los números (1.1, 1.2, 2, 3, 4) indican el orden de capas desde la UI hasta los datos.

---

## Estructura por Proyecto

### 1. Dominio

**Tipo de Proyecto**: Class Library (.NET 6.0)
**Responsabilidad**: Lógica de negocio, entidades, servicios de dominio, interfaces

```
Dominio/
│
├── Application/                          # Capa de aplicación del dominio
│   ├── DTOs/                            # Data Transfer Objects
│   │   ├── EspecialidadDto.cs           # DTO para especialidades médicas
│   │   ├── FarmacoDto.cs                # DTO para fármacos/medicamentos
│   │   ├── LineaOrdenMedicaDto.cs       # DTO para líneas de receta
│   │   ├── MedicoDto.cs                 # DTO para médicos
│   │   ├── ObraSocialDto.cs             # DTO para obras sociales
│   │   ├── OrdenMedicaDto.cs            # DTO para órdenes médicas/recetas
│   │   ├── PacienteDto.cs               # DTO para pacientes
│   │   ├── TipoUsuarioDto.cs            # DTO para tipos de usuario
│   │   └── UsuarioDto.cs                # DTO para usuarios del sistema
│   │
│   └── Mappings/
│       └── AutoMapperProfile.cs         # Configuración de mapeos AutoMapper
│
├── Core/                                # Componentes centrales
│   └── Genericos/
│       └── IRepository.cs               # Interfaz genérica de repositorio
│
├── Entidades/                           # Entidades del dominio (modelos)
│   ├── Especialidad.cs                  # Especialidades médicas (ej: Cardiología)
│   ├── Farmaco.cs                       # Medicamentos/fármacos
│   ├── LineaOrdenMedica.cs              # Detalle de medicamentos en una receta
│   ├── Medico.cs                        # Médicos del sistema
│   ├── ObraSocial.cs                    # Obras sociales/seguros
│   ├── OrdenMedica.cs                   # Recetas médicas
│   ├── Paciente.cs                      # Pacientes registrados
│   ├── TipoUsuario.cs                   # Roles de usuario (Admin, Usuario, etc.)
│   └── Usuario.cs                       # Usuarios del sistema
│
├── Servicios/                           # Servicios de dominio (lógica de negocio)
│   │
│   ├── Farmacos/
│   │   ├── FarmacosService.cs           # Servicio de búsqueda de fármacos
│   │   ├── IFarmacosRepository.cs       # Interfaz de repositorio de fármacos
│   │   └── IFarmacosService.cs          # Interfaz de servicio de fármacos
│   │
│   ├── Medicos/
│   │   ├── IMedicoRepository.cs         # Interfaz de repositorio de médicos
│   │   ├── IMedicoService.cs            # Interfaz de servicio de médicos
│   │   └── MedicoService.cs             # Servicio de gestión de médicos
│   │
│   ├── OrdenesMedicas/
│   │   ├── ILineaOrdenMedicaRepository.cs   # Interfaz de repositorio de líneas
│   │   ├── IOrdenMedicaRepository.cs        # Interfaz de repositorio de órdenes
│   │   ├── IOrdenMedicaService.cs           # Interfaz de servicio de órdenes
│   │   └── OrdenMedicaService.cs            # Servicio de gestión de recetas
│   │
│   ├── Pacientes/
│   │   ├── IPacienteRepository.cs       # Interfaz de repositorio de pacientes
│   │   ├── IPacienteService.cs          # Interfaz de servicio de pacientes
│   │   └── PacienteService.cs           # Servicio de gestión de pacientes
│   │
│   ├── Usuarios/
│   │   ├── IUsuarioRepository.cs        # Interfaz de repositorio de usuarios
│   │   ├── IUsuarioService.cs           # Interfaz de servicio de usuarios
│   │   └── UsuarioService.cs            # Servicio de autenticación y gestión
│   │
│   └── Utils/                           # Utilidades y servicios auxiliares
│       ├── ApiResponse.cs               # Modelo de respuesta de API CIMA
│       ├── CimaHttpClient.cs            # Cliente HTTP para API CIMA
│       └── ICimaHttpClient.cs           # Interfaz del cliente HTTP
│
├── Shared/                              # Código compartido
│   ├── ServiceError.cs                  # Modelo de errores de servicio
│   └── ServiceResponse.cs               # Respuesta estándar de servicios
│
└── Dominio.csproj                       # Archivo de proyecto
```

**Puntos Clave**:
- **Entidades**: Modelos de negocio sin dependencias de infraestructura
- **Servicios**: Lógica de negocio compleja (validaciones, reglas, coordinación)
- **DTOs**: Objetos para transferir datos sin exponer entidades
- **Interfaces**: Contratos que define el dominio para otros proyectos

---

### 2. Infraestructura

**Tipo de Proyecto**: Class Library (.NET 6.0)
**Responsabilidad**: Persistencia, acceso a datos, implementación de repositorios

```
Infraestructura/
│
├── ContextoBD/                          # Contexto de base de datos
│   ├── AplicacionBDContexto.cs          # DbContext de Entity Framework
│   └── IAplicacionBDContexto.cs         # Interfaz del DbContext
│
├── Genericos/                           # Implementaciones genéricas
│   └── Repository.cs                    # Implementación genérica de IRepository<T>
│
├── Migrations/                          # Migraciones de Entity Framework
│   ├── 20250111213737_PrimeraMigracion.cs
│   ├── 20250111213737_PrimeraMigracion.Designer.cs
│   ├── 20250112013350_SegundaMigracion.cs
│   ├── 20250112013350_SegundaMigracion.Designer.cs
│   ├── 20250112191111_TercerMigracion.cs
│   ├── 20250112191111_TercerMigracion.Designer.cs
│   ├── 20250510050355_CuartaMigracion_LineaOrdenMedica.cs
│   ├── 20250510050355_CuartaMigracion_LineaOrdenMedica.Designer.cs
│   ├── 20250510234111_QuintaMigracion_LineaOrdenMedica.cs
│   ├── 20250510234111_QuintaMigracion_LineaOrdenMedica.Designer.cs
│   ├── 20251015000000_SextaMigracion_GoogleIdEmail.cs
│   └── AplicacionBDContextoModelSnapshot.cs  # Snapshot del modelo actual
│
├── Repositorios/                        # Implementaciones específicas de repositorios
│   ├── LineaOrdenMedicaRepository.cs    # Repositorio de líneas de receta
│   ├── MedicoRepository.cs              # Repositorio de médicos
│   ├── OrdenMedicaRepository.cs         # Repositorio de órdenes médicas
│   ├── PacienteRepository.cs            # Repositorio de pacientes
│   └── UsuarioRepository.cs             # Repositorio de usuarios
│
└── Infraestructura.csproj               # Archivo de proyecto
```

**Puntos Clave**:
- **ContextoBD**: Configuración de Entity Framework, relaciones, constraints
- **Repositorios**: Implementan interfaces del dominio con lógica de acceso a datos
- **Migrations**: Historial de cambios en el esquema de base de datos
- **Genericos**: Repositorio base con operaciones CRUD comunes

---

### 3. Aplicacion

**Tipo de Proyecto**: Class Library (.NET 6.0)
**Responsabilidad**: Configuración de inyección de dependencias

```
Aplicacion/
│
├── AddIoC.cs                            # Configuración de IoC Container
└── Aplicacion.csproj                    # Archivo de proyecto
```

**Contenido de AddIoC.cs**:
- Registro de servicios (Transient, Scoped, Singleton)
- Configuración de AutoMapper
- Configuración de HttpClient para API CIMA
- Registro de repositorios

---

### 4. Presentacion (Web MVC)

**Tipo de Proyecto**: ASP.NET Core Web App (MVC) (.NET 6.0)
**Responsabilidad**: Interfaz web para usuarios finales

```
Presentacion/
│
├── Attributes/                          # Atributos personalizados
│   └── RoleAuthorizationAttribute.cs    # Autorización por roles
│
├── Controllers/                         # Controladores MVC
│   ├── FarmacoController.cs             # CRUD de fármacos
│   ├── HomeController.cs                # Página principal
│   ├── LoginController.cs               # Inicio de sesión
│   ├── MedicoController.cs              # CRUD de médicos
│   ├── OrdenMedicaController.cs         # CRUD de órdenes médicas
│   ├── PacienteController.cs            # CRUD de pacientes
│   ├── RegisterController.cs            # Registro de usuarios
│   └── UsuarioController.cs             # Gestión de usuarios
│
├── Core/                                # Núcleo de la capa de presentación
│   ├── DTOs/                            # DTOs específicos de la presentación
│   │   ├── EspecialidadDto.cs
│   │   ├── FarmacoDto.cs
│   │   ├── LineaOrdenMedicaDto.cs
│   │   ├── MedicoDto.cs
│   │   ├── ObraSocialDto.cs
│   │   ├── OrdenMedicaDto.cs
│   │   ├── PacienteDto.cs
│   │   ├── TipoUsuario.cs
│   │   └── UsuarioDto.cs
│   │
│   ├── Responses/                       # Modelos de respuesta
│   │   ├── ServiceError.cs
│   │   └── ServiceResponse.cs
│   │
│   └── TipoOperacion.cs                 # Enum de tipos de operación (Alta, Baja, Modificación)
│
├── Middleware/                          # Middleware personalizado
│   └── SessionValidationMiddleware.cs   # Validación de sesión activa
│
├── Models/                              # ViewModels
│   ├── CompleteGoogleRegistrationViewModel.cs  # Completar registro con Google
│   ├── ErrorViewModel.cs                # Modelo de error
│   ├── FarmacoViewModel.cs              # ViewModel de fármacos
│   ├── MedicoViewModel.cs               # ViewModel de médicos
│   ├── OrdenMedicaViewModel.cs          # ViewModel de órdenes médicas
│   ├── PacienteViewModel.cs             # ViewModel de pacientes
│   └── UsuarioViewModel.cs              # ViewModel de usuarios
│
├── Properties/
│   └── launchSettings.json              # Configuración de lanzamiento
│
├── Services/                            # Servicios web (consumo de API)
│   ├── FarmacoServiceWeb.cs             # Servicio web de fármacos
│   ├── HttpClientService.cs             # Servicio HTTP base
│   ├── IFarmacoServiceWeb.cs            # Interfaz de servicio de fármacos
│   ├── IMedicoServiceWeb.cs             # Interfaz de servicio de médicos
│   ├── IOrdenMedicaServiceWeb.cs        # Interfaz de servicio de órdenes
│   ├── IPacienteServiceWeb.cs           # Interfaz de servicio de pacientes
│   ├── IUsuarioServiceWeb.cs            # Interfaz de servicio de usuarios
│   ├── MedicoServiceWeb.cs              # Servicio web de médicos
│   ├── OrdenMedicaServiceWeb.cs         # Servicio web de órdenes
│   ├── PacienteServiceWeb.cs            # Servicio web de pacientes
│   └── UsuarioServiceWeb.cs             # Servicio web de usuarios
│
├── Tools/                               # Herramientas y utilidades
│   │
│   ├── QueryBuilder/
│   │   └── QueryStringBuilder.cs        # Constructor de query strings
│   │
│   ├── Serializations/
│   │   └── Serialization.cs             # Utilidades de serialización
│   │
│   └── Validators/                      # Validadores FluentValidation
│       ├── Logic/
│       │   └── LogicsForValidator.cs    # Lógica compartida de validación
│       ├── MedicoValidator.cs           # Validador de médicos
│       ├── OrdenMedicaValidator.cs      # Validador de órdenes médicas
│       ├── PacienteValidator.cs         # Validador de pacientes
│       └── UsuarioValidator.cs          # Validador de usuarios
│
├── Views/                               # Vistas Razor (no listadas por brevedad)
│   ├── Home/
│   ├── Login/
│   ├── Medico/
│   ├── Paciente/
│   ├── OrdenMedica/
│   ├── Shared/
│   └── _ViewImports.cshtml
│
├── wwwroot/                             # Archivos estáticos (CSS, JS, imágenes)
│   ├── css/
│   ├── js/
│   └── lib/
│
├── appsettings.json                     # Configuración de la aplicación
├── appsettings.Development.json         # Configuración de desarrollo
├── Program.cs                           # Punto de entrada
└── Presentacion.csproj                  # Archivo de proyecto
```

**Puntos Clave**:
- **Controllers**: Manejan requests y retornan vistas
- **Services**: Capa adicional que consume PresentacionApi
- **Validators**: Validación de modelos con FluentValidation
- **Middleware**: Validación de sesión en cada request
- **ViewModels**: Modelos específicos para vistas

---

### 5. PresentacionApi (Web API)

**Tipo de Proyecto**: ASP.NET Core Web API (.NET 6.0)
**Responsabilidad**: Exponer endpoints REST

```
PresentacionApi/
│
├── Controllers/                         # Controladores API REST
│   ├── FarmacoController.cs             # Endpoints de fármacos
│   ├── MedicoController.cs              # Endpoints de médicos
│   ├── OrdenMedicaController.cs         # Endpoints de órdenes médicas
│   ├── PacienteController.cs            # Endpoints de pacientes
│   └── UsuarioController.cs             # Endpoints de usuarios
│
├── Properties/
│   └── launchSettings.json              # Configuración de lanzamiento
│
├── Tools/
│   └── FiltersDto/                      # DTOs para filtros de consultas
│       ├── FiltroMedicoDto.cs           # Filtros para búsqueda de médicos
│       ├── FiltroOrdenMedicaDto.cs      # Filtros para búsqueda de órdenes
│       ├── FiltroPacienteDto.cs         # Filtros para búsqueda de pacientes
│       └── FiltroUsuarioDto.cs          # Filtros para búsqueda de usuarios
│
├── appsettings.json                     # Configuración de la API
├── appsettings.Development.json         # Configuración de desarrollo
├── Program.cs                           # Punto de entrada y configuración
└── PresentacionApi.csproj               # Archivo de proyecto
```

**Puntos Clave**:
- **Controllers**: Atributo `[ApiController]`, retornan JSON
- **FiltersDto**: Permiten consultas complejas con múltiples criterios
- **Swagger**: Documentación interactiva en `/swagger`

---

### 6. DefaultDI

**Tipo de Proyecto**: Class Library (.NET 6.0)
**Responsabilidad**: Configuración adicional de inyección de dependencias (actualmente vacío)

```
DefaultDI/
│
└── DefaultDI.csproj                     # Archivo de proyecto
```

---

## Convenciones de Nombres

### Entidades
- **PascalCase**: `Paciente`, `Medico`, `OrdenMedica`
- **Singular**: Representan una única instancia del concepto

### DTOs
- **PascalCase + Dto**: `PacienteDto`, `MedicoDto`
- **Coinciden con entidades**: Facilita mapeo

### Servicios
- **Interfaz**: `I + Nombre + Service` → `IPacienteService`
- **Implementación**: `Nombre + Service` → `PacienteService`

### Repositorios
- **Interfaz**: `I + Nombre + Repository` → `IPacienteRepository`
- **Implementación**: `Nombre + Repository` → `PacienteRepository`

### Controladores
- **MVC**: `Nombre + Controller` → `PacienteController`
- **API**: Igual que MVC, pero con atributo `[ApiController]`

### ViewModels
- **Nombre + ViewModel**: `PacienteViewModel`

### Validadores
- **Nombre + Validator**: `PacienteValidator`

### Archivos de Configuración
- **appsettings.json**: Configuración base
- **appsettings.Development.json**: Override para desarrollo
- **appsettings.Production.json**: Override para producción (no incluido en repo)

---

## Organización de Archivos

### Principios de Organización

1. **Agrupación por Funcionalidad**: Los archivos se agrupan por entidad/dominio
   - Ejemplo: Todo lo relacionado con Paciente está junto

2. **Separación por Tipo**: Dentro de cada proyecto, separación por tipo
   - Controllers, Services, Models, etc.

3. **Cohesión**: Archivos relacionados están cerca
   - `IPacienteService.cs` y `PacienteService.cs` en la misma carpeta

4. **Consistencia**: Misma estructura en proyectos similares
   - PresentacionApi y Presentacion tienen estructura análoga

### Ejemplo: Flujo de Paciente

```
Dominio/
├── Entidades/Paciente.cs                # Entidad de dominio
├── Application/DTOs/PacienteDto.cs      # DTO para transferencia
├── Servicios/Pacientes/
│   ├── IPacienteService.cs              # Interfaz de servicio
│   ├── IPacienteRepository.cs           # Interfaz de repositorio
│   └── PacienteService.cs               # Lógica de negocio

Infraestructura/
└── Repositorios/PacienteRepository.cs   # Acceso a datos

PresentacionApi/
├── Controllers/PacienteController.cs    # Endpoints REST
└── Tools/FiltersDto/FiltroPacienteDto.cs

Presentacion/
├── Controllers/PacienteController.cs    # Controlador MVC
├── Models/PacienteViewModel.cs          # ViewModel
├── Services/
│   ├── IPacienteServiceWeb.cs
│   └── PacienteServiceWeb.cs
├── Tools/Validators/PacienteValidator.cs
└── Views/Paciente/                      # Vistas Razor
    ├── Index.cshtml
    ├── Create.cshtml
    ├── Edit.cshtml
    └── Delete.cshtml
```

---

## Archivos de Configuración

### appsettings.json (Estructura base)

```json
{
  "ConnectionStrings": {
    "LocalDbConnection": "Server=...;Database=SistemaAlarmaMedica;..."
  },
  "Authentication": {
    "Google": {
      "ClientId": "...",
      "ClientSecret": "..."
    }
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### launchSettings.json

Define perfiles de ejecución:
- **IIS Express**: Para desarrollo local con IIS
- **Proyecto**: Ejecución directa con Kestrel

---

## Archivos de Proyecto (.csproj)

### Estructura común

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="..." Version="..." />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..." />
  </ItemGroup>
</Project>
```

**PropertyGroup**:
- `TargetFramework`: Versión de .NET
- `Nullable`: Habilita nullable reference types
- `ImplicitUsings`: Using globales automáticos

**PackageReference**: Dependencias de NuGet

**ProjectReference**: Referencias a otros proyectos de la solución

---

## Archivos Especiales

### Program.cs (Minimal Hosting Model)

Punto de entrada de aplicaciones ASP.NET Core 6.0:

```csharp
var builder = WebApplication.CreateBuilder(args);
// Configuración de servicios
builder.Services.AddControllersWithViews();

var app = builder.Build();
// Configuración de middleware
app.UseRouting();
app.MapControllers();

app.Run();
```

### Global Usings (Implícito)

.NET 6 habilita usings globales automáticos para tipos comunes:
- `System`
- `System.Collections.Generic`
- `System.Linq`
- `Microsoft.AspNetCore.Mvc` (en proyectos web)

---

## Patrones de Carpetas

### Patrón por Entidad (Dominio)

```
Servicios/
├── Pacientes/
│   ├── IPacienteService.cs
│   ├── IPacienteRepository.cs
│   └── PacienteService.cs
├── Medicos/
│   ├── IMedicoService.cs
│   ├── IMedicoRepository.cs
│   └── MedicoService.cs
```

**Ventaja**: Todo lo relacionado con una entidad está junto

### Patrón por Tipo (Infraestructura)

```
Infraestructura/
├── ContextoBD/
├── Repositorios/
├── Migrations/
└── Genericos/
```

**Ventaja**: Fácil encontrar todos los repositorios juntos

---

## Navegación Rápida

### Para agregar una nueva entidad:

1. **Dominio/Entidades/**: Crear entidad
2. **Dominio/Application/DTOs/**: Crear DTO
3. **Dominio/Application/Mappings/**: Agregar mapeo
4. **Dominio/Servicios/NuevaEntidad/**: Crear servicio e interfaces
5. **Infraestructura/Repositorios/**: Crear repositorio
6. **Infraestructura/ContextoBD/**: Agregar DbSet y configuración
7. **Aplicacion/AddIoC.cs**: Registrar servicios
8. **PresentacionApi/Controllers/**: Crear controlador API
9. **Presentacion/Controllers/**: Crear controlador MVC
10. **Presentacion/Models/**: Crear ViewModel
11. **Presentacion/Views/NuevaEntidad/**: Crear vistas

---

## Resumen

La estructura del proyecto sigue principios de:
- **Clean Architecture**: Separación clara de responsabilidades
- **Domain-Driven Design**: Dominio en el centro
- **SOLID**: Single Responsibility, Open/Closed, Dependency Inversion
- **Convención sobre Configuración**: Nombres y estructura predecibles

Esta organización facilita:
- **Mantenimiento**: Fácil localizar código relacionado
- **Escalabilidad**: Agregar nuevas entidades es sistemático
- **Colaboración**: Estructura clara para equipos
- **Testing**: Dependencias claras y abstracciones

**Última actualización**: 2025-10-20
