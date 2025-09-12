

using WebAppSistemaMedico.Models;

namespace WebAppSistemaMedico.Models
{
    public class LineaOrdenMedica 
    {
        public int Id { get; set; }
        public int OrdenMedicaId { get; set; }
        public string? Nombre { get; set; }
        public int Cantidad { get; set; }  
        public string? Dosis { get; set; }
        public string? FrecuenciaHoras { get; set; }
        public string? Observacion { get; set; }
        public bool UnicaAplicacion { get; set; }
        public bool TratamientoEmpezado { get; set; }
        public int Duracion { get; set; }
        
        virtual public OrdenMedica? OrdenMedica { get; set; }
    }
}
