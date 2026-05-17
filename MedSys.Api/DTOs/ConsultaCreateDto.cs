namespace MedSys.Api.DTOs;

public class ConsultaCreateDto
{
    public int MedicoId { get; set; }
    public int PacienteId { get; set; }

    public DateTime DataHora { get; set; }

    public string ModalidadeAtendimento { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? Observacoes { get; set; }

    public decimal Valor { get; set; }
    public string? FormaPagamento { get; set; }
    public string? LocalAtendimento { get; set; }
}