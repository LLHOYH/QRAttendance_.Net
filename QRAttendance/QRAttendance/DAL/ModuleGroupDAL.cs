using MySql.Data.MySqlClient;
using QRAttendance.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace QRAttendance.DAL
{
    public class ModuleGroupDAL
    {
        ModuleGroup moduleGrp;
        List<ModuleGroup> moduleGrpList;
        private string queryStr;
        private MySqlConnection sqlCon = SqlConfiguration.GetSqlConnection;
        private MySqlCommand sqlCmd;
        private MySqlDataReader reader;
        private int result = 0;
        private DataTable dt;

        public DataTable GetAllModuleGrpsInDT()
        {
            dt = new DataTable();
            queryStr = "Select * From ModuleGroup";

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
                ErrorLogDAL.WriteLog("GetAllModuleGrpsInDT() - " + e.Message);
            }
            return dt;

        }

        public ModuleGroup GetModuleGroupDetails(string adminNumber)
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
                                moduleGrp = new ModuleGroup();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogDAL.WriteLog("GetModuleGroupDetails() - " + e.Message);
            }
            return moduleGrp;
        }

        public ModuleGroup GetModuleGroupByCodeAndID(string moduleCode, int moduleGrpID)
        {
            moduleGrp = new ModuleGroup();

            queryStr = "Select Distinct m.ModuleCode, m.ModuleName, mg.ModuleGroupID, mg.GroupNumber From Module m " +
                "Inner Join ModuleGroup mg " +
                "On m.ModuleCode = mg.ModuleCode " +
                "Where m.ModuleCode = @moduleCode ";
            if (moduleGrpID != 0)
                queryStr += "And mg.ModuleGroupID = @moduleGroupID";

            try
            {
                using (sqlCon)
                {
                    sqlCon.Open();

                    using (sqlCmd = new MySqlCommand(queryStr, sqlCon))
                    {
                        sqlCmd.Parameters.AddWithValue("@moduleCode", moduleCode);
                        sqlCmd.Parameters.AddWithValue("@moduleGroupID", moduleGrpID);

                        using (reader = sqlCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                moduleGrp.ModuleCode = reader["ModuleCode"].ToString();
                                moduleGrp.ModuleGroupID = moduleGrpID != 0 ? int.Parse(reader["ModuleGroupID"].ToString()) : 0;
                                moduleGrp.ModuleName = reader["ModuleName"].ToString();
                                moduleGrp.GroupNumber = int.Parse(reader["GroupNumber"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogDAL.WriteLog("GetModuleGroupByCodeAndID() - " + e.Message);
            }
            return moduleGrp;
        }

        public int InsertAllModuleGroups(DataTable dt)
        {
            queryStr = "Insert Into ModuleGroup " +
                "(ModuleCode, GroupNumber) " +
                "Values " +
                "(@moduleCode, @groupNumber)";

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
                                sqlCmd.Parameters.AddWithValue("@groupNumber", dr["GroupNumber"]);

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
                ErrorLogDAL.WriteLog("InsertAllModuleGroups() - " + e.Message);
            }
            return result;
        }

        public int ClearAllModuleGroups()
        {
            queryStr = "Truncate Table ModuleGroup";

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
                ErrorLogDAL.WriteLog("ClearAllModuleGroups() - " + e.Message);
            }

            return result;
        }
    }
}