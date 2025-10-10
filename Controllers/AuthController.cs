using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistMedWebApp.Controllers.Request.Login;
using System.Security.Claims;
using WebAppSistemaMedico.Data;
using WebAppSistemaMedico.Models;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SistMedWebApp.Controllers.Apis
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly WebAppSistemaMedicoContext _context;
        public AuthController(WebAppSistemaMedicoContext context)
        {
            _context = context;
        }
        // GET: api/<AuthController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AuthController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AuthController>
        [HttpPost]
        public async Task<ActionResult> AuthLogin([FromBody] LoginRequest request)
        {
            if (request.usuarioname == null || request.password == null)
                return BadRequest(new { success = false, message = "Usuario y contraseña son requeridos" });

            Usuario? user = await _context.Usuario.FirstOrDefaultAsync(u => u.NombreUsuario.Equals(request.usuarioname) && u.Contrasena.Equals(request.password));

            if (user == null)
            {
                return Unauthorized(new { success = false, message = "Credenciales inválidas o usuario no existe" });
            }

            // Crear claims para la sesión (ESTO ES NECESARIO)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.NombreUsuario ?? ""),
                new Claim(ClaimTypes.NameIdentifier, user.UsuarioId.ToString()),
                new Claim(ClaimTypes.Role, user.TipoUsuarioId.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // Mantener sesión después de cerrar el navegador
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(24)
            };

            // Crear la sesión
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Ok(new { success = true, redirectUrl = "/Home/Index" , data = user});
        }

        // PUT api/<AuthController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AuthController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

}
