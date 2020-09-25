using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace QRAttendance.DAL
{
    public static class SqlConfiguration
    {

        public static MySqlConnection GetSqlConnection
        {
            get
            {
                return new MySqlConnection(ConfigurationManager.ConnectionStrings["QRAttendance"].ToString());
            }
        }

    }

}