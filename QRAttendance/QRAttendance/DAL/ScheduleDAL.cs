using MySql.Data.MySqlClient;
using QRAttendance.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace QRAttendance.DAL
{
    public class ScheduleDAL
    {
        Schedule schedule;
        List<Schedule> scheList;
        private string queryStr;
        private MySqlConnection sqlCon = SqlConfiguration.GetSqlConnection;
        private MySqlCommand sqlCmd;
        private MySqlDataReader reader;
        private int result = 0;

        private DataTable dt;

        public Schedule GetAttendanceByID(int scheID)
        {
            queryStr = "Select * from Schedule s Where ScheduleID = @scheID";

            try
            {
                using (sqlCon)
                {
                    sqlCon.Open();
                    using (sqlCmd = new MySqlCommand(queryStr, sqlCon))
                    {
                        sqlCmd.Parameters.AddWithValue("@scheID", scheID);

                        reader = sqlCmd.ExecuteReader();
                        if (reader.Read())
                        {
                            schedule = new Schedule();
                            schedule.ScheduleID = int.Parse(reader["ScheduleID"].ToString());
                            schedule.LessonID = int.Parse(reader["LessonID"].ToString());
                            schedule.AdminNumber = reader["AdminNumber"].ToString();
                            int attStatus = 0;
                            schedule.AttendanceStatus = int.TryParse(reader["AttendanceStatus"].ToString(), out attStatus) ? attStatus == 1 ? true : false : false;
                            schedule.ClockInTime = reader["ClockInTime"].ToString();
                            schedule.ClockOutTime = reader["ClockOutTime"].ToString();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogDAL.WriteLog("GetAttendanceByID() - " + e.Message);
            }
            return schedule;
        }


        public DataTable GetAllScheduleInDT()
        {
            dt = new DataTable();
            queryStr = "Select * From Schedule";

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
                ErrorLogDAL.WriteLog("GetAllScheduleInDT() - " + e.Message);
            }
            return dt;


        }

        public List<Schedule> GetAttendanceByLesson(int lessonID)
        {
            scheList = new List<Schedule>();
            queryStr = "Select s.ScheduleID, s.LessonID, s.AdminNumber, s.ClockInTime, s.ClockOutTime, s.AttendanceStatus, st.FullName from Schedule s " +
                "Inner Join Student st " +
                "On s.AdminNumber = st.AdminNumber " +
                "Where LessonID = @lessonID " +
                "Order By s.AdminNumber";

            try
            {
                using (sqlCon)
                {
                    sqlCon.Open();
                    using (sqlCmd = new MySqlCommand(queryStr, sqlCon))
                    {
                        sqlCmd.Parameters.AddWithValue("@lessonID", lessonID);

                        reader = sqlCmd.ExecuteReader();
                        while (reader.Read())
                        {
                            schedule = new Schedule();
                            schedule.ScheduleID = int.Parse(reader["ScheduleID"].ToString());
                            schedule.LessonID = int.Parse(reader["LessonID"].ToString());
                            schedule.AdminNumber = reader["AdminNumber"].ToString();
                            schedule.StudentName = reader["FullName"].ToString();
                            int attStatus = 0;
                            schedule.AttendanceStatus = int.TryParse(reader["AttendanceStatus"].ToString(), out attStatus) ? attStatus == 1 ? true : false : false;
                            schedule.ClockInTime = reader["ClockInTime"].ToString();
                            schedule.ClockOutTime = reader["ClockOutTime"].ToString();
                            scheList.Add(schedule);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogDAL.WriteLog("GetAttendanceByLesson() - " + e.Message);
            }
            return scheList;
        }

        public int InsertAllSchedule(DataTable dt)
        {
            queryStr = "Insert Into Schedule " +
                "(ScheduleID, LessonID, AdminNumber) " +
                "Values " +
                "(@scheduleID, @lessonID, @adminNum)";

            try
            {
                using (sqlCon)
                {
                    sqlCon.Open();

                    foreach (DataRow dr in dt.Rows)
                    {
                        using (sqlCmd = new MySqlCommand(queryStr, sqlCon))
                        {
                            if (!string.IsNullOrEmpty(dr["ScheduleID"].ToString()) && !string.IsNullOrWhiteSpace(dr["ScheduleID"].ToString()))
                            {
                                sqlCmd.Parameters.AddWithValue("@scheduleID", dr["ScheduleID"]);
                                sqlCmd.Parameters.AddWithValue("@lessonID", dr["LessonID"]);
                                sqlCmd.Parameters.AddWithValue("@adminNum", dr["AdminNumber"]);
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
                ErrorLogDAL.WriteLog("InsertAllSchedule() - " + e.Message);
            }
            return result;
        }

        public int UpdateAttendance(int scheduleID, int status, string clockInTime, string clockOutTime)
        {
            queryStr = "Update Schedule Set AttendanceStatus = @status";

            DateTime clockIn, clockOut;
            if (DateTime.TryParse(clockInTime, out clockIn))  //this checks if input is null or invalid format.
                queryStr += ", ClockInTime = @clockInTime";

            if (DateTime.TryParse(clockOutTime, out clockOut))  //this checks if input is null or invalid format.
                queryStr += ", ClockOutTIme = @clockOutTime";

            queryStr += " Where ScheduleID = @scheID ";

            try
            {
                using (sqlCon)
                {
                    sqlCon.Open();

                    using (sqlCmd = new MySqlCommand(queryStr, sqlCon))
                    {
                        sqlCmd.Parameters.AddWithValue("@scheID", scheduleID);
                        sqlCmd.Parameters.AddWithValue("@status", status);
                        sqlCmd.Parameters.AddWithValue("@clockInTime", clockInTime);
                        sqlCmd.Parameters.AddWithValue("@clockOutTime", clockOutTime);

                        result = sqlCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogDAL.WriteLog("UpdateAttendance() - " + e.Message);
            }
            return result;
        }

        public int UpdateAttendanceOnlyStatus(int scheduleID, int status)
        {

            DateTime clockInTime = DateTime.Now;

            queryStr = "Update Schedule Set AttendanceStatus = @status, ClockInTime = @clockInTime Where ScheduleID = @scheID ";

            try
            {
                using (sqlCon)
                {
                    sqlCon.Open();

                    using (sqlCmd = new MySqlCommand(queryStr, sqlCon))
                    {
                        sqlCmd.Parameters.AddWithValue("@scheID", scheduleID);
                        if (status == 1)
                            sqlCmd.Parameters.AddWithValue("@clockInTime", clockInTime);
                        else
                            sqlCmd.Parameters.AddWithValue("@clockInTime", null);

                        sqlCmd.Parameters.AddWithValue("@status", status);

                        result = sqlCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogDAL.WriteLog("UpdateAttendanceOnlyStatus() - " + e.Message);
            }
            return result;
        }

        public int ClearAllSchedule()
        {
            queryStr = "Truncate Table Schedule";

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
                ErrorLogDAL.WriteLog("ClearAllSchedule() - " + e.Message);
            }

            return result;
        }
    }
}