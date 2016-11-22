namespace MassDefect.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using MassDefect.Models;
    using Newtonsoft.Json;
    public class MassDefectContext : DbContext
    {
        // Your context has been configured to use a 'MassDefectContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'MassDefect.Data.MassDefectContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'MassDefectContext' 
        // connection string in the application configuration file.
        public MassDefectContext()
            : base("name=MassDefectContext")
        {
            //Database.SetInitializer(new DropCreateDatabaseAlways<MassDefectContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Anomaly>()
             .HasMany<Person>(s => s.PersonVictims)
             .WithMany(c => c.Anomalies)
             .Map(cs =>
             {
                 cs.MapLeftKey("AnomalyId");
                 cs.MapRightKey("PersonId");
                 cs.ToTable("AnomalyVictims");
             });
        }

        public virtual IDbSet<Anomaly> Anomalies { get; set; }

        public virtual IDbSet<Person> Persons { get; set; }

        public virtual IDbSet<Planet> Planets  { get; set; }
        public virtual IDbSet<SolarSystem> SolarSystems { get; set; }
        public virtual IDbSet<Star> Stars { get; set; }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}