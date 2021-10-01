using Microsoft.EntityFrameworkCore;

namespace Database {

public class OrangeBushContext : DbContext
{
    public OrangeBushContext(DbContextOptions<OrangeBushContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<UserAdvice> UserAdvices{get;set;}
    public DbSet<Advice> Advices {get;set;}
    public DbSet<Board> Boards {get;set;}
    public DbSet<Dashboard> Dashboards {get;set;}
    public DbSet<Measurement> Measurements {get;set;}
    public DbSet<MeasurementDefinition> MeasurementDefinitions  {get;set;}
    public DbSet<Note> Notes {get;set;}
    public DbSet<Plant> Plants {get;set;}
    public DbSet<UserPlant> UserPlants {get;set;}
    
    

     protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserAdvice>().HasKey(a => new { a.AdviceId, a.UserId});
    }
}

}