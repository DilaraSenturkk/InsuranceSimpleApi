using InsuranceSimpleApi.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceSimpleApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly InsuranceReportService _reportService;

        public ReportsController(InsuranceReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("system-analysis")]
        public async Task<IActionResult> GetSystemAnalysis()
        {
     
            var (details, totalRevenue, offerCount) = await _reportService.GetSystemAnalysisAsync();

            return Ok(new
            {
                Status = "Success",
                Message = "Sistem analizi başarıyla tamamlandı.",
                Data = new
                {
                    AllUserOffers = details,
                    TotalPotentialRevenue = totalRevenue,
                    TotalOfferCount = offerCount
                }
            });
        }
    }
}