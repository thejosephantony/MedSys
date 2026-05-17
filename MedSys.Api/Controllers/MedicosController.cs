using MedSys.Api.Data;
using MedSys.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedSys.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicosController : ControllerBase
{
    private readonly MedSysDbContext _context;

    public MedicosController(MedSysDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Medico>>> Listar()
    {
        return await _context.Medicos
            .Include(m => m.Especialidade)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Medico>> BuscarPorId(int id)
    {
        var medico = await _context.Medicos
            .Include(m => m.Especialidade)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (medico == null)
            return NotFound("Médico não encontrado.");

        return medico;
    }

    [HttpPost]
    public async Task<ActionResult<Medico>> Criar(Medico medico)
    {
        var especialidadeExiste = await _context.Especialidades
            .AnyAsync(e => e.Id == medico.EspecialidadeId);

        if (!especialidadeExiste)
            return BadRequest("Especialidade informada não existe.");

        _context.Medicos.Add(medico);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(BuscarPorId), new { id = medico.Id }, medico);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, Medico medicoAtualizado)
    {
        var medico = await _context.Medicos.FindAsync(id);

        if (medico == null)
            return NotFound("Médico não encontrado.");

        var especialidadeExiste = await _context.Especialidades
            .AnyAsync(e => e.Id == medicoAtualizado.EspecialidadeId);

        if (!especialidadeExiste)
            return BadRequest("Especialidade informada não existe.");

        medico.PrimeiroNome = medicoAtualizado.PrimeiroNome;
        medico.Sobrenome = medicoAtualizado.Sobrenome;
        medico.CPF = medicoAtualizado.CPF;
        medico.CRM = medicoAtualizado.CRM;
        medico.Telefone = medicoAtualizado.Telefone;
        medico.Email = medicoAtualizado.Email;
        medico.Endereco = medicoAtualizado.Endereco;
        medico.Sexo = medicoAtualizado.Sexo;
        medico.DataNascimento = medicoAtualizado.DataNascimento;
        medico.FotoPerfil = medicoAtualizado.FotoPerfil;
        medico.Biografia = medicoAtualizado.Biografia;
        medico.Formacao = medicoAtualizado.Formacao;
        medico.Experiencia = medicoAtualizado.Experiencia;
        medico.LocalAtendimento = medicoAtualizado.LocalAtendimento;
        medico.ModalidadesAtendimento = medicoAtualizado.ModalidadesAtendimento;
        medico.Avaliacao = medicoAtualizado.Avaliacao;
        medico.EspecialidadeId = medicoAtualizado.EspecialidadeId;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(int id)
    {
        var medico = await _context.Medicos.FindAsync(id);

        if (medico == null)
            return NotFound("Médico não encontrado.");

        _context.Medicos.Remove(medico);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}