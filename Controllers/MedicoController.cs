using Fiap.Api.Handlers;
using Fiap.Api.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class MedicoController : ControllerBase
{
    private readonly IMedicoHandler _medicoHandler;

    public MedicoController(IMedicoHandler medicoHandler)
    {
        _medicoHandler = medicoHandler;
    }

    [HttpPost]
    public async Task<IActionResult> CadastrarMedico([FromBody] Medico medico)
    {
        if (medico == null)
            return BadRequest("Dados inválidos.");

        await _medicoHandler.CadastrarMedicoAsync(medico);

        return CreatedAtAction(nameof(ObterMedicoPorId), new { id = medico.Id }, medico);
    }

    [HttpGet]
    public async Task<IActionResult> ListarMedicos()
    {
        var medicos = await _medicoHandler.ListarMedicosAsync();
        return Ok(medicos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterMedicoPorId(int id)
    {
        var medico = await _medicoHandler.ObterMedicoPorIdAsync(id);
        if (medico == null)
            return NotFound("Médico não encontrado.");

        return Ok(medico);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoverMedico(int id)
    {
        var medico = await _medicoHandler.ObterMedicoPorIdAsync(id);
        if (medico == null)
            return NotFound("Médico não encontrado.");

        await _medicoHandler.RemoverMedicoAsync(id);
        return NoContent();
    }
}
