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
using RoboSchoolBDProjectBackend.Models.Admin;
using RoboSchoolBDProjectBackend.Models.OutObjects.Course;
using RoboSchoolBDProjectBackend.Models.OutObjects.ComplexObjDB;
using RoboSchoolBDProjectBackend.Models.OutObjects.Request;

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
            var managers = await _context.ManagersOut.FromSqlRaw("SELECT id_manager, name, surname, lastname, email FROM Managers").ToListAsync();
            //var a = User.Identity.Name;  know who
            if (managers == null)
            {
                return NotFound();
            }
            return Ok(managers);
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
            var teachers = await _context.TeachersOut.FromSqlRaw("SELECT id_teacher, name, surname, lastname, email, work_begin, work_exp FROM Teachers").ToListAsync();
            //var a = User.Identity.Name;  know who
            if (teachers == null)
            {
                return NotFound();
            }
            return Ok(teachers);
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
            var schools_items = await _context.School_items.FromSqlRaw("SELECT Schools.id_school, Schools.adress, Schools.open_date, Schools.aud_number, Schools.id_teacher, Schools.id_manager, Items.id_item, Items.name, School_items.items_num FROM Schools, School_items, Items WHERE Schools.id_school = School_items.id_school AND School_items.id_item = Items.id_item").ToListAsync();
            
            List<SchoolOut> schools = new List<SchoolOut>();

            foreach (School_items school in schools_items)
            {
                var foundObj = schools.Find(elem => elem.id_school == school.id_school);
                if (foundObj != null)
                {
                    foundObj.items.Add(new ItemForRequest(school.id_item, school.name, school.items_num));
                }
                else
                {
                    schools.Add(new SchoolOut(school));
                }
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
            await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO Schools VALUES ({null}, {school.adress}, {DateTime.Now},{school.aud_number}, {school.id_manager}, {school.id_teacher});");
            return Ok();
        }
        #endregion

        #region Providers

        [Authorize]
        [HttpGet("get_all_providers")]
        public async Task<IActionResult> GetAllProviders()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var providers = await _context.ProvidersOut.FromSqlRaw("SELECT prov_name, contact_number, site_link FROM Providers").ToListAsync();
            
            return Ok(providers);
        }


        [Authorize]
        [HttpGet("delete_provider/{prov_name}")]
        public async Task<IActionResult> DeleteProvider([FromRoute] String prov_name)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM Providers WHERE Providers.prov_name = {prov_name}");

            return Ok();
        }

        [Authorize]
        [HttpPost("add_provider")]
        [Consumes("application/json")]
        public async Task<IActionResult> ProviderRegister(ProviderIn provider)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO Providers VALUES ({provider.prov_name}, {provider.contact_number}, {provider.site_link});");
            return Ok();
        }

        #endregion

        #region Items

        [Authorize]
        [HttpGet("get_all_items")]
        public async Task<IActionResult> GetAllItems()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var items = await _context.ItemsOut.FromSqlRaw("SELECT id_item, cost, prov_name, name FROM Items").ToListAsync();

            return Ok(items);
        }


        [Authorize]
        [HttpGet("delete_item/{item_id}")]
        public async Task<IActionResult> DeleteItem([FromRoute] int item_id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM Items WHERE Items.item_id = {item_id}");

            return Ok();
        }

        [Authorize]
        [HttpPost("add_item")]
        [Consumes("application/json")]
        public async Task<IActionResult> ItemRegister(ItemIn item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO Items VALUES ({null}, {item.cost}, {item.prov_name}, {item.name});");
            return Ok();
        }


        #endregion

        #region Courses

        [Authorize]
        [HttpGet("get_all_courses")]
        public async Task<IActionResult> GetAllCourses()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var courses_items = await _context.Course_items.FromSqlRaw("SELECT Courses.name_course, Items.id_item, Items.name FROM Courses, Course_items, Items WHERE Courses.name_course = Course_items.name_course AND Course_items.id_item = Items.id_item").ToListAsync();

            List<CourseOut> courses = new List<CourseOut>();

            foreach (Course_items course in courses_items)
            {
                var foundObj = courses.Find(elem => elem.name_course == course.name_course);
                if (foundObj != null)
                {
                    foundObj.items.Add(new ItemForCourse(course.id_item, course.name));
                }
                else
                {
                    courses.Add(new CourseOut(course));
                }
            }
            return Ok(courses);
        }


        [Authorize]
        [HttpGet("delete_course/{name_course}")]
        public async Task<IActionResult> DeleteCourse([FromRoute] String name_course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM Courses WHERE Courses.name_course = {name_course}");

            return Ok();
        }

        [Authorize]
        [HttpPost("add_course")]
        [Consumes("application/json")]
        public async Task<IActionResult> CourseRegister(CourseIn course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO Courses VALUES ({course.name_course});");
            foreach (ItemForCourseIn item in course.id_item)
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO Course_items VALUES ({null}, {course.name_course}, {item.id_item});");
            }
            return Ok();
        }
        #endregion

        #region Courses

        [Authorize]
        [HttpGet("get_all_requests")]
        public async Task<IActionResult> GetAllRequests()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var requests_items = await _context.Request_items.FromSqlRaw("SELECT Requests.id_request, Requests.date, Requests.confirmed, Requests.date_confirmed, Requests.finished, Requests.date_finished, Requests.id_teacher, Requests.id_manager, Items.id_item, Items.name, Request_items.items_num FROM Requests, Request_items, Items WHERE Requests.id_request = Request_items.id_request AND Request_items.id_item = Items.id_item").ToListAsync();

            List<RequestOut> requests = new List<RequestOut>();

            foreach (Request_items request in requests_items)
            {
                var foundObj = requests.Find(elem => elem.id_request == request.id_request);
                if (foundObj != null)
                {
                    foundObj.items.Add(new ItemForRequest(request.id_item, request.name, request.items_num));
                }
                else
                {
                    requests.Add(new RequestOut(request));
                }
            }
            return Ok(requests);
        }


      //  [Authorize]
        [HttpGet("delete_request/{id_request}")]
        public async Task<IActionResult> DeleteRequest([FromRoute] int id_request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM Requests WHERE Requests.id_request = {id_request}");

            return Ok();
        }
        #endregion

    }
}