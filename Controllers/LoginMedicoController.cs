using Fiap.Api.Data;
using Fiap.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginMedicoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LoginMedicoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Login request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Senha))
                return BadRequest("Email e senha são obrigatórios.");

            var usuario = _context.Medicos
                .FirstOrDefault(m => m.Email == request.Email && m.Senha == request.Senha);

            if (usuario == null)
                return Unauthorized("Credenciais inválidas.");

            return Ok(new { message = "Login bem-sucedido!", usuarioId = usuario.Id, nome = usuario.Nome });
        }
    }
}
