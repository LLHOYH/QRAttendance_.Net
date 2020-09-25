using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace QRAttendance.DAL
{
    public class ErrorLogDAL
    {
        public static void WriteLog(string str_ErrorMessage)
        {
            using (StreamWriter sw = new StreamWriter(System.Web.HttpContext.Current.Server.MapPath("~/ErrorLog.log"), true))
            {
                sw.WriteLine(DateTime.Now + " - " + str_ErrorMessage);
            }
        }
    }
}