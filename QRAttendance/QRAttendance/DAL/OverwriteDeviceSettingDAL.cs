using MySql.Data.MySqlClient;
using QRAttendance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRAttendance.DAL
{
    public class OverwriteDeviceSettingDAL
    {
        OverwriteDevice_Setting overwriteSetting;
        string queryStr;
        int result = 0;
        MySqlConnection sqlCon = SqlConfiguration.GetSqlConnection;
        MySqlCommand sqlCmd;
        MySqlDataReader reader;

        public static OverwriteDevice_Setting GetOverwriteSettings()
        {
            OverwriteDevice_Setting overwriteSetting = new OverwriteDevice_Setting();
            MySqlConnection sqlCon = SqlConfiguration.GetSqlConnection;
            string queryStr = "Select * From OverwriteDevice_Setting";

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
                                overwriteSetting.OverwriteDeviceID = int.Parse(reader["OverwriteDeviceID"].ToString());
                                overwriteSetting.OverwriteEnabled = int.Parse(reader["OverwriteEnabled"].ToString()) == 1 ? true : false;
                                overwriteSetting.TotalNumberOfChances = int.Parse(reader["TotalNumberOfChances"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogDAL.WriteLog("GetOverwriteSettings() - " + e.Message);
            }
            return overwriteSetting;
        }
        //bool locEnabled, double schLat, double schLong, double schRad, bool includeField, double sch
        public static int InsertOverwriteDeviceSetting(OverwriteDevice_Setting overwriteSetting)
        {
            MySqlConnection sqlCon = SqlConfiguration.GetSqlConnection;
            int result = 0;

            int ovewriteEnabled = 0;
            if (overwriteSetting.OverwriteEnabled)
                ovewriteEnabled = 1;

            string queryStr = "Insert Into OverwriteDevice_Setting (OverwriteEnabled, TotalNumberOfChances) " +
                "Values (@ovewriteEnabled, @totalChances)";

            try
            {
                using (sqlCon)
                {
                    sqlCon.Open();

                    using (MySqlCommand sqlCmd = new MySqlCommand(queryStr, sqlCon))
                    {
                        sqlCmd.Parameters.AddWithValue("@ovewriteEnabled", ovewriteEnabled);
                        sqlCmd.Parameters.AddWithValue("@totalChances", overwriteSetting.TotalNumberOfChances);

                        result = sqlCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogDAL.WriteLog("InsertOverwriteDeviceSetting() - " + e.Message);
            }
            return result;
        }


        public static int UpdateOverwriteSetting(OverwriteDevice_Setting overwriteSetting)
        {
            int result = 0;
            MySqlConnection sqlCon = SqlConfiguration.GetSqlConnection;

            int ovewriteEnabled = 0;
            if (overwriteSetting.OverwriteEnabled)
                ovewriteEnabled = 1;

            string queryStr = "Update OverwriteDevice_Setting " +
                "Set OverwriteEnabled = @ovewriteEnabled, TotalNumberOfChances = @totalChances " +
                "Where OverwriteDeviceID = @overwriteID";

            try
            {
                using (sqlCon)
                {
                    sqlCon.Open();

                    using (MySqlCommand sqlCmd = new MySqlCommand(queryStr, sqlCon))
                    {
                        sqlCmd.Parameters.AddWithValue("@ovewriteEnabled", ovewriteEnabled);
                        sqlCmd.Parameters.AddWithValue("@totalChances", overwriteSetting.TotalNumberOfChances);
                        sqlCmd.Parameters.AddWithValue("@overwriteID", overwriteSetting.OverwriteDeviceID);

                        result = sqlCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogDAL.WriteLog("UpdateOverwriteSetting() - " + e.Message);
            }
            return result;
        }

    }
}