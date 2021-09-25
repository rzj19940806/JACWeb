using HfutIE.Business;
using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HfutIE.WebApp.Areas.BaseModule.Controllers
{
    /// <summary>
    /// 缺陷类别表控制器
    /// </summary>
    public class BBdbR_DefectCatgBaseController : PublicController<BBdbR_DefectCatgBase>
    {
        //#region 10.导入
        

        ///// <summary>
        ///// 清除Datatable空行
        ///// </summary>
        ///// <param name="dt"></param>
        //public void RemoveEmpty(DataTable dt)
        //{
        //    List<DataRow> removelist = new List<DataRow>();
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        bool rowdataisnull = true;
        //        for (int j = 0; j < dt.Columns.Count; j++)
        //        {
        //            if (!string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()))
        //            {

        //                rowdataisnull = false;
        //            }
        //        }
        //        if (rowdataisnull)
        //        {
        //            removelist.Add(dt.Rows[i]);
        //        }

        //    }
        //    for (int i = 0; i < removelist.Count; i++)
        //    {
        //        dt.Rows.Remove(removelist[i]);
        //    }
        //}

        ///// <summary>
        ///// 导入Excell数据
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult ImportExel(string AlarmId)
        //{
        //    int IsOk = 0;//导入状态
        //    int IsCheck = 1;//用作检验重复地址的标识
        //    DataTable Result = new DataTable();//导入错误记录表
        //    IDatabase database = DataFactory.Database();
        //    List<BBdbR_DvcBase> DeviceList = new List<BBdbR_DvcBase>();

        //    //构造导入返回结果表
        //    DataTable Newdt = new DataTable("Result");
        //    Newdt.Columns.Add("rowid", typeof(System.String));                 //行号
        //    Newdt.Columns.Add("locate", typeof(System.String));                 //位置
        //    Newdt.Columns.Add("reason", typeof(System.String));                 //原因
        //    int errorNum = 1;
        //    try
        //    {
        //        string moduleId = Request["moduleId"]; //表名
        //        StringBuilder sb_table = new StringBuilder();
        //        HttpFileCollectionBase files = Request.Files;
        //        HttpPostedFileBase file = files["filePath"];//获取上传的文件
        //        string fullname = file.FileName;
        //        string IsXls = System.IO.Path.GetExtension(fullname).ToString().ToLower();//System.IO.Path.GetExtension获得文件的扩展名
        //        if (!IsXls.EndsWith(".xls") && !IsXls.EndsWith(".xlsx"))
        //        {
        //            IsOk = 0;
        //        }
        //        else
        //        {

        //            string filename = Guid.NewGuid().ToString() + ".xls";
        //            if (fullname.EndsWith(".xlsx"))
        //            {
        //                filename = Guid.NewGuid().ToString() + ".xlsx";
        //            }
        //            if (file != null && file.FileName != "")
        //            {
        //                string msg = UploadHelper.FileUpload(file, Server.MapPath("~/Resource/UploadFile/ImportExcel/"), filename);
        //            }

                    //DataTable dt = ImportExcel.ExcelToDataTable(Server.MapPath("~/Resource/UploadFile/ImportExcel/") + filename);

                    //RemoveEmpty(dt);//清除空行
                    //dt.Columns.Add("rowid", typeof(System.String));//用来标识excell行ID
                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //    dt.Rows[i]["rowid"] = i + 1;
                    //}
                    //#region 设备信息导入
                    ////校验
                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //    IsCheck = 1;//重置标识
                    //    DataRow dr = Newdt.NewRow();
                    //    string Enabled = "";
                    //    string DvcCatg = "";
                    //    string DvcTyp = "";
                    //    switch (dt.Rows[i]["有效性"].ToString())
                    //    {
                    //        case "有效":
                    //            Enabled = "1"; break;
                    //        case "无效":
                    //            Enabled = "0"; break;
                    //        default:
                    //            dr = Newdt.NewRow();
                    //            dr[0] = errorNum;
                    //            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[是否启用]";
                    //            dr[2] = "数字格式不正确请重新输入";
                    //            Newdt.Rows.Add(dr);
                    //            errorNum++;
                    //            IsCheck = 0;
                    //            break;
                    //    }
                    //    switch (dt.Rows[i]["设备类别"].ToString())
                    //    {
                    //        case "工艺设备":
                    //            DvcCatg = "C001"; break;
                    //        case "控制设备":
                    //            DvcCatg = "C002"; break;
                    //        case "系统设备":
                    //            DvcCatg = "C003"; break;
                    //        default:
                    //            dr = Newdt.NewRow();
                    //            dr[0] = errorNum;
                    //            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[设备类别]";
                    //            dr[2] = "数字格式不正确请重新输入";
                    //            Newdt.Rows.Add(dr);
                    //            errorNum++;
                    //            IsCheck = 0;
                    //            break;
                    //    }
                    //    switch (dt.Rows[i]["设备类型"].ToString())
                    //    {
                    //        case "冲压机":
                    //            DvcTyp = "T001"; break;
                    //        case "焊接机":
                    //            DvcTyp = "T002"; break;
                    //        default:
                    //            dr = Newdt.NewRow();
                    //            dr[0] = errorNum;
                    //            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[设备类型]";
                    //            dr[2] = "数字格式不正确请重新输入";
                    //            Newdt.Rows.Add(dr);
                    //            errorNum++;
                    //            IsCheck = 0;
                    //            break;
                    //    }
                    //    if (dt.Rows[i]["设备编号"].ToString().Trim() != "")
                    //    {
                    //        int DeviceCount = MyBll.CheckCount("DvcCd", dt.Rows[i]["设备编号"].ToString());//是否有相同设备编码
                    //        if (DeviceCount > 0)
                    //        {
                    //            dr = Newdt.NewRow();
                    //            dr[0] = errorNum;
                    //            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[设备编号]";
                    //            dr[2] = "在系统中已存在不能重复插入";
                    //            Newdt.Rows.Add(dr);
                    //            errorNum++;
                    //            IsCheck = 0;
        //                        continue;
        //                    }
        //                    else
        //                    {
        //                        BBdbR_DvcBase Device = new BBdbR_DvcBase();
        //                        Device.DvcId = System.Guid.NewGuid().ToString();
        //                        Device.WorkshopId = dt.Rows[i]["车间主键"].ToString().Trim();
        //                        Device.DvcCd = dt.Rows[i]["设备编号"].ToString().Trim();
        //                        Device.DvcNm = dt.Rows[i]["设备名称"].ToString().Trim();
        //                        Device.DvcCatg = DvcCatg;
        //                        Device.DvcTyp = DvcTyp;
        //                        Device.IPAddr = dt.Rows[i]["IP地址"].ToString().Trim();
        //                        Device.Port = dt.Rows[i]["端口"].ToString().Trim();
        //                        Device.DvcMaker = dt.Rows[i]["设备型号"].ToString().Trim();
        //                        Device.DvcMdl = dt.Rows[i]["设备产商"].ToString().Trim();
        //                        Device.DvcLife = dt.Rows[i]["设备寿命"].ToString().Trim();
        //                        Device.DvcLocatn = dt.Rows[i]["设备位置"].ToString().Trim();
        //                        //Device.DvcMDt = DateTime.ParseExact(dt.Rows[i]["设备制造日期"].ToString().Trim(), "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                        Device.MaintCycle = int.Parse(dt.Rows[i]["维保周期(天)"].ToString().Trim());
        //                        Device.LeadTm = int.Parse(dt.Rows[i]["提前期（天）"].ToString().Trim());
        //                        Device.Dsc = dt.Rows[i]["设备描述"].ToString().Trim();
        //                        Device.Enabled = Enabled;
        //                        DeviceList.Add(Device);
        //                        int b = database.Insert(DeviceList);
        //                        if (b > 0)
        //                        {
        //                            IsOk = IsOk + b;
        //                            DeviceList.Clear();
        //                        }
        //                        else
        //                        {
        //                            dr = Newdt.NewRow();
        //                            dr[0] = errorNum;
        //                            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行";
        //                            dr[2] = "设备信息插入失败";
        //                            Newdt.Rows.Add(dr);
        //                            IsCheck = 0;
        //                            continue;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    dr = Newdt.NewRow();
        //                    dr[0] = errorNum;
        //                    dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行";
        //                    dr[2] = "设备编码不能为空";
        //                    Newdt.Rows.Add(dr);
        //                    errorNum++;
        //                    IsCheck = 0;
        //                    continue;
        //                }
        //            }
        //            if (IsCheck == 0)
        //            {
        //                IsOk = 0;
        //            }
        //            #endregion

        //        }
        //        Result = Newdt;
        //    }
        //    catch (Exception ex)
        //    {
        //        Base_SysLogBll.Instance.WriteLog("", OperationType.Add, "-1", "异常错误：" + ex.Message);
        //        IsOk = 0;
        //    }
        //    if (Result.Rows.Count > 0)
        //    {
        //        IsOk = 0;
        //    }
        //    var JsonData = new
        //    {
        //        Status = IsOk > 0 ? "true" : "false",
        //        ResultData = Result
        //    };
        //    return Content(JsonData.ToJson());
        //}
        //#endregion
    }
}