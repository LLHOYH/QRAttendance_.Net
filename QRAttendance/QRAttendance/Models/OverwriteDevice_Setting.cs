using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRAttendance.Models
{
    public class OverwriteDevice_Setting
    {
        int overwriteDeviceID;
        bool overwriteEnabled;
        int totalNumberOfChances;


        public int OverwriteDeviceID { get => overwriteDeviceID; set => overwriteDeviceID = value; }
        public bool OverwriteEnabled { get => overwriteEnabled; set => overwriteEnabled = value; }
        public int TotalNumberOfChances { get => totalNumberOfChances; set => totalNumberOfChances = value; }
    }
}