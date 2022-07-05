using HfutIE.Business;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
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
    /// <summary>
    /// 工位物料配置控制器
    /// </summary>
    public class BBdbR_ProductWcConfigController : PublicController<BBdbR_ProductBase>
    {
        #region 创建数据库操作对象区域
        BBdbR_ProductWcConfigBll MyBll = new BBdbR_ProductWcConfigBll(); //===复制时需要修改===
        BBdbR_ProductWcMatConfigBll MyBll2 = new BBdbR_ProductWcMatConfigBll();
        #endregion

        #region 打开视图区域
        public ActionResult PictureEdit()//打开图片编辑页面
        {
            return View();
        }
        public ActionResult ConfigMat()//打开物料配置界面
        {
            return View();
        }
        public ActionResult PrintPage(string date1,string date2)//打开打印界面
        {
            return View();
        }
        #endregion

        #region 方法区

        #region 1.获取树
        public ActionResult TreeJson(string productMatId)
        {
            try
            {
                DataTable dt = MyBll.GetTree(productMatId);//获取树所需数据
                List<TreeJsonEntity> TreeList = new List<TreeJsonEntity>();
                if (DataHelper.IsExistRows(dt))
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string area_key = row["keys"].ToString();
                        bool hasChildren = false;
                        DataTable childnode = DataHelper.GetNewDataTable(dt, "parentid='" + area_key + "'");
                        if (childnode.Rows.Count > 0)
                        {
                            hasChildren = true;
                        }
                        TreeJsonEntity tree = new TreeJsonEntity();
                        tree.id = area_key;
                        tree.text = row["name"].ToString();
                        tree.value = row["code"].ToString();
                        tree.parentId = row["parentId"].ToString();
                        tree.Attribute = "Type";
                        tree.AttributeValue = row["sort"].ToString();
                        tree.isexpand = true;
                        tree.complete = true;
                        tree.hasChildren = hasChildren;
                        if (row["sort"].ToString() == "0")
                        {
                            tree.img = "/Content/Images/Icon16/house.png";
                        }
                        else if (row["sort"].ToString() == "1")
                        {
                            tree.img = "/Content/Images/Icon16/outlook_new_meeting.png";
                        }
                        else if (row["sort"].ToString() == "2")
                        {
                            tree.img = "/Content/Images/Icon16/role.png";
                        }
                        //tree.img = Business.FactoryModuleHelper<WORKSHOP>.GetAreaImg(row["table_name"].ToString());
                        TreeList.Add(tree);
                    }
                }
                return Content(TreeList.TreeToJson());
            }
            catch (Exception ex)
            {
                this.WriteLog(-1, null, null, null, "操作失败" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 2.点击树展示表格
        /// <summary>
        /// 根据点击树的节点在数据库中查询相应的信息
        /// </summary>
        /// <param name="areaId">点击的节点的主键</param>
        /// <param name="parentId">节点的父级主键</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public ActionResult TttGridListJson(string sort,string areaId,string text,string value, JqGridParam jqgridparam )
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                //===复制时需要修改===
                DataTable ListData = MyBll.GetList( areaId, text, value,sort, ref jqgridparam);
                for (int i=0;i<ListData.Rows.Count ;i++)//遍历，如果图片名称位空则显示“默认图片”
                {
                    var filenm = ListData.Rows[i]["filenm"].ToString();
                    if ( filenm == "" || filenm == null || filenm =="null")
                    {
                        ListData.Rows[i].BeginEdit();
                        ListData.Rows[i]["filenm"] = "0";//如果图片名称为空则返回前端页面“0”
                        ListData.Rows[i].EndEdit();
                        ListData.AcceptChanges();
                    }
                }
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
            catch(Exception ex)
            {
                this.WriteLog(-1, null, null, null, "操作失败" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 3.加载表格（没有datatable返回）
        public ActionResult GridPageJsonProduct(JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = MyBll.GridPageJson(jqgridparam);
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
            catch(Exception ex)
            {
                this.WriteLog(-1, null, null, null, "操作失败" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        public ActionResult GridPageJsonProduct1(string VIN,string ProductCd,string ProductNm, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = MyBll.GridPageJson1(VIN, ProductCd, ProductNm,jqgridparam);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = ListData,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "制造BOM信息查询成功");
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "制造BOM信息查询发生异常错误：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 4.加载未配置产品
        public ActionResult GridNotConfigPageJson(JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable dt = MyBll.GetNotConfigProduct(ref jqgridparam);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                return Content(JsonData.ToJson());
            }
            catch(Exception ex)
            {
                this.WriteLog(-1, null, null, null, "操作失败" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 5.搜索展示未配置产品
        public ActionResult GridNotConfigPageByCondition(string keywords, string Condition,JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable dt = MyBll.GetNotConfigProductbycondition(keywords, Condition,ref jqgridparam);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                this.WriteLog(-1, null, null, null, "操作失败" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 6.加载已配置产品的表(没用到)
        public ActionResult AddProductGrid(string array,JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData;
                if (array == "")
                {
                    ListData = null;
                }
                else
                {
                    ListData = MyBll.GetProductList(array);
                }
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
            catch(Exception ex)
            {
                this.WriteLog(-1, null, null, null, "操作失败" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 7.提交新增产品表单
        public  ActionResult Submit(List<BBdbR_ProductWcConfig> tabledata, string PlineNm,string PlineId)
        {
            try
            {
                int IsOk = -1;//用于判断,0表示没选中，1表示成功，-1表示失败
                if (tabledata==null)
                {
                    IsOk = 0;
                }
                else
                {
                    DataTable Wcdt = MyBll.GetWcdt(PlineId);//查询该产线下所有的工位
                    for(int i = 0; i < Wcdt.Rows.Count; i++)
                    {
                        var WcId = Wcdt.Rows[i]["WcId"];
                        var WcCd = Wcdt.Rows[i]["WcCd"];
                        var WcNm = Wcdt.Rows[i]["WcNm"];
                        for (int r=0;r<tabledata.Count;r++)//查询每一条产品是否在物料文档配置表中有图片信息
                        {
                            var MatId = tabledata[r].MatId;
                            var FileNm = "默认图片";
                            var FileCd = "";
                            var ConfigId = "";
                            tabledata[r].ProductClassId = System.Guid.NewGuid().ToString();
                            tabledata[r].PlineId = PlineId;
                            tabledata[r].ConfigId= ConfigId;
                            tabledata[r].FileNm = FileNm;
                            tabledata[r].FileCd = FileCd;
                            tabledata[r].WcId = WcId.ToString();
                            tabledata[r].WcCd = WcCd.ToString();
                            tabledata[r].WcNm = WcNm.ToString();
                            tabledata[r].LockStationNum = 1;
                            tabledata[r].VersionNumber = "V1.0";
                            tabledata[r].Enabled = "1";
                            tabledata[r].CreTm = DateTime.Now;
                            tabledata[r].CreCd = ManageProvider.Provider.Current().UserId;
                            tabledata[r].CreNm = ManageProvider.Provider.Current().UserName;
                            IsOk = MyBll.Insert(tabledata[r]);
                        }
                    }
                }
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString()}.ToString());
            }
            catch (Exception ex)
            {
                this.WriteLog(-1, null, null, null, "操作失败" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 8.编辑图片表单
        #region 8.1图片编号下拉框
        public ActionResult get_filecd(string matid)
        {
            try
            {
                DataTable dt = MyBll.GetFaileCd(matid);
                var JsonData = new
                {
                    rows = dt,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                this.WriteLog(-1, null, null, null, "操作失败" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion
        #region 8.2根据编号加载
        public ActionResult get_filedata(string filecd,string matid)
        {
            try
            {
                DataTable dt = MyBll.GetFaileData(filecd,matid);
                var JsonData = new
                {
                    rows = dt,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                this.WriteLog(-1, null, null, null, "操作失败" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion
        #region 8.3将修改的图片信息保存到数据库
        public ActionResult SubmitPicture(string newfilecd,string newfilenm,string newconfigid,string productclassid)
        {
            try
            {
                int isok = MyBll.SubmitPicture(newfilecd ,newfilenm,newconfigid, productclassid);
                var JsonData = new
                {
                    rows = isok ,
                };
                return Content(JsonData.ToJson());
            } 
            catch (Exception ex)
            {
                this.WriteLog(-1, null, null, null, "操作失败" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion
        #endregion

        #region 9.物料配置
        
        public ActionResult GridMatListJson(string productMatId, string wcId, JqGridParam jqgridparam)//在首页加载已经配置过的物料信息
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable dt=MyBll2.GetMatList(productMatId, wcId);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "物料配置加载成功");
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "物料配置加载发生异常错误：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        public ActionResult GridNotConfigJson(string productclassid ,JqGridParam jqgridparam)//加载未配置物料
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable dt = MyBll2.GetNotConfigMat(productclassid,ref jqgridparam);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                this.WriteLog(-1, null, null, null, "操作失败" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        public ActionResult NotConfigMatByCondition(string keywords, string Condition, string productclassid ,JqGridParam jqgridparam)//查询为配置物料清单
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable dt = MyBll2.GetNotConfigMatbycondition(keywords, Condition, productclassid,ref jqgridparam);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                this.WriteLog(-1, null, null, null, "操作失败" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        public ActionResult MatSubmit(string productclassid,string WcId,string WcCd,string WcNm, List<BBdbR_ProductWcMatConfig> tabledata)//物料配置提交
        {
            try
            {
                int IsOk = 0;//用于判断提交是否成功，成功为1，失败为0
                DataTable dt = MyBll2.GetPlineId(WcId);
                string PlineId = dt.Rows[0]["PlineId"].ToString();             
                for (int i=0;i< tabledata.Count; i++)
                {
                    tabledata[i].ProductClassMatId = System.Guid.NewGuid().ToString();
                    tabledata[i].ProductClassId = productclassid;
                    tabledata[i].PlineId = PlineId ;
                    tabledata[i].WcId = WcId;
                    tabledata[i].WcCd = WcCd;
                    tabledata[i].WcNm = WcNm;
                    tabledata[i].VersionNumber = "V1.0";
                    tabledata[i].Enabled = "1";
                    tabledata[i].CreTm = DateTime.Now;
                    tabledata[i].CreCd = ManageProvider.Provider.Current().UserId;
                    tabledata[i].CreNm = ManageProvider.Provider.Current().UserName;
                    IsOk = MyBll2.MatInsert(tabledata[i]);
                }
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString() }.ToString());
            }
            catch(Exception ex)
            {
                this.WriteLog (-1,null, null, null, "操作失败" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString()); 
            }
        }
        #endregion

        #region 10.删除配置
        public ActionResult ConfigMatDelete(string MatKeyValue)
        {
            try
            {
                int isok = MyBll.ConfigMatDelete(MatKeyValue);
                var JsonData = new
                {
                    rows = isok,
                };
                return Content(new JsonMessage { Success = true, Code = isok.ToString() }.ToString());
            }
            catch (Exception ex)
            {
                this.WriteLog(-1, null, null, null, "操作失败" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 11.打印
        public ActionResult PrintDataSource(string productMatId,string PlineCd)
        {
            try
            {
                DataTable dt = MyBll.Getprintdata1(productMatId, PlineCd);//PlineTyp,PlineNm,MatCd,MatNm 
                DataTable dt1 = MyBll.Getprintdata2(productMatId, PlineCd);
                DataTable dt2 = MyBll.GetPlineData(PlineCd);

                var Plinetype= dt2.Rows[0]["PlineTyp"].ToString();
                var Plinename = dt2.Rows[0]["PlineNm"].ToString();
                var JsonData = new
                {
                    rows= dt,
                    rows1 = dt1,
                    plinetype = Plinetype,
                    plinename=Plinename,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                this.WriteLog(-1, null, null, null, "操作失败" + ex.Message);////
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion
        #endregion


    }
}