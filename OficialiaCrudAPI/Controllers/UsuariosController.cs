using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OficialiaCrudAPI.Data;
using OficialiaCrudAPI.Models;

namespace OficialiaCrudAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDataDbContext appDbContext;

        public UsuariosController(AppDataDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }


        [Route("GetAllUsuarios")]
        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public IActionResult GetAllStudents()
        {
            var getStudents = appDbContext.Students.ToList();
            return Ok(getStudents);
        }


        [Authorize(Roles = "Admin")]

        [Route("AddUsuario")]
        [HttpPost]
        public IActionResult AddStudent(Usuario student)
        {
            try
            {
                appDbContext.Students.Add(student);
                var result = appDbContext.SaveChanges() > 0;
                if (result)
                {
                    return Ok(new { message = "data submitted successfully" });
                }
                return BadRequest(new { message = "operation failed" });
            }
            catch (Exception ex)
            {

                return BadRequest(new { message = "operation failed " + ex.Message });
            }

        }

        [Authorize(Roles = "Admin")]

        [Route("UpdateUsuario")]
        [HttpPut]
        public IActionResult UpdateStudent(Usuario student, int id)
        {
            var std = appDbContext.Students.Find(id);
            if (std == null)
            {
                return NotFound();
            }

            std.Name = student.Name;
            std.Email = student.Email;
            std.Semester = student.Semester;
            std.Address = student.Address;

            appDbContext.SaveChanges();
            return Ok(std);
        }

    }
}
