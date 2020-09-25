using MySql.Data.MySqlClient;
using QRAttendance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRAttendance.DAL
{
    public class DataImportDAL
    {
        DataImport dataImport;
        List<DataImport> dataImportList;
        string queryStr;
        int result = 0;
        MySqlConnection sqlCon = SqlConfiguration.GetSqlConnection;
        MySqlCommand sqlCmd;
        MySqlDataReader reader;
        string error = null;

        public DataImport GetLastDataImport()
        {
            queryStr = "select * from DataImport Order by ImportID Desc Limit 0, 1";

            try
            {
                using (sqlCon)
                {
                    sqlCon.Open();

                    using (sqlCmd = new MySqlCommand(queryStr, sqlCon))
                    {

                        using (reader = sqlCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                dataImport = new DataImport();

                                dataImport.ImportID = int.Parse(reader["ImportID"].ToString());
                                dataImport.ImportedDate = Convert.ToDateTime(reader["ImportedDate"]);
                                dataImport.ImportedTime = reader["ImportedTime"].ToString();
                                dataImport.ImportedBy = reader["ImportedBy"].ToString();
                                dataImport.ImportForAcadYr = reader["ImportForAcadYr"].ToString();
                                dataImport.ImportForSemester = reader["ImportForSemester"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogDAL.WriteLog("GetLastDataImport() - " + e.Message);
            }
            return dataImport;
        }

        public List<DataImport> AllDataImports()
        {
            queryStr = "Select * From DataImport";
            dataImportList = new List<DataImport>();
            try
            {
                using (sqlCon)
                {
                    sqlCon.Open();

                    using (sqlCmd = new MySqlCommand(queryStr, sqlCon))
                    {
                        using (reader = sqlCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                dataImport = new DataImport();

                                dataImport.ImportID = int.Parse(reader["ImportID"].ToString());
                                dataImport.ImportedDate = Convert.ToDateTime(reader["ImportedDate"]);
                                dataImport.ImportedTime = reader["ImportedTime"].ToString();
                                dataImport.ImportedBy = reader["ImportedBy"].ToString();
                                dataImport.ImportForAcadYr = reader["ImportForAcadYr"].ToString();
                                dataImport.ImportForSemester = reader["ImportForSemester"].ToString();
                                dataImportList.Add(dataImport);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogDAL.WriteLog("AllDataImports() - " + e.Message);
            }
            return dataImportList;
        }

        public int InsertCurrentImportInfo(DateTime importedDate, string importedTime, string importedBy, string importForAcadYr, string importForSemeseter)
        {
            queryStr = "Insert into DataImport " +
                "(ImportedDate, ImportedTime, ImportedBy, ImportForAcadYr, ImportForSemester) " +
                "Values " +
                "(@importedDate, @importedTime, @importedBy, @importForAcadYr, @importForSemester)";
            try
            {
                using (sqlCon)
                {
                    sqlCon.Open();

                    using (sqlCmd = new MySqlCommand(queryStr, sqlCon))
                    {
                        sqlCmd.Parameters.AddWithValue("@importedDate", importedDate);
                        sqlCmd.Parameters.AddWithValue("@importedTime", importedTime);
                        sqlCmd.Parameters.AddWithValue("@importedBy", importedBy);
                        sqlCmd.Parameters.AddWithValue("@importForAcadYr", importForAcadYr);
                        sqlCmd.Parameters.AddWithValue("@importForSemester", importForSemeseter);

                        result = sqlCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogDAL.WriteLog("InsertCurrentImportInfo() - " + e.Message);
            }

            return result;
        }

    }
}