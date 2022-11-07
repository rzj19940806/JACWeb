using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace HfutIE.Utilities
{
    /// <summary>
    /// 导出Excel帮助类
    /// </summary>
    public class DeriveExcel
    {

        public static string cookieData = "";
        public static string cookieColumn = "";
        public static string cookieFooter = "";
        public static string cookieName = "";
        /// <summary>
        /// IList导出Excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">集合</param>
        /// <param name="DataColumn">字段</param>
        /// <param name="fileName"></param>
        public static void ListToExcel<T>(IList list, string[] DataColumn, string fileName)
        {
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            HttpContext.Current.Response.Charset = "Utf-8";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName + ".xls", System.Text.Encoding.UTF8));
            StringBuilder sbHtml = new StringBuilder();
            sbHtml.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
            sbHtml.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
            //写出列名
            sbHtml.AppendLine("<tr style=\"background-color: #FFE88C;font-weight: bold; white-space: nowrap;\">");
            foreach (string item in DataColumn)
            {
                string[] stritem = item.Split(':');
                sbHtml.AppendLine("<td>" + stritem[0] + "</td>");
            }
            sbHtml.AppendLine("</tr>");
            //写数据
            foreach (T entity in list)
            {
                Hashtable ht = HashtableHelper.GetModelToHashtable<T>(entity);
                sbHtml.Append("<tr>");
                foreach (string item in DataColumn)
                {
                    string[] stritem = item.Split(':');
                    sbHtml.Append("<td>").Append(ht[stritem[1]]).Append("</td>");
                }
                sbHtml.AppendLine("</tr>");
            }
            sbHtml.AppendLine("</table>");
            HttpContext.Current.Response.Write(sbHtml.ToString());
            HttpContext.Current.Response.End();
        }
        /// <summary>
        /// DataTable导出Excel
        /// </summary>
        /// <param name="data">集合</param>
        /// <param name="DataColumn">字段</param>
        /// <param name="fileName">文件名称</param>
        public static void DataTableToExcel(DataTable data, string[] DataColumn, string fileName)
        {
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            HttpContext.Current.Response.Charset = "Utf-8";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName + ".xls", System.Text.Encoding.UTF8));
            StringBuilder sbHtml = new StringBuilder();
            sbHtml.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
            sbHtml.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
            //写出列名
            sbHtml.AppendLine("<tr style=\"background-color: #FFE88C;font-weight: bold; white-space: nowrap;\">");
            foreach (string item in DataColumn)
            {
                sbHtml.AppendLine("<td>" + item + "</td>");
            }
            sbHtml.AppendLine("</tr>");
            //写数据
            foreach (DataRow row in data.Rows)
            {
                sbHtml.Append("<tr>");
                foreach (string item in DataColumn)
                {
                    sbHtml.Append("<td>").Append(row[item]).Append("</td>");
                }
                sbHtml.AppendLine("</tr>");
            }
            sbHtml.AppendLine("</table>");
            HttpContext.Current.Response.Write(sbHtml.ToString());
            HttpContext.Current.Response.End();
        }
        /// <summary>
        /// Table标签导出Excel
        /// </summary>
        /// <param name="sbHtml">html标签</param>
        /// <param name="fileName">文件名</param>
        public static void HtmlToExcel(StringBuilder sbHtml, string fileName)
        {
            if (sbHtml.Length > 0)
            {
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
                HttpContext.Current.Response.Charset = "Utf-8";
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName + ".xls", System.Text.Encoding.UTF8));
                HttpContext.Current.Response.Write(sbHtml.ToString());
                HttpContext.Current.Response.End();
            }
        }
        /// <summary>
        /// JqGrid导出Excel
        /// </summary>
        /// <param name="JsonColumn">表头</param>
        /// <param name="JsonData">数据</param>
        /// <param name="ExportField">导出字段</param>
        /// <param name="fileName">文件名</param>
        public static void JqGridToExcel(string JsonColumn, string JsonData, string ExportField, string fileName)
        {
            List<JqGridColumn> ListColumn = JsonConvert.DeserializeObject<List<JqGridColumn>>(JsonColumn);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            HttpContext.Current.Response.Charset = "Utf-8";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName + ".xls", System.Text.Encoding.UTF8));
            StringBuilder sbHtml = new StringBuilder();
            sbHtml.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
            sbHtml.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
            //写出列名
            sbHtml.AppendLine("<tr style=\"background-color: #FFE88C;font-weight: bold; white-space: nowrap;\">");
            string[] FieldInfo = ExportField.Split(',');
            foreach (string item in FieldInfo)
            {
                IList list = ListColumn.FindAll(t => t.name == item);
                foreach (JqGridColumn jqgridcolumn in list)
                {
                    if (jqgridcolumn.hidden.ToLower() == "false" && jqgridcolumn.label != null)
                    {
                        sbHtml.AppendLine("<td style=\"width:" + jqgridcolumn.width + "px;text-align:" + jqgridcolumn.align + "\">" + jqgridcolumn.label + "</td>");
                    }
                }
            }
            sbHtml.AppendLine("</tr>");
            //写数据

            DataTable dt = JsonData.JsonToDataTable();
            foreach (DataRow row in dt.Rows)
            {
                sbHtml.Append("<tr>");
                foreach (string item in FieldInfo)
                {
                    IList list = ListColumn.FindAll(t => t.name == item);
                    foreach (JqGridColumn jqgridcolumn in list)
                    {
                        if (jqgridcolumn.hidden.ToLower() == "false" && jqgridcolumn.label != null)
                        {
                            object text = row[jqgridcolumn.name];
                            sbHtml.Append("<td style=\"width:" + jqgridcolumn.width + "px;text-align:" + jqgridcolumn.align + "\">").Append(text).Append("</td>");
                        }
                    }
                }
                sbHtml.AppendLine("</tr>");
            }
            sbHtml.AppendLine("</table>");
            HttpContext.Current.Response.Write(sbHtml.ToString());
            HttpContext.Current.Response.End();
        }


        //WKL
        public static void JqGridToExcelNew(string JsonColumn, string JsonData, string ExportField, string fileName)
        {
            List<JqGridColumn> ListColumn = JsonConvert.DeserializeObject<List<JqGridColumn>>(JsonColumn);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            HttpContext.Current.Response.Charset = "Utf-8";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName + ".xls", System.Text.Encoding.UTF8));
            StringBuilder sbHtml = new StringBuilder();
            sbHtml.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
            sbHtml.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
            //写出列名
            sbHtml.AppendLine("<tr style=\"background-color: #FFE88C;font-weight: bold; white-space: nowrap;\">");
            string[] FieldInfo = ExportField.Split(',');
            foreach (string item in FieldInfo)
            {
                IList list = ListColumn.FindAll(t => t.name == item);
                foreach (JqGridColumn jqgridcolumn in list)
                {
                    if (jqgridcolumn.hidden.ToLower() == "false" && jqgridcolumn.label != null)
                    {
                        sbHtml.AppendLine("<td style=\"width:" + jqgridcolumn.width + "px;text-align:" + jqgridcolumn.align + "\">" + jqgridcolumn.label + "</td>");
                    }
                }
            }
            sbHtml.AppendLine("</tr>");
            //写数据

            DataTable dt = JsonData.JsonToDataTable();
            foreach (DataRow row in dt.Rows)
            {
                sbHtml.Append("<tr>");
                foreach (string item in FieldInfo)
                {
                    IList list = ListColumn.FindAll(t => t.name == item);
                    foreach (JqGridColumn jqgridcolumn in list)
                    {
                        if (jqgridcolumn.hidden.ToLower() == "false" && jqgridcolumn.label != null)
                        {
                            object text = row[jqgridcolumn.name];
                            sbHtml.Append("<td style=\"width:" + jqgridcolumn.width + "px;text-align:" + jqgridcolumn.align + "\">").Append(text).Append("</td>");
                        }
                    }
                }
                sbHtml.AppendLine("</tr>");
            }
            sbHtml.AppendLine("</table>");
            HttpContext.Current.Response.Write(sbHtml.ToString());
            HttpContext.Current.Response.End();
        }

        #region//质量数据
        public static MemoryStream ExportExcel_quality(DataSet ds, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }

            #region 开始循环DS中的Table,DS中的每个表创建一个Sheet
            for (int p = 0; p < ds.Tables.Count; p += 2)
            {
                #region 创建一个sheet
                string name = "质量数据采集信息";
                ISheet sheet = workbook.CreateSheet(name);
                //sheet.SheetName = ds.Tables[((p + 1) / 2)].Rows[0]["bill_code"].ToString();
                //设置大标题行   
                int rowCount = 0;

                //设置全局列宽和行高   
                sheet.DefaultColumnWidth = 20; //全局列宽   
                sheet.DefaultRowHeightInPoints = 15; //全局行高


                //byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\Administrator\Desktop\常用图标\意通logo.png");
                //int pictureIdx = workbook.AddPicture(bytes, PictureType.PNG);
                //HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                //HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, 1, 1);
                ////把图片插到相应的位置
                //HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);

                //设置标题行数据
                //int a = 0;
                IRow row1 = sheet.CreateRow(rowCount); //创建报表表头标题行 
                row1.Height = 300;
                //row1.RowStyle.SetFont(workbook.CreateFont().Boldweight);= FontBoldWeight.BOLD;
                ICell cell0 = row1.CreateCell(0);
                cell0.SetCellValue("质量数据采集信息");

                IRow row1_1 = sheet.CreateRow(rowCount + 1);
                row1_1.Height = 300;

                CellRangeAddress cra = new CellRangeAddress(0, 1, 0, 12);//起始行，终止行，起始列，终止列
                sheet.AddMergedRegion(cra);

                //row1.RowStyle = workbook.CreateCellStyle();
                //ICellStyle styleCommonRight = workbook.CreateCellStyle();
                cell0.CellStyle.VerticalAlignment = VerticalAlignment.CENTER;
                cell0.CellStyle.Alignment = HorizontalAlignment.CENTER;
                cell0.CellStyle.BorderBottom = BorderStyle.THIN;
                cell0.CellStyle.BorderLeft = BorderStyle.THIN;
                cell0.CellStyle.BorderRight = BorderStyle.THIN;
                cell0.CellStyle.BorderTop = BorderStyle.THIN;



                //row2是表头行
                //IRow row2 = sheet.CreateRow(rowCount + 2);
                //row2.Height = 400;

                //ICell cell201 = row2.CreateCell(0);
                //cell201.SetCellValue("产品出生证");
                //ICell cell202 = row2.CreateCell(1);
                //cell202.SetCellValue(ds.Tables[p].Rows[0][0].ToString());

                //ICell cell203 = row2.CreateCell(2);
                //cell203.SetCellValue("产品图号");
                //ICell cell204 = row2.CreateCell(3);
                //cell204.SetCellValue(ds.Tables[p].Rows[0][1].ToString());

                //ICell cell205 = row2.CreateCell(4);
                //cell205.SetCellValue("产品名称");
                //ICell cell206 = row2.CreateCell(5);
                //cell206.SetCellValue(ds.Tables[p].Rows[0][2].ToString());

                //ICell cell207 = row2.CreateCell(6);
                //cell207.SetCellValue("产品上线时间");
                //ICell cell208 = row2.CreateCell(7);
                //cell208.SetCellValue(ds.Tables[p].Rows[0][3].ToString());

                //ICell cell209 = row2.CreateCell(8);
                //cell209.SetCellValue("产品上线时间");
                //ICell cell10 = row2.CreateCell(9);
                //cell10.SetCellValue(ds.Tables[p].Rows[0][4].ToString());
                //ICell cell11 = row2.CreateCell(4);
                //跨越合并单元格
                //CellRangeAddress cra1 = new CellRangeAddress(2, 2, 1, 3);//起始行，终止行，起始列，终止列
                //CellRangeAddress cra5 = new CellRangeAddress(2, 2, 5, 7);//起始行，终止行，起始列，终止列
                //sheet.AddMergedRegion(cra1);
                //sheet.AddMergedRegion(cra5);

                IRow row2 = sheet.CreateRow(rowCount + 2);
                row2.Height = 400;

                ICell cell301 = row2.CreateCell(0);
                cell301.SetCellValue("产品出生证");

                ICell cell302 = row2.CreateCell(1);
                cell302.SetCellValue("Parm名称");

                ICell cell303 = row2.CreateCell(2);
                cell303.SetCellValue("Parm描述");

                ICell cell304 = row2.CreateCell(3);
                cell304.SetCellValue("数据采集值");

                ICell cell305 = row2.CreateCell(4);
                cell305.SetCellValue("单位");

                ICell cell306 = row2.CreateCell(5);
                cell306.SetCellValue("是否合格");

                ICell cell307 = row2.CreateCell(6);
                cell307.SetCellValue("采集时间");

                ICell cell308 = row2.CreateCell(7);
                cell308.SetCellValue("产线编号");

                ICell cell309 = row2.CreateCell(8);
                cell309.SetCellValue("产线名称");

                ICell cell310 = row2.CreateCell(9);
                cell310.SetCellValue("工位编号");

                ICell cell311 = row2.CreateCell(10);
                cell311.SetCellValue("工位名称");

                ICell cell312 = row2.CreateCell(11);
                cell312.SetCellValue("设备编号");

                ICell cell313 = row2.CreateCell(12);
                cell313.SetCellValue("设备名称");

                //ICell cell294 = row3.CreateCell(6);
                //cell294.SetCellValue("备注");

                //ICell cell295 = row3.CreateCell(7);
                //CellRangeAddress cra2 = new CellRangeAddress(3, 3, 5, 8);//起始行，终止行，起始列，终止列
                //sheet.AddMergedRegion(cra2);


                for (int k = 0; k < ds.Tables[p].Rows.Count; k++)
                {
                    IRow row3 = sheet.CreateRow(k + rowCount + 3);//是内容行
                    row3.Height = 400;
                    int b = 0;
                    for (int j = 0; j < ds.Tables[p].Columns.Count; j++)
                    {
                        string dgvValue = string.Empty;
                        dgvValue = ds.Tables[p].Rows[k][j].ToString();
                        ICell cell = row3.CreateCell(b);
                        cell.SetCellValue(dgvValue);
                        //CellRangeAddress cari = new CellRangeAddress(k + rowCount + 4, k + rowCount + 4, 5, 8);//起始行，终止行，起始列，终止列
                        //sheet.AddMergedRegion(cari);
                        //  cell.CellStyle = style;
                        b++;
                    }
                }
                #endregion
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion

        #region//TOPS质量数据
        public static MemoryStream ExportExcel_quality_Tops(DataSet ds, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }

            #region 开始循环DS中的Table,DS中的每个表创建一个Sheet
            for (int p = 0; p < ds.Tables.Count; p += 2)
            {
                #region 创建一个sheet
                string name = "质量数据采集信息";
                ISheet sheet = workbook.CreateSheet(name);
                //sheet.SheetName = ds.Tables[((p + 1) / 2)].Rows[0]["bill_code"].ToString();
                //设置大标题行   
                int rowCount = 0;

                //设置全局列宽和行高   
                sheet.DefaultColumnWidth = 20; //全局列宽   
                sheet.DefaultRowHeightInPoints = 15; //全局行高


                //byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\Administrator\Desktop\常用图标\意通logo.png");
                //int pictureIdx = workbook.AddPicture(bytes, PictureType.PNG);
                //HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                //HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, 1, 1);
                ////把图片插到相应的位置
                //HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);

                //设置标题行数据
                //int a = 0;
                IRow row1 = sheet.CreateRow(rowCount); //创建报表表头标题行 
                row1.Height = 300;
                //row1.RowStyle.SetFont(workbook.CreateFont().Boldweight);= FontBoldWeight.BOLD;
                ICell cell0 = row1.CreateCell(0);
                cell0.SetCellValue("质量数据采集信息");

                IRow row1_1 = sheet.CreateRow(rowCount + 1);
                row1_1.Height = 300;

                CellRangeAddress cra = new CellRangeAddress(0, 1, 0, 12);//起始行，终止行，起始列，终止列
                sheet.AddMergedRegion(cra);

                //row1.RowStyle = workbook.CreateCellStyle();
                //ICellStyle styleCommonRight = workbook.CreateCellStyle();
                cell0.CellStyle.VerticalAlignment = VerticalAlignment.CENTER;
                cell0.CellStyle.Alignment = HorizontalAlignment.CENTER;
                cell0.CellStyle.BorderBottom = BorderStyle.THIN;
                cell0.CellStyle.BorderLeft = BorderStyle.THIN;
                cell0.CellStyle.BorderRight = BorderStyle.THIN;
                cell0.CellStyle.BorderTop = BorderStyle.THIN;



                //row2是表头行
                //IRow row2 = sheet.CreateRow(rowCount + 2);
                //row2.Height = 400;

                //ICell cell201 = row2.CreateCell(0);
                //cell201.SetCellValue("产品出生证");
                //ICell cell202 = row2.CreateCell(1);
                //cell202.SetCellValue(ds.Tables[p].Rows[0][0].ToString());

                //ICell cell203 = row2.CreateCell(2);
                //cell203.SetCellValue("产品图号");
                //ICell cell204 = row2.CreateCell(3);
                //cell204.SetCellValue(ds.Tables[p].Rows[0][1].ToString());

                //ICell cell205 = row2.CreateCell(4);
                //cell205.SetCellValue("产品名称");
                //ICell cell206 = row2.CreateCell(5);
                //cell206.SetCellValue(ds.Tables[p].Rows[0][2].ToString());

                //ICell cell207 = row2.CreateCell(6);
                //cell207.SetCellValue("产品上线时间");
                //ICell cell208 = row2.CreateCell(7);
                //cell208.SetCellValue(ds.Tables[p].Rows[0][3].ToString());

                //ICell cell209 = row2.CreateCell(8);
                //cell209.SetCellValue("产品上线时间");
                //ICell cell10 = row2.CreateCell(9);
                //cell10.SetCellValue(ds.Tables[p].Rows[0][4].ToString());
                //ICell cell11 = row2.CreateCell(4);
                //跨越合并单元格
                //CellRangeAddress cra1 = new CellRangeAddress(2, 2, 1, 3);//起始行，终止行，起始列，终止列
                //CellRangeAddress cra5 = new CellRangeAddress(2, 2, 5, 7);//起始行，终止行，起始列，终止列
                //sheet.AddMergedRegion(cra1);
                //sheet.AddMergedRegion(cra5);

                IRow row2 = sheet.CreateRow(rowCount + 2);
                row2.Height = 400;

                ICell cell301 = row2.CreateCell(0);
                cell301.SetCellValue("产品出生证");

                ICell cell302 = row2.CreateCell(1);
                cell302.SetCellValue("检测项描述");

                ICell cell303 = row2.CreateCell(2);
                cell303.SetCellValue("数据采集值");

                ICell cell304 = row2.CreateCell(3);
                cell304.SetCellValue("单位");

                ICell cell305 = row2.CreateCell(4);
                cell305.SetCellValue("合格状态");

                ICell cell306 = row2.CreateCell(5);
                cell306.SetCellValue("上控制线");

                ICell cell307 = row2.CreateCell(6);
                cell307.SetCellValue("标准值");

                ICell cell308 = row2.CreateCell(7);
                cell308.SetCellValue("下控制线");

                ICell cell309 = row2.CreateCell(8);
                cell309.SetCellValue("采集时间");

                ICell cell310 = row2.CreateCell(9);
                cell310.SetCellValue("产线编号");

                ICell cell311 = row2.CreateCell(10);
                cell311.SetCellValue("产线名称");

                ICell cell312 = row2.CreateCell(11);
                cell312.SetCellValue("工位编号");

                ICell cell313 = row2.CreateCell(12);
                cell313.SetCellValue("工位名称");

                //ICell cell294 = row3.CreateCell(6);
                //cell294.SetCellValue("备注");

                //ICell cell295 = row3.CreateCell(7);
                //CellRangeAddress cra2 = new CellRangeAddress(3, 3, 5, 8);//起始行，终止行，起始列，终止列
                //sheet.AddMergedRegion(cra2);


                for (int k = 0; k < ds.Tables[p].Rows.Count; k++)
                {
                    IRow row3 = sheet.CreateRow(k + rowCount + 3);//是内容行
                    row3.Height = 400;
                    int b = 0;
                    for (int j = 0; j < ds.Tables[p].Columns.Count; j++)
                    {
                        string dgvValue = string.Empty;
                        dgvValue = ds.Tables[p].Rows[k][j].ToString();
                        ICell cell = row3.CreateCell(b);
                        cell.SetCellValue(dgvValue);
                        //CellRangeAddress cari = new CellRangeAddress(k + rowCount + 4, k + rowCount + 4, 5, 8);//起始行，终止行，起始列，终止列
                        //sheet.AddMergedRegion(cari);
                        //  cell.CellStyle = style;
                        b++;
                    }
                }
                #endregion
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion


        #region//过点数据
        public static MemoryStream ExportExcel_pass(DataSet ds, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }

            #region 开始循环DS中的Table,DS中的每个表创建一个Sheet
            for (int p = 0; p < ds.Tables.Count; p += 2)
            {
                #region 创建一个sheet
                string name = "过点信息";
                ISheet sheet = workbook.CreateSheet(name);
                int rowCount = 0;
                //设置全局列宽和行高   
                sheet.DefaultColumnWidth = 20; //全局列宽   
                sheet.DefaultRowHeightInPoints = 15; //全局行高
                IRow row1 = sheet.CreateRow(rowCount); //创建报表表头标题行 
                row1.Height = 300;
                ICell cell0 = row1.CreateCell(0);
                cell0.SetCellValue("过点信息");

                IRow row1_1 = sheet.CreateRow(rowCount + 1);
                row1_1.Height = 300;
                CellRangeAddress cra = new CellRangeAddress(0, 1, 0, 10);//起始行，终止行，起始列，终止列
                sheet.AddMergedRegion(cra);
                cell0.CellStyle.VerticalAlignment = VerticalAlignment.CENTER;
                cell0.CellStyle.Alignment = HorizontalAlignment.CENTER;
                cell0.CellStyle.BorderBottom = BorderStyle.THIN;
                cell0.CellStyle.BorderLeft = BorderStyle.THIN;
                cell0.CellStyle.BorderRight = BorderStyle.THIN;
                cell0.CellStyle.BorderTop = BorderStyle.THIN;

                IRow row2 = sheet.CreateRow(rowCount + 2);
                row2.Height = 400;

                ICell cell301 = row2.CreateCell(0);
                cell301.SetCellValue("产品出生证");

                ICell cell302 = row2.CreateCell(1);
                cell302.SetCellValue("订单号");

                ICell cell303 = row2.CreateCell(2);
                cell303.SetCellValue("开始时间");

                ICell cell304 = row2.CreateCell(3);
                cell304.SetCellValue("结束时间");

                ICell cell305 = row2.CreateCell(4);
                cell305.SetCellValue("采集时间");

                ICell cell306 = row2.CreateCell(5);
                cell306.SetCellValue("产线编号");

                ICell cell307 = row2.CreateCell(6);
                cell307.SetCellValue("产线名称");

                ICell cell308 = row2.CreateCell(7);
                cell308.SetCellValue("工位编号");

                ICell cell309 = row2.CreateCell(8);
                cell309.SetCellValue("工位名称");

                ICell cell310 = row2.CreateCell(9);
                cell310.SetCellValue("设备编号");

                ICell cell311 = row2.CreateCell(10);
                cell311.SetCellValue("设备名称");

                for (int k = 0; k < ds.Tables[p].Rows.Count; k++)
                {
                    IRow row3 = sheet.CreateRow(k + rowCount + 3);//是内容行
                    row3.Height = 400;
                    int b = 0;
                    for (int j = 0; j < ds.Tables[p].Columns.Count; j++)
                    {
                        string dgvValue = string.Empty;
                        dgvValue = ds.Tables[p].Rows[k][j].ToString();
                        ICell cell = row3.CreateCell(b);
                        cell.SetCellValue(dgvValue);
                        //CellRangeAddress cari = new CellRangeAddress(k + rowCount + 4, k + rowCount + 4, 5, 8);//起始行，终止行，起始列，终止列
                        //sheet.AddMergedRegion(cari);
                        //  cell.CellStyle = style;
                        b++;
                    }
                }
                #endregion
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion

        #region//Tcu质量数据
        public static MemoryStream ExportExcel_Tcu(DataSet ds, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }

            #region 开始循环DS中的Table,DS中的每个表创建一个Sheet
            for (int p = 0; p < ds.Tables.Count; p += 2)
            {
                #region 创建一个sheet
                string name = "TCU数据采集信息";
                ISheet sheet = workbook.CreateSheet(name);
                //sheet.SheetName = ds.Tables[((p + 1) / 2)].Rows[0]["bill_code"].ToString();
                //设置大标题行   
                int rowCount = 0;

                //设置全局列宽和行高   
                sheet.DefaultColumnWidth = 20; //全局列宽   
                sheet.DefaultRowHeightInPoints = 15; //全局行高


                //byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\Administrator\Desktop\常用图标\意通logo.png");
                //int pictureIdx = workbook.AddPicture(bytes, PictureType.PNG);
                //HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                //HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, 1, 1);
                ////把图片插到相应的位置
                //HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);

                //设置标题行数据
                //int a = 0;
                IRow row1 = sheet.CreateRow(rowCount); //创建报表表头标题行 
                row1.Height = 300;
                //row1.RowStyle.SetFont(workbook.CreateFont().Boldweight);= FontBoldWeight.BOLD;
                ICell cell0 = row1.CreateCell(0);
                cell0.SetCellValue("质量数据采集信息");

                IRow row1_1 = sheet.CreateRow(rowCount + 1);
                row1_1.Height = 300;

                CellRangeAddress cra = new CellRangeAddress(0, 1, 0, 18);//起始行，终止行，起始列，终止列
                sheet.AddMergedRegion(cra);

                //row1.RowStyle = workbook.CreateCellStyle();
                //ICellStyle styleCommonRight = workbook.CreateCellStyle();
                cell0.CellStyle.VerticalAlignment = VerticalAlignment.CENTER;
                cell0.CellStyle.Alignment = HorizontalAlignment.CENTER;
                cell0.CellStyle.BorderBottom = BorderStyle.THIN;
                cell0.CellStyle.BorderLeft = BorderStyle.THIN;
                cell0.CellStyle.BorderRight = BorderStyle.THIN;
                cell0.CellStyle.BorderTop = BorderStyle.THIN;


                IRow row2 = sheet.CreateRow(rowCount + 2);
                row2.Height = 400;

                ICell cell301 = row2.CreateCell(0);
                cell301.SetCellValue("ID");

                ICell cell302 = row2.CreateCell(1);
                cell302.SetCellValue("订单号");

                ICell cell303 = row2.CreateCell(2);
                cell303.SetCellValue("产品出生证");

                ICell cell304 = row2.CreateCell(3);
                cell304.SetCellValue("测试文件");

                ICell cell305 = row2.CreateCell(4);
                cell305.SetCellValue("测试文件名称");

                ICell cell306 = row2.CreateCell(5);
                cell306.SetCellValue("Tcu码");

                ICell cell307 = row2.CreateCell(6);
                cell307.SetCellValue("开始日期");

                ICell cell308 = row2.CreateCell(7);
                cell308.SetCellValue("开始时间");

                ICell cell309 = row2.CreateCell(8);
                cell309.SetCellValue("结束日期");

                ICell cell310 = row2.CreateCell(9);
                cell310.SetCellValue("结束时间");

                ICell cell311 = row2.CreateCell(10);
                cell311.SetCellValue("合格");

                ICell cell312 = row2.CreateCell(11);
                cell312.SetCellValue("进程码");

                ICell cell313 = row2.CreateCell(12);
                cell313.SetCellValue("工位");

                ICell cell314 = row2.CreateCell(13);
                cell314.SetCellValue("机型编号");

                ICell cell315 = row2.CreateCell(14);
                cell315.SetCellValue("状态");

                ICell cell316 = row2.CreateCell(15);
                cell316.SetCellValue("软件编号");

                ICell cell317 = row2.CreateCell(16);
                cell317.SetCellValue("软件版本");

                ICell cell318 = row2.CreateCell(17);
                cell318.SetCellValue("供应商");

                

                for (int k = 0; k < ds.Tables[p].Rows.Count; k++)
                {
                    IRow row3 = sheet.CreateRow(k + rowCount + 3);//是内容行
                    row3.Height = 400;
                    int b = 0;
                    for (int j = 0; j < ds.Tables[p].Columns.Count; j++)
                    {
                        string dgvValue = string.Empty;
                        dgvValue = ds.Tables[p].Rows[k][j].ToString();
                        ICell cell = row3.CreateCell(b);
                        cell.SetCellValue(dgvValue);
                        //CellRangeAddress cari = new CellRangeAddress(k + rowCount + 4, k + rowCount + 4, 5, 8);//起始行，终止行，起始列，终止列
                        //sheet.AddMergedRegion(cari);
                        //  cell.CellStyle = style;
                        b++;
                    }
                }
                #endregion
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion

        #region//总装关重件数据
        public static MemoryStream ExportExcel_MA_keypart(DataSet ds, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }

            #region 开始循环DS中的Table,DS中的每个表创建一个Sheet
            for (int p = 0; p < ds.Tables.Count; p += 2)
            {
                #region 创建一个sheet
                string name = "总装关重件采集信息";
                ISheet sheet = workbook.CreateSheet(name);
                //sheet.SheetName = ds.Tables[((p + 1) / 2)].Rows[0]["bill_code"].ToString();
                //设置大标题行   
                int rowCount = 0;

                //设置全局列宽和行高   
                sheet.DefaultColumnWidth = 20; //全局列宽   
                sheet.DefaultRowHeightInPoints = 15; //全局行高


                //byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\Administrator\Desktop\常用图标\意通logo.png");
                //int pictureIdx = workbook.AddPicture(bytes, PictureType.PNG);
                //HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                //HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, 1, 1);
                ////把图片插到相应的位置
                //HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);

                //设置标题行数据
                //int a = 0;
                IRow row1 = sheet.CreateRow(rowCount); //创建报表表头标题行 
                row1.Height = 300;
                //row1.RowStyle.SetFont(workbook.CreateFont().Boldweight);= FontBoldWeight.BOLD;
                ICell cell0 = row1.CreateCell(0);
                cell0.SetCellValue("总装关重件采集信息");

                IRow row1_1 = sheet.CreateRow(rowCount + 1);
                row1_1.Height = 300;

                CellRangeAddress cra = new CellRangeAddress(0, 1, 0, 14);//起始行，终止行，起始列，终止列
                sheet.AddMergedRegion(cra);

                //row1.RowStyle = workbook.CreateCellStyle();
                //ICellStyle styleCommonRight = workbook.CreateCellStyle();
                cell0.CellStyle.VerticalAlignment = VerticalAlignment.CENTER;
                cell0.CellStyle.Alignment = HorizontalAlignment.CENTER;
                cell0.CellStyle.BorderBottom = BorderStyle.THIN;
                cell0.CellStyle.BorderLeft = BorderStyle.THIN;
                cell0.CellStyle.BorderRight = BorderStyle.THIN;
                cell0.CellStyle.BorderTop = BorderStyle.THIN;

                IRow row2 = sheet.CreateRow(rowCount + 2);
                row2.Height = 400;
                
                ICell cell301 = row2.CreateCell(0);
                cell301.SetCellValue("机型编号");

                ICell cell302 = row2.CreateCell(1);
                cell302.SetCellValue("机型名称");

                ICell cell303 = row2.CreateCell(2);
                cell303.SetCellValue("机型类别");

                ICell cell304 = row2.CreateCell(3);
                cell304.SetCellValue("机型区别号");

                ICell cell305 = row2.CreateCell(4);
                cell305.SetCellValue("产品出生证");

                ICell cell306 = row2.CreateCell(5);
                cell306.SetCellValue("关重件编号");

                ICell cell307 = row2.CreateCell(6);
                cell307.SetCellValue("关重件名称");

                ICell cell308 = row2.CreateCell(7);
                cell308.SetCellValue("数量");

                ICell cell309 = row2.CreateCell(8);
                cell309.SetCellValue("关重件条码");

                ICell cell310 = row2.CreateCell(9);
                cell310.SetCellValue("采集时间");

                ICell cell311 = row2.CreateCell(10);
                cell311.SetCellValue("产线编号");

                ICell cell312 = row2.CreateCell(11);
                cell312.SetCellValue("产线名称");

                ICell cell313 = row2.CreateCell(12);
                cell313.SetCellValue("工位编号");

                ICell cell314 = row2.CreateCell(13);
                cell314.SetCellValue("工位名称");

                ICell cell315 = row2.CreateCell(14);
                cell315.SetCellValue("备注");

                //ICell cell294 = row3.CreateCell(6);
                //cell294.SetCellValue("备注");

                //ICell cell295 = row3.CreateCell(7);
                //CellRangeAddress cra2 = new CellRangeAddress(3, 3, 5, 8);//起始行，终止行，起始列，终止列
                //sheet.AddMergedRegion(cra2);


                for (int k = 0; k < ds.Tables[p].Rows.Count; k++)
                {
                    IRow row3 = sheet.CreateRow(k + rowCount + 3);//是内容行
                    row3.Height = 400;
                    int b = 0;
                    for (int j = 0; j < ds.Tables[p].Columns.Count; j++)
                    {
                        string dgvValue = string.Empty;
                        dgvValue = ds.Tables[p].Rows[k][j].ToString();
                        ICell cell = row3.CreateCell(b);
                        cell.SetCellValue(dgvValue);
                        //CellRangeAddress cari = new CellRangeAddress(k + rowCount + 4, k + rowCount + 4, 5, 8);//起始行，终止行，起始列，终止列
                        //sheet.AddMergedRegion(cari);
                        //  cell.CellStyle = style;
                        b++;
                    }
                }
                #endregion
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion

        #region//轴系关重件数据
        public static MemoryStream ExportExcel_SA_keypart(DataSet ds, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }

            #region 开始循环DS中的Table,DS中的每个表创建一个Sheet
            for (int p = 0; p < ds.Tables.Count; p += 2)
            {
                #region 创建一个sheet
                string name = "轴系关重件采集信息";
                ISheet sheet = workbook.CreateSheet(name);
                //sheet.SheetName = ds.Tables[((p + 1) / 2)].Rows[0]["bill_code"].ToString();
                //设置大标题行   
                int rowCount = 0;

                //设置全局列宽和行高   
                sheet.DefaultColumnWidth = 20; //全局列宽   
                sheet.DefaultRowHeightInPoints = 15; //全局行高


                //byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\Administrator\Desktop\常用图标\意通logo.png");
                //int pictureIdx = workbook.AddPicture(bytes, PictureType.PNG);
                //HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                //HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, 1, 1);
                ////把图片插到相应的位置
                //HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);

                //设置标题行数据
                //int a = 0;
                IRow row1 = sheet.CreateRow(rowCount); //创建报表表头标题行 
                row1.Height = 300;
                //row1.RowStyle.SetFont(workbook.CreateFont().Boldweight);= FontBoldWeight.BOLD;
                ICell cell0 = row1.CreateCell(0);
                cell0.SetCellValue("总装关重件采集信息");

                IRow row1_1 = sheet.CreateRow(rowCount + 1);
                row1_1.Height = 300;

                CellRangeAddress cra = new CellRangeAddress(0, 1, 0, 11);//起始行，终止行，起始列，终止列
                sheet.AddMergedRegion(cra);

                //row1.RowStyle = workbook.CreateCellStyle();
                //ICellStyle styleCommonRight = workbook.CreateCellStyle();
                cell0.CellStyle.VerticalAlignment = VerticalAlignment.CENTER;
                cell0.CellStyle.Alignment = HorizontalAlignment.CENTER;
                cell0.CellStyle.BorderBottom = BorderStyle.THIN;
                cell0.CellStyle.BorderLeft = BorderStyle.THIN;
                cell0.CellStyle.BorderRight = BorderStyle.THIN;
                cell0.CellStyle.BorderTop = BorderStyle.THIN;

                IRow row2 = sheet.CreateRow(rowCount + 2);
                row2.Height = 400;

                ICell cell301 = row2.CreateCell(0);
                cell301.SetCellValue("机型编号");

                ICell cell302 = row2.CreateCell(1);
                cell302.SetCellValue("机型名称");

                ICell cell303 = row2.CreateCell(2);
                cell303.SetCellValue("机型类别");

                ICell cell304 = row2.CreateCell(3);
                cell304.SetCellValue("机型区别号");

                ICell cell305 = row2.CreateCell(4);
                cell305.SetCellValue("产品出生证");

                ICell cell306 = row2.CreateCell(5);
                cell306.SetCellValue("关重件编号");

                ICell cell307 = row2.CreateCell(6);
                cell307.SetCellValue("关重件名称");

                ICell cell308 = row2.CreateCell(7);
                cell308.SetCellValue("数量");

                ICell cell309 = row2.CreateCell(8);
                cell309.SetCellValue("关重件条码");

                ICell cell310 = row2.CreateCell(9);
                cell310.SetCellValue("采集时间");

                ICell cell311 = row2.CreateCell(10);
                cell311.SetCellValue("产线编号");

                ICell cell312 = row2.CreateCell(11);
                cell312.SetCellValue("产线名称");

                ICell cell313 = row2.CreateCell(12);
                cell313.SetCellValue("工位编号");

                ICell cell314 = row2.CreateCell(13);
                cell314.SetCellValue("工位名称");

                ICell cell315 = row2.CreateCell(14);
                cell315.SetCellValue("备注");

                //ICell cell294 = row3.CreateCell(6);
                //cell294.SetCellValue("备注");

                //ICell cell295 = row3.CreateCell(7);
                //CellRangeAddress cra2 = new CellRangeAddress(3, 3, 5, 8);//起始行，终止行，起始列，终止列
                //sheet.AddMergedRegion(cra2);


                for (int k = 0; k < ds.Tables[p].Rows.Count; k++)
                {
                    IRow row3 = sheet.CreateRow(k + rowCount + 3);//是内容行
                    row3.Height = 400;
                    int b = 0;
                    for (int j = 0; j < ds.Tables[p].Columns.Count; j++)
                    {
                        string dgvValue = string.Empty;
                        dgvValue = ds.Tables[p].Rows[k][j].ToString();
                        ICell cell = row3.CreateCell(b);
                        cell.SetCellValue(dgvValue);
                        //CellRangeAddress cari = new CellRangeAddress(k + rowCount + 4, k + rowCount + 4, 5, 8);//起始行，终止行，起始列，终止列
                        //sheet.AddMergedRegion(cari);
                        //  cell.CellStyle = style;
                        b++;
                    }
                }
                #endregion
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion

        #region//HCU质量数据
        public static MemoryStream ExportExcel_HCU(DataSet ds, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }

            #region 开始循环DS中的Table,DS中的每个表创建一个Sheet
            for (int p = 0; p < ds.Tables.Count; p += 2)
            {
                #region 创建一个sheet
                string name = "HCU测试数据采集信息";
                ISheet sheet = workbook.CreateSheet(name);
                //sheet.SheetName = ds.Tables[((p + 1) / 2)].Rows[0]["bill_code"].ToString();
                //设置大标题行   
                int rowCount = 0;

                //设置全局列宽和行高   
                sheet.DefaultColumnWidth = 20; //全局列宽   
                sheet.DefaultRowHeightInPoints = 15; //全局行高


                //byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\Administrator\Desktop\常用图标\意通logo.png");
                //int pictureIdx = workbook.AddPicture(bytes, PictureType.PNG);
                //HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                //HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, 1, 1);
                ////把图片插到相应的位置
                //HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);

                //设置标题行数据
                //int a = 0;
                IRow row1 = sheet.CreateRow(rowCount); //创建报表表头标题行 
                row1.Height = 300;
                //row1.RowStyle.SetFont(workbook.CreateFont().Boldweight);= FontBoldWeight.BOLD;
                ICell cell0 = row1.CreateCell(0);
                cell0.SetCellValue("HCU测试数据采集信息");

                IRow row1_1 = sheet.CreateRow(rowCount + 1);
                row1_1.Height = 300;

                CellRangeAddress cra = new CellRangeAddress(0, 1, 0, 75);//起始行，终止行，起始列，终止列
                sheet.AddMergedRegion(cra);

                //row1.RowStyle = workbook.CreateCellStyle();
                //ICellStyle styleCommonRight = workbook.CreateCellStyle();
                cell0.CellStyle.VerticalAlignment = VerticalAlignment.CENTER;
                cell0.CellStyle.Alignment = HorizontalAlignment.CENTER;
                cell0.CellStyle.BorderBottom = BorderStyle.THIN;
                cell0.CellStyle.BorderLeft = BorderStyle.THIN;
                cell0.CellStyle.BorderRight = BorderStyle.THIN;
                cell0.CellStyle.BorderTop = BorderStyle.THIN;


                IRow row2 = sheet.CreateRow(rowCount + 2);
                row2.Height = 400;

                ICell cell301 = row2.CreateCell(0);
                cell301.SetCellValue("线体区别号");

                ICell cell302 = row2.CreateCell(1);
                cell302.SetCellValue("产品出生证");

                ICell cell303 = row2.CreateCell(2);
                cell303.SetCellValue("是否合格");

                ICell cell304 = row2.CreateCell(3);
                cell304.SetCellValue("HCU测试时间");

                ICell cell305 = row2.CreateCell(4);
                cell305.SetCellValue("PI_CPCV1_1");

                ICell cell306 = row2.CreateCell(5);
                cell306.SetCellValue("PI_CPCV1_2");

                ICell cell307 = row2.CreateCell(6);
                cell307.SetCellValue("PI_CPCV1_3");

                ICell cell308 = row2.CreateCell(7);
                cell308.SetCellValue("PI_CPCV1_4");

                ICell cell309 = row2.CreateCell(8);
                cell309.SetCellValue("PI_CPCV1_5");

                ICell cell310 = row2.CreateCell(9);
                cell310.SetCellValue("PI_CPCV1_6");

                ICell cell311 = row2.CreateCell(10);
                cell311.SetCellValue("PI_CPCV1_7");

                ICell cell312 = row2.CreateCell(11);
                cell312.SetCellValue("PI_CPCV2_1");

                ICell cell313 = row2.CreateCell(12);
                cell313.SetCellValue("PI_CPCV2_2");

                ICell cell314 = row2.CreateCell(13);
                cell314.SetCellValue("PI_CPCV2_3");

                ICell cell315 = row2.CreateCell(14);
                cell315.SetCellValue("PI_CPCV2_4");

                ICell cell316 = row2.CreateCell(15);
                cell316.SetCellValue("PI_CPCV2_5");

                ICell cell317 = row2.CreateCell(16);
                cell317.SetCellValue("PI_CPCV2_6");

                ICell cell318 = row2.CreateCell(17);
                cell318.SetCellValue("PI_CPCV2_7");

                ICell cell319 = row2.CreateCell(18);
                cell319.SetCellValue("PI_MPCV_1");

                ICell cell320 = row2.CreateCell(19);
                cell320.SetCellValue("PI_MPCV_2");

                ICell cell321 = row2.CreateCell(20);
                cell321.SetCellValue("PI_MPCV_3");

                ICell cell322 = row2.CreateCell(21);
                cell322.SetCellValue("PI_MPCV_4");

                ICell cell323 = row2.CreateCell(22);
                cell323.SetCellValue("PI_MPCV_5");

                ICell cell324 = row2.CreateCell(23);
                cell324.SetCellValue("PI_MPCV_6");

                ICell cell325 = row2.CreateCell(24);
                cell325.SetCellValue("PI_MPCV_7");

                ICell cell326 = row2.CreateCell(25);
                cell326.SetCellValue("PI_GPCV1_1");

                ICell cell327 = row2.CreateCell(26);
                cell327.SetCellValue("PI_GPCV1_2");

                ICell cell328 = row2.CreateCell(27);
                cell328.SetCellValue("PI_GPCV1_3");

                ICell cell329 = row2.CreateCell(28);
                cell329.SetCellValue("PI_GPCV1_4");

                ICell cell330 = row2.CreateCell(29);
                cell330.SetCellValue("PI_GPCV1_5");

                ICell cell331 = row2.CreateCell(30);
                cell331.SetCellValue("PI_GPCV1_6");

                ICell cell332 = row2.CreateCell(31);
                cell332.SetCellValue("PI_GPCV1_7");

                ICell cell333 = row2.CreateCell(32);
                cell333.SetCellValue("PI_GPCV1_8");

                ICell cell334 = row2.CreateCell(33);
                cell334.SetCellValue("PI_GPCV2_1");

                ICell cell335 = row2.CreateCell(34);
                cell335.SetCellValue("PI_GPCV2_2");

                ICell cell336 = row2.CreateCell(35);
                cell336.SetCellValue("PI_GPCV2_3");

                ICell cell337 = row2.CreateCell(36);
                cell337.SetCellValue("PI_GPCV2_4");

                ICell cell338 = row2.CreateCell(37);
                cell338.SetCellValue("PI_GPCV2_5");

                ICell cell339 = row2.CreateCell(38);
                cell339.SetCellValue("PI_GPCV2_6");

                ICell cell340 = row2.CreateCell(39);
                cell340.SetCellValue("PI_GPCV2_7");

                ICell cell341 = row2.CreateCell(40);
                cell341.SetCellValue("PI_GPCV2_8");

                ICell cell342 = row2.CreateCell(41);
                cell342.SetCellValue("QI_SCV1_1");

                ICell cell343 = row2.CreateCell(42);
                cell343.SetCellValue("QI_SCV1_2");

                ICell cell344 = row2.CreateCell(43);
                cell344.SetCellValue("QI_SCV1_3");

                ICell cell345 = row2.CreateCell(44);
                cell345.SetCellValue("QI_SCV1_4");

                ICell cell346 = row2.CreateCell(45);
                cell346.SetCellValue("QI_SCV1_5");

                ICell cell347 = row2.CreateCell(46);
                cell347.SetCellValue("QI_SCV1_6");

                ICell cell348 = row2.CreateCell(47);
                cell348.SetCellValue("QI_SCV1_7");

                ICell cell349 = row2.CreateCell(48);
                cell349.SetCellValue("QI_SCV1_8");

                ICell cell350 = row2.CreateCell(49);
                cell350.SetCellValue("QI_SCV1_9");

                ICell cell351 = row2.CreateCell(50);
                cell351.SetCellValue("QI_SCV1_10");

                ICell cell352 = row2.CreateCell(51);
                cell352.SetCellValue("QI_SCV1_11");

                ICell cell353 = row2.CreateCell(52);
                cell353.SetCellValue("QI_SCV1_12");

                ICell cell354 = row2.CreateCell(53);
                cell354.SetCellValue("QI_SCV1_13");

                ICell cell355 = row2.CreateCell(54);
                cell355.SetCellValue("QI_SCV2_1");

                ICell cell356 = row2.CreateCell(55);
                cell356.SetCellValue("QI_SCV2_2");

                ICell cell357 = row2.CreateCell(56);
                cell357.SetCellValue("QI_SCV2_3");

                ICell cell358 = row2.CreateCell(57);
                cell358.SetCellValue("QI_SCV2_4");

                ICell cell359 = row2.CreateCell(58);
                cell359.SetCellValue("QI_SCV2_5");

                ICell cell360 = row2.CreateCell(59);
                cell360.SetCellValue("QI_SCV2_6");

                ICell cell361 = row2.CreateCell(60);
                cell361.SetCellValue("QI_SCV2_7");

                ICell cell362 = row2.CreateCell(61);
                cell362.SetCellValue("QI_SCV2_8");

                ICell cell363 = row2.CreateCell(62);
                cell363.SetCellValue("QI_SCV2_9");

                ICell cell364 = row2.CreateCell(63);
                cell364.SetCellValue("QI_SCV2_10");

                ICell cell365 = row2.CreateCell(64);
                cell365.SetCellValue("QI_SCV2_11");

                ICell cell366 = row2.CreateCell(65);
                cell366.SetCellValue("QI_SCV2_12");

                ICell cell367 = row2.CreateCell(66);
                cell367.SetCellValue("QI_SCV2_13");

                ICell cell368 = row2.CreateCell(67);
                cell368.SetCellValue("QI_COFCV_1");

                ICell cell369 = row2.CreateCell(68);
                cell369.SetCellValue("QI_COFCV_2");

                ICell cell370 = row2.CreateCell(69);
                cell370.SetCellValue("QI_COFCV_3");

                ICell cell371 = row2.CreateCell(70);
                cell371.SetCellValue("QI_COFCV_4");

                ICell cell372 = row2.CreateCell(71);
                cell372.SetCellValue("QI_COFCV_5");

                ICell cell373 = row2.CreateCell(72);
                cell373.SetCellValue("QI_COFCV_6");

                ICell cell374 = row2.CreateCell(73);
                cell374.SetCellValue("QI_COFCV_7");

                ICell cell375 = row2.CreateCell(74);
                cell375.SetCellValue("QI_COFCV_8");

                ICell cell376 = row2.CreateCell(75);
                cell376.SetCellValue("QI_COFCV_9");


                for (int k = 0; k < ds.Tables[p].Rows.Count; k++)
                {
                    IRow row3 = sheet.CreateRow(k + rowCount + 3);//是内容行
                    row3.Height = 400;
                    int b = 0;
                    for (int j = 0; j < ds.Tables[p].Columns.Count; j++)
                    {
                        string dgvValue = string.Empty;
                        dgvValue = ds.Tables[p].Rows[k][j].ToString();
                        ICell cell = row3.CreateCell(b);
                        cell.SetCellValue(dgvValue);
                        //CellRangeAddress cari = new CellRangeAddress(k + rowCount + 4, k + rowCount + 4, 5, 8);//起始行，终止行，起始列，终止列
                        //sheet.AddMergedRegion(cari);
                        //  cell.CellStyle = style;
                        b++;
                    }
                }
                #endregion
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion

        #region//EOL质量数据 
        public static MemoryStream ExportExcel_EOL(DataSet ds, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }

            #region 开始循环DS中的Table,DS中的每个表创建一个Sheet
            for (int p = 0; p < ds.Tables.Count; p += 2)
            {
                #region 创建一个sheet
                string name = "EOL测试数据采集信息";
                ISheet sheet = workbook.CreateSheet(name);
                //sheet.SheetName = ds.Tables[((p + 1) / 2)].Rows[0]["bill_code"].ToString();
                //设置大标题行
                int rowCount = 0;

                //设置全局列宽和行高   
                sheet.DefaultColumnWidth = 20; //全局列宽   
                sheet.DefaultRowHeightInPoints = 15; //全局行高


                //byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\Administrator\Desktop\常用图标\意通logo.png");
                //int pictureIdx = workbook.AddPicture(bytes, PictureType.PNG);
                //HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                //HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, 1, 1);
                ////把图片插到相应的位置
                //HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);

                //设置标题行数据
                //int a = 0;
                IRow row1 = sheet.CreateRow(rowCount); //创建报表表头标题行 
                row1.Height = 300;
                //row1.RowStyle.SetFont(workbook.CreateFont().Boldweight);= FontBoldWeight.BOLD;
                ICell cell0 = row1.CreateCell(0);
                cell0.SetCellValue("EOL测试数据采集信息");

                IRow row1_1 = sheet.CreateRow(rowCount + 1);
                row1_1.Height = 300;

                CellRangeAddress cra = new CellRangeAddress(0, 1, 0, 24);//起始行，终止行，起始列，终止列
                sheet.AddMergedRegion(cra);

                //row1.RowStyle = workbook.CreateCellStyle();
                //ICellStyle styleCommonRight = workbook.CreateCellStyle();
                cell0.CellStyle.VerticalAlignment = VerticalAlignment.CENTER;
                cell0.CellStyle.Alignment = HorizontalAlignment.CENTER;
                cell0.CellStyle.BorderBottom = BorderStyle.THIN;
                cell0.CellStyle.BorderLeft = BorderStyle.THIN;
                cell0.CellStyle.BorderRight = BorderStyle.THIN;
                cell0.CellStyle.BorderTop = BorderStyle.THIN;


                IRow row2 = sheet.CreateRow(rowCount + 2);
                row2.Height = 400;

                ICell cell301 = row2.CreateCell(0);
                cell301.SetCellValue("线体区别号");

                ICell cell302 = row2.CreateCell(1);
                cell302.SetCellValue("产品出生证");

                ICell cell303 = row2.CreateCell(2);
                cell303.SetCellValue("是否合格");

                ICell cell304 = row2.CreateCell(3);
                cell304.SetCellValue("HCU序列号");

                ICell cell305 = row2.CreateCell(4);
                cell305.SetCellValue("EOL测试时间");

                ICell cell306 = row2.CreateCell(5);
                cell306.SetCellValue("Clutch1");

                ICell cell307 = row2.CreateCell(6);
                cell307.SetCellValue("Clutch2");

                ICell cell308 = row2.CreateCell(7);
                cell308.SetCellValue("GearActuator31");

                ICell cell309 = row2.CreateCell(8);
                cell309.SetCellValue("GearActuator26");

                ICell cell310 = row2.CreateCell(9);
                cell310.SetCellValue("GearActuator5N");

                ICell cell311 = row2.CreateCell(10);
                cell311.SetCellValue("GearActuator4R");

                ICell cell312 = row2.CreateCell(11);
                cell312.SetCellValue("CPCV11");

                ICell cell313 = row2.CreateCell(12);
                cell313.SetCellValue("CPCV12");

                ICell cell314 = row2.CreateCell(13);
                cell314.SetCellValue("CPCV13");

                ICell cell315 = row2.CreateCell(14);
                cell315.SetCellValue("CPCV14");

                ICell cell316 = row2.CreateCell(15);
                cell316.SetCellValue("CPCV15");

                ICell cell317 = row2.CreateCell(16);
                cell317.SetCellValue("CPCV16");

                ICell cell318 = row2.CreateCell(17);
                cell318.SetCellValue("CPCV17");

                ICell cell319 = row2.CreateCell(18);
                cell319.SetCellValue("CPCV21");

                ICell cell320 = row2.CreateCell(19);
                cell320.SetCellValue("CPCV22");

                ICell cell321 = row2.CreateCell(20);
                cell321.SetCellValue("CPCV23");

                ICell cell322 = row2.CreateCell(21);
                cell322.SetCellValue("CPCV24");

                ICell cell323 = row2.CreateCell(22);
                cell323.SetCellValue("CPCV25");

                ICell cell324 = row2.CreateCell(23);
                cell324.SetCellValue("CPCV26");

                ICell cell325 = row2.CreateCell(24);
                cell325.SetCellValue("CPCV27");


                for (int k = 0; k < ds.Tables[p].Rows.Count; k++)
                {
                    IRow row3 = sheet.CreateRow(k + rowCount + 3);//是内容行
                    row3.Height = 400;
                    int b = 0;
                    for (int j = 0; j < ds.Tables[p].Columns.Count; j++)
                    {
                        string dgvValue = string.Empty;
                        dgvValue = ds.Tables[p].Rows[k][j].ToString();
                        ICell cell = row3.CreateCell(b);
                        cell.SetCellValue(dgvValue);
                        //CellRangeAddress cari = new CellRangeAddress(k + rowCount + 4, k + rowCount + 4, 5, 8);//起始行，终止行，起始列，终止列
                        //sheet.AddMergedRegion(cari);
                        //  cell.CellStyle = style;
                        b++;
                    }
                }
                #endregion
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion

        #region//HCU1-EOL质量数据 
        public static MemoryStream ExportExcel_EOL_HCU1(DataSet ds, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }

            #region 开始循环DS中的Table,DS中的每个表创建一个Sheet
            for (int p = 0; p < ds.Tables.Count; p += 2)
            {
                #region 创建一个sheet
                string name = "HCU测试数据采集信息";
                ISheet sheet = workbook.CreateSheet(name);
                //sheet.SheetName = ds.Tables[((p + 1) / 2)].Rows[0]["bill_code"].ToString();
                //设置大标题行
                int rowCount = 0;

                //设置全局列宽和行高   
                sheet.DefaultColumnWidth = 20; //全局列宽   
                sheet.DefaultRowHeightInPoints = 15; //全局行高


                //byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\Administrator\Desktop\常用图标\意通logo.png");
                //int pictureIdx = workbook.AddPicture(bytes, PictureType.PNG);
                //HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                //HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, 1, 1);
                ////把图片插到相应的位置
                //HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);

                //设置标题行数据
                //int a = 0;
                IRow row1 = sheet.CreateRow(rowCount); //创建报表表头标题行 
                row1.Height = 300;
                //row1.RowStyle.SetFont(workbook.CreateFont().Boldweight);= FontBoldWeight.BOLD;
                ICell cell0 = row1.CreateCell(0);
                cell0.SetCellValue("HCU测试数据采集信息");

                IRow row1_1 = sheet.CreateRow(rowCount + 1);
                row1_1.Height = 300;
                DataTable dt=  ds.Tables[0];
                CellRangeAddress cra = new CellRangeAddress(0, 1, 0, dt.Columns.Count-1);//起始行，终止行，起始列，终止列
                sheet.AddMergedRegion(cra);
             

                //row1.RowStyle = workbook.CreateCellStyle();
                //ICellStyle styleCommonRight = workbook.CreateCellStyle();
                cell0.CellStyle.VerticalAlignment = VerticalAlignment.CENTER;
                cell0.CellStyle.Alignment = HorizontalAlignment.CENTER;
                cell0.CellStyle.BorderBottom = BorderStyle.THIN;
                cell0.CellStyle.BorderLeft = BorderStyle.THIN;
                cell0.CellStyle.BorderRight = BorderStyle.THIN;
                cell0.CellStyle.BorderTop = BorderStyle.THIN;


                IRow row2 = sheet.CreateRow(rowCount + 2);
                row2.Height = 400;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ICell cell1 = row2.CreateCell(i);
                    if (i==0)
                    {
                        cell1.SetCellValue("产品出生证");
                    }
                    else if (i==1)
                    {
                        cell1.SetCellValue("工位序列");
                    }
                    else if (i == 2)
                    {
                        cell1.SetCellValue("是否合格");
                    }
                    else if (i == 3)
                    {
                        cell1.SetCellValue("测试时间");
                    }
                    else
                    {
                        cell1.SetCellValue(dt.Columns[i].ColumnName);
                    }

                }
                //ICell cell301 = row2.CreateCell(0);
                //cell301.SetCellValue("产品出生证");

                //ICell cell302 = row2.CreateCell(1);
                //cell302.SetCellValue("工位序列");

                //ICell cell303 = row2.CreateCell(2);
                //cell303.SetCellValue("是否合格");

                //ICell cell304 = row2.CreateCell(3);
                //cell304.SetCellValue("测试时间");

                //ICell cell305 = row2.CreateCell(4);
                //cell305.SetCellValue("PI_CPCV1_1");

                //ICell cell306 = row2.CreateCell(5);
                //cell306.SetCellValue("PI_CPCV1_2");

                //ICell cell307 = row2.CreateCell(6);
                //cell307.SetCellValue("PI_CPCV1_3");

                //ICell cell308 = row2.CreateCell(7);
                //cell308.SetCellValue("PI_CPCV1_4");

                //ICell cell309 = row2.CreateCell(8);
                //cell309.SetCellValue("PI_CPCV1_5");

                //ICell cell310 = row2.CreateCell(9);
                //cell310.SetCellValue("PI_CPCV1_6");

                //ICell cell311 = row2.CreateCell(10);
                //cell311.SetCellValue("PI_CPCV1_7");

                //ICell cell312 = row2.CreateCell(11);
                //cell312.SetCellValue("PI_CPCV2_1");

                //ICell cell313 = row2.CreateCell(12);
                //cell313.SetCellValue("PI_CPCV2_2");

                //ICell cell314 = row2.CreateCell(13);
                //cell314.SetCellValue("PI_CPCV2_3");

                //ICell cell315 = row2.CreateCell(14);
                //cell315.SetCellValue("PI_CPCV2_4");

                //ICell cell316 = row2.CreateCell(15);
                //cell316.SetCellValue("PI_CPCV2_5");

                //ICell cell317 = row2.CreateCell(16);
                //cell317.SetCellValue("PI_CPCV2_6");

                //ICell cell318 = row2.CreateCell(17);
                //cell318.SetCellValue("PI_CPCV2_7");

                //ICell cell319 = row2.CreateCell(18);
                //cell319.SetCellValue("PI_MPCV_1");

                //ICell cell320 = row2.CreateCell(19);
                //cell320.SetCellValue("PI_MPCV_2");

                //ICell cell321 = row2.CreateCell(20);
                //cell321.SetCellValue("PI_MPCV_3");

                //ICell cell322 = row2.CreateCell(21);
                //cell322.SetCellValue("PI_MPCV_4");

                //ICell cell323 = row2.CreateCell(22);
                //cell323.SetCellValue("PI_MPCV_5");

                //ICell cell324 = row2.CreateCell(23);
                //cell324.SetCellValue("PI_MPCV_6");

                //ICell cell325 = row2.CreateCell(24);
                //cell325.SetCellValue("PI_MPCV_7");

                //ICell cell326 = row2.CreateCell(25);
                //cell326.SetCellValue("PI_GPCV1_1");

                //ICell cell327 = row2.CreateCell(26);
                //cell327.SetCellValue("PI_GPCV1_2");

                //ICell cell328 = row2.CreateCell(27);
                //cell328.SetCellValue("PI_GPCV1_3");

                //ICell cell329 = row2.CreateCell(28);
                //cell329.SetCellValue("PI_GPCV1_4");

                //ICell cell330 = row2.CreateCell(29);
                //cell330.SetCellValue("PI_GPCV1_5");

                //ICell cell331 = row2.CreateCell(30);
                //cell331.SetCellValue("PI_GPCV1_6");

                //ICell cell332 = row2.CreateCell(31);
                //cell332.SetCellValue("PI_GPCV1_7");

                //ICell cell333 = row2.CreateCell(32);
                //cell333.SetCellValue("PI_GPCV1_8");

                //ICell cell334 = row2.CreateCell(33);
                //cell334.SetCellValue("PI_GPCV2_1");

                //ICell cell335 = row2.CreateCell(34);
                //cell335.SetCellValue("PI_GPCV2_2");

                //ICell cell336 = row2.CreateCell(35);
                //cell336.SetCellValue("PI_GPCV2_3");

                //ICell cell337 = row2.CreateCell(36);
                //cell337.SetCellValue("PI_GPCV2_4");

                //ICell cell338 = row2.CreateCell(37);
                //cell338.SetCellValue("PI_GPCV2_5");

                //ICell cell339 = row2.CreateCell(38);
                //cell339.SetCellValue("PI_GPCV2_6");

                //ICell cell340 = row2.CreateCell(39);
                //cell340.SetCellValue("PI_GPCV2_7");

                //ICell cell341 = row2.CreateCell(40);
                //cell341.SetCellValue("PI_GPCV2_8");

                //ICell cell342 = row2.CreateCell(41);
                //cell342.SetCellValue("QI_SCV1_1");

                //ICell cell343 = row2.CreateCell(42);
                //cell343.SetCellValue("QI_SCV1_2");

                

                for (int k = 0; k < ds.Tables[p].Rows.Count; k++)
                {
                    IRow row3 = sheet.CreateRow(k + rowCount + 3);//是内容行
                    row3.Height = 400;
                    int b = 0;
                    for (int j = 0; j < ds.Tables[p].Columns.Count; j++)
                    {
                        string dgvValue = string.Empty;
                        dgvValue = ds.Tables[p].Rows[k][j].ToString();
                        ICell cell = row3.CreateCell(b);
                        cell.SetCellValue(dgvValue);
                        //CellRangeAddress cari = new CellRangeAddress(k + rowCount + 4, k + rowCount + 4, 5, 8);//起始行，终止行，起始列，终止列
                        //sheet.AddMergedRegion(cari);
                        //  cell.CellStyle = style;
                        b++;
                    }
                }
                #endregion
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion

        #region//HCU2-EOL质量数据 
        public static MemoryStream ExportExcel_EOL_HCU2(DataSet ds, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }

            #region 开始循环DS中的Table,DS中的每个表创建一个Sheet
            for (int p = 0; p < ds.Tables.Count; p += 2)
            {
                #region 创建一个sheet
                string name = "HCU测试数据采集信息";
                ISheet sheet = workbook.CreateSheet(name);
                //sheet.SheetName = ds.Tables[((p + 1) / 2)].Rows[0]["bill_code"].ToString();
                //设置大标题行
                int rowCount = 0;

                //设置全局列宽和行高   
                sheet.DefaultColumnWidth = 20; //全局列宽   
                sheet.DefaultRowHeightInPoints = 15; //全局行高


                //byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\Administrator\Desktop\常用图标\意通logo.png");
                //int pictureIdx = workbook.AddPicture(bytes, PictureType.PNG);
                //HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                //HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, 1, 1);
                ////把图片插到相应的位置
                //HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);

                //设置标题行数据
                //int a = 0;
                IRow row1 = sheet.CreateRow(rowCount); //创建报表表头标题行 
                row1.Height = 300;
                //row1.RowStyle.SetFont(workbook.CreateFont().Boldweight);= FontBoldWeight.BOLD;
                ICell cell0 = row1.CreateCell(0);
                cell0.SetCellValue("HCU测试数据采集信息");

                IRow row1_1 = sheet.CreateRow(rowCount + 1);
                row1_1.Height = 300;

                CellRangeAddress cra = new CellRangeAddress(0, 1, 0, 18);//起始行，终止行，起始列，终止列
                sheet.AddMergedRegion(cra);

                //row1.RowStyle = workbook.CreateCellStyle();
                //ICellStyle styleCommonRight = workbook.CreateCellStyle();
                cell0.CellStyle.VerticalAlignment = VerticalAlignment.CENTER;
                cell0.CellStyle.Alignment = HorizontalAlignment.CENTER;
                cell0.CellStyle.BorderBottom = BorderStyle.THIN;
                cell0.CellStyle.BorderLeft = BorderStyle.THIN;
                cell0.CellStyle.BorderRight = BorderStyle.THIN;
                cell0.CellStyle.BorderTop = BorderStyle.THIN;


                IRow row2 = sheet.CreateRow(rowCount + 2);
                row2.Height = 400;

                ICell cell301 = row2.CreateCell(0);
                cell301.SetCellValue("产品出生证");

                ICell cell302 = row2.CreateCell(1);
                cell302.SetCellValue("工位序列");

                ICell cell303 = row2.CreateCell(2);
                cell303.SetCellValue("是否合格");

                ICell cell304 = row2.CreateCell(3);
                cell304.SetCellValue("序列号");

                ICell cell305 = row2.CreateCell(4);
                cell305.SetCellValue("测试时间");

                ICell cell306 = row2.CreateCell(5);
                cell306.SetCellValue("Data_1");

                ICell cell307 = row2.CreateCell(6);
                cell307.SetCellValue("Data_2");

                ICell cell308 = row2.CreateCell(7);
                cell308.SetCellValue("Data_3");

                ICell cell309 = row2.CreateCell(8);
                cell309.SetCellValue("Data_4");

                ICell cell310 = row2.CreateCell(9);
                cell310.SetCellValue("Data_5");

                ICell cell311 = row2.CreateCell(10);
                cell311.SetCellValue("Data_6");

                ICell cell312 = row2.CreateCell(11);
                cell312.SetCellValue("Data_7");

                ICell cell313 = row2.CreateCell(12);
                cell313.SetCellValue("Data_8");

                ICell cell314 = row2.CreateCell(13);
                cell314.SetCellValue("Data_9");

                ICell cell315 = row2.CreateCell(14);
                cell315.SetCellValue("Data_10");

                ICell cell316 = row2.CreateCell(15);
                cell316.SetCellValue("Data_11");

                ICell cell317 = row2.CreateCell(16);
                cell317.SetCellValue("Data_12");

                ICell cell318 = row2.CreateCell(17);
                cell318.SetCellValue("Data_13");

                ICell cell319 = row2.CreateCell(18);
                cell319.SetCellValue("Data_14");


                for (int k = 0; k < ds.Tables[p].Rows.Count; k++)
                {
                    IRow row3 = sheet.CreateRow(k + rowCount + 3);//是内容行
                    row3.Height = 400;
                    int b = 0;
                    for (int j = 0; j < ds.Tables[p].Columns.Count; j++)
                    {
                        string dgvValue = string.Empty;
                        dgvValue = ds.Tables[p].Rows[k][j].ToString();
                        ICell cell = row3.CreateCell(b);
                        cell.SetCellValue(dgvValue);
                        //CellRangeAddress cari = new CellRangeAddress(k + rowCount + 4, k + rowCount + 4, 5, 8);//起始行，终止行，起始列，终止列
                        //sheet.AddMergedRegion(cari);
                        //  cell.CellStyle = style;
                        b++;
                    }
                }
                #endregion
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion

        #region//EOL质量数据 order_code,plat_S,part_number,order_date_S,create_time,description,state,remark
        public static MemoryStream ExportExcel_ERP(DataSet ds, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }

            #region 开始循环DS中的Table,DS中的每个表创建一个Sheet
            for (int p = 0; p < ds.Tables.Count; p += 2)
            {
                #region 创建一个sheet
                string name = "ERP订单接收日志记录";
                ISheet sheet = workbook.CreateSheet(name);
                //sheet.SheetName = ds.Tables[((p + 1) / 2)].Rows[0]["bill_code"].ToString();
                //设置大标题行
                int rowCount = 0;

                //设置全局列宽和行高   
                sheet.DefaultColumnWidth = 20; //全局列宽   
                sheet.DefaultRowHeightInPoints = 15; //全局行高


                //byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\Administrator\Desktop\常用图标\意通logo.png");
                //int pictureIdx = workbook.AddPicture(bytes, PictureType.PNG);
                //HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                //HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, 1, 1);
                ////把图片插到相应的位置
                //HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);

                //设置标题行数据
                //int a = 0;
                IRow row1 = sheet.CreateRow(rowCount); //创建报表表头标题行 
                row1.Height = 300;
                //row1.RowStyle.SetFont(workbook.CreateFont().Boldweight);= FontBoldWeight.BOLD;
                ICell cell0 = row1.CreateCell(0);
                cell0.SetCellValue("ERP订单转移日志记录表");

                IRow row1_1 = sheet.CreateRow(rowCount + 1);
                row1_1.Height = 300;

                CellRangeAddress cra = new CellRangeAddress(0, 1, 0, 7);//起始行，终止行，起始列，终止列
                sheet.AddMergedRegion(cra);

                //row1.RowStyle = workbook.CreateCellStyle();
                //ICellStyle styleCommonRight = workbook.CreateCellStyle();
                cell0.CellStyle.VerticalAlignment = VerticalAlignment.CENTER;
                cell0.CellStyle.Alignment = HorizontalAlignment.CENTER;
                cell0.CellStyle.BorderBottom = BorderStyle.THIN;
                cell0.CellStyle.BorderLeft = BorderStyle.THIN;
                cell0.CellStyle.BorderRight = BorderStyle.THIN;
                cell0.CellStyle.BorderTop = BorderStyle.THIN;

                IRow row2 = sheet.CreateRow(rowCount + 2);
                row2.Height = 400;

                ICell cell301 = row2.CreateCell(0);
                cell301.SetCellValue("订单编号");

                ICell cell302 = row2.CreateCell(1);
                cell302.SetCellValue("平台号");

                ICell cell303 = row2.CreateCell(2);
                cell303.SetCellValue("机型编号");

                ICell cell304 = row2.CreateCell(3);
                cell304.SetCellValue("订单开始时间");

                ICell cell305 = row2.CreateCell(4);
                cell305.SetCellValue("日志记录时间");

                ICell cell306 = row2.CreateCell(5);
                cell306.SetCellValue("日志内容");

                ICell cell307 = row2.CreateCell(6);
                cell307.SetCellValue("转移状态");

                ICell cell308 = row2.CreateCell(7);
                cell308.SetCellValue("备注");


                for (int k = 0; k < ds.Tables[p].Rows.Count; k++)
                {
                    IRow row3 = sheet.CreateRow(k + rowCount + 3);//是内容行
                    row3.Height = 400;
                    int b = 0;
                    for (int j = 0; j < ds.Tables[p].Columns.Count; j++)
                    {
                        string dgvValue = string.Empty;
                        dgvValue = ds.Tables[p].Rows[k][j].ToString();
                        ICell cell = row3.CreateCell(b);
                        cell.SetCellValue(dgvValue);
                        //CellRangeAddress cari = new CellRangeAddress(k + rowCount + 4, k + rowCount + 4, 5, 8);//起始行，终止行，起始列，终止列
                        //sheet.AddMergedRegion(cari);
                        //  cell.CellStyle = style;
                        b++;
                    }
                }
                #endregion
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion

        #region 设备状态信息检索导出数据

        public static MemoryStream ExportExcel_Equip(DataSet ds, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }

            #region 开始循环DS中的Table,DS中的每个表创建一个Sheet
            for (int p = 0; p < ds.Tables.Count; p += 2)
            {
                #region 创建一个sheet
                string name = "设备状态信息检索";
                ISheet sheet = workbook.CreateSheet(name);
                //sheet.SheetName = ds.Tables[((p + 1) / 2)].Rows[0]["bill_code"].ToString();
                //设置大标题行   
                int rowCount = 0;

                //设置全局列宽和行高   
                sheet.DefaultColumnWidth = 20; //全局列宽   
                sheet.DefaultRowHeightInPoints = 15; //全局行高


                //byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\Administrator\Desktop\常用图标\意通logo.png");
                //int pictureIdx = workbook.AddPicture(bytes, PictureType.PNG);
                //HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                //HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, 1, 1);
                ////把图片插到相应的位置
                //HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);

                //设置标题行数据
                //int a = 0;
                IRow row1 = sheet.CreateRow(rowCount); //创建报表表头标题行 
                row1.Height = 300;
                //row1.RowStyle.SetFont(workbook.CreateFont().Boldweight);= FontBoldWeight.BOLD;
                ICell cell0 = row1.CreateCell(0);
                cell0.SetCellValue("设备状态检索信息");

                IRow row1_1 = sheet.CreateRow(rowCount + 1);
                row1_1.Height = 300;

                CellRangeAddress cra = new CellRangeAddress(0, 1, 0, 6);//起始行，终止行，起始列，终止列
                sheet.AddMergedRegion(cra);

                //row1.RowStyle = workbook.CreateCellStyle();
                //ICellStyle styleCommonRight = workbook.CreateCellStyle();
                cell0.CellStyle.VerticalAlignment = VerticalAlignment.CENTER;
                cell0.CellStyle.Alignment = HorizontalAlignment.CENTER;
                cell0.CellStyle.BorderBottom = BorderStyle.THIN;
                cell0.CellStyle.BorderLeft = BorderStyle.THIN;
                cell0.CellStyle.BorderRight = BorderStyle.THIN;
                cell0.CellStyle.BorderTop = BorderStyle.THIN;

                IRow row2 = sheet.CreateRow(rowCount + 2);
                row2.Height = 400;

                ICell cell301 = row2.CreateCell(0);
                cell301.SetCellValue("设备编号");

                ICell cell302 = row2.CreateCell(1);
                cell302.SetCellValue("设备名称");

                ICell cell303 = row2.CreateCell(2);
                cell303.SetCellValue("设备状态");

                ICell cell304 = row2.CreateCell(3);
                cell304.SetCellValue("开始时间");

                ICell cell305 = row2.CreateCell(4);
                cell305.SetCellValue("结束时间");

                ICell cell306 = row2.CreateCell(5);
                cell306.SetCellValue("工位编号");

                ICell cell307 = row2.CreateCell(6);
                cell307.SetCellValue("工位名称");


                //ICell cell294 = row3.CreateCell(6);
                //cell294.SetCellValue("备注");

                //ICell cell295 = row3.CreateCell(7);
                //CellRangeAddress cra2 = new CellRangeAddress(3, 3, 5, 8);//起始行，终止行，起始列，终止列
                //sheet.AddMergedRegion(cra2);


                for (int k = 0; k < ds.Tables[p].Rows.Count; k++)
                {
                    IRow row3 = sheet.CreateRow(k + rowCount + 3);//是内容行
                    row3.Height = 400;
                    int b = 0;
                    for (int j = 0; j < ds.Tables[p].Columns.Count; j++)
                    {
                        string dgvValue = string.Empty;
                        dgvValue = ds.Tables[p].Rows[k][j].ToString();
                        ICell cell = row3.CreateCell(b);
                        cell.SetCellValue(dgvValue);
                        //CellRangeAddress cari = new CellRangeAddress(k + rowCount + 4, k + rowCount + 4, 5, 8);//起始行，终止行，起始列，终止列
                        //sheet.AddMergedRegion(cari);
                        //  cell.CellStyle = style;
                        b++;
                    }
                }
                #endregion
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        #endregion

        #region//根据parm检索导出质量数据
        public static MemoryStream ExportExcel_quality_all(DataSet ds, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }

            #region 开始循环DS中的Table,DS中的每个表创建一个Sheet
            for (int p = 0; p < ds.Tables.Count; p += 2)
            {
                #region 创建一个sheet
                string name = "质量数据采集信息";
                ISheet sheet = workbook.CreateSheet(name);
                //sheet.SheetName = ds.Tables[((p + 1) / 2)].Rows[0]["bill_code"].ToString();
                //设置大标题行   
                int rowCount = 0;

                //设置全局列宽和行高   
                sheet.DefaultColumnWidth = 20; //全局列宽   
                sheet.DefaultRowHeightInPoints = 15; //全局行高


                //byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\Administrator\Desktop\常用图标\意通logo.png");
                //int pictureIdx = workbook.AddPicture(bytes, PictureType.PNG);
                //HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                //HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, 1, 1);
                ////把图片插到相应的位置
                //HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);

                //设置标题行数据
                //int a = 0;
                IRow row1 = sheet.CreateRow(rowCount); //创建报表表头标题行 
                row1.Height = 300;
                //row1.RowStyle.SetFont(workbook.CreateFont().Boldweight);= FontBoldWeight.BOLD;
                ICell cell0 = row1.CreateCell(0);
                cell0.SetCellValue("质量数据采集信息");

                IRow row1_1 = sheet.CreateRow(rowCount + 1);
                row1_1.Height = 300;

                CellRangeAddress cra = new CellRangeAddress(0, 1, 0, 9);//起始行，终止行，起始列，终止列
                sheet.AddMergedRegion(cra);

                //row1.RowStyle = workbook.CreateCellStyle();
                //ICellStyle styleCommonRight = workbook.CreateCellStyle();
                cell0.CellStyle.VerticalAlignment = VerticalAlignment.CENTER;
                cell0.CellStyle.Alignment = HorizontalAlignment.CENTER;
                cell0.CellStyle.BorderBottom = BorderStyle.THIN;
                cell0.CellStyle.BorderLeft = BorderStyle.THIN;
                cell0.CellStyle.BorderRight = BorderStyle.THIN;
                cell0.CellStyle.BorderTop = BorderStyle.THIN;



                //row2是表头行
                //IRow row2 = sheet.CreateRow(rowCount + 2);
                //row2.Height = 400;

                //ICell cell201 = row2.CreateCell(0);
                //cell201.SetCellValue("产品出生证");
                //ICell cell202 = row2.CreateCell(1);
                //cell202.SetCellValue(ds.Tables[p].Rows[0][0].ToString());

                //ICell cell203 = row2.CreateCell(2);
                //cell203.SetCellValue("产品图号");
                //ICell cell204 = row2.CreateCell(3);
                //cell204.SetCellValue(ds.Tables[p].Rows[0][1].ToString());

                //ICell cell205 = row2.CreateCell(4);
                //cell205.SetCellValue("产品名称");
                //ICell cell206 = row2.CreateCell(5);
                //cell206.SetCellValue(ds.Tables[p].Rows[0][2].ToString());

                //ICell cell207 = row2.CreateCell(6);
                //cell207.SetCellValue("产品上线时间");
                //ICell cell208 = row2.CreateCell(7);
                //cell208.SetCellValue(ds.Tables[p].Rows[0][3].ToString());

                //ICell cell209 = row2.CreateCell(8);
                //cell209.SetCellValue("产品上线时间");
                //ICell cell10 = row2.CreateCell(9);
                //cell10.SetCellValue(ds.Tables[p].Rows[0][4].ToString());
                //ICell cell11 = row2.CreateCell(4);
                //跨越合并单元格
                //CellRangeAddress cra1 = new CellRangeAddress(2, 2, 1, 3);//起始行，终止行，起始列，终止列
                //CellRangeAddress cra5 = new CellRangeAddress(2, 2, 5, 7);//起始行，终止行，起始列，终止列
                //sheet.AddMergedRegion(cra1);
                //sheet.AddMergedRegion(cra5);

                IRow row2 = sheet.CreateRow(rowCount + 2);
                row2.Height = 400;

                ICell cell301 = row2.CreateCell(0);
                cell301.SetCellValue("产品出生证");

                ICell cell302 = row2.CreateCell(1);
                cell302.SetCellValue("DCS检测项");

                ICell cell303 = row2.CreateCell(2);
                cell303.SetCellValue("Parm");

                ICell cell304 = row2.CreateCell(3);
                cell304.SetCellValue("检测项描述");

                ICell cell305 = row2.CreateCell(4);
                cell305.SetCellValue("数据值");

                ICell cell306 = row2.CreateCell(5);
                cell306.SetCellValue("单位");

                ICell cell307 = row2.CreateCell(6);
                cell307.SetCellValue("上限值");

                ICell cell308 = row2.CreateCell(7);
                cell308.SetCellValue("目标值");

                ICell cell309 = row2.CreateCell(8);
                cell309.SetCellValue("下限值");

                ICell cell310 = row2.CreateCell(9);
                cell310.SetCellValue("采集时间");

                //ICell cell294 = row3.CreateCell(6);
                //cell294.SetCellValue("备注");

                //ICell cell295 = row3.CreateCell(7);
                //CellRangeAddress cra2 = new CellRangeAddress(3, 3, 5, 8);//起始行，终止行，起始列，终止列
                //sheet.AddMergedRegion(cra2);


                for (int k = 0; k < ds.Tables[p].Rows.Count; k++)
                {
                    IRow row3 = sheet.CreateRow(k + rowCount + 3);//是内容行
                    row3.Height = 400;
                    int b = 0;
                    for (int j = 0; j < ds.Tables[p].Columns.Count; j++)
                    {
                        string dgvValue = string.Empty;
                        dgvValue = ds.Tables[p].Rows[k][j].ToString();
                        ICell cell = row3.CreateCell(b);
                        cell.SetCellValue(dgvValue);
                        //CellRangeAddress cari = new CellRangeAddress(k + rowCount + 4, k + rowCount + 4, 5, 8);//起始行，终止行，起始列，终止列
                        //sheet.AddMergedRegion(cari);
                        //  cell.CellStyle = style;
                        b++;
                    }
                }
                #endregion
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion

        #region
        public static MemoryStream ExportToExcel(DataSet ds, string excelType, string[] columnHeaders)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }

            #region 开始循环DS中的Table,DS中的每个表创建一个Sheet
            for (int p = 0; p < ds.Tables.Count; p++)
            {
                #region 创建一个sheet
                ISheet sheet = workbook.CreateSheet("sheet" + (p + 1));
                //设置大标题行   
                int rowCount = 0;


                //设置全局列宽和行高   
                sheet.DefaultColumnWidth = 14; //全局列宽   
                sheet.DefaultRowHeightInPoints = 15; //全局行高   
                //设置标题行数据   
                int a = 0;


                IRow row1 = sheet.CreateRow(rowCount); //创建报表表头标题列   
                                                       //for (int k = 0; k < ds.Tables[p].Columns.Count; k++)
                                                       //{ //将DataTable的列标题输出到Excel
                                                       //    columnName = ds.Tables[p].Columns[k].ColumnName;
                                                       //    row1.CreateCell(a).SetCellValue(columnName);
                                                       //    a++;
                                                       //}
                                                       // var style = SetCellBorder(workbook);

                for (int k = 0; k < columnHeaders.Length; k++)
                {  //将传递过来的字符串表头进行拆分到Excel

                    string columnName = columnHeaders[k];
                    ICell cell = row1.CreateCell(a);
                    cell.SetCellValue(columnName);

                    #region 设置单元格的边框
                    //    cell.CellStyle = style;
                    #endregion
                    a++;
                }

                //填写ds数据进excel   
                for (int i = 0; i < ds.Tables[p].Rows.Count; i++) //写行数据   
                {
                    IRow row2 = sheet.CreateRow(i + rowCount + 1);
                    int b = 0;
                    for (int j = 0; j < ds.Tables[p].Columns.Count; j++)
                    {
                        string dgvValue = string.Empty;
                        dgvValue = ds.Tables[p].Rows[i][j].ToString();
                        ICell cell = row2.CreateCell(b);
                        cell.SetCellValue(dgvValue);

                        #region 设置单元格的边框
                        //  cell.CellStyle = style;
                        #endregion
                        b++;
                    }
                }

                #endregion
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion

        #region GY操作Excel自定义代码
        //GY操作Excel表格自定义代码2017.05.31
        public static MemoryStream ExportDiyExcel(string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }

            #region 创建一个sheet
            ISheet sheet = workbook.CreateSheet("sheet" + (1));
            //设置大标题行   
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高
            IRow row2 = sheet.CreateRow(0);
            row2.Height = 400;
            ICell cell21 = row2.CreateCell(0);
            cell21.SetCellValue("物料编号");
            ICell cell22 = row2.CreateCell(1);
            cell22.SetCellValue("物料名称");
            ICell cell23 = row2.CreateCell(2);
            cell23.SetCellValue("物料数量");
            ICell cell25 = row2.CreateCell(3);
            cell25.SetCellValue("计量单位");
            #endregion
            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        #endregion

        #region WKL操作Excel自定义方法
        //2017.05.21由个人修改，不是公共方法
        public static MemoryStream ExportDiyExcel(DataSet ds, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }

            #region 开始循环DS中的Table,DS中的每个表创建一个Sheet
            for (int p = 1; p < ds.Tables.Count; p += 2)
            {
                #region 创建一个sheet
                string name = ds.Tables[((p - 1))].Rows[0]["bill_code"].ToString();
                ISheet sheet = workbook.CreateSheet(name);
                //sheet.SheetName = ds.Tables[((p + 1) / 2)].Rows[0]["bill_code"].ToString();
                //设置大标题行   
                int rowCount = 0;

                //设置全局列宽和行高   
                sheet.DefaultColumnWidth = 20; //全局列宽   
                sheet.DefaultRowHeightInPoints = 15; //全局行高


                byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\Administrator\Desktop\常用图标\意通logo.png");
                int pictureIdx = workbook.AddPicture(bytes, PictureType.PNG);
                HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, 1, 1);
                //把图片插到相应的位置
                HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);

                //设置标题行数据   
                //int a = 0;
                IRow row1 = sheet.CreateRow(rowCount); //创建报表表头标题列  
                row1.Height = 500;
                //row1.RowStyle.SetFont(workbook.CreateFont().Boldweight);= FontBoldWeight.BOLD;
                ICell cell0 = row1.CreateCell(0);
                cell0.SetCellValue("产品出库单");

                IRow row1_1 = sheet.CreateRow(rowCount+1);
                row1_1.Height = 500;

                CellRangeAddress cra = new CellRangeAddress(0, 1, 0, 5);//起始行，终止行，起始列，终止列
                sheet.AddMergedRegion(cra);

                //row1.RowStyle = workbook.CreateCellStyle();
                //ICellStyle styleCommonRight = workbook.CreateCellStyle();
                cell0.CellStyle.VerticalAlignment = VerticalAlignment.CENTER;
                cell0.CellStyle.Alignment = HorizontalAlignment.CENTER;
                cell0.CellStyle.BorderBottom = BorderStyle.THIN;
                cell0.CellStyle.BorderLeft = BorderStyle.THIN;
                cell0.CellStyle.BorderRight = BorderStyle.THIN;
                cell0.CellStyle.BorderTop = BorderStyle.THIN;
                //IFont fontRight = workbook.CreateFont();
                //fontRight.FontHeightInPoints = 16;
                //fontRight.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
                //fontRight.FontName = "宋体";
                //styleCommonRight.SetFont(fontRight);
                //row1.GetCell(0).CellStyle.SetFont(fontRight);




                IRow row2 = sheet.CreateRow(rowCount + 2);
                row2.Height = 400;

                ICell cell21 = row2.CreateCell(0);
                cell21.SetCellValue("单据编号");
                ICell cell22 = row2.CreateCell(1);
                cell22.SetCellValue(ds.Tables[p - 1].Rows[0][0].ToString());

                ICell cell23 = row2.CreateCell(2);
                cell23.SetCellValue("项目编号");
                ICell cell24 = row2.CreateCell(3);
                cell24.SetCellValue(ds.Tables[p - 1].Rows[0][1].ToString());

                ICell cell25 = row2.CreateCell(4);
                cell25.SetCellValue("项目名称");
                ICell cell26 = row2.CreateCell(5);
                cell26.SetCellValue(ds.Tables[p - 1].Rows[0][2].ToString());


                IRow row3 = sheet.CreateRow(rowCount + 3);
                row3.Height = 400;

                ICell cell31 = row3.CreateCell(0);
                cell31.SetCellValue("销售订单编号");
                ICell cell32 = row3.CreateCell(1);
                cell32.SetCellValue(ds.Tables[p - 1].Rows[0][3].ToString());

                ICell cell33 = row3.CreateCell(2);
                cell33.SetCellValue("出库时间");
                ICell cell34 = row3.CreateCell(3);
                cell34.SetCellValue(ds.Tables[p - 1].Rows[0][4].ToString());

                ICell cell35 = row3.CreateCell(4);
                cell35.SetCellValue("出库人");
                ICell cell36 = row3.CreateCell(5);
                cell36.SetCellValue(ds.Tables[p - 1].Rows[0][5].ToString());

                IRow row4 = sheet.CreateRow(rowCount + 4);
                row4.Height = 400;
                ICell cell41 = row4.CreateCell(0);
                cell41.SetCellValue("客户地址");
                ICell cell42 = row4.CreateCell(1);
                cell42.SetCellValue("");
                CellRangeAddress cra7 = new CellRangeAddress(4, 4, 1, 5);
                sheet.AddMergedRegion(cra7);

                IRow row5 = sheet.CreateRow(rowCount + 5);
                row5.Height = 400;
                ICell cell51 = row5.CreateCell(0);
                cell51.SetCellValue("联系人");
                ICell cell52 = row4.CreateCell(1);
                cell52.SetCellValue("");
                CellRangeAddress cra8 = new CellRangeAddress(5, 5, 1, 2);
                sheet.AddMergedRegion(cra8);

                ICell cell54 = row5.CreateCell(3);
                cell54.SetCellValue("联系电话");
                ICell cell55 = row5.CreateCell(4);
                cell55.SetCellValue("");
                CellRangeAddress cra9 = new CellRangeAddress(5, 5, 4, 5);
                sheet.AddMergedRegion(cra9);

                IRow row6 = sheet.CreateRow(rowCount + 6);
                row6.Height = 400;
                ICell cell61 = row6.CreateCell(0);
                cell61.SetCellValue("产品编号");

                ICell cell62 = row6.CreateCell(1);
                cell62.SetCellValue("产品名称");

                ICell cell63 = row6.CreateCell(2);
                cell63.SetCellValue("出库数量");

                ICell cell64 = row6.CreateCell(3);
                cell64.SetCellValue("单位");

                ICell cell65 = row6.CreateCell(4);
                cell65.SetCellValue("产品条码");

                ICell cell66 = row6.CreateCell(5);
                cell66.SetCellValue("备注");

                for (int k = 0; k < ds.Tables[p].Rows.Count; k++)
                {
                    IRow row7 = sheet.CreateRow(k + rowCount + 7);
                    row7.Height = 400;
                    int b = 0;
                    for (int j = 0; j < ds.Tables[p].Columns.Count; j++)
                    {
                        string dgvValue = string.Empty;
                        dgvValue = ds.Tables[p].Rows[k][j].ToString();
                        ICell cell = row7.CreateCell(b);
                        cell.SetCellValue(dgvValue);
                        //  cell.CellStyle = style;
                        b++;
                    }
                }
                #endregion
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }



        //2017.05.24创建，物料采购订单的信息导入，不是公共方法
        public static MemoryStream ExportModelExcel(string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            ISheet sheet = workbook.CreateSheet("sheet1");

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            //设置标题行数据
            IRow row = sheet.CreateRow(0);

            ICell cell0 = row.CreateCell(0);
            cell0.SetCellValue("项目编号");

            ICell cell1 = row.CreateCell(1);
            cell1.SetCellValue("项目名称");

            ICell cell2 = row.CreateCell(2);
            cell2.SetCellValue("采购单据编号");

            ICell cell3 = row.CreateCell(3);
            cell3.SetCellValue("下单日期");

            ICell cell4 = row.CreateCell(4);
            cell4.SetCellValue("交货期");

            ICell cell5 = row.CreateCell(5);
            cell5.SetCellValue("物料号");

            ICell cell6 = row.CreateCell(6);
            cell6.SetCellValue("物料名称");

            ICell cell7 = row.CreateCell(7);
            cell7.SetCellValue("采购数量");

            ICell cell8 = row.CreateCell(8);
            cell8.SetCellValue("计量单位");

            ICell cell9 = row.CreateCell(9);
            cell9.SetCellValue("供应商编号");

            ICell cell10 = row.CreateCell(10);
            cell10.SetCellValue("供应商名称");

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }


        //2017.05.24创建，产品销售订单的信息导入，不是公共方法
        public static MemoryStream ExportModelExcel1(string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            ISheet sheet = workbook.CreateSheet("sheet1");

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            //设置标题行数据
            IRow row = sheet.CreateRow(0);

            ICell cell0 = row.CreateCell(0);
            cell0.SetCellValue("项目编号");

            ICell cell1 = row.CreateCell(1);
            cell1.SetCellValue("项目名称");

            ICell cell2 = row.CreateCell(2);
            cell2.SetCellValue("销售单据编号");

            ICell cell3 = row.CreateCell(3);
            cell3.SetCellValue("交货期");


            ICell cell4 = row.CreateCell(4);
            cell4.SetCellValue("产品编号");

            ICell cell5 = row.CreateCell(5);
            cell5.SetCellValue("产品名称");

            ICell cell6 = row.CreateCell(6);
            cell6.SetCellValue("客户编号");

            ICell cell7 = row.CreateCell(7);
            cell7.SetCellValue("客户名称");

            ICell cell8 = row.CreateCell(8);
            cell8.SetCellValue("销售数量");

            ICell cell9 = row.CreateCell(9);
            cell9.SetCellValue("计量单位");

            ICell cell10 = row.CreateCell(10);
            cell10.SetCellValue("是否附件包（0/1）");

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }


        //2017.05.30创建，销售清单的附件包的物料信息的导入，不是公共方法
        public static MemoryStream ExportModelExcel2(string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            ISheet sheet = workbook.CreateSheet("sheet1");

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            //设置标题行数据
            IRow row = sheet.CreateRow(0);

            ICell cell0 = row.CreateCell(0);
            cell0.SetCellValue("层级关系");

            ICell cell1 = row.CreateCell(1);
            cell1.SetCellValue("物料号");

            ICell cell2 = row.CreateCell(2);
            cell2.SetCellValue("物料名称");

            ICell cell3 = row.CreateCell(3);
            cell3.SetCellValue("消耗数量");

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        //2017.06.03创建，采购订单的导出，不是公共方法
        public static MemoryStream ExportPurExcel(DataSet ds, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }

            #region 开始循环DS中的Table,DS中的每个表创建一个Sheet
            for (int p = 1; p < ds.Tables.Count; p += 2)
            {
                #region 创建一个sheet
                ISheet sheet = workbook.CreateSheet("sheet" + ((p + 1) / 2));
                //设置大标题行   
                int rowCount = 0;

                //设置全局列宽和行高   
                sheet.DefaultColumnWidth = 20; //全局列宽   
                sheet.DefaultRowHeightInPoints = 15; //全局行高


                //设置标题行数据   
                //int a = 0;
                IRow row1 = sheet.CreateRow(rowCount); //创建报表表头标题列  
                row1.Height = 800;
                //row1.RowStyle.SetFont(workbook.CreateFont().Boldweight);= FontBoldWeight.BOLD;

                ICell cell0 = row1.CreateCell(0);
                cell0.SetCellValue("采购订单");
                CellRangeAddress cra = new CellRangeAddress(0, 0, 0, 7);
                sheet.AddMergedRegion(cra);
                //row1.RowStyle = workbook.CreateCellStyle();
                //ICellStyle styleCommonRight = workbook.CreateCellStyle();
                cell0.CellStyle.VerticalAlignment = VerticalAlignment.CENTER;
                cell0.CellStyle.Alignment = HorizontalAlignment.CENTER;
                cell0.CellStyle.BorderBottom = BorderStyle.THIN;
                cell0.CellStyle.BorderLeft = BorderStyle.THIN;
                cell0.CellStyle.BorderRight = BorderStyle.THIN;
                cell0.CellStyle.BorderTop = BorderStyle.THIN;
                //IFont fontRight = workbook.CreateFont();
                //fontRight.FontHeightInPoints = 16;
                //fontRight.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
                //fontRight.FontName = "宋体";
                //styleCommonRight.SetFont(fontRight);
                //row1.GetCell(0).CellStyle.SetFont(fontRight);






                IRow row2 = sheet.CreateRow(rowCount + 1);
                row2.Height = 400;

                ICell cell21 = row2.CreateCell(0);
                cell21.SetCellValue("采购单编号：");
                ICell cell22 = row2.CreateCell(1);
                cell22.SetCellValue(ds.Tables[p - 1].Rows[0][0].ToString());

                ICell cell23 = row2.CreateCell(2);
                cell23.SetCellValue("项目编号：");
                ICell cell24 = row2.CreateCell(3);
                cell24.SetCellValue(ds.Tables[p - 1].Rows[0][1].ToString());

                ICell cell25 = row2.CreateCell(4);
                cell25.SetCellValue("项目名称：");
                ICell cell26 = row2.CreateCell(5);
                cell26.SetCellValue(ds.Tables[p - 1].Rows[0][2].ToString());

                ICell cell27 = row2.CreateCell(6);
                cell27.SetCellValue("到货时间：");
                ICell cell28 = row2.CreateCell(7);
                cell28.SetCellValue(DateTime.Now.ToShortDateString());

                IRow row3 = sheet.CreateRow(rowCount + 2);
                row3.Height = 400;
                ICell cell31 = row3.CreateCell(0);
                cell31.SetCellValue("物料编号");

                ICell cell32 = row3.CreateCell(1);
                cell32.SetCellValue("物料名称");

                ICell cell33 = row3.CreateCell(2);
                cell33.SetCellValue("到货数量");

                ICell cell34 = row3.CreateCell(3);
                cell34.SetCellValue("单位");

                ICell cell35 = row3.CreateCell(4);
                cell35.SetCellValue("物料条码");


                ICell cell36 = row3.CreateCell(5);
                cell36.SetCellValue("供应商编号");

                ICell cell37 = row3.CreateCell(6);
                cell37.SetCellValue("供应商名称");

                ICell cell38 = row3.CreateCell(7);
                cell38.SetCellValue("备注");

                for (int k = 0; k < ds.Tables[p].Rows.Count; k++)
                {
                    IRow row4 = sheet.CreateRow(k + rowCount + 3);
                    row4.Height = 400;
                    int b = 0;
                    for (int j = 0; j < ds.Tables[p].Columns.Count; j++)
                    {
                        string dgvValue = string.Empty;
                        dgvValue = ds.Tables[p].Rows[k][j].ToString();
                        ICell cell = row4.CreateCell(b);
                        cell.SetCellValue(dgvValue);
                        //  cell.CellStyle = style;
                        b++;
                    }
                }
                #endregion
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        //2017.06.14创建，下载库位信息导入的模板，不是公共方法
        public static MemoryStream ExportStorageModelExcel(string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            ISheet sheet = workbook.CreateSheet("sheet1");

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            //设置标题行数据
            IRow row = sheet.CreateRow(0);

            ICell cell0 = row.CreateCell(0);
            cell0.SetCellValue("库位编号");

            ICell cell1 = row.CreateCell(1);
            cell1.SetCellValue("库位描述");

            ICell cell2 = row.CreateCell(2);
            cell2.SetCellValue("负责人编号");

            ICell cell3 = row.CreateCell(3);
            cell3.SetCellValue("负责人姓名");

            ICell cell4 = row.CreateCell(4);
            cell4.SetCellValue("负责人电话");

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        #endregion

        #region//阀体新线质量数据
        public static MemoryStream ExportExcel_quality_VS(DataSet ds, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }

            #region 开始循环DS中的Table,DS中的每个表创建一个Sheet
            for (int p = 0; p < ds.Tables.Count; p += 2)
            {
                #region 创建一个sheet
                string name = "质量数据采集信息";
                ISheet sheet = workbook.CreateSheet(name);
                //sheet.SheetName = ds.Tables[((p + 1) / 2)].Rows[0]["bill_code"].ToString();
                //设置大标题行   
                int rowCount = 0;

                //设置全局列宽和行高   
                sheet.DefaultColumnWidth = 20; //全局列宽   
                sheet.DefaultRowHeightInPoints = 15; //全局行高


                //byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\Administrator\Desktop\常用图标\意通logo.png");
                //int pictureIdx = workbook.AddPicture(bytes, PictureType.PNG);
                //HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                //HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, 1, 1);
                ////把图片插到相应的位置
                //HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);

                //设置标题行数据
                //int a = 0;
                IRow row1 = sheet.CreateRow(rowCount); //创建报表表头标题行 
                row1.Height = 300;
                //row1.RowStyle.SetFont(workbook.CreateFont().Boldweight);= FontBoldWeight.BOLD;
                ICell cell0 = row1.CreateCell(0);
                cell0.SetCellValue("质量数据采集信息");

                IRow row1_1 = sheet.CreateRow(rowCount + 1);
                row1_1.Height = 300;

                CellRangeAddress cra = new CellRangeAddress(0, 1, 0, 15);//起始行，终止行，起始列，终止列
                sheet.AddMergedRegion(cra);

                //row1.RowStyle = workbook.CreateCellStyle();
                //ICellStyle styleCommonRight = workbook.CreateCellStyle();
                cell0.CellStyle.VerticalAlignment = VerticalAlignment.CENTER;
                cell0.CellStyle.Alignment = HorizontalAlignment.CENTER;
                cell0.CellStyle.BorderBottom = BorderStyle.THIN;
                cell0.CellStyle.BorderLeft = BorderStyle.THIN;
                cell0.CellStyle.BorderRight = BorderStyle.THIN;
                cell0.CellStyle.BorderTop = BorderStyle.THIN;



                //row2是表头行
                //IRow row2 = sheet.CreateRow(rowCount + 2);
                //row2.Height = 400;

                //ICell cell201 = row2.CreateCell(0);
                //cell201.SetCellValue("产品出生证");
                //ICell cell202 = row2.CreateCell(1);
                //cell202.SetCellValue(ds.Tables[p].Rows[0][0].ToString());

                //ICell cell203 = row2.CreateCell(2);
                //cell203.SetCellValue("产品图号");
                //ICell cell204 = row2.CreateCell(3);
                //cell204.SetCellValue(ds.Tables[p].Rows[0][1].ToString());

                //ICell cell205 = row2.CreateCell(4);
                //cell205.SetCellValue("产品名称");
                //ICell cell206 = row2.CreateCell(5);
                //cell206.SetCellValue(ds.Tables[p].Rows[0][2].ToString());

                //ICell cell207 = row2.CreateCell(6);
                //cell207.SetCellValue("产品上线时间");
                //ICell cell208 = row2.CreateCell(7);
                //cell208.SetCellValue(ds.Tables[p].Rows[0][3].ToString());

                //ICell cell209 = row2.CreateCell(8);
                //cell209.SetCellValue("产品上线时间");
                //ICell cell10 = row2.CreateCell(9);
                //cell10.SetCellValue(ds.Tables[p].Rows[0][4].ToString());
                //ICell cell11 = row2.CreateCell(4);
                //跨越合并单元格
                //CellRangeAddress cra1 = new CellRangeAddress(2, 2, 1, 3);//起始行，终止行，起始列，终止列
                //CellRangeAddress cra5 = new CellRangeAddress(2, 2, 5, 7);//起始行，终止行，起始列，终止列
                //sheet.AddMergedRegion(cra1);
                //sheet.AddMergedRegion(cra5);

                IRow row2 = sheet.CreateRow(rowCount + 2);
                row2.Height = 400;

                ICell cell301 = row2.CreateCell(0);
                cell301.SetCellValue("订单号");

                ICell cell302 = row2.CreateCell(1);
                cell302.SetCellValue("产品出生证");

                ICell cell303 = row2.CreateCell(2);
                cell303.SetCellValue("机型编号");

                ICell cell304 = row2.CreateCell(3);
                cell304.SetCellValue("机型名称");

                ICell cell305 = row2.CreateCell(4);
                cell305.SetCellValue("检测项描述");

                ICell cell306 = row2.CreateCell(5);
                cell306.SetCellValue("检测值");

                ICell cell307 = row2.CreateCell(6);
                cell307.SetCellValue("合格状态");

                ICell cell308 = row2.CreateCell(7);
                cell308.SetCellValue("单位");

                ICell cell309 = row2.CreateCell(8);
                cell309.SetCellValue("上控制线");

                ICell cell310 = row2.CreateCell(9);
                cell310.SetCellValue("目标值");

                ICell cell311 = row2.CreateCell(10);
                cell311.SetCellValue("下控制线");

                ICell cell312 = row2.CreateCell(11);
                cell312.SetCellValue("检测时间");

                ICell cell313 = row2.CreateCell(12);
                cell313.SetCellValue("产线编号");

                ICell cell314 = row2.CreateCell(13);
                cell314.SetCellValue("产线名称");

                ICell cell315 = row2.CreateCell(14);
                cell315.SetCellValue("工位编号");

                ICell cell316 = row2.CreateCell(15);
                cell316.SetCellValue("工位名称");

                //ICell cell294 = row3.CreateCell(6);
                //cell294.SetCellValue("备注");

                //ICell cell295 = row3.CreateCell(7);
                //CellRangeAddress cra2 = new CellRangeAddress(3, 3, 5, 8);//起始行，终止行，起始列，终止列
                //sheet.AddMergedRegion(cra2);


                for (int k = 0; k < ds.Tables[p].Rows.Count; k++)
                {
                    IRow row3 = sheet.CreateRow(k + rowCount + 3);//是内容行
                    row3.Height = 400;
                    int b = 0;
                    for (int j = 0; j < ds.Tables[p].Columns.Count; j++)
                    {
                        string dgvValue = string.Empty;
                        dgvValue = ds.Tables[p].Rows[k][j].ToString();
                        ICell cell = row3.CreateCell(b);
                        cell.SetCellValue(dgvValue);
                        //CellRangeAddress cari = new CellRangeAddress(k + rowCount + 4, k + rowCount + 4, 5, 8);//起始行，终止行，起始列，终止列
                        //sheet.AddMergedRegion(cari);
                        //  cell.CellStyle = style;
                        b++;
                    }
                }
                #endregion
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion

        #region 设备运行时间信息检索导出数据

        public static MemoryStream ExportExcel_Equip_Time(DataSet ds, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }

            #region 开始循环DS中的Table,DS中的每个表创建一个Sheet
            for (int p = 0; p < ds.Tables.Count; p += 2)
            {
                #region 创建一个sheet
                string name = "设备运行时间信息检索";
                ISheet sheet = workbook.CreateSheet(name);
                //sheet.SheetName = ds.Tables[((p + 1) / 2)].Rows[0]["bill_code"].ToString();
                //设置大标题行   
                int rowCount = 0;

                //设置全局列宽和行高   
                sheet.DefaultColumnWidth = 20; //全局列宽   
                sheet.DefaultRowHeightInPoints = 15; //全局行高


                //byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\Administrator\Desktop\常用图标\意通logo.png");
                //int pictureIdx = workbook.AddPicture(bytes, PictureType.PNG);
                //HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                //HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, 1, 1);
                ////把图片插到相应的位置
                //HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);

                //设置标题行数据
                //int a = 0;
                IRow row1 = sheet.CreateRow(rowCount); //创建报表表头标题行 
                row1.Height = 300;
                //row1.RowStyle.SetFont(workbook.CreateFont().Boldweight);= FontBoldWeight.BOLD;
                ICell cell0 = row1.CreateCell(0);
                cell0.SetCellValue("设备运行时间信息检索");

                IRow row1_1 = sheet.CreateRow(rowCount + 1);
                row1_1.Height = 300;

                CellRangeAddress cra = new CellRangeAddress(0, 1, 0, 10);//起始行，终止行，起始列，终止列
                sheet.AddMergedRegion(cra);

                //row1.RowStyle = workbook.CreateCellStyle();
                //ICellStyle styleCommonRight = workbook.CreateCellStyle();
                cell0.CellStyle.VerticalAlignment = VerticalAlignment.CENTER;
                cell0.CellStyle.Alignment = HorizontalAlignment.CENTER;
                cell0.CellStyle.BorderBottom = BorderStyle.THIN;
                cell0.CellStyle.BorderLeft = BorderStyle.THIN;
                cell0.CellStyle.BorderRight = BorderStyle.THIN;
                cell0.CellStyle.BorderTop = BorderStyle.THIN;

                IRow row2 = sheet.CreateRow(rowCount + 2);
                row2.Height = 400;

                ICell cell301 = row2.CreateCell(0);
                cell301.SetCellValue("设备编号");

                ICell cell302 = row2.CreateCell(1);
                cell302.SetCellValue("设备名称");

                ICell cell303 = row2.CreateCell(2);
                cell303.SetCellValue("开始时间");

                ICell cell304 = row2.CreateCell(3);
                cell304.SetCellValue("结束时间");

                ICell cell305 = row2.CreateCell(4);
                cell305.SetCellValue("持续时间(小时)");

                ICell cell306 = row2.CreateCell(5);
                cell306.SetCellValue("持续时间(分钟)");

                ICell cell307 = row2.CreateCell(6);
                cell307.SetCellValue("持续时间(秒)");

                ICell cell308 = row2.CreateCell(7);
                cell308.SetCellValue("工位编号");

                ICell cell309 = row2.CreateCell(8);
                cell309.SetCellValue("工位名称");

                ICell cell310 = row2.CreateCell(9);
                cell310.SetCellValue("产线编号");

                ICell cell311 = row2.CreateCell(10);
                cell311.SetCellValue("产线名称");


                //ICell cell294 = row3.CreateCell(6);
                //cell294.SetCellValue("备注");

                //ICell cell295 = row3.CreateCell(7);
                //CellRangeAddress cra2 = new CellRangeAddress(3, 3, 5, 8);//起始行，终止行，起始列，终止列
                //sheet.AddMergedRegion(cra2);


                for (int k = 0; k < ds.Tables[p].Rows.Count; k++)
                {
                    IRow row3 = sheet.CreateRow(k + rowCount + 3);//是内容行
                    row3.Height = 400;
                    int b = 0;
                    for (int j = 0; j < ds.Tables[p].Columns.Count; j++)
                    {
                        string dgvValue = string.Empty;
                        dgvValue = ds.Tables[p].Rows[k][j].ToString();
                        ICell cell = row3.CreateCell(b);
                        cell.SetCellValue(dgvValue);
                        //CellRangeAddress cari = new CellRangeAddress(k + rowCount + 4, k + rowCount + 4, 5, 8);//起始行，终止行，起始列，终止列
                        //sheet.AddMergedRegion(cari);
                        //  cell.CellStyle = style;
                        b++;
                    }
                }
                #endregion
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        #endregion

        #region 设备运行时间信息检索导出数据

        public static MemoryStream ExportExcel_Equip_Time_Part(DataSet ds, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }

            #region 开始循环DS中的Table,DS中的每个表创建一个Sheet
            for (int p = 0; p < ds.Tables.Count; p += 2)
            {
                #region 创建一个sheet
                string name = "设备加工产品信息检索";
                ISheet sheet = workbook.CreateSheet(name);
                //sheet.SheetName = ds.Tables[((p + 1) / 2)].Rows[0]["bill_code"].ToString();
                //设置大标题行   
                int rowCount = 0;

                //设置全局列宽和行高   
                sheet.DefaultColumnWidth = 20; //全局列宽   
                sheet.DefaultRowHeightInPoints = 15; //全局行高


                //byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\Administrator\Desktop\常用图标\意通logo.png");
                //int pictureIdx = workbook.AddPicture(bytes, PictureType.PNG);
                //HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                //HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, 1, 1);
                ////把图片插到相应的位置
                //HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);

                //设置标题行数据
                //int a = 0;
                IRow row1 = sheet.CreateRow(rowCount); //创建报表表头标题行 
                row1.Height = 300;
                //row1.RowStyle.SetFont(workbook.CreateFont().Boldweight);= FontBoldWeight.BOLD;
                ICell cell0 = row1.CreateCell(0);
                cell0.SetCellValue("设备加工产品信息检索");

                IRow row1_1 = sheet.CreateRow(rowCount + 1);
                row1_1.Height = 300;

                CellRangeAddress cra = new CellRangeAddress(0, 1, 0, 10);//起始行，终止行，起始列，终止列
                sheet.AddMergedRegion(cra);

                //row1.RowStyle = workbook.CreateCellStyle();
                //ICellStyle styleCommonRight = workbook.CreateCellStyle();
                cell0.CellStyle.VerticalAlignment = VerticalAlignment.CENTER;
                cell0.CellStyle.Alignment = HorizontalAlignment.CENTER;
                cell0.CellStyle.BorderBottom = BorderStyle.THIN;
                cell0.CellStyle.BorderLeft = BorderStyle.THIN;
                cell0.CellStyle.BorderRight = BorderStyle.THIN;
                cell0.CellStyle.BorderTop = BorderStyle.THIN;

                IRow row2 = sheet.CreateRow(rowCount + 2);
                row2.Height = 400;

                ICell cell301 = row2.CreateCell(0);
                cell301.SetCellValue("设备编号");

                ICell cell302 = row2.CreateCell(1);
                cell302.SetCellValue("设备名称");

                ICell cell303 = row2.CreateCell(2);
                cell303.SetCellValue("设备状态");

                ICell cell304 = row2.CreateCell(3);
                cell304.SetCellValue("机型编号");

                ICell cell305 = row2.CreateCell(4);
                cell305.SetCellValue("机型名称");

                ICell cell306 = row2.CreateCell(5);
                cell306.SetCellValue("产品出生证");

                ICell cell307 = row2.CreateCell(6);
                cell307.SetCellValue("开始时间");

                ICell cell308 = row2.CreateCell(7);
                cell308.SetCellValue("结束时间");

                ICell cell309 = row2.CreateCell(8);
                cell309.SetCellValue("持续时间(小时)");

                ICell cell310 = row2.CreateCell(9);
                cell310.SetCellValue("持续时间(分钟)");

                ICell cell311 = row2.CreateCell(10);
                cell311.SetCellValue("持续时间(秒)");

                ICell cell312 = row2.CreateCell(11);
                cell312.SetCellValue("工位编号");

                ICell cell313 = row2.CreateCell(12);
                cell313.SetCellValue("工位名称");

                ICell cell314 = row2.CreateCell(13);
                cell314.SetCellValue("产线编号");

                ICell cell315 = row2.CreateCell(14);
                cell315.SetCellValue("产线名称");


                //ICell cell294 = row3.CreateCell(6);
                //cell294.SetCellValue("备注");

                //ICell cell295 = row3.CreateCell(7);
                //CellRangeAddress cra2 = new CellRangeAddress(3, 3, 5, 8);//起始行，终止行，起始列，终止列
                //sheet.AddMergedRegion(cra2);


                for (int k = 0; k < ds.Tables[p].Rows.Count; k++)
                {
                    IRow row3 = sheet.CreateRow(k + rowCount + 3);//是内容行
                    row3.Height = 400;
                    int b = 0;
                    for (int j = 0; j < ds.Tables[p].Columns.Count; j++)
                    {
                        string dgvValue = string.Empty;
                        dgvValue = ds.Tables[p].Rows[k][j].ToString();
                        ICell cell = row3.CreateCell(b);
                        cell.SetCellValue(dgvValue);
                        //CellRangeAddress cari = new CellRangeAddress(k + rowCount + 4, k + rowCount + 4, 5, 8);//起始行，终止行，起始列，终止列
                        //sheet.AddMergedRegion(cari);
                        //  cell.CellStyle = style;
                        b++;
                    }
                }
                #endregion
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        #endregion

        #region//壳体机加上线毛坯码信息 
        public static MemoryStream ExportExcel_GH_CH(DataSet ds, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }

            #region 开始循环DS中的Table,DS中的每个表创建一个Sheet
            for (int p = 0; p < ds.Tables.Count; p += 2)
            {
                #region 创建一个sheet
                string name = "机加壳体毛坯码上线信息";
                ISheet sheet = workbook.CreateSheet(name);
                //sheet.SheetName = ds.Tables[((p + 1) / 2)].Rows[0]["bill_code"].ToString();
                //设置大标题行
                int rowCount = 0;

                //设置全局列宽和行高   
                sheet.DefaultColumnWidth = 20; //全局列宽   
                sheet.DefaultRowHeightInPoints = 15; //全局行高


                //byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\Administrator\Desktop\常用图标\意通logo.png");
                //int pictureIdx = workbook.AddPicture(bytes, PictureType.PNG);
                //HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                //HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, 1, 1);
                ////把图片插到相应的位置
                //HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);

                //设置标题行数据
                //int a = 0;
                IRow row1 = sheet.CreateRow(rowCount); //创建报表表头标题行 
                row1.Height = 300;
                //row1.RowStyle.SetFont(workbook.CreateFont().Boldweight);= FontBoldWeight.BOLD;
                ICell cell0 = row1.CreateCell(0);
                cell0.SetCellValue("机加壳体毛坯码上线信息");

                IRow row1_1 = sheet.CreateRow(rowCount + 1);
                row1_1.Height = 300;

                CellRangeAddress cra = new CellRangeAddress(0, 1, 0, 6);//起始行，终止行，起始列，终止列
                sheet.AddMergedRegion(cra);

                //row1.RowStyle = workbook.CreateCellStyle();
                //ICellStyle styleCommonRight = workbook.CreateCellStyle();
                cell0.CellStyle.VerticalAlignment = VerticalAlignment.CENTER;
                cell0.CellStyle.Alignment = HorizontalAlignment.CENTER;
                cell0.CellStyle.BorderBottom = BorderStyle.THIN;
                cell0.CellStyle.BorderLeft = BorderStyle.THIN;
                cell0.CellStyle.BorderRight = BorderStyle.THIN;
                cell0.CellStyle.BorderTop = BorderStyle.THIN;


                IRow row2 = sheet.CreateRow(rowCount + 2);
                row2.Height = 400;

                ICell cell301 = row2.CreateCell(0);
                cell301.SetCellValue("毛坯码");

                ICell cell302 = row2.CreateCell(1);
                cell302.SetCellValue("上线时间");

                ICell cell303 = row2.CreateCell(2);
                cell303.SetCellValue("机型编号");

                ICell cell304 = row2.CreateCell(3);
                cell304.SetCellValue("订单编号");

                ICell cell305 = row2.CreateCell(4);
                cell305.SetCellValue("计划时间");

                ICell cell306 = row2.CreateCell(5);
                cell306.SetCellValue("计划数量");

                ICell cell307 = row2.CreateCell(6);
                cell307.SetCellValue("合格状态");

                for (int k = 0; k < ds.Tables[p].Rows.Count; k++)
                {
                    IRow row3 = sheet.CreateRow(k + rowCount + 3);//是内容行
                    row3.Height = 400;
                    int b = 0;
                    for (int j = 0; j < ds.Tables[p].Columns.Count; j++)
                    {
                        string dgvValue = string.Empty;
                        dgvValue = ds.Tables[p].Rows[k][j].ToString();
                        ICell cell = row3.CreateCell(b);
                        cell.SetCellValue(dgvValue);
                        //CellRangeAddress cari = new CellRangeAddress(k + rowCount + 4, k + rowCount + 4, 5, 8);//起始行，终止行，起始列，终止列
                        //sheet.AddMergedRegion(cari);
                        //  cell.CellStyle = style;
                        b++;
                    }
                }
                #endregion
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion

        #region//产品出生证查质量数据导出
        public static MemoryStream ExportExcel_quality_pro(DataSet ds, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }

            #region 开始循环DS中的Table,DS中的每个表创建一个Sheet
            for (int p = 0; p < ds.Tables.Count; p += 2)
            {
                #region 创建一个sheet
                string name = "质量数据采集信息";
                ISheet sheet = workbook.CreateSheet(name);
                //sheet.SheetName = ds.Tables[((p + 1) / 2)].Rows[0]["bill_code"].ToString();
                //设置大标题行   
                int rowCount = 0;

                //设置全局列宽和行高   
                sheet.DefaultColumnWidth = 20; //全局列宽   
                sheet.DefaultRowHeightInPoints = 15; //全局行高


                //byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\Administrator\Desktop\常用图标\意通logo.png");
                //int pictureIdx = workbook.AddPicture(bytes, PictureType.PNG);
                //HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                //HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, 1, 1);
                ////把图片插到相应的位置
                //HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);

                //设置标题行数据
                //int a = 0;
                IRow row1 = sheet.CreateRow(rowCount); //创建报表表头标题行 
                row1.Height = 300;
                //row1.RowStyle.SetFont(workbook.CreateFont().Boldweight);= FontBoldWeight.BOLD;
                ICell cell0 = row1.CreateCell(0);
                cell0.SetCellValue("质量数据采集信息");

                IRow row1_1 = sheet.CreateRow(rowCount + 1);
                row1_1.Height = 300;

                CellRangeAddress cra = new CellRangeAddress(0, 1, 0, 9);//起始行，终止行，起始列，终止列
                sheet.AddMergedRegion(cra);

                //row1.RowStyle = workbook.CreateCellStyle();
                //ICellStyle styleCommonRight = workbook.CreateCellStyle();
                cell0.CellStyle.VerticalAlignment = VerticalAlignment.CENTER;
                cell0.CellStyle.Alignment = HorizontalAlignment.CENTER;
                cell0.CellStyle.BorderBottom = BorderStyle.THIN;
                cell0.CellStyle.BorderLeft = BorderStyle.THIN;
                cell0.CellStyle.BorderRight = BorderStyle.THIN;
                cell0.CellStyle.BorderTop = BorderStyle.THIN;



                //row2是表头行
                //IRow row2 = sheet.CreateRow(rowCount + 2);
                //row2.Height = 400;

                //ICell cell201 = row2.CreateCell(0);
                //cell201.SetCellValue("产品出生证");
                //ICell cell202 = row2.CreateCell(1);
                //cell202.SetCellValue(ds.Tables[p].Rows[0][0].ToString());

                //ICell cell203 = row2.CreateCell(2);
                //cell203.SetCellValue("产品图号");
                //ICell cell204 = row2.CreateCell(3);
                //cell204.SetCellValue(ds.Tables[p].Rows[0][1].ToString());

                //ICell cell205 = row2.CreateCell(4);
                //cell205.SetCellValue("产品名称");
                //ICell cell206 = row2.CreateCell(5);
                //cell206.SetCellValue(ds.Tables[p].Rows[0][2].ToString());

                //ICell cell207 = row2.CreateCell(6);
                //cell207.SetCellValue("产品上线时间");
                //ICell cell208 = row2.CreateCell(7);
                //cell208.SetCellValue(ds.Tables[p].Rows[0][3].ToString());

                //ICell cell209 = row2.CreateCell(8);
                //cell209.SetCellValue("产品上线时间");
                //ICell cell10 = row2.CreateCell(9);
                //cell10.SetCellValue(ds.Tables[p].Rows[0][4].ToString());
                //ICell cell11 = row2.CreateCell(4);
                //跨越合并单元格
                //CellRangeAddress cra1 = new CellRangeAddress(2, 2, 1, 3);//起始行，终止行，起始列，终止列
                //CellRangeAddress cra5 = new CellRangeAddress(2, 2, 5, 7);//起始行，终止行，起始列，终止列
                //sheet.AddMergedRegion(cra1);
                //sheet.AddMergedRegion(cra5);

                IRow row2 = sheet.CreateRow(rowCount + 2);
                row2.Height = 400;

                ICell cell301 = row2.CreateCell(0);
                cell301.SetCellValue("产品出生证");

                ICell cell302 = row2.CreateCell(1);
                cell302.SetCellValue("DCS");

                ICell cell303 = row2.CreateCell(2);
                cell303.SetCellValue("Parm");

                ICell cell304 = row2.CreateCell(3);
                cell304.SetCellValue("检测项描述");

                ICell cell305 = row2.CreateCell(4);
                cell305.SetCellValue("数据值");

                ICell cell306 = row2.CreateCell(5);
                cell306.SetCellValue("单位");

                ICell cell307 = row2.CreateCell(6);
                cell307.SetCellValue("目标值");

                ICell cell308 = row2.CreateCell(7);
                cell308.SetCellValue("上限值");

                ICell cell309 = row2.CreateCell(8);
                cell309.SetCellValue("下限值");

                ICell cell310 = row2.CreateCell(9);
                cell310.SetCellValue("采集时间");


                //ICell cell294 = row3.CreateCell(6);
                //cell294.SetCellValue("备注");

                //ICell cell295 = row3.CreateCell(7);
                //CellRangeAddress cra2 = new CellRangeAddress(3, 3, 5, 8);//起始行，终止行，起始列，终止列
                //sheet.AddMergedRegion(cra2);


                for (int k = 0; k < ds.Tables[p].Rows.Count; k++)
                {
                    IRow row3 = sheet.CreateRow(k + rowCount + 3);//是内容行
                    row3.Height = 400;
                    int b = 0;
                    for (int j = 0; j < ds.Tables[p].Columns.Count; j++)
                    {
                        string dgvValue = string.Empty;
                        dgvValue = ds.Tables[p].Rows[k][j].ToString();
                        ICell cell = row3.CreateCell(b);
                        cell.SetCellValue(dgvValue);
                        //CellRangeAddress cari = new CellRangeAddress(k + rowCount + 4, k + rowCount + 4, 5, 8);//起始行，终止行，起始列，终止列
                        //sheet.AddMergedRegion(cari);
                        //  cell.CellStyle = style;
                        b++;
                    }
                }
                #endregion
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion

        #region//马波斯数据导出
        public static MemoryStream ExportExcel_Marposs(DataSet ds, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }

            #region 开始循环DS中的Table,DS中的每个表创建一个Sheet
            for (int p = 0; p < ds.Tables.Count; p += 2)
            {
                #region 创建一个sheet
                string name = "过点信息";
                ISheet sheet = workbook.CreateSheet(name);
                int rowCount = 0;
                //设置全局列宽和行高   
                sheet.DefaultColumnWidth = 20; //全局列宽   
                sheet.DefaultRowHeightInPoints = 15; //全局行高
                IRow row1 = sheet.CreateRow(rowCount); //创建报表表头标题行 
                row1.Height = 300;
                ICell cell0 = row1.CreateCell(0);
                cell0.SetCellValue("过点信息");

                IRow row1_1 = sheet.CreateRow(rowCount + 1);
                row1_1.Height = 300;
                CellRangeAddress cra = new CellRangeAddress(0, 1, 0, 10);//起始行，终止行，起始列，终止列
                sheet.AddMergedRegion(cra);
                cell0.CellStyle.VerticalAlignment = VerticalAlignment.CENTER;
                cell0.CellStyle.Alignment = HorizontalAlignment.CENTER;
                cell0.CellStyle.BorderBottom = BorderStyle.THIN;
                cell0.CellStyle.BorderLeft = BorderStyle.THIN;
                cell0.CellStyle.BorderRight = BorderStyle.THIN;
                cell0.CellStyle.BorderTop = BorderStyle.THIN;

                IRow row2 = sheet.CreateRow(rowCount + 2);
                row2.Height = 400;

                ICell cell301 = row2.CreateCell(0);
                cell301.SetCellValue("产品出生证");

                ICell cell302 = row2.CreateCell(1);
                cell302.SetCellValue("DCS");

                ICell cell303 = row2.CreateCell(2);
                cell303.SetCellValue("Parm");

                ICell cell304 = row2.CreateCell(3);
                cell304.SetCellValue("检测项描述");

                ICell cell305 = row2.CreateCell(4);
                cell305.SetCellValue("检测值");

                ICell cell306 = row2.CreateCell(5);
                cell306.SetCellValue("是否合格");

                ICell cell307 = row2.CreateCell(6);
                cell307.SetCellValue("单位");

                ICell cell308 = row2.CreateCell(7);
                cell308.SetCellValue("上限值");

                ICell cell309 = row2.CreateCell(8);
                cell309.SetCellValue("目标值");

                ICell cell310 = row2.CreateCell(9);
                cell310.SetCellValue("下限值");

                ICell cell311 = row2.CreateCell(10);
                cell311.SetCellValue("检测时间");

                for (int k = 0; k < ds.Tables[p].Rows.Count; k++)
                {
                    IRow row3 = sheet.CreateRow(k + rowCount + 3);//是内容行
                    row3.Height = 400;
                    int b = 0;
                    for (int j = 0; j < ds.Tables[p].Columns.Count; j++)
                    {
                        string dgvValue = string.Empty;
                        dgvValue = ds.Tables[p].Rows[k][j].ToString();
                        ICell cell = row3.CreateCell(b);
                        cell.SetCellValue(dgvValue);
                        //CellRangeAddress cari = new CellRangeAddress(k + rowCount + 4, k + rowCount + 4, 5, 8);//起始行，终止行，起始列，终止列
                        //sheet.AddMergedRegion(cari);
                        //  cell.CellStyle = style;
                        b++;
                    }
                }
                #endregion
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion

        #region 设备报警信息检索导出数据

        public static MemoryStream ExportExcel_Equip_Alarm(DataSet ds, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }

            #region 开始循环DS中的Table,DS中的每个表创建一个Sheet
            for (int p = 0; p < ds.Tables.Count; p += 2)
            {
                #region 创建一个sheet
                string name = "设备报警信息检索";
                ISheet sheet = workbook.CreateSheet(name);
                //sheet.SheetName = ds.Tables[((p + 1) / 2)].Rows[0]["bill_code"].ToString();
                //设置大标题行   
                int rowCount = 0;

                //设置全局列宽和行高   
                sheet.DefaultColumnWidth = 20; //全局列宽   
                sheet.DefaultRowHeightInPoints = 15; //全局行高


                //byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\Administrator\Desktop\常用图标\意通logo.png");
                //int pictureIdx = workbook.AddPicture(bytes, PictureType.PNG);
                //HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                //HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, 1, 1);
                ////把图片插到相应的位置
                //HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);

                //设置标题行数据
                //int a = 0;
                IRow row1 = sheet.CreateRow(rowCount); //创建报表表头标题行 
                row1.Height = 300;
                //row1.RowStyle.SetFont(workbook.CreateFont().Boldweight);= FontBoldWeight.BOLD;
                ICell cell0 = row1.CreateCell(0);
                cell0.SetCellValue("设备状态检索信息");

                IRow row1_1 = sheet.CreateRow(rowCount + 1);
                row1_1.Height = 300;

                CellRangeAddress cra = new CellRangeAddress(0, 1, 0, 9);//起始行，终止行，起始列，终止列
                sheet.AddMergedRegion(cra);

                //row1.RowStyle = workbook.CreateCellStyle();
                //ICellStyle styleCommonRight = workbook.CreateCellStyle();
                cell0.CellStyle.VerticalAlignment = VerticalAlignment.CENTER;
                cell0.CellStyle.Alignment = HorizontalAlignment.CENTER;
                cell0.CellStyle.BorderBottom = BorderStyle.THIN;
                cell0.CellStyle.BorderLeft = BorderStyle.THIN;
                cell0.CellStyle.BorderRight = BorderStyle.THIN;
                cell0.CellStyle.BorderTop = BorderStyle.THIN;

                IRow row2 = sheet.CreateRow(rowCount + 2);
                row2.Height = 400;

                ICell cell301 = row2.CreateCell(0);
                cell301.SetCellValue("设备编号");

                ICell cell302 = row2.CreateCell(1);
                cell302.SetCellValue("设备名称");

                ICell cell303 = row2.CreateCell(2);
                cell303.SetCellValue("报警编号");

                ICell cell304 = row2.CreateCell(3);
                cell304.SetCellValue("报警描述");

                ICell cell305 = row2.CreateCell(4);
                cell305.SetCellValue("报警等级");

                ICell cell306 = row2.CreateCell(5);
                cell306.SetCellValue("报警开始时间");

                ICell cell307 = row2.CreateCell(6);
                cell307.SetCellValue("报警结束时间");

                ICell cell308 = row2.CreateCell(7);
                cell308.SetCellValue("报警持续时间（h）");

                ICell cell309 = row2.CreateCell(8);
                cell309.SetCellValue("报警持续时间（m）");

                ICell cell310 = row2.CreateCell(9);
                cell310.SetCellValue("报警持续时间（s）");

                //ICell cell294 = row3.CreateCell(6);
                //cell294.SetCellValue("备注");

                //ICell cell295 = row3.CreateCell(7);
                //CellRangeAddress cra2 = new CellRangeAddress(3, 3, 5, 8);//起始行，终止行，起始列，终止列
                //sheet.AddMergedRegion(cra2);


                for (int k = 0; k < ds.Tables[p].Rows.Count; k++)
                {
                    IRow row3 = sheet.CreateRow(k + rowCount + 3);//是内容行
                    row3.Height = 400;
                    int b = 0;
                    for (int j = 0; j < ds.Tables[p].Columns.Count; j++)
                    {
                        string dgvValue = string.Empty;
                        dgvValue = ds.Tables[p].Rows[k][j].ToString();
                        ICell cell = row3.CreateCell(b);
                        cell.SetCellValue(dgvValue);
                        //CellRangeAddress cari = new CellRangeAddress(k + rowCount + 4, k + rowCount + 4, 5, 8);//起始行，终止行，起始列，终止列
                        //sheet.AddMergedRegion(cari);
                        //  cell.CellStyle = style;
                        b++;
                    }
                }
                #endregion
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        #endregion

        #region 生产报警信息检索导出数据

        public static MemoryStream ExportExcel_Alarm(DataSet ds, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }

            #region 开始循环DS中的Table,DS中的每个表创建一个Sheet
            for (int p = 0; p < ds.Tables.Count; p += 2)
            {
                #region 创建一个sheet
                string name = "设备报警信息检索";
                ISheet sheet = workbook.CreateSheet(name);
                //sheet.SheetName = ds.Tables[((p + 1) / 2)].Rows[0]["bill_code"].ToString();
                //设置大标题行   
                int rowCount = 0;

                //设置全局列宽和行高   
                sheet.DefaultColumnWidth = 20; //全局列宽   
                sheet.DefaultRowHeightInPoints = 15; //全局行高


                //byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\Administrator\Desktop\常用图标\意通logo.png");
                //int pictureIdx = workbook.AddPicture(bytes, PictureType.PNG);
                //HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                //HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, 1, 1);
                ////把图片插到相应的位置
                //HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);

                //设置标题行数据
                //int a = 0;
                IRow row1 = sheet.CreateRow(rowCount); //创建报表表头标题行 
                row1.Height = 300;
                //row1.RowStyle.SetFont(workbook.CreateFont().Boldweight);= FontBoldWeight.BOLD;
                ICell cell0 = row1.CreateCell(0);
                cell0.SetCellValue("设备状态检索信息");

                IRow row1_1 = sheet.CreateRow(rowCount + 1);
                row1_1.Height = 300;

                CellRangeAddress cra = new CellRangeAddress(0, 1, 0, 7);//起始行，终止行，起始列，终止列
                sheet.AddMergedRegion(cra);

                //row1.RowStyle = workbook.CreateCellStyle();
                //ICellStyle styleCommonRight = workbook.CreateCellStyle();
                cell0.CellStyle.VerticalAlignment = VerticalAlignment.CENTER;
                cell0.CellStyle.Alignment = HorizontalAlignment.CENTER;
                cell0.CellStyle.BorderBottom = BorderStyle.THIN;
                cell0.CellStyle.BorderLeft = BorderStyle.THIN;
                cell0.CellStyle.BorderRight = BorderStyle.THIN;
                cell0.CellStyle.BorderTop = BorderStyle.THIN;

                IRow row2 = sheet.CreateRow(rowCount + 2);
                row2.Height = 400;

                ICell cell301 = row2.CreateCell(0);
                cell301.SetCellValue("工位编号");

                ICell cell302 = row2.CreateCell(1);
                cell302.SetCellValue("工位名称");

                ICell cell303 = row2.CreateCell(2);
                cell303.SetCellValue("报警类别");

                ICell cell304 = row2.CreateCell(3);
                cell304.SetCellValue("报警描述");
                

                ICell cell305 = row2.CreateCell(4);
                cell305.SetCellValue("报警开始时间");

                ICell cell306 = row2.CreateCell(5);
                cell306.SetCellValue("报警结束时间");

                ICell cell307 = row2.CreateCell(6);
                cell307.SetCellValue("报警持续时间（s）");
                

                //ICell cell294 = row3.CreateCell(6);
                //cell294.SetCellValue("备注");

                //ICell cell295 = row3.CreateCell(7);
                //CellRangeAddress cra2 = new CellRangeAddress(3, 3, 5, 8);//起始行，终止行，起始列，终止列
                //sheet.AddMergedRegion(cra2);


                for (int k = 0; k < ds.Tables[p].Rows.Count; k++)
                {
                    IRow row3 = sheet.CreateRow(k + rowCount + 3);//是内容行
                    row3.Height = 400;
                    int b = 0;
                    for (int j = 0; j < ds.Tables[p].Columns.Count; j++)
                    {
                        string dgvValue = string.Empty;
                        dgvValue = ds.Tables[p].Rows[k][j].ToString();
                        ICell cell = row3.CreateCell(b);
                        cell.SetCellValue(dgvValue);
                        //CellRangeAddress cari = new CellRangeAddress(k + rowCount + 4, k + rowCount + 4, 5, 8);//起始行，终止行，起始列，终止列
                        //sheet.AddMergedRegion(cari);
                        //  cell.CellStyle = style;
                        b++;
                    }
                }
                #endregion
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        #endregion

        #region 生产Andon信息检索导出数据

        public static MemoryStream ExportExcel_Andon(DataSet ds, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }

            #region 开始循环DS中的Table,DS中的每个表创建一个Sheet
            for (int p = 0; p < ds.Tables.Count; p += 2)
            {
                #region 创建一个sheet
                string name = "设备报警信息检索";
                ISheet sheet = workbook.CreateSheet(name);
                //sheet.SheetName = ds.Tables[((p + 1) / 2)].Rows[0]["bill_code"].ToString();
                //设置大标题行   
                int rowCount = 0;

                //设置全局列宽和行高   
                sheet.DefaultColumnWidth = 20; //全局列宽   
                sheet.DefaultRowHeightInPoints = 15; //全局行高


                //byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\Administrator\Desktop\常用图标\意通logo.png");
                //int pictureIdx = workbook.AddPicture(bytes, PictureType.PNG);
                //HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                //HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, 1, 1);
                ////把图片插到相应的位置
                //HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);

                //设置标题行数据
                //int a = 0;
                IRow row1 = sheet.CreateRow(rowCount); //创建报表表头标题行 
                row1.Height = 300;
                //row1.RowStyle.SetFont(workbook.CreateFont().Boldweight);= FontBoldWeight.BOLD;
                ICell cell0 = row1.CreateCell(0);
                cell0.SetCellValue("设备状态检索信息");

                IRow row1_1 = sheet.CreateRow(rowCount + 1);
                row1_1.Height = 300;

                CellRangeAddress cra = new CellRangeAddress(0, 1, 0, 10);//起始行，终止行，起始列，终止列
                sheet.AddMergedRegion(cra);

                //row1.RowStyle = workbook.CreateCellStyle();
                //ICellStyle styleCommonRight = workbook.CreateCellStyle();
                cell0.CellStyle.VerticalAlignment = VerticalAlignment.CENTER;
                cell0.CellStyle.Alignment = HorizontalAlignment.CENTER;
                cell0.CellStyle.BorderBottom = BorderStyle.THIN;
                cell0.CellStyle.BorderLeft = BorderStyle.THIN;
                cell0.CellStyle.BorderRight = BorderStyle.THIN;
                cell0.CellStyle.BorderTop = BorderStyle.THIN;

                IRow row2 = sheet.CreateRow(rowCount + 2);
                row2.Height = 400;

                ICell cell301 = row2.CreateCell(0);
                cell301.SetCellValue("工位编号");

                ICell cell302 = row2.CreateCell(1);
                cell302.SetCellValue("工位名称");

                ICell cell303 = row2.CreateCell(2);
                cell303.SetCellValue("Andon类别");

                ICell cell304 = row2.CreateCell(3);
                cell304.SetCellValue("Andon名称");

                ICell cell305 = row2.CreateCell(4);
                cell305.SetCellValue("Andon描述");

                ICell cell306 = row2.CreateCell(5);
                cell306.SetCellValue("Andon开始时间");

                ICell cell307 = row2.CreateCell(6);
                cell307.SetCellValue("Andon结束时间");

                ICell cell308 = row2.CreateCell(7);
                cell308.SetCellValue("Andon持续时间（s）");

                ICell cell309 = row2.CreateCell(8);
                cell309.SetCellValue("物料编号");

                ICell cell310 = row2.CreateCell(9);
                cell310.SetCellValue("物料名称");

                ICell cell311 = row2.CreateCell(10);
                cell311.SetCellValue("Andon数量");


                //ICell cell294 = row3.CreateCell(6);
                //cell294.SetCellValue("备注");

                //ICell cell295 = row3.CreateCell(7);
                //CellRangeAddress cra2 = new CellRangeAddress(3, 3, 5, 8);//起始行，终止行，起始列，终止列
                //sheet.AddMergedRegion(cra2);


                for (int k = 0; k < ds.Tables[p].Rows.Count; k++)
                {
                    IRow row3 = sheet.CreateRow(k + rowCount + 3);//是内容行
                    row3.Height = 400;
                    int b = 0;
                    for (int j = 0; j < ds.Tables[p].Columns.Count; j++)
                    {
                        string dgvValue = string.Empty;
                        dgvValue = ds.Tables[p].Rows[k][j].ToString();
                        ICell cell = row3.CreateCell(b);
                        cell.SetCellValue(dgvValue);
                        //CellRangeAddress cari = new CellRangeAddress(k + rowCount + 4, k + rowCount + 4, 5, 8);//起始行，终止行，起始列，终止列
                        //sheet.AddMergedRegion(cari);
                        //  cell.CellStyle = style;
                        b++;
                    }
                }
                #endregion
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        #endregion


        #region 总装系统导出
        #region 1.AVI站点信息导出
        /// <summary>
        /// AVI站点信息导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_AVI(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "AVI站点信息数据";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("AVI站点编号");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("AVI站点名称");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("AVI站点类型");

            ICell cell301 = row2.CreateCell(3);
            cell301.SetCellValue("工序编号");

            ICell cell302 = row2.CreateCell(4);
            cell302.SetCellValue("工序名称");

            ICell cell303 = row2.CreateCell(5);
            cell303.SetCellValue("AVI站点顺序");

            ICell cell305 = row2.CreateCell(6);
            cell305.SetCellValue("是否报工");

            ICell cell306 = row2.CreateCell(7);
            cell306.SetCellValue("AVI描述");

            ICell cell307 = row2.CreateCell(8);
            cell307.SetCellValue("创建时间");

            ICell cell308 = row2.CreateCell(9);
            cell308.SetCellValue("创建人名称");

            ICell cell401 = row2.CreateCell(10);
            cell401.SetCellValue("修改时间");

            ICell cell203 = row2.CreateCell(11);
            cell203.SetCellValue("修改人名称");

            ICell cell309 = row2.CreateCell(12);
            cell309.SetCellValue("备注");
            



            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();
                    if (j == 2)
                    {
                        if (dt.Rows[k][j].ToString() == "PaintFinalTransPoint")
                        {
                            dgvValue = "涂总转接点";
                        }
                        else if (dt.Rows[k][j].ToString() == "StrongInteraction")
                        {
                            dgvValue = "强交互站点";
                        }
                        else if (dt.Rows[k][j].ToString() == "WeakInteraction")
                        {
                            dgvValue = "弱交互站点";
                        }
                        else if (dt.Rows[k][j].ToString() == "QualityMonitoring")
                        {
                            dgvValue = "质量校核AVI站点";
                        }
                        else
                        {
                            dgvValue = "车身调度AVI站点";
                        }
                    }
                    else if (j == 6)
                    {
                        if (dt.Rows[k][j].ToString() == "0")
                        {
                            dgvValue = "否";
                        }
                        else if (dt.Rows[k][j].ToString() == "1")
                        {
                            dgvValue = "是";
                        }
                        else if (dt.Rows[k][j].ToString() == "&nbsp;")
                        {
                            dgvValue = "";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    else
                    {
                        if (dt.Rows[k][j].ToString() == "&nbsp;")
                        {
                            dgvValue = "";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 2.工位信息导出-质控点
        /// <summary>
        /// 质控点信息导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_Wc2(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "质控点信息数据";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("质控点编号");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("质控点名称");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("产线编号");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("产线名称");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("质控点描述");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("创建时间");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("创建人编号");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("创建人名称");

            ICell cell208 = row2.CreateCell(8);
            cell208.SetCellValue("修改时间");

            ICell cell209 = row2.CreateCell(9);
            cell209.SetCellValue("修改人编号");

            ICell cell210 = row2.CreateCell(10);
            cell210.SetCellValue("修改人名称");

            ICell cell211 = row2.CreateCell(11);
            cell211.SetCellValue("备注");

            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();
                    if (dt.Rows[k][j].ToString() == "&nbsp;")
                    {
                        dgvValue = "";
                    }
                    else
                    {
                        dgvValue = dt.Rows[k][j].ToString();
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 3.岗位信息导出
        /// <summary>
        /// AVI站点信息导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_Post(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "岗位信息数据";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("岗位编号");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("岗位名称");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("岗位类型");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("工位编号");

            ICell cell301 = row2.CreateCell(4);
            cell301.SetCellValue("工位名称");

            ICell cell302 = row2.CreateCell(5);
            cell302.SetCellValue("工位类型");

            ICell cell303 = row2.CreateCell(6);
            cell303.SetCellValue("岗位位置");

            ICell cell305 = row2.CreateCell(7);
            cell305.SetCellValue("创建时间");

            ICell cell306 = row2.CreateCell(8);
            cell306.SetCellValue("创建人编号");

            ICell cell307 = row2.CreateCell(9);
            cell307.SetCellValue("创建人名称");

            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();
                    if (j == 6)
                    {
                        if (dt.Rows[k][j].ToString() == "&nbsp;")
                        {
                            dgvValue = "";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    else
                    {
                        dgvValue = dt.Rows[k][j].ToString();
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 4.设备信息导出
        /// <summary>
        /// 设备信息导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_Dvc(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "设备信息数据";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;


            ICell cell201 = row2.CreateCell(0);
            cell201.SetCellValue("设备编号");

            ICell cell202 = row2.CreateCell(1);
            cell202.SetCellValue("设备名称");

            ICell cell200 = row2.CreateCell(2);
            cell200.SetCellValue("机构级别");

            ICell cell204 = row2.CreateCell(3);
            cell204.SetCellValue("机构名称");

            ICell cell205 = row2.CreateCell(4);
            cell205.SetCellValue("设备位置");

            ICell cell203 = row2.CreateCell(5);
            cell203.SetCellValue("IP地址");

            ICell cell301 = row2.CreateCell(6);
            cell301.SetCellValue("设备类别");

            ICell cell303 = row2.CreateCell(7);
            cell303.SetCellValue("设备类型");

            ICell cell305 = row2.CreateCell(8);
            cell305.SetCellValue("创建时间");

            ICell cell306 = row2.CreateCell(9);
            cell306.SetCellValue("创建人名称");

            ICell cell307 = row2.CreateCell(10);
            cell307.SetCellValue("修改时间");

            ICell cell302 = row2.CreateCell(11);
            cell302.SetCellValue("修改人名称");

            ICell cell401 = row2.CreateCell(12);
            cell401.SetCellValue("备注");

            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();
                    if (dt.Rows[k][j].ToString() == "&nbsp;")
                    {
                        dgvValue = "";
                    }
                    else
                    {
                        dgvValue = dt.Rows[k][j].ToString();
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 5.部门信息导出
        /// <summary>
        /// AVI站点信息导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_Department(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "部门信息数据";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("部门编号");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("部门名称");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("公司编号");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("公司名称");

            ICell cell301 = row2.CreateCell(4);
            cell301.SetCellValue("负责人员姓名");

            ICell cell302 = row2.CreateCell(5);
            cell302.SetCellValue("负责人手机号");

            ICell cell303 = row2.CreateCell(6);
            cell303.SetCellValue("部门描述");

            ICell cell305 = row2.CreateCell(7);
            cell305.SetCellValue("创建时间");

            ICell cell306 = row2.CreateCell(8);
            cell306.SetCellValue("创建人名称");

            ICell cell307 = row2.CreateCell(9);
            cell307.SetCellValue("修改时间");

            ICell cell308 = row2.CreateCell(10);
            cell308.SetCellValue("修改人名称");

            ICell cell309 = row2.CreateCell(11);
            cell309.SetCellValue("备注");
            

            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();
                    if (dt.Rows[k][j].ToString() == "&nbsp;")
                    {
                        dgvValue = "";
                    }
                    else
                    {
                        dgvValue = dt.Rows[k][j].ToString();
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 6.人员信息导出
        /// <summary>
        /// AVI站点信息导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_Stf(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "人员信息数据";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("人员编号");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("人员姓名");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("部门编号");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("部门名称");

            ICell cell301 = row2.CreateCell(4);
            cell301.SetCellValue("性别");

            ICell cell302 = row2.CreateCell(5);
            cell302.SetCellValue("手机号");

            ICell cell303 = row2.CreateCell(6);
            cell303.SetCellValue("企业微信号");

            ICell cell305 = row2.CreateCell(7);
            cell305.SetCellValue("邮箱");

            ICell cell306 = row2.CreateCell(8);
            cell306.SetCellValue("人员职位");

            ICell cell314 = row2.CreateCell(9);
            cell314.SetCellValue("创建时间");

            ICell cell315 = row2.CreateCell(10);
            cell315.SetCellValue("创建人编号");

            ICell cell316 = row2.CreateCell(11);
            cell316.SetCellValue("创建人名称");

            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();
                    if (dt.Rows[k][j].ToString() == "&nbsp;")
                    {
                        dgvValue = "";
                    }
                    else
                    {
                        dgvValue = dt.Rows[k][j].ToString();

                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 7.班制信息导出
        /// <summary>
        /// 班制信息导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_Class(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "班制信息数据";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("班制编号");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("班制名称");

            ICell cell211 = row2.CreateCell(2);
            cell211.SetCellValue("班制类型");

            ICell cell212 = row2.CreateCell(3);
            cell212.SetCellValue("班制开始时间");

            ICell cell213 = row2.CreateCell(4);
            cell213.SetCellValue("班制总时长");

            ICell cell202 = row2.CreateCell(5);
            cell202.SetCellValue("创建时间");

            ICell cell203 = row2.CreateCell(6);
            cell203.SetCellValue("创建人编号");

            ICell cell301 = row2.CreateCell(7);
            cell301.SetCellValue("创建人名称");

            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();
                    if (dt.Rows[k][j].ToString() == "&nbsp;")
                    {
                        dgvValue = "";
                    }
                    else
                    {
                        dgvValue = dt.Rows[k][j].ToString();
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 8.班次信息导出
        /// <summary>
        /// AVI站点信息导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_Shift(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "班次信息数据";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("班次编号");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("班次名称");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("创建时间");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("创建人编号");

            ICell cell301 = row2.CreateCell(4);
            cell301.SetCellValue("创建人名称");

            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();
                    if (dt.Rows[k][j].ToString() == "&nbsp;")
                    {
                        dgvValue = "";
                    }
                    else
                    {
                        dgvValue = dt.Rows[k][j].ToString();
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 9.滞留区域信息导出
        /// <summary>
        /// AVI站点信息导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_CarStranded(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "滞留区域信息数据";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("区域编号");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("区域名称");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("区域类别");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("起始AVI站点");

            ICell cell301 = row2.CreateCell(4);
            cell301.SetCellValue("结束AVI站点");

            ICell cell302 = row2.CreateCell(5);
            cell302.SetCellValue("滞留等级");

            ICell cell303 = row2.CreateCell(6);
            cell303.SetCellValue("滞留时间临界值");

            ICell cell305 = row2.CreateCell(7);
            cell305.SetCellValue("滞留报警规则");

            ICell cell308 = row2.CreateCell(8);
            cell308.SetCellValue("创建时间");

            ICell cell309 = row2.CreateCell(9);
            cell309.SetCellValue("创建人编号");

            ICell cell310 = row2.CreateCell(10);
            cell310.SetCellValue("创建人名称");

            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();
                    if (j == 2)
                    {
                        if (dt.Rows[k][j].ToString() == "0")
                        {
                            dgvValue = "车间";
                        }
                        else if (dt.Rows[k][j].ToString() == "1")
                        {
                            dgvValue = "区域";
                        }

                        else
                        {
                            dgvValue = "产线";
                        }
                    }
                    else if (dt.Rows[k][j].ToString() == "&nbsp;")
                    {
                        dgvValue = "";
                    }
                    else
                    {
                        dgvValue = dt.Rows[k][j].ToString();
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 10.设备报警信息导出
        /// <summary>
        /// AVI站点信息导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_AlarmAddress(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "设备报警信息数据";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("设备名称");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("报警地址");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("报警位数");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("路径");

            ICell cell301 = row2.CreateCell(4);
            cell301.SetCellValue("报警等级");

            ICell cell302 = row2.CreateCell(5);
            cell302.SetCellValue("描述");

            ICell cell308 = row2.CreateCell(6);
            cell308.SetCellValue("创建时间");

            ICell cell309 = row2.CreateCell(7);
            cell309.SetCellValue("创建人编号");

            ICell cell310 = row2.CreateCell(8);
            cell310.SetCellValue("创建人名称");

            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (dt.Rows[k][j].ToString() == "&nbsp;")
                    {
                        dgvValue = "";
                    }
                    else
                    {
                        dgvValue = dt.Rows[k][j].ToString();
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 11.车身质量检查销项过程表导出
        /// <summary>
        /// 车身质量检查销项过程表导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_CarQualityOutput(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "质量录入数据";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("工段");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("质控点");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("VIN");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("车型");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("车身组件名称");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("缺陷名称");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("备注");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("状态");

            ICell cell208 = row2.CreateCell(8);
            cell208.SetCellValue("复检次数");

            ICell cell209 = row2.CreateCell(9);
            cell209.SetCellValue("录入人员编号");

            ICell cell210 = row2.CreateCell(10);
            cell210.SetCellValue("录入人员姓名");

            ICell cell211 = row2.CreateCell(11);
            cell211.SetCellValue("录入时间");

            ICell cell212 = row2.CreateCell(12);
            cell212.SetCellValue("维修人员编号");

            ICell cell213 = row2.CreateCell(13);
            cell213.SetCellValue("维修人员姓名");

            ICell cell214 = row2.CreateCell(14);
            cell214.SetCellValue("维修时间");

            ICell cell215 = row2.CreateCell(15);
            cell215.SetCellValue("维修备注");

            ICell cell216 = row2.CreateCell(16);
            cell216.SetCellValue("复检人员编号");

            ICell cell217 = row2.CreateCell(17);
            cell217.SetCellValue("复检人员姓名");

            ICell cell218 = row2.CreateCell(18);
            cell218.SetCellValue("复检时间");

            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (j == 21)
                    {
                        if (dt.Rows[k][j].ToString() == "0")
                        {
                            dgvValue = "否";
                        }
                        else if (dt.Rows[k][j].ToString() == "1")
                        {
                            dgvValue = "是";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    else
                    {
                        if (dt.Rows[k][j].ToString() == "&nbsp;")
                        {
                            dgvValue = "";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 11.1冲焊车身质量检查销项过程表导出
        /// <summary>
        /// 车身质量检查销项过程表导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_HTCarQualityOutput(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "质量录入数据";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;


            ICell cell201 = row2.CreateCell(0);
            cell201.SetCellValue("质控点（工段）");

            ICell cell202 = row2.CreateCell(1);
            cell202.SetCellValue("VIN");

            ICell cell203 = row2.CreateCell(2);
            cell203.SetCellValue("车型");

            ICell cell204 = row2.CreateCell(3);
            cell204.SetCellValue("车身组件名称");

            ICell cell205 = row2.CreateCell(4);
            cell205.SetCellValue("缺陷名称");

            ICell cell206 = row2.CreateCell(5);
            cell206.SetCellValue("备注");

            ICell cell209 = row2.CreateCell(6);
            cell209.SetCellValue("录入人员编号");

            ICell cell210 = row2.CreateCell(7);
            cell210.SetCellValue("录入人员姓名");

            ICell cell211 = row2.CreateCell(8);
            cell211.SetCellValue("录入时间");

            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (j == 21)
                    {
                        if (dt.Rows[k][j].ToString() == "0")
                        {
                            dgvValue = "否";
                        }
                        else if (dt.Rows[k][j].ToString() == "1")
                        {
                            dgvValue = "是";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    else
                    {
                        if (dt.Rows[k][j].ToString() == "&nbsp;")
                        {
                            dgvValue = "";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion

        #region 12.日计划表导出
        /// <summary>
        /// 日计划表导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_ProducePlan(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "日计划表导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("计划编号");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("订单号");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("VIN码");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("车身物料号");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("车型");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("颜色");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("预计生产时间");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("计划状态");

            ICell cell208 = row2.CreateCell(8);
            cell208.SetCellValue("接收时间");


            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (j == 7) //计划执行状态
                    {
                        if (dt.Rows[k][j].ToString() == "0")
                        {
                            dgvValue = "未上线";
                        }
                        else if (dt.Rows[k][j].ToString() == "1")
                        {
                            dgvValue = "执行中";
                        }
                        else if (dt.Rows[k][j].ToString() == "2")
                        {
                            dgvValue = "已上线";
                        }
                        else if (dt.Rows[k][j].ToString() == "3")
                        {
                            dgvValue = "已完成";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    else
                    {
                        if (dt.Rows[k][j].ToString() == "&nbsp;")
                        {
                            dgvValue = "";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 13.小时计划表导出
        /// <summary>
        /// 小时计划表导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_HourProducePlan(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "小时计划表导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("计划编号");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("订单号");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("VIN码");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("车身物料号");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("车型");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("颜色");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("预计生产时间");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("计划状态");

            ICell cell208 = row2.CreateCell(8);
            cell208.SetCellValue("接收时间");


            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (j == 7) //计划执行状态
                    {
                        if (dt.Rows[k][j].ToString() == "0")
                        {
                            dgvValue = "未上线";
                        }
                        else if (dt.Rows[k][j].ToString() == "1")
                        {
                            dgvValue = "执行中";
                        }
                        else if (dt.Rows[k][j].ToString() == "2")
                        {
                            dgvValue = "已上线";
                        }
                        else if (dt.Rows[k][j].ToString() == "3")
                        {
                            dgvValue = "已完成";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    else
                    {
                        if (dt.Rows[k][j].ToString() == "&nbsp;")
                        {
                            dgvValue = "";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 14.计划信息录入表导出
        /// <summary>
        /// 计划信息录入表导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_PlanBoard(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "计划信息录入表导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("日期");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("计划UPH");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("日排产数量");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("月排产数量");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("标语");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("数据来源");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("日上线数量");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("日下线数量");

            ICell cell208 = row2.CreateCell(8);
            cell208.SetCellValue("日入库数量");

            ICell cell209 = row2.CreateCell(9);
            cell209.SetCellValue("月上线数量");

            ICell cell210 = row2.CreateCell(10);
            cell210.SetCellValue("月下线数量");

            ICell cell211 = row2.CreateCell(11);
            cell211.SetCellValue("月入库数量");

            ICell cell212 = row2.CreateCell(12);
            cell212.SetCellValue("创建时间");

            ICell cell213 = row2.CreateCell(13);
            cell213.SetCellValue("创建人名称");

            ICell cell214 = row2.CreateCell(14);
            cell214.SetCellValue("修改时间");

            ICell cell215 = row2.CreateCell(15);
            cell215.SetCellValue("修改人名称");

            ICell cell216 = row2.CreateCell(16);
            cell216.SetCellValue("备注");


            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (dt.Rows[k][j].ToString() == "&nbsp;")
                    {
                        dgvValue = "";
                    }
                    else
                    {
                        dgvValue = dt.Rows[k][j].ToString();
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 15.Andon补录数据导出
        /// <summary>
        /// Andon补录数据导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_DataAcqPro(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "Andon补录数据导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("产线名称");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("工位名称");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("信号来源");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("信号详情");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("补录状态");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("处理状态");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("采集时间");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("结束时间");

            ICell cell208 = row2.CreateCell(8);
            cell208.SetCellValue("处理时间");

            ICell cell209 = row2.CreateCell(9);
            cell209.SetCellValue("处理人编号");

            ICell cell210 = row2.CreateCell(10);
            cell210.SetCellValue("处理人姓名");

            ICell cell211 = row2.CreateCell(11);
            cell211.SetCellValue("异常类型");

            ICell cell212 = row2.CreateCell(12);
            cell212.SetCellValue("异常描述");

            ICell cell213 = row2.CreateCell(13);
            cell213.SetCellValue("处理结果");

            ICell cell214 = row2.CreateCell(14);
            cell214.SetCellValue("修改时间");

            ICell cell215 = row2.CreateCell(15);
            cell215.SetCellValue("修改人名称");

            ICell cell216 = row2.CreateCell(16);
            cell216.SetCellValue("备注");



            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (j == 5) //计划执行状态
                    {
                        if (dt.Rows[k][j].ToString() == "0")
                        {
                            dgvValue = "异常中";
                        }
                        else if (dt.Rows[k][j].ToString() == "1")
                        {
                            dgvValue = "已恢复";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    else
                    {
                        if (dt.Rows[k][j].ToString() == "&nbsp;")
                        {
                            dgvValue = "";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 16.物料基础信息导出
        /// <summary>
        /// 物料基础信息导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_MatBase(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "物料基础信息导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("物料编号");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("物料名称");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("关重件工位编号");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("是否关重件");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("数量");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("是否打印");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("打印工位编号");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("存在下级件");

            ICell cell208 = row2.CreateCell(8);
            cell208.SetCellValue("安全件");

            ICell cell209 = row2.CreateCell(9);
            cell209.SetCellValue("简码");

            ICell cell210 = row2.CreateCell(10);
            cell210.SetCellValue("物料类别");

            ICell cell211 = row2.CreateCell(11);
            cell211.SetCellValue("禁止重复");

            ICell cell212 = row2.CreateCell(12);
            cell212.SetCellValue("创建时间");

            ICell cell213 = row2.CreateCell(13);
            cell213.SetCellValue("修改时间");

            ICell cell214 = row2.CreateCell(14);
            cell214.SetCellValue("备注");


            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (j == 3 || j == 5) //是否关重件
                    {
                        if (dt.Rows[k][j].ToString() == "0" || dt.Rows[k][j].ToString() == "")
                        {
                            dgvValue = "0";
                        }
                        else if (dt.Rows[k][j].ToString() == "1")
                        {
                            dgvValue = "1";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    else if (j == 7)//是否有下级件
                    {
                        if (dt.Rows[k][j].ToString() == "0")
                        {
                            dgvValue = "无";
                        }
                        else if (dt.Rows[k][j].ToString() == "1")
                        {
                            dgvValue = "有";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    else if (j == 10)//物料类别
                    {
                        if (dt.Rows[k][j].ToString() == "0")
                        {
                            dgvValue = "零部件";
                        }
                        else if (dt.Rows[k][j].ToString() == "1")
                        {
                            dgvValue = "产品";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    else if (j == 11)//禁止重复
                    {
                        if (dt.Rows[k][j].ToString() == "0")
                        {
                            dgvValue = "允许";
                        }
                        else if (dt.Rows[k][j].ToString() == "1")
                        {
                            dgvValue = "禁止";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    else
                    {
                        if (dt.Rows[k][j].ToString() == "&nbsp;")
                        {
                            dgvValue = "";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 17.AVI过点信息导出
        /// <summary>
        /// AVI过点信息导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_CarPastRecord(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "AVI过点信息导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("AVI站点编号");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("AVI站点名称");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("车型");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("车身物料号");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("车身颜色");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("VIN码");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("流水号");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("计划编号");

            ICell cell208 = row2.CreateCell(8);
            cell208.SetCellValue("过点方式");

            ICell cell209 = row2.CreateCell(9);
            cell209.SetCellValue("车身去向产线编号");

            ICell cell210 = row2.CreateCell(10);
            cell210.SetCellValue("车身去向产线名称");

            ICell cell211 = row2.CreateCell(11);
            cell211.SetCellValue("过点类型");

            ICell cell212 = row2.CreateCell(12);
            cell212.SetCellValue("过点时间");

            ICell cell213 = row2.CreateCell(13);
            cell213.SetCellValue("PBS过点顺序");

            ICell cell214 = row2.CreateCell(14);
            cell214.SetCellValue("录入人员");

            ICell cell215 = row2.CreateCell(15);
            cell215.SetCellValue("备注");




            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (j == 8) //过点方式
                    {
                        if (dt.Rows[k][j].ToString() == "0")
                        {
                            dgvValue = "";
                        }
                        else if (dt.Rows[k][j].ToString() == "1")
                        {
                            dgvValue = "正常";
                        }
                        else if (dt.Rows[k][j].ToString() == "2")
                        {
                            dgvValue = "拉出";
                        }
                        else if (dt.Rows[k][j].ToString() == "3")
                        {
                            dgvValue = "拉入";
                        }
                        else if (dt.Rows[k][j].ToString() == "4")
                        {
                            dgvValue = "插车";
                        }
                        else if (dt.Rows[k][j].ToString() == "5")
                        {
                            dgvValue = "补扫";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    else if (j == 11)//过点类型
                    {
                        if (dt.Rows[k][j].ToString() == "0")
                        {
                            dgvValue = "";
                        }
                        else if (dt.Rows[k][j].ToString() == "1")
                        {
                            dgvValue = "自动";
                        }
                        else if (dt.Rows[k][j].ToString() == "2")
                        {
                            dgvValue = "后台补录数据";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    else
                    {
                        if (dt.Rows[k][j].ToString() == "&nbsp;")
                        {
                            dgvValue = "";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 18.区间查询信息导出
        /// <summary>
        /// 区间查询信息导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_CarInventoryInfo(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "区间查询信息导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("订单编号");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("工单号");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("底盘号");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("VIN码");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("车型");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("车型编码");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("车型名称");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("颜色");

            ICell cell208 = row2.CreateCell(8);
            cell208.SetCellValue("当前AVI站点名称");

            ICell cell209 = row2.CreateCell(9);
            cell209.SetCellValue("采集时间");

            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (dt.Rows[k][j].ToString() == "&nbsp;")
                    {
                        dgvValue = "";
                    }
                    else
                    {
                        dgvValue = dt.Rows[k][j].ToString();
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 19.产品基础信息导出
        /// <summary>
        /// 产品基础信息导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_ProductBase(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "产品基础信息导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("产品编号");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("产品名称");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("车型");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("颜色");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("整车型号(公告号)");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("发动机排量");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("发动机型号");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("发动机最大净功率/转速");

            ICell cell208 = row2.CreateCell(8);
            cell208.SetCellValue("最大允许总质量");

            ICell cell209 = row2.CreateCell(9);
            cell209.SetCellValue("载客人数");

            ICell cell210 = row2.CreateCell(10);
            cell210.SetCellValue("整车整备质量");

            ICell cell211 = row2.CreateCell(11);
            cell211.SetCellValue("用途");

            ICell cell212 = row2.CreateCell(12);
            cell212.SetCellValue("特殊车辆名称");

            ICell cell213 = row2.CreateCell(13);
            cell213.SetCellValue("发动机额定功率");

            ICell cell214 = row2.CreateCell(14);
            cell214.SetCellValue("燃油标识主键");

            ICell cell215 = row2.CreateCell(15);
            cell215.SetCellValue("描述");

            ICell cell216 = row2.CreateCell(16);
            cell216.SetCellValue("备注");


            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (dt.Rows[k][j].ToString() == "&nbsp;")
                    {
                        dgvValue = "";
                    }
                    else
                    {
                        dgvValue = dt.Rows[k][j].ToString();
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 20.关重件录入数据导出
        /// <summary>
        /// 关重件录入数据导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_KeyPartsBind(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "关重件录入数据导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("VIN码");     
                                               
            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("产品名称");    
                                               
            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("订单号");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("车型");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("车身编号");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("颜色");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("关重件条码");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("批次号");

            ICell cell208 = row2.CreateCell(8);
            cell208.SetCellValue("供应商编号");

            ICell cell209 = row2.CreateCell(9);
            cell209.SetCellValue("供应商名称");

            ICell cell210 = row2.CreateCell(10);
            cell210.SetCellValue("关重件物料编号");

            ICell cell211 = row2.CreateCell(11);
            cell211.SetCellValue("关重件物料名称");

            ICell cell212 = row2.CreateCell(12);
            cell212.SetCellValue("工厂名称");

            ICell cell213 = row2.CreateCell(13);
            cell213.SetCellValue("车间名称");

            ICell cell214 = row2.CreateCell(14);
            cell214.SetCellValue("工段名称");

            ICell cell215 = row2.CreateCell(15);
            cell215.SetCellValue("产线名称");

            ICell cell216 = row2.CreateCell(16);
            cell216.SetCellValue("工位编号");

            ICell cell217 = row2.CreateCell(17);
            cell217.SetCellValue("人员名称");

            ICell cell218 = row2.CreateCell(18);
            cell218.SetCellValue("采集时间");


            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (dt.Rows[k][j].ToString() == "&nbsp;")
                    {
                        dgvValue = "";
                    }
                    else
                    {
                        dgvValue = dt.Rows[k][j].ToString();
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 21.关重件ByPass数据导出
        /// <summary>
        /// 关重件ByPass数据导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_KeyByPass(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "关重件ByPass数据导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("VIN码");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("产品名称");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("订单号");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("车型");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("车身编号");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("颜色");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("物料编号");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("物料名称");

            ICell cell208 = row2.CreateCell(8);
            cell208.SetCellValue("工厂名称");

            ICell cell209 = row2.CreateCell(9);
            cell209.SetCellValue("车间名称");

            ICell cell210 = row2.CreateCell(10);
            cell210.SetCellValue("工段名称");

            ICell cell211 = row2.CreateCell(11);
            cell211.SetCellValue("产线名称");

            ICell cell212 = row2.CreateCell(12);
            cell212.SetCellValue("工位编号");

            ICell cell213 = row2.CreateCell(13);
            cell213.SetCellValue("人员名称");

            ICell cell214 = row2.CreateCell(14);
            cell214.SetCellValue("采集时间");

            
            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (dt.Rows[k][j].ToString() == "&nbsp;")
                    {
                        dgvValue = "";
                    }
                    else
                    {
                        dgvValue = dt.Rows[k][j].ToString();
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 22.供应商数据导出
        /// <summary>
        /// 供应商数据导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_SupplierBase(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "供应商数据导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("供应商代码");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("供应商名称");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("供应商类型");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("供应商等级");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("供应商联系电话");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("负责人");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("供应商地址");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("供应商邮箱");

            ICell cell208 = row2.CreateCell(8);
            cell208.SetCellValue("供应商网址");

            ICell cell209 = row2.CreateCell(9);
            cell209.SetCellValue("供应商描述");

            ICell cell210 = row2.CreateCell(10);
            cell210.SetCellValue("备注");






            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (dt.Rows[k][j].ToString() == "&nbsp;")
                    {
                        dgvValue = "";
                    }
                    else
                    {
                        dgvValue = dt.Rows[k][j].ToString();
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 23.检验岗数据导出
        /// <summary>
        /// 检验岗数据导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_QualityCarPosition(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "检验岗数据导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("检验岗编号");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("检验岗名称");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("检验岗描述");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("创建时间");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("创建人名称");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("修改时间");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("修改人名称");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("备注");

            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (dt.Rows[k][j].ToString() == "&nbsp;")
                    {
                        dgvValue = "";
                    }
                    else
                    {
                        dgvValue = dt.Rows[k][j].ToString();
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 24.检验项目数据导出
        /// <summary>
        /// 检验项目数据导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_QualityCarPositionGroup(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "检验项目数据导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("检验项目编码");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("检验项目名称");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("检验岗名称");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("检验项目描述");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("创建时间");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("创建人名称");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("修改时间");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("修改人名称");

            ICell cell208 = row2.CreateCell(8);
            cell208.SetCellValue("备注");
            
            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (dt.Rows[k][j].ToString() == "&nbsp;")
                    {
                        dgvValue = "";
                    }
                    else
                    {
                        dgvValue = dt.Rows[k][j].ToString();
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 25.检验部件数据导出
        /// <summary>
        /// 检验部件数据导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_QualityCarComponent(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "检验部件数据导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("部件编号");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("部件名称");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("检验岗名称");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("检验项目名称");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("部件描述");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("创建时间");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("创建人名称");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("修改时间");

            ICell cell208 = row2.CreateCell(8);
            cell208.SetCellValue("修改人名称 ");

            ICell cell209 = row2.CreateCell(9);
            cell209.SetCellValue("备注");



            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (dt.Rows[k][j].ToString() == "&nbsp;")
                    {
                        dgvValue = "";
                    }
                    else
                    {
                        dgvValue = dt.Rows[k][j].ToString();
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 26.缺陷类型数据导出
        /// <summary>
        /// 检验岗数据导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_DefectCatgBase(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "缺陷类型数据导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("缺陷类型编号");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("缺陷类型名称");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("描述");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("创建时间");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("创建人名称");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("修改时间");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("修改人名称");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("备注");

            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (dt.Rows[k][j].ToString() == "&nbsp;")
                    {
                        dgvValue = "";
                    }
                    else
                    {
                        dgvValue = dt.Rows[k][j].ToString();
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 27.缺陷分组数据导出
        /// <summary>
        /// 缺陷分组数据导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_DefectCatgGroupBase(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "缺陷分组数据导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("缺陷分组编号");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("缺陷分组名称");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("缺陷类别名称");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("描述");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("检验项目描述");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("创建时间");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("创建人名称");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("修改时间");

            ICell cell208 = row2.CreateCell(8);
            cell208.SetCellValue("修改人名称");

            ICell cell209 = row2.CreateCell(9);
            cell209.SetCellValue("备注");

            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (dt.Rows[k][j].ToString() == "&nbsp;")
                    {
                        dgvValue = "";
                    }
                    else
                    {
                        dgvValue = dt.Rows[k][j].ToString();
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 28.缺陷详情数据导出
        /// <summary>
        /// 缺陷详情数据导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_DefectCatgDeTail(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "缺陷详情数据导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("缺陷编号");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("缺陷名称");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("缺陷类型名称");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("缺陷分组名称");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("描述");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("创建时间");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("创建人名称");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("修改时间");

            ICell cell208 = row2.CreateCell(8);
            cell208.SetCellValue("修改人名称");

            ICell cell209 = row2.CreateCell(9);
            cell209.SetCellValue("备注");
            



            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (dt.Rows[k][j].ToString() == "&nbsp;")
                    {
                        dgvValue = "";
                    }
                    else
                    {
                        dgvValue = dt.Rows[k][j].ToString();
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 29.EOL检测数据导出
        /// <summary>
        /// EOL检测数据导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_EOL(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "EOL检测数据导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("VIN码");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("车型编码");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("车型");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("车辆名称");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("站点");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("时间");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("检测次数");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("检测结果");


            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (dt.Rows[k][j].ToString() == "&nbsp;")
                    {
                        dgvValue = "";
                    }
                    else
                    {
                        dgvValue = dt.Rows[k][j].ToString();
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 30.检测线数据导出
        /// <summary>
        /// 检测线数据导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_TestLine(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "检测线数据导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("VIN码");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("车辆编码");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("车型");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("颜色");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("ABS检测结果");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("四轮定位检测结果");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("转角检测结果");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("制动检测结果");

            ICell cell208 = row2.CreateCell(8);
            cell208.SetCellValue("灯光检测结果");

            ICell cell209 = row2.CreateCell(9);
            cell209.SetCellValue("喇叭检测结果");

            ICell cell210 = row2.CreateCell(10);
            cell210.SetCellValue("速度检测结果");





            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (dt.Rows[k][j].ToString() == "&nbsp;")
                    {
                        dgvValue = "";
                    }
                    else
                    {
                        dgvValue = dt.Rows[k][j].ToString();
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 31.加注数据导出
        /// <summary>
        /// 检测线数据导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_JZ(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "加注数据导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("VIN码");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("车型编码");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("车型");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("颜色");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("防冻液加注结果");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("转向液加注结果");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("制动液加注结果");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("冷媒加注结果");

            ICell cell208 = row2.CreateCell(8);
            cell208.SetCellValue("洗涤液加注结果");

            ICell cell209 = row2.CreateCell(9);
            cell209.SetCellValue("正压CL检测结果");

            ICell cell210 = row2.CreateCell(10);
            cell210.SetCellValue("正压BR检测结果");

            ICell cell211 = row2.CreateCell(11);
            cell211.SetCellValue("正压AC检测结果");


            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (dt.Rows[k][j].ToString() == "&nbsp;")
                    {
                        dgvValue = "";
                    }
                    else
                    {
                        dgvValue = dt.Rows[k][j].ToString();
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 32.胎压数据导出
        /// <summary>
        /// 检测线数据导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_TY(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "胎压数据导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("VIN");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("车型编码");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("车型");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("传感器名称");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("右前轮ID");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("右前轮胎压上限");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("右前轮胎压下限");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("右前轮胎压检测值");

            ICell cell208 = row2.CreateCell(8);
            cell208.SetCellValue("右前轮胎压检测结果");

            ICell cell209 = row2.CreateCell(9);
            cell209.SetCellValue("左前轮ID");

            ICell cell210 = row2.CreateCell(10);
            cell210.SetCellValue("左前轮胎压上限");

            ICell cell211 = row2.CreateCell(11);
            cell211.SetCellValue("左前轮胎压下限");

            ICell cell212 = row2.CreateCell(12);
            cell212.SetCellValue("左前轮胎压检测值");

            ICell cell213 = row2.CreateCell(13);
            cell213.SetCellValue("左前轮胎压检测结果");

            ICell cell214 = row2.CreateCell(14);
            cell214.SetCellValue("右后轮ID");

            ICell cell215 = row2.CreateCell(15);
            cell215.SetCellValue("右后轮胎压上限");

            ICell cell216 = row2.CreateCell(16);
            cell216.SetCellValue("右后轮胎压下限");

            ICell cell217 = row2.CreateCell(17);
            cell217.SetCellValue("右后轮胎压检测值");

            ICell cell218 = row2.CreateCell(18);
            cell218.SetCellValue("右后轮胎压检测结果");

            ICell cell219 = row2.CreateCell(19);
            cell219.SetCellValue("左后轮ID");

            ICell cell220 = row2.CreateCell(20);
            cell220.SetCellValue("左后轮胎压上限");

            ICell cell221 = row2.CreateCell(21);
            cell221.SetCellValue("左后轮胎压下限");

            ICell cell222 = row2.CreateCell(22);
            cell222.SetCellValue("左后轮胎压检测值");

            ICell cell223 = row2.CreateCell(23);
            cell223.SetCellValue("左后轮胎压检测结果");

            ICell cell224 = row2.CreateCell(24);
            cell224.SetCellValue("胎压检测单位");

            ICell cell225 = row2.CreateCell(25);
            cell225.SetCellValue("检测路径");

            ICell cell226 = row2.CreateCell(26);
            cell226.SetCellValue("检测时间");

            ICell cell227 = row2.CreateCell(27);
            cell227.SetCellValue("总结果");



            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (dt.Rows[k][j].ToString() == "&nbsp;")
                    {
                        dgvValue = "";
                    }
                    else
                    {
                        dgvValue = dt.Rows[k][j].ToString();
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 33.公司数据导出
        /// <summary>
        /// 检测线数据导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_CompanyBase(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "公司数据导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("公司编号");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("公司名称");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("法人");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("联系电话");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("传真");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("邮箱");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("公司地址");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("公司描述");

            ICell cell208 = row2.CreateCell(8);
            cell208.SetCellValue("顺序号");

            ICell cell209 = row2.CreateCell(9);
            cell209.SetCellValue("创建人编号");

            ICell cell210 = row2.CreateCell(10);
            cell210.SetCellValue("创建人名称");

            ICell cell211 = row2.CreateCell(11);
            cell211.SetCellValue("创建时间");

            ICell cell212 = row2.CreateCell(12);
            cell212.SetCellValue("修改人编号");

            ICell cell213 = row2.CreateCell(13);
            cell213.SetCellValue("修改人名称");

            ICell cell214 = row2.CreateCell(14);
            cell214.SetCellValue("修改时间");

            ICell cell215 = row2.CreateCell(15);
            cell215.SetCellValue("备注");
            

            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (dt.Rows[k][j].ToString() == "&nbsp;")
                    {
                        dgvValue = "";
                    }
                    else
                    {
                        dgvValue = dt.Rows[k][j].ToString();
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 34.工厂基础信息导出
        /// <summary>
        /// 工厂基础信息导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_FacBase(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "工厂基础信息导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("工厂编号");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("工厂名称");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("公司编号");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("公司名称");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("工厂类型");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("工厂地址");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("工厂描述");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("工厂联系电话");

            ICell cell208 = row2.CreateCell(8);
            cell208.SetCellValue("工厂传真");

            ICell cell209 = row2.CreateCell(9);
            cell209.SetCellValue("负责人手机号");

            ICell cell210 = row2.CreateCell(10);
            cell210.SetCellValue("邮箱");

            ICell cell211 = row2.CreateCell(11);
            cell211.SetCellValue("顺序号");

            ICell cell212 = row2.CreateCell(12);
            cell212.SetCellValue("创建时间");

            ICell cell213 = row2.CreateCell(13);
            cell213.SetCellValue("创建人名称");

            ICell cell214 = row2.CreateCell(14);
            cell214.SetCellValue("修改时间");

            ICell cell215 = row2.CreateCell(15);
            cell215.SetCellValue("修改人名称");

            ICell cell216 = row2.CreateCell(16);
            cell216.SetCellValue("备注");
            
            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (dt.Rows[k][j].ToString() == "&nbsp;")
                    {
                        dgvValue = "";
                    }
                    else
                    {
                        dgvValue = dt.Rows[k][j].ToString();
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 35.车间基础信息导出
        /// <summary>
        /// 车间基础信息导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_WorkshopBase(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "车间基础信息导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("车间编号");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("车间名称");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("车间类型");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("工厂编号");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("工厂名称");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("车间描述");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("顺序号");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("创建时间");

            ICell cell208 = row2.CreateCell(8);
            cell208.SetCellValue("创建人名称");

            ICell cell209 = row2.CreateCell(9);
            cell209.SetCellValue("修改时间");

            ICell cell210 = row2.CreateCell(10);
            cell210.SetCellValue("修改人编号");

            ICell cell211 = row2.CreateCell(11);
            cell211.SetCellValue("备注");
            
            
            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (dt.Rows[k][j].ToString() == "&nbsp;")
                    {
                        dgvValue = "";
                    }
                    else
                    {
                        dgvValue = dt.Rows[k][j].ToString();
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 36.工段基础信息导出
        /// <summary>
        /// 工段基础信息导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_WorkSectionBase(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "工段基础信息导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("工段编号");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("工段名称");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("工段类型");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("车间编号");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("车间名称");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("车间类型");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("顺序号");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("工段描述");

            ICell cell208 = row2.CreateCell(8);
            cell208.SetCellValue("创建时间");

            ICell cell209 = row2.CreateCell(9);
            cell209.SetCellValue("创建人名称");

            ICell cell210 = row2.CreateCell(10);
            cell210.SetCellValue("修改时间");

            ICell cell211 = row2.CreateCell(11);
            cell211.SetCellValue("修改人名称");

            ICell cell212 = row2.CreateCell(12);
            cell212.SetCellValue("备注");

          

            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (dt.Rows[k][j].ToString() == "&nbsp;")
                    {
                        dgvValue = "";
                    }
                    else
                    {
                        dgvValue = dt.Rows[k][j].ToString();
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 37.产线基础信息导出
        /// <summary>
        /// 产线基础信息导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_PlineBase(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "产线基础信息导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("产线编号");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("产线名称");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("工段编号");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("工段名称");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("顺序号");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("产线类型");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("工位数量");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("工位默认长度");

            ICell cell208 = row2.CreateCell(8);
            cell208.SetCellValue("工位默认截距");

            ICell cell209 = row2.CreateCell(9);
            cell209.SetCellValue("产线描述");

            ICell cell210 = row2.CreateCell(10);
            cell210.SetCellValue("创建时间");

            ICell cell211 = row2.CreateCell(11);
            cell211.SetCellValue("创建人名称");

            ICell cell212 = row2.CreateCell(12);
            cell212.SetCellValue("修改时间");

            ICell cell213 = row2.CreateCell(13);
            cell213.SetCellValue("修改人编号");

            ICell cell214 = row2.CreateCell(14);
            cell214.SetCellValue("备注");



            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (dt.Rows[k][j].ToString() == "&nbsp;")
                    {
                        dgvValue = "";
                    }
                    else
                    {
                        dgvValue = dt.Rows[k][j].ToString();
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 38.工位基础信息导出
        /// <summary>
        /// 工位基础信息导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_Wc(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "工位基础信息导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("工位编号");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("工位名称");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("工位类型");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("产线编号");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("产线名称");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("顺序号");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("开始");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("预警");

            ICell cell208 = row2.CreateCell(8);
            cell208.SetCellValue("停线");

            ICell cell209 = row2.CreateCell(9);
            cell209.SetCellValue("结束");

            ICell cell210 = row2.CreateCell(10);
            cell210.SetCellValue("工位描述");

            ICell cell211 = row2.CreateCell(11);
            cell211.SetCellValue("Pass次数");

            ICell cell212 = row2.CreateCell(12);
            cell212.SetCellValue("是否允许强制录入");

            ICell cell213 = row2.CreateCell(13);
            cell213.SetCellValue("创建时间");

            ICell cell214 = row2.CreateCell(14);
            cell214.SetCellValue("创建人名称");

            ICell cell215 = row2.CreateCell(15);
            cell215.SetCellValue("修改时间");

            ICell cell216 = row2.CreateCell(16);
            cell216.SetCellValue("修改人名称");

            ICell cell217 = row2.CreateCell(17);
            cell217.SetCellValue("备注");



            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (j == 12) //是否允许强制录入
                    {
                        if (dt.Rows[k][j].ToString() == "1")
                        {
                            dgvValue = "允许";
                        }
                        else if (dt.Rows[k][j].ToString() == "0")
                        {
                            dgvValue = "不允许";
                        }
                        else
                        {
                            dgvValue = "不允许";
                        }
                    }
                    else
                    {
                        if (dt.Rows[k][j].ToString() == "&nbsp;")
                        {
                            dgvValue = "";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 39.生产日历信息导出
        /// <summary>
        /// 生产日历信息导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_BPdb_Dt(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "生产日历信息导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("机构级别");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("机构名称");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("班制名称");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("日期");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("工作时间(h)");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("早上开始");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("早上结束");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("下午开始");

            ICell cell208 = row2.CreateCell(8);
            cell208.SetCellValue("下午结束");

            ICell cell209 = row2.CreateCell(9);
            cell209.SetCellValue("晚上开始");

            ICell cell210 = row2.CreateCell(10);
            cell210.SetCellValue("晚上结束");

            ICell cell211 = row2.CreateCell(11);
            cell211.SetCellValue("日期类型");

            ICell cell212 = row2.CreateCell(12);
            cell212.SetCellValue("创建人名称");

            ICell cell213 = row2.CreateCell(13);
            cell213.SetCellValue("创建时间");

            ICell cell214 = row2.CreateCell(14);
            cell214.SetCellValue("修改人名称");

            ICell cell215 = row2.CreateCell(15);
            cell215.SetCellValue("修改时间");

            ICell cell216 = row2.CreateCell(16);
            cell216.SetCellValue("备注");



            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (j == 11) //过点方式
                    {
                        if (dt.Rows[k][j].ToString() == "1")
                        {
                            dgvValue = "工作";
                        }
                        else if (dt.Rows[k][j].ToString() == "0")
                        {
                            dgvValue = "休息";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    else
                    {
                        if (dt.Rows[k][j].ToString() == "&nbsp;")
                        {
                            dgvValue = "";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 39.燃油标识信息导出
        /// <summary>
        /// 燃油标识信息导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_FUELOIL(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "燃油标识信息导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("生产企业");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("变速器类型");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("能源种类");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("整车整备质量");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("排量");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("最大设计总质量");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("最大净功率");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("驱动型式");

            ICell cell208 = row2.CreateCell(8);
            cell208.SetCellValue("其他信息");

            ICell cell209 = row2.CreateCell(9);
            cell209.SetCellValue("驱动电机峰值功率");

            ICell cell210 = row2.CreateCell(10);
            cell210.SetCellValue("市区工况");

            ICell cell211 = row2.CreateCell(11);
            cell211.SetCellValue("综合工况");

            ICell cell212 = row2.CreateCell(12);
            cell212.SetCellValue("综合工况电能消耗量");

            ICell cell213 = row2.CreateCell(13);
            cell213.SetCellValue("电能当量燃料消耗量");

            ICell cell214 = row2.CreateCell(14);
            cell214.SetCellValue("市郊工况");

            ICell cell215 = row2.CreateCell(15);
            cell215.SetCellValue("领跑值");

            ICell cell216 = row2.CreateCell(16);
            cell216.SetCellValue("限值");

            ICell cell217 = row2.CreateCell(17);
            cell217.SetCellValue("备案号");

            ICell cell218 = row2.CreateCell(18);
            cell218.SetCellValue("启用日期");

            ICell cell219 = row2.CreateCell(19);
            cell219.SetCellValue("国标编号");

            ICell cell220 = row2.CreateCell(20);
            cell220.SetCellValue("车辆型号");

            ICell cell221 = row2.CreateCell(21);
            cell221.SetCellValue("发动机型号");

            ICell cell222 = row2.CreateCell(22);
            cell222.SetCellValue("续航里程");



            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (j == 11) //过点方式
                    {
                        if (dt.Rows[k][j].ToString() == "1")
                        {
                            dgvValue = "工作";
                        }
                        else if (dt.Rows[k][j].ToString() == "0")
                        {
                            dgvValue = "休息";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    else
                    {
                        if (dt.Rows[k][j].ToString() == "&nbsp;")
                        {
                            dgvValue = "";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 40.条码解析导出
        /// <summary>
        /// 条码解析导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_BarCodeBase(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "条码解析导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("条码编号");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("条码名称");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("条码长度");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("优先级");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("创建时间");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("创建人名称");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("修改时间");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("修改人名称");

            ICell cell208 = row2.CreateCell(8);
            cell208.SetCellValue("备注");

           

            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (j == 11) //过点方式
                    {
                        if (dt.Rows[k][j].ToString() == "1")
                        {
                            dgvValue = "工作";
                        }
                        else if (dt.Rows[k][j].ToString() == "0")
                        {
                            dgvValue = "休息";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    else
                    {
                        if (dt.Rows[k][j].ToString() == "&nbsp;")
                        {
                            dgvValue = "";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 41.拧紧信息导出
        /// <summary>
        /// 拧紧信息导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_NJ(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "拧紧信息导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("VIN");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("车型编号");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("车型");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("JOB号");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("批次号");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("批次大小");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("批次号");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("批次状态");

            ICell cell208 = row2.CreateCell(8);
            cell208.SetCellValue("拧紧状态");

            ICell cell209 = row2.CreateCell(9);
            cell209.SetCellValue("扭矩最小值");

            ICell cell210 = row2.CreateCell(10);
            cell210.SetCellValue("扭矩最大值");

            ICell cell211 = row2.CreateCell(11);
            cell211.SetCellValue("扭矩目标值");

            ICell cell212 = row2.CreateCell(12);
            cell212.SetCellValue("扭矩");

            ICell cell213 = row2.CreateCell(13);
            cell213.SetCellValue("角度状态");

            ICell cell214 = row2.CreateCell(14);
            cell214.SetCellValue("角度最小值");

            ICell cell215 = row2.CreateCell(15);
            cell215.SetCellValue("角度最大值");

            ICell cell216 = row2.CreateCell(16);
            cell216.SetCellValue("角度目标值");

            ICell cell217 = row2.CreateCell(17);
            cell217.SetCellValue("角度");

            ICell cell218 = row2.CreateCell(18);
            cell218.SetCellValue("拧紧号");

            ICell cell219 = row2.CreateCell(19);
            cell219.SetCellValue("拧紧状态");

            ICell cell220 = row2.CreateCell(20);
            cell220.SetCellValue("执行时间");



            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (j == 11) //过点方式
                    {
                        if (dt.Rows[k][j].ToString() == "1")
                        {
                            dgvValue = "工作";
                        }
                        else if (dt.Rows[k][j].ToString() == "0")
                        {
                            dgvValue = "休息";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    else
                    {
                        if (dt.Rows[k][j].ToString() == "&nbsp;")
                        {
                            dgvValue = "";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 42.系统日志信息导出
        /// <summary>
        /// 拧紧信息导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_SysLog(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "系统日志信息导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("操作时间");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("操作模块");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("操作类型");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("ip地址");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("操作用户");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("操作状态");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("操作描述");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("字段");

            ICell cell208 = row2.CreateCell(8);
            cell208.SetCellValue("字段中文名称");

            ICell cell209 = row2.CreateCell(9);
            cell209.SetCellValue("修改后");

            ICell cell210 = row2.CreateCell(10);
            cell210.SetCellValue("修改前");

            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (j == 2) //操作类型
                    {
                        if (dt.Rows[k][j].ToString() == "0")
                        {
                            dgvValue = "登录";
                        }
                        else if (dt.Rows[k][j].ToString() == "1")
                        {
                            dgvValue = "新增";
                        }
                        else if (dt.Rows[k][j].ToString() == "2")
                        {
                            dgvValue = "修改";
                        }
                        else if (dt.Rows[k][j].ToString() == "3")
                        {
                            dgvValue = "删除";
                        }
                        else if (dt.Rows[k][j].ToString() == "4")
                        {
                            dgvValue = "其他";
                        }
                        else if (dt.Rows[k][j].ToString() == "5")
                        {
                            dgvValue = "访问";
                        }
                        else if (dt.Rows[k][j].ToString() == "6")
                        {
                            dgvValue = "离开";
                        }
                        else if (dt.Rows[k][j].ToString() == "7")
                        {
                            dgvValue = "查询";
                        }
                        else if (dt.Rows[k][j].ToString() == "8")
                        {
                            dgvValue = "安全退出";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    else if (j==5)//操作状态
                    {
                        if (dt.Rows[k][j].ToString() == "1")
                        {
                            dgvValue = "成功";
                        }
                        else
                        {
                            dgvValue = "失败";
                        }
                    }
                    else
                    {
                        if (dt.Rows[k][j].ToString() == "&nbsp;")
                        {
                            dgvValue = "";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 43.登录日志信息导出
        /// <summary>
        /// 拧紧信息导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_LoginLogMana(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "登录日志信息导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("操作时间");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("操作类型");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("IP地址");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("操作用户");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("操作状态");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("操作描述");

            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (j == 1) //操作类型
                    {
                        dgvValue = "登录";
                    }
                    else if (j == 4)//操作状态
                    {
                        if (dt.Rows[k][j].ToString() == "1")
                        {
                            dgvValue = "成功";
                        }
                        else if (dt.Rows[k][j].ToString() == "0")
                        {
                            dgvValue = "失败";
                        }
                        else
                        {
                            dgvValue = "异常";
                        }
                    }
                    else
                    {
                        if (dt.Rows[k][j].ToString() == "&nbsp;")
                        {
                            dgvValue = "";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 44.接口日志信息导出
        /// <summary>
        /// 拧紧信息导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_P_ProductionLog(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "接口日志信息导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("项目");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("模块");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("主对象");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("辅对象");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("操作");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("结果");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("备注");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("时间");

            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;

                    if (dt.Rows[k][j].ToString() == "&nbsp;")
                    {
                        dgvValue = "";
                    }
                    else
                    {
                        dgvValue = dt.Rows[k][j].ToString();
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 45.账号信息导出
        /// <summary>
        /// 拧紧信息导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_UserMana(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "账号信息导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("工号");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("登录账户");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("姓名");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("性别");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("出生日期");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("手机");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("电子邮件");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("登录次数");

            ICell cell208 = row2.CreateCell(8);
            cell208.SetCellValue("最后访问时间");

            ICell cell209 = row2.CreateCell(9);
            cell209.SetCellValue("IP地址");

            ICell cell210 = row2.CreateCell(10);
            cell210.SetCellValue("上一次修改密码时间");

            ICell cell211 = row2.CreateCell(11);
            cell211.SetCellValue("创建时间");

            ICell cell212 = row2.CreateCell(12);
            cell212.SetCellValue("创建人");

            ICell cell213 = row2.CreateCell(13);
            cell213.SetCellValue("修改时间");

            ICell cell214 = row2.CreateCell(14);
            cell214.SetCellValue("修改人");

            ICell cell215 = row2.CreateCell(15);
            cell215.SetCellValue("备注");

            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;

                    if (dt.Rows[k][j].ToString() == "&nbsp;")
                    {
                        dgvValue = "";
                    }
                    else
                    {
                        dgvValue = dt.Rows[k][j].ToString();
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 46.发布序列导出
        /// <summary>
        /// 日计划表导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_PublishPlan(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "发布序列导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("计划编号");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("订单号");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("VIN码");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("车身编码");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("车型");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("颜色");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("预计生产时间");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("序列是否校核");

            ICell cell208 = row2.CreateCell(8);
            cell208.SetCellValue("铭牌是否打印");

            ICell cell209 = row2.CreateCell(9);
            cell209.SetCellValue("顺序号");

            ICell cell210 = row2.CreateCell(10);
            cell210.SetCellValue("铭牌打印顺序");

            ICell cell211 = row2.CreateCell(11);
            cell211.SetCellValue("计划状态");

            ICell cell212 = row2.CreateCell(12);
            cell212.SetCellValue("计划发布时间");

            ICell cell213 = row2.CreateCell(13);
            cell213.SetCellValue("备注");


            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (j == 11) //计划执行状态
                    {
                        if (dt.Rows[k][j].ToString() == "0")
                        {
                            dgvValue = "未上线";
                        }
                        else if (dt.Rows[k][j].ToString() == "1")
                        {
                            dgvValue = "执行中";
                        }
                        else if (dt.Rows[k][j].ToString() == "2")
                        {
                            dgvValue = "已上线";
                        }
                        else if (dt.Rows[k][j].ToString() == "3")
                        {
                            dgvValue = "已完成";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    else if (j==7)
                    {
                        if (dt.Rows[k][j].ToString() == "0")
                        {
                            dgvValue = "未校核";
                        }
                        else if (dt.Rows[k][j].ToString() == "1")
                        {
                            dgvValue = "已校核";
                        }
                        else if (dt.Rows[k][j].ToString() == "2")
                        {
                            dgvValue = "冻结";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    else if (j == 8)
                    {
                        if (dt.Rows[k][j].ToString() == "0")
                        {
                            dgvValue = "未打印";
                        }
                        else if (dt.Rows[k][j].ToString() == "1")
                        {
                            dgvValue = "已打印";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }
                    }
                    else
                    {
                        if (dt.Rows[k][j].ToString() == "&nbsp;")
                        {
                            dgvValue = "";
                        }
                        else
                        {
                            dgvValue = dt.Rows[k][j].ToString();
                        }

                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 47.物料加注配置信息导出
        /// <summary>
        /// 日计划表导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_MatJZConfig(DataTable dt, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "物料加注配置信息导出";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;

            ICell cell200 = row2.CreateCell(0);
            cell200.SetCellValue("物料编号");

            ICell cell201 = row2.CreateCell(1);
            cell201.SetCellValue("加注类型");

            ICell cell202 = row2.CreateCell(2);
            cell202.SetCellValue("加注量");

            ICell cell203 = row2.CreateCell(3);
            cell203.SetCellValue("创建时间");

            ICell cell204 = row2.CreateCell(4);
            cell204.SetCellValue("创建人编号");

            ICell cell205 = row2.CreateCell(5);
            cell205.SetCellValue("创建人名称");

            ICell cell206 = row2.CreateCell(6);
            cell206.SetCellValue("修改时间");

            ICell cell207 = row2.CreateCell(7);
            cell207.SetCellValue("修改人编号");

            ICell cell208 = row2.CreateCell(8);
            cell208.SetCellValue("修改人名称");

            ICell cell209 = row2.CreateCell(9);
            cell209.SetCellValue("备注");
            
            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();

                    if (dt.Rows[k][j].ToString() == "&nbsp;")
                    {
                        dgvValue = "";
                    }
                    else
                    {
                        dgvValue = dt.Rows[k][j].ToString();
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #region 100.产品上下线导出
        /// <summary>
        /// 设备信息导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelType"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel_Online(DataTable dt, DataTable dtname, string excelType)
        {
            IWorkbook workbook = null;
            if (excelType == "xls")
            {
                workbook = new HSSFWorkbook();//2003
            }
            else
            {
                workbook = new XSSFWorkbook();//2007
            }
            #region 创建一个sheet
            string name = "产品上下线数据";
            ISheet sheet = workbook.CreateSheet(name);
            int rowCount = 0;

            //设置全局列宽和行高   
            sheet.DefaultColumnWidth = 20; //全局列宽   
            sheet.DefaultRowHeightInPoints = 15; //全局行高

            IRow row2 = sheet.CreateRow(rowCount);
            row2.Height = 400;
            for (int i = 0; i < dtname.Rows.Count; i++)
            {
                row2.CreateCell(i).SetCellValue(dtname.Rows[i]["names"].ToString());
            }


            for (int k = 0; k < dt.Rows.Count; k++)
            {
                IRow row3 = sheet.CreateRow(k + rowCount + 1);//是内容行
                row3.Height = 400;
                int b = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dgvValue = string.Empty;
                    //dgvValue = dt.Rows[k][j].ToString();
                    if (dt.Rows[k][j].ToString() == "&nbsp;")
                    {
                        dgvValue = "";
                    }
                    else
                    {
                        dgvValue = dt.Rows[k][j].ToString();
                    }
                    ICell cell = row3.CreateCell(b);
                    cell.SetCellValue(dgvValue);
                    b++;
                }
            }
            #endregion

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        #endregion
        #endregion
    }
}
