using System;
using HfutIE.Business;
using HfutIE.Entity;
using HfutIE.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HfutIE.WebApp.Areas.CommonModule.Controllers
{
    //PublicController<P_CarPastInfo_Pro>
    public class getCarInfoController : PublicController<P_CarPastInfo_Pro>
    {
        // GET: CommonModule/getCarInfo
        P_CarPastInfo_ProBll p_CarPastInfo_ = new P_CarPastInfo_ProBll();
        HfutIE.Business.P_CarPastInfo_ProBll P_CarBll = new HfutIE.Business.P_CarPastInfo_ProBll();

        public ActionResult getAllCarInfo()
        {
            DataTable ListData = P_CarBll.getAllCarInfo();
            var JsonData = new
            {
                rows = ListData,
            };
            return Content(JsonData.ToJson());
        }

        public ActionResult getAllCarInfo2()
        {
            DataTable ListData = P_CarBll.getAllCarInfo2();
            var JsonData = new
            {
                rows = ListData,
            };
            return Content(JsonData.ToJson());
        }

        public ActionResult getAlarmInfo()
        {
            DataTable ListData = P_CarBll.getAlarmInfo();
            var JsonData = new
            {
                rows = ListData,
            };
            return Content(JsonData.ToJson());
        }

        public ActionResult getAllCarQuene()
        {
            DataTable ListData = P_CarBll.getAllCarQuene();
            var JsonData = new
            {
                rows = ListData,
            };
            return Content(JsonData.ToJson());
        }

        public ActionResult getAllDataAcq()
        {
            DataTable ListData = P_CarBll.getAllDataAcq();
            var JsonData = new
            {
                rows = ListData,
            };
            return Content(JsonData.ToJson());
        }

        public ActionResult getAllOutPut()
        {
            DataTable ListData = P_CarBll.getAllOutPut();
            var JsonData = new
            {
                rows = ListData,
            };
            return Content(JsonData.ToJson());
        }

    }
}
