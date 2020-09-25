using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRAttendance.Models
{
    [Serializable]
    public class Lesson
    {
        private int lessonID, lessonDuration;
        private string moduleCode, staffID, lessonType, lessonVenue,  lessonDay, lessonTime, lessonQRText;
        private DateTime lessonDate;
        private bool lessonToModuleGroup;  //this attribute specifies whether this lesson is held for a modulegroup or for the whole module
                                            //(e.g. a lecture or a tutorial)(database store bool as numbers, 0 = false, non 0 = true;)
        private int moduleGroupID; //if lessonToModuleGroup is true, this value will be used to set up the lesson, else will use moduleCode;


        public int LessonID { get => lessonID; set => lessonID = value; }
        public string ModuleCode { get => moduleCode; set => moduleCode = value; }
        public string StaffID { get => staffID; set => staffID = value; }
        public string LessonType { get => lessonType; set => lessonType = value; }
        public string LessonVenue { get => lessonVenue; set => lessonVenue = value; }
        public string LessonTime { get => lessonTime; set => lessonTime = value; }
        public DateTime LessonDate { get => lessonDate; set => lessonDate = value; }
        public string LessonDay { get => lessonDay; set => lessonDay = value; }
        public int LessonDuration { get => lessonDuration; set => lessonDuration = value; }
        public bool LessonToModuleGroup { get => lessonToModuleGroup; set => lessonToModuleGroup = value; }
        public int ModuleGroupID { get => moduleGroupID; set => moduleGroupID = value; }
        public string LessonQRText { get => lessonQRText; set => lessonQRText = value; }



        //not related
        private string lessonDisplayText;
        public string LessonDisplayText { get => lessonDisplayText; set => lessonDisplayText = value; }
    }
}