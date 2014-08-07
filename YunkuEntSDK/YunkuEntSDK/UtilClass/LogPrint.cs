using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace YunkuEntSDK.UtilClass
{
    class LogPrint
    {
        private const string YUNKU_FILE = "yunkusdk_{0}.log";

        public static void Print(string log)
        {
            if (ParentManager.LogPrintAvialable)
            {

                StreamWriter logWriter;
                string dateStr=DateTime.Now.ToString("yyyy_MM_dd");
                string fileName=string.Format(YUNKU_FILE,dateStr);

                if (!File.Exists(fileName))
                {
                    logWriter = new StreamWriter(fileName);
                }
                else
                {
                    logWriter = File.AppendText(fileName);
                }

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
