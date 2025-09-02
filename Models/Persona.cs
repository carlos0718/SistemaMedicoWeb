namespace WebAppSistemaMedico.Models
{

    /*
        Id int primary key identity(1,1) not null,
        Nombre varchar(15) not null,
        Apellido varchar(30) not null,
        FechaNacimiento datetime not null,
        Genero varchar(15) not null,
        DNI varchar(12) unique not null,
        Domicilio varchar(150),
        Telefono varchar(20),
        Email varchar(100),
     */
    public class Persona
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? Genero { get; set; }
        public string? DNI { get; set; }
        public string? Domicilio { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }

    }
}
