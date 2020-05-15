using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoboSchoolBDProjectBackend.Models;
using RoboSchoolBDProjectBackend.Tools;
using RoboSchoolBDProjectBackend.Models.Teacher;
using RoboSchoolBDProjectBackend.Models.OutObjects;

namespace RoboSchoolBDProjectBackend.Controllers
{
    [Route("api/[controller]")]
    [EnableCors]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private AdminContext _context;

        public AdminController(AdminContext context)
        {
            _context = context;
        }

        [HttpPost("token")]
        [Consumes("application/json")]
        public IActionResult Token(SignInForm form)
        {
            var manager = _context.HashSalts.FromSqlInterpolated($"SELECT hash, salt FROM Admin WHERE Admin.email = {form.Login}").ToList();
            if (manager == null) { return BadRequest(new { errorText = "Invalid username" }); }

            var response = AuthenticationManager.Response(form, manager.First());
            if (response == null) { return BadRequest(new { errorText = "Invalid password" }); }

            return Ok(response);
        }

        #region Managers
        [Authorize]
        [HttpGet("get_all_managers")]
        public async Task<IActionResult> GetAllManagers()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var manager = await _context.ManagersOut.FromSqlRaw("SELECT id_manager, name, surname, lastname, email FROM Managers").ToListAsync();

            if (manager == null)
            {
                return NotFound();
            }
            //var a = User.Identity.Name;  know who
            return Ok(manager);
        }

        [Authorize]
        [HttpGet("delete_manager/{id}")]
        public async Task<IActionResult> DeleteManager([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM Managers WHERE Managers.id_manager = {id}");

            return Ok();
        }

        [Authorize]
        [HttpPost("add_manager")]
        [Consumes("application/json")]
        public async Task<IActionResult> ManagerRegister(ManagerIn manager)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            byte[] salt = PasswordManager.GenerateSalt_128();
            string saltStr = Encoding.ASCII.GetString(salt);
            await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO Managers VALUES ({null}, {manager.name}, {manager.surname}, {manager.lastname}, {manager.email}, {PasswordManager.PasswordSaveHashing(manager.Password_temp, salt)} , {saltStr});");
            if (manager == null)
            {
                return NotFound();
            }
            return Ok();
        }
        #endregion



        #region Teachers
        [Authorize]
        [HttpGet("get_all_teachers")]
        public async Task<IActionResult> GetAllTeachers()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var teacher = await _context.TeachersOut.FromSqlRaw("SELECT id_teacher, name, surname, lastname, email, work_begin, work_exp FROM Teachers").ToListAsync();

            if (teacher == null)
            {
                return NotFound();
            }
            //var a = User.Identity.Name;  know who
            return Ok(teacher);
        }

        [Authorize]
        [HttpGet("delete_teacher/{id}")]
        public async Task<IActionResult> DeleteTeacher([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM Teachers WHERE Teachers.id_teacher = {id}");

            return Ok();
        }

        [Authorize]
        [HttpPost("add_teacher")]
        [Consumes("application/json")]
        public async Task<IActionResult> TeacherRegister(TeacherIn teacher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            byte[] salt = PasswordManager.GenerateSalt_128();
            string saltStr = Encoding.ASCII.GetString(salt);
            await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO Teachers VALUES ({null}, {teacher.name}, {teacher.surname}, {teacher.lastname}, {teacher.email},  {DateTime.Now}, {0}, {PasswordManager.PasswordSaveHashing(teacher.Password_temp, salt)} , {saltStr});");
            if (teacher == null)
            {
                return NotFound();
            }
            return Ok();
        }
        #endregion

        #region Schools

        [Authorize]
        [HttpGet("get_all_schools")]
        public async Task<IActionResult> GetAllSchools()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var schools = await _context.SchoolsOut.FromSqlRaw("SELECT id_school, adress, open_date, aud_number, id_teacher, id_manager FROM Schools").ToListAsync();

            if (schools == null)
            {
                return NotFound();
            }
            return Ok(schools);
        }


        [Authorize]
        [HttpGet("delete_school/{id}")]
        public async Task<IActionResult> DeleteSchool([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM Schools WHERE Schools.id_school = {id}");

            return Ok();
        }

        [Authorize]
        [HttpPost("add_school")]
        [Consumes("application/json")]
        public async Task<IActionResult> SchoolRegister(SchoolIn school)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO Schools VALUES ({null}, {school.adress}, {DateTime.Now},{Int32.Parse(school.aud_number)}, {school.id_manager}, {school.id_teacher});");
            if (school == null)
            {
                return NotFound();
            }
            return Ok();
        }
        #endregion

    }
}