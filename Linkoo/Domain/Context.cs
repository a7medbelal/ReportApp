using Microsoft.EntityFrameworkCore;
using ReportApp.Model;
using ReportApp.Model.Enum;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;

namespace ReportApp.Domain
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }
        public DbSet<Model.AppUser> AppUsers { get; set; }
        public DbSet<Model.Report> Reports { get; set; }
        public DbSet<Model.Comment> Comments { get; set; }
        public DbSet<Model.Notification> Notifications { get; set; }    
        public DbSet<Model.Attachment> attachments {get; set; }
        public DbSet<Model.RoleFeature> RoleFeatures { get; set; }  


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Model.AppUser>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Report>()
            .HasIndex(r => r.Number)
            .IsUnique()
            .HasFilter("[Number] IS NOT NULL");



            #region Seed Admin User    
            var passwordHash = "OwsaNEXpDDAXEx+oo3zeNejRgwsl/ezkljw1uA0Thuw=";
            var passwordSalt = "iCkBAbFby/I7C2G5Hg2CWQ==";



            var superAdminId = new Guid("11111111-1111-1111-1111-111111111111");
            modelBuilder.Entity<AppUser>().HasData(
                new AppUser
                {
                    Id = superAdminId,
                    Fname = "System Admin",
                    Email = "admin@university.com",
                    HashedPassword = passwordHash,
                    role = Role.Admin,
                    SaltPassword = passwordSalt,
                    Createdby = "system"
                }
            );
            #endregion

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //here as i can see every quary i can see the sql sentacse about that query in the debug console
            //I made NoTracking prevent the change tracker
            //from tracking the entities  that will be slow the perfomance 
            optionsBuilder
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .LogTo(log => Debug.WriteLine(log), LogLevel.Information)
                .EnableSensitiveDataLogging();
        }

    }
}
