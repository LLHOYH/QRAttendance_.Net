using QRAttendance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QRAttendance
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        Staff staff;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["staffInfo"] == null)
                Response.Redirect("Login.aspx");

            if (!IsPostBack)
            {
                staff = (Staff)Session["staffInfo"];
                lb_StaffName.Text = staff.StaffName;

                SetSensitivePageVisibility(staff);
            }
        }


        //sensitive page includes dataimport page and settings page
        public void SetSensitivePageVisibility(Staff staff)
        {
            lkbtn_GoToImportDataPage.Visible = false;  
            lkbtn_GoToSettingsPage.Visible = false;

            if (staff != null)
            {
                if (staff.StaffType.Equals("Super"))
                {
                    lkbtn_GoToImportDataPage.Visible = true;
                    lkbtn_GoToSettingsPage.Visible = true;
                }
            }
        }

        protected void lkbtn_GoToImportDataPage_Click(object sender, EventArgs e)
        {
            Response.Redirect("ImportData.aspx");
        }

        protected void lkbtn_GoToSettingsPage_Click(object sender, EventArgs e)
        {
            Response.Redirect("Settings.aspx");
        }

        protected void btn_Logout_Click(object sender, EventArgs e)
        {
            Session["staffInfo"] = null;
            Response.Redirect("login.aspx");
        }
    }
}