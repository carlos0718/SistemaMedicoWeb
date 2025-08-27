--create database SistemaMedicoMartin
/*### Esquema de Entidades
- **Persona** - Tabla Base
- **Paciente** - Datos de pacientes (hereda de Persona)
- **Medico** - Datos de médicos (hereda de Persona)
- **Usuario** - Usuarios del sistema
- **TipoUsuario** - Tipos de usuarios del sistema
- **ObraSocial** - Obras sociales disponibles
- **Especialidad** - Especialidades médicas
- **OrdenMedica** - Órdenes médicas
- **LineaOrdenMedica** - Líneas de tratamiento de órdenes médicas
*/

create table Persona (
	Id int primary key identity(1,1),
	Nombre varchar(15) not null,
	Apellido varchar(30) not null,
	DNI varchar(12) not null,
)


create table Obrasocial(
	Id int primary key identity(1,1),
	Nombre varchar(50) not null,
	Activo bit not null default 1
)

create table Paciente(
	Id int primary key identity(1,1) not null,
	PersonaId int not null Foreign key references Persona(Id),
	Activo bit not null default 1,
	ObrasocialId int not null  Foreign key references Obrasocial(Id),
	FechaCreacion datetime not null,
	FechaModificacion datetime

	--Constraint FK_Paciente_Persona Foreign key (PersonaId) references Persona(Id),
)

create table Especialidad(
	Id int primary key identity(1,1) not null,
	Descripcion varchar(100) not null,
	Activo bit not null default 1,
)

create table Medicos(
	Id int identity(1,1) not null,
	PersonaId int not null Foreign key references Persona(Id),
	EspecialidadId int not null Foreign key references Especialidad(Id),
	Activo bit not null default 1,
	FechaCreacion datetime not null,
	FechaModificacion datetime
)
--Este es un comentario de prueba