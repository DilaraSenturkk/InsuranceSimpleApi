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
        [Authorize(Roles = "User")] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest("Sayfa ve sayfa boyutu 0'dan büyük olmalı");

            var query = _context.InsuranceTypes.AsQueryable();

            var totalCount = query.Count();

            var items = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                totalCount,
                page,
                pageSize,
                totalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                data = items
            });
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