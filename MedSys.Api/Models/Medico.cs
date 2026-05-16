namespace MedSys.Api.Models;

public class Medico
{
    public int Id { get; set; }

    public string PrimeiroNome { get; set; } = string.Empty;

    public string Sobrenome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;

    public string CRM { get; set; } = string.Empty;

    public string Telefone { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Endereco { get; set; } = string.Empty;

    public string Sexo { get; set; } = string.Empty;

    public DateTime DataNascimento { get; set; }

    public string FotoPerfil { get; set; } = string.Empty;

    public string Biografia { get; set; } = string.Empty;

    public string Formacao { get; set; } = string.Empty;

    public string Experiencia { get; set; } = string.Empty;

    public string LocalAtendimento { get; set; } = string.Empty;

    public string ModalidadesAtendimento { get; set; } = string.Empty;

    public int Avaliacao { get; set; }

    public int EspecialidadeId { get; set; }

    public Especialidade Especialidade { get; set; } = null!;

    public List<Consulta> Consultas { get; set; } = new();

    // public string Idiomas { get; set; } = string.Empty;
    // public string Certificacoes { get; set; } = string.Empty;
    // public string RedesSociais { get; set; } = string.Empty;
    // public List<Exame> Exames { get; set; } = new();
    // public List<Prescricao> Prescricoes { get; set; } = new();
    // public List<HistoricoMedico> HistoricoMedico { get; set; } = new();
    // public List<Pagamento> Pagamentos { get; set; } = new();
    // public List<Agendamento> Agendamentos { get; set; } = new();

}