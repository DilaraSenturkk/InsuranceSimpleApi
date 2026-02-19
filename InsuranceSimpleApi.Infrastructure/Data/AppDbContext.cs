using InsuranceSimpleApi.Infrastructure;
using InsuranceSimpleApi.Domain.Models;
using InsuranceSimpleApi.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace InsuranceSimpleApi.Infrastructure.Data
{
    public class AppDbContext : DbContext, IApplicationDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<InsuranceType> InsuranceTypes { get; set; }
        public DbSet<User> Users { get; set; }
    }
}