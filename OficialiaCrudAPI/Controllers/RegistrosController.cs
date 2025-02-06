using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OficialiaCrudAPI.Models;
using OficialiaCrudAPI.Data;
using Microsoft.AspNetCore.Authorization;

namespace OficialiaCrudAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrosController : ControllerBase
    {
        private readonly AppDataDbContext _appDataDbContext;

        public RegistrosController(AppDataDbContext appDataDbContext) => _appDataDbContext = appDataDbContext;

        [Authorize(Roles = "User")]
        [HttpGet]
        public ActionResult<IEnumerable<Correspondencias>> Get()
        {
            return _appDataDbContext.Correspondencia;
        }

        [Authorize(Roles = "User")]
        [HttpGet("id")]
        public async Task<ActionResult<Correspondencias?>> GetById(int id)
        {
            return await _appDataDbContext.Correspondencia.Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        [Authorize(Roles = "User")]
        [Route("Nuevo")]
        [HttpPost]
        public async Task<ActionResult> Create(Correspondencias registros)
        {
            await _appDataDbContext.Correspondencia.AddAsync(registros);
            await _appDataDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = registros.Id }, registros);
        }

        [HttpPut]
        public async Task<ActionResult> Update(Correspondencias registros)
        {
            _appDataDbContext.Correspondencia.Update(registros);
            await _appDataDbContext.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = "User")]
        [HttpDelete("id")]
        public async Task<ActionResult> Delete(int id)
        {
            var productGetByIdResult = await GetById(id);
            if (productGetByIdResult.Value is null)
            {
                return NotFound();
            }
            _appDataDbContext.Correspondencia.Remove(productGetByIdResult.Value);
            await _appDataDbContext.SaveChangesAsync();
            return Ok();
        }
    }
}