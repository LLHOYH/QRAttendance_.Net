using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace QRAttendance.DAL
{
    public class ExcelDAL
    {
        DataTable dt;
        OleDbConnection excelCon;
        OleDbCommand excelCmd;
        OleDbDataAdapter excelAdapter;
        string queryStr;

        public OleDbConnection ExcelConnection
        {
            get
            {
                return new OleDbConnection("Provider = Microsoft.ACE.OLEDB.12.0; Data Source=" + ExcelPath + "; Extended Properties=Excel 8.0; Persist Security Info=false");
            }
        }

        private string excelPath;

        public string ExcelPath { get => excelPath; set => excelPath = value; }

        public DataTable GetDataFromExcel(IXLWorksheet worksheet)
        {
            bool firstRow = true;
            dt = new DataTable();
            foreach (IXLRow row in worksheet.Rows())
            {
                if (firstRow)
                {
                    foreach (IXLCell cell in row.Cells())
                    {
                        dt.Columns.Add(cell.Value.ToString());
                    }
                    firstRow = false;
                }
                else
                {
                    dt.Rows.Add();
                    int i = 0;
                    foreach (IXLCell cell in row.Cells())
                    {
                        dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                        i++;
                    }
                }
            }

            return dt;
        }
    }
}