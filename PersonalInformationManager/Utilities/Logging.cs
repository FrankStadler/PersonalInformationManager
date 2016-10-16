using System;
using System.Text;

namespace PersonalInformationManager.Utilities
{
    public static class Logging
    {
        public static void WriteLog(string logFile, string text)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine(DateTime.Now.ToString());
            message.AppendLine(text);
            message.AppendLine("=========================================");

            System.IO.File.AppendAllText(logFile, message.ToString());
        }
    }
}