using MedSys.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MedSys.Api.Data;

public class MedSysDbContext : DbContext
{
    public MedSysDbContext(DbContextOptions<MedSysDbContext> options)
        : base(options)
    {
    }

    public DbSet<Medico> Medicos { get; set; } = null!;

    public DbSet<Paciente> Pacientes { get; set; } = null!;

    public DbSet<Consulta> Consultas { get; set; } = null!;

    public DbSet<Especialidade> Especialidades { get; set; } = null!;
}