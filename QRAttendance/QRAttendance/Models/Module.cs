using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRAttendance.Models
{
    [Serializable]
    public class Module
    {
        private string moduleCode, moduleName;

        public string ModuleCode { get => moduleCode; set => moduleCode = value; }
        public string ModuleName { get => moduleName; set => moduleName = value; }
    }
}