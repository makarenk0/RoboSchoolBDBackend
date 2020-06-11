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
using RoboSchoolBDProjectBackend.Models.IO_Objects.Manager;
using RoboSchoolBDProjectBackend.Models.IO_Objects.Provider;
using RoboSchoolBDProjectBackend.Models.IO_Objects.School;
using RoboSchoolBDProjectBackend.Models.OutObjects.Course;
using RoboSchoolBDProjectBackend.Models.OutObjects.Request;
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
            if(String.IsNullOrWhiteSpace(form.Login)|| String.IsNullOrWhiteSpace(form.Password))
            {
                return BadRequest("All fields are required");
            }
            var manager = _context.HashSalts.FromSqlInterpolated($"SELECT hash, salt FROM Managers WHERE Managers.email = {form.Login}").ToList();
            if (manager.Count == 0){ return BadRequest(new { errorText = "Invalid username" }); }
            
            var response = AuthenticationManager.Response(form, manager.First());
            if (response == null){ return BadRequest(new { errorText = "Invalid password" }); }

            return Ok(response);
        }

        [Authorize]
        [HttpGet("get")]
        public async Task<IActionResult> GetManager()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            String email = User.Identity.Name;
            var manager = await _context.Managers.FromSqlInterpolated($"SELECT * FROM Managers WHERE Managers.email = {email}").Include(ma => ma.phones).ToListAsync();  //   .Where(m => m.email == email)

            if (manager == null)
            {
                return NotFound();
            }

            ManagerOut managerOut = new ManagerOut(manager.First());
            return Ok(managerOut);
        }


        #region Groups
        [Authorize]
        [HttpGet("get_manager_groups")]
        public async Task<IActionResult> GetManagerGroups()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int schoolId = get_schoolId_from_ManagerEmail(User.Identity.Name);
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

        [Authorize]
        [HttpGet("delete_group/{id}")]
        public async Task<IActionResult> DeleteGroup([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM `Groups` WHERE `Groups`.id_group = {id}");

            return Ok();
        }

        [Authorize]
        [HttpPost("add_group")]
        [Consumes("application/json")]
        public async Task<IActionResult> GroupRegister(GroupIn group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }  
            await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO `Groups` VALUES ({null}, {group.pupils_number}, {group.name_course}, {get_schoolId_from_ManagerEmail(User.Identity.Name)});");
            return Ok();
        }
        #endregion

        #region Items
        [Authorize]
        [HttpGet("get_manager_items")]
        public async Task<IActionResult> GetAllItems()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int schoolId = get_schoolId_from_ManagerEmail(User.Identity.Name);
            
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
        [HttpGet("delete_item/{id_school_items}")]
        public async Task<IActionResult> DeleteItem([FromRoute] int id_school_items)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM School_items WHERE School_items.id_school_items = {id_school_items}");

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
            foreach (Courses course in courses)
            {
                result.Add(new CourseOut(course));
            }
            return Ok(result);
        }
        #endregion

        #region Requests
        [Authorize]
        [HttpGet("get_manager_requests")]
        public async Task<IActionResult> GetAllRequests()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var requests = await _context.Requests.Where(req =>(req.id_manager == get_managerId_from_ManagerEmail(User.Identity.Name))).Include(r => r.items)
                                               .ThenInclude(i => i.Item).ToListAsync();   //"SELECT Requests.id_request, Requests.date, Requests.confirmed, Requests.date_confirmed, Requests.finished, Requests.date_finished, Requests.id_teacher, Requests.id_manager, Items.id_item, Items.name, Request_items.items_num FROM Requests, Request_items, Items WHERE Requests.id_request = Request_items.id_request AND Request_items.id_item = Items.id_item").ToListAsync();
            List<RequestOut> result = new List<RequestOut>();
            foreach (Requests request in requests)
            {
                result.Add(new RequestOut(request));
            }
            return Ok(result);
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

            RequestOut currentRequest = new RequestOut(request.First());

            foreach (RequestOut.RequestItems item in currentRequest.items)
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO School_items VALUES ({null}, {item.amount}, {DateTime.Now}, {get_schoolId_from_ManagerEmail(User.Identity.Name)} ,{item.id_item});");
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
        private int get_schoolId_from_ManagerEmail(String email)
        {
            var schoolId = _context.Schools.FromSqlInterpolated($"SELECT Schools.id_school, Schools.adress, Schools.open_date, Schools.aud_number, Schools.id_manager, Schools.id_teacher FROM Schools, Managers WHERE Schools.id_manager = Managers.id_manager AND Managers.email = {email}").ToList();
            return schoolId.First().id_school;
        }

        private int get_managerId_from_ManagerEmail(String email)
        {
            var managerId = _context.Managers.FromSqlInterpolated($"SELECT * FROM Managers WHERE Managers.email = {email}").ToList();
            return managerId.First().id_manager;
        }


        #endregion


    }
}