# Arquitectura del Sistema - Sistema de Alarma Médica

## Visión Arquitectónica

El Sistema de Alarma Médica implementa una **Arquitectura en Capas (Layered Architecture)** inspirada en los principios de **Clean Architecture** y **Domain-Driven Design (DDD)**. Esta arquitectura garantiza la separación de responsabilidades, facilita el mantenimiento y permite la evolución del sistema de manera ordenada.

---

## Principios Arquitectónicos Aplicados

### 1. Separación de Responsabilidades (Separation of Concerns)
Cada capa tiene responsabilidades claramente definidas y no se mezclan entre sí:
- **Dominio**: Lógica de negocio pura
- **Infraestructura**: Persistencia y acceso a datos
- **Aplicacion**: Configuración y orquestación
- **Presentacion/PresentacionApi**: Interfaces de usuario

### 2. Inversión de Dependencias (Dependency Inversion)
Las capas superiores dependen de abstracciones (interfaces) de las capas inferiores, no de implementaciones concretas:
```
Dominio define interfaces → Infraestructura las implementa
Aplicacion configura → Presentacion consume
```

### 3. Independencia de Frameworks
La lógica de negocio (Dominio) no depende de frameworks específicos como Entity Framework, ASP.NET, etc.

### 4. Testabilidad
El diseño facilita la creación de pruebas unitarias mediante:
- Uso de interfaces
- Inyección de dependencias
- Separación clara de responsabilidades

---

## Diagrama de Capas

```
┌─────────────────────────────────────────────────────────────┐
│                     CAPA DE PRESENTACIÓN                    │
│  ┌──────────────────────┐    ┌─────────────────────────┐   │
│  │   Presentacion       │    │   PresentacionApi       │   │
│  │   (ASP.NET MVC)      │    │   (Web API REST)        │   │
│  └──────────────────────┘    └─────────────────────────┘   │
└───────────────────┬──────────────────┬──────────────────────┘
                    │                  │
                    ▼                  ▼
┌─────────────────────────────────────────────────────────────┐
│                    CAPA DE APLICACIÓN                       │
│  ┌────────────────────────────────────────────────────┐     │
│  │            Aplicacion (AddIoC)                     │     │
│  │  • Configuración de Inyección de Dependencias      │     │
│  │  • Registro de servicios                           │     │
│  │  • Configuración de infraestructura                │     │
│  └────────────────────────────────────────────────────┘     │
└───────────────────────────┬─────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                      CAPA DE DOMINIO                        │
│  ┌────────────────────────────────────────────────────┐     │
│  │  • Entidades (Paciente, Medico, OrdenMedica, etc.) │     │
│  │  • Servicios de Dominio (Business Logic)           │     │
│  │  • Interfaces de Repositorio                       │     │
│  │  • DTOs y Mapeos                                   │     │
│  │  • Reglas de Negocio                               │     │
│  └────────────────────────────────────────────────────┘     │
└───────────────────────────┬─────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                  CAPA DE INFRAESTRUCTURA                    │
│  ┌────────────────────────────────────────────────────┐     │
│  │  • Implementación de Repositorios                  │     │
│  │  • DbContext (Entity Framework)                    │     │
│  │  • Migraciones de Base de Datos                    │     │
│  │  • Acceso a APIs Externas                          │     │
│  └────────────────────────────────────────────────────┘     │
└───────────────────────────┬─────────────────────────────────┘
                            │
                            ▼
                   ┌────────────────┐
                   │   SQL Server   │
                   └────────────────┘
```

---

## Descripción de Capas

### 1. Capa de Dominio (Dominio)

**Ubicación**: Proyecto `Dominio`

**Responsabilidad**: Contiene la lógica de negocio central, independiente de cualquier framework o tecnología de infraestructura.

**Componentes**:

