using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;

namespace RoboSchoolBDProjectBackend.Tools
{
    internal static class PasswordManager
    {

        internal static String PasswordSaveHashing(String password, byte[] salt)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));
            return hashed;
        } 

        internal static byte[] GenerateSalt_128()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            for (int i = 0; i < salt.Length; i++)
            {
                salt[i] = (byte)((int)salt[i] % 64);
            }
            return salt;
        }
    }
}
