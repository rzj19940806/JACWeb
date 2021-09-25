using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace HfutIE.Utilities
{
    /// <summary>
    /// 导入Excel帮助类
    /// </summary>
    public class ImportExcel
    {
        /// <summary>
        /// Excel检查版本
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static string ConnectionString(string fileName)
        {
            bool isExcel2003 = fileName.EndsWith(".xls");//加x后缀的用不了
            string connectionString = string.Format(
                isExcel2003
                    ? "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=Excel 8.0;"
                    : "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES\"",
                fileName);
            return connectionString;
        }
        /// <summary>
        /// Excel导入数据源
        /// </summary>
        /// <param name="sheet">sheet</param>
        /// <param name="filename">文件路径</param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable( string filename)
        {
            OleDbConnection myConn = new OleDbConnection(ConnectionString(filename));
            try
            {
                myConn.Open();
                //返回Excel的架构，包括各个sheet表的名称,类型，创建时间和修改时间等
                DataTable dtSheetName = myConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });
                string sheetName = dtSheetName.Rows[0]["TABLE_NAME"].ToString();
                DataSet ds;
                string strCom = string.Format(" SELECT * FROM [{0}]", sheetName);
                OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, myConn);
                ds = new DataSet();
                myCommand.Fill(ds);
                myConn.Close();
                return ds.Tables[0];
            }
            catch (Exception)
            {
                myConn.Close();
                myConn.Dispose();
                throw;
            }
        }


        /// <summary>
        /// 获取sheet名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string[] GetSheetsName(string fileName)
        {
            OleDbConnection oleconn = new OleDbConnection(ConnectionString(fileName));
            oleconn.Open();
            //返回Excel的架构，包括各个sheet表的名称,类型，创建时间和修改时间等
            DataTable dtSheetName = oleconn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });
            //包含excel中表名的字符串数组
            string[] strTableNames = new string[dtSheetName.Rows.Count];

            for (int k = 0; k < dtSheetName.Rows.Count; k++)
            {

                strTableNames[k] = dtSheetName.Rows[k]["TABLE_NAME"].ToString();
            }
            return strTableNames;
        }

    }
}
