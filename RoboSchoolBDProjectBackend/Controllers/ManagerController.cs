using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoboSchoolBDProjectBackend.Models;
using RoboSchoolBDProjectBackend.Tools;

namespace RoboSchoolBDProjectBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {

        private ManagerContext _context;

        public ManagerController(ManagerContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return Ok(_context.Managers.ToArray());
        }
        
        [HttpGet("all/{id}")]
        public async Task<IActionResult> GetManager([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var manager = await _context.Managers.FromSqlInterpolated($"SELECT * FROM Managers WHERE Managers.Manager_id = {id}").ToListAsync();

            if(manager == null)
            {
                return NotFound();
            }
            return Ok(manager);
        }

        [HttpPost("add")]
        [Consumes("application/json")]
        public async Task<IActionResult> ManagerRegister(Manager manager)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            byte[] salt = PasswordManager.GenerateSalt_128();
            await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO Managers VALUES ({manager.Id}, {manager.Name}, {manager.Surname}, {manager.Lastname}, {PasswordManager.PasswordSaveHashing(manager.Password_temp, salt)}, {Convert.ToBase64String(salt)});");
            if (manager == null)
            {
                return NotFound();
            }
            return Ok();
        }

    }
}