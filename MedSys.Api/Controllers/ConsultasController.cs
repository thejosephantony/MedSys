using MedSys.Api.Data;
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
    public async Task<ActionResult<List<Consulta>>> Listar()
    {
        return await _context.Consultas
            .Include(c => c.Medico!)
                .ThenInclude(m => m.Especialidade)
            .Include(c => c.Paciente)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Consulta>> BuscarPorId(int id)
    {
        var consulta = await _context.Consultas
            .Include(c => c.Medico!)
                .ThenInclude(m => m.Especialidade)
            .Include(c => c.Paciente)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (consulta == null)
            return NotFound("Consulta não encontrada.");

        return consulta;
    }

    [HttpPost]
    public async Task<ActionResult<Consulta>> Criar(Consulta consulta)
    {
        consulta.DataHora = DateTime.SpecifyKind(consulta.DataHora, DateTimeKind.Utc);
        var medicoExiste = await _context.Medicos
            .AnyAsync(m => m.Id == consulta.MedicoId);

        if (!medicoExiste)
            return BadRequest("Médico informado não existe.");

        var pacienteExiste = await _context.Pacientes
            .AnyAsync(p => p.Id == consulta.PacienteId);

        if (!pacienteExiste)
            return BadRequest("Paciente informado não existe.");

        if (consulta.DataHora < DateTime.UtcNow)
            return BadRequest("Não é possível agendar consulta no passado.");

        var medicoOcupado = await _context.Consultas.AnyAsync(c =>
            c.MedicoId == consulta.MedicoId &&
            c.DataHora == consulta.DataHora &&
            c.Status == StatusConsulta.Agendada);

        if (medicoOcupado)
            return BadRequest("O médico já possui uma consulta agendada nesse horário.");

        var pacienteOcupado = await _context.Consultas.AnyAsync(c =>
            c.PacienteId == consulta.PacienteId &&
            c.DataHora == consulta.DataHora &&
            c.Status == StatusConsulta.Agendada);

        if (pacienteOcupado)
            return BadRequest("O paciente já possui uma consulta agendada nesse horário.");

        consulta.Status = StatusConsulta.Agendada;
        consulta.DataCriacao = DateTime.UtcNow;
        consulta.DataAtualizacao = DateTime.UtcNow;

        _context.Consultas.Add(consulta);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(BuscarPorId), new { id = consulta.Id }, consulta);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, Consulta consultaAtualizada)
    {
        consultaAtualizada.DataHora = DateTime.SpecifyKind(
        consultaAtualizada.DataHora,
        DateTimeKind.Utc
        );
        var consulta = await _context.Consultas.FindAsync(id);

        if (consulta == null)
            return NotFound("Consulta não encontrada.");

        var medicoExiste = await _context.Medicos
            .AnyAsync(m => m.Id == consultaAtualizada.MedicoId);

        if (!medicoExiste)
            return BadRequest("Médico informado não existe.");

        var pacienteExiste = await _context.Pacientes
            .AnyAsync(p => p.Id == consultaAtualizada.PacienteId);

        if (!pacienteExiste)
            return BadRequest("Paciente informado não existe.");

        if (consultaAtualizada.DataHora < DateTime.UtcNow)
            return BadRequest("Não é possível reagendar consulta para uma data no passado.");

        var medicoOcupado = await _context.Consultas.AnyAsync(c =>
            c.Id != id &&
            c.MedicoId == consultaAtualizada.MedicoId &&
            c.DataHora == consultaAtualizada.DataHora &&
            c.Status == StatusConsulta.Agendada);

        if (medicoOcupado)
            return BadRequest("O médico já possui uma consulta agendada nesse horário.");

        var pacienteOcupado = await _context.Consultas.AnyAsync(c =>
            c.Id != id &&
            c.PacienteId == consultaAtualizada.PacienteId &&
            c.DataHora == consultaAtualizada.DataHora &&
            c.Status == StatusConsulta.Agendada);

        if (pacienteOcupado)
            return BadRequest("O paciente já possui uma consulta agendada nesse horário.");

        consulta.MedicoId = consultaAtualizada.MedicoId;
        consulta.PacienteId = consultaAtualizada.PacienteId;
        consulta.DataHora = consultaAtualizada.DataHora;
        consulta.ModalidadeAtendimento = consultaAtualizada.ModalidadeAtendimento;
        consulta.Descricao = consultaAtualizada.Descricao;
        consulta.Status = consultaAtualizada.Status;
        consulta.Diagnostico = consultaAtualizada.Diagnostico;
        consulta.Prescricao = consultaAtualizada.Prescricao;
        consulta.Exames = consultaAtualizada.Exames;
        consulta.Observacoes = consultaAtualizada.Observacoes;
        consulta.Valor = consultaAtualizada.Valor;
        consulta.FormaPagamento = consultaAtualizada.FormaPagamento;
        consulta.LocalAtendimento = consultaAtualizada.LocalAtendimento;
        consulta.Feedback = consultaAtualizada.Feedback;
        consulta.Avaliacao = consultaAtualizada.Avaliacao;
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
}