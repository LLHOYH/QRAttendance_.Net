using QRAttendance.DAL;
using QRAttendance.Models;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Web.Services;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace QRAttendance
{
    public partial class GenerateQR : System.Web.UI.Page
    {
        Staff staff;
        ModuleDAL moduleDal;
        ModuleGroupDAL moduleGrpDal;
        Lesson lesson;
        LessonDAL lessonDal;
        List<Lesson> lessonList;
        List<ModuleGroup> moduleGrpList;
        List<Schedule> scheList;
        ScheduleDAL scheDal;

        int countDownTotal = 30;
        int attendanceRefresh = 3;
        int countDown = 0;
        int result = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                countDown = countDownTotal;
                hf_QRInProgress.Value = "FALSE";
                staff = (Staff)Session["staffInfo"];


                //these two have to be before loading other components!
                LoadMostLikelyLessonToConduct();
                GetLessonsForTheDay(); 




                LoadModules();
                LoadModuleGroups();
                LoadLessons();
            }
        }



        //get that specific lesson that the staff is most likely to be conducting at the point of time and store into a session for later use.
        public void LoadMostLikelyLessonToConduct()
        {
            lessonDal = new LessonDAL();
            lesson = new Lesson();

            lesson = lessonDal.GetLessonWhenLogin(staff.StaffID);
            Session["lessonOnLogin"] = lesson;
        } 



        //get all the lesson for that staff of the day and store into a session for later use.
        //need this step to be done before loading modules is because only lessons contains date and time.
        //we are only getting lessons for the day. 
        //so we get the lessons first, look for the distinct modules or modulegroups, and we then load those modules.
        public void GetLessonsForTheDay()  
        {
            if (staff != null)
            {
                lessonDal = new LessonDAL();
                lessonList = lessonDal.GetAllLessonsOfStaffForTheDay(staff.StaffID, DateTime.Now.Date);
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


            //check if there's a most likely to conduct lesson
            //if yes, auto select the module of the lesson
            if (!IsPostBack)
            {
                lesson = new Lesson();
                lesson = (Lesson)Session["lessonOnLogin"];
                if (lesson != null)
                    ddl_Modules.SelectedValue = lesson.ModuleCode;
            }
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


            //check if there's a most likely to conduct lesson
            //if yes, auto select the modulegroup of the lesson
            if (!IsPostBack)
            {
                lesson = new Lesson();
                lesson = (Lesson)Session["lessonOnLogin"];

                if (lesson != null)
                    ddl_ModuleGroups.SelectedValue = lesson.ModuleGroupID.ToString();
            }

        }

        public void LoadLessons()
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




            //check if there's a most likely to conduct lesson
            //if yes, auto select the lesson
            if (!IsPostBack)
            {
                lesson = new Lesson();
                lesson = (Lesson)Session["lessonOnLogin"];

                if (lesson != null)
                    ddl_Lessons.SelectedValue = lesson.LessonID.ToString();
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






        #region QRCode and CountDown Region
        //this region....
        //used two update panels with a timer that triggers time_Countdown_Tick function every 1 second.
        //one update panel takes care of the change of QRcode text every 'countDownTotal' seconds, the other panel takes care of refreshing attendance list every 'attendanceRefresh' seconds.
        //The total countdown duration before switching to a new QR code text is set above using a global variable - countDownTotal
        //The total countdown duration before refreshing the attendance list is set above using global variable - attendanceRefresh
        //For every second tick, variable 'countDown' will +1; once it reach countDownTotal, QR text change; once it reach attendanceRefresh, attendance list refresh.

        //Note:
        //FYPJ lesson and Non-FYPJ lesson makes a huge difference.
        //only Non-FYPJ lessons will have it's QRCode text switched every 'countDownTotal' seconds and have the students' attendance list shown.
        //FYPJ lessons will only have a static QRcode text.


        protected void btn_GenerateQR_Click(object sender, EventArgs e)
        {
            if (!hf_QRInProgress.Value.Equals("TRUE"))  //if not enabled, set to enabled
                StartAttendanceTaking();
            else
                StopAttendanceTaking();
        }

        protected void timer_CountDown_Tick(object sender, EventArgs e)
        {
            SetCountDown();
        }  //timer ticks every second. Set at frontend.

        protected void cb_AttendanceStatus_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            ListViewItem item = (ListViewItem)cb.NamingContainer;
            ListViewDataItem dataItem = (ListViewDataItem)item;

            int scheduleID = int.Parse(lv_StudentAttendance.DataKeys[dataItem.DisplayIndex].Value.ToString());

            scheDal = new ScheduleDAL();
            int statusInt = cb.Checked ? 1 : 0;
            result = scheDal.UpdateAttendanceOnlyStatus(scheduleID, statusInt);  //update status to false, means absent

        }  //on attendance list (student attendance being changed manually by staff)

        protected void lv_StudentAttendance_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            (lv_StudentAttendance.FindControl("DataPager1") as DataPager).SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            ShowAttendanceList();
        }

        public void StartAttendanceTaking()
        {
            hf_LessonID.Value = ddl_Lessons.SelectedValue;

            lbl_LessonID.Text = ddl_Lessons.SelectedValue;
            up_QR.Visible = true;
            btn_GenerateQR.Text = "Stop Attendance Taking";
            hf_QRInProgress.Value = "TRUE";
            SetIsFYPJ();

            if (!hf_IsFYPJ.Value.Equals("TRUE"))
            {
                ViewState["countDown"] = countDownTotal;
                timer_CountDown.Enabled = true;
                up_Attendance.Visible = true;
                divFYPJ.Visible = false;
                lbl_CountDown.Visible = true;
            }
            else
            {
                SetQRCode();
            }

        }

        public void StopAttendanceTaking()
        {
            up_QR.Visible = false;
            timer_CountDown.Enabled = false;
            up_Attendance.Visible = false;
            lbl_CountDown.Visible = false;
            divFYPJ.Visible = true;
            hf_QRInProgress.Value = "FALSE";
            btn_GenerateQR.Text = "Start Attendance Taking";
            lbl_LessonID.Text = null;

            int lessonID = 0;
            if (int.TryParse(hf_LessonID.Value, out lessonID) && lessonID != 0)
            {
                lessonDal = new LessonDAL();
                lessonDal.UpdateQRText(lessonID, null, null);
            }
        }

        public void SetQRCode()
        {
            int lessonID = 0;

            lessonDal = new LessonDAL();
            lesson = new Lesson();
            if (int.TryParse(hf_LessonID.Value, out lessonID) && lessonID != 0)
            {
                lesson = lessonDal.GetLessonByID(lessonID);
                if (lesson != null)
                {
                    string guid = Guid.NewGuid().ToString();
                    string encodedText = lesson.ModuleCode + "_" + lesson.LessonDate.ToString("dd-MM-yyyy") +"_" + guid;

                    string qrJsonText = "{\"Source\":\"NYP_LESSONS\", \"QRText\":\"" + encodedText + "\"}";

                    result = lessonDal.UpdateQRText(lessonID, encodedText, DateTime.Now.AddSeconds(countDownTotal));
                    if (result != 0)
                    {
                        QRCodeGenerator qrGenerator = new QRCodeGenerator();
                        QRCodeData qrData = qrGenerator.CreateQrCode(qrJsonText, QRCodeGenerator.ECCLevel.Q);
                        QRCode qrCode = new QRCode(qrData);
                        System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
                        imgBarCode.Width = 500;
                        imgBarCode.Height = 500;

                        using (Bitmap bitMap = qrCode.GetGraphic(20))
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                byte[] byteImage = ms.ToArray();
                                img_QRCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                                return;
                            }
                        }
                    }
                    else
                    {
                        StopAttendanceTaking();
                        SetAlert(false, "Something Went Wrong!");
                    }
                }
                else
                {
                    StopAttendanceTaking();
                    SetAlert(false, "Something Went Wrong!");
                }
            }
            else
            {
                StopAttendanceTaking();
                SetAlert(false, "Please Make a Valid Selection of Lesson Before Start Attendance Taking!");
            }
        }

        public void SetCountDown()
        {
            if (ViewState["countDown"] == null)
                ViewState["countDown"] = countDownTotal;

            countDown = (int)ViewState["countDown"];

            if (countDown == countDownTotal)
                SetQRCode();

            if (countDown % attendanceRefresh == 0)
                ShowAttendanceList();


            countDown--;
            lbl_CountDown.Text = countDown.ToString() + " second(s) to generate next QR code.";
            if (countDown == 0)
                countDown = countDownTotal;

            ViewState["countDown"] = countDown;

        }

        #endregion




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

        public void ShowAttendanceList()
        {
            scheDal = new ScheduleDAL();
            scheList = new List<Schedule>();
            int lessonID = 0;
            if (int.TryParse(hf_LessonID.Value, out lessonID))
            {
                scheList = scheDal.GetAttendanceByLesson(lessonID);
                lv_StudentAttendance.DataSource = scheList;
                lv_StudentAttendance.DataBind();
            }
        }

        public void HideAttendanceList()
        {
            scheList = new List<Schedule>();
            lv_StudentAttendance.DataSource = scheList;
            lv_StudentAttendance.DataBind();
        }

        public void SetIsFYPJ()
        {
            try
            {
                if (ddl_Lessons.SelectedItem.Text.Split(',')[0].Contains("FYPJ"))
                {
                    hf_IsFYPJ.Value = "TRUE";
                    lbl_FYPJ.Visible = true;
                }
                else
                {
                    lbl_FYPJ.Visible = false;
                    hf_IsFYPJ.Value = "FALSE";
                }
            }
            catch (NullReferenceException e) { };

        }

        public void SetAlert(bool success, string alertMsg)
        {
            if (success)
            {
                panel_Error.Visible = false;
                panel_Success.Visible = true;
                lbl_Success.Text = alertMsg;
            }
            else
            {
                panel_Success.Visible = false;
                panel_Error.Visible = true;
                lbl_Error.Text = alertMsg;
            }
        }

    }
}