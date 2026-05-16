namespace MedSys.Api.Models;

public class Paciente
{
    public int Id { get; set; }

    public string PrimeiroNome { get; set; } = string.Empty;

    public string Sobrenome { get; set; } = string.Empty;

    public string CPF { get; set; } = string.Empty;

    public string Telefone { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Endereco { get; set; } = string.Empty;

    public string Nacionalidade { get; set; } = string.Empty;

    public string FotoPerfil { get; set; } = string.Empty;

    public string ModalidadeAtendimento { get; set; } = string.Empty;

    public DateTime DataNascimento { get; set; }

    public string Sexo { get; set; } = string.Empty;

    public string PlanoSaude { get; set; } = string.Empty;

    public List<Consulta> Consultas { get; set; } = new();

    // public List<Exame> Exames { get; set; } = new();
    // public List<Prescricao> Prescricoes { get; set; } = new();
    // public List<HistoricoMedico> HistoricoMedico { get; set; } = new();
    // public List<Pagamento> Pagamentos { get; set; } = new();
}