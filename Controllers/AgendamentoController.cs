using Fiap.Api.Data;
using Fiap.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;

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

        [HttpGet("agendamentos")]
        public IActionResult GetAgendamentos()
        {
            var agendamentos = _context.Agendamentos.ToList();

            if (!agendamentos.Any())
                return NotFound("Nenhum agendamento encontrado.");

            return Ok(agendamentos);
        }

        [HttpGet("agendamentos/paciente/{idPaciente}")]
        public IActionResult GetAgendamentosPaciente(int idPaciente)
        {
            var agendamentos = _context.Agendamentos
                .Where(a => a.PacienteId == idPaciente)
                .ToList();

            if (!agendamentos.Any())
                return NotFound("Nenhum agendamento encontrado.");

            return Ok(agendamentos);
        }

        [HttpGet("agendamentos/medico/{idMedico}")]
        public IActionResult GetAgendamentosMedico(int idMedico)
        {

            // Buscar agendamentos cujos horários ainda estão disponíveis
            var agendamentosPendentes = _context.Agendamentos
                .Where(a => _context.HorariosDisponiveis.Any(h => h.Id == a.HorarioId && h.MedicoId == idMedico))
                .ToList();

            if (!agendamentosPendentes.Any())
                return NotFound("Nenhum agendamento pendente encontrado.");

            return Ok(agendamentosPendentes);
        }

        [HttpPut("agendamentos/{id}/trocar-horario")]
        public IActionResult AtualizarHorarioAgendamento(int id, [FromBody] int novoHorarioId)
        {
            var agendamento = _context.Agendamentos.FirstOrDefault(a => a.Id == id);
            if (agendamento == null)
                return NotFound("Agendamento não encontrado.");

            var horarioAntigo = _context.HorariosDisponiveis.FirstOrDefault(h => h.Id == agendamento.HorarioId);
            if (horarioAntigo != null)
                horarioAntigo.Disponivel = true; // Liberar horário antigo

            var horarioNovo = _context.HorariosDisponiveis.FirstOrDefault(h => h.Id == novoHorarioId && h.Disponivel);
            if (horarioNovo == null)
                return BadRequest("Novo horário não disponível.");

            // Atualizar agendamento para o novo horário
            agendamento.HorarioId = novoHorarioId;
            horarioNovo.Disponivel = false; // Bloquear novo horário

            _context.SaveChanges();

            return Ok(new { mensagem = "Horário do agendamento atualizado com sucesso!", agendamento });
        }

    }
}
