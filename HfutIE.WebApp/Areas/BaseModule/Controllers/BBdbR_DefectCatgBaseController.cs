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
    /// ȱ�����������
    /// </summary>
    public class BBdbR_DefectCatgBaseController : PublicController<BBdbR_DefectCatgBase>
    {
        //#region 10.����
        

        ///// <summary>
        ///// ���Datatable����
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
        ///// ����Excell����
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult ImportExel(string AlarmId)
        //{
        //    int IsOk = 0;//����״̬
        //    int IsCheck = 1;//���������ظ���ַ�ı�ʶ
        //    DataTable Result = new DataTable();//��������¼��
        //    IDatabase database = DataFactory.Database();
        //    List<BBdbR_DvcBase> DeviceList = new List<BBdbR_DvcBase>();

        //    //���쵼�뷵�ؽ����
        //    DataTable Newdt = new DataTable("Result");
        //    Newdt.Columns.Add("rowid", typeof(System.String));                 //�к�
        //    Newdt.Columns.Add("locate", typeof(System.String));                 //λ��
        //    Newdt.Columns.Add("reason", typeof(System.String));                 //ԭ��
        //    int errorNum = 1;
        //    try
        //    {
        //        string moduleId = Request["moduleId"]; //����
        //        StringBuilder sb_table = new StringBuilder();
        //        HttpFileCollectionBase files = Request.Files;
        //        HttpPostedFileBase file = files["filePath"];//��ȡ�ϴ����ļ�
        //        string fullname = file.FileName;
        //        string IsXls = System.IO.Path.GetExtension(fullname).ToString().ToLower();//System.IO.Path.GetExtension����ļ�����չ��
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

                    //RemoveEmpty(dt);//�������
                    //dt.Columns.Add("rowid", typeof(System.String));//������ʶexcell��ID
                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //    dt.Rows[i]["rowid"] = i + 1;
                    //}
                    //#region �豸��Ϣ����
                    ////У��
                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //    IsCheck = 1;//���ñ�ʶ
                    //    DataRow dr = Newdt.NewRow();
                    //    string Enabled = "";
                    //    string DvcCatg = "";
                    //    string DvcTyp = "";
                    //    switch (dt.Rows[i]["��Ч��"].ToString())
                    //    {
                    //        case "��Ч":
                    //            Enabled = "1"; break;
                    //        case "��Ч":
                    //            Enabled = "0"; break;
                    //        default:
                    //            dr = Newdt.NewRow();
                    //            dr[0] = errorNum;
                    //            dr[1] = "��[" + dt.Rows[i]["rowid"].ToString() + "]��[�Ƿ�����]";
                    //            dr[2] = "���ָ�ʽ����ȷ����������";
                    //            Newdt.Rows.Add(dr);
                    //            errorNum++;
                    //            IsCheck = 0;
                    //            break;
                    //    }
                    //    switch (dt.Rows[i]["�豸���"].ToString())
                    //    {
                    //        case "�����豸":
                    //            DvcCatg = "C001"; break;
                    //        case "�����豸":
                    //            DvcCatg = "C002"; break;
                    //        case "ϵͳ�豸":
                    //            DvcCatg = "C003"; break;
                    //        default:
                    //            dr = Newdt.NewRow();
                    //            dr[0] = errorNum;
                    //            dr[1] = "��[" + dt.Rows[i]["rowid"].ToString() + "]��[�豸���]";
                    //            dr[2] = "���ָ�ʽ����ȷ����������";
                    //            Newdt.Rows.Add(dr);
                    //            errorNum++;
                    //            IsCheck = 0;
                    //            break;
                    //    }
                    //    switch (dt.Rows[i]["�豸����"].ToString())
                    //    {
                    //        case "��ѹ��":
                    //            DvcTyp = "T001"; break;
                    //        case "���ӻ�":
                    //            DvcTyp = "T002"; break;
                    //        default:
                    //            dr = Newdt.NewRow();
                    //            dr[0] = errorNum;
                    //            dr[1] = "��[" + dt.Rows[i]["rowid"].ToString() + "]��[�豸����]";
                    //            dr[2] = "���ָ�ʽ����ȷ����������";
                    //            Newdt.Rows.Add(dr);
                    //            errorNum++;
                    //            IsCheck = 0;
                    //            break;
                    //    }
                    //    if (dt.Rows[i]["�豸���"].ToString().Trim() != "")
                    //    {
                    //        int DeviceCount = MyBll.CheckCount("DvcCd", dt.Rows[i]["�豸���"].ToString());//�Ƿ�����ͬ�豸����
                    //        if (DeviceCount > 0)
                    //        {
                    //            dr = Newdt.NewRow();
                    //            dr[0] = errorNum;
                    //            dr[1] = "��[" + dt.Rows[i]["rowid"].ToString() + "]��[�豸���]";
                    //            dr[2] = "��ϵͳ���Ѵ��ڲ����ظ�����";
                    //            Newdt.Rows.Add(dr);
                    //            errorNum++;
                    //            IsCheck = 0;
        //                        continue;
        //                    }
        //                    else
        //                    {
        //                        BBdbR_DvcBase Device = new BBdbR_DvcBase();
        //                        Device.DvcId = System.Guid.NewGuid().ToString();
        //                        Device.WorkshopId = dt.Rows[i]["��������"].ToString().Trim();
        //                        Device.DvcCd = dt.Rows[i]["�豸���"].ToString().Trim();
        //                        Device.DvcNm = dt.Rows[i]["�豸����"].ToString().Trim();
        //                        Device.DvcCatg = DvcCatg;
        //                        Device.DvcTyp = DvcTyp;
        //                        Device.IPAddr = dt.Rows[i]["IP��ַ"].ToString().Trim();
        //                        Device.Port = dt.Rows[i]["�˿�"].ToString().Trim();
        //                        Device.DvcMaker = dt.Rows[i]["�豸�ͺ�"].ToString().Trim();
        //                        Device.DvcMdl = dt.Rows[i]["�豸����"].ToString().Trim();
        //                        Device.DvcLife = dt.Rows[i]["�豸����"].ToString().Trim();
        //                        Device.DvcLocatn = dt.Rows[i]["�豸λ��"].ToString().Trim();
        //                        //Device.DvcMDt = DateTime.ParseExact(dt.Rows[i]["�豸��������"].ToString().Trim(), "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
        //                        Device.MaintCycle = int.Parse(dt.Rows[i]["ά������(��)"].ToString().Trim());
        //                        Device.LeadTm = int.Parse(dt.Rows[i]["��ǰ�ڣ��죩"].ToString().Trim());
        //                        Device.Dsc = dt.Rows[i]["�豸����"].ToString().Trim();
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
        //                            dr[1] = "��[" + dt.Rows[i]["rowid"].ToString() + "]��";
        //                            dr[2] = "�豸��Ϣ����ʧ��";
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
        //                    dr[1] = "��[" + dt.Rows[i]["rowid"].ToString() + "]��";
        //                    dr[2] = "�豸���벻��Ϊ��";
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
        //        Base_SysLogBll.Instance.WriteLog("", OperationType.Add, "-1", "�쳣����" + ex.Message);
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