using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRAttendance.Models
{
    [Serializable]
    public class ModuleGroup : Module
    {
        int moduleGroupID, groupNumber;
        string moduleCode;


        public int ModuleGroupID { get => moduleGroupID; set => moduleGroupID = value; }
        public int GroupNumber { get => groupNumber; set => groupNumber = value; }
    }
}