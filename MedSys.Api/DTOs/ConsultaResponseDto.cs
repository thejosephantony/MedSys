using MedSys.Api.Enums;

namespace MedSys.Api.DTOs;

public class ConsultaResponseDto
{
    public int Id { get; set; }

    public int MedicoId { get; set; }
    public string NomeMedico { get; set; } = string.Empty;
    public string Especialidade { get; set; } = string.Empty;

    public int PacienteId { get; set; }
    public string NomePaciente { get; set; } = string.Empty;

    public DateTime DataHora { get; set; }

    public string ModalidadeAtendimento { get; set; } = string.Empty;
    public string? Descricao { get; set; }

    public StatusConsulta Status { get; set; }

    public decimal Valor { get; set; }
    public string? FormaPagamento { get; set; }
    public string? LocalAtendimento { get; set; }

    public DateTime DataCriacao { get; set; }
    public DateTime DataAtualizacao { get; set; }
}