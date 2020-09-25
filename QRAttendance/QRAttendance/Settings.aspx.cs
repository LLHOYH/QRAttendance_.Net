using QRAttendance.DAL;
using QRAttendance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QRAttendance
{
    public partial class Settings : System.Web.UI.Page
    {
        Location_Function_Setting locSetting;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DisplayAllFields();
            }
        }

        public static Location_Function_Setting GetPresetLoctionSetting()
        {

            Location_Function_Setting presetLoc = new Location_Function_Setting();
            presetLoc.LocationEnabled = true;
            presetLoc.NypSchoolLat = 1.379542;
            presetLoc.NypSchoolLong = 103.849769;
            presetLoc.SchoolRadiusInMetres = 330;
            presetLoc.IncludeField = true;
            presetLoc.NypFieldLat = 1.38322;
            presetLoc.NypFieldLong = 103.849876;
            presetLoc.FieldRadiusInMetres = 130;

            return presetLoc;
        }

        public void DisplayAllFields()
        {
            Location_Function_Setting locationSetting = LocationSettingDAL.GetLocationSettings();
            lbl_SchoolLat.Text = locationSetting.NypSchoolLat.ToString();
            lbl_SchoolLong.Text = locationSetting.NypSchoolLong.ToString();
            lbl_SchoolRadius.Text = locationSetting.SchoolRadiusInMetres.ToString();
            lbl_FieldLat.Text = locationSetting.NypFieldLat.ToString();
            lbl_FieldLong.Text = locationSetting.NypFieldLong.ToString();
            lbl_FieldRadius.Text = locationSetting.FieldRadiusInMetres.ToString();


            OverwriteDevice_Setting overwriteSetting = OverwriteDeviceSettingDAL.GetOverwriteSettings();
            tb_Chances.Text = overwriteSetting.TotalNumberOfChances.ToString();

        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public static bool GetLocationSettings()  //get location setting on page load (called in jquery)
        {
            Location_Function_Setting locationSetting = LocationSettingDAL.GetLocationSettings();
            if (locationSetting.LocationSettingID == 0) //if database is empty, insert preset settings.
            {
                locationSetting = GetPresetLoctionSetting();
                LocationSettingDAL.InsertLocationSetting(locationSetting);
            }
            return locationSetting.LocationEnabled;
        }


        [WebMethod]
        public static string UpdateLocationEnable(string locationStatus)
        {
            Location_Function_Setting locationSetting = LocationSettingDAL.GetLocationSettings();
            bool status = false;
            if (locationStatus.Equals("enable"))
                status = true;
            else if (locationStatus.Equals("disable"))
                status = false;
            else
                return "0";

            int result = LocationSettingDAL.UpdateLocationEnable(status, locationSetting.LocationSettingID);

            return result.ToString();
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public static bool GetIncludeFieldStatus()
        {
            Location_Function_Setting locationSetting = LocationSettingDAL.GetLocationSettings();
            return locationSetting.IncludeField;
        }

        [WebMethod]
        public static string UpdateIncludeFieldStatus(string incFieldStatus)
        {
            Location_Function_Setting locationSetting = LocationSettingDAL.GetLocationSettings();
            bool status = false;
            if (incFieldStatus.Equals("include"))
                status = true;
            else if (incFieldStatus.Equals("disclude"))
                status = false;
            else
                return "0";

            int result = LocationSettingDAL.UpdateIncludeFieldStatus(status, locationSetting.LocationSettingID);

            return result.ToString();
        }



        //overwrite device setting


        public static OverwriteDevice_Setting GetPresetOverwriteSetting()
        {

            OverwriteDevice_Setting overwriteSetting = new OverwriteDevice_Setting();
            overwriteSetting.OverwriteEnabled = true;
            overwriteSetting.TotalNumberOfChances = 3;

            return overwriteSetting;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public static bool GetOverwriteStatus()  //get location setting on page load (called in jquery)
        {
            OverwriteDevice_Setting overwriteSetting = OverwriteDeviceSettingDAL.GetOverwriteSettings();
            if (overwriteSetting.OverwriteDeviceID == 0) //if database is empty, insert preset settings.
            {
                overwriteSetting = GetPresetOverwriteSetting();
                OverwriteDeviceSettingDAL.InsertOverwriteDeviceSetting(overwriteSetting);
            }
            return overwriteSetting.OverwriteEnabled;
        }


        [WebMethod]
        public static string UpdateOverwriteSetting(string overwriteStatus, string totalChances)
        {

            //call this method to get ID.
            OverwriteDevice_Setting overwriteSetting = OverwriteDeviceSettingDAL.GetOverwriteSettings();

            bool status = false;
            if (overwriteStatus.Equals("enable"))
                status = true;
            else if (overwriteStatus.Equals("disable"))
                status = false;
            else
                return "0";

            int chancesInt = 0;
            int result = 0;

            if (int.TryParse(totalChances, out chancesInt))
            {
                overwriteSetting.OverwriteEnabled = status;
                overwriteSetting.TotalNumberOfChances = chancesInt;
                result  = OverwriteDeviceSettingDAL.UpdateOverwriteSetting(overwriteSetting);
            }

            return result.ToString();
        }
    }
}