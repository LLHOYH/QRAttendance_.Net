using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRAttendance.Models
{
    public class Schedule
    {
        private int scheduleID, lessonID;
        private string adminNumber;
        private string clockInTime, clockOutTime;
        private bool attendanceStatus;

        private string studentName;

        public int ScheduleID { get => scheduleID; set => scheduleID = value; }
        public int LessonID { get => lessonID; set => lessonID = value; }
        public string AdminNumber { get => adminNumber; set => adminNumber = value; }
        public string ClockInTime { get => clockInTime; set => clockInTime = value; }
        public string ClockOutTime { get => clockOutTime; set => clockOutTime = value; }
        public string StudentName { get => studentName; set => studentName = value; }
        public bool AttendanceStatus { get => attendanceStatus; set => attendanceStatus = value; }
    }
}