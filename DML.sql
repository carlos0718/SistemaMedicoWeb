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

create table Medico(
	Id int identity(1,1) not null,
	PersonaId int not null Foreign key references Persona(Id),
	EspecialidadId int not null Foreign key references Especialidad(Id),
	Activo bit not null default 1,
	FechaCreacion datetime not null,
	FechaModificacion datetime
	Matricula varchar(100) not null,
	Telefono int not null
	Email varchar(100) not null,
)
--Este es un comentario de prueba 2

create table Usuario(
    Id int primary Key identity(1,1) not null,
	Username varchar(100) not null,
	Password varchar(100) not null,
	Email varchar (100) not null,
	Personald varchar(100) not null Foreign key references Persona(Id),
	TipoUsuario int Foreign key references TipoUsuario(Id),
	Activo bit not null default 1,
	FechaCreacion datetime not null,
	FechaModificacion datetime
)
create table TipoUsuario(
    Id int primary Key identity(1,1) not null,
	Descripcion int not null,
	Activo bit not null default 1,
)
create table OrdenMedica(
	Id int primary Key identity(1,1) not null,
	Medicold int Foreign Key references Medicos(Id),
	Pacienteld int Forgein Key references Paciente(Id),
	Diagnostico varchar(100) not null,
	Observaciones varchar(100) not null,
	Estado varcahar(100) not null,
	FechaCreacion datetime not null,
)
create table LineaOrdenMedica(
	Id int primary Key identity(1,1) not null,
	OrdenMedicald int Forgein Key references OrdenMedica(Id),
	Medicamento varchar(100) not null,
	Dosis int not null,
	Frecuencia varchar(100) not null,
	Duracion varchar(100) not null,
	Instrucciones varchar(100) not null,
)
