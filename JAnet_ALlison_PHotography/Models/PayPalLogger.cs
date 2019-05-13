using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace JAnet_ALlison_PHotography.Models
{
    public class PayPalLogger
    {
        public static string LogDirectoryPath = Environment.CurrentDirectory;

        public static void Log(String messages)
        {
            try
            {
                StreamWriter strw = new StreamWriter(LogDirectoryPath + "\\PayPalError.log", true);
                strw.WriteLine("{0}-->{1}", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), messages);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}