namespace SistMedWebApp.Controllers.Request.Login
{
    public class LoginRequest
    {
        public string? usuarioname { get; set; }
        public string password { get; set; } = string.Empty;
    }
}
