using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRAttendance.Models
{
    public class Student
    {
        private string adminNumber, status, email, password, school, diploma, fullName, uuID, lastRegisterDate, remarks;

        public string AdminNumber { get => adminNumber; set => adminNumber = value; }
        public string Status { get => status; set => status = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public string School { get => school; set => school = value; }
        public string Diploma { get => diploma; set => diploma = value; }
        public string FullName { get => fullName; set => fullName = value; }
        public string UUID { get => uuID; set => uuID = value; }
        public string LastRegisterDate { get => lastRegisterDate; set => lastRegisterDate = value; }
        public string Remarks { get => remarks; set => remarks = value; }
    }
}