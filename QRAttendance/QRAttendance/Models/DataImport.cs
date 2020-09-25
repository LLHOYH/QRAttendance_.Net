using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRAttendance.Models
{
    public class DataImport
    {
        int importID;
        DateTime importedDate;
        string importedBy, importForAcadYr, importForSemester, importedTime;

        public int ImportID { get => importID; set => importID = value; }
        public DateTime ImportedDate { get => importedDate; set => importedDate = value; }
        public string ImportedTime { get => importedTime; set => importedTime = value; }
        public string ImportedBy { get => importedBy; set => importedBy = value; }
        public string ImportForAcadYr { get => importForAcadYr; set => importForAcadYr = value; }
        public string ImportForSemester { get => importForSemester; set => importForSemester = value; }
    }
}