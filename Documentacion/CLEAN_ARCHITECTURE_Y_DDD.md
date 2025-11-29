# Clean Architecture y Domain-Driven Design (DDD)

## Índice
1. [¿Qué es Clean Architecture?](#qué-es-clean-architecture)
2. [¿Qué es Domain-Driven Design (DDD)?](#qué-es-domain-driven-design-ddd)
3. [Aplicación en el Proyecto](#aplicación-en-el-proyecto)
4. [Ejemplos Concretos](#ejemplos-concretos)
5. [Beneficios Obtenidos](#beneficios-obtenidos)

---

## ¿Qué es Clean Architecture?

**Clean Architecture** es un patrón arquitectónico propuesto por Robert C. Martin (Uncle Bob) que organiza el código en capas concéntricas, donde **las dependencias apuntan hacia el centro** (el dominio).

### Principios Fundamentales

#### 1. Independencia de Frameworks
La lógica de negocio no debe depender de frameworks específicos (ASP.NET, Entity Framework, etc.).

**En el proyecto**:
```csharp
// ✅ CORRECTO - Dominio/Entidades/Paciente.cs
public class Paciente
{
    public int PacienteId { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    // Sin atributos de EF Core, sin dependencias de frameworks
}

// ❌ INCORRECTO (no se hace en el proyecto)
[Table("Pacientes")]  // ← Dependencia de EF Core en el dominio
public class Paciente { ... }
```

**Dónde se aplica**: La capa de **Dominio** no tiene referencias a ASP.NET Core ni Entity Framework.

---

#### 2. Independencia de UI
La lógica de negocio puede funcionar sin la interfaz de usuario.

**En el proyecto**:
- La misma lógica de negocio (`PacienteService`, `OrdenMedicaService`) es utilizada por:
  - **Presentacion** (MVC Web)
  - **PresentacionApi** (REST API)
  - Podría usarse por una app móvil o de escritorio sin cambios

**Ubicación**: `Dominio/Servicios/`

```csharp
// Dominio/Servicios/Pacientes/PacienteService.cs
public class PacienteService : IPacienteService
{
    public async Task<ServiceResponse<PacienteDto>> CrearPaciente(PacienteDto dto)
    {
        // Lógica de negocio pura, sin saber si viene de Web, API o móvil
        if (string.IsNullOrEmpty(dto.Documento))
            return ServiceResponse<PacienteDto>.Failure("Documento requerido");

        // ...
    }
}
```

---

#### 3. Independencia de Base de Datos
La lógica de negocio no conoce qué base de datos se usa (SQL Server, MySQL, MongoDB, etc.).

**En el proyecto**:
```csharp
// Dominio define la interfaz (contrato)
// Dominio/Servicios/Pacientes/IPacienteRepository.cs
public interface IPacienteRepository
{
    Task<IEnumerable<Paciente>> ObtenerTodosAsync();
    Task<Paciente> ObtenerPorIdAsync(int id);
    // No menciona SQL, tablas, DbContext...
}

// Infraestructura implementa con SQL Server
// Infraestructura/Repositorios/PacienteRepository.cs
public class PacienteRepository : IPacienteRepository
{
    private readonly AplicacionBDContexto _context;  // ← Aquí sí usa EF Core

    public async Task<IEnumerable<Paciente>> ObtenerTodosAsync()
    {
        return await _context.Pacientes.ToListAsync();  // ← SQL Server específico
    }
}
```

**Dónde se aplica**:
- **Dominio**: Define interfaces (`IPacienteRepository`)
- **Infraestructura**: Implementa con tecnología específica (EF Core + SQL Server)

---

#### 4. Testeable
La arquitectura facilita pruebas unitarias sin bases de datos ni frameworks.

**Ejemplo de test (no implementado, pero posible)**:
```csharp
[Test]
public async Task CrearPaciente_SinDocumento_RetornaError()
{
    // Arrange
    var mockRepo = new Mock<IPacienteRepository>();
    var service = new PacienteService(mockRepo.Object);
    var dto = new PacienteDto { Nombre = "Juan", Documento = "" };

    // Act
    var result = await service.CrearPaciente(dto);

    // Assert
    Assert.IsFalse(result.Success);
    Assert.AreEqual("Documento requerido", result.ErrorMessage);
}
```

**Dónde se aplica**: El uso de interfaces (`IPacienteRepository`, `IPacienteService`) permite crear mocks para testing.

---

#### 5. Regla de Dependencia

**Regla de Oro**: Las dependencias de código fuente solo pueden apuntar **hacia adentro** (hacia el dominio).

```
┌─────────────────────────────────────────┐
│  Presentacion / PresentacionApi         │  ← Capa Externa
│  (Controllers, Views, API)              │
└──────────────┬──────────────────────────┘
               │ Depende de ↓
┌──────────────▼──────────────────────────┐
│  Aplicacion (IoC, Config)               │
└──────────────┬──────────────────────────┘
               │ Depende de ↓
┌──────────────▼──────────────────────────┐
│  Infraestructura (Repositorios, DbContext) │
└──────────────┬──────────────────────────┘
               │ Depende de ↓ (solo interfaces)
┌──────────────▼──────────────────────────┐
│  DOMINIO (Entidades, Servicios)         │  ← Núcleo
│  ❌ NO DEPENDE DE NADA                   │
└─────────────────────────────────────────┘
```

**En el proyecto**:
- `Dominio.csproj` **NO** tiene referencias a `Infraestructura` ni `Presentacion`
- `Infraestructura.csproj` **SÍ** referencia a `Dominio`
- `Presentacion.csproj` **SÍ** referencia a `Aplicacion` y `Dominio`

---

### Las 4 Capas de Clean Architecture

#### 1️⃣ **Entities (Entidades)** - Centro del círculo

**Qué es**: Objetos de negocio empresariales con reglas críticas.

**En el proyecto**: `Dominio/Entidades/`

```csharp
// Dominio/Entidades/OrdenMedica.cs
public class OrdenMedica
{
    public int OrdenMedicaId { get; set; }
    public DateTime Fecha { get; set; }
    public int PacienteId { get; set; }
    public int MedicoId { get; set; }
    public bool EntregadaAlPaciente { get; set; }

    // Navegación
    public Paciente Paciente { get; set; }
    public Medico Medico { get; set; }
    public List<LineaOrdenMedica> LineaOrdenMedica { get; set; }
}
```

**Características**:
- ✅ Sin dependencias de frameworks
- ✅ Representan conceptos del dominio médico
- ✅ Puras clases C# (POCOs)

---

#### 2️⃣ **Use Cases (Casos de Uso)** - Segundo círculo

**Qué es**: Lógica de aplicación específica, orquesta el flujo de datos.

**En el proyecto**: `Dominio/Servicios/`

```csharp
// Dominio/Servicios/OrdenesMedicas/OrdenMedicaService.cs
public class OrdenMedicaService : IOrdenMedicaService
{
    private readonly IOrdenMedicaRepository _ordenRepo;
    private readonly ILineaOrdenMedicaRepository _lineaRepo;
    private readonly IMedicoRepository _medicoRepo;

    public async Task<ServiceResponse<OrdenMedicaDto>> CrearOrdenMedica(OrdenMedicaDto dto)
    {
        // CASO DE USO: Crear una orden médica

        // 1. Validar que el médico esté activo
        var medico = await _medicoRepo.ObtenerPorIdAsync(dto.MedicoId);
        if (medico == null)
            return ServiceResponse<OrdenMedicaDto>.Failure("Médico no encontrado");

        // 2. Validar que el paciente exista
        var paciente = await _pacienteRepo.ObtenerPorIdAsync(dto.PacienteId);
        if (paciente == null)
            return ServiceResponse<OrdenMedicaDto>.Failure("Paciente no encontrado");

        // 3. Crear la orden médica
        var orden = _mapper.Map<OrdenMedica>(dto);
        await _ordenRepo.AgregarAsync(orden);

        // 4. Agregar líneas de medicamentos
        foreach (var linea in dto.LineaOrdenMedica)
        {
            linea.OrdenMedicaId = orden.OrdenMedicaId;
            await _lineaRepo.AgregarAsync(_mapper.Map<LineaOrdenMedica>(linea));
        }

        return ServiceResponse<OrdenMedicaDto>.Success(_mapper.Map<OrdenMedicaDto>(orden));
    }
}
```

**Ubicación en el proyecto**:
- `Dominio/Servicios/Pacientes/PacienteService.cs`
- `Dominio/Servicios/Medicos/MedicoService.cs`
- `Dominio/Servicios/OrdenesMedicas/OrdenMedicaService.cs`
- `Dominio/Servicios/Usuarios/UsuarioService.cs`
- `Dominio/Servicios/Farmacos/FarmacosService.cs`

---

#### 3️⃣ **Interface Adapters (Adaptadores de Interfaz)** - Tercer círculo

**Qué es**: Convierten datos entre casos de uso y el mundo externo.

**En el proyecto**:
- **DTOs**: `Dominio/Application/DTOs/`
- **Controllers**: `PresentacionApi/Controllers/`, `Presentacion/Controllers/`
- **ViewModels**: `Presentacion/Models/`

```csharp
// Dominio/Application/DTOs/OrdenMedicaDto.cs
public class OrdenMedicaDto
{
    public int OrdenMedicaId { get; set; }
    public DateTime Fecha { get; set; }
    public int PacienteId { get; set; }
    public int MedicoId { get; set; }
    public List<LineaOrdenMedicaDto> LineaOrdenMedica { get; set; }
    // DTO adaptado para transferencia, no es la entidad del dominio
}

// PresentacionApi/Controllers/OrdenMedicaController.cs
[ApiController]
[Route("api/[controller]")]
public class OrdenMedicaController : ControllerBase
{
    private readonly IOrdenMedicaService _service;

    [HttpPost]
    public async Task<IActionResult> CrearOrdenMedica([FromBody] OrdenMedicaDto dto)
    {
        // Adaptador: Convierte HTTP Request → DTO → Servicio
        var result = await _service.CrearOrdenMedica(dto);

        if (!result.Success)
            return BadRequest(result.ErrorMessage);

        // Adaptador: Convierte resultado → HTTP Response (JSON)
        return CreatedAtAction(nameof(ObtenerPorId), new { id = result.Data.OrdenMedicaId }, result.Data);
    }
}
```

**Responsabilidad**: Adaptar entre el formato externo (HTTP/JSON/Views) y el dominio interno.

---

#### 4️⃣ **Frameworks & Drivers (Frameworks y Controladores)** - Círculo externo

**Qué es**: Detalles de implementación (BD, Web, UI, dispositivos).

**En el proyecto**:
- **Entity Framework Core**: `Infraestructura/ContextoBD/`
- **ASP.NET Core MVC**: `Presentacion/`
- **ASP.NET Core Web API**: `PresentacionApi/`
- **SQL Server**: Base de datos

```csharp
// Infraestructura/ContextoBD/AplicacionBDContexto.cs
public class AplicacionBDContexto : DbContext  // ← Framework: Entity Framework
{
    public DbSet<Paciente> Pacientes { get; set; }
    public DbSet<Medico> Medicos { get; set; }
    public DbSet<OrdenMedica> OrdenMedicas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuración específica de EF Core / SQL Server
        modelBuilder.Entity<OrdenMedica>().ToTable("OrdenMedicas");
        modelBuilder.Entity<OrdenMedica>().HasKey(u => u.OrdenMedicaId);
        // ...
    }
}
```

**Características**:
- ✅ Contiene detalles técnicos (ORM, HTTP, etc.)
- ✅ Puede cambiarse sin afectar el dominio
- ✅ Ejemplo: Cambiar de SQL Server a PostgreSQL solo requiere modificar `Infraestructura`

---

## ¿Qué es Domain-Driven Design (DDD)?

**Domain-Driven Design** es un enfoque de desarrollo de software que pone el **dominio del negocio** en el centro del diseño.

### Conceptos Clave de DDD

#### 1. **Ubiquitous Language (Lenguaje Ubicuo)**

**Definición**: El mismo vocabulario compartido entre desarrolladores y expertos del dominio.

**En el proyecto**:
- **Médico**: `Medico` (no `Doctor` o `Physician`)
- **Orden Médica**: `OrdenMedica` (no `Prescription` o `Receta`)
- **Paciente**: `Paciente` (no `Patient` o `Cliente`)
- **Línea de Orden Médica**: `LineaOrdenMedica` (no `OrderLine`)
- **Obra Social**: `ObraSocial` (término específico de Argentina/Uruguay)

**Dónde se ve**:
```csharp
// Dominio/Entidades/ - Nombres coinciden con el lenguaje del dominio médico
public class Medico { ... }
public class Paciente { ... }
public class OrdenMedica { ... }  // ← Término del dominio, no "Receta" ni "Prescription"
public class ObraSocial { ... }   // ← Término regional específico
```

**Beneficio**: Un médico que lea el código entiende los conceptos sin traducción.

---

#### 2. **Entities (Entidades)**

**Definición**: Objetos con identidad única que persiste en el tiempo.

**En el proyecto**: `Dominio/Entidades/`

```csharp
// Dominio/Entidades/Paciente.cs
public class Paciente
{
    public int PacienteId { get; set; }  // ← Identidad única
    public string Documento { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }

    // Dos pacientes son el mismo si tienen el mismo PacienteId,
    // aunque cambien de nombre o dirección
}
```

**Ubicación**: Todas las clases en `Dominio/Entidades/`
- `Paciente`, `Medico`, `OrdenMedica`, `Usuario`, `LineaOrdenMedica`

**Característica**: La identidad (`PacienteId`) es lo que define la igualdad, no los atributos.

---

#### 3. **Value Objects (Objetos de Valor)**

**Definición**: Objetos sin identidad propia, definidos por sus atributos.

**En el proyecto**: Aunque no están explícitamente implementados como clases separadas, existen conceptos:

```csharp
// Ejemplo de Value Object (no implementado como clase, pero podría serlo)
public class Direccion  // ← Value Object potencial
{
    public string Calle { get; set; }
    public string Numero { get; set; }
    public string CodigoPostal { get; set; }

    // Dos direcciones son iguales si todos sus atributos coinciden
}

// Actualmente en Paciente.cs es un string simple
public class Paciente
{
    public string Direccion { get; set; }  // ← Podría ser un Value Object
}
```

**Dónde podría aplicarse**:
- `Direccion`: Calle, número, código postal
- `RangoDosis`: Dosis mínima, máxima, unidad
- `Documento`: Tipo (DNI/Pasaporte), número

---

#### 4. **Aggregates (Agregados)**

**Definición**: Grupo de entidades y value objects que se tratan como una unidad.

**En el proyecto**: `OrdenMedica` es un **Aggregate Root** (raíz del agregado)

```csharp
// Dominio/Entidades/OrdenMedica.cs - AGGREGATE ROOT
public class OrdenMedica
{
    public int OrdenMedicaId { get; set; }
    public DateTime Fecha { get; set; }

    // Agregado: OrdenMedica "posee" sus LineaOrdenMedica
    public List<LineaOrdenMedica> LineaOrdenMedica { get; set; }

    // ✅ CORRECTO: Acceder a líneas a través de la orden
    // orden.LineaOrdenMedica.Add(nuevaLinea);

    // ❌ INCORRECTO: Crear líneas sin orden
    // var linea = new LineaOrdenMedica();  // Sin OrdenMedicaId
}
```

**Regla del Agregado**: Las `LineaOrdenMedica` **no existen** sin una `OrdenMedica`.

**Dónde se aplica**:
```csharp
// Infraestructura/ContextoBD/AplicacionBDContexto.cs
modelBuilder.Entity<OrdenMedica>()
    .HasMany(s => s.LineaOrdenMedica)
    .WithOne(si => si.OrdenMedica)
    .HasForeignKey(si => si.OrdenMedicaId)
    .OnDelete(DeleteBehavior.Cascade);  // ← Si se borra la orden, se borran las líneas
```

**Otros ejemplos de agregados potenciales**:
- `Medico` + `Especialidad` (aunque no es ownership estricto)
- `Paciente` + `ObraSocial`

---

#### 5. **Repositories (Repositorios)**

**Definición**: Abstracción para acceso a agregados, simula una colección en memoria.

**En el proyecto**: `Dominio/Servicios/.../I*Repository.cs` + `Infraestructura/Repositorios/`

```csharp
// Dominio/Servicios/OrdenesMedicas/IOrdenMedicaRepository.cs
public interface IOrdenMedicaRepository
{
    Task<IEnumerable<OrdenMedica>> ObtenerTodosAsync();
    Task<OrdenMedica> ObtenerPorIdAsync(int id);
    Task AgregarAsync(OrdenMedica orden);
    Task ActualizarAsync(OrdenMedica orden);
    Task EliminarAsync(int id);

    // Repositorio simula: List<OrdenMedica> órdenes
    // órdenes.FirstOrDefault(o => o.OrdenMedicaId == id);
}

// Infraestructura/Repositorios/OrdenMedicaRepository.cs
public class OrdenMedicaRepository : IOrdenMedicaRepository
{
    private readonly AplicacionBDContexto _context;

    public async Task<OrdenMedica> ObtenerPorIdAsync(int id)
    {
        return await _context.OrdenMedicas
            .Include(o => o.LineaOrdenMedica)  // ← Carga el agregado completo
            .Include(o => o.Paciente)
            .Include(o => o.Medico)
            .FirstOrDefaultAsync(o => o.OrdenMedicaId == id);
    }
}
```

**Repositorios en el proyecto**:
- `IPacienteRepository` / `PacienteRepository`
- `IMedicoRepository` / `MedicoRepository`
- `IOrdenMedicaRepository` / `OrdenMedicaRepository`
- `ILineaOrdenMedicaRepository` / `LineaOrdenMedicaRepository`
- `IUsuarioRepository` / `UsuarioRepository`

**Ubicación**:
- Interfaces: `Dominio/Servicios/.../`
- Implementaciones: `Infraestructura/Repositorios/`

---

#### 6. **Services (Servicios de Dominio)**

**Definición**: Lógica de negocio que no pertenece a una entidad específica.

**En el proyecto**: `Dominio/Servicios/`

```csharp
// Dominio/Servicios/OrdenesMedicas/OrdenMedicaService.cs
public class OrdenMedicaService : IOrdenMedicaService
{
    public async Task<ServiceResponse<OrdenMedicaDto>> CrearOrdenMedica(OrdenMedicaDto dto)
    {
        // Lógica de negocio que involucra múltiples entidades:
        // - Validar médico
        // - Validar paciente
        // - Crear orden
        // - Agregar líneas de medicamentos

        // ¿Dónde va esta lógica?
        // ❌ No en Paciente (no es responsabilidad del paciente)
        // ❌ No en Medico (no es responsabilidad del médico)
        // ✅ En OrdenMedicaService (servicio de dominio)
    }
}
```

**Servicios de dominio en el proyecto**:
- `FarmacosService`: Búsqueda de medicamentos en API externa
- `MedicoService`: Validaciones y gestión de médicos
- `PacienteService`: Gestión de pacientes
- `OrdenMedicaService`: Creación y validación de recetas médicas
- `UsuarioService`: Autenticación y gestión de usuarios

---

#### 7. **Bounded Contexts (Contextos Acotados)**

**Definición**: Límites explícitos donde un modelo de dominio es válido.

**En el proyecto**: Aunque es un monolito, hay contextos implícitos:

```
┌─────────────────────────────────────┐
│  Contexto: Gestión de Pacientes    │
│  - Paciente                         │
│  - ObraSocial                       │
│  - PacienteService                  │
└─────────────────────────────────────┘

┌─────────────────────────────────────┐
│  Contexto: Gestión de Médicos       │
│  - Medico                           │
│  - Especialidad                     │
│  - MedicoService                    │
└─────────────────────────────────────┘

┌─────────────────────────────────────┐
│  Contexto: Órdenes Médicas          │
│  - OrdenMedica                      │
│  - LineaOrdenMedica                 │
│  - Farmaco (datos de CIMA)          │
│  - OrdenMedicaService               │
└─────────────────────────────────────┘

┌─────────────────────────────────────┐
│  Contexto: Autenticación            │
│  - Usuario                          │
│  - TipoUsuario                      │
│  - UsuarioService                   │
└─────────────────────────────────────┘
```

**Dónde se ve**:
- Carpetas en `Dominio/Servicios/`: `Pacientes/`, `Medicos/`, `OrdenesMedicas/`, `Usuarios/`, `Farmacos/`
- Cada carpeta representa un mini-contexto

**Evolución futura**: Estos contextos podrían convertirse en microservicios independientes.

---

#### 8. **Domain Events (Eventos de Dominio)**

**Definición**: Algo importante que ocurrió en el dominio.

**En el proyecto**: **No implementado actualmente**, pero ejemplos potenciales:

```csharp
// Ejemplo de evento (no implementado)
public class OrdenMedicaCreadaEvent
{
    public int OrdenMedicaId { get; set; }
    public int MedicoId { get; set; }
    public int PacienteId { get; set; }
    public DateTime FechaCreacion { get; set; }
}

// Uso potencial:
// - Enviar email al paciente cuando se crea la orden
// - Notificar a la farmacia
// - Auditar la creación de recetas
```

**Dónde podría aplicarse**:
- `OrdenMedicaCreada`
- `PacienteRegistrado`
- `UsuarioAutenticado`

---

## Aplicación en el Proyecto

### Mapa de Clean Architecture en el Proyecto

```
┌─────────────────────────────────────────────────────────────┐
│  FRAMEWORKS & DRIVERS (Círculo Externo)                     │
│  ┌────────────────────┐  ┌─────────────────────────────┐   │
│  │  Presentacion      │  │  PresentacionApi            │   │
│  │  (ASP.NET MVC)     │  │  (ASP.NET Web API)          │   │
│  │  - Controllers     │  │  - Controllers              │   │
│  │  - Views           │  │  - Swagger                  │   │
│  │  - wwwroot         │  └─────────────────────────────┘   │
│  └────────────────────┘                                     │
│                                                              │
│  ┌─────────────────────────────────────────────────────┐    │
│  │  Infraestructura (Entity Framework + SQL Server)    │    │
│  │  - AplicacionBDContexto                             │    │
│  │  - Migrations                                       │    │
│  └─────────────────────────────────────────────────────┘    │
└──────────────────────────┬───────────────────────────────────┘
                           │
┌──────────────────────────▼───────────────────────────────────┐
│  INTERFACE ADAPTERS (Círculo 3)                              │
│  ┌─────────────────────────────────────────────────────┐     │
│  │  Dominio/Application/DTOs                           │     │
│  │  - PacienteDto, MedicoDto, OrdenMedicaDto           │     │
│  │  - AutoMapperProfile                                │     │
│  └─────────────────────────────────────────────────────┘     │
│                                                               │
│  ┌─────────────────────────────────────────────────────┐     │
│  │  Infraestructura/Repositorios                       │     │
│  │  - PacienteRepository, MedicoRepository             │     │
│  └─────────────────────────────────────────────────────┘     │
└──────────────────────────┬────────────────────────────────────┘
                           │
┌──────────────────────────▼────────────────────────────────────┐
│  USE CASES (Círculo 2)                                        │
│  ┌─────────────────────────────────────────────────────┐      │
│  │  Dominio/Servicios                                  │      │
│  │  - PacienteService (Casos de uso de pacientes)      │      │
│  │  - MedicoService (Casos de uso de médicos)          │      │
│  │  - OrdenMedicaService (Casos de uso de recetas)     │      │
│  │  - UsuarioService (Casos de uso de auth)            │      │
│  │  - FarmacosService (Casos de uso de búsqueda)       │      │
│  └─────────────────────────────────────────────────────┘      │
└──────────────────────────┬────────────────────────────────────┘
                           │
┌──────────────────────────▼────────────────────────────────────┐
│  ENTITIES (Círculo Central - Núcleo)                          │
│  ┌─────────────────────────────────────────────────────┐      │
│  │  Dominio/Entidades                                  │      │
│  │  - Paciente (Reglas de negocio de pacientes)        │      │
│  │  - Medico (Reglas de negocio de médicos)            │      │
│  │  - OrdenMedica (Reglas de negocio de recetas)       │      │
│  │  - LineaOrdenMedica (Reglas de líneas de receta)    │      │
│  │  - Usuario (Reglas de usuarios)                     │      │
│  └─────────────────────────────────────────────────────┘      │
└───────────────────────────────────────────────────────────────┘
```

---

### Mapa de DDD en el Proyecto

```
DOMINIO (Bounded Context: Sistema de Alarma Médica)
│
├── ENTITIES (Entidades con identidad)
│   ├── Paciente           → Dominio/Entidades/Paciente.cs
│   ├── Medico             → Dominio/Entidades/Medico.cs
│   ├── Usuario            → Dominio/Entidades/Usuario.cs
│   ├── OrdenMedica        → Dominio/Entidades/OrdenMedica.cs (Aggregate Root)
│   ├── LineaOrdenMedica   → Dominio/Entidades/LineaOrdenMedica.cs
│   ├── Especialidad       → Dominio/Entidades/Especialidad.cs
│   ├── ObraSocial         → Dominio/Entidades/ObraSocial.cs
│   └── TipoUsuario        → Dominio/Entidades/TipoUsuario.cs
│
├── VALUE OBJECTS (No implementados explícitamente, pero candidatos)
│   ├── Direccion (actualmente string en Paciente)
│   ├── Documento (actualmente string en Paciente)
│   └── RangoDosis (potencial para LineaOrdenMedica)
│
├── AGGREGATES (Agregados)
│   ├── OrdenMedica + LineaOrdenMedica
│   │   └── Dominio/Entidades/OrdenMedica.cs (Root)
│   │       └── List<LineaOrdenMedica>
│   └── Medico + Especialidad (relación, no ownership)
│
├── REPOSITORIES (Abstracciones de persistencia)
│   ├── IPacienteRepository     → Dominio/Servicios/Pacientes/
│   ├── IMedicoRepository       → Dominio/Servicios/Medicos/
│   ├── IOrdenMedicaRepository  → Dominio/Servicios/OrdenesMedicas/
│   ├── IUsuarioRepository      → Dominio/Servicios/Usuarios/
│   └── Implementaciones        → Infraestructura/Repositorios/
│
├── DOMAIN SERVICES (Servicios de dominio)
│   ├── PacienteService         → Dominio/Servicios/Pacientes/
│   ├── MedicoService           → Dominio/Servicios/Medicos/
│   ├── OrdenMedicaService      → Dominio/Servicios/OrdenesMedicas/
│   ├── UsuarioService          → Dominio/Servicios/Usuarios/
│   └── FarmacosService         → Dominio/Servicios/Farmacos/
│
├── UBIQUITOUS LANGUAGE (Lenguaje ubicuo)
│   ├── "Orden Médica" (no "Receta" ni "Prescription")
│   ├── "Obra Social" (término argentino/uruguayo)
│   ├── "Línea de Orden Médica" (no "Item")
│   ├── "Médico" (no "Doctor")
│   └── "Paciente" (no "Cliente")
│
└── BOUNDED CONTEXTS (Contextos implícitos)
    ├── Gestión de Pacientes (Pacientes/, ObraSocial)
    ├── Gestión de Médicos (Medicos/, Especialidad)
    ├── Órdenes Médicas (OrdenesMedicas/, Farmacos)
    └── Autenticación (Usuarios/, TipoUsuario)
```

---

## Ejemplos Concretos

### Ejemplo 1: Crear una Orden Médica (Caso de Uso Completo)

#### Flujo según Clean Architecture:

```
1. REQUEST (Frameworks & Drivers)
   Usuario completa formulario en View → POST a /api/OrdenMedica

2. CONTROLLER (Interface Adapter)
   PresentacionApi/Controllers/OrdenMedicaController.cs

   [HttpPost]
   public async Task<IActionResult> CrearOrdenMedica([FromBody] OrdenMedicaDto dto)
   {
       // Adaptador: HTTP → DTO
       var result = await _ordenMedicaService.CrearOrdenMedica(dto);
       return Ok(result);
   }

3. SERVICE (Use Case)
   Dominio/Servicios/OrdenesMedicas/OrdenMedicaService.cs

   public async Task<ServiceResponse<OrdenMedicaDto>> CrearOrdenMedica(OrdenMedicaDto dto)
   {
       // LÓGICA DE NEGOCIO:

       // a) Validar médico autorizado
       var medico = await _medicoRepo.ObtenerPorIdAsync(dto.MedicoId);
       if (medico == null) return Failure("Médico no encontrado");

       // b) Validar paciente registrado
       var paciente = await _pacienteRepo.ObtenerPorIdAsync(dto.PacienteId);
       if (paciente == null) return Failure("Paciente no encontrado");

       // c) Mapear DTO → Entidad
       var orden = _mapper.Map<OrdenMedica>(dto);

       // d) Persistir orden
       await _ordenRepo.AgregarAsync(orden);

       // e) Persistir líneas de medicamentos
       foreach (var linea in dto.LineaOrdenMedica)
       {
           linea.OrdenMedicaId = orden.OrdenMedicaId;
           await _lineaRepo.AgregarAsync(_mapper.Map<LineaOrdenMedica>(linea));
       }

       return Success(_mapper.Map<OrdenMedicaDto>(orden));
   }

4. REPOSITORY (Interface Adapter)
   Infraestructura/Repositorios/OrdenMedicaRepository.cs

   public async Task AgregarAsync(OrdenMedica orden)
   {
       await _context.OrdenMedicas.AddAsync(orden);
       await _context.SaveChangesAsync();
   }

5. DBCONTEXT (Frameworks & Drivers)
   Infraestructura/ContextoBD/AplicacionBDContexto.cs

   public DbSet<OrdenMedica> OrdenMedicas { get; set; }

   → Entity Framework → SQL Server → INSERT INTO OrdenMedicas ...

6. RESPONSE (Frameworks & Drivers)
   HTTP 201 Created con OrdenMedicaDto en JSON
```

#### Aplicación de DDD:

```
ENTITIES:
- OrdenMedica (Aggregate Root)
- LineaOrdenMedica (parte del agregado)
- Medico (entidad referenciada)
- Paciente (entidad referenciada)

UBIQUITOUS LANGUAGE:
- "Orden Médica" (no "Prescription")
- "Línea de Orden Médica" (no "Item")

AGGREGATE:
OrdenMedica {
    OrdenMedicaId
    Fecha
    PacienteId
    MedicoId
    LineaOrdenMedica[] ← Agregado: las líneas pertenecen a la orden
}

DOMAIN SERVICE:
OrdenMedicaService.CrearOrdenMedica()
- Coordina múltiples repositorios
- Valida reglas de negocio
- No pertenece a una sola entidad

REPOSITORY:
IOrdenMedicaRepository → simula colección en memoria
```

---

### Ejemplo 2: Buscar Fármacos en API CIMA

#### Flujo según Clean Architecture:

```
1. REQUEST
   Usuario busca "ibuprofeno" en formulario

2. CONTROLLER
   PresentacionApi/Controllers/FarmacoController.cs

   [HttpGet]
   public async Task<IActionResult> BuscarFarmaco(string nombre)
   {
       var result = await _farmacosService.BuscarFarmacoPorNombre(nombre);
       return Ok(result);
   }

3. DOMAIN SERVICE
   Dominio/Servicios/Farmacos/FarmacosService.cs

   public async Task<ServiceResponse<List<FarmacoDto>>> BuscarFarmacoPorNombre(string nombre)
   {
       // Lógica de negocio: validar búsqueda
       if (string.IsNullOrEmpty(nombre))
           return Failure("Nombre requerido");

       // Delegar a cliente HTTP
       var response = await _cimaHttpClient.BuscarFarmaco(nombre);

       // Procesar respuesta
       var farmacos = JsonConvert.DeserializeObject<List<FarmacoDto>>(response);
       return Success(farmacos);
   }

4. HTTP CLIENT (Interface Adapter)
   Dominio/Servicios/Utils/CimaHttpClient.cs

   public async Task<string> BuscarFarmaco(string nombre)
   {
       var response = await _httpClient.GetAsync($"medicamentos?nombre={nombre}");
       return await response.Content.ReadAsStringAsync();
   }

5. EXTERNAL API (Frameworks & Drivers)
   https://cima.aemps.es/cima/rest/medicamentos?nombre=ibuprofeno
```

#### Aplicación de DDD:

```
DOMAIN SERVICE:
FarmacosService
- Lógica de búsqueda de medicamentos
- No pertenece a una entidad específica
- Coordina con API externa

REPOSITORY (Anti-corruption Layer):
CimaHttpClient actúa como "repositorio" de la API externa
- Abstrae detalles de la API CIMA
- El dominio solo ve ICimaHttpClient
- Protege el dominio de cambios en CIMA

BOUNDED CONTEXT:
API CIMA es un contexto externo
- Modelo de datos diferente al nuestro
- CimaHttpClient traduce entre contextos
```

---

### Ejemplo 3: Autenticación de Usuario con Google

#### Flujo según Clean Architecture:

```
1. REQUEST
   Usuario hace clic en "Iniciar sesión con Google"

2. CONTROLLER
   Presentacion/Controllers/LoginController.cs

   [HttpGet]
   public IActionResult GoogleLogin()
   {
       var properties = new AuthenticationProperties
       {
           RedirectUri = Url.Action("GoogleResponse")
       };
       return Challenge(properties, GoogleDefaults.AuthenticationScheme);
   }

3. GOOGLE OAUTH (External)
   Usuario autentica en Google → Callback a /signin-google

4. CONTROLLER
   [HttpGet]
   public async Task<IActionResult> GoogleResponse()
   {
       var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
       var googleId = result.Principal.FindFirst(ClaimTypes.NameIdentifier).Value;
       var email = result.Principal.FindFirst(ClaimTypes.Email).Value;

       // Llamar al servicio de dominio
       var usuarioDto = await _usuarioServiceWeb.ObtenerOCrearUsuarioPorGoogle(googleId, email);

       // Crear sesión
       HttpContext.Session.SetString("UsuarioId", usuarioDto.UsuarioId.ToString());
       return RedirectToAction("Index", "Home");
   }

5. SERVICE
   Dominio/Servicios/Usuarios/UsuarioService.cs

   public async Task<ServiceResponse<UsuarioDto>> ObtenerOCrearUsuarioPorGoogle(string googleId, string email)
   {
       // Buscar usuario existente
       var usuario = await _usuarioRepo.ObtenerPorGoogleIdAsync(googleId);

       if (usuario == null)
       {
           // Crear nuevo usuario
           usuario = new Usuario
           {
               GoogleId = googleId,
               Email = email,
               Nombre = email,
               TipoUsuarioId = 2, // Usuario normal
               Activo = true
           };
           await _usuarioRepo.AgregarAsync(usuario);
       }

       return Success(_mapper.Map<UsuarioDto>(usuario));
   }

6. REPOSITORY
   Infraestructura/Repositorios/UsuarioRepository.cs

   public async Task<Usuario> ObtenerPorGoogleIdAsync(string googleId)
   {
       return await _context.Usuarios
           .FirstOrDefaultAsync(u => u.GoogleId == googleId);
   }
```

#### Aplicación de DDD:

```
ENTITY:
Usuario
- Identidad: UsuarioId
- Atributos: GoogleId, Email, Nombre, Activo

DOMAIN SERVICE:
UsuarioService.ObtenerOCrearUsuarioPorGoogle()
- Lógica de negocio: crear usuario si no existe
- Coordina búsqueda y creación

ANTI-CORRUPTION LAYER:
Google OAuth es un sistema externo
- El dominio no conoce detalles de Google
- Controller adapta claims de Google a UsuarioDto
```

---

## Beneficios Obtenidos

### ✅ Beneficios de Clean Architecture

1. **Testeable sin Base de Datos**
   ```csharp
   // Prueba unitaria (ejemplo)
   var mockRepo = new Mock<IPacienteRepository>();
   var service = new PacienteService(mockRepo.Object);
   // No necesita SQL Server para testear lógica de negocio
   ```

2. **Cambio de BD sin afectar lógica**
   - Puedes cambiar de SQL Server a PostgreSQL modificando solo `Infraestructura`
   - El dominio no cambia

3. **Múltiples UIs con la misma lógica**
   - `Presentacion` (Web MVC)
   - `PresentacionApi` (REST API)
   - Ambos usan los mismos servicios de dominio

4. **Independencia de Frameworks**
   - Puedes cambiar de Entity Framework a Dapper
   - Solo modificas `Infraestructura/Repositorios/`

---

### ✅ Beneficios de DDD

1. **Lenguaje Común con Expertos del Dominio**
   - Un médico lee `OrdenMedica`, `Medico`, `ObraSocial` y entiende inmediatamente
   - No hay desconexión entre código y realidad

2. **Modelo Rico en el Dominio**
   - Las entidades tienen comportamiento, no solo datos
   - Ejemplo futuro: `OrdenMedica.MarcarComoEntregada()`

3. **Contextos Claros**
   - Cada carpeta en `Dominio/Servicios/` representa un subdominio
   - Facilita dividir en microservicios en el futuro

4. **Agregados Protegen Invariantes**
   - No puedes crear `LineaOrdenMedica` sin `OrdenMedica`
   - Cascade delete asegura integridad

---

## Resumen: Dónde se Usan en el Proyecto

### Clean Architecture

| Concepto | Ubicación | Ejemplo |
|----------|-----------|---------|
| **Entities** | `Dominio/Entidades/` | `Paciente.cs`, `OrdenMedica.cs` |
| **Use Cases** | `Dominio/Servicios/` | `OrdenMedicaService.cs` |
| **Interface Adapters** | `Dominio/Application/DTOs/`<br>`PresentacionApi/Controllers/` | `OrdenMedicaDto.cs`<br>`OrdenMedicaController.cs` |
| **Frameworks** | `Infraestructura/ContextoBD/`<br>`Presentacion/` | `AplicacionBDContexto.cs`<br>ASP.NET MVC |
| **Dependency Rule** | Referencias de proyectos | `Infraestructura` → `Dominio`<br>`Dominio` → ❌ nada |

---

### Domain-Driven Design

| Concepto | Ubicación | Ejemplo |
|----------|-----------|---------|
| **Entities** | `Dominio/Entidades/` | `Paciente`, `Medico`, `Usuario` |
| **Value Objects** | (No implementados) | Potencial: `Direccion`, `Documento` |
| **Aggregates** | `Dominio/Entidades/` | `OrdenMedica` + `LineaOrdenMedica` |
| **Repositories** | `Dominio/Servicios/.../I*Repository.cs`<br>`Infraestructura/Repositorios/` | `IPacienteRepository`<br>`PacienteRepository` |
| **Domain Services** | `Dominio/Servicios/` | `OrdenMedicaService`, `UsuarioService` |
| **Ubiquitous Language** | Nombres de entidades | `OrdenMedica`, `ObraSocial`, `Medico` |
| **Bounded Contexts** | Carpetas en `Dominio/Servicios/` | `Pacientes/`, `Medicos/`, `OrdenesMedicas/` |

---

## Recursos Adicionales

### Libros Recomendados

1. **Clean Architecture** - Robert C. Martin
   - Capítulos clave: 17-22 (The Clean Architecture)

2. **Domain-Driven Design** - Eric Evans (Libro Azul)
   - Capítulos clave: 5 (Model-Driven Design), 6 (Domain Object Lifecycle)

3. **Implementing Domain-Driven Design** - Vaughn Vernon (Libro Rojo)
   - Más práctico y aplicado que el anterior

### Artículos

- **The Clean Architecture** - Uncle Bob
  https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html

- **DDD, Hexagonal, Onion, Clean, CQRS** - Herberto Graca
  https://herbertograca.com/2017/11/16/explicit-architecture-01-ddd-hexagonal-onion-clean-cqrs-how-i-put-it-all-together/

---

**Última actualización**: 2025-10-20
