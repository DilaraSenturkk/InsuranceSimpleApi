using InsuranceSimpleApi.Data;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceSimpleApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var query = _context.Users.AsQueryable();

            var totalCount = query.Count();

            var users = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                totalCount,
                page,
                pageSize,
                data = users
            });
        }
    }
}
