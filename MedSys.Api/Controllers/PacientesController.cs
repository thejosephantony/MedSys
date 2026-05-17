using MedSys.Api.Data;
using MedSys.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedSys.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PacientesController : ControllerBase
{
    private readonly MedSysDbContext _context;

    public PacientesController(MedSysDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Paciente>>> Listar()
    {
        return await _context.Pacientes.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Paciente>> BuscarPorId(int id)
    {
        var paciente = await _context.Pacientes.FindAsync(id);

        if (paciente == null)
            return NotFound("Paciente não encontrado.");

        return paciente;
    }

    [HttpPost]
    public async Task<ActionResult<Paciente>> Criar(Paciente paciente)
    {
        _context.Pacientes.Add(paciente);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(BuscarPorId), new { id = paciente.Id }, paciente);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, Paciente pacienteAtualizado)
    {
        var paciente = await _context.Pacientes.FindAsync(id);

        if (paciente == null)
            return NotFound("Paciente não encontrado.");

        paciente.PrimeiroNome = pacienteAtualizado.PrimeiroNome;
        paciente.Sobrenome = pacienteAtualizado.Sobrenome;
        paciente.CPF = pacienteAtualizado.CPF;
        paciente.Telefone = pacienteAtualizado.Telefone;
        paciente.Email = pacienteAtualizado.Email;
        paciente.Endereco = pacienteAtualizado.Endereco;
        paciente.Nacionalidade = pacienteAtualizado.Nacionalidade;
        paciente.FotoPerfil = pacienteAtualizado.FotoPerfil;
        paciente.ModalidadeAtendimento = pacienteAtualizado.ModalidadeAtendimento;
        paciente.DataNascimento = pacienteAtualizado.DataNascimento;
        paciente.Sexo = pacienteAtualizado.Sexo;
        paciente.PlanoSaude = pacienteAtualizado.PlanoSaude;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(int id)
    {
        var paciente = await _context.Pacientes.FindAsync(id);

        if (paciente == null)
            return NotFound("Paciente não encontrado.");

        _context.Pacientes.Remove(paciente);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}