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
    public class DeriveExcelDIY
    {
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

            ICell cell11 = row.CreateCell(11);
            cell11.SetCellValue("是否挂单（0/6）");

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


        //采购到货信息导出
        public static MemoryStream ExportExcel_Arrive(DataSet ds, string excelType)
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


                //byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\Administrator\Desktop\常用图标\意通logo.png");
                //int pictureIdx = workbook.AddPicture(bytes, PictureType.PNG);
                //HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                //HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, 1, 1);
                ////把图片插到相应的位置
                //HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);

                //设置标题行数据   
                //int a = 0;
                IRow row1 = sheet.CreateRow(rowCount); //创建报表表头标题列  
                row1.Height = 500;
                //row1.RowStyle.SetFont(workbook.CreateFont().Boldweight);= FontBoldWeight.BOLD;
                ICell cell0 = row1.CreateCell(0);
                cell0.SetCellValue("采购到货单");

                IRow row1_1 = sheet.CreateRow(rowCount + 1);
                row1_1.Height = 500;

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
                //IFont fontRight = workbook.CreateFont();
                //fontRight.FontHeightInPoints = 16;
                //fontRight.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
                //fontRight.FontName = "宋体";
                //styleCommonRight.SetFont(fontRight);
                //row1.GetCell(0).CellStyle.SetFont(fontRight);




                IRow row2 = sheet.CreateRow(rowCount + 2);
                row2.Height = 400;

                ICell cell21 = row2.CreateCell(0);
                cell21.SetCellValue("采购单号");
                ICell cell22 = row2.CreateCell(1);
                cell22.SetCellValue(ds.Tables[p - 1].Rows[0]["bill_code"].ToString());

                ICell cell23 = row2.CreateCell(2);
                cell23.SetCellValue("项目编号");
                ICell cell24 = row2.CreateCell(3);
                cell24.SetCellValue(ds.Tables[p - 1].Rows[0]["project_code"].ToString());

                ICell cell25 = row2.CreateCell(4);
                cell25.SetCellValue("项目名称");
                ICell cell26 = row2.CreateCell(5);
                cell26.SetCellValue(ds.Tables[p - 1].Rows[0]["project_name"].ToString());

                ICell cell27 = row2.CreateCell(6);
                cell27.SetCellValue("到货时间");
                ICell cell28 = row2.CreateCell(7);
                cell28.SetCellValue(ds.Tables[p - 1].Rows[0]["arrive_time"].ToString());


                IRow row3 = sheet.CreateRow(rowCount + 3);
                row3.Height = 400;

                ICell cell31 = row3.CreateCell(0);
                cell31.SetCellValue("收货人");
                ICell cell32 = row3.CreateCell(1);
                cell32.SetCellValue(ds.Tables[p - 1].Rows[0]["operator_name"].ToString());

                ICell cell33 = row3.CreateCell(2);
                cell33.SetCellValue("下单时间");
                ICell cell34 = row3.CreateCell(3);
                cell34.SetCellValue(ds.Tables[p - 1].Rows[0]["order_time"].ToString());
                CellRangeAddress cra1 = new CellRangeAddress(3, 3, 3, 4);
                sheet.AddMergedRegion(cra1);

                ICell cell36 = row3.CreateCell(5);
                cell36.SetCellValue("交货期");
                ICell cell37 = row3.CreateCell(6);
                cell37.SetCellValue(ds.Tables[p - 1].Rows[0]["due_time"].ToString());
                CellRangeAddress cra2 = new CellRangeAddress(3, 3, 6, 7);
                sheet.AddMergedRegion(cra2);

                IRow row4 = sheet.CreateRow(rowCount + 4);
                row4.Height = 400;
                ICell cell41 = row4.CreateCell(0);
                cell41.SetCellValue("物料号");

                ICell cell42 = row4.CreateCell(1);
                cell42.SetCellValue("物料名称");

                ICell cell43 = row4.CreateCell(2);
                cell43.SetCellValue("到货数量");

                ICell cell44 = row4.CreateCell(3);
                cell44.SetCellValue("计量单位");

                ICell cell45 = row4.CreateCell(4);
                cell45.SetCellValue("供应商编号");

                ICell cell46 = row4.CreateCell(5);
                cell46.SetCellValue("供应商名称");

                ICell cell47 = row4.CreateCell(6);
                cell47.SetCellValue("备注");
                CellRangeAddress cra3 = new CellRangeAddress(4, 4, 6, 7);
                sheet.AddMergedRegion(cra3);

                for (int k = 0; k < ds.Tables[p].Rows.Count; k++)
                {
                    IRow row5 = sheet.CreateRow(k + rowCount + 5);
                    row5.Height = 400;
                    int b = 0;
                    for (int j = 0; j < 7; j++)
                    {
                        string dgvValue = string.Empty;
                        dgvValue = ds.Tables[p].Rows[k][j].ToString();
                        
                        ICell cell = row5.CreateCell(b);
                        if (j == 6)
                        {
                            ICell cell6 = row5.CreateCell(b);
                            CellRangeAddress cra4 = new CellRangeAddress(5+k, 5+k, 6, 7);
                            sheet.AddMergedRegion(cra4);
                        }
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



        //物料入库信息导出
        public static MemoryStream ExportExcel_PartInstock(DataSet ds, string excelType)
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
                string name = ds.Tables[((p - 1))].Rows[0]["purbill_code"].ToString();
                ISheet sheet = workbook.CreateSheet();
                //sheet.SheetName = ds.Tables[((p + 1) / 2)].Rows[0]["bill_code"].ToString();
                //设置大标题行   
                int rowCount = 0;

                //设置全局列宽和行高   
                sheet.DefaultColumnWidth = 15; //全局列宽   
                sheet.DefaultRowHeightInPoints = 15; //全局行高


                //byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\Administrator\Desktop\常用图标\意通logo.png");
                //int pictureIdx = workbook.AddPicture(bytes, PictureType.PNG);
                //HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                //HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, 1, 1);
                ////把图片插到相应的位置
                //HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);

                //设置标题行数据   
                //int a = 0;
                IRow row1 = sheet.CreateRow(rowCount); //创建报表表头标题列  
                row1.Height = 500;
                //row1.RowStyle.SetFont(workbook.CreateFont().Boldweight);= FontBoldWeight.BOLD;
                ICell cell0 = row1.CreateCell(0);
                cell0.SetCellValue("物料入库单");

                IRow row1_1 = sheet.CreateRow(rowCount + 1);
                row1_1.Height = 500;

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
                //IFont fontRight = workbook.CreateFont();
                //fontRight.FontHeightInPoints = 16;
                //fontRight.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
                //fontRight.FontName = "宋体";
                //styleCommonRight.SetFont(fontRight);
                //row1.GetCell(0).CellStyle.SetFont(fontRight);




                IRow row2 = sheet.CreateRow(rowCount + 2);
                row2.Height = 400;

                ICell cell21 = row2.CreateCell(0);
                cell21.SetCellValue("采购单号：");
                ICell cell22 = row2.CreateCell(1);
                cell22.SetCellValue(ds.Tables[p - 1].Rows[0]["purbill_code"].ToString());

                ICell cell23 = row2.CreateCell(2);
                cell23.SetCellValue("项目编号：");
                ICell cell24 = row2.CreateCell(3);
                cell24.SetCellValue(ds.Tables[p - 1].Rows[0]["project_code"].ToString());

                ICell cell25 = row2.CreateCell(4);
                cell25.SetCellValue("项目名称：");
                ICell cell26 = row2.CreateCell(5);
                cell26.SetCellValue(ds.Tables[p - 1].Rows[0]["project_name"].ToString());

                ICell cell27 = row2.CreateCell(6);
                cell27.SetCellValue("入库时间：");
                ICell cell28 = row2.CreateCell(7);
                cell28.SetCellValue(ds.Tables[p - 1].Rows[0]["instock_time"].ToString());

                ICell cell29 = row2.CreateCell(8);
                cell29.SetCellValue("入库人：");
                ICell cell210 = row2.CreateCell(9);
                cell210.SetCellValue(ds.Tables[p - 1].Rows[0]["operator_name"].ToString());


                IRow row3 = sheet.CreateRow(rowCount + 3);
                row3.Height = 400;
                ICell cell31 = row3.CreateCell(0);
                cell31.SetCellValue("物料号");

                ICell cell32 = row3.CreateCell(1);
                cell32.SetCellValue("物料名称");

                ICell cell33 = row3.CreateCell(2);
                cell33.SetCellValue("入库数量");

                ICell cell34 = row3.CreateCell(3);
                cell34.SetCellValue("计量单位");

                ICell cell35 = row3.CreateCell(4);
                cell35.SetCellValue("供应商编号");

                ICell cell36 = row3.CreateCell(5);
                cell36.SetCellValue("供应商名称");

                ICell cell37 = row3.CreateCell(6);
                cell37.SetCellValue("备注");
                CellRangeAddress cra3 = new CellRangeAddress(3, 3, 6, 9);
                sheet.AddMergedRegion(cra3);

                for (int k = 0; k < ds.Tables[p].Rows.Count; k++)
                {
                    IRow row5 = sheet.CreateRow(k + rowCount + 4);
                    row5.Height = 400;
                    int b = 0;
                    for (int j = 0; j < 7; j++)
                    {
                        string dgvValue = string.Empty;
                        dgvValue = ds.Tables[p].Rows[k][j].ToString();

                        ICell cell = row5.CreateCell(b);
                        if (j == 6)
                        {
                            ICell cell6 = row5.CreateCell(b);
                            CellRangeAddress cra4 = new CellRangeAddress(4 + k, 4 + k, 6, 9);
                            sheet.AddMergedRegion(cra4);
                        }
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


        //物料出库信息导出
        public static MemoryStream ExportExcel_PartOutstock(DataSet ds, string excelType)
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
                string name = ds.Tables[((p - 1))].Rows[0]["outbill_code"].ToString();
                ISheet sheet = workbook.CreateSheet();
                //sheet.SheetName = ds.Tables[((p + 1) / 2)].Rows[0]["bill_code"].ToString();
                //设置大标题行   
                int rowCount = 0;

                //设置全局列宽和行高   
                sheet.DefaultColumnWidth = 15; //全局列宽   
                sheet.DefaultRowHeightInPoints = 15; //全局行高


                //byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\Administrator\Desktop\常用图标\意通logo.png");
                //int pictureIdx = workbook.AddPicture(bytes, PictureType.PNG);
                //HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                //HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, 1, 1);
                ////把图片插到相应的位置
                //HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);

                //设置标题行数据   
                //int a = 0;
                IRow row1 = sheet.CreateRow(rowCount); //创建报表表头标题列  
                row1.Height = 500;
                //row1.RowStyle.SetFont(workbook.CreateFont().Boldweight);= FontBoldWeight.BOLD;
                ICell cell0 = row1.CreateCell(0);
                cell0.SetCellValue("物料出库单");

                IRow row1_1 = sheet.CreateRow(rowCount + 1);
                row1_1.Height = 500;

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
                //IFont fontRight = workbook.CreateFont();
                //fontRight.FontHeightInPoints = 16;
                //fontRight.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
                //fontRight.FontName = "宋体";
                //styleCommonRight.SetFont(fontRight);
                //row1.GetCell(0).CellStyle.SetFont(fontRight);




                IRow row2 = sheet.CreateRow(rowCount + 2);
                row2.Height = 400;

                ICell cell21 = row2.CreateCell(0);
                cell21.SetCellValue("出库单号：");
                ICell cell22 = row2.CreateCell(1);
                cell22.SetCellValue(ds.Tables[p - 1].Rows[0]["outbill_code"].ToString());

                ICell cell23 = row2.CreateCell(2);
                cell23.SetCellValue("项目编号：");
                ICell cell24 = row2.CreateCell(3);
                cell24.SetCellValue(ds.Tables[p - 1].Rows[0]["project_code"].ToString());

                ICell cell25 = row2.CreateCell(4);
                cell25.SetCellValue("项目名称：");
                ICell cell26 = row2.CreateCell(5);
                cell26.SetCellValue(ds.Tables[p - 1].Rows[0]["project_name"].ToString());

                ICell cell27 = row2.CreateCell(6);
                cell27.SetCellValue("出库时间：");
                ICell cell28 = row2.CreateCell(7);
                cell28.SetCellValue(ds.Tables[p - 1].Rows[0]["outstock_time"].ToString());

                ICell cell29 = row2.CreateCell(8);
                cell29.SetCellValue("出库人：");
                ICell cell210 = row2.CreateCell(9);
                cell210.SetCellValue(ds.Tables[p - 1].Rows[0]["operator_name"].ToString());


                IRow row3 = sheet.CreateRow(rowCount + 3);
                row3.Height = 400;
                ICell cell31 = row3.CreateCell(0);
                cell31.SetCellValue("物料号");
                CellRangeAddress cra1 = new CellRangeAddress(3, 3, 0, 1);
                sheet.AddMergedRegion(cra1);

                ICell cell32 = row3.CreateCell(2);
                cell32.SetCellValue("物料名称");
                CellRangeAddress cra2 = new CellRangeAddress(3, 3, 2, 3);
                sheet.AddMergedRegion(cra2);

                ICell cell33 = row3.CreateCell(4);
                cell33.SetCellValue("出库数量");
                CellRangeAddress cra3 = new CellRangeAddress(3, 3, 4, 5);
                sheet.AddMergedRegion(cra3);

                ICell cell34 = row3.CreateCell(6);
                cell34.SetCellValue("计量单位");
                CellRangeAddress cra4 = new CellRangeAddress(3, 3, 6, 7);
                sheet.AddMergedRegion(cra4);

                ICell cell37 = row3.CreateCell(8);
                cell37.SetCellValue("备注");
                CellRangeAddress cra5 = new CellRangeAddress(3, 3, 8, 9);
                sheet.AddMergedRegion(cra5);

                for (int k = 0; k < ds.Tables[p].Rows.Count; k++)
                {
                    IRow row5 = sheet.CreateRow(k + rowCount + 4);
                    row5.Height = 400;
                    int b = 0;
                    for (int j = 0; j < 5; j++)
                    {
                        string dgvValue = string.Empty;
                        dgvValue = ds.Tables[p].Rows[k][j].ToString();

                        ICell cell = row5.CreateCell(b);
                        cell.SetCellValue(dgvValue);
                        CellRangeAddress cra6 = new CellRangeAddress(4+k, 4+k, b, b+1);
                        sheet.AddMergedRegion(cra6);
                        //  cell.CellStyle = style;
                        b +=2;
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

        //产品入库信息导出
        public static MemoryStream ExportExcel_ProInstock(DataSet ds, string excelType)
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
                string name = ds.Tables[((p - 1))].Rows[0]["inbill_code"].ToString();
                ISheet sheet = workbook.CreateSheet();
                //sheet.SheetName = ds.Tables[((p + 1) / 2)].Rows[0]["bill_code"].ToString();
                //设置大标题行   
                int rowCount = 0;

                //设置全局列宽和行高   
                sheet.DefaultColumnWidth = 15; //全局列宽   
                sheet.DefaultRowHeightInPoints = 15; //全局行高


                //byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\Administrator\Desktop\常用图标\意通logo.png");
                //int pictureIdx = workbook.AddPicture(bytes, PictureType.PNG);
                //HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                //HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, 1, 1);
                ////把图片插到相应的位置
                //HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);

                //设置标题行数据   
                //int a = 0;
                IRow row1 = sheet.CreateRow(rowCount); //创建报表表头标题列  
                row1.Height = 500;
                //row1.RowStyle.SetFont(workbook.CreateFont().Boldweight);= FontBoldWeight.BOLD;
                ICell cell0 = row1.CreateCell(0);
                cell0.SetCellValue("产品入库单");

                IRow row1_1 = sheet.CreateRow(rowCount + 1);
                row1_1.Height = 500;

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
                //IFont fontRight = workbook.CreateFont();
                //fontRight.FontHeightInPoints = 16;
                //fontRight.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
                //fontRight.FontName = "宋体";
                //styleCommonRight.SetFont(fontRight);
                //row1.GetCell(0).CellStyle.SetFont(fontRight);




                IRow row2 = sheet.CreateRow(rowCount + 2);
                row2.Height = 400;

                ICell cell21 = row2.CreateCell(0);
                cell21.SetCellValue("入库单号：");
                ICell cell22 = row2.CreateCell(1);
                cell22.SetCellValue(ds.Tables[p - 1].Rows[0]["inbill_code"].ToString());

                ICell cell23 = row2.CreateCell(2);
                cell23.SetCellValue("项目编号：");
                ICell cell24 = row2.CreateCell(3);
                cell24.SetCellValue(ds.Tables[p - 1].Rows[0]["project_code"].ToString());

                ICell cell25 = row2.CreateCell(4);
                cell25.SetCellValue("项目名称：");
                ICell cell26 = row2.CreateCell(5);
                cell26.SetCellValue(ds.Tables[p - 1].Rows[0]["project_name"].ToString());

                ICell cell27 = row2.CreateCell(6);
                cell27.SetCellValue("入库时间：");
                ICell cell28 = row2.CreateCell(7);
                cell28.SetCellValue(ds.Tables[p - 1].Rows[0]["instock_time"].ToString());

                ICell cell29 = row2.CreateCell(8);
                cell29.SetCellValue("入库人：");
                ICell cell210 = row2.CreateCell(9);
                cell210.SetCellValue(ds.Tables[p - 1].Rows[0]["operator_name"].ToString());


                IRow row3 = sheet.CreateRow(rowCount + 3);
                row3.Height = 400;
                ICell cell31 = row3.CreateCell(0);
                cell31.SetCellValue("产品号");
                CellRangeAddress cra1 = new CellRangeAddress(3, 3, 0, 1);
                sheet.AddMergedRegion(cra1);

                ICell cell32 = row3.CreateCell(2);
                cell32.SetCellValue("出生证号");
                CellRangeAddress cra2 = new CellRangeAddress(3, 3, 2, 3);
                sheet.AddMergedRegion(cra2);

                ICell cell33 = row3.CreateCell(4);
                cell33.SetCellValue("产品名称");
                CellRangeAddress cra3 = new CellRangeAddress(3, 3, 4, 5);
                sheet.AddMergedRegion(cra3);

                ICell cell34 = row3.CreateCell(6);
                cell34.SetCellValue("入库数量");

                ICell cell35 = row3.CreateCell(7);
                cell35.SetCellValue("计量单位");


                ICell cell37 = row3.CreateCell(8);
                cell37.SetCellValue("备注");
                CellRangeAddress cra4 = new CellRangeAddress(3, 3, 8, 9);
                sheet.AddMergedRegion(cra4);

                for (int k = 0; k < ds.Tables[p].Rows.Count; k++)
                {
                    IRow row5 = sheet.CreateRow(k + rowCount + 4);
                    row5.Height = 400;
                    int b = 0;
                    for (int j = 0; j < 6; j++)
                    {
                        string dgvValue = string.Empty;
                        dgvValue = ds.Tables[p].Rows[k][j].ToString();
                        if (j==0||j == 1 || j == 2 || j == 5)
                        {
                            

                            ICell cell = row5.CreateCell(b);
                            cell.SetCellValue(dgvValue);
                            CellRangeAddress cra6 = new CellRangeAddress(4 + k, 4 + k, b, b + 1);
                            sheet.AddMergedRegion(cra6);
                            //  cell.CellStyle = style;
                            b += 2;
                        }
                        else
                        {
                            ICell cell1 = row5.CreateCell(b);
                            cell1.SetCellValue(dgvValue);
                            b++;
                        }
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



        //产品出库信息导出
        public static MemoryStream ExportExcel_ProOutstock(DataSet ds, string excelType)
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
                string name = ds.Tables[((p - 1))].Rows[0]["d_bill_code"].ToString();
                ISheet sheet = workbook.CreateSheet();
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
                IRow row1 = sheet.CreateRow(rowCount); //创建报表表头标题列  
                row1.Height = 500;
                //row1.RowStyle.SetFont(workbook.CreateFont().Boldweight);= FontBoldWeight.BOLD;
                ICell cell0 = row1.CreateCell(0);
                cell0.SetCellValue("产品出库单");

                IRow row1_1 = sheet.CreateRow(rowCount + 1);
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
                cell22.SetCellValue(ds.Tables[p - 1].Rows[0]["d_bill_code"].ToString());

                ICell cell23 = row2.CreateCell(2);
                cell23.SetCellValue("项目编号");
                ICell cell24 = row2.CreateCell(3);
                cell24.SetCellValue(ds.Tables[p - 1].Rows[0]["project_code"].ToString());

                ICell cell25 = row2.CreateCell(4);
                cell25.SetCellValue("项目名称");
                ICell cell26 = row2.CreateCell(5);
                cell26.SetCellValue(ds.Tables[p - 1].Rows[0]["project_name"].ToString());


                IRow row3 = sheet.CreateRow(rowCount + 3);
                row3.Height = 400;

                ICell cell31 = row3.CreateCell(0);
                cell31.SetCellValue("销售订单编号");
                ICell cell32 = row3.CreateCell(1);
                cell32.SetCellValue(ds.Tables[p - 1].Rows[0]["salebill_code"].ToString());

                ICell cell33 = row3.CreateCell(2);
                cell33.SetCellValue("出库时间");
                ICell cell34 = row3.CreateCell(3);
                cell34.SetCellValue(ds.Tables[p - 1].Rows[0]["outstock_time"].ToString());

                ICell cell35 = row3.CreateCell(4);
                cell35.SetCellValue("出库人");
                ICell cell36 = row3.CreateCell(5);
                cell36.SetCellValue(ds.Tables[p - 1].Rows[0]["operator_name"].ToString());

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


        //产品盘点信息导出
        public static MemoryStream ExportExcel_ProStockTaking(DataSet ds, string excelType)
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
                ISheet sheet = workbook.CreateSheet();
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
                IRow row1 = sheet.CreateRow(rowCount); //创建报表表头标题列  
                row1.Height = 500;
                //row1.RowStyle.SetFont(workbook.CreateFont().Boldweight);= FontBoldWeight.BOLD;
                ICell cell0 = row1.CreateCell(0);
                cell0.SetCellValue("产品盘点单");

                IRow row1_1 = sheet.CreateRow(rowCount + 1);
                row1_1.Height = 500;

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
                //IFont fontRight = workbook.CreateFont();
                //fontRight.FontHeightInPoints = 16;
                //fontRight.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
                //fontRight.FontName = "宋体";
                //styleCommonRight.SetFont(fontRight);
                //row1.GetCell(0).CellStyle.SetFont(fontRight);




                IRow row2 = sheet.CreateRow(rowCount + 2);
                row2.Height = 400;

                ICell cell21 = row2.CreateCell(0);
                cell21.SetCellValue("单据编号：");
                ICell cell22 = row2.CreateCell(1);
                cell22.SetCellValue(ds.Tables[p - 1].Rows[0]["bill_code"].ToString());

                ICell cell23 = row2.CreateCell(2);
                cell23.SetCellValue("创建时间：");
                ICell cell24 = row2.CreateCell(3);
                cell24.SetCellValue(ds.Tables[p - 1].Rows[0]["create_time"].ToString());

                ICell cell25 = row2.CreateCell(4);
                cell25.SetCellValue("盘点时间：");
                ICell cell26 = row2.CreateCell(5);
                cell26.SetCellValue(ds.Tables[p - 1].Rows[0]["stocktaking_time"].ToString());

                ICell cell27 = row2.CreateCell(6);
                cell27.SetCellValue("盘点人：");
                ICell cell28 = row2.CreateCell(5);
                cell28.SetCellValue(ds.Tables[p - 1].Rows[0]["operator_name"].ToString());

                IRow row3 = sheet.CreateRow(rowCount + 3);
                row3.Height = 400;
                ICell cell31 = row3.CreateCell(0);
                cell31.SetCellValue("产品出生证");

                ICell cell32 = row3.CreateCell(1);
                cell32.SetCellValue("产品名称");

                ICell cell33 = row3.CreateCell(2);
                cell33.SetCellValue("当前数量");

                ICell cell34 = row3.CreateCell(3);
                cell34.SetCellValue("实际数量");

                ICell cell35 = row3.CreateCell(4);
                cell35.SetCellValue("单位");

                ICell cell36 = row3.CreateCell(5);
                cell36.SetCellValue("库区");

                ICell cell37 = row3.CreateCell(6);
                cell37.SetCellValue("库位");

                ICell cell38 = row3.CreateCell(7);
                cell38.SetCellValue("备注");

                for (int k = 0; k < ds.Tables[p].Rows.Count; k++)
                {
                    IRow row7 = sheet.CreateRow(k + rowCount + 4);
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

        //物料盘点信息导出
        public static MemoryStream ExportExcel_PartStockTaking(DataSet ds, string excelType)
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
                ISheet sheet = workbook.CreateSheet();
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
                IRow row1 = sheet.CreateRow(rowCount); //创建报表表头标题列  
                row1.Height = 500;
                //row1.RowStyle.SetFont(workbook.CreateFont().Boldweight);= FontBoldWeight.BOLD;
                ICell cell0 = row1.CreateCell(0);
                cell0.SetCellValue("物料盘点单");

                IRow row1_1 = sheet.CreateRow(rowCount + 1);
                row1_1.Height = 500;

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
                //IFont fontRight = workbook.CreateFont();
                //fontRight.FontHeightInPoints = 16;
                //fontRight.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
                //fontRight.FontName = "宋体";
                //styleCommonRight.SetFont(fontRight);
                //row1.GetCell(0).CellStyle.SetFont(fontRight);




                IRow row2 = sheet.CreateRow(rowCount + 2);
                row2.Height = 400;

                ICell cell21 = row2.CreateCell(0);
                cell21.SetCellValue("单据编号：");
                ICell cell22 = row2.CreateCell(1);
                cell22.SetCellValue(ds.Tables[p - 1].Rows[0]["bill_code"].ToString());

                ICell cell23 = row2.CreateCell(2);
                cell23.SetCellValue("创建时间：");
                ICell cell24 = row2.CreateCell(3);
                cell24.SetCellValue(ds.Tables[p - 1].Rows[0]["create_time"].ToString());

                ICell cell25 = row2.CreateCell(4);
                cell25.SetCellValue("盘点时间：");
                ICell cell26 = row2.CreateCell(5);
                cell26.SetCellValue(ds.Tables[p - 1].Rows[0]["stocktaking_time"].ToString());

                ICell cell27 = row2.CreateCell(6);
                cell27.SetCellValue("盘点人：");
                ICell cell28 = row2.CreateCell(5);
                cell28.SetCellValue(ds.Tables[p - 1].Rows[0]["operator_name"].ToString());

                IRow row3 = sheet.CreateRow(rowCount + 3);
                row3.Height = 400;
                ICell cell31 = row3.CreateCell(0);
                cell31.SetCellValue("物料号");

                ICell cell32 = row3.CreateCell(1);
                cell32.SetCellValue("物料名称");

                ICell cell33 = row3.CreateCell(2);
                cell33.SetCellValue("当前数量");

                ICell cell34 = row3.CreateCell(3);
                cell34.SetCellValue("实际数量");

                ICell cell35 = row3.CreateCell(4);
                cell35.SetCellValue("单位");

                ICell cell36 = row3.CreateCell(5);
                cell36.SetCellValue("库区");

                ICell cell37 = row3.CreateCell(6);
                cell37.SetCellValue("库位");

                ICell cell38 = row3.CreateCell(7);
                cell38.SetCellValue("备注");

                for (int k = 0; k < ds.Tables[p].Rows.Count; k++)
                {
                    IRow row7 = sheet.CreateRow(k + rowCount + 4);
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

        //物料移库信息导出
        public static MemoryStream ExportExcel_Transfer(DataSet ds, string excelType)
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
                ISheet sheet = workbook.CreateSheet();
                //sheet.SheetName = ds.Tables[((p + 1) / 2)].Rows[0]["bill_code"].ToString();
                //设置大标题行   
                int rowCount = 0;

                //设置全局列宽和行高   
                sheet.DefaultColumnWidth = 15; //全局列宽   
                sheet.DefaultRowHeightInPoints = 15; //全局行高


                //byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\Administrator\Desktop\常用图标\意通logo.png");
                //int pictureIdx = workbook.AddPicture(bytes, PictureType.PNG);
                //HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                //HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, 1, 1);
                ////把图片插到相应的位置
                //HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);

                //设置标题行数据   
                //int a = 0;
                IRow row1 = sheet.CreateRow(rowCount); //创建报表表头标题列  
                row1.Height = 500;
                //row1.RowStyle.SetFont(workbook.CreateFont().Boldweight);= FontBoldWeight.BOLD;
                ICell cell0 = row1.CreateCell(0);
                cell0.SetCellValue("物料移库单");

                IRow row1_1 = sheet.CreateRow(rowCount + 1);
                row1_1.Height = 500;

                CellRangeAddress cra = new CellRangeAddress(0, 1, 0, 8);//起始行，终止行，起始列，终止列
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
                cell21.SetCellValue("单据编号：");
                ICell cell22 = row2.CreateCell(1);
                cell22.SetCellValue(ds.Tables[p - 1].Rows[0]["bill_code"].ToString());

                ICell cell23 = row2.CreateCell(2);
                cell23.SetCellValue("创建时间：");
                ICell cell24 = row2.CreateCell(3);
                cell24.SetCellValue(ds.Tables[p - 1].Rows[0]["create_time"].ToString());

                ICell cell25 = row2.CreateCell(4);
                cell25.SetCellValue("移库时间：");
                ICell cell26 = row2.CreateCell(5);
                cell26.SetCellValue(ds.Tables[p - 1].Rows[0]["transfer_time"].ToString());

                ICell cell27 = row2.CreateCell(6);
                cell27.SetCellValue("移库人：");
                ICell cell28 = row2.CreateCell(5);
                cell28.SetCellValue(ds.Tables[p - 1].Rows[0]["operator_name"].ToString());
                CellRangeAddress cra1 = new CellRangeAddress(2, 2, 7, 8);//起始行，终止行，起始列，终止列
                sheet.AddMergedRegion(cra1);

                IRow row3 = sheet.CreateRow(rowCount + 3);
                row3.Height = 400;
                ICell cell31 = row3.CreateCell(0);
                cell31.SetCellValue("原库区");

                ICell cell32 = row3.CreateCell(1);
                cell32.SetCellValue("原库位");

                row3.Height = 400;
                ICell cell33 = row3.CreateCell(2);
                cell33.SetCellValue("物料号");

                ICell cell34 = row3.CreateCell(3);
                cell34.SetCellValue("物料名称");

                ICell cell35 = row3.CreateCell(4);
                cell35.SetCellValue("移库数量");


                ICell cell36 = row3.CreateCell(5);
                cell36.SetCellValue("单位");

                ICell cell37 = row3.CreateCell(6);
                cell37.SetCellValue("新库区");

                ICell cell38 = row3.CreateCell(7);
                cell38.SetCellValue("新库位");

                ICell cell39 = row3.CreateCell(8);
                cell39.SetCellValue("备注");

                for (int k = 0; k < ds.Tables[p].Rows.Count; k++)
                {
                    IRow row7 = sheet.CreateRow(k + rowCount + 4);
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

        //下载库存导入模板
        public static MemoryStream ExportStockModelExcel(string excelType)
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
            cell0.SetCellValue("物料号");

            ICell cell1 = row.CreateCell(1);
            cell1.SetCellValue("物料名称");

            ICell cell2 = row.CreateCell(2);
            cell2.SetCellValue("入库时间");

            ICell cell3 = row.CreateCell(3);
            cell3.SetCellValue("库位编号");

            ICell cell4 = row.CreateCell(4);
            cell4.SetCellValue("库区编号");

            ICell cell5 = row.CreateCell(5);
            cell5.SetCellValue("当前数量");

            ICell cell6 = row.CreateCell(6);
            cell6.SetCellValue("单位");

            ICell cell7 = row.CreateCell(7);
            cell7.SetCellValue("备注");

            //创建excel   
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        public static MemoryStream ExportExcel_PartApply(DataSet ds, string excelType)
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
                ISheet sheet = workbook.CreateSheet();
                //sheet.SheetName = ds.Tables[((p + 1) / 2)].Rows[0]["bill_code"].ToString();
                //设置大标题行   
                int rowCount = 0;

                //设置全局列宽和行高   
                sheet.DefaultColumnWidth = 16; //全局列宽   
                sheet.DefaultRowHeightInPoints = 12; //全局行高


                //byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\Administrator\Desktop\常用图标\意通logo.png");
                //int pictureIdx = workbook.AddPicture(bytes, PictureType.PNG);
                //HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                //HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, 1, 1);
                ////把图片插到相应的位置
                //HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);

                //设置标题行数据   
                //int a = 0;
                IRow row1 = sheet.CreateRow(rowCount); //创建报表表头标题列  
                row1.Height = 500;
                //row1.RowStyle.SetFont(workbook.CreateFont().Boldweight);= FontBoldWeight.BOLD;
                ICell cell0 = row1.CreateCell(0);
                cell0.SetCellValue("物料申购单");

                CellRangeAddress cra = new CellRangeAddress(0, 1, 0, 6);//起始行，终止行，起始列，终止列
                sheet.AddMergedRegion(cra);

                //row1.RowStyle = workbook.CreateCellStyle();
                //ICellStyle styleCommonRight = workbook.CreateCellStyle();
                cell0.CellStyle.VerticalAlignment = VerticalAlignment.CENTER;
                cell0.CellStyle.Alignment = HorizontalAlignment.CENTER;
                //cell0.CellStyle.BorderBottom = BorderStyle.THIN;
                //cell0.CellStyle.BorderLeft = BorderStyle.THIN;
                //cell0.CellStyle.BorderRight = BorderStyle.THIN;
                //cell0.CellStyle.BorderTop = BorderStyle.THIN;
                //IFont fontRight = workbook.CreateFont();
                //fontRight.FontHeightInPoints = 16;
                //fontRight.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
                //fontRight.FontName = "宋体";
                //styleCommonRight.SetFont(fontRight);
                //row1.GetCell(0).CellStyle.SetFont(fontRight);




                IRow row2 = sheet.CreateRow(rowCount + 2);
                row2.Height = 400;

                ICell cell21 = row2.CreateCell(0);
                cell21.SetCellValue("单据编号：");
                ICell cell22 = row2.CreateCell(1);
                cell22.SetCellValue(ds.Tables[p - 1].Rows[0]["bill_code"].ToString());

                ICell cell23 = row2.CreateCell(2);
                cell23.SetCellValue("项目编号：");
                ICell cell24 = row2.CreateCell(3);
                cell24.SetCellValue(ds.Tables[p - 1].Rows[0]["project_code"].ToString());

                ICell cell25 = row2.CreateCell(4);
                cell25.SetCellValue("项目名称：");
                ICell cell26 = row2.CreateCell(5);
                cell26.SetCellValue(ds.Tables[p - 1].Rows[0]["project_name"].ToString());
                CellRangeAddress cra1 = new CellRangeAddress(2, 2, 5, 6);//起始行，终止行，起始列，终止列
                sheet.AddMergedRegion(cra1);

                IRow row3 = sheet.CreateRow(rowCount + 3);
                row3.Height = 400;

                ICell cell31 = row3.CreateCell(0);
                cell31.SetCellValue("申购部门：");
                ICell cell32 = row3.CreateCell(1);
                cell32.SetCellValue(ds.Tables[p - 1].Rows[0]["apply_dep_name"].ToString());

                ICell cell33 = row3.CreateCell(2);
                cell33.SetCellValue("申购人：");
                ICell cell34 = row3.CreateCell(3);
                cell34.SetCellValue(ds.Tables[p - 1].Rows[0]["creator_name"].ToString());

                ICell cell35 = row3.CreateCell(4);
                cell35.SetCellValue("申购时间：");
                ICell cell36 = row3.CreateCell(5);
                cell36.SetCellValue(ds.Tables[p - 1].Rows[0]["create_time"].ToString());
                CellRangeAddress cra2 = new CellRangeAddress(3, 3, 5, 6);//起始行，终止行，起始列，终止列
                sheet.AddMergedRegion(cra2);

                IRow row4 = sheet.CreateRow(rowCount + 4);
                row4.Height = 400;
                ICell cell41 = row4.CreateCell(0);
                cell41.SetCellValue("物料号");

                ICell cell42 = row4.CreateCell(1);
                cell42.SetCellValue("物料名称");

                ICell cell43 = row4.CreateCell(2);
                cell43.SetCellValue("安全库存");

                ICell cell44 = row4.CreateCell(3);
                cell44.SetCellValue("当前库存");

                ICell cell45 = row4.CreateCell(4);
                cell45.SetCellValue("申购数量");

                ICell cell46 = row4.CreateCell(5);
                cell46.SetCellValue("计量单位");

                ICell cell47 = row4.CreateCell(6);
                cell47.SetCellValue("备注");

                for (int k = 0; k < ds.Tables[p].Rows.Count; k++)
                {
                    IRow row7 = sheet.CreateRow(k + rowCount + 5);
                    row7.Height = 400;
                    int b = 0;
                    for (int j = 0; j < ds.Tables[p].Columns.Count; j++)
                    {
                        string dgvValue = string.Empty;
                        dgvValue = ds.Tables[p].Rows[k][j].ToString();
                        ICell cell = row7.CreateCell(b);
                        cell.SetCellValue(dgvValue);
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
