using MySql.Data.MySqlClient;
using QRAttendance.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace QRAttendance.DAL
{
    public class StudentDAL
    {
        Student student;
        private string queryStr;
        private MySqlConnection sqlCon = SqlConfiguration.GetSqlConnection;
        private MySqlCommand sqlCmd;
        private MySqlDataReader reader;
        private int result = 0;
        private DataTable dt;

        public DataTable GetAllStudentsInDT()
        {
            dt = new DataTable();
            queryStr = "Select AdminNumber, Email, School, Diploma, FullName, UUID, LastRegisterDate, " +
                "Remarks, Status From Student";

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
                ErrorLogDAL.WriteLog("GetAllStudentsInDT() - " + e.Message);
            }

            return dt;
        }

        public Student GetStudentDetails(string adminNumber)
        {
            queryStr = "Select * From Student " +
                "Where StaffID = @staffID " +
                "And Password = @password";

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
                                student = new Student();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogDAL.WriteLog("GetStudentDetails() - " + e.Message);
            }
            return student;
        }


        public int InsertAllStudents(DataTable dt)
        {
            queryStr = "Insert Into Student " +
                "(AdminNumber, Email, School, Diploma, FullName) " +
                "Values " +
                "(@adminNum, @email, @school, @diploma, @fullName)";

            try
            {
                using (sqlCon)
                {
                    sqlCon.Open();

                    foreach (DataRow dr in dt.Rows)
                    {
                        using (sqlCmd = new MySqlCommand(queryStr, sqlCon))
                        {
                            //check if the line is empty by using the first attribute
                            if (!string.IsNullOrEmpty(dr["AdminNumber"].ToString()) && !string.IsNullOrWhiteSpace(dr["AdminNumber"].ToString()))
                            {
                                sqlCmd.Parameters.AddWithValue("@adminNum", dr["AdminNumber"]);
                                sqlCmd.Parameters.AddWithValue("@email", dr["Email"]);
                                sqlCmd.Parameters.AddWithValue("@school", dr["School"]);
                                sqlCmd.Parameters.AddWithValue("@diploma", dr["Diploma"]);
                                sqlCmd.Parameters.AddWithValue("@fullName", dr["FullName"]);

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
                ErrorLogDAL.WriteLog("InsertAllStudents() - " + e.Message);
            }

            return result;
        }

        public int ClearAllStudents()
        {
            queryStr = "Truncate Table Student";

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
                ErrorLogDAL.WriteLog("ClearAllStudents() - " + e.Message);
            }

            return result;
        }
    }
}