#pragma warning disable 1591
using Microsoft.EntityFrameworkCore;

namespace BackendBPR.Database 
{
    /// <summary>
    /// Class that represents the database context
    /// </summary>
    public class OrangeBushContext : DbContext
    {
        public OrangeBushContext(DbContextOptions<OrangeBushContext> options)
            : base(options)
        {
            
        }
        
        public OrangeBushContext()
        {
            
        }
        /// <summary>
        /// Representation of a user
        /// </summary>
        /// <value></value>
        public virtual DbSet<User> Users { get; set; }
        /// <summary>
        /// Representation of a tag for a plant
        /// </summary>
        /// <value></value>
        public virtual DbSet<Tag> Tags { get; set; }
        /// <summary>
        /// Representation of a creation, like, etc action for an advice
        /// </summary>
        /// <value></value>
        public virtual DbSet<UserAdvice> UserAdvices{get;set;}
        /// <summary>
        /// Representation of a piece of advice given by a user
        /// </summary>
        /// <value></value>
        public virtual DbSet<Advice> Advices {get;set;}
        /// <summary>
        /// Representation of a single board of a whole dashboard
        /// </summary>
        /// <value></value>
        public virtual DbSet<Board> Boards {get;set;}
        /// <summary>
        /// Representation of a full dashboard
        /// </summary>
        /// <value></value>
        public virtual DbSet<Dashboard> Dashboards {get;set;}
        /// <summary>
        /// Representation of a single data point for a plant measurementDefinition
        /// </summary>
        /// <value></value>
        public virtual DbSet<Measurement> Measurements {get;set;}
        /// <summary>
        /// Representation of the default measurement definitions that define a plant
        /// </summary>
        /// <value></value>
        public virtual DbSet<MeasurementDefinition> MeasurementDefinitions  {get;set;}    
        /// <summary>
        /// Representation of a custom made measurementDefinition defined by the user
        /// </summary>
        /// <value></value>    
        public virtual DbSet<CustomMeasurementDefinition> CustomMeasurementDefinitions  {get;set;}
        /// <summary>
        /// Representation of a personal note of a user
        /// </summary>
        /// <value></value>
        public virtual DbSet<Note> Notes {get;set;}
        /// <summary>
        /// Representation of a plant
        /// </summary>
        /// <value></value>
        public virtual DbSet<Plant> Plants {get;set;}
        /// <summary>
        /// Representation of a personal plant of a user
        /// </summary>
        /// <value></value>
        public virtual DbSet<UserPlant> UserPlants {get;set;}
        
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAdvice>().HasKey(a => new { a.AdviceId, a.UserId});
            modelBuilder.Entity<User>().HasIndex(a=> a.Username).IsUnique();
            modelBuilder.Entity<User>().HasIndex(a=> a.Email).IsUnique();
        }
    }

}