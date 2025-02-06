using Fiap.Api.Data;
using Fiap.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiap.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HorarioDisponivelController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HorarioDisponivelController(ApplicationDbContext context)
        {
            _context = context;
        }
           
        [HttpPost]
        public IActionResult CadastrarHorario([FromBody] HorarioDisponivel horario)
        {
            if (horario == null)
            {
                return BadRequest("Invalid data.");
            }

            // Você pode validar o MedicoId aqui, se necessário
            var medico = _context.Medicos.Find(horario.MedicoId);
            if (medico == null)
            {
                return BadRequest("Medico não encontrado.");
            }

            // Salve o novo HorarioDisponivel
            _context.HorariosDisponiveis.Add(horario);
            _context.SaveChanges();

            return CreatedAtAction(nameof(ObterHorarioPorId), new { id = horario.Id }, horario);
        }

        [HttpPost("atualizar/{id}")]
        public IActionResult AtualizarHorario(int id, [FromBody] HorarioDisponivel horarioAtualizado)
        {
            if (horarioAtualizado == null)
                return BadRequest("Dados inválidos.");

            var horarioExistente = _context.HorariosDisponiveis.Find(id);

            if (horarioExistente == null)
                return NotFound("Horário não encontrado.");

            // Atualiza os campos
            horarioExistente.Data = horarioAtualizado.Data;
            horarioExistente.Hora = horarioAtualizado.Hora;
            horarioExistente.Disponivel = horarioAtualizado.Disponivel;

            _context.HorariosDisponiveis.Update(horarioExistente);
            _context.SaveChanges();

            return Ok(new
            {
                mensagem = "Horário atualizado com sucesso!",
                horario = horarioExistente
            });
        }

        [HttpPost("aceiterecusa/{id}")]
        public IActionResult AceiteRecusaHorarioMedico(int id, [FromBody] HorarioDisponivel horarioAtualizado)
        {
            if (horarioAtualizado == null)
                return BadRequest("Dados inválidos.");

            var horarioExistente = _context.HorariosDisponiveis.Find(id);

            if (horarioExistente == null)
                return NotFound("Horário não encontrado.");

            // Atualiza os campos            
            horarioExistente.Disponivel = horarioAtualizado.Disponivel;

            _context.HorariosDisponiveis.Update(horarioExistente);
            _context.SaveChanges();

            //inserir dados em uma tabela que alimanta os email que serão disparados

            if (horarioExistente.Disponivel)
            {
                return Ok(new
                {
                    mensagem = "Horário confirmado!",
                    horario = horarioExistente
                });
            }
            else
            {
                return Ok(new
                {
                    mensagem = "Horário recusado!",
                    horario = horarioExistente
                });
            }
        }

        [HttpGet]
        public IActionResult ListarHorarios()
        {
            var horarios = _context.HorariosDisponiveis.ToList();
            return Ok(horarios);
        }
       
        [HttpGet("{id}")]
        public IActionResult ObterHorarioPorId(int id)
        {
            var horario = _context.HorariosDisponiveis.Find(id);
            if (horario == null)
                return NotFound("Horário não encontrado.");

            return Ok(new
            {
                horario.Id,
                horario.MedicoId,
                horario.Data,
                horario.Hora,
                horario.Disponivel
            });
        }
        
        [HttpDelete("{id}")]
        public IActionResult RemoverHorario(int id)
        {
            var horario = _context.HorariosDisponiveis.Find(id);
            if (horario == null)
                return NotFound("Horário não encontrado.");

            _context.HorariosDisponiveis.Remove(horario);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
