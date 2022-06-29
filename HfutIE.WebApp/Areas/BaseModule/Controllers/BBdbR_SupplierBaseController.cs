using HfutIE.Business;
using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HfutIE.WebApp.Areas.BaseModule.Controllers
{
    /// <summary>
    /// ��Ӧ�̻�����Ϣ�������
    /// </summary>
    public class BBdbR_SupplierBaseController : PublicController<BBdbR_SupplierBase>
    {
        #region �������ݿ������������
        //�������ݿ���ʶ������Է������в������ݿ�ķ���
        BBdbR_SupplierBaseBll MyBll = new BBdbR_SupplierBaseBll(); //===����ʱ��Ҫ�޸�===
        public readonly RepositoryFactory<BBdbR_SupplierBase> repository_avibase = new RepositoryFactory<BBdbR_SupplierBase>();
        #endregion

        #region ������   
        #region 1.չʾҳ����
        /// <summary>
        /// ������������е����ݣ�����json����ʽ����
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public ActionResult GridPage(JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<BBdbR_SupplierBase> ListData = MyBll.GetPageList(jqgridparam);    //===����ʱ��Ҫ�޸�===
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = ListData,
                };
                return Content(ListData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "�쳣����" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 2.�����༭����
        //entity��ʾҳ���д����ʵ�壬KeyValue��ʾ���������
        //�������������Ǳ༭��ҳ���ж��ᴫ��ʵ�壨entity��
        //����ʱʵ����һ��ȫ�µ�ʵ��
        //�༭ʱʵ�����޸ĺ��ʵ��
        //ֻ���ڱ༭ʱҳ���вŻᴫ������entity��KeyValue��������������Ҫ�༭�Ǹ�ʵ�������
        //
        //�༭�������ȸ���KeyValue�Ƿ���ֵ�ж�����Ҫ�༭������Ҫ����
        //����������ͽ���ʵ�����������ݿ���
        //����Ǳ༭�ͽ���ʵ����µ�������
        //
        //�������������Ǳ༭�����ж�ҳ������ı���Ƿ��Ѿ�����
        //����Ѿ����ھ�ֱ�ӷ��ء��ñ���Ѿ����ڣ�������Ϣ
        //�������ٽ�����һ��
        public override ActionResult SubmitForm(BBdbR_SupplierBase entity, string KeyValue)//===����ʱ��Ҫ�޸�===
        {
            try
            {
                //IDatabase database = DataFactory.Database();
                int IsOk = 0;//�����ж�
                string Name = "SupplierCd";        //ҳ���еı���ֶ������磺��˾���   //===����ʱ��Ҫ�޸�===
                string Value = entity.SupplierCd;  //ҳ���еı���ֶ�ֵ                 //===����ʱ��Ҫ�޸�===\
                string Message = "";
                if (KeyValue == "")
                {
                    Message = "�����ɹ���";
                }
                else
                {
                    Message = "�༭�ɹ���";
                }

                if (!string.IsNullOrEmpty(KeyValue))//�༭����
                {
                    //===����ʱ��Ҫ�޸�===
                    BBdbR_SupplierBase Oldentity = repositoryfactory.Repository().FindEntity(KeyValue);//��ȡû����֮ǰʵ�����

                    entity.Modify(KeyValue);
                    //IsOk = database.Update(entity);
                    IsOk = MyBll.Update(entity);//���޸ĺ��ʵ����µ����ݿ⣬����ɹ�����1��ʧ�ܷ���0��
                    this.WriteLog(IsOk, entity, Oldentity, KeyValue, Message);//��¼��־

                }
                else//��������
                {
                    IsOk = MyBll.CheckCount(Name, Value);//�ж�ҳ������д�����ݵı���ֶε�ֵ�Ƿ����
                    if (IsOk > 0)//����
                    {
                        Message = "�ñ���Ѿ����ڣ�";
                        return Content(new JsonMessage { Success = false, Code = IsOk.ToString(), Message = Message }.ToString());
                    }
                    entity.Create();
                    //IsOk = database.Insert(entity);
                    IsOk = MyBll.Insert(entity);//��ʵ��������ݿ⣬����ɹ�����1��ʧ�ܷ���0��
                    this.WriteLog(IsOk, entity, null, KeyValue, Message);//��¼��־

                }
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                this.WriteLog(-1, entity, null, KeyValue, "����ʧ�ܣ�" + ex.Message);//��¼��־
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 3.ɾ������
        /// <summary>
        /// �����ж���Ҫɾ������Ϣ�Ƿ����������Ϣ
        ///     �磺ɾ��һ����˾��Ϣ��Ҫ�жϸ�����˾��Ϣ�����Ƿ���˹�����Ϣ
        ///         ���������Ϣ������ʾ����ǰ��ѡ���ӽڵ����ݣ�����ɾ������������
        ///  ��ȷ��û�����ݵ�����£�ɾ��������
        ///     ɾ����ʾ�������ݵ�IsAvailable������Ϊfalse���������ɾ���ü�¼
        /// </summary>
        /// <param name="KeyValue">ҳ�����ṩ����Ҫɾ�������ݵ�����,�����Ƕ������ݵ����������������</param>
        /// <param name="ParentId">����Ҫ</param>
        /// <param name="DeleteMark">����Ҫ</param>
        /// <returns></returns>
        public override ActionResult Delete(string KeyValue, string ParentId, string DeleteMark)
        {
            //�����Ƕ���������ǵ�����������������ֳ���������������
            string[] array = KeyValue.Split(',');
            try
            {
                var Message = "ɾ��ʧ�ܡ�";//���巵����Ϣ������Ϣ�����ص������ϣ����û��ۿ�
                int IsOk = 0;//�ж�ɾ�������Ƿ�ɣ�0��ʾ���ɹ�������0��ʾ�ɹ�   
                IsOk = MyBll.Delete(array);//ִ��ɾ������
                if (IsOk > 0)
                {
                    Message = "ɾ���ɹ�";
                }
                WriteLog(IsOk, array, Message);
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
                //return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, array, "����ʧ�ܣ�" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 6.��ѯ����
        //��ѯ������������Ϊ��������ѯ��������һ���������в�ѯ
        //��ѯ����ΪCondition��Ҳ�����ݿ��_CompanyBaseInformation�е�һ���ֶ���
        //��ѯֵΪkeywords��Ҳ�����ݿ��_CompanyBaseInformation�е��ֶ������ֶ�ֵ
        //����ѯ���ý��Ʋ�ѯ��like��

        public ActionResult GridPageByCondition(string keywords, string Condition, JqGridParam jqgridparam)
        {
            try
            {
                string keyword = keywords.Trim();
                Stopwatch watch = CommonHelper.TimerStart();
                List<BBdbR_SupplierBase> ListData = MyBll.GetPageListByCondition(keyword, Condition, jqgridparam);//===����ʱ��Ҫ�޸�===
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = ListData,
                };
                return Content(ListData.ToJson());
            }
            catch (Exception ex)
            {
                //CCSLog.CCSLogHelper.WriteExLog(ex, CCSLog.LogType.WebSite);
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "�쳣����" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 7.����ֶ��Ƿ�Ψһ
        /// <summary>
        /// ���ݴ�����ֶ������ֶ�ֵ�жϸ��ֶ��Ƿ����
        /// </summary>
        /// <param name="KeyName">�ֶ���</param>
        /// <param name="KeyValue">�ֶ�ֵ</param>
        /// <returns>���ظ��жϽ��</returns>
        public ActionResult ChectOnlyOne(string KeyName, string KeyValue)
        {
            try
            {
                int IsOk = 0;
                string Message = "���ֶ��Ѿ����ڣ�";

                if (!string.IsNullOrEmpty(KeyValue))
                {
                    IsOk = MyBll.CheckCount(KeyName, KeyValue);
                }
                if (IsOk > 0)
                {
                    return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message }.ToString());
            }
        }

        #endregion

        #region 10.����
        /// <summary>
        /// ����Excel������ҳ��
        /// </summary>
        /// <returns></returns>
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult ExcelImportDialog()
        {
            string moduleId = DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId"));
            //ģ������
            Base_ExcelImport base_excellimport = DataFactory.Database().FindEntity<Base_ExcelImport>("ModuleId", moduleId);
            if (base_excellimport.ModuleId != null)
            {
                ViewBag.ModuleId = moduleId;
                ViewBag.ImportFileName = base_excellimport.ImportFileName;
                ViewBag.ImportName = base_excellimport.ImportName;
                ViewBag.ImportId = base_excellimport.ImportId;
            }
            else
            {
                ViewBag.ModuleId = "0";
            }
            //ViewBag.ID = Request.Params["ID"];
            //ID1 = ViewBag.ID;
            //ViewBag.OrderID = Request.Params["OrderID"];
            //OrderID1 = ViewBag.OrderID;
            return View();
        }
        #region ����ģ��
        /// <summary>
        /// ����Excellģ��
        /// </summary>
        /// <returns></returns>
        public ActionResult GetExcellTemperature(string ImportId)
        {
            if (!string.IsNullOrEmpty(ImportId))
            {
                DataSet ds = new DataSet();
                DataTable data = new DataTable(); string DataColumn = ""; string fileName;
                MyBll.GetExcellTemperature(ImportId, out data, out DataColumn, out fileName);
                ds.Tables.Add(data);
                MemoryStream ms = DeriveExcel.ExportToExcel(ds, "xls", DataColumn.Split('|'));
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }
                return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
            }
            else
            {
                return null;
            }
        }
        #endregion
        /// <summary>
        /// ���Datatable����
        /// </summary>
        /// <param name="dt"></param>
        public void RemoveEmpty(DataTable dt)
        {
            List<DataRow> removelist = new List<DataRow>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                bool rowdataisnull = true;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()))
                    {

                        rowdataisnull = false;
                    }
                }
                if (rowdataisnull)
                {
                    removelist.Add(dt.Rows[i]);
                }

            }
            for (int i = 0; i < removelist.Count; i++)
            {
                dt.Rows.Remove(removelist[i]);
            }
        }

        /// <summary>
        /// ����Excell����
        /// </summary>
        /// <returns></returns>
        public ActionResult ImportExel()
        {
            int IsOk = 0;//����״̬
            int IsCheck = 1;//���������ظ���ַ�ı�ʶ
            DataTable Result = new DataTable();//��������¼��
            IDatabase database = DataFactory.Database();
            List<BBdbR_SupplierBase> EntityList = new List<BBdbR_SupplierBase>();

            //���쵼�뷵�ؽ����
            DataTable Newdt = new DataTable("Result");
            Newdt.Columns.Add("rowid", typeof(System.String));                 //�к�
            Newdt.Columns.Add("locate", typeof(System.String));                 //λ��
            Newdt.Columns.Add("reason", typeof(System.String));                 //ԭ��
            int errorNum = 1;
            try
            {
                string moduleId = Request["moduleId"]; //����
                //StringBuilder sb_table = new StringBuilder();
                HttpFileCollectionBase files = Request.Files;
                HttpPostedFileBase file = files["filePath"];//��ȡ�ϴ����ļ�
                string fullname = file.FileName;
                string IsXls = System.IO.Path.GetExtension(fullname).ToString().ToLower();//System.IO.Path.GetExtension����ļ�����չ��
                if (!IsXls.EndsWith(".xls") && !IsXls.EndsWith(".xlsx"))
                {
                    IsOk = 0;
                }
                else
                {

                    string filename = Guid.NewGuid().ToString() + ".xls";
                    if (fullname.EndsWith(".xlsx"))
                    {
                        filename = Guid.NewGuid().ToString() + ".xlsx";
                    }
                    if (file != null && file.FileName != "")
                    {
                        string msg = UploadHelper.FileUpload(file, Server.MapPath("~/Resource/UploadFile/ImportExcel/"), filename);
                    }

                    DataTable dt = ImportExcel.ExcelToDataTable(Server.MapPath("~/Resource/UploadFile/ImportExcel/") + filename);

                    RemoveEmpty(dt);//�������
                    dt.Columns.Add("rowid", typeof(System.String));//������ʶexcell��ID
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["rowid"] = i + 1;
                    }
                    #region �����Ŀ��Ϣ����
                    //У��
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        IsCheck = 1;//���ñ�ʶ
                        DataRow dr = Newdt.NewRow();

                        //��Ӧ������
                        Base_DataDictionary SupplyType = database.FindEntity<Base_DataDictionary>("Code", "SupplierCatg");
                        List<Base_DataDictionaryDetail> SupplyType1 = database.FindList<Base_DataDictionaryDetail>("DataDictionaryId", SupplyType.DataDictionaryId);
                        foreach (var item in SupplyType1)
                        {
                            
                            IsCheck = 0;
                            if (item.FullName == dt.Rows[i]["��Ӧ������"].ToString().Trim())
                            {
                                IsCheck = 1;
                                break;
                            }
                        }
                        if (IsCheck == 0)
                        {
                            dr = Newdt.NewRow();
                            dr[0] = errorNum;
                            dr[1] = "��[" + dt.Rows[i]["rowid"].ToString() + "]��[��Ӧ������]";
                            dr[2] = "��ϵͳ�в����ڴ˹�Ӧ�����ͻ���������";
                            Newdt.Rows.Add(dr);
                            errorNum++;
                            IsCheck = 0;
                        }
                        //��Ӧ�̵ȼ�
                        Base_DataDictionary SupplyGrade = database.FindEntity<Base_DataDictionary>("Code", "SupplierGrade");
                        List<Base_DataDictionaryDetail> SupplyGrade1 = database.FindList<Base_DataDictionaryDetail>("DataDictionaryId", SupplyGrade.DataDictionaryId);
                        foreach (var item in SupplyGrade1)
                        {
                            IsCheck = 0;
                            if (item.FullName == dt.Rows[i]["��Ӧ�̵ȼ�"].ToString().Trim())
                            {
                                IsCheck = 1;
                                break;
                            }
                        }
                        if (IsCheck == 0)
                        {
                            dr = Newdt.NewRow();
                            dr[0] = errorNum;
                            dr[1] = "��[" + dt.Rows[i]["rowid"].ToString() + "]��[��Ӧ�̵ȼ�]";
                            dr[2] = "��ϵͳ�в����ڴ˹�Ӧ�̵ȼ�����������";
                            Newdt.Rows.Add(dr);
                            errorNum++;
                            IsCheck = 0;
                        }


                        if (dt.Rows[i]["��Ӧ�̴���"].ToString().Trim() != "")
                        {
                            int Count = MyBll.CheckCount("SupplierCd", dt.Rows[i]["��Ӧ�̴���"].ToString().Trim());//�Ƿ�����ͬ���
                            if (Count > 0)
                            {
                                dr = Newdt.NewRow();
                                dr[0] = errorNum;
                                dr[1] = "��[" + dt.Rows[i]["rowid"].ToString() + "]��[��Ӧ�̴���]";
                                dr[2] = "��ϵͳ���Ѵ��ڲ����ظ�����";
                                Newdt.Rows.Add(dr);
                                errorNum++;
                                IsCheck = 0;
                                continue;
                            }
                            else
                            {
                                if (IsCheck == 0)//�������������ʽ����
                                {
                                    continue;
                                }
                                BBdbR_SupplierBase entity = new BBdbR_SupplierBase();
                                entity.SupplierCd = dt.Rows[i]["��Ӧ�̴���"].ToString().Trim();
                                entity.SupplierNm = dt.Rows[i]["��Ӧ������"].ToString().Trim();
                                entity.SupplierCatg = dt.Rows[i]["��Ӧ������"].ToString().Trim();
                                entity.SupplierGrade = dt.Rows[i]["��Ӧ�̵ȼ�"].ToString().Trim();
                                entity.SupplierTeleph = dt.Rows[i]["��Ӧ����ϵ�绰"].ToString().Trim();
                                entity.Mgr = dt.Rows[i]["������"].ToString().Trim();
                                entity.SupplierEmail = dt.Rows[i]["��Ӧ������"].ToString().Trim();
                                entity.SupplierAddress = dt.Rows[i]["��Ӧ�̵�ַ"].ToString().Trim();
                                entity.SupplierWebsite = dt.Rows[i]["��Ӧ����ַ"].ToString().Trim();

                                entity.Description = dt.Rows[i]["��Ӧ������"].ToString().Trim();
                                entity.Remark = dt.Rows[i]["��ע"].ToString().Trim();

                                entity.Create();
                                
                                EntityList.Add(entity);
                                int b = database.Insert(EntityList);
                                if (b > 0)
                                {
                                    IsOk = IsOk + b;
                                    EntityList.Clear();
                                }
                                else
                                {
                                    dr = Newdt.NewRow();
                                    dr[0] = errorNum;
                                    dr[1] = "��[" + dt.Rows[i]["rowid"].ToString() + "]��";
                                    dr[2] = "��Ӧ����Ϣ����ʧ��";
                                    Newdt.Rows.Add(dr);
                                    IsCheck = 0;
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            dr = Newdt.NewRow();
                            dr[0] = errorNum;
                            dr[1] = "��[" + dt.Rows[i]["rowid"].ToString() + "]��";
                            dr[2] = "��Ӧ�̴��벻��Ϊ��";
                            Newdt.Rows.Add(dr);
                            errorNum++;
                            IsCheck = 0;
                            continue;
                        }
                    }
                    if (IsCheck == 0)
                    {
                        IsOk = 0;
                    }
                    #endregion

                }
                Result = Newdt;
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Add, "-1", "�쳣����" + ex.Message);
                IsOk = 0;
            }
            if (Result.Rows.Count > 0)
            {
                IsOk = 0;
            }
            var JsonData = new
            {
                Status = IsOk > 0 ? "true" : "false",
                ResultData = Result
            };
            return Content(JsonData.ToJson());
        }
        #endregion

        #endregion

    }
}