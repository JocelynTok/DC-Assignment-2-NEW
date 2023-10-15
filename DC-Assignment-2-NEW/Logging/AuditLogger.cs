using System;
using System.IO;

namespace DC_Assignment_2_NEW.Logging
{
    public static class AuditLogger
    {
        private static string logFilePath = "audit_log.txt";

        public static void LogActivity(string adminId, string activity)
        {
            string logEntry = $"{DateTime.Now}: Admin {adminId} performed activity: {activity}";

            try
            {
                using (StreamWriter writer = File.AppendText(logFilePath))
                {
                    writer.WriteLine(logEntry);
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur while writing to the log file
                Console.WriteLine("Error writing to audit log: " + ex.Message);
            }
        }
    }
}