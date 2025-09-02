namespace WebAppSistemaMedico.Models
{
    /*
     Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Nombre NVARCHAR(100) NOT NULL UNIQUE,
        Codigo NVARCHAR(10) NOT NULL UNIQUE,
        Descripcion NVARCHAR(255) NULL,
        Activo BIT NOT NULL DEFAULT 1,
        FechaCreacion DATETIME2(7) NOT NULL DEFAULT GETDATE(),
        FechaModificacion DATETIME2(7) NULL
     */
    public class ObraSocial
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Codigo { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