#### a) Entidades (Entidades/)
Representan los conceptos del dominio del negocio:
- `Paciente`: Personas que reciben atención médica
- `Medico`: Profesionales autorizados para emitir órdenes médicas
- `OrdenMedica`: Recetas médicas
- `LineaOrdenMedica`: Medicamentos dentro de una receta
- `Usuario`: Usuarios del sistema
- `Especialidad`, `ObraSocial`, `TipoUsuario`: Entidades de soporte

**Características**:
- Clases POCO (Plain Old CLR Objects)
- Sin dependencias de frameworks
- Propiedades de dominio
- Navegación entre entidades relacionadas

#### b) Servicios de Dominio (Servicios/)
Encapsulan lógica de negocio compleja que no pertenece a una entidad específica:
- `FarmacosService`: Búsqueda de medicamentos en API CIMA
- `MedicoService`: Gestión de médicos y validaciones
- `PacienteService`: Gestión de pacientes
- `OrdenMedicaService`: Lógica de creación y gestión de órdenes médicas
- `UsuarioService`: Autenticación y gestión de usuarios

**Patrón**: Service Layer Pattern

#### c) Interfaces de Repositorio (Core/Genericos/ y Servicios/)
Contratos que define el dominio para acceso a datos:
- `IRepository<T>`: Interfaz genérica base
- `IMedicoRepository`, `IPacienteRepository`, etc.: Interfaces específicas

**Beneficio**: El dominio define QUÉ necesita sin saber CÓMO se implementa (Dependency Inversion)

#### d) DTOs y Mapeos (Application/)
- DTOs: Objetos de transferencia de datos
- `AutoMapperProfile`: Configuración de mapeos entre Entidades ↔ DTOs

#### e) Objetos Compartidos (Shared/)
- `ServiceResponse<T>`: Respuesta estándar de operaciones
- `ServiceError`: Manejo de errores

**Dependencias**:
- `AutoMapper`: Para mapeo objeto-objeto
- `Microsoft.EntityFrameworkCore`: Solo para atributos/abstracciones
- `Newtonsoft.Json`: Serialización JSON

---

### 2. Capa de Infraestructura (Infraestructura)

**Ubicación**: Proyecto `Infraestructura`

**Responsabilidad**: Implementa las interfaces definidas por el Dominio y maneja toda la persistencia de datos.

**Componentes**:

#### a) DbContext (ContextoBD/)
- `AplicacionBDContexto`: Implementa `DbContext` de Entity Framework
- `IAplicacionBDContexto`: Interfaz que abstrae el DbContext

**Funciones**:
- Configuración de entidades (`OnModelCreating`)
- Definición de relaciones
- Configuración de restricciones
- Manejo de transacciones

#### b) Repositorios (Repositorios/)
Implementaciones concretas de las interfaces del dominio:
- `Repository<T>`: Implementación genérica de CRUD
- `MedicoRepository`, `PacienteRepository`, etc.: Implementaciones específicas con consultas personalizadas

**Patrón**: Repository Pattern

#### c) Migraciones (Migrations/)
Historial de cambios en la base de datos:
- Control de versiones del esquema
- Permite rollback
- Facilita despliegue en diferentes entornos

**Dependencias**:
- `Microsoft.EntityFrameworkCore.SqlServer`: Proveedor de SQL Server
- `Microsoft.EntityFrameworkCore.Tools`: Herramientas de migración
- `Microsoft.Extensions.Configuration`: Lectura de connection strings

---

### 3. Capa de Aplicación (Aplicacion)

**Ubicación**: Proyecto `Aplicacion`

**Responsabilidad**: Configuración y orquestación del sistema. Conecta todas las capas.

**Componentes**:

#### AddIoC (Inversión de Control)
Método de extensión `AddInversionOfControl` que registra servicios en el contenedor de DI:

```csharp
services.AddAutoMapper(...)
services.AddScoped<IAplicacionBDContexto, AplicacionBDContexto>()
services.AddTransient<IFarmacosService, FarmacosService>()
services.AddTransient<IMedicoRepository, MedicoRepository>()
// etc.
```

