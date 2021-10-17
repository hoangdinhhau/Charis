using System;
using System.Security.Cryptography;
using System.Text;

namespace Charis.Utilities.Exceptions
{
    public static class Hash
    {
        public static string hash(string input)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] originalBytes = ASCIIEncoding.Default.GetBytes(input);
            byte[] encodedBytes = md5.ComputeHash(originalBytes);

            return BitConverter.ToString(encodedBytes).Replace("-", "").Replace("r", "c").Replace("g", "h");
        }
    }
}