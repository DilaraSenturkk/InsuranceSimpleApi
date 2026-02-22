using InsuranceSimpleApi.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InsuranceSimpleApi.Infrastructure.Data
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<InsuranceType> InsuranceTypes { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}