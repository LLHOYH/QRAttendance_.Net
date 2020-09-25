using MySql.Data.MySqlClient;
using QRAttendance.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace QRAttendance.DAL
{
    public class LessonDAL
    {

        Lesson lesson;
        List<Lesson> lessonList;
        private string queryStr;
        private MySqlConnection sqlCon = SqlConfiguration.GetSqlConnection;
        private MySqlCommand sqlCmd;
        private MySqlDataReader reader;
        private int result = 0;
        private DataTable dt;

        public DataTable GetAllLessonsInDT()
        {
            dt = new DataTable();
            queryStr = "Select * From Lesson";

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
                ErrorLogDAL.WriteLog("GetAllLessonsInDT() - " + e.Message);
            }
            return dt;
        }

        public Lesson GetLessonWhenLogin(string staffID)
        {

            try
            {
                queryStr = "select * from Lesson " +
                    "Where SubTime(LessonTime, '0:10') < " +
                    "Convert(AddTime(utc_timeStamp(), '08:00:00'), Time) " +
                    "And SubTime(AddTime(LessonTime, Concat(Convert(LessonDuration, char),':0:0')), '0:10') > " +
                    "Convert(AddTime(utc_timeStamp(), '08:00:00'), Time) " +
                    "And LessonDate = Convert(AddTime(utc_timeStamp(), '08:00:00'), Date) " +
                    "And StaffID = @staffID";

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
                                lesson = new Lesson();
                                int moduleGrpID = 0;
                                lesson.LessonID = int.Parse(reader["LessonID"].ToString());
                                lesson.StaffID = reader["StaffID"].ToString();
                                lesson.ModuleCode = reader["ModuleCode"].ToString();
                                lesson.LessonType = reader["LessonType"].ToString();
                                lesson.LessonTime = reader["LessonTime"].ToString();
                                lesson.LessonDate = Convert.ToDateTime(reader["LessonDate"].ToString());
                                lesson.LessonDay = reader["LessonDay"].ToString();
                                lesson.LessonVenue = reader["LessonVenue"].ToString();
                                lesson.LessonToModuleGroup = int.Parse(reader["LessonToModuleGroup"].ToString()) == 0 ? false : true;
                                lesson.LessonDisplayText = lesson.LessonType + ", " + lesson.LessonVenue + ", " + lesson.LessonDate.ToString("dd-MMM-yy") + ", " + lesson.LessonTime.Substring(0, 5) + " (" + lesson.LessonDay + ")";

                                int.TryParse(reader["ModuleGroupID"].ToString(), out moduleGrpID);
                                lesson.ModuleGroupID = moduleGrpID;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogDAL.WriteLog("GetLessonWhenLogin() - " + e.Message);
            }

            return lesson;
        }

        public List<Lesson> GetAllLessonsOfStaffForTheDay(string staffID, DateTime lessonDate)
        {
            lessonList = new List<Lesson>();

            queryStr = "select * from Lesson " +
                "Where LessonDate = Date_Format(@lessonDate, '%Y-%m-%d') " +
                "And StaffID = @staffID";

            try
            {
                using (sqlCon)
                {
                    sqlCon.Open();

                    using (sqlCmd = new MySqlCommand(queryStr, sqlCon))
                    {
                        sqlCmd.Parameters.AddWithValue("@staffID", staffID);
                        sqlCmd.Parameters.AddWithValue("@lessonDate", lessonDate);

                        using (reader = sqlCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lesson = new Lesson();
                                int moduleGrpID = 0;
                                lesson.LessonID = int.Parse(reader["LessonID"].ToString());
                                lesson.StaffID = reader["StaffID"].ToString();
                                lesson.ModuleCode = reader["ModuleCode"].ToString();
                                lesson.LessonType = reader["LessonType"].ToString();
                                lesson.LessonTime = reader["LessonTime"].ToString();
                                lesson.LessonDate = Convert.ToDateTime(reader["LessonDate"].ToString());
                                lesson.LessonDay = reader["LessonDay"].ToString();
                                lesson.LessonVenue = reader["LessonVenue"].ToString();
                                lesson.LessonToModuleGroup = int.Parse(reader["LessonToModuleGroup"].ToString()) == 0 ? false : true;
                                lesson.LessonDisplayText = lesson.LessonType + ", " + lesson.LessonVenue + ", " + lesson.LessonDate.ToString("dd-MMM-yy") + ", " + lesson.LessonTime.Substring(0, 5) + " (" + lesson.LessonDay + ")";

                                int.TryParse(reader["ModuleGroupID"].ToString(), out moduleGrpID);
                                lesson.ModuleGroupID = moduleGrpID;

                                lessonList.Add(lesson);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogDAL.WriteLog("GetAllLessonsOfStaffForTheDay() - " + e.Message);
            }
            return lessonList;
        }

        public Lesson GetLessonByID(int lessonID)
        {
            queryStr = "select * from Lesson " +
                "Where LessonID=@lessonID";

            try
            {
                using (sqlCon)
                {
                    sqlCon.Open();

                    using (sqlCmd = new MySqlCommand(queryStr, sqlCon))
                    {
                        sqlCmd.Parameters.AddWithValue("@lessonID", lessonID);

                        using (reader = sqlCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lesson = new Lesson();
                                int moduleGrpID = 0;
                                lesson.LessonID = int.Parse(reader["LessonID"].ToString());
                                lesson.StaffID = reader["StaffID"].ToString();
                                lesson.ModuleCode = reader["ModuleCode"].ToString();
                                lesson.LessonType = reader["LessonType"].ToString();
                                lesson.LessonTime = reader["LessonTime"].ToString();
                                lesson.LessonDate = Convert.ToDateTime(reader["LessonDate"].ToString());
                                lesson.LessonDay = reader["LessonDay"].ToString();
                                lesson.LessonVenue = reader["LessonVenue"].ToString();
                                lesson.LessonToModuleGroup = int.Parse(reader["LessonToModuleGroup"].ToString()) == 0 ? false : true;
                                lesson.LessonDisplayText = lesson.LessonType + ", " + lesson.LessonVenue + ", " + lesson.LessonDate.ToString("dd-MMM-yy") + ", " + lesson.LessonTime.Substring(0, 5) + " (" + lesson.LessonDay + ")";

                                int.TryParse(reader["ModuleGroupID"].ToString(), out moduleGrpID);
                                lesson.ModuleGroupID = moduleGrpID;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogDAL.WriteLog("GetLessonByID() - " + e.Message);
            }
            return lesson;
        }

        public int InsertAllLessons(DataTable dt)
        {
            queryStr = "Insert Into Lesson " +
                "(LessonID, ModuleCode, StaffID, LessonType, LessonVenue, " +
                "LessonTime, LessonDate, LessonDay, LessonDuration, ModuleGroupID, LessonToModuleGroup) " +
                "Values " +
                "(@lessonID, @moduleCode, @staffID, @lessonType, @lessonVenue, " +
                "@lessonTime, @lessonDate, @lessonDay, @lessonDuration, @moduleGrpID, @lessonToModuleGrp)";

            try
            {
                using (sqlCon)
                {
                    sqlCon.Open();

                    foreach (DataRow dr in dt.Rows)
                    {
                        using (sqlCmd = new MySqlCommand(queryStr, sqlCon))
                        {
                            if (!string.IsNullOrEmpty(dr["LessonID"].ToString()) && !string.IsNullOrWhiteSpace(dr["LessonID"].ToString()))
                            {
                                sqlCmd.Parameters.AddWithValue("@lessonID", dr["LessonID"]);
                                sqlCmd.Parameters.AddWithValue("@moduleCode", dr["ModuleCode"]);
                                sqlCmd.Parameters.AddWithValue("@staffID", dr["StaffID"]);
                                sqlCmd.Parameters.AddWithValue("@lessonType", dr["LessonType"]);
                                sqlCmd.Parameters.AddWithValue("@lessonVenue", dr["LessonVenue"]);
                                sqlCmd.Parameters.AddWithValue("@lessonTime", Convert.ToDateTime(dr["LessonTime"].ToString()).ToString("HH:mm"));
                                sqlCmd.Parameters.AddWithValue("@lessonDate", Convert.ToDateTime(dr["LessonDate"].ToString()).ToString("yyyy-MM-dd"));
                                sqlCmd.Parameters.AddWithValue("@lessonDay", dr["LessonDay"]);
                                sqlCmd.Parameters.AddWithValue("@lessonDuration", dr["LessonDuration"]);
                                sqlCmd.Parameters.AddWithValue("@moduleGrpID", dr["ModuleGroupID"]);
                                sqlCmd.Parameters.AddWithValue("@lessonToModuleGrp", dr["LessonToModuleGroup"]);

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
                ErrorLogDAL.WriteLog("InsertAllLessons() - " + e.Message);
            }
            return result;
        }

        public int UpdateQRText(int lessonID, string qrText, DateTime? validTil)
        {
            queryStr = "Update Lesson Set LessonQRText = @qrText, QRValidUntil = @validTil " +
                "Where LessonID=@lessonID";

            try
            {
                using (sqlCon)
                {
                    sqlCon.Open();

                    using (sqlCmd = new MySqlCommand(queryStr, sqlCon))
                    {
                        sqlCmd.Parameters.AddWithValue("@lessonID", lessonID);
                        sqlCmd.Parameters.AddWithValue("@validTil", validTil);
                        sqlCmd.Parameters.AddWithValue("@qrText", qrText);

                        result = sqlCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogDAL.WriteLog("UpdateQRText() - " + e.Message);
            }
            return result;
        }

        public int ClearAllLessons()
        {
            queryStr = "Truncate Table Lesson";

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
                ErrorLogDAL.WriteLog("ClearAllLessons() - " + e.Message);
            }
            return result;
        }
    }
}