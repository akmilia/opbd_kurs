using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace opbd_kurs
{
    public class Hash
    {
        public static string hashPassword(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            bytes = md5.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
