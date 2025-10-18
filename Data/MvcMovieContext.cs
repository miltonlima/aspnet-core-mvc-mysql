using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;

namespace MvcMovie.Data
{
    public class MvcMovieContext : DbContext
    {
        public MvcMovieContext (DbContextOptions<MvcMovieContext> options)
            : base(options)
        {
        }

    public DbSet<MvcMovie.Models.Movie> Movie { get; set; } = default!;
    public DbSet<MvcMovie.Models.Pessoa> Pessoa { get; set; } = default!;
    public DbSet<MvcMovie.Models.Turma> Turma { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Garantir que o enum StatusTurma seja persistido como INT
        modelBuilder.Entity<MvcMovie.Models.Turma>()
            .Property(t => t.Status)
            .HasConversion<int>();

        // DataCriacao gerada pelo banco (assume coluna DATETIME / TIMESTAMP com DEFAULT CURRENT_TIMESTAMP)
        modelBuilder.Entity<MvcMovie.Models.Turma>()
            .Property(t => t.DataCriacao)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
    }
}