**Lifetimes utilizados**:
- `Scoped`: DbContext, repositorios (por request HTTP)
- `Transient`: Servicios de dominio (nueva instancia cada vez)
- `Singleton`: No utilizado actualmente

**Patrón**: Dependency Injection Container

**Dependencias**:
- `Microsoft.Extensions.DependencyInjection.Abstractions`
- `Microsoft.Extensions.Http`: Para HttpClientFactory

---

### 4. Capa de Presentación Web (Presentacion)

**Ubicación**: Proyecto `Presentacion`

**Responsabilidad**: Interfaz de usuario web basada en ASP.NET Core MVC.

**Componentes**:

#### a) Controladores (Controllers/)
Manejan requests HTTP y retornan vistas:
- `HomeController`: Página principal
- `LoginController`, `RegisterController`: Autenticación
- `MedicoController`, `PacienteController`, `FarmacoController`, `OrdenMedicaController`: CRUD de entidades
- `UsuarioController`: Gestión de usuarios

**Patrón**: MVC (Model-View-Controller)

#### b) ViewModels (Models/)
Modelos específicos para las vistas:
- `MedicoViewModel`, `PacienteViewModel`, etc.
- `CompleteGoogleRegistrationViewModel`: Flujo de registro con Google

#### c) Servicios Web (Services/)
Capa adicional que consume la API (PresentacionApi):
- `HttpClientService`: Servicio base HTTP
- `FarmacoServiceWeb`, `MedicoServiceWeb`, etc.: Servicios específicos

**Arquitectura**: La aplicación web consume su propia API REST

#### d) Validadores (Tools/Validators/)
Validación de modelos con FluentValidation:
- `MedicoValidator`, `PacienteValidator`, etc.
- `LogicsForValidator`: Lógica reutilizable

#### e) Middleware (Middleware/)
- `SessionValidationMiddleware`: Valida sesión en cada request

#### f) Atributos (Attributes/)
- `RoleAuthorizationAttribute`: Autorización basada en roles

**Configuración (Program.cs)**:
- Autenticación híbrida (Cookies + Google OAuth)
- Sesión con timeout de 30 minutos
- Registro de servicios web

**Dependencias**:
- `FluentValidation.AspNetCore`: Validación
- `Microsoft.AspNetCore.Authentication.Google`: OAuth

---

### 5. Capa de Presentación API (PresentacionApi)

**Ubicación**: Proyecto `PresentacionApi`

**Responsabilidad**: Exponer endpoints REST para consumo (tanto de Presentacion como clientes externos).

**Componentes**:

#### a) Controladores API (Controllers/)
Endpoints REST:
- `FarmacoController`: GET, POST, PUT, DELETE de fármacos
- `MedicoController`, `PacienteController`, `OrdenMedicaController`, `UsuarioController`

**Convenciones**:
- Atributo `[ApiController]`
- Rutas: `/api/[controller]`
- Respuestas HTTP estándar (200, 201, 400, 404, 500)

#### b) Filtros DTO (Tools/FiltersDto/)
DTOs para consultas con filtros:
- `FiltroMedicoDto`, `FiltroPacienteDto`, etc.

**Configuración (Program.cs)**:
- Swagger/OpenAPI habilitado en Development
- DbContext configurado
- AddInversionOfControl() para DI

**Dependencias**:
- `Swashbuckle.AspNetCore`: Documentación Swagger

---

## Flujo de Datos

### Ejemplo: Crear una Orden Médica

