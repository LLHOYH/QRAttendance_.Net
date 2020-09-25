using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRAttendance.Models
{
    public class Staff
    {
        private string staffID, staffName, password, staffType;

        public string StaffID { get => staffID; set => staffID = value; }
        public string StaffName { get => staffName; set => staffName = value; }
        public string Password { get => password; set => password = value; }
        public string StaffType { get => staffType; set => staffType = value; }
    }
}