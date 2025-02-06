using Fiap.Api.Data;
using Fiap.Api.Handlers;
using Fiap.Api.Models;
using Microsoft.EntityFrameworkCore;

public class MedicoHandler : IMedicoHandler
{
    private readonly ApplicationDbContext _context;

    public MedicoHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CadastrarMedicoAsync(Medico medico)
    {
        _context.Medicos.Add(medico);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Medico>> ListarMedicosAsync()
    {
        return await _context.Medicos.ToListAsync();
    }

    public async Task<Medico> ObterMedicoPorIdAsync(int id)
    {
        return await _context.Medicos.FindAsync(id);
    }

    public async Task RemoverMedicoAsync(int id)
    {
        var medico = await _context.Medicos.FindAsync(id);
        if (medico != null)
        {
            _context.Medicos.Remove(medico);
            await _context.SaveChangesAsync();
        }
    }
}
