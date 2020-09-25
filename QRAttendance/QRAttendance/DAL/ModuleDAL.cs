using MySql.Data.MySqlClient;
using QRAttendance.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace QRAttendance.DAL
{
    public class ModuleDAL
    {
        Module module;
        private string queryStr;
        private MySqlConnection sqlCon = SqlConfiguration.GetSqlConnection;
        private MySqlCommand sqlCmd;
        private MySqlDataReader reader;
        private int result = 0;

        private DataTable dt;

        public DataTable GetAllModulesInDT()
        {
            dt = new DataTable();
            queryStr = "Select * From Module";

            try
            {
                using (sqlCon)
                {
                    sqlCon.Open();

                    using (sqlCmd = new MySqlCommand(queryStr, sqlCon))
                    {

                        using (MySqlDataAdapter adap = new MySqlDataAdapter())
                        {
                            adap.SelectCommand = sqlCmd;
                            adap.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogDAL.WriteLog("GetAllModulesInDT() - " + e.Message);
            }
            return dt;

        }

        public Module GetModuleDetails(string adminNumber)
        {

            try
            {
                using (sqlCon)
                {
                    sqlCon.Open();

                    using (sqlCmd = new MySqlCommand(queryStr, sqlCon))
                    {

                        using (reader = sqlCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                module = new Module();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogDAL.WriteLog("GetModuleDetails() - " + e.Message);
            }
            return module;
        }


        public int InsertAllModules(DataTable dt)
        {
            queryStr = "Insert Into Module " +
                "(ModuleCode, ModuleName) " +
                "Values " +
                "(@moduleCode, @moduleName)";

            try
            {
                using (sqlCon)
                {
                    sqlCon.Open();

                    foreach (DataRow dr in dt.Rows)
                    {
                        using (sqlCmd = new MySqlCommand(queryStr, sqlCon))
                        {
                            if (!string.IsNullOrEmpty(dr["ModuleCode"].ToString()) && !string.IsNullOrWhiteSpace(dr["ModuleCode"].ToString()))
                            {
                                sqlCmd.Parameters.AddWithValue("@moduleCode", dr["ModuleCode"]);
                                sqlCmd.Parameters.AddWithValue("@moduleName", dr["ModuleName"]);

                                result = sqlCmd.ExecuteNonQuery();
                                if (result < 1)
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogDAL.WriteLog("InsertAllModules() - " + e.Message);
            }
            return result;
        }

        public int ClearAllModules()
        {
            queryStr = "Truncate Table Module";

            try
            {
                using (sqlCon)
                {
                    sqlCon.Open();

                    using (sqlCmd = new MySqlCommand(queryStr, sqlCon))
                    {
                        result = sqlCmd.ExecuteNonQuery();
                    }


                }
            }
            catch (Exception e)
            {
                ErrorLogDAL.WriteLog("ClearAllModules() - " + e.Message);
            }
            return result;
        }
    }
}