using System;

namespace QRAttendance.DAL
{
    public class Login
    {

        public static int valLogin(string str_StaffID, string str_Password, out bool error)
        {
            error = false;
            int int_LoginResult = -1;

                var ws = (dynamic)null;

                //if (Global.getMode() == "DEBUG")
                //    int_LoginResult = 0;
                //else
                //{
                    //if (Global.getServer() == "UAT")
                    //    ws = new StaffLoginUATWS.StaffLoginClient();
                    //else
                        ws = new StaffLoginWS.StaffLoginClient();

            try
            {
                    using (ws)
                        int_LoginResult = ws.valLogin(str_StaffID, str_Password);
            }
            catch(System.ServiceModel.Security.MessageSecurityException e)
            {
                error = true;
                return -1;
            }

                //}

            return int_LoginResult;
        }
    }
}
