using InsuranceSimpleApi.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InsuranceSimpleApi.Application.Services;

public class InsuranceReportService
{
    private readonly IApplicationDbContext _context;

    public InsuranceReportService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<object> Details, decimal TotalPotentialRevenue, int OfferCount)> GetSystemAnalysisAsync()
    {
        
        using (var writer = new StreamWriter("system_logs.txt", append: true))
        {
            await writer.WriteLineAsync($"{DateTime.Now} - Analiz raporu oluşturuluyor...");
        }

        var allOffersQuery = _context.Users
            .SelectMany(user => user.Offers); 

        var offerList = await allOffersQuery.ToListAsync();

        var userBasedDetails = await _context.Users
            .Where(u => u.Offers.Any())
            .Select(u => new
            {
                User = u.Username,
                
                Insurances = u.Offers.Select(o => o.Name).ToList(),
                TotalValue = u.Offers.Sum(o => o.BasePrice)
            })
            .ToListAsync();

        decimal totalRevenue = offerList.Sum(x => x.BasePrice);
        int count = offerList.Count;

        return (userBasedDetails, totalRevenue, count);
    }
}