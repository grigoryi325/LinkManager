using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SaveLink.Models
{
    public class DataContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }   
        public DbSet<Link> Links { get; set; }
        public DbSet<Shared> Shareds { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Shared>().HasMany(c => c.Links)
                .WithMany(s => s.Shareds)
                .Map(t => t.MapLeftKey("SharedId")
                .MapRightKey("LinkId")
                .ToTable("SharedLink"));
        }
        
    }
}