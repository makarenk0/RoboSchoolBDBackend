using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoboSchoolBDProjectBackend.DataBaseModel;
using RoboSchoolBDProjectBackend.Models;
using RoboSchoolBDProjectBackend.Models.IO_Objects.Group;
using RoboSchoolBDProjectBackend.Models.IO_Objects.Item;
using RoboSchoolBDProjectBackend.Models.IO_Objects.Provider;
using RoboSchoolBDProjectBackend.Models.IO_Objects.School;
using RoboSchoolBDProjectBackend.Models.IO_Objects.Teacher;
using RoboSchoolBDProjectBackend.Models.OutObjects.Course;
using RoboSchoolBDProjectBackend.Models.OutObjects.Request;
using RoboSchoolBDProjectBackend.Models.Teacher;
using RoboSchoolBDProjectBackend.Tools;

namespace RoboSchoolBDProjectBackend.Controllers
{
    [Route("api/[controller]")]
    [EnableCors]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private TeacherContext _context;

        public TeacherController(TeacherContext context)
        {
            _context = context;
        }

        [HttpPost("token")]
        [Consumes("application/json")]
        public IActionResult Token(SignInForm form)
        {
            if (String.IsNullOrWhiteSpace(form.Login) || String.IsNullOrWhiteSpace(form.Password))
            {
                return BadRequest(new { errorText = "All fields are required" });
            }

            var teacher = _context.HashSalts.FromSqlInterpolated($"SELECT hash, salt FROM Teachers WHERE Teachers.email = {form.Login}").ToList();
            if (teacher.Count == 0) { return BadRequest(new { errorText = "Invalid username" }); }

            var response = AuthenticationManager.Response(form, teacher.First());
            if (response == null) { return BadRequest(new { errorText = "Invalid password" }); }

            return Ok(response);
        }

        [Authorize]
        [HttpGet("get")]
        public async Task<IActionResult> GetTeacher()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            String email = User.Identity.Name;
            var teacher = await _context.Teachers.FromSqlInterpolated($"SELECT * FROM Teachers WHERE Teachers.email = {email}").Include(t => t.phones).ToListAsync();
            
            if (teacher == null)
            {
                return NotFound();
            }
            TeacherOut teacherOut = new TeacherOut(teacher.First());
            return Ok(teacherOut);
        }

        [Authorize]
        [HttpPost("save_teacher_info")]
        [Consumes("application/json")]
        public async Task<IActionResult> TeacherUpdate(TeacherUpdate teacher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            foreach (var item in teacher.phones)
            {
                if (item==null|| String.IsNullOrEmpty(item.phone)) return BadRequest(new { errorText = "Empty phone field!" });
            }


            await _context.Database.ExecuteSqlInterpolatedAsync($"UPDATE Teachers SET name = {teacher.name}, surname={teacher.surname}, lastname={teacher.lastname} WHERE Teachers.id_teacher = {teacher.id_teacher}");

            await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM Teacher_phones WHERE Teacher_phones.id_teacher = {teacher.id_teacher}");
            foreach (Models.IO_Objects.Teacher.PhonesIn phone in teacher.phones)
            {
                
                await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO Teacher_phones VALUES ({phone.phone}, {teacher.id_teacher});");
            }

            //await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO Teachers VALUES ({null}, {teacher.name}, {teacher.surname}, {teacher.lastname}, {teacher.email},  {DateTime.Now}, {0}, {PasswordManager.PasswordSaveHashing(teacher.Password_temp, salt)} , {saltStr});");
            return Ok();
        }


        #region Groups
        [Authorize]
        [HttpGet("get_teacher_groups")]
        public async Task<IActionResult> GetTeacherGroups()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int schoolId = get_schoolId_from_TeacherEmail(User.Identity.Name);
            var groups = await _context.Groups.FromSqlRaw($"SELECT `Groups`.id_group, `Groups`.pupils_num, `Groups`.name_course, `Groups`.id_school FROM `Groups`, Schools WHERE `Groups`.id_school = Schools.id_school AND Schools.id_school = {schoolId}").ToListAsync();

            if (groups == null)
            {
                return NotFound();
            }

            List<GroupOut> result = new List<GroupOut>();
            foreach (Groups group in groups)
            {
                result.Add(new GroupOut(group));
            }
            return Ok(result);
        }
        #endregion

        #region Items
        [Authorize]
        [HttpGet("get_teacher_items")]
        public async Task<IActionResult> GetTeacherItems()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int schoolId = get_schoolId_from_TeacherEmail(User.Identity.Name);

            var schools = await _context.Schools.Where(sc => (sc.id_school == schoolId)).Include(s => s.items)
                                                .ThenInclude(i => i.Item).ToListAsync();

            List<SchoolOut.SchoolItems> result = new List<SchoolOut.SchoolItems>();
            foreach (School_items school_items in schools.First().items)
            {
                result.Add(new SchoolOut.SchoolItems(school_items.id_school_items, school_items.id_item, school_items.Item.cost, school_items.items_num, school_items.Item.name));
            }

            return Ok(result);
        }

        [Authorize]
        [HttpGet("get_all_items")]
        public async Task<IActionResult> GetAllItems()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var items = await _context.Items.FromSqlRaw("SELECT id_item, cost, prov_name, name FROM Items").ToListAsync();
            List<ItemOut> result = new List<ItemOut>();
            foreach (Items item in items)
            {
                result.Add(new ItemOut(item));
            }
            return Ok(result);
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
            var courses = await _context.Courses.Include(c => c.items)
                                                .ThenInclude(i => i.Item).ToListAsync();     //("SELECT Courses.name_course, Items.id_item, Items.name FROM Courses, Course_items, Items WHERE Courses.name_course = Course_items.name_course AND Course_items.id_item = Items.id_item").ToListAsync();

            List<CourseOut> result = new List<CourseOut>();
            foreach (Courses course in courses)
            {
                result.Add(new CourseOut(course));
            }
            return Ok(result);
        }
        #endregion

        #region Requests
        [Authorize]
        [HttpGet("get_teacher_requests")]
        public async Task<IActionResult> GetTeacherRequests()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var requests = await _context.Requests.Where(req => (req.id_teacher == get_teacherId_from_TeacherEmail(User.Identity.Name))).Include(r => r.items)
                                               .ThenInclude(i => i.Item).ToListAsync();   //"SELECT Requests.id_request, Requests.date, Requests.confirmed, Requests.date_confirmed, Requests.finished, Requests.date_finished, Requests.id_teacher, Requests.id_manager, Items.id_item, Items.name, Request_items.items_num FROM Requests, Request_items, Items WHERE Requests.id_request = Request_items.id_request AND Request_items.id_item = Items.id_item").ToListAsync();
            List<RequestOut> result = new List<RequestOut>();
            foreach (Requests request in requests)
            {
                Teachers teacher = get_teacher_from_teacherId(request.id_teacher);
                Managers manager = get_manager_from_managerId(request.id_manager);
                result.Add(new RequestOut(request, manager.name + " " + manager.surname + " " + manager.lastname, teacher.name + " " + teacher.surname + " " + teacher.lastname));
            }
            return Ok(result);
        }

        [Authorize]
        [HttpPost("add_request")]
        [Consumes("application/json")]
        public async Task<IActionResult> RequestRegister(RequestIn request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            foreach (var item in request.items)
            {
                if (item.id_item == null) return BadRequest(new { errorText = "Empty item field!" });
            }


            // TO DO count sum of items
            double sum = 0;
            var items = await _context.Items.FromSqlRaw("SELECT id_item, cost, prov_name, name FROM Items").ToListAsync();
            foreach (ItemForRequestIn item in request.items)
            {
                sum += items.Where(i => i.id_item == item.id_item).First().cost * item.amount;
            }

            Requests requests = new Requests();
            requests.date = DateTime.Now;
            requests.sum = sum;
            requests.id_teacher = get_teacherId_from_TeacherEmail(User.Identity.Name);
            requests.id_manager = get_managerId_from_TeacherEmail(User.Identity.Name);
            _context.Requests.Add(requests);
            _context.SaveChanges();


            foreach (ItemForRequestIn ifr in request.items)
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO Request_items VALUES ({null}, {requests.id_request}, {ifr.id_item}, {ifr.amount} );");
            }

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
            var providers = await _context.Providers.FromSqlRaw("SELECT prov_name, contact_number, site_link FROM Providers").ToListAsync();
            List<ProviderOut> result = new List<ProviderOut>();
            foreach (Providers provider in providers)
            {
                result.Add(new ProviderOut(provider));
            }
            return Ok(result);
        }
        #endregion

        #region Utilities
        private int get_schoolId_from_TeacherEmail(String email)
        {
            var school = _context.Schools.FromSqlInterpolated($"SELECT Schools.id_school, Schools.adress, Schools.open_date, Schools.aud_number, Schools.id_manager, Schools.id_teacher FROM Schools, Teachers WHERE Schools.id_teacher = Teachers.id_teacher AND Teachers.email = {email}").ToList();
            return school.First().id_school;
        }

        private int get_teacherId_from_TeacherEmail(String email)
        {
            var teacher = _context.Teachers.FromSqlInterpolated($"SELECT * FROM Teachers WHERE Teachers.email = {email}").ToList();
            return teacher.First().id_teacher;
        }

        private int get_managerId_from_TeacherEmail(String email)
        {
            var school = _context.Schools.FromSqlInterpolated($"SELECT Schools.id_school, Schools.adress, Schools.open_date, Schools.aud_number, Schools.id_manager, Schools.id_teacher FROM Schools, Teachers WHERE Schools.id_teacher = Teachers.id_teacher AND Teachers.email = {email}").ToList();
            return school.First().id_manager;
        }

        private Teachers get_teacher_from_teacherId(int teacherId)
        {
            var teachers = _context.Teachers.FromSqlInterpolated($"SELECT * FROM Teachers WHERE Teachers.id_teacher = {teacherId}").ToList();
            return teachers.First();
        }

        private Managers get_manager_from_managerId(int managerId)
        {
            var managers = _context.Managers.FromSqlInterpolated($"SELECT * FROM Managers WHERE Managers.id_manager = {managerId}").ToList();
            return managers.First();
        }
        #endregion

    }
}
