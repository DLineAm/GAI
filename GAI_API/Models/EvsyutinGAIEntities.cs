using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GAI_API.Models
{
    public class EvsyutinGAIEntities : DbContext
    {
        public EvsyutinGAIEntities(DbContextOptions opt) : base(opt)
        {
            
        }

        public EvsyutinGAIEntities()
        {
            
        }

        public virtual DbSet<Drivers> Drivers { get; set; }
        public virtual DbSet<Region_Codes> Region_Codes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
            {
                var root = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                var conStr = root.GetConnectionString("DefaultConStr");
                optionsBuilder.UseSqlServer(conStr);
            }
        }
    }
}