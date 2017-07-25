using System;
using System.IO;

namespace YunkuEntSDK.UtilClass
{
    public class LogPrint
    {
        private const string YunkuFile = "yunkusdk_{0}.log";

        public static void Print(string log)
        {
            if (DebugConfig.LogPrintAvialable)
            {
                StreamWriter logWriter;
                string dateStr = DateTime.Now.ToString("yyyy_MM_dd");
                string fileName = string.Format(DebugConfig.LogPath + "/" + YunkuFile, dateStr);

                if (!string.IsNullOrEmpty(DebugConfig.LogPath) && !Directory.Exists(DebugConfig.LogPath))
                {
                    Directory.CreateDirectory(DebugConfig.LogPath);
                }

                if (!File.Exists(fileName))
                {
                    logWriter = new StreamWriter(fileName);
                }
                else
                {
                    logWriter = File.AppendText(fileName);
                }

                Console.WriteLine(log);
                // Write to the file:
                logWriter.WriteLine(DateTime.Now);
                logWriter.WriteLine(log);
                logWriter.WriteLine();

                // Close the stream:
                logWriter.Close();
            }
        }
    }
}