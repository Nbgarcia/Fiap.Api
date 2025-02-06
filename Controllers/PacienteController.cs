using Fiap.Api.Data;
using Fiap.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacienteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public PacienteController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [HttpPost]
        public IActionResult CadastrarPaciente([FromBody] Paciente paciente)
        {
            if (paciente == null)
                return BadRequest("Dados inválidos.");

            _context.Pacientes.Add(paciente);
            _context.SaveChanges();

            return CreatedAtAction(nameof(ObterPacientePorId), new { id = paciente.Id }, paciente);
        }

        [HttpGet("{id}")]
        public IActionResult ObterPacientePorId(int id)
        {
            var paciente = _context.Pacientes.Find(id);
            if (paciente == null)
                return NotFound("Paciente não encontrado.");

            return Ok(paciente);
        }

        [HttpGet]
        public IActionResult ListarPacientes()
        {
            var pacientes = _context.Pacientes.ToList();
            return Ok(pacientes);
        }

        [HttpDelete("{id}")]
        public IActionResult RemoverPaciente(int id)
        {
            var paciente = _context.Pacientes.Find(id);
            if (paciente == null)
                return NotFound("Pacientes não encontrado.");

            _context.Pacientes.Remove(paciente);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpGet("disponiveis")]
        public IActionResult BuscarMedicosDisponiveis([FromQuery] string data)
        {
            if (string.IsNullOrEmpty(data))
                return BadRequest("A data é obrigatória.");

            var medicosDisponiveis = _context.Medicos
                .Where(m => m.HorariosDisponiveis.Any(h => h.Data == data && h.Disponivel))
                .Select(m => new
                {
                    m.Id,
                    m.Nome,
                    m.CRM,
                    m.Email,
                    Horarios = m.HorariosDisponiveis
                        .Where(h => h.Data == data && h.Disponivel)
                        .Select(h => new { h.Id, h.Data, h.Hora })
                })
                .ToList();

            if (!medicosDisponiveis.Any())
                return NotFound("Nenhum médico disponível para esta data.");

            return Ok(medicosDisponiveis);
        }
    }
}
