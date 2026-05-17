using MedSys.Api.Data;
using MedSys.Api.DTOs;
using MedSys.Api.Enums;
using MedSys.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedSys.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConsultasController : ControllerBase
{
    private readonly MedSysDbContext _context;

    public ConsultasController(MedSysDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<ConsultaResponseDto>>> Listar()
    {
        var consultas = await _context.Consultas
            .OrderBy(c => c.DataHora)
            .Select(c => new ConsultaResponseDto
            {
                Id = c.Id,

                MedicoId = c.MedicoId,
                NomeMedico = c.Medico != null
                    ? c.Medico.PrimeiroNome + " " + c.Medico.Sobrenome
                    : string.Empty,

                Especialidade = c.Medico != null && c.Medico.Especialidade != null
                    ? c.Medico.Especialidade.Nome
                    : string.Empty,

                PacienteId = c.PacienteId,
                NomePaciente = c.Paciente != null
                    ? c.Paciente.PrimeiroNome + " " + c.Paciente.Sobrenome
                    : string.Empty,

                DataHora = c.DataHora,
                ModalidadeAtendimento = c.ModalidadeAtendimento,
                Descricao = c.Descricao,
                Status = c.Status,
                Valor = c.Valor,
                FormaPagamento = c.FormaPagamento,
                LocalAtendimento = c.LocalAtendimento,
                DataCriacao = c.DataCriacao,
                DataAtualizacao = c.DataAtualizacao
            })
            .ToListAsync();

        return Ok(consultas);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ConsultaResponseDto>> BuscarPorId(int id)
    {
        var consulta = await _context.Consultas
            .Where(c => c.Id == id)
            .Select(c => new ConsultaResponseDto
            {
                Id = c.Id,

                MedicoId = c.MedicoId,
                NomeMedico = c.Medico != null
                    ? c.Medico.PrimeiroNome + " " + c.Medico.Sobrenome
                    : string.Empty,

                Especialidade = c.Medico != null && c.Medico.Especialidade != null
                    ? c.Medico.Especialidade.Nome
                    : string.Empty,

                PacienteId = c.PacienteId,
                NomePaciente = c.Paciente != null
                    ? c.Paciente.PrimeiroNome + " " + c.Paciente.Sobrenome
                    : string.Empty,

                DataHora = c.DataHora,
                ModalidadeAtendimento = c.ModalidadeAtendimento,
                Descricao = c.Descricao,
                Status = c.Status,
                Valor = c.Valor,
                FormaPagamento = c.FormaPagamento,
                LocalAtendimento = c.LocalAtendimento,
                DataCriacao = c.DataCriacao,
                DataAtualizacao = c.DataAtualizacao
            })
            .FirstOrDefaultAsync();

        if (consulta == null)
            return NotFound("Consulta não encontrada.");

        return Ok(consulta);
    }

    [HttpPost]
    public async Task<ActionResult<ConsultaResponseDto>> Criar(ConsultaCreateDto dto)
    {
        var dataHoraUtc = GarantirUtc(dto.DataHora);

        var medico = await _context.Medicos
            .Include(m => m.Especialidade)
            .FirstOrDefaultAsync(m => m.Id == dto.MedicoId);

        if (medico == null)
            return BadRequest("Médico informado não existe.");

        var paciente = await _context.Pacientes
            .FirstOrDefaultAsync(p => p.Id == dto.PacienteId);

        if (paciente == null)
            return BadRequest("Paciente informado não existe.");

        if (dataHoraUtc < DateTime.UtcNow)
            return BadRequest("Não é possível agendar consulta no passado.");

        var medicoOcupado = await _context.Consultas.AnyAsync(c =>
            c.MedicoId == dto.MedicoId &&
            c.DataHora == dataHoraUtc &&
            c.Status == StatusConsulta.Agendada);

        if (medicoOcupado)
            return BadRequest("O médico já possui uma consulta agendada nesse horário.");

        var pacienteOcupado = await _context.Consultas.AnyAsync(c =>
            c.PacienteId == dto.PacienteId &&
            c.DataHora == dataHoraUtc &&
            c.Status == StatusConsulta.Agendada);

        if (pacienteOcupado)
            return BadRequest("O paciente já possui uma consulta agendada nesse horário.");

        var consulta = new Consulta
        {
            MedicoId = dto.MedicoId,
            PacienteId = dto.PacienteId,
            DataHora = dataHoraUtc,
            ModalidadeAtendimento = dto.ModalidadeAtendimento,
            Descricao = dto.Descricao ?? string.Empty,
            Observacoes = dto.Observacoes ?? string.Empty,
            FormaPagamento = dto.FormaPagamento ?? string.Empty,
            LocalAtendimento = dto.LocalAtendimento ?? string.Empty,
            Status = StatusConsulta.Agendada,
            DataCriacao = DateTime.UtcNow,
            DataAtualizacao = DateTime.UtcNow
        };

        _context.Consultas.Add(consulta);
        await _context.SaveChangesAsync();

        var responseDto = new ConsultaResponseDto
        {
            Id = consulta.Id,

            MedicoId = consulta.MedicoId,
            NomeMedico = medico.PrimeiroNome + " " + medico.Sobrenome,
            Especialidade = medico.Especialidade != null
                ? medico.Especialidade.Nome
                : string.Empty,

            PacienteId = consulta.PacienteId,
            NomePaciente = paciente.PrimeiroNome + " " + paciente.Sobrenome,

            DataHora = consulta.DataHora,
            ModalidadeAtendimento = consulta.ModalidadeAtendimento,
            Descricao = consulta.Descricao,
            Status = consulta.Status,
            Valor = consulta.Valor,
            FormaPagamento = consulta.FormaPagamento,
            LocalAtendimento = consulta.LocalAtendimento,
            DataCriacao = consulta.DataCriacao,
            DataAtualizacao = consulta.DataAtualizacao
        };

        return CreatedAtAction(nameof(BuscarPorId), new { id = consulta.Id }, responseDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, ConsultaUpdateDto dto)
    {
        var dataHoraUtc = GarantirUtc(dto.DataHora);

        var consulta = await _context.Consultas.FindAsync(id);

        if (consulta == null)
            return NotFound("Consulta não encontrada.");

        var medicoExiste = await _context.Medicos
            .AnyAsync(m => m.Id == dto.MedicoId);

        if (!medicoExiste)
            return BadRequest("Médico informado não existe.");

        var pacienteExiste = await _context.Pacientes
            .AnyAsync(p => p.Id == dto.PacienteId);

        if (!pacienteExiste)
            return BadRequest("Paciente informado não existe.");

        if (dataHoraUtc < DateTime.UtcNow)
            return BadRequest("Não é possível reagendar consulta para uma data no passado.");

        var medicoOcupado = await _context.Consultas.AnyAsync(c =>
            c.Id != id &&
            c.MedicoId == dto.MedicoId &&
            c.DataHora == dataHoraUtc &&
            c.Status == StatusConsulta.Agendada);

        if (medicoOcupado)
            return BadRequest("O médico já possui uma consulta agendada nesse horário.");

        var pacienteOcupado = await _context.Consultas.AnyAsync(c =>
            c.Id != id &&
            c.PacienteId == dto.PacienteId &&
            c.DataHora == dataHoraUtc &&
            c.Status == StatusConsulta.Agendada);

        if (pacienteOcupado)
            return BadRequest("O paciente já possui uma consulta agendada nesse horário.");

        consulta.MedicoId = dto.MedicoId;
        consulta.PacienteId = dto.PacienteId;
        consulta.DataHora = dataHoraUtc;
        consulta.ModalidadeAtendimento = dto.ModalidadeAtendimento;
        consulta.Descricao = dto.Descricao ?? string.Empty;
        consulta.Diagnostico = dto.Diagnostico ?? string.Empty;
        consulta.Prescricao = dto.Prescricao ?? string.Empty;
        consulta.Exames = dto.Exames ?? string.Empty;
        consulta.Observacoes = dto.Observacoes ?? string.Empty;
        consulta.FormaPagamento = dto.FormaPagamento ?? string.Empty;
        consulta.LocalAtendimento = dto.LocalAtendimento ?? string.Empty;
        consulta.Feedback = dto.Feedback ?? string.Empty;
        consulta.Avaliacao = dto.Avaliacao;
        consulta.DataAtualizacao = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(int id)
    {
        var consulta = await _context.Consultas.FindAsync(id);

        if (consulta == null)
            return NotFound("Consulta não encontrada.");

        _context.Consultas.Remove(consulta);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch("{id}/cancelar")]
    public async Task<IActionResult> Cancelar(int id)
    {
        var consulta = await _context.Consultas.FindAsync(id);

        if (consulta == null)
            return NotFound("Consulta não encontrada.");

        if (consulta.Status == StatusConsulta.Cancelada)
            return BadRequest("A consulta já está cancelada.");

        if (consulta.Status == StatusConsulta.Realizada)
            return BadRequest("Não é possível cancelar uma consulta já realizada.");

        consulta.Status = StatusConsulta.Cancelada;
        consulta.DataAtualizacao = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch("{id}/realizar")]
    public async Task<IActionResult> Realizar(int id)
    {
        var consulta = await _context.Consultas.FindAsync(id);

        if (consulta == null)
            return NotFound("Consulta não encontrada.");

        if (consulta.Status == StatusConsulta.Cancelada)
            return BadRequest("Não é possível realizar uma consulta cancelada.");

        if (consulta.Status == StatusConsulta.Realizada)
            return BadRequest("A consulta já está marcada como realizada.");

        consulta.Status = StatusConsulta.Realizada;
        consulta.DataAtualizacao = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static DateTime GarantirUtc(DateTime dataHora)
    {
        return dataHora.Kind switch
        {
            DateTimeKind.Utc => dataHora,
            DateTimeKind.Local => dataHora.ToUniversalTime(),
            _ => DateTime.SpecifyKind(dataHora, DateTimeKind.Utc)
        };
    }
}