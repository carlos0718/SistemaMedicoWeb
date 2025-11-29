# Sistema de Alarma Médica - Documentación Técnica

## Índice
1. [Visión General](#visión-general)
2. [Arquitectura del Sistema](#arquitectura-del-sistema)
3. [Estructura de la Solución](#estructura-de-la-solución)
4. [Tecnologías y Librerías](#tecnologías-y-librerías)
5. [Componentes Principales](#componentes-principales)
6. [Patrones de Diseño](#patrones-de-diseño)
7. [Base de Datos](#base-de-datos)

---

## Visión General

**Sistema de Alarma Médica** es una aplicación para la gestión de órdenes médicas, pacientes, médicos y fármacos. El sistema permite crear y administrar recetas médicas (órdenes médicas) con sus respectivas líneas de medicamentos.

### Características Principales
- Gestión de pacientes, médicos, usuarios y órdenes médicas
- Integración con API externa CIMA (Agencia Española de Medicamentos) para búsqueda de fármacos
- Autenticación y autorización con soporte para Google OAuth
- Arquitectura basada en capas con separación de responsabilidades
- Doble interfaz: Web MVC y API REST

---

## Arquitectura del Sistema

El proyecto sigue una **Arquitectura en Capas (Layered Architecture)** inspirada en los principios de **Clean Architecture** y **Domain-Driven Design (DDD)**.

### Principios Arquitectónicos
- **Separación de Responsabilidades**: Cada capa tiene responsabilidades claramente definidas
- **Inversión de Dependencias**: Las capas superiores dependen de abstracciones de las capas inferiores
- **Independencia de Frameworks**: La lógica de negocio no depende de frameworks específicos
- **Testabilidad**: Diseño que facilita pruebas unitarias e integración

### Flujo de Dependencias
```
Presentacion/PresentacionApi → Aplicacion → Infraestructura → Dominio
```

**Importante**: El Dominio no depende de ninguna otra capa, manteniendo la lógica de negocio pura.

---

## Estructura de la Solución

La solución está organizada en 6 proyectos principales:

### 1. **Dominio** (Capa de Dominio)
**Responsabilidad**: Contiene la lógica de negocio central, entidades y contratos.

**Estructura**:
```
Dominio/
├── Application/
│   ├── DTOs/                    # Data Transfer Objects
│   │   ├── FarmacoDto.cs
│   │   ├── MedicoDto.cs
│   │   ├── PacienteDto.cs
│   │   ├── OrdenMedicaDto.cs
│   │   ├── LineaOrdenMedicaDto.cs
│   │   ├── ObraSocialDto.cs
│   │   ├── EspecialidadDto.cs
│   │   └── UsuarioDto.cs
│   └── Mappings/
│       └── AutoMapperProfile.cs  # Configuración de mapeos
├── Core/
│   └── Genericos/
│       └── IRepository.cs        # Interfaz genérica de repositorio
├── Entidades/                    # Entidades del dominio
│   ├── Farmaco.cs
│   ├── Medico.cs
│   ├── Paciente.cs
│   ├── OrdenMedica.cs
│   ├── LineaOrdenMedica.cs
│   ├── ObraSocial.cs
│   ├── Especialidad.cs
│   ├── TipoUsuario.cs
│   └── Usuario.cs
├── Servicios/                    # Lógica de negocio
│   ├── Farmacos/
│   │   ├── IFarmacosService.cs
│   │   ├── IFarmacosRepository.cs
│   │   └── FarmacosService.cs
│   ├── Medicos/
│   │   ├── IMedicoService.cs
│   │   ├── IMedicoRepository.cs
│   │   └── MedicoService.cs
│   ├── Pacientes/
│   │   ├── IPacienteService.cs
│   │   ├── IPacienteRepository.cs
│   │   └── PacienteService.cs
│   ├── OrdenesMedicas/
│   │   ├── IOrdenMedicaService.cs
│   │   ├── IOrdenMedicaRepository.cs
│   │   ├── ILineaOrdenMedicaRepository.cs
│   │   └── OrdenMedicaService.cs
│   ├── Usuarios/
│   │   ├── IUsuarioService.cs
│   │   ├── IUsuarioRepository.cs
│   │   └── UsuarioService.cs
│   └── Utils/
│       ├── ICimaHttpClient.cs    # Cliente HTTP para API CIMA
│       ├── CimaHttpClient.cs
│       └── ApiResponse.cs
└── Shared/
    ├── ServiceResponse.cs        # Respuesta estándar de servicios
    └── ServiceError.cs           # Gestión de errores
```

**Dependencias**:
- `AutoMapper` (13.0.1) - Mapeo objeto a objeto
- `Microsoft.EntityFrameworkCore` (6.0.5) - ORM
- `Newtonsoft.Json` (13.0.3) - Serialización JSON

---

### 2. **Infraestructura** (Capa de Acceso a Datos)
**Responsabilidad**: Implementación de repositorios y acceso a base de datos.

**Estructura**:
```
Infraestructura/
├── ContextoBD/
│   ├── AplicacionBDContexto.cs   # DbContext de Entity Framework
│   └── IAplicacionBDContexto.cs
├── Genericos/
│   └── Repository.cs             # Implementación genérica de repositorio
├── Repositorios/                 # Implementaciones específicas
│   ├── MedicoRepository.cs
│   ├── PacienteRepository.cs
│   ├── OrdenMedicaRepository.cs
│   ├── LineaOrdenMedicaRepository.cs
│   └── UsuarioRepository.cs
└── Migrations/                   # Migraciones de Entity Framework
    ├── 20250111213737_PrimeraMigracion.cs
    ├── 20250112013350_SegundaMigracion.cs
    ├── 20250112191111_TercerMigracion.cs
    ├── 20250510050355_CuartaMigracion_LineaOrdenMedica.cs
    ├── 20250510234111_QuintaMigracion_LineaOrdenMedica.cs
    └── 20251015000000_SextaMigracion_GoogleIdEmail.cs
```

**Dependencias**:
- `Microsoft.EntityFrameworkCore` (6.0.5)
- `Microsoft.EntityFrameworkCore.SqlServer` (6.0.5)
- `Microsoft.EntityFrameworkCore.Tools` (6.0.5)
- `Microsoft.EntityFrameworkCore.Design` (6.0.5)
- `Microsoft.Extensions.Configuration` (6.0.1)
- `Microsoft.Extensions.Configuration.Json` (6.0.0)

**Configuración**:
- Utiliza SQL Server como base de datos
- Connection string definida en `appsettings.json` bajo `ConnectionStrings:LocalDbConnection`
- Manejo de transacciones a nivel de DbContext

---

### 3. **Aplicacion** (Capa de Configuración)
**Responsabilidad**: Configuración de Inyección de Dependencias (IoC Container).

**Estructura**:
```
Aplicacion/
└── AddIoC.cs                     # Extensión para registro de servicios
```

**Servicios Registrados**:
```csharp
- AutoMapper
- IAplicacionBDContexto → AplicacionBDContexto
- IRepository<T> → Repository<T>
- IFarmacosService → FarmacosService
- IMedicoService → MedicoService / IMedicoRepository → MedicoRepository
- IPacienteService → PacienteService / IPacienteRepository → PacienteRepository
- IUsuarioService → UsuarioService / IUsuarioRepository → UsuarioRepository
- IOrdenMedicaService → OrdenMedicaService / IOrdenMedicaRepository → OrdenMedicaRepository
- ILineaOrdenMedicaRepository → LineaOrdenMedicaRepository
- ICimaHttpClient → CimaHttpClient (con configuración HttpClient)
```

**Dependencias**:
- `Microsoft.Extensions.DependencyInjection.Abstractions` (6.0.0)
- `Microsoft.Extensions.Http` (6.0.1)
- `Microsoft.EntityFrameworkCore.Design` (6.0.5)

---

### 4. **Presentacion** (Aplicación Web MVC)
**Responsabilidad**: Interfaz de usuario web con ASP.NET Core MVC.

**Estructura**:
```
Presentacion/
├── Controllers/                  # Controladores MVC
│   ├── HomeController.cs
│   ├── LoginController.cs
│   ├── RegisterController.cs
│   ├── MedicoController.cs
│   ├── PacienteController.cs
│   ├── FarmacoController.cs
│   ├── OrdenMedicaController.cs
│   └── UsuarioController.cs
├── Models/                       # ViewModels
│   ├── ErrorViewModel.cs
│   ├── MedicoViewModel.cs
│   ├── FarmacoViewModel.cs
│   ├── PacienteViewModel.cs
│   ├── OrdenMedicaViewModel.cs
│   ├── UsuarioViewModel.cs
│   └── CompleteGoogleRegistrationViewModel.cs
├── Services/                     # Servicios web (consumo de API)
│   ├── HttpClientService.cs
│   ├── FarmacoServiceWeb.cs
│   ├── MedicoServiceWeb.cs
│   ├── PacienteServiceWeb.cs
│   ├── OrdenMedicaServiceWeb.cs
│   └── UsuarioServiceWeb.cs
├── Tools/
│   ├── Validators/               # Validadores con FluentValidation
│   │   ├── MedicoValidator.cs
│   │   ├── PacienteValidator.cs
│   │   ├── OrdenMedicaValidator.cs
│   │   ├── UsuarioValidator.cs
│   │   └── Logic/
│   │       └── LogicsForValidator.cs
│   ├── Serializations/
│   │   └── Serialization.cs
│   └── QueryBuilder/
│       └── QueryStringBuilder.cs
├── Middleware/
│   └── SessionValidationMiddleware.cs  # Validación de sesión
├── Attributes/
│   └── RoleAuthorizationAttribute.cs   # Atributo de autorización por roles
├── Core/
│   ├── DTOs/                     # DTOs para la capa de presentación
│   ├── Responses/
│   │   ├── ServiceResponse.cs
│   │   └── ServiceError.cs
│   └── TipoOperacion.cs
├── Program.cs                    # Punto de entrada y configuración
└── Views/                        # Vistas Razor
```

**Dependencias**:
- `FluentValidation.AspNetCore` (11.3.0)
- `Microsoft.AspNetCore.Authentication.Google` (6.0.0)
- `Microsoft.EntityFrameworkCore.Design` (6.0.5)
- `Microsoft.VisualStudio.Web.CodeGeneration.Design` (6.0.18)

**Configuración de Autenticación**:
- Sistema híbrido: Cookies + Google OAuth
- Soporte para registro e inicio de sesión con Google
- Manejo de sesiones con timeout de 30 minutos
- Middleware personalizado para validación de sesión

---

### 5. **PresentacionApi** (API REST)
**Responsabilidad**: Exponer endpoints REST para consumo externo.

**Estructura**:
```
PresentacionApi/
├── Controllers/                  # Controladores API
│   ├── FarmacoController.cs
│   ├── MedicoController.cs
│   ├── PacienteController.cs
│   ├── OrdenMedicaController.cs
│   └── UsuarioController.cs
├── Tools/
│   └── FiltersDto/               # DTOs para filtros de consulta
│       ├── FiltroMedicoDto.cs
│       ├── FiltroPacienteDto.cs
│       ├── FiltroOrdenMedicaDto.cs
│       └── FiltroUsuarioDto.cs
└── Program.cs                    # Configuración API
```

**Dependencias**:
- `Swashbuckle.AspNetCore` (6.5.0) - Documentación Swagger/OpenAPI
- `Microsoft.EntityFrameworkCore.Design` (6.0.5)

**Características**:
- Documentación automática con Swagger
- Endpoints RESTful
- Integración con la capa de Aplicación para acceso a servicios

---

### 6. **DefaultDI** (Proyecto auxiliar)
**Responsabilidad**: Proyecto de configuración adicional para inyección de dependencias.

**Estructura**:
```
DefaultDI/
└── (Actualmente vacío, solo configuración base)
```

---

## Tecnologías y Librerías

### Framework Base
- **.NET 6.0**: Framework de desarrollo
- **C# 10**: Lenguaje de programación
- **ASP.NET Core 6.0**: Framework web

### ORM y Base de Datos
- **Entity Framework Core 6.0.5**: ORM
- **SQL Server**: Base de datos relacional
- **Entity Framework Core Tools**: Migraciones y scaffolding

### Mapeo y Validación
- **AutoMapper 13.0.1**: Mapeo objeto a objeto
- **FluentValidation.AspNetCore 11.3.0**: Validación de modelos

### Autenticación y Autorización
- **Microsoft.AspNetCore.Authentication.Google 6.0.0**: OAuth con Google
- **Cookie Authentication**: Autenticación basada en cookies

### Documentación API
- **Swashbuckle.AspNetCore 6.5.0**: Generación de documentación OpenAPI/Swagger

### Serialización
- **Newtonsoft.Json 13.0.3**: Serialización/deserialización JSON

### Inyección de Dependencias
- **Microsoft.Extensions.DependencyInjection.Abstractions 6.0.0**
- **Microsoft.Extensions.Http 6.0.1**: HttpClientFactory

### Generación de Código
- **Microsoft.VisualStudio.Web.CodeGeneration.Design 6.0.18**: Scaffolding

---

## Componentes Principales

### 1. Entidades del Dominio

#### **Paciente**
Representa a los pacientes del sistema.
- `PacienteId` (int): Identificador único
- `Documento` (string): DNI/documento
- `Nombre`, `Apellido` (string)
- `FechaNacimiento` (DateTime)
- `Direccion`, `Telefono`, `Email` (string)
- `ObraSocial` (ObraSocial): Obra social del paciente

#### **Medico**
Representa a los médicos autorizados.
- `MedicoId` (int): Identificador único
- `Nombre`, `Apellido` (string)
- `Matricula` (string): Matrícula profesional
- `EspecialidadId` (int): FK a Especialidad
- `Especialidad` (Especialidad)

#### **OrdenMedica**
Representa una receta médica.
- `OrdenMedicaId` (int): Identificador único
- `Fecha` (DateTime): Fecha de emisión
- `PacienteId` (int): FK a Paciente
- `MedicoId` (int): FK a Medico
- `Paciente`, `Medico`: Navegación
- `LineaOrdenMedica` (List<LineaOrdenMedica>): Medicamentos recetados
- `EntregadaAlPaciente` (bool): Estado de entrega

#### **LineaOrdenMedica**
Representa un medicamento dentro de una orden médica.
- `LineaOrdenMedicaId` (int): Identificador único
- `OrdenMedicaId` (int): FK a OrdenMedica
- `NumeroCima` (string): Código del medicamento en CIMA
- `NombreComercial`, `FormaFarmaceutica`, `DosisYUnidad` (string)
- `Cantidad` (int): Cantidad recetada
- `UnicaAplicacion` (bool): Si es de aplicación única
- `OrdenMedica`: Navegación

#### **Usuario**
Representa usuarios del sistema.
- `UsuarioId` (int): Identificador único
- `Nombre` (string): Nombre de usuario
- `Contrasena` (string): Hash de contraseña
- `GoogleId`, `Email` (string): Para autenticación con Google
- `TipoUsuarioId` (int): FK a TipoUsuario (rol)
- `Activo` (bool): Estado de la cuenta

#### **Especialidad**
Especialidades médicas.
- `EspecialidadId` (int)
- `Nombre` (string)

#### **ObraSocial**
Obras sociales disponibles.
- `ObraSocialId` (int)
- `Nombre` (string)

#### **TipoUsuario**
Tipos/roles de usuario.
- `TipoUsuarioId` (int)
- `Nombre` (string)

---

### 2. Servicios de Dominio

#### **FarmacosService**
**Responsabilidad**: Búsqueda de fármacos en la API CIMA.
- `BuscarFarmacoPorNombre(string nombre)`: Busca medicamentos por nombre
- Utiliza `ICimaHttpClient` para consumir API externa

#### **MedicoService**
**Responsabilidad**: Gestión de médicos.
- CRUD completo de médicos
- Validación de datos
- Manejo de especialidades

#### **PacienteService**
**Responsabilidad**: Gestión de pacientes.
- CRUD completo de pacientes
- Validación de documentos
- Gestión de obras sociales

#### **OrdenMedicaService**
**Responsabilidad**: Gestión de órdenes médicas.
- Creación y modificación de órdenes médicas
- Gestión de líneas de medicamentos (LineaOrdenMedica)
- Validación de permisos (médico autorizado)
- Consultas con filtros complejos

#### **UsuarioService**
**Responsabilidad**: Gestión de usuarios y autenticación.
- Registro de usuarios
- Inicio de sesión
- Autenticación con Google OAuth
- Validación de credenciales

---

### 3. Repositorios

Todos los repositorios implementan el patrón Repository con operaciones CRUD genéricas:

- **IRepository<T>**: Interfaz genérica base
- **Repository<T>**: Implementación genérica en Infraestructura

**Repositorios específicos**:
- `IMedicoRepository` / `MedicoRepository`
- `IPacienteRepository` / `PacienteRepository`
- `IOrdenMedicaRepository` / `OrdenMedicaRepository`
- `ILineaOrdenMedicaRepository` / `LineaOrdenMedicaRepository`
- `IUsuarioRepository` / `UsuarioRepository`

---

### 4. DTOs (Data Transfer Objects)

Los DTOs se utilizan para transferir datos entre capas sin exponer entidades directamente.

**Mapeos con AutoMapper** (`AutoMapperProfile.cs`):
```csharp
Farmaco ↔ FarmacoDto
Medico ↔ MedicoDto
Paciente ↔ PacienteDto
OrdenMedica ↔ OrdenMedicaDto
LineaOrdenMedica ↔ LineaOrdenMedicaDto
Usuario ↔ UsuarioDto
```

---

## Patrones de Diseño

### 1. **Repository Pattern**
Abstracción del acceso a datos con interfaz genérica `IRepository<T>`.

**Beneficios**:
- Desacoplamiento de lógica de negocio y acceso a datos
- Facilita pruebas unitarias (mocking)
- Centraliza operaciones de datos

### 2. **Dependency Injection (DI)**
Configurado en `AddIoC.cs` con lifetimes:
- `Scoped`: DbContext, repositorios
- `Transient`: Servicios, HttpClient

**Beneficios**:
- Bajo acoplamiento
- Facilita testeo
- Flexibilidad en configuración

### 3. **Service Layer Pattern**
Servicios de dominio encapsulan lógica de negocio compleja.

**Beneficios**:
- Reutilización de lógica
- Separación de responsabilidades
- Transacciones y reglas de negocio centralizadas

### 4. **DTO Pattern**
Transferencia de datos sin exponer entidades del dominio.

**Beneficios**:
- Seguridad (no expone estructura interna)
- Optimización de datos transferidos
- Versionado de API

### 5. **Unit of Work (implícito)**
Implementado a través de `DbContext` con manejo de transacciones.

**Métodos**:
- `SaveChangesAsync()`
- `CommitTransactionAsync()`
- `RollbackTransaction()`

### 6. **Middleware Pattern**
`SessionValidationMiddleware` para validación de sesión en cada request.

### 7. **Factory Pattern**
`HttpClientFactory` para gestión de HttpClient en `ICimaHttpClient`.

---

## Base de Datos

### Modelo de Datos

**Tablas principales**:
1. `Usuarios` - Usuarios del sistema
2. `Pacientes` - Pacientes registrados
3. `Medicos` - Médicos autorizados
4. `Especialidades` - Especialidades médicas
5. `ObrasSociales` - Obras sociales
6. `TiposUsuario` - Roles de usuario
7. `OrdenMedicas` - Órdenes médicas (recetas)
8. `LineaOrdenMedicas` - Detalle de medicamentos por orden

### Relaciones
- `Medicos` → `Especialidades` (N:1)
- `Pacientes` → `ObrasSociales` (N:1)
- `Usuarios` → `TiposUsuario` (N:1)
- `OrdenMedicas` → `Pacientes` (N:1, Restrict)
- `OrdenMedicas` → `Medicos` (N:1, Restrict)
- `OrdenMedicas` → `LineaOrdenMedicas` (1:N, Cascade)

### Migraciones
Las migraciones se encuentran en `Infraestructura/Migrations/`:
1. **PrimeraMigracion** (2025-01-11): Creación de estructura inicial
2. **SegundaMigracion** (2025-01-12): Ajustes en relaciones
3. **TerceraMigracion** (2025-01-12): Modificaciones adicionales
4. **CuartaMigracion** (2025-05-10): Mejoras en LineaOrdenMedica
5. **QuintaMigracion** (2025-05-10): Ajustes en LineaOrdenMedica
6. **SextaMigracion** (2025-10-15): Agregado de GoogleId y Email para OAuth

### Connection String
Configurada en `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "LocalDbConnection": "Server=...;Database=SistemaAlarmaMedica;..."
  }
}
```

---

## Integración Externa

### API CIMA (Centro de Información de Medicamentos)
**URL Base**: `https://cima.aemps.es/cima/rest/`

**Cliente**: `CimaHttpClient` implementa `ICimaHttpClient`

**Configuración**:
- BaseAddress configurada en AddIoC
- Timeout: 30 segundos
- Utiliza HttpClientFactory para gestión eficiente

**Uso**: Búsqueda de medicamentos por nombre comercial o principio activo.

---

## Configuración de Autenticación

### Esquema Híbrido
1. **Cookies**: Autenticación tradicional con usuario/contraseña
2. **Google OAuth**: Autenticación con cuenta de Google

### Configuración (Presentacion/Program.cs)
```csharp
- DefaultScheme: "Cookies"
- LoginPath: "/Login/Index"
- LogoutPath: "/Login/Logout"
- Google ClientId/Secret: Configurado en appsettings.json
- CallbackPath: "/signin-google"
- Scopes: openid, profile, email
```

### Flujo de Google OAuth
1. Usuario hace clic en "Iniciar sesión con Google"
2. Redirección a Google para autenticación
3. Callback a `/signin-google`
4. Si es usuario nuevo, se solicita completar registro
5. Creación de sesión y cookie

---

## Validación

### FluentValidation
Utilizado en la capa de Presentacion para validar ViewModels:

- `MedicoValidator`: Valida datos de médicos
- `PacienteValidator`: Valida datos de pacientes
- `OrdenMedicaValidator`: Valida órdenes médicas
- `UsuarioValidator`: Valida registro de usuarios

**Lógica Compartida**: `LogicsForValidator.cs` contiene validaciones reutilizables.

---

## Configuración del Proyecto

### appsettings.json (Ejemplo)
```json
{
  "ConnectionStrings": {
    "LocalDbConnection": "Server=localhost;Database=SistemaAlarmaMedica;..."
  },
  "Authentication": {
    "Google": {
      "ClientId": "tu-client-id.apps.googleusercontent.com",
      "ClientSecret": "tu-client-secret"
    }
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

### nuget.config
Configuración de fuentes de paquetes NuGet.

---

## Guías de Desarrollo

### Agregar una Nueva Entidad
1. Crear clase de entidad en `Dominio/Entidades/`
2. Crear DTO en `Dominio/Application/DTOs/`
3. Agregar mapeo en `AutoMapperProfile.cs`
4. Definir interfaz de servicio en `Dominio/Servicios/`
5. Implementar servicio
6. Crear repositorio en `Infraestructura/Repositorios/`
7. Agregar DbSet en `AplicacionBDContexto.cs`
8. Configurar modelo en `OnModelCreating`
9. Registrar en `AddIoC.cs`
10. Crear migración: `dotnet ef migrations add NombreMigracion --project Infraestructura`
11. Aplicar migración: `dotnet ef database update --project Infraestructura`

### Ejecutar el Proyecto

#### Presentacion (Web MVC)
```bash
cd Presentacion
dotnet run
```
Acceder a: `https://localhost:7xxx`

#### PresentacionApi (API REST)
```bash
cd PresentacionApi
dotnet run
```
Acceder a Swagger: `https://localhost:7xxx/swagger`

---

## Estructura de Archivos de Solución

```
SistemaAlarmaMedica.sln
├── 1.1.PresentacionWeb/
│   └── Presentacion.csproj
├── 1.2.PresentacionApi/
│   └── PresentacionApi.csproj
├── 2.Dominio/
│   └── Dominio.csproj
├── 3.Infraestructura/
│   └── Infraestructura.csproj
├── 4.Aplicacion/
│   └── Aplicacion.csproj
└── DefaultDI/
    └── DefaultDI.csproj
```

---

## Consideraciones de Seguridad

1. **Contraseñas**: Deben hashearse antes de almacenar (implementar en UsuarioService)
2. **Autorización por Roles**: Implementada con `RoleAuthorizationAttribute`
3. **Validación de Sesión**: Middleware personalizado valida sesiones activas
4. **Google OAuth**: Tokens almacenados de forma segura
5. **Configuración Sensible**: ClientId/Secret en appsettings.json (no versionar con datos reales)

---

## Próximas Mejoras Sugeridas

1. Implementar hashing de contraseñas (bcrypt/PBKDF2)
2. Agregar logging estructurado (Serilog)
3. Implementar caching (Redis/MemoryCache)
4. Agregar pruebas unitarias e integración
5. Implementar paginación en consultas
6. Agregar manejo centralizado de excepciones
7. Implementar versionado de API
8. Agregar auditoría de cambios (CreatedBy, ModifiedBy)
9. Implementar soft delete en entidades
10. Agregar documentación XML para Swagger

---

## Contacto y Soporte

Para más información sobre el proyecto, consultar con el equipo de desarrollo.

**Última actualización**: 2025-10-20
