using System.Text.Json.Serialization;

namespace Fiap.Api.Models
{
    public class Medico
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string CRM { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }

        [JsonIgnore]
        public List<HorarioDisponivel> HorariosDisponiveis { get; set; } = new List<HorarioDisponivel>();
    }
}

