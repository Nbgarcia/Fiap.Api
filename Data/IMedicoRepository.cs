using Fiap.Api.Models;

namespace Fiap.Api.Data
{
    public interface IMedicoRepository
    {
        Task<List<Medico>> ListarMedicosAsync();
        Task<Medico> ObterMedicoPorIdAsync(int id);
        Task CadastrarMedicoAsync(Medico medico);
        Task RemoverMedicoAsync(int id);
    }

}
