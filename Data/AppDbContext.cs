using InsuranceSimpleApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace InsuranceSimpleApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<InsuranceType> InsuranceTypes { get; set; }
        public DbSet<User> Users { get; set; }
    }
}