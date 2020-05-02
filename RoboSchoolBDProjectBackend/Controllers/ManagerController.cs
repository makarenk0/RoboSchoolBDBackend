using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
    }
}