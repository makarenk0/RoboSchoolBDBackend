using Microsoft.IdentityModel.Tokens;
using RoboSchoolBDProjectBackend.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RoboSchoolBDProjectBackend.Tools
{
    public static class AuthenticationManager
    {
        public static object Response(SignInForm form, HashSalt hashSalt)
        {
            var identity = GetIdentity(form, hashSalt);
            if (identity == null)
            {
                return null;
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
            return response;
        }


        private static ClaimsIdentity GetIdentity(SignInForm form, HashSalt hashSalt)
        {
            byte[] byteArr = Encoding.ASCII.GetBytes(hashSalt.salt);
            string passwordHashed = PasswordManager.PasswordSaveHashing(form.Password, byteArr);

            if (passwordHashed == hashSalt.hash)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, form.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, form.Role)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            return null;
        }
    }
}