```
1. Usuario completa formulario en la vista (Presentacion)
   └─> View → ViewModel (OrdenMedicaViewModel)

2. Controlador recibe el POST
   └─> OrdenMedicaController.Create(OrdenMedicaViewModel)

3. Validación con FluentValidation
   └─> OrdenMedicaValidator.Validate()

4. Servicio web llama a la API
   └─> OrdenMedicaServiceWeb.Post(OrdenMedicaViewModel)
       └─> HttpClient → PresentacionApi/OrdenMedicaController

5. Controlador API recibe el request
   └─> PresentacionApi.OrdenMedicaController.Post(OrdenMedicaDto)

6. Mapeo DTO → Entidad
   └─> AutoMapper: OrdenMedicaDto → OrdenMedica

7. Servicio de dominio ejecuta lógica de negocio
   └─> OrdenMedicaService.CrearOrdenMedica(OrdenMedica)
       • Valida que el médico esté activo
       • Valida que el paciente exista
       • Procesa líneas de medicamentos

8. Repositorio persiste en BD
   └─> OrdenMedicaRepository.Add(OrdenMedica)
       └─> DbContext.OrdenMedicas.Add()
       └─> DbContext.SaveChangesAsync()

9. SQL Server ejecuta INSERT
   └─> INSERT INTO OrdenMedicas ...

10. Respuesta retorna por las capas
    └─> ServiceResponse<OrdenMedicaDto>
        └─> API retorna 201 Created
            └─> Servicio web retorna al controlador MVC
                └─> Controlador redirige a vista de éxito
```

---

## Patrones de Diseño Aplicados

### 1. Repository Pattern
**Ubicación**: Dominio (interfaces) + Infraestructura (implementación)

**Propósito**: Abstrae el acceso a datos

**Implementación**:
```csharp
// Dominio
public interface IRepository<T> {
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}

// Infraestructura
public class Repository<T> : IRepository<T> where T : class {
    private readonly AplicacionBDContexto _context;
    // Implementación...
}
```

### 2. Service Layer Pattern
**Ubicación**: Dominio/Servicios/

**Propósito**: Encapsular lógica de negocio compleja

**Ejemplo**:
```csharp
public class OrdenMedicaService : IOrdenMedicaService {
    public async Task<ServiceResponse<OrdenMedicaDto>> CrearOrdenMedica(...) {
        // Validaciones de negocio
        // Coordinación entre múltiples repositorios
        // Aplicación de reglas de negocio
    }
}
```

### 3. Dependency Injection
**Ubicación**: Aplicacion/AddIoC.cs

**Propósito**: Invertir dependencias y facilitar testeo

**Configuración**:
```csharp
services.AddTransient<IOrdenMedicaService, OrdenMedicaService>();
services.AddScoped<IOrdenMedicaRepository, OrdenMedicaRepository>();
```

### 4. DTO Pattern
**Ubicación**: Dominio/Application/DTOs/

**Propósito**: Transferir datos entre capas sin exponer entidades

**Mapeo**:
```csharp
CreateMap<OrdenMedica, OrdenMedicaDto>().ReverseMap();
```

### 5. Unit of Work (implícito)
**Ubicación**: Infraestructura/ContextoBD/AplicacionBDContexto.cs

**Propósito**: Coordinar múltiples operaciones en una transacción

**Métodos**:
```csharp
await _context.CommitTransactionAsync();
_context.RollbackTransaction();
```

### 6. Factory Pattern
**Ubicación**: HttpClientFactory para ICimaHttpClient

**Propósito**: Gestionar ciclo de vida de HttpClient

### 7. Middleware Pattern
**Ubicación**: Presentacion/Middleware/SessionValidationMiddleware.cs

**Propósito**: Interceptar requests para validación

### 8. MVC Pattern
**Ubicación**: Presentacion

**Propósito**: Separar lógica de presentación

---

## Dependencias entre Proyectos

```
Presentacion
    └─> Aplicacion
    └─> Dominio
    └─> DefaultDI

PresentacionApi
    └─> Aplicacion
    └─> Dominio
    └─> Infraestructura

Aplicacion
    └─> Infraestructura
    └─> Dominio

Infraestructura
    └─> Dominio

Dominio
    (sin dependencias de otros proyectos)
```

**Regla**: El Dominio nunca depende de otros proyectos, solo de librerías externas mínimas.

