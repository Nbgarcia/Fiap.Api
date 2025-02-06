using Fiap.Api.Data;
using Fiap.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Fiap.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgendamentoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AgendamentoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult AgendarConsulta([FromBody] Agendamento agendamento)
        {
            if (agendamento == null)
                return BadRequest("Dados inválidos.");

            // Verificar se o horário existe e está disponível
            var horario = _context.HorariosDisponiveis
                .FirstOrDefault(h => h.Id == agendamento.HorarioId && h.Disponivel);

            if (horario == null)
                return BadRequest("Horário não disponível.");

            var medico = _context.HorariosDisponiveis
               .FirstOrDefault(h => h.Id == agendamento.MedicoId);

            if (medico == null)
                return BadRequest("Médico não cadastrado.");

            var paciente = _context.HorariosDisponiveis
               .FirstOrDefault(h => h.Id == agendamento.PacienteId);

            if (paciente == null)
                return BadRequest("Paciente não cadastrado.");

            _context.Agendamentos.Add(agendamento);

            // Marcar horário como indisponível
            horario.Disponivel = false;
            _context.SaveChanges();

            return Ok(new { mensagem = "Consulta agendada com sucesso!", agendamento });
        }
    }
}
