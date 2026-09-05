using Microsoft.EntityFrameworkCore;
using Notely.Entities;

namespace Notely.Data;

public class NotelyDbContext : DbContext
{
    public NotelyDbContext(DbContextOptions<NotelyDbContext> options) : base(options)
    {
    }

    public DbSet<Compte> Comptes => Set<Compte>();
    public DbSet<CompteAccesPage> AccesPages => Set<CompteAccesPage>();
    public DbSet<Cours> Cours => Set<Cours>();
    public DbSet<Chapitre> Chapitres => Set<Chapitre>();
    public DbSet<Todo> Todos => Set<Todo>();
    public DbSet<Note> Notes => Set<Note>();
    public DbSet<Evenement> Evenements => Set<Evenement>();
    public DbSet<Seance> Seances => Set<Seance>();
    public DbSet<ExerciceSeance> ExercicesSeance => Set<ExerciceSeance>();
    public DbSet<Serie> Series => Set<Serie>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("public");

        modelBuilder.Entity<Compte>()
            .HasIndex(c => c.Email)
            .IsUnique();

        modelBuilder.Entity<CompteAccesPage>()
            .HasOne(a => a.CompteNav)
            .WithMany(c => c.AccesPages)
            .HasForeignKey(a => a.IdCompte)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CompteAccesPage>()
            .HasIndex(a => new { a.IdCompte, a.CodePage })
            .IsUnique();

        modelBuilder.Entity<Cours>()
            .HasOne(c => c.CompteNav)
            .WithMany(co => co.Cours)
            .HasForeignKey(c => c.IdCompte)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Chapitre>()
            .HasOne(ch => ch.CoursNav)
            .WithMany(c => c.Chapitres)
            .HasForeignKey(ch => ch.IdCours)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Todo>()
            .HasOne(t => t.CoursNav)
            .WithMany(c => c.Todos)
            .HasForeignKey(t => t.IdCours)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Todo>()
            .HasOne(t => t.CompteNav)
            .WithMany(co => co.Todos)
            .HasForeignKey(t => t.IdCompte)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Note>()
            .HasOne(n => n.CompteNav)
            .WithMany(c => c.Notes)
            .HasForeignKey(n => n.IdCompte)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Evenement>()
            .HasOne(e => e.CompteNav)
            .WithMany(c => c.Evenements)
            .HasForeignKey(e => e.IdCompte)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Seance>()
            .HasOne(s => s.CompteNav)
            .WithMany(c => c.Seances)
            .HasForeignKey(s => s.IdCompte)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ExerciceSeance>()
            .HasOne(e => e.SeanceNav)
            .WithMany(s => s.ExercicesSeance)
            .HasForeignKey(e => e.IdSeance)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Serie>()
            .HasOne(s => s.ExerciceSeanceNav)
            .WithMany(e => e.Series)
            .HasForeignKey(s => s.IdExerciceSeance)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
