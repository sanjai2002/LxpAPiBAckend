using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Common.Utils
{
    public  class Encryption
    {

        public static string ComputePasswordToSha256Hash(string plainText)
        {
            // Create a SHA256 hash from string   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Computing Hash - returns here byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(plainText));

                // now convert byte array to a string   
                StringBuilder stringbuilder = new StringBuilder();
                foreach (byte b in bytes) 
                {
                    stringbuilder.Append(b.ToString("x2"));
                }
                return stringbuilder.ToString();
            }
        }
    }
}
