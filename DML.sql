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

--use SistemaMedicoMartin

create table Persona (
	Id int primary key identity(1,1) not null,
	Nombre varchar(15) not null,
	Apellido varchar(30) not null,
	FechaNacimiento datetime not null,
	Genero varchar(15) not null,
	DNI varchar(12) unique not null,
	Domicilio varchar(150),
	Telefono varchar(20),
	Email varchar(100),

);


CREATE TABLE ObraSocial (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL UNIQUE,
    Codigo NVARCHAR(10) NOT NULL UNIQUE,
    Descripcion NVARCHAR(255) NULL,
    Activo BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT GETDATE(),
    FechaModificacion DATETIME2(7) NULL
);

create table Paciente(
	Id int primary key identity(1,1) not null,
	PersonaId int not null Foreign key references Persona(Id),
	ObrasocialId int not null  Foreign key references Obrasocial(Id),
	NumeroAfiliado varchar(30) not null,
	FechaCreacion datetime not null,
	FechaModificacion datetime,
	Activo bit not null default 1

	--Constraint FK_Paciente_Persona Foreign key (PersonaId) references Persona(Id),
);

create table Especialidad(
	Id int primary key identity(1,1) not null,
	Descripcion varchar(100) not null,
	Activo bit not null default 1,
);

create table Medico(
	Id int identity(1,1) primary key not null,
	PersonaId int not null Foreign key references Persona(Id),
	EspecialidadId int not null Foreign key references Especialidad(Id),
	Matricula varchar(20) not null,
	FechaCreacion datetime not null,
	Activo bit not null default 1,
	FechaModificacion datetime
);

CREATE TABLE TipoUsuario (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL UNIQUE,
    Descripcion NVARCHAR(255) NULL,
    Activo BIT NOT NULL DEFAULT 1
);

CREATE TABLE Usuario (
    UsuarioId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    NombreUsuario NVARCHAR(50) NOT NULL UNIQUE,
    Contrasena NVARCHAR(255) NOT NULL,
    TipoUsuarioId INT NOT NULL FOREIGN KEY REFERENCES TipoUsuario(Id),
    Activo BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT GETDATE(),
	FechaModificacion datetime,
    UltimoAcceso DATETIME2(7) NULL
);

CREATE TABLE OrdenMedica (
    OrdenMedicaId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    PacienteId INT NOT NULL FOREIGN KEY REFERENCES Paciente(Id),
    MedicoId INT NOT NULL FOREIGN KEY REFERENCES Medico(Id),
    Fecha DATETIME2(7) NOT NULL DEFAULT GETDATE() CHECK (Fecha <= GETDATE()),
    ObraSocialId INT NULL FOREIGN KEY REFERENCES ObraSocial(Id),
    EntregadaAlPaciente BIT NOT NULL DEFAULT 0,
    Observaciones NVARCHAR(1000) NULL
);

CREATE TABLE LineaOrdenMedica (
    LineaOrdenMedicaId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    OrdenMedicaId INT NOT NULL FOREIGN KEY REFERENCES OrdenMedica(OrdenMedicaId) ON DELETE CASCADE,
    Cantidad INT NOT NULL CHECK (Cantidad > 0),
    NumeroRegistro NVARCHAR(20) NOT NULL,
    Nombre NVARCHAR(200) NOT NULL,
    FrecuenciaHoras INT NULL CHECK (FrecuenciaHoras > 0),
    UnicaAplicacion BIT NOT NULL DEFAULT 0,
    Observacion NVARCHAR(500) NULL,
    TratamientoEmpezado BIT NOT NULL DEFAULT 0
)
