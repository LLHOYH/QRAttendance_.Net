using MySql.Data.MySqlClient;
using QRAttendance.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace QRAttendance.DAL
{
    public class StaffDAL
    {
        private Staff staff;
        private string queryStr;
        private MySqlConnection sqlCon = SqlConfiguration.GetSqlConnection;
        private MySqlCommand sqlCmd;
        private MySqlDataReader reader;
        private int result = 0;

        private DataTable dt;

        public DataTable GetAllStaffsInDT()
        {
            dt = new DataTable();
            queryStr = "Select * From Staff";

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
                ErrorLogDAL.WriteLog("GetAllStaffsInDT() - " + e.Message);
            }

            return dt;


        }

        public Staff GetStaffByID(string staffID)
        {
            queryStr = "Select StaffID, StaffName, StaffType From Staff " +
                "Where StaffID = @staffID ";

            try
            {
                using (sqlCon)
                {
                    sqlCon.Open();

                    using (sqlCmd = new MySqlCommand(queryStr, sqlCon))
                    {
                        sqlCmd.Parameters.AddWithValue("@staffID", staffID);

                        using (reader = sqlCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                staff = new Staff();

                                staff.StaffID = staffID;
                                staff.StaffName = reader["StaffName"].ToString();
                                staff.StaffType = reader["StaffType"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogDAL.WriteLog("GetStaffByID() - " + e.Message);
            }

            return staff;
        }


        public int InsertAllStaffs(DataTable dt)
        {
            queryStr = "Insert Into Staff " +
                "(StaffID, StaffName, StaffType) " +
                "Values " +
                "(@staffID, @staffName, @staffType)";

            try
            {
                using (sqlCon)
                {
                    sqlCon.Open();

                    foreach (DataRow dr in dt.Rows)
                    {

                        using (sqlCmd = new MySqlCommand(queryStr, sqlCon))
                        {
                            if (!string.IsNullOrEmpty(dr["StaffID"].ToString()) && !string.IsNullOrWhiteSpace(dr["StaffID"].ToString()))
                            {
                                sqlCmd.Parameters.AddWithValue("@staffID", dr["StaffID"]);
                                sqlCmd.Parameters.AddWithValue("@staffName", dr["StaffName"]);
                                sqlCmd.Parameters.AddWithValue("@staffType", dr["StaffType"]);

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
                ErrorLogDAL.WriteLog("InsertAllStaffs() - " + e.Message);
            }

            return result;
        }

        public int ClearStaffsOtherThenSuper()
        {
            queryStr = "Delete From Staff Where StaffType!='Super'";

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
                ErrorLogDAL.WriteLog("ClearStaffsOtherThenSuper() - " + e.Message);
            }

            return result;
        }

        public int ClearAllStaff()
        {
            queryStr = "Truncate Table Staff";

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
                ErrorLogDAL.WriteLog("ClearAllStaff() - " + e.Message);
            }

            return result;
        }

    }
}