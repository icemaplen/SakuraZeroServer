using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SakuraZeroServer.Tool
{
    public static class MD5Hash
    {
        public static string GetHashCode(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);    
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(bytes);
            string result = BitConverter.ToString(output).Replace("-", "");  
            return result;
        }
    }
}
