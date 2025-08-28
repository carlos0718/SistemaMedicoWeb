# Sistema Médico Web - Base de Datos

Este documento describe las tablas del sistema médico que necesitan ser completadas/implementadas.

## 📋 Lista de Tareas - Tablas de la Base de Datos

### ✅ Tablas Base Creadas

-   [x] **Persona** - Tabla base que almacena información personal común (nombre, apellido, DNI)
-   [x] **ObraSocial** - Catálogo de obras sociales disponibles con estado activo/inactivo
-   [x] **Paciente** - Información de pacientes vinculada a personas y obras sociales
-   [x] **Especialidad** - Catálogo de especialidades médicas disponibles
-   [x] **Medicos** - Información de médicos vinculada a personas y especialidades

### ⏳ Tablas Pendientes de Implementación

Según el esquema de entidades comentado en el archivo SQL, faltan las siguientes tablas:

-   [ ] **Usuario** - Tabla para gestionar usuarios del sistema de autenticación

    -   Campos sugeridos: Id, Username, Password, Email, PersonaId (FK), TipoUsuarioId (FK), Activo, FechaCreacion, FechaModificacion

-   [ ] **TipoUsuario** - Catálogo de tipos de usuarios del sistema

    -   Campos sugeridos: Id, Descripcion (Administrador, Médico, Recepcionista, etc.), Activo

-   [ ] **OrdenMedica** - Órdenes médicas emitidas por los médicos

    -   Campos sugeridos: Id, MedicoId (FK), PacienteId (FK), Fecha, Diagnostico, Observaciones, Estado, FechaCreacion

-   [ ] **LineaOrdenMedica** - Líneas de tratamiento/medicamentos de cada orden médica
    -   Campos sugeridos: Id, OrdenMedicaId (FK), Medicamento, Dosis, Frecuencia, Duracion, Instrucciones

### 🔧 Mejoras Pendientes en Tablas Existentes

-   [ ] **Persona** - Agregar campos adicionales si son necesarios

    -   Sugerencias: Telefono, Email, Direccion, FechaNacimiento, Sexo

-   [ ] **Medicos** - Corregir nombre de tabla (debería ser singular: "Medico")

    -   Agregar campos: Matricula

-   [ ] **Paciente** - Considerar campos adicionales
    -   Sugerencias: NumeroAfiliado, ContactoEmergencia

### 🎯 Próximos Pasos

1. **Completar las tablas faltantes** según el esquema de entidades definido
2. **Revisar relaciones** entre tablas y claves foráneas
3. **Insertar datos de prueba** para mostrar en las consultas.

### 📝 Notas Técnicas

-   Todas las tablas usan `identity(1,1)` para claves primarias
-   Se mantiene un patrón de campos `Activo`, `FechaCreacion`, `FechaModificacion` para auditoría
-   Las relaciones están establecidas mediante claves foráneas

---

**Estado del Proyecto**: En desarrollo
**Última actualización**: 28 de agosto de 2025
