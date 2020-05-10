using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

        public IActionResult Index()
        {
            return Ok(_context.ManagersOut.ToArray());
        }


        [HttpPost("token")]
        [Consumes("application/json")]
        public IActionResult Token(SignInForm form)
        {
            var identity = GetIdentity(form);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            return Ok(response);
        }

        private ClaimsIdentity GetIdentity(SignInForm form)
        {
            var manager = _context.HashSalts.FromSqlInterpolated($"SELECT Manager_hash, Manager_salt FROM Managers WHERE Managers.Email = {form.Login}").ToList();
            if (manager == null)
            {
                return null;
            }
            byte[] byteArr = Encoding.ASCII.GetBytes(manager.First().Manager_salt);
            string passwordHashed = PasswordManager.PasswordSaveHashing(form.Password, byteArr);

            if (passwordHashed == manager.First().Manager_hash)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, form.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, "manager")
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllManagers()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var manager = await _context.ManagersOut.FromSqlInterpolated($"SELECT Manager_id, Manager_name, Manager_surname, Manager_lastname, Email FROM Managers").ToListAsync();

            if (manager == null)
            {
                return NotFound();
            }
            return Ok(manager);
        }


        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetManager([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var manager = await _context.ManagersOut.FromSqlInterpolated($"SELECT Manager_id, Manager_name, Manager_surname, Manager_lastname, Email FROM Managers WHERE Managers.Manager_id = {id}").ToListAsync();

            if(manager == null)
            {
                return NotFound();
            }
            return Ok(manager);
        }

        [HttpGet("delete/{id}")]
        public async Task<IActionResult> DeleteManager([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM Managers WHERE Managers.Manager_id = {id}");

            return Ok();
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
            string saltStr = Encoding.ASCII.GetString(salt);
            await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO Managers VALUES ({manager.Manager_id}, {manager.Manager_name}, {manager.Manager_surname}, {manager.Manager_lastname}, {manager.Email}, {PasswordManager.PasswordSaveHashing(manager.Password_temp, salt)} , {saltStr});");
            if (manager == null)
            {
                return NotFound();
            }
            return Ok();
        }

    }
}