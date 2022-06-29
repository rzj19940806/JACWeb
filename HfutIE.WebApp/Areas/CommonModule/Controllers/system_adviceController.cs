using HfutIE.Business;
using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HfutIE.WebApp.Areas.CommonModule.Controllers
{
    /// <summary>
    /// system_advice������
    /// </summary>
    public class system_adviceController : PublicController<system_advice>
    {
        public virtual ActionResult Form1()
        {
            return View();
        }
        string UserId = ManageProvider.Provider.Current().UserId;
        system_adviceBll system_advicebll = new system_adviceBll();
        string moudulebuttunid = "15dafe79-2334-4269-9a40-fb2102221629";
        public ActionResult ListJson(string type, string keywords, string ParameterJson, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                int count = system_advicebll.GetCount(UserId, moudulebuttunid);//�жϸ��û��Ƿ��д��������Ȩ�ޣ�
                DataTable ListData;
                if (count == 0) {
                     ListData = system_advicebll.GetPageList(UserId, type, keywords, ParameterJson, ref jqgridparam);
                }
                else {
                    ListData = system_advicebll.GetPageList1(type, keywords, ParameterJson, ref jqgridparam);
                }              
                //DataTable ListData = system_advicebll.GetPageList(type, keywords, ParameterJson, ref jqgridparam);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = ListData,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "�쳣����" + ex.Message);
                return null;
            }
            //List<FM_Bloc> list = fm_blocbll.GetList();
            //return Content(list.ToJson());
        }
        public virtual ActionResult AddareaForm()
        {
            system_advice entity = new Entity.system_advice();//�༭ʱ�鵽�����ݣ���entity����ʽ����
            CommandType cmdText = new CommandType();
            string sql1 = "select advice_code from system_advice where create_time in (select max(create_time) from system_advice)";
            DataTable dt = DbHelper.GetDataSet(cmdText, sql1).Tables[0];
            string advice_code;
            if (dt.Rows.Count > 0)
            {
                advice_code = dt.Rows[0][0].ToString();
                string code = advice_code.Substring(8, 4);
                advice_code = "PSITMAMS" + (Convert.ToInt32(code) + 1).ToString("0000");
            }
            else {
                advice_code = "PSITMAMS0000";
            }
            entity.advice_code = advice_code;
            string strJson = entity.ToJson();     //��entity�е�����ת��Ϊ��ֵ�Ե�Json����
            return Content(strJson);
        }
    }
}