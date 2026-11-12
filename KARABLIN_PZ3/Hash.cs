using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PZ6
{
    internal class Hash
    {

        static public string HashPasswords(string password)
        {
            using(SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] passwordBytesHash = sha256.ComputeHash(passwordBytes);
                string hashPassword = BitConverter.ToString(passwordBytesHash).Replace("-", String.Empty);
                return hashPassword;

            }
        }
    }
}
