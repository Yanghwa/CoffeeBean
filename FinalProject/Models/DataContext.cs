namespace FinalProject.Models
{
    using global::CoffeeBean.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class DataContext : DbContext
    {
        // Your context has been configured to use a 'DataContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'FinalProject.Models.DataContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'DataContext' 
        // connection string in the application configuration file.
        public DataContext()
            : base("name=DefaultConnection")
        {
        }
        public virtual DbSet<CoffeeBean> CoffeeBeans { get; set; }
        public virtual DbSet<Bean> Beans { get; set; }
        public virtual DbSet<Coffee> Coffees { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<IdentityUserLogin>();
            modelBuilder.Ignore<IdentityUserRole>();
            modelBuilder.Ignore<IdentityUserClaim>();

            modelBuilder.Entity<Bean>()
                .HasMany(e => e.Coffees)
                .WithRequired(e => e.Bean)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Coffee>()
                .HasMany(e => e.Beans)
                .WithRequired(e => e.Coffee)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Rating>()
                .HasRequired(e => e.User)
                .WithMany(e => e.Ratings)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Rating>()
                .HasRequired(e => e.Bean)
                .WithMany(e => e.Ratings)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers")
                .HasMany(e => e.Ratings)
                .WithRequired(e => e.User);

        }

        //public System.Data.Entity.DbSet<CoffeeBean.Models.ApplicationUser> ApplicationUsers { get; set; }
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