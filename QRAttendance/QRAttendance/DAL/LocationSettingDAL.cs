using MySql.Data.MySqlClient;
using QRAttendance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRAttendance.DAL
{
    public class LocationSettingDAL
    {
        Location_Function_Setting locSetting;
        string queryStr;
        int result = 0;
        MySqlConnection sqlCon = SqlConfiguration.GetSqlConnection;
        MySqlCommand sqlCmd;
        MySqlDataReader reader;
        bool error = false;

        public static Location_Function_Setting GetLocationSettings()
        {
            Location_Function_Setting locSetting = new Location_Function_Setting();
            MySqlConnection sqlCon = SqlConfiguration.GetSqlConnection;
            string queryStr = "Select * From Location_Function_Setting";

            try
            {
                using (sqlCon)
                {
                    sqlCon.Open();

                    using (MySqlCommand sqlCmd = new MySqlCommand(queryStr, sqlCon))
                    {

                        using (MySqlDataReader reader = sqlCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                locSetting.LocationSettingID = int.Parse(reader["LocationSettingID"].ToString());
                                locSetting.LocationEnabled = int.Parse(reader["LocationEnabled"].ToString()) == 1 ? true : false;
                                locSetting.IncludeField = int.Parse(reader["IncludeField"].ToString()) == 1 ? true : false;
                                locSetting.NypSchoolLat = double.Parse(reader["NYPSchoolLat"].ToString());
                                locSetting.NypSchoolLong = double.Parse(reader["NYPSchoolLong"].ToString());
                                locSetting.NypFieldLat = double.Parse(reader["NYPFieldLat"].ToString());
                                locSetting.NypFieldLong = double.Parse(reader["NYPFieldLong"].ToString());
                                locSetting.SchoolRadiusInMetres = double.Parse(reader["SchoolRadiusInMetres"].ToString());
                                locSetting.FieldRadiusInMetres = double.Parse(reader["FieldRadiusInMetres"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogDAL.WriteLog("GetLocationSettings() - " + e.Message);
            }
            return locSetting;
        }
        //bool locEnabled, double schLat, double schLong, double schRad, bool includeField, double sch
        public static int InsertLocationSetting(Location_Function_Setting locSetting)
        {
            MySqlConnection sqlCon = SqlConfiguration.GetSqlConnection;
            int result = 0;

            int locEnabledInt = 0, includeFieldInt = 0;
            if (locSetting.LocationEnabled)
                locEnabledInt = 1;

            if (locSetting.IncludeField)
                includeFieldInt = 1;

            string queryStr = "Insert Into Location_Function_Setting " +
                "(LocationEnabled, NYPSchoolLat, NYPSchoolLong, SchoolRadiusInMetres, IncludeField, NYPFieldLat, NYPFieldLong, FieldRadiusInMetres)  " +
                "Values " +
                "(@locEnabled, @nypSchLat, @nypSchLong, @schoolRad, @includeField, @nypFieldLat, @nypFieldLong, @fieldRad)";

            try
            {
                using (sqlCon)
                {
                    sqlCon.Open();

                    using (MySqlCommand sqlCmd = new MySqlCommand(queryStr, sqlCon))
                    {
                        sqlCmd.Parameters.AddWithValue("@locEnabled", locEnabledInt);
                        sqlCmd.Parameters.AddWithValue("@nypSchLat", locSetting.NypSchoolLat);
                        sqlCmd.Parameters.AddWithValue("@nypSchLong", locSetting.NypSchoolLong);
                        sqlCmd.Parameters.AddWithValue("@schoolRad", locSetting.SchoolRadiusInMetres);
                        sqlCmd.Parameters.AddWithValue("@includeField", includeFieldInt);
                        sqlCmd.Parameters.AddWithValue("@nypFieldLat", locSetting.NypFieldLat);
                        sqlCmd.Parameters.AddWithValue("@nypFieldLong", locSetting.NypFieldLong);
                        sqlCmd.Parameters.AddWithValue("@fieldRad", locSetting.FieldRadiusInMetres);

                        result = sqlCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogDAL.WriteLog("InsertLocationSetting() - " + e.Message);
            }
            return result;
        }


        public static int UpdateLocationEnable(bool locEnabled, int locID)
        {
            int result = 0;
            MySqlConnection sqlCon = SqlConfiguration.GetSqlConnection;

            int locEnabledInt = 0;
            if (locEnabled)
                locEnabledInt = 1;

            string queryStr = "Update Location_Function_Setting Set LocationEnabled = @locEnabled " +
                "Where LocationSettingID = @locID ";

            try
            {
                using (sqlCon)
                {
                    sqlCon.Open();

                    using (MySqlCommand sqlCmd = new MySqlCommand(queryStr, sqlCon))
                    {
                        sqlCmd.Parameters.AddWithValue("@locEnabled", locEnabledInt);
                        sqlCmd.Parameters.AddWithValue("@locID", locID);

                        result = sqlCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogDAL.WriteLog("UpdateLocationEnable() - " + e.Message);
            }
            return result;
        }

        public static int UpdateIncludeFieldStatus(bool includeField, int locID)
        {
            int result = 0;
            MySqlConnection sqlCon = SqlConfiguration.GetSqlConnection;

            int includeFieldInt = 0;
            if (includeField)
                includeFieldInt = 1;

            string queryStr = "Update Location_Function_Setting Set IncludeField = @includeField " +
                "Where LocationSettingID = @locID ";

            try
            {
                using (sqlCon)
                {
                    sqlCon.Open();

                    using (MySqlCommand sqlCmd = new MySqlCommand(queryStr, sqlCon))
                    {
                        sqlCmd.Parameters.AddWithValue("@includeField", includeFieldInt);
                        sqlCmd.Parameters.AddWithValue("@locID", locID);

                        result = sqlCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogDAL.WriteLog("UpdateIncludeFieldStatus() - " + e.Message);
            }
            return result;
        }


    }
}