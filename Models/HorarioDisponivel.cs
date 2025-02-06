using System.Text.Json.Serialization;

namespace Fiap.Api.Models
{
    public class HorarioDisponivel
    {
        public int Id { get; set; }
        public int MedicoId { get; set; }
        public string Data { get; set; }
        public string Hora { get; set; }
        public bool Disponivel { get; set; } = true;

        // Relacionamento com Médico
        //[JsonIgnore]
        //public Medico Medico { get; set; } = null;
    }
}
