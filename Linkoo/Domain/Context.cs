using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design.Serialization;

namespace Linkoo.Domain
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }
        public DbSet<Model.AppUser> AppUsers { get; set; }
        public DbSet<Model.Post> Posts { get; set; }
        public DbSet<Model.Comment> Comments { get; set; }
        public DbSet<Model.Likes> Likes { get; set; }
        public DbSet<Model.Tags> Tags { get; set; }


    }
}
