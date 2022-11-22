using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HfutIE.WebApp.Areas.TightModule.Controllers
{
    public class WcControlController : Controller
    {
        // GET: TightModule/WcControl
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TreeJson()
        {
            try
            {
                //同步数据
                string str1 = @"SELECT DISTINCT A.WcId,A.WcCd,A.WcNm,D.PlineNm FROM dbo.Tg_WcJobConfig A LEFT JOIN Tg_WcControl B 
                                ON B.WcId = A.WcId JOIN dbo.BBdbR_WcBase C ON C.WcId = A.WcId 
                                JOIN dbo.BBdbR_PlineBase D ON C.PlineId=D.PlineId WHERE B.ID IS NULL";
                DataTable dt1 = DbHelperSQL.OpenTable(str1);
                if(dt1.Rows.Count > 0 )
                {
                    ArrayList list= new ArrayList();
                    for(int i=0;i<dt1.Rows.Count;i++)
                    {
                        string str2 = @"INSERT INTO dbo.Tg_WcControl
                                        (   PlineNm,
                                            WcId,
                                            WcCd,
                                            WcNm,
                                            Status,
                                            EditTime,
                                            EditUserCd,
                                            EditUserNm
                                        )
                                        VALUES
                                    ('" + dt1.Rows[i][3] +"','"+ dt1.Rows[i][0] + "','"+ dt1.Rows[i][1] + "','"+ dt1.Rows[i][2] + "',1,NULL,NULL,NULL)";
                        list.Add(str2);
                    }
                    DbHelperSQL.ExecuteSqlTran(list);
                }
                List<TreeJsonEntity> TreeList = new List<TreeJsonEntity>();
                string str3 = "SELECT DISTINCT PlineNm FROM dbo.Tg_WcControl";
                DataTable dt3 = DbHelperSQL.OpenTable(str3);
                for(int i=0;i< dt3.Rows.Count;i++) 
                {
                    TreeJsonEntity treeson = new TreeJsonEntity();
                    treeson.id = (i+1).ToString();
                    treeson.text = dt3.Rows[i][0].ToString();
                    treeson.value = dt3.Rows[i][0].ToString();
                    treeson.parentId = "999";
                    treeson.isexpand = true;
                    treeson.complete = true;
                    treeson.hasChildren = false;
                    treeson.img = "/Content/Images/Icon16/accordion.png";
                    TreeList.Add(treeson);
                }
                TreeJsonEntity tree = new TreeJsonEntity();
                tree.id = "999";
                tree.text = "装配车间";
                tree.value = "装配车间";
                tree.parentId = "0";
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = true;
                tree.img = "/Content/Images/Icon16/house.png";
                TreeList.Add(tree);
                return Content(TreeList.TreeToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        public ActionResult GridListJson(string WcCd,string Cd,string Nm, JqGridParam jqgridparam)
        {
            try
            {
                #region 查询修改-考虑搜索条件
                Stopwatch watch = CommonHelper.TimerStart();
                StringBuilder strSql = new StringBuilder();
                strSql.Append("SELECT * FROM dbo.Tg_WcControl ");
                if (!string.IsNullOrEmpty(WcCd))
                {
                    if (WcCd != "装配车间")
                    {
                        strSql.Append(" WHERE PlineNm='"+WcCd+"'");
                    }
                }
                //工位编号模糊搜索
                if (!string.IsNullOrEmpty(Cd))
                {
                    strSql.Append(" WHERE WcCd LIKE '%"+Cd+"%'");
                }
                //工位名称模糊搜索
                if (!string.IsNullOrEmpty(Nm))
                {
                    if (!string.IsNullOrEmpty(Cd))
                    {
                        strSql.Append(" AND WcNm LIKE '%" + Cd + "%'");
                    }
                    else
                    {
                        strSql.Append(" WHERE WcNm LIKE '%" + Nm + "%'");
                    }
                    
                }
                //排序
                strSql.Append(" ORDER BY WcId");
                DataTable dt = DbHelperSQL.OpenTable(strSql.ToString());
                var JsonData = new
                {
                    jqgridparam.total,
                    jqgridparam.page,
                    jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                return Content(JsonData.ToJson());
                #endregion
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        public ActionResult DoBusiness(string Type,string Wc)
        {
            //不管是多个主键还是单个主键，将主键拆分出来，放在数组中
            string[] array = Wc.Split(',');
            try
            {
                ArrayList list = new ArrayList();
                for (int i = 0; i < array.Length; i++)
                {
                    string str = "UPDATE Tg_WcControl SET Status='" + Type + "', EditTime=GETDATE(),EditUserCd='"+ ManageProvider.Provider.Current().UserId + "',EditUserNm='"+ ManageProvider.Provider.Current().UserName + "' WHERE WcId='" + array[i] + "'";
                    list.Add(str);
                }
                DbHelperSQL.ExecuteSqlTran(list);
                return Content(new JsonMessage { Success = true, Code = "1", Message = "操作成功" }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
    }
}