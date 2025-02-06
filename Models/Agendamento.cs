namespace Fiap.Api.Models
{
    public class Agendamento
    {
        public int Id { get; set; }
        public int MedicoId { get; set; }
        //[JsonIgnore]
        //public Medico Medico { get; set; }
        public int PacienteId { get; set; }
        //[JsonIgnore]
        //public Paciente Paciente { get; set; }
        public int HorarioId { get; set; }
        //[JsonIgnore]
        //public HorarioDisponivel Horario { get; set; }
    }
}
