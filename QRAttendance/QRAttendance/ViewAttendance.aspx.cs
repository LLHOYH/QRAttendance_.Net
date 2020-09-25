using QRAttendance.DAL;
using QRAttendance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QRAttendance
{
    

    public partial class ViewAttendance : System.Web.UI.Page
    {
        Staff staff;
        ModuleDAL moduleDal;
        ModuleGroupDAL moduleGrpDal;
        Schedule schedule;
        Lesson lesson;
        List<Lesson> lessonList;
        List<ModuleGroup> moduleGrpList;
        List<Schedule> scheList;
        ScheduleDAL scheDal;
        DateTime lessonDate;
        LessonDAL lessonDal;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                staff = (Staff)Session["staffInfo"];
            }
        }

        protected void tb_LessonDate_TextChanged(object sender, EventArgs e)
        {
            lessonDate = new DateTime();
            if (DateTime.TryParse(tb_LessonDate.Text, out lessonDate))
            {

                //this has to be before loading other components!
                GetLessons(lessonDate);



                LoadModules();
                LoadModuleGroups();
                LoadLessons();
            }
        }


        //get all the lesson for that staff of the day and store into a session for later use.
        //need this step to be done before loading modules is because only lessons contains date and time.
        //we are only getting lessons for the day. 
        //so we get the lessons first, look for the distinct modules or modulegroups, and we then load those modules.
        public void GetLessons(DateTime lessonDate)
        {
            staff = (Staff)Session["staffInfo"];

            if (staff != null)
            {
                lessonDal = new LessonDAL();
                lessonList = lessonDal.GetAllLessonsOfStaffForTheDay(staff.StaffID, lessonDate);
                Session["AllLessons"] = lessonList;
                moduleGrpList = GetDistinctModules(lessonList);
                Session["moduleGrpList"] = moduleGrpList;
            }
        }

        public void LoadModules()
        {
            List<ModuleGroup> modules = new List<ModuleGroup>();
            modules = (List<ModuleGroup>)Session["moduleGrpList"];

            Dictionary<string, string> moduleDict = new Dictionary<string, string>();
            moduleDict.Add("0", "Select Module: ");

            foreach (var module in modules)
            {
                moduleDict.Add(module.ModuleCode, module.ModuleName);
            }

            ddl_Modules.DataSource = moduleDict;
            ddl_Modules.DataTextField = "Value";
            ddl_Modules.DataValueField = "Key";
            ddl_Modules.DataBind();
        }

        public void LoadModuleGroups()
        {
            List<ModuleGroup> moduleGroups = new List<ModuleGroup>();
            moduleGroups = (List<ModuleGroup>)Session["moduleGrpList"];

            Dictionary<int, string> mgDict = new Dictionary<int, string>();

            moduleGroups = moduleGroups.Where(mg => mg.ModuleCode.Equals(ddl_Modules.SelectedValue)).ToList();

            mgDict.Add(0, "Select Module Group: ");
            foreach (var mg in moduleGroups)
            {
                if (mg.ModuleGroupID != 0 && mg.GroupNumber != 0)
                    mgDict.Add(mg.ModuleGroupID, mg.GroupNumber.ToString());
            }
            ddl_ModuleGroups.DataSource = mgDict;
            ddl_ModuleGroups.DataTextField = "Value";
            ddl_ModuleGroups.DataValueField = "Key";
            ddl_ModuleGroups.DataBind();
        }

        public void LoadLessons() //load all lessons based on lesson date selected.
        {
            List<Lesson> allLessons = new List<Lesson>();
            Dictionary<int, string> lessonDict = new Dictionary<int, string>();

            allLessons = (List<Lesson>)Session["AllLessons"];
            if (!ddl_Modules.SelectedValue.Equals("0"))
            {
                allLessons = allLessons.Where(l => l.ModuleCode.Equals(ddl_Modules.SelectedValue)).ToList();
                if (!ddl_ModuleGroups.SelectedValue.Equals("0"))
                    allLessons = allLessons.Where(l => l.ModuleGroupID.ToString().Equals(ddl_ModuleGroups.SelectedValue)).ToList();
            }

            lessonDict.Add(0, "Select Lesson: ");

            foreach (var lesson in allLessons)
            {
                lessonDict.Add(lesson.LessonID, lesson.LessonDisplayText);
            }
            ddl_Lessons.DataSource = lessonDict;
            ddl_Lessons.DataTextField = "Value";
            ddl_Lessons.DataValueField = "Key";
            ddl_Lessons.DataBind();
        }

        public void LoadAttendanceList()
        {
            scheDal = new ScheduleDAL();
            scheList = new List<Schedule>();
            int lessonID = 0;
            if (int.TryParse(ddl_Lessons.SelectedValue, out lessonID))
            {
                scheList = scheDal.GetAttendanceByLesson(lessonID);

                gv_ViewAttendance.DataSource = scheList;
                gv_ViewAttendance.DataBind();

                if (!ddl_Lessons.SelectedItem.Text.Contains("FYPJ"))
                {
                    gv_ViewAttendance.Columns[4].Visible = false;
                    hf_LessonType.Value = "Lesson";
                }
                else
                {
                    gv_ViewAttendance.Columns[4].Visible = true;
                    hf_LessonType.Value = "FYPJ";
                }



                lnkBtn_Edit.Visible = true;
            }
        }




        protected void ddl_Modules_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadModuleGroups();
            LoadLessons();
        }

        protected void ddl_ModuleGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadLessons();
        }

        protected void btn_ViewAttendance_Click(object sender, EventArgs e)
        {
            LoadAttendanceList();
        }



        protected void lnkBtn_Edit_Click(object sender, EventArgs e)
        {

            ShowHideCBAttStatus(true);

            lnkBtn_Edit.Visible = false;
            panel_LnkBtn.Visible = true;
        }

        protected void lnkBtn_Update_Click(object sender, EventArgs e)
        {

            //first validate if inputs are in correct format
            foreach (GridViewRow row in gv_ViewAttendance.Rows)
            {
                CheckBox cb_AttendanceStatus = row.FindControl("cb_AttendanceStatus") as CheckBox;
                TextBox tb_ClockInTime = row.FindControl("tb_ClockInTime") as TextBox;
                TextBox tb_ClockOutTime = row.FindControl("tb_ClockOutTime") as TextBox; ;

                DateTime clockIn = new DateTime(), clockOut = new DateTime();

                if (cb_AttendanceStatus.Checked)  //if checked, clockin clockout time cannot be null
                {
                    if (string.IsNullOrEmpty(tb_ClockInTime.Text) || string.IsNullOrWhiteSpace(tb_ClockInTime.Text) ||
                        (hf_LessonType.Value.Equals("FYPJ") && (string.IsNullOrEmpty(tb_ClockOutTime.Text) || string.IsNullOrWhiteSpace(tb_ClockOutTime.Text)))){

                        AlertError("Time Field Has To Be Filled!");
                        return;
                    }
                }

                // clockin time must be valid.
                if (!string.IsNullOrEmpty(tb_ClockInTime.Text) && !string.IsNullOrWhiteSpace(tb_ClockInTime.Text))
                {
                    if (!DateTime.TryParse(tb_ClockInTime.Text, out clockIn))
                    {
                        AlertError("Invalid Time Format!");
                        return;
                    }
                }

                // if is fypj, clockout time must be valid.
                if (hf_LessonType.Value.Equals("FYPJ") && !string.IsNullOrEmpty(tb_ClockOutTime.Text) && !string.IsNullOrWhiteSpace(tb_ClockOutTime.Text))
                {
                    if (!DateTime.TryParse(tb_ClockOutTime.Text, out clockOut))
                    {
                        AlertError("Invalid Time Format!");
                        return;
                    }

                    int compareRes = TimeSpan.Compare(clockIn.TimeOfDay, clockOut.TimeOfDay); //return 1 if clockin is earlier, 0 is same time, 1 if clockout is earlier
                    if(compareRes != -1)
                    {
                        AlertError("Clock Out Time Should Be Later Than Clock In Time!");
                        return;
                    }
                }
            }



            //if valid, then proceed to update.
            int rowCount = 0;
            foreach (GridViewRow row in gv_ViewAttendance.Rows)
            {
                CheckBox cb_AttendanceStatus = row.FindControl("cb_AttendanceStatus") as CheckBox;
                Label lbl_AttendanceStatus = row.FindControl("lbl_AttendanceStatus") as Label;

                Label lbl_ClockInTime = row.FindControl("lbl_ClockInTime") as Label;
                TextBox tb_ClockInTime = row.FindControl("tb_ClockInTime") as TextBox;

                Label lbl_ClockOutTime = row.FindControl("lbl_ClockOutTime") as Label;
                TextBox tb_ClockOutTime = row.FindControl("tb_ClockOutTime") as TextBox; ;

                bool status = lbl_AttendanceStatus.Text.Equals("Present") ? true : false;

                if (cb_AttendanceStatus.Checked != status || !lbl_ClockInTime.Text.Equals(tb_ClockInTime.Text) || !lbl_ClockOutTime.Text.Equals(tb_ClockOutTime.Text)) ;  //if staff has made changes to attendance, result in original value different from final value, then update final value.
                {
                    scheDal = new ScheduleDAL();

                    int scheduleID = int.Parse(gv_ViewAttendance.DataKeys[rowCount].Value.ToString());

                    int statusInt = cb_AttendanceStatus.Checked ? 1 : 0;

                    int result = scheDal.UpdateAttendance(scheduleID, statusInt, tb_ClockInTime.Text, tb_ClockOutTime.Text);  //update attendance status to false, means absent

                    if (result > 0)
                        AlerSuccess("Update Done!");
                    else
                        AlertError("Update Failed!");

                }
                rowCount++;
            }

            lnkBtn_Edit.Visible = true;
            panel_LnkBtn.Visible = false;

            LoadAttendanceList();  //after update, reload gridview.
        }

        protected void lnkBtn_Cancel_Click(object sender, EventArgs e)
        {
            ShowHideCBAttStatus(false);

            lnkBtn_Edit.Visible = true;
            panel_LnkBtn.Visible = false;

        }




        public List<ModuleGroup> GetDistinctModules(List<Lesson> lessonList)
        {
            moduleGrpList = new List<ModuleGroup>();
            moduleGrpDal = new ModuleGroupDAL();
            foreach (Lesson les in lessonList)
            {
                moduleGrpList.Add(moduleGrpDal.GetModuleGroupByCodeAndID(les.ModuleCode, les.ModuleGroupID));
            }

            moduleGrpList = moduleGrpList.Distinct().ToList();
            return moduleGrpList;
        }

        public void ShowHideCBAttStatus(bool show)
        {
            int rowCount = 0;
            foreach (GridViewRow row in gv_ViewAttendance.Rows)
            {
                CheckBox cb_AttendanceStatus = row.FindControl("cb_AttendanceStatus") as CheckBox;
                Label lbl_AttendanceStatus = row.FindControl("lbl_AttendanceStatus") as Label;

                Label lbl_ClockInTime = row.FindControl("lbl_ClockInTime") as Label;
                TextBox tb_ClockInTime = row.FindControl("tb_ClockInTime") as TextBox;

                Label lbl_ClockOutTime = row.FindControl("lbl_ClockOutTime") as Label;
                TextBox tb_ClockOutTime = row.FindControl("tb_ClockOutTime") as TextBox; ;


                if (show)
                {

                    //hide display element and show inputs
                    cb_AttendanceStatus.Visible = true;
                    lbl_AttendanceStatus.Visible = false;

                    tb_ClockInTime.Visible = true;
                    lbl_ClockInTime.Visible = false;

                    tb_ClockOutTime.Visible = true;
                    lbl_ClockOutTime.Visible = false;


                    //set inputs value same to display element's value
                    if (lbl_AttendanceStatus.Text.Equals("Present"))
                        cb_AttendanceStatus.Checked = true;
                    else
                        cb_AttendanceStatus.Checked = false;

                    if (cb_AttendanceStatus.Checked) //if attendance is checked, set
                    {
                        schedule = new Schedule();
                        scheDal = new ScheduleDAL();

                        int scheduleID = int.Parse(gv_ViewAttendance.DataKeys[rowCount].Value.ToString());
                        schedule = scheDal.GetAttendanceByID(scheduleID);
                        if (schedule != null)
                        {
                            tb_ClockInTime.Text = schedule.ClockInTime;
                            tb_ClockOutTime.Text = schedule.ClockOutTime;
                        }
                    }
                }
                else
                {
                    cb_AttendanceStatus.Visible = false;
                    lbl_AttendanceStatus.Visible = true;

                    tb_ClockInTime.Visible = false;
                    lbl_ClockInTime.Visible = true;

                    tb_ClockOutTime.Visible = false;
                    lbl_ClockOutTime.Visible = true;
                }
                rowCount++;

            }
        }  //hides text "Present" and  "Absent", show checkbox instead. staff can change attendance through checkbox.

        public void AlerSuccess(string msg)
        {
            panel_Success.Visible = true;
            panel_Error.Visible = false;
            lbl_Success.Text = msg;
        }

        public void AlertError(string errorMsg)
        {
            panel_Success.Visible = false;
            panel_Error.Visible = true;
            lbl_Error.Text = errorMsg;
        }

    }
}