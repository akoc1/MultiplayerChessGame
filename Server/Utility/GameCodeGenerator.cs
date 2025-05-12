using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Utility
{
    public static class GameCodeGenerator
    {
        public static string Generate()
        {
            string output = "";
            Random random = new Random();

            for (int i = 1; i <= 6; i++)
            {
                char c = (char)random.Next(65, 90);
                output += c;
            }

            return output;
        }
    }
}
