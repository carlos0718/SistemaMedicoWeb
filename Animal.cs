namespace WebAppSistemaMedico
{
    public class Animal
    {
        public string? Tipo { get; set; }   
        public string? Nombre { get; set; }
        public string? Raza { get; set; }

        public Animal()
        {
            Tipo = "Perro";
            Nombre = "Dalma";
            Raza = "Dalmata";
        }
        public Animal(string tipo, string nombre, string raza)
        {
            Tipo = tipo;
            Nombre = nombre;
            Raza = raza;
        }
        public Animal(string tipo, string nombre)
        {
            Tipo = tipo;
            Nombre = nombre;
            
        }
        public void Saludar()
        {
            Console.WriteLine(value: $"Hola, soy un {Tipo}, me llamo {Nombre} y soy de raza {Raza}");
        }
        
    }   
}
