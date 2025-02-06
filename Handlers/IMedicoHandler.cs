using Fiap.Api.Models;

namespace Fiap.Api.Handlers
{
    public interface IMedicoHandler
    {
        Task CadastrarMedicoAsync(Medico medico);
        Task<List<Medico>> ListarMedicosAsync();
        Task<Medico> ObterMedicoPorIdAsync(int id);
        Task RemoverMedicoAsync(int id);
    }

}
