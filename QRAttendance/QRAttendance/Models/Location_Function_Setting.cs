using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRAttendance.Models
{
    public class Location_Function_Setting
    {
        int locationSettingID;
        bool locationEnabled, includeField;
        double nypSchoolLat, nypSchoolLong, schoolRadiusInMetres, nypFieldLat, nypFieldLong, fieldRadiusInMetres;
        
        public int LocationSettingID { get => locationSettingID; set => locationSettingID = value; }

        //determin whether this whole location function will be used or not
        public bool LocationEnabled { get => locationEnabled; set => locationEnabled = value; }

        public double NypSchoolLat { get => nypSchoolLat; set => nypSchoolLat = value; }
        public double NypSchoolLong { get => nypSchoolLong; set => nypSchoolLong = value; }
        public double SchoolRadiusInMetres { get => schoolRadiusInMetres; set => schoolRadiusInMetres = value; }


        //determin whether to use the school field(sports side) or not
        public bool IncludeField { get => includeField; set => includeField = value; }

        public double NypFieldLat { get => nypFieldLat; set => nypFieldLat = value; }
        public double NypFieldLong { get => nypFieldLong; set => nypFieldLong = value; }
        public double FieldRadiusInMetres { get => fieldRadiusInMetres; set => fieldRadiusInMetres = value; }
    }
}