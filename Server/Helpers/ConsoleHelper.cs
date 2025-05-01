using Server.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Helpers
{
    public static class ConsoleHelper
    {
        public static void WriteLine(string message)
        {
            Console.WriteLine($"[{DateTime.Now}] {message}");
        }

        public static void WriteLine(string message, MessageType type = MessageType.Default)
        {
            switch (type)
            {
                case MessageType.Default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case MessageType.Info:
                    Console.ForegroundColor = ConsoleColor.Cyan;

                    break;
                case MessageType.Success:
                    Console.ForegroundColor = ConsoleColor.Green;

                    break;
                case MessageType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;

                    break;
                case MessageType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;

                    break;
            }

            WriteLine(message);
        }
    }
}
