using HfutIE.Business;
using HfutIE.Entity;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HfutIE.WebApp.Areas.CommonModule.Controllers
{
    /// <summary>
    /// Base_Factory¿ØÖÆÆ÷
    /// </summary>
    //public class FactoryController : PublicController<Base_Factory>
    //{
    //    Base_FactoryBll base_companybll = new Base_FactoryBll();
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <returns></returns>
    //    public ActionResult TreeJson()
    //    {
    //        List<Base_Factory> list = base_companybll.GetList();
    //        List<TreeJsonEntity> TreeList = new List<TreeJsonEntity>();
    //        foreach (Base_Factory item in list)
    //        {
    //            bool hasChildren = false;
    //            IList childnode = list.FindAll(t => t.ParentId == item.FactoryId);
    //            if (childnode.Count > 0)
    //            {
    //                hasChildren = true;
    //            }
    //            TreeJsonEntity tree = new TreeJsonEntity();
    //            tree.id = item.FactoryId;
    //            tree.text = item.FactoryName;
    //            tree.value = item.FactoryId;
              
    //            tree.isexpand = true;
    //            tree.complete = true;
    //            tree.hasChildren = hasChildren;
    //            tree.parentId = item.ParentId;
    //            if (item.ParentId == "0")
    //            {
    //                tree.img = "/Content/Images/Icon16/molecule.png";
    //            }
    //            else
    //            {
    //                tree.img = "/Content/Images/Icon16/hostname.png";
    //            }
    //            TreeList.Add(tree);
    //        }
    //        return Content(TreeList.TreeToJson());
    //    }
    //}
}