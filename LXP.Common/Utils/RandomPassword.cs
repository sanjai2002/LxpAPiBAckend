using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Common.Utils
{
    public  class RandomPassword
    {

        public static string Randompasswordgenerator()
        {
            Random random = new Random();
            //int passwordlenth = random.Next(6, 10);
            const string Validcharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder password = new StringBuilder();
            for (int i = 0; i < 6; i++)
            {
                int randomcode = random.Next(0, Validcharacters.Length);
                password.Append(Validcharacters[randomcode]);
            }
            return password.ToString();
        }



    }
}
