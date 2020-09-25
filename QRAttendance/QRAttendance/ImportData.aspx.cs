using ClosedXML.Excel;
using QRAttendance.DAL;
using QRAttendance.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace QRAttendance
{
    public partial class ImportData : System.Web.UI.Page
    {
        DataImportDAL importDal;
        DataImport dataImport;
        ExcelDAL excelDal;
        Staff staff;
        int result = 0;
        string[] allowedFile = { ".xls", ".xlsx" };
        string[] tableNames = { "Student", "Staff", "Module", "ModuleGroup", "Lesson", "Schedule" };

        bool operateStatus = false;

        StudentDAL studentDal;
        StaffDAL staffDal;
        ModuleDAL moduleDal;
        ModuleGroupDAL moduleGrpDal;
        LessonDAL lessonDal;
        ScheduleDAL scheduleDal;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                staff = (Staff)Session["staffInfo"];


                //prevent non super staff enter this page
                if (!staff.StaffType.Equals("Super"))
                {
                    Response.Write("<script type='text/javascript'>");
                    Response.Write("alert('You Are Not Authorized To Access This Page');");
                    Response.Write("document.location.href='../GenerateQR.aspx';");
                    Response.Write("</script>");
                }
                LoadLastImportInfo();
            }
        }

        protected void btn_ImportData_Click(object sender, EventArgs e)
        {
            if (fu_ExcelUpload.HasFile)
            {
                if (allowedFile.Contains(Path.GetExtension(fu_ExcelUpload.FileName)))
                {
                    using (XLWorkbook workbook = new XLWorkbook(fu_ExcelUpload.PostedFile.InputStream))
                    {
                        operateStatus = WorkBookHandler(workbook);
                        if (operateStatus)
                            operateStatus = CreateNewImportInfo();

                        if (operateStatus)
                            SetAlert(true, "Excel Data Uploaded Successfully.");
                        else
                            SetAlert(false, "Failed To Upload Excel.");
                    }
                }
                else
                {
                    SetAlert(false, "Please Upload Valid Excel File!");
                }
            }
            else
            {
                SetAlert(false, "No File Detected!");
            }
        }

        public void LoadLastImportInfo()
        {
            importDal = new DataImportDAL();
            StaffDAL staffDal = new StaffDAL();
            dataImport = importDal.GetLastDataImport();

            if (dataImport != null)
            {
                lbl_LastImportDate.Text = dataImport.ImportedDate.ToString("dd-MMM-yy") + "   " + dataImport.ImportedTime.Substring(0, 5);
                lbl_ImportForAcadYr.Text = dataImport.ImportForAcadYr;
                lbl_ImportForSemester.Text = dataImport.ImportForSemester;
                lbl_ImportedBy.Text = staffDal.GetStaffByID(dataImport.ImportedBy).StaffName;
            }
        }

        public bool CreateNewImportInfo()
        {
            importDal = new DataImportDAL();
            staff = (Staff)Session["staffInfo"];
            if (staff != null)
                result = importDal.InsertCurrentImportInfo(DateTime.Now.Date, DateTime.Now.TimeOfDay.ToString(), staff.StaffID, tb_ImportForAcadYr.Text, ddl_ImportForSemester.SelectedValue);

            return result > 0 ? true : false;
        }

        public bool WorkBookHandler(XLWorkbook workbook)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            excelDal = new ExcelDAL();

            for (int i = 0; i < tableNames.Count(); i++)
            {
                dt = excelDal.GetDataFromExcel(workbook.Worksheet(tableNames[i]));
                if (dt.Rows.Count > 1)
                {
                    switch (tableNames[i])
                    {
                        case "Student":
                            studentDal = new StudentDAL();
                            studentDal.ClearAllStudents();  //no need to get return type as Truncate always return 0 in Mysql
                            result = studentDal.InsertAllStudents(dt);
                            if (result < 1)
                                return false;
                            break;
                        case "Staff":
                            staffDal = new StaffDAL();


                            //staffDal.ClearStaffsOtherThenSuper();
                            //uses Delete here instead of truncate;  
                            //this will cause error as super staff will not be deleted, but will import duplicate staff in. cause duplicate PK error


                            staffDal.ClearAllStaff();
                            result = staffDal.InsertAllStaffs(dt);
                            if (result < 1)
                                return false;
                            break;
                        case "Module":
                            moduleDal = new ModuleDAL();
                            moduleDal.ClearAllModules(); //no need to get return type as Truncate always return 0 in Mysql
                            result = moduleDal.InsertAllModules(dt);
                            if (result < 1)
                                return false;
                            break;
                        case "ModuleGroup":
                            moduleGrpDal = new ModuleGroupDAL();
                            moduleGrpDal.ClearAllModuleGroups(); //no need to get return type as Truncate always return 0 in Mysql
                            result = moduleGrpDal.InsertAllModuleGroups(dt);
                            if (result < 1)
                                return false;
                            break;
                        case "Lesson":
                            lessonDal = new LessonDAL();
                            lessonDal.ClearAllLessons(); //no need to get return type as Truncate always return 0 in Mysql
                            result = lessonDal.InsertAllLessons(dt);
                            if (result < 1)
                                return false;
                            break;
                        case "Schedule":
                            scheduleDal = new ScheduleDAL();
                            scheduleDal.ClearAllSchedule(); //no need to get return type as Truncate always return 0 in Mysql
                            result = scheduleDal.InsertAllSchedule(dt);
                            if (result < 1)
                                return false;
                            break;
                    }
                }
                else
                    return false;
            }

            if (result > 0)
                operateStatus = true;

            return operateStatus;
        }

        protected void lknBtn_Export_Click(object sender, EventArgs e)
        {
            DataTable dt;
            bool exportStatus = false;

            using (XLWorkbook wb = new XLWorkbook())
            {

                studentDal = new StudentDAL();
                dt = studentDal.GetAllStudentsInDT();
                    wb.Worksheets.Add(dt,"Student");

                staffDal = new StaffDAL();
                dt = staffDal.GetAllStaffsInDT();
                if (dt != null)
                    wb.Worksheets.Add(dt,"Staff");

                moduleDal = new ModuleDAL();
                dt = moduleDal.GetAllModulesInDT();
                if (dt != null)
                    wb.Worksheets.Add(dt,"Module");

                moduleGrpDal = new ModuleGroupDAL();
                dt = moduleGrpDal.GetAllModuleGrpsInDT();
                if (dt != null)
                    wb.Worksheets.Add(dt,"ModuleGroup");

                lessonDal = new LessonDAL();
                dt = lessonDal.GetAllLessonsInDT();
                if (dt != null)
                    wb.Worksheets.Add(dt,"Lesson");

                scheduleDal = new ScheduleDAL();
                dt = scheduleDal.GetAllScheduleInDT();
                if (dt != null)
                    wb.Worksheets.Add(dt,"Schedule");

                foreach(IXLWorksheet ws in wb.Worksheets)
                {
                    ws.Columns("A", "Z").AdjustToContents();
                }

                string fileName = "NYP_Attendance_" + DateTime.Now.ToString("dd_MMM_yyyy")+".xlsx";

                // Prepare the response
                HttpResponse httpResponse = Response;
                httpResponse.Clear();
                httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                httpResponse.AddHeader("content-disposition", "attachment;filename="+fileName);

                // Flush the workbook to the Response.OutputStream
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    wb.SaveAs(memoryStream);
                    memoryStream.WriteTo(httpResponse.OutputStream);
                    memoryStream.Close();
                }

                httpResponse.End();
            }

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