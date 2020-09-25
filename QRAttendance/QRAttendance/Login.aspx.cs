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
    public partial class Login : System.Web.UI.Page
    {
        StaffDAL staffDal;
        Staff staff;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["staffInfo"] = new Staff()
            //{
            //    StaffID = "B66666Z",
            //    StaffName = "Super",
            //    StaffType="Super"
            //};
            //Response.Redirect("ViewAttendance.aspx");
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string str_TempStaffID = "B66666Z";
            string str_TempPassword = "P@ssw0rd!@#";


            string str_StaffID = Server.HtmlEncode(tb_StaffID.Text.ToUpper().Trim());
            string str_Password = Server.HtmlEncode(tb_Password.Text);


            int int_LoginResult = -1;

            if (String.IsNullOrWhiteSpace(str_StaffID) || String.IsNullOrWhiteSpace(str_Password))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please enter your user ID and password.')", true);
                return;
            }

            if (String.Equals(str_StaffID, str_TempStaffID)) //for debugging login
            {
                if (!String.Equals(str_Password, str_TempPassword))
                    return;

                staff = new Staff()
                {
                    StaffID = str_TempStaffID,
                    StaffName = "Super"
                };

                Session["staffInfo"] = staff;
                Response.Redirect("GenerateQR.aspx");
            }

            bool error = false;
            int_LoginResult = DAL.Login.valLogin(str_StaffID, str_Password, out error);

            if (int_LoginResult != 0)
            {
                if (!error)
                {
                    lb_Error.Text = "Login unsuccessful. Please check your login credentials.";
                    lb_Error.Visible = true;
                }
                else
                {
                    lb_Error.Text = "Login unsuccessful. Please make sure you are in a safe environment";
                    lb_Error.Visible = true;
                }
                return;
            }

            staff = new StaffDAL().GetStaffByID(str_StaffID);

            if (staff == null)
            {
                lb_Error.Text = "Staff is not registered on this system.";
                lb_Error.Visible = true;
                return;
            }

            Session["staffInfo"] = staff;
            Response.Redirect("GenerateQR.aspx");
        }
    }
}