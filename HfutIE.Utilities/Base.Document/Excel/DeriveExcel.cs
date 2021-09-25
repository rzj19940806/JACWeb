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


    }
}