---

## Consideraciones Arquitectónicas

### Ventajas de esta Arquitectura

1. **Mantenibilidad**: Cambios en una capa no afectan otras
2. **Testabilidad**: Fácil crear mocks de interfaces
3. **Escalabilidad**: Capas pueden evolucionar independientemente
4. **Reutilización**: Lógica de negocio compartida entre API y Web
5. **Flexibilidad**: Fácil cambiar implementaciones (ej: BD, frameworks)

### Desventajas / Trade-offs

1. **Complejidad inicial**: Más proyectos y abstracciones
2. **Overhead**: Mapeos entre DTOs y entidades
3. **Curva de aprendizaje**: Requiere entender múltiples patrones

### Cuándo es apropiada

Esta arquitectura es ideal para:
- Aplicaciones de mediano/largo plazo
- Equipos de desarrollo múltiples
- Requisitos cambiantes
- Necesidad de múltiples interfaces (Web, API, móvil)

### Cuándo podría ser excesiva

Para proyectos muy pequeños o prototipos rápidos, una arquitectura más simple (ej: MVC tradicional) podría ser suficiente.

---

## Evolución Futura

### Posibles Extensiones

1. **CQRS (Command Query Responsibility Segregation)**
   - Separar modelos de lectura y escritura
   - Optimizar consultas complejas

2. **Event Sourcing**
   - Auditoría completa de cambios
   - Historial de órdenes médicas

3. **Microservicios**
   - Dividir en servicios independientes
   - Ej: Servicio de Pacientes, Servicio de Órdenes

4. **API Gateway**
   - Si se implementan microservicios
   - Unificar acceso a múltiples APIs

5. **Cache Distribuido**
   - Redis para consultas frecuentes
   - Mejorar rendimiento

---

## Diagramas Adicionales

### Diagrama de Componentes

```
┌────────────────────────────────────────────────────────┐
│                    Presentacion                        │
│  ┌──────────┐  ┌──────────┐  ┌───────────────────┐    │
│  │Controllers│─▶│ViewModels│  │ServiceWeb (HTTP)  │    │
│  └──────────┘  └──────────┘  └───────────────────┘    │
└─────────────────────────┬──────────────────────────────┘
                          │ HTTP
                          ▼
┌────────────────────────────────────────────────────────┐
│                   PresentacionApi                      │
│  ┌──────────────┐  ┌──────────────┐                    │
│  │API Controllers│─▶│FilterDtos    │                    │
│  └──────────────┘  └──────────────┘                    │
└─────────────────────────┬──────────────────────────────┘
                          │
                          ▼
┌────────────────────────────────────────────────────────┐
│                      Dominio                           │
│  ┌─────────┐  ┌─────────┐  ┌──────┐  ┌──────────┐     │
│  │Entidades│  │Servicios│  │DTOs  │  │Interfaces│     │
│  └─────────┘  └─────────┘  └──────┘  └──────────┘     │
└─────────────────────────┬──────────────────────────────┘
                          │
                          ▼
┌────────────────────────────────────────────────────────┐
│                  Infraestructura                       │
│  ┌──────────┐  ┌─────────────┐  ┌──────────────┐      │
│  │DbContext │  │Repositorios │  │Migrations    │      │
│  └──────────┘  └─────────────┘  └──────────────┘      │
└─────────────────────────┬──────────────────────────────┘
                          │
                          ▼
                   [ SQL Server ]
```

---

## Resumen

La arquitectura del Sistema de Alarma Médica está diseñada para:

- **Escalabilidad**: Puede crecer en funcionalidad y usuarios
- **Mantenibilidad**: Cambios localizados y controlados
- **Flexibilidad**: Fácil adaptación a nuevos requisitos
- **Calidad**: Facilita testing y código limpio

Es una arquitectura sólida para aplicaciones empresariales de gestión médica que requieren robustez, seguridad y evolución continua.

**Última actualización**: 2025-10-20
