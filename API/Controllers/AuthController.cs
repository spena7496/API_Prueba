using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] User usuario)
        {
            // Validar el usuario (ejemplo básico, reemplazar con una validación real)
            if (usuario.userName == "admin" && usuario.password == "admin")
            {
                // Generar token
                var token = GenerarToken(usuario);
                return Ok(new { token });
            }

            return Unauthorized(new { mensaje = "Credenciales incorrectas" });
        }
        private string GenerarToken(User usuario)
        {
            // Información sobre el usuario
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, usuario.userName),
            new Claim(ClaimTypes.Role, "Admin") // Role opcional
        };

            // Clave secreta desde configuración
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Crear el token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
