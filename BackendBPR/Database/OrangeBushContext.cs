using Microsoft.EntityFrameworkCore;

namespace BackendBPR.Database 
{

    public class OrangeBushContext : DbContext
    {
        public OrangeBushContext(DbContextOptions<OrangeBushContext> options)
            : base(options)
        {
            
        }
        
        public OrangeBushContext()
        {
            
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<UserAdvice> UserAdvices{get;set;}
        public virtual DbSet<Advice> Advices {get;set;}
        public virtual DbSet<Board> Boards {get;set;}
        public virtual DbSet<Dashboard> Dashboards {get;set;}
        public virtual DbSet<Measurement> Measurements {get;set;}
        public virtual DbSet<MeasurementDefinition> MeasurementDefinitions  {get;set;}        
        public virtual DbSet<CustomMeasurementDefinition> CustomMeasurementDefinitions  {get;set;}
        public virtual DbSet<Note> Notes {get;set;}
        public virtual DbSet<Plant> Plants {get;set;}
        public virtual DbSet<UserPlant> UserPlants {get;set;}
        
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAdvice>().HasKey(a => new { a.AdviceId, a.UserId});
        }
    }

}