using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoboSchoolBDProjectBackend.Models;

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
        
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetManager([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var manager = await _context.Managers.SingleOrDefaultAsync(m => m.Id == id);

            if(manager == null)
            {
                return NotFound();
            }
            return Ok(manager);
        }
    }
}