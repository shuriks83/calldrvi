using System;
using System.IO;
using System.Text;

namespace calldrvi
{
    static class Logger
    {
        private static readonly object _lock = new object();
        private static string logFile = "calldrvi.log";

        public static void Write(string text)
        {
            lock (_lock)
            {
                File.AppendAllText(logFile, text + Environment.NewLine, Encoding.UTF8);
            }
        }

        public static void Separator()
        {
            Write(new string('-', 60));
        }
    }
}