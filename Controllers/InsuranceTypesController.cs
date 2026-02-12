using InsuranceSimpleApi.Data; 
using InsuranceSimpleApi.DTOs;
using InsuranceSimpleApi.Interfaces;
using InsuranceSimpleApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsuranceSimpleApi.Controllers
{
    [ApiController]
    [Route("api/insurance-types")]
    public class InsuranceTypesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAuthenticatedUser _user;

        public InsuranceTypesController(AppDbContext context, IAuthenticatedUser user)
        {
            _context = context;
            _user = user;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<InsuranceType>), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            return Ok(_context.InsuranceTypes.ToList());
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(InsuranceType), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create(CreateInsuranceTypeRequest request)
        {
            var insuranceType = new InsuranceType
            {
                Name = request.Name,
                Description = request.Description
            };

            _context.InsuranceTypes.Add(insuranceType);
            await _context.SaveChangesAsync();

            return Ok(insuranceType);
        }

        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update(int id, UpdateInsuranceTypeRequest request)
        {
            var entity = await _context.InsuranceTypes.FindAsync(id);
            if (entity == null)
                return NotFound();

            entity.Name = request.Name;
            entity.Description = request.Description;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Patch(int id,PatchInsuranceTypeRequest request)
        {
            var entity = await _context.InsuranceTypes.FindAsync(id);
            if (entity == null)
                return NotFound();

            if (request.Description != null)
                entity.Description = request.Description;

            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}