using MedSys.Api.Enums;

namespace MedSys.Api.DTOs;

public class ConsultaUpdateDto
{
    public int MedicoId { get; set; }
    public int PacienteId { get; set; }

    public DateTime DataHora { get; set; }

    public string ModalidadeAtendimento { get; set; } = string.Empty;
    public string? Descricao { get; set; }

    public StatusConsulta Status { get; set; }

    public string? Diagnostico { get; set; }
    public string? Prescricao { get; set; }
    public string? Exames { get; set; }
    public string? Observacoes { get; set; }

    public decimal Valor { get; set; }
    public string? FormaPagamento { get; set; }
    public string? LocalAtendimento { get; set; }

    public string? Feedback { get; set; }
    public int Avaliacao { get; set; }
}