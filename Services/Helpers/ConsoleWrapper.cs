using System;

namespace Services.Helpers
{
    public class ConsoleWrapper : IConsole
    {
        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
