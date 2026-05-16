using MedSys.Api.Enums;

namespace MedSys.Api.Models;

public class Consulta
{
    public int Id { get; set; }

    public int MedicoId { get; set; }
    public Medico Medico { get; set; } = null!;

    public int PacienteId { get; set; }
    public Paciente Paciente { get; set; } = null!;

    public DateTime DataHora { get; set; }

    public string ModalidadeAtendimento { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public StatusConsulta Status { get; set; } = StatusConsulta.Agendada;

    public string Diagnostico { get; set; } = string.Empty;

    public string Prescricao { get; set; } = string.Empty;

    public string Exames { get; set; } = string.Empty;

    public string Observacoes { get; set; } = string.Empty;

    public decimal Valor { get; set; }

    public string FormaPagamento { get; set; } = string.Empty;

    public DateTime DataCriacao { get; set; } = DateTime.Now;

    public DateTime DataAtualizacao { get; set; } = DateTime.Now;

    public string LocalAtendimento { get; set; } = string.Empty;

    public string Feedback { get; set; } = string.Empty;

    public int Avaliacao { get; set; }
}