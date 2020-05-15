using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoboSchoolBDProjectBackend.Models;
using RoboSchoolBDProjectBackend.Tools;

namespace RoboSchoolBDProjectBackend.Controllers
{
    [Route("api/[controller]")]
    [EnableCors]
    [ApiController]
    public class ManagerController : ControllerBase
    {

        private ManagerContext _context;

        public ManagerController(ManagerContext context)
        {
            _context = context;
        }

        [HttpPost("token")]
        [Consumes("application/json")]
        public IActionResult Token(SignInForm form)
        {
            var manager = _context.HashSalts.FromSqlInterpolated($"SELECT hash, salt FROM Admin WHERE Admin.email = {form.Login}").ToList();
            if (manager == null){ return BadRequest(new { errorText = "Invalid username" }); }
            
            var response = AuthenticationManager.Response(form, manager.First());
            if (response == null){ return BadRequest(new { errorText = "Invalid password" }); }

            return Ok(response);
        }
           

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetManager([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var manager = await _context.ManagersOut.FromSqlInterpolated($"SELECT id_manager, name, surname, lastname, email FROM Managers WHERE Managers.id_manager = {id}").ToListAsync();

            if(manager == null)
            {
                return NotFound();
            }
            return Ok(manager);
        }

        

        

    }
}