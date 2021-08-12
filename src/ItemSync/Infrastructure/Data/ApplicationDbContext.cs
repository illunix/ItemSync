using ItemSync.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDBXLib.Definitions.WotLK;

namespace ItemSync.Infrastrtucture.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ItemTemplate> ItemTemplate { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ItemTemplate>()
                .ToTable("item_template")
                .HasKey(q => q.Entry);
        }
    }
}
