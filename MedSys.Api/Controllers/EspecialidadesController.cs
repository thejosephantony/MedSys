using MedSys.Api.Data;
using MedSys.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedSys.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EspecialidadesController : ControllerBase
{
    private readonly MedSysDbContext _context;

    public EspecialidadesController(MedSysDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Especialidade>>> Listar()
    {
        return await _context.Especialidades.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Especialidade>> BuscarPorId(int id)
    {
        var especialidade = await _context.Especialidades.FindAsync(id);

        if (especialidade == null)
            return NotFound("Especialidade não encontrada.");

        return especialidade;
    }

    [HttpPost]
    public async Task<ActionResult<Especialidade>> Criar(Especialidade especialidade)
    {
        _context.Especialidades.Add(especialidade);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(BuscarPorId), new { id = especialidade.Id }, especialidade);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, Especialidade especialidadeAtualizada)
    {
        var especialidade = await _context.Especialidades.FindAsync(id);

        if (especialidade == null)
            return NotFound("Especialidade não encontrada.");

        especialidade.Nome = especialidadeAtualizada.Nome;
        especialidade.Descricao = especialidadeAtualizada.Descricao;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(int id)
    {
        var especialidade = await _context.Especialidades.FindAsync(id);

        if (especialidade == null)
            return NotFound("Especialidade não encontrada.");

        _context.Especialidades.Remove(especialidade);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}