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
using RoboSchoolBDProjectBackend.Models.OutObjects.Course;
using RoboSchoolBDProjectBackend.Models.OutObjects.Request;
using RoboSchoolBDProjectBackend.Models.IO_Objects.Manager;
using RoboSchoolBDProjectBackend.DataBaseModel;
using RoboSchoolBDProjectBackend.Models.IO_Objects.Teacher;
using RoboSchoolBDProjectBackend.Models.IO_Objects.School;
using RoboSchoolBDProjectBackend.Models.IO_Objects.Provider;
using RoboSchoolBDProjectBackend.Models.IO_Objects.Item;
using System.Globalization;
using RoboSchoolBDProjectBackend.Models.IO_Objects;

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

            if (String.IsNullOrWhiteSpace(form.Login) || String.IsNullOrWhiteSpace(form.Password))
            {
                return BadRequest(new { errorText = "All fields are required" });
            }

            var admin = _context.HashSalts.FromSqlInterpolated($"SELECT hash, salt FROM Admin WHERE Admin.email = {form.Login}").ToList();
            if (admin.Count==0) { return BadRequest(new { errorText = "Invalid username" }); }

            var response = AuthenticationManager.Response(form, admin.First());
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
            //var managers = await _context.Managers.FromSqlRaw("SELECT id_manager, name, surname, lastname, email FROM Managers").ToListAsync();
            //var a = User.Identity.Name;  know who

            var managers = await _context.Managers.Include(t => t.phones).ToListAsync();
            if (managers == null)
            {
                return NotFound();
            }

            List<ManagerOut> result = new List<ManagerOut>();
            foreach(Managers manager in managers)
            {
                result.Add(new ManagerOut(manager));
            }
            return Ok(result);
        }

        [Authorize]
        [HttpGet("get_all_free_managers")]
        public async Task<IActionResult> GetAllFreeManagers()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var managers = await _context.Managers.FromSqlRaw("SELECT * FROM Managers WHERE Managers.id_manager NOT IN (SELECT Schools.id_manager FROM Schools);").ToListAsync();
            if (managers == null)
            {
                return NotFound();
            }

            List<ManagerOut> result = new List<ManagerOut>();
            foreach (Managers manager in managers)
            {
                result.Add(new ManagerOut(manager));
            }
            return Ok(result);
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
            foreach (var item in manager.phones)
            {
                if (String.IsNullOrEmpty(item.phone_number)) return BadRequest(new { errorText = "Empty phone field!" });
            }

            byte[] salt = PasswordManager.GenerateSalt_128();
            string saltStr = Encoding.ASCII.GetString(salt);

            Managers withId = new Managers();
            withId.name = manager.name;
            withId.surname = manager.surname;
            withId.lastname = manager.lastname;
            withId.email = manager.email;
            withId.hash = PasswordManager.PasswordSaveHashing(manager.Password_temp, salt);
            withId.salt = saltStr;
            _context.Managers.Add(withId);
            _context.SaveChanges();


            int id = withId.id_manager;

            //await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO Managers VALUES ({null}, {manager.name}, {manager.surname}, {manager.lastname}, {manager.email}, {PasswordManager.PasswordSaveHashing(manager.Password_temp, salt)} , {saltStr});");

            foreach (Models.PhonesIn phone in manager.phones)
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO Manager_phones VALUES ({phone.phone_number}, {id});");
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
            //var teachers = await _context.Teachers.FromSqlRaw("SELECT id_teacher, name, surname, lastname, email, work_begin, work_exp FROM Teachers").ToListAsync();


            var teachers = await _context.Teachers.Include(t => t.phones).ToListAsync();
            if (teachers == null)
            {
                return NotFound();
            }

            List<TeacherOut> result = new List<TeacherOut>();
            foreach (Teachers teacher in teachers)
            {
                result.Add(new TeacherOut(teacher));
            }
            return Ok(result);
        }

        [Authorize]
        [HttpGet("get_all_free_teachers")]
        public async Task<IActionResult> GetAllFreeTeachers()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var teachers = await _context.Teachers.FromSqlRaw("SELECT * FROM Teachers WHERE Teachers.id_teacher NOT IN (SELECT Schools.id_teacher FROM Schools);").ToListAsync();
            if (teachers == null)
            {
                return NotFound();
            }

            List<TeacherOut> result = new List<TeacherOut>();
            foreach (Teachers teacher in teachers)
            {
                result.Add(new TeacherOut(teacher));
            }
            return Ok(result);
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
            foreach (var item in teacher.phones)
            {
                if (String.IsNullOrEmpty(item.phone_number)) return BadRequest(new { errorText = "Empty phone field!" });
            }
            byte[] salt = PasswordManager.GenerateSalt_128();
            string saltStr = Encoding.ASCII.GetString(salt);

            Teachers withId = new Teachers();
            withId.name = teacher.name;
            withId.surname = teacher.surname;
            withId.lastname = teacher.lastname;
            withId.email = teacher.email;
            withId.work_begin = DateTime.Now;
            withId.work_exp = 0;
            withId.hash = PasswordManager.PasswordSaveHashing(teacher.Password_temp, salt);
            withId.salt = saltStr;
            _context.Teachers.Add(withId);
            _context.SaveChanges();


            int id = withId.id_teacher;
            foreach (Models.Teacher.PhonesIn phone in teacher.phones)
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO Teacher_phones VALUES ({phone.phone_number}, {id});");
            }

            //await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO Teachers VALUES ({null}, {teacher.name}, {teacher.surname}, {teacher.lastname}, {teacher.email},  {DateTime.Now}, {0}, {PasswordManager.PasswordSaveHashing(teacher.Password_temp, salt)} , {saltStr});");
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
            var schools = await _context.Schools.Include(s => s.items)
                                                .ThenInclude(i => i.Item).ToListAsync();   //"SELECT Schools.id_school, Schools.adress, Schools.open_date, Schools.aud_number, Schools.id_teacher, Schools.id_manager, Items.id_item, Items.name, School_items.items_num FROM Schools, School_items, Items WHERE Schools.id_school = School_items.id_school AND School_items.id_item = Items.id_item").ToListAsync();

            List<SchoolOut> result = new List<SchoolOut>();
            foreach (Schools school in schools)
            {
                Teachers teacher = get_teacher_from_teacherId(school.id_teacher);
                Managers manager = get_manager_from_managerId(school.id_manager);
                result.Add(new SchoolOut(school, manager.name + " " + manager.surname + " " + manager.lastname, teacher.name + " " + teacher.surname + " " + teacher.lastname));
            }
            return Ok(result);
        }

        [Authorize]
        [HttpPost("get_schools_with_adress")]
        public async Task<IActionResult> GetSchools(FilterIn filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var schools = await _context.Schools.Where(sch => sch.adress == filter.adress).Include(s => s.items)  //FromSqlRaw($"SELECT * FROM Schools WHERE Schools.adress = {filter.adress}")
                                                .ThenInclude(i => i.Item).ToListAsync();
            if (schools.Count == 0)
            {
                return BadRequest(new { errorText = "No such schools" });
            }

            List<SchoolOut> result = new List<SchoolOut>();
            foreach (Schools school in schools)
            {
                Teachers teacher = get_teacher_from_teacherId(school.id_teacher);
                Managers manager = get_manager_from_managerId(school.id_manager);
                result.Add(new SchoolOut(school, manager.name + " " + manager.surname + " " + manager.lastname, teacher.name + " " + teacher.surname + " " + teacher.lastname));
            }
            return Ok(result);
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
            if (school.aud_number < 1)
            {
                return BadRequest(new { errorText = "Classrooms number is invalid!" });
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
            var providers = await _context.Providers.FromSqlRaw("SELECT prov_name, contact_number, site_link FROM Providers").ToListAsync();
            List<ProviderOut> result = new List<ProviderOut>();
            foreach (Providers provider in providers)
            {
                result.Add(new ProviderOut(provider));
            }
            return Ok(result);
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
            await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO Providers VALUES ({provider.provider_name}, {provider.contact_number}, {provider.site_link});");
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
            var items = await _context.Items.FromSqlRaw("SELECT id_item, cost, prov_name, name FROM Items").ToListAsync();
            List<ItemOut> result = new List<ItemOut>();
            foreach (Items item in items)
            {
                result.Add(new ItemOut(item));
            }
            return Ok(result);
        }


        [Authorize]
        [HttpGet("delete_item/{item_id}")]
        public async Task<IActionResult> DeleteItem([FromRoute] int item_id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM Items WHERE Items.id_item = {item_id}");
            }
            catch(Exception e)
            {
                return BadRequest(new { errorText = "Item is used! Please, make sure it is excluded from the system" });
            }
            
            return Ok();
        }

        [Authorize]
        [HttpPost("add_item")]
        [Consumes("application/json")]
        public async Task<IActionResult> ItemRegister(ItemIn item)
        {
            double cost = 0;
            if (Double.TryParse(item.cost, NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out cost)&& cost>0.01)
            {
                cost = Math.Round(cost, 2);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO Items VALUES ({null}, {cost}, {item.provider_name}, {item.name});");
                
            }
            else
            {
                return BadRequest(new { errorText = "Invalid cost input! Example: 25,55" });
            }

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
            var courses = await _context.Courses.Include(c => c.items)
                                                .ThenInclude(i => i.Item).ToListAsync();     //("SELECT Courses.name_course, Items.id_item, Items.name FROM Courses, Course_items, Items WHERE Courses.name_course = Course_items.name_course AND Course_items.id_item = Items.id_item").ToListAsync();

            List<CourseOut> result = new List<CourseOut>();
            foreach(Courses course in courses)
            {
                result.Add(new CourseOut(course));
            }
            return Ok(result);
        }


        [Authorize]
        [HttpGet("delete_course/{name_course}")]
        public async Task<IActionResult> DeleteCourse([FromRoute] String name_course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM Course_items WHERE Course_items.name_course = {name_course}");
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
            foreach(var item in course.items)
            {
                if(item.id_item == null) return BadRequest(new { errorText = "Empty item field!" });
            }
           

            await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO Courses VALUES ({course.name_course});");
            foreach (ItemForCourseIn item in course.items)
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO Course_items VALUES ({null}, {course.name_course}, {item.id_item});");
            }
            return Ok();
        }
        #endregion

        #region Requests

        [Authorize]
        [HttpGet("get_all_requests")]
        public async Task<IActionResult> GetAllRequests()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var requests = await _context.Requests.Include(r => r.items)
                                               .ThenInclude(i => i.Item).ToListAsync();   //"SELECT Requests.id_request, Requests.date, Requests.confirmed, Requests.date_confirmed, Requests.finished, Requests.date_finished, Requests.id_teacher, Requests.id_manager, Items.id_item, Items.name, Request_items.items_num FROM Requests, Request_items, Items WHERE Requests.id_request = Request_items.id_request AND Request_items.id_item = Items.id_item").ToListAsync();


            List<RequestOut> result = new List<RequestOut>();
            foreach (Requests request in requests)
            {
                Teachers teacher = get_teacher_from_teacherId(request.id_teacher);
                Managers manager = get_manager_from_managerId(request.id_manager);
                result.Add(new RequestOut(request, manager.name +" "+ manager.surname +" "+ manager.lastname, teacher.name + " " + teacher.surname + " " + teacher.lastname));
            }
            return Ok(result);
        }

        [Authorize]
        [HttpGet("get_requested_items_num_per_teacher")]
        public async Task<IActionResult> GetRequestedItemsNumPerTeacher()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var requests = await _context.Requests.FromSqlRaw("SELECT id_teacher, Sum(tems_num) AS ItemSum FROM Requests INNER JOIN Request_items ON Requests.id_request = Request_items.id_request WHERE finished = true GROUP BY id_teacher; ").ToListAsync(); 

            return Ok(requests);
        }


        [Authorize]
        [HttpGet("confirm_request/{id_request}")]
        public async Task<IActionResult> ConfirmRequest([FromRoute] int id_request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _context.Database.ExecuteSqlInterpolatedAsync($"UPDATE Requests SET confirmed = {true}, date_confirmed = {DateTime.Now} WHERE Requests.id_request = {id_request}");

            return Ok();
        }

        [Authorize]
        [HttpGet("finish_request/{id_request}")]
        public async Task<IActionResult> FinishRequest([FromRoute] int id_request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _context.Database.ExecuteSqlInterpolatedAsync($"UPDATE Requests SET finished = {true}, date_finished = {DateTime.Now} WHERE Requests.id_request = {id_request}");

            var request = await _context.Requests.Where(ar => (ar.id_request == id_request))
                                                 .Include(r => r.items)
                                                 .ThenInclude(i => i.Item).ToListAsync();

           
            foreach (Request_items item in request.First().items)
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO School_items VALUES ({null}, {item.items_num}, {DateTime.Now}, {get_schoolId_from_teacherId(request.First().id_teacher)} ,{item.id_item});");
            }
            return Ok();
        }


        [Authorize]
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


        
        [HttpGet("requests_with_all_items")]
        public async Task<IActionResult> RequestsWithAllItems()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var requestsIds = await _context.Request_items.FromSqlRaw("SELECT DISTINCT id_request_items, id_request, id_item, items_num FROM Request_items AS ri1 WHERE NOT EXISTS( SELECT * FROM Items WHERE NOT EXISTS( SELECT * FROM Request_items AS ri2 WHERE(ri1.id_request = ri2.id_request) AND(ri2.id_item = Items.id_item))); ").ToListAsync();

            List<int> ids = new List<int>();
            int bufPrev = 0;
            foreach(Request_items item in requestsIds)
            {
                if (bufPrev != item.id_request) {
                    ids.Add(item.id_request);
                }
                bufPrev = item.id_request;
            }


            var requests = await _context.Requests.Where(req=> ids.Contains(req.id_request)).Include(r => r.items)
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
        [HttpPost("get_requests_with_adress")]
        public async Task<IActionResult> GetFilterRequests(FilterIn filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            List<RequestOut> result;
            try
            {
                var requests = await _context.Requests.Where(req => (req.id_teacher == get_teacherId_from_adress(filter.adress))).Include(r => r.items)
                                              .ThenInclude(i => i.Item).ToListAsync();
                if (requests.Count == 0)
                {
                    return BadRequest(new { errorText = "No requests in this school" });
                }
                result = new List<RequestOut>();
                foreach (Requests request in requests)
                {
                    Teachers teacher = get_teacher_from_teacherId(request.id_teacher);
                    Managers manager = get_manager_from_managerId(request.id_manager);
                    result.Add(new RequestOut(request, manager.name + " " + manager.surname + " " + manager.lastname, teacher.name + " " + teacher.surname + " " + teacher.lastname));
                }
            }
            catch(Exception e)
            {
                return BadRequest(new { errorText = "No such school" });
            }
              //"SELECT Requests.id_request, Requests.date, Requests.confirmed, Requests.date_confirmed, Requests.finished, Requests.date_finished, Requests.id_teacher, Requests.id_manager, Items.id_item, Items.name, Request_items.items_num FROM Requests, Request_items, Items WHERE Requests.id_request = Request_items.id_request AND Request_items.id_item = Items.id_item").ToListAsync();

            
            return Ok(result);
        }
        #endregion

        # region Utilities
        private int get_schoolId_from_teacherId(int teacherId)
        {
            var schoolId = _context.Schools.FromSqlInterpolated($"SELECT * FROM Schools WHERE id_teacher = {teacherId}").ToList();
            return schoolId.First().id_school;
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

        private int get_teacherId_from_adress(String adress)
        {
            var schoolId = _context.Schools.FromSqlInterpolated($"SELECT * FROM Schools WHERE adress = {adress}").ToList();
            return schoolId.First().id_teacher;
        }



        #endregion

    }
}