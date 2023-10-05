using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace cldv6212_part_2_function 
{
    public class VaccinationDataContext : DbContext
    {
        public DbSet<VaccinationDataModel> VaccinationData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("DbContextConnectionString"));
        }
    }
}
