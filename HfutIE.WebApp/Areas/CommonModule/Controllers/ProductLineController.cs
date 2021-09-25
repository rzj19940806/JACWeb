using HfutIE.Business;
using HfutIE.Entity;
using HfutIE.Utilities;
using HfutIE.DataAccess;
using HfutIE.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Data.Common;

namespace HfutIE.WebApp.Areas.CommonModule.Controllers
{
    /// <summary>
    /// Base_ProductLine
    /// </summary>
    public class ProductLineController : PublicController<Base_ProductLine>
    {
        Base_ProductLineBll base_productlinebll = new Base_ProductLineBll();
     
      
        public ActionResult GridListJson(string FactoryId)
        {
            DataTable ListData = base_productlinebll.GetList(FactoryId);
            var JsonData = new
            {
                rows = ListData,
            };
            return Content(JsonData.ToJson());
        }
       
        /// <summary>
        /// 【部门管理】返回列表JONS
        /// </summary>
        /// <param name="CompanyId">公司ID</param>
        /// <returns></returns>
        public ActionResult ListJson(string FactoryId)
        {
            DataTable ListData = base_productlinebll.GetList(FactoryId);
            return Content(ListData.ToJson());
        }       
    }
}