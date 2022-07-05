using HfutIE.Business;
using HfutIE.Entity;
using HfutIE.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HfutIE.WebApp.Areas.BaseModule.Controllers
{
    public class BPIE_SLDocController : PublicController<BPIE_SLDoc>
    {
        // GET: BaseModule/BPIE_SLDoc
        #region  创建数据库操作对象区域
        BPIE_SLDocBll MyBll = new BPIE_SLDocBll();
        #endregion

        #region 2.高级检索
        public ActionResult GridListJson2(string keyvalue, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            DateTime? starttime = Convert.ToDateTime(StartTime);
            DateTime? endtime = Convert.ToDateTime(EndTime).AddDays(1);
            List<ConditionAndKey> list = new List<ConditionAndKey>();
            if (!string.IsNullOrEmpty(keyvalue))//存在输入的检索条件
            {
                JObject tempobj = JObject.Parse(keyvalue);
                foreach (var item in tempobj)
                {
                    ConditionAndKey cond_key = new ConditionAndKey();
                    cond_key.Condition = tempobj[item.Key]["Condition"].ToString();
                    cond_key.Keywords = tempobj[item.Key]["Keywords"].ToString();
                    cond_key.AndOr = tempobj[item.Key]["AndOr"].ToString();
                    cond_key.ExpressExtension = tempobj[item.Key]["ExpressExtension"].ToString();
                    list.Add(cond_key);
                }
            }
            Stopwatch watch = CommonHelper.TimerStart();
            DataTable ListData = MyBll.GridPageStockTryList(list, starttime, endtime, ref jqgridparam);
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
        #endregion

        #region 3.获取下拉框内容
        /// <summary>
        /// 获取设备名称
        /// </summary>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        //public ActionResult GetDeviceNm(JqGridParam jqgridparam)
        //{
        //    try
        //    {
        //        Stopwatch watch = CommonHelper.TimerStart();
        //        BBdbR_DvcBaseBll DvcBll = new BBdbR_DvcBaseBll();
        //        DataTable Dvcdt = DvcBll.GrtDvcNm();
        //        var JsonData = new
        //        {
        //            total = jqgridparam.total,
        //            page = jqgridparam.page,
        //            records = jqgridparam.records,
        //            costtime = CommonHelper.TimerEnd(watch),
        //            rows = Dvcdt,
        //        };
        //        return Content(Dvcdt.ToJson());
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
        #endregion
    }
}