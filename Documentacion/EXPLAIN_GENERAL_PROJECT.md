# DESCRIPCIÓN TÉCNICA - Sistema de Alarma Médica

Proyecto está desarrollado en .NET 6, combina librería de clases, ASP .NET Core Web App(MVC) y ASP.NET Core API. Como ORM usamos EF Core para la conexión con la Base de datos de SQL Server.

## Visión General

**Sistema de Alarma Médica** es una aplicación para la gestión de órdenes médicas, pacientes, médicos y fármacos. El sistema permite crear y administrar recetas médicas (órdenes médicas) con sus respectivas líneas de medicamentos.

### Características Principales
- Gestión de pacientes, médicos, usuarios y órdenes médicas
- Separación de Roles: Administrador, Médico y Paciente
- Integración con API externa CIMA (Agencia Española de Medicamentos) para búsqueda de fármacos
- Autenticación y autorización con soporte para Google OAuth
- Arquitectura basada en capas con separación de responsabilidades
- Doble interfaz: Web MVC y API REST

---

## Visión Arquitectónica

El Sistema de Alarma Médica implementa una **Arquitectura en Capas (Layered Architecture)** inspirada en los principios de **Clean Architecture**. Esta arquitectura garantiza la separación de responsabilidades, facilita el mantenimiento y permite la evolución del sistema de manera ordenada.
![Diagrama de Arquitectura Limpia del proyecto](clena-architecture.png)

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

```


## DTOs (Data Transfer Objects)

Los DTOs son un patrón de diseño que define objetos. Se utilizan para transferir datos entre capas sin exponer entidades directamente.

Propósito Principal
•	Transferencia de datos: Transportar información sin lógica de negocio
•	Encapsulación: Agrupar datos relacionados en un solo objeto
•	Comunicación entre capas: Facilitar el intercambio de datos

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
2. **Google OAuth**: Autenticación con cuenta de Google.

