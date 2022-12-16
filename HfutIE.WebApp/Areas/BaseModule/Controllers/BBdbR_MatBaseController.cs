using HfutIE.Business;
using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HfutIE.WebApp.Areas.BaseModule.Controllers
{
    /// <summary>
    /// _FactoryBaseInformation控制器
    /// </summary>
    public class BBdbR_MatBaseController : PublicController<BBdbR_MatBase>
    {
        #region 全局变量定义区

        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BBdbR_MatBase";//===复制时需要修改===

        #endregion

        #region 定义全局变量datatable用于存放搜索结果和导出
        public static DataTable dtExport = new DataTable();
        #endregion

        #region 创建数据库操作对象区域
        BBdbR_MatBaseBll MyBll = new BBdbR_MatBaseBll(); //===复制时需要修改===
        BBdbR_MatFileConfigBll MyBll2 = new BBdbR_MatFileConfigBll();
        BBdbR_PartMatConfigBll MyBll3 = new BBdbR_PartMatConfigBll();
        public readonly RepositoryFactory<BBdbR_MatBase> repository_matbase = new RepositoryFactory<BBdbR_MatBase>();
        public readonly RepositoryFactory<BBdbR_PartMatConfig> repository_partmatconfigbase = new RepositoryFactory<BBdbR_PartMatConfig>();
        #endregion

        #region 打开视图区域
        public ActionResult Picture()//打开上传图片子界面
        {
            return View();
        }
        public ActionResult Index2()//打开物料文档配置子界面
        {
            return View();
        }
        public ActionResult Form2()//打开物料文档配置子界面-配置界面
        {
            return View();
        }

        public ActionResult Form3()//打开组件物料配置子界面-配置界面
        {
            return View();
        }


        #endregion

        #region 方法区   

        #region 1.加载表格数据
        /// <summary>
        /// 加载列表 主页面
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageJson1()
        {
            try
            {
                #region List接收并传回前端
                //List<BBdbR_MatBase> ListData = MyBll.GetPlanList("");
                //var JsonData = new
                //{
                //    rows = ListData,
                //};
                //string a = JsonData.ToJson();
                //return Content(JsonData.ToJson());
                #endregion

                #region datatable 查询并传回前端
                Stopwatch watch = CommonHelper.TimerStart();
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select IIF(MatImg is null,'0','1') IfMatImg,
                IIF((select count(*) from BBdbR_PartMatConfig where PartCd=BBdbR_MatBase.MatCd)>0,'1','0') IfPart,* 
                from BBdbR_MatBase 
                where Enabled = 1 order by WcCd");
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                var JsonData = new
                {
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

        /// <summary>
        /// 加载表格 物料文档配置
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageJson2(string KeyValue)
        {
            try
            {
                List<BBdbR_MatFileConfig> ListData = MyBll2.GetList(KeyValue);
                var JsonData = new
                {
                    rows = ListData,
                };
                string a = JsonData.ToJson();
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 加载表格 组件物料配置
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageJson3(string KeyValue)
        {
            try
            {
                List<BBdbR_PartMatConfig> ListData = MyBll3.GetList(KeyValue);
                var JsonData = new
                {
                    rows = ListData,
                };
                string a = JsonData.ToJson();
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 3.展示页面表格
        /// <summary>
        /// 搜索表格中所有的数据，并以json的形式返回
        /// </summary>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public ActionResult GridPage(JqGridParam jqgridparam)
        {
            try
            {
                #region 查询原方法
                Stopwatch watch = CommonHelper.TimerStart();
                List<BBdbR_MatBase> ListData = MyBll.GetPageList(jqgridparam);    //===复制时需要修改===
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = ListData,
                };
                return Content(ListData.ToJson());
                #endregion



            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 4.新增编辑方法
        //entity表示页面中传入的实体，KeyValue表示传入的主键
        //不管是新增还是编辑，页面中都会传入实体（entity）
        //新增时实体是一个全新的实体
        //编辑时实体是修改后的实体
        //只有在编辑时页面中才会传入主键entity（KeyValue），该主键是需要编辑那个实体的主键
        //
        //编辑方法首先根据KeyValue是否有值判断是需要编辑还是需要新增
        //如果是新增就将该实体新增到数据库中
        //如果是编辑就将该实体更新到数据中
        //
        //不管是新增还是编辑首先判断页面输入的编号是否已经存在
        //如果已经存在就直接返回“该编号已经存在！”的信息
        //不存在再进行下一步
        public override ActionResult SubmitForm(BBdbR_MatBase entity, string KeyValue)//===复制时需要修改===
        {
            try
            {
                int IsOk = 0;//用于判断
                string Name = "MatCd";        //页面中的编号字段名，如：公司编号   //===复制时需要修改===
                string Value = entity.MatCd;  //页面中的编号字段值                 //===复制时需要修改===
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";

                if (!string.IsNullOrEmpty(KeyValue))//编辑操作
                {
                    //===复制时需要修改===
                    BBdbR_MatBase Oldentity = repositoryfactory.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    entity.MatId = KeyValue;
                    IsOk = MyBll.Update(entity);//将修改后的实体更新到数据库，插入成功返回1，失败返回0；
                    this.WriteLog(IsOk, entity, Oldentity, KeyValue, Message);//记录日志
                }
                else//新增操作
                {
                    IsOk = MyBll.CheckCount(tableName, Name);//判断页面中填写的数据的编号字段的值是否存在
                    if (IsOk > 0)//存在
                    {
                        Message = "该编号已经存在！";
                        return Content(new JsonMessage { Success = false, Code = IsOk.ToString(), Message = Message }.ToString());
                    }
                    entity.MatId = System.Guid.NewGuid().ToString();
                    IsOk = MyBll.Insert(entity);//将实体插入数据库，插入成功返回1，失败返回0；
                    this.WriteLog(IsOk, entity, null, KeyValue, Message);//记录日志
                }
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                this.WriteLog(-1, entity, null, KeyValue, "操作失败：" + ex.Message);//记录日志
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 5.删除方法
        /// <summary>
        /// 首先判断需要删除的信息是否绑定了其他信息
        ///     如：删除一条工厂信息先要判断该条公司信息下面是否绑定了车间信息
        ///         如果绑定了信息，则提示“当前所选有子节点数据，不能删除。”并结束
        ///  在确定没绑定数据的情况下，删除该数据
        ///     删除表示将该数据的Enable属性设为false，并不真的删除该记录
        /// </summary>
        /// <param name="KeyValue">页面中提供的需要删除的数据的主键,可能是多条数据的主键，即多个主键</param>
        /// <returns></returns>
        public  ActionResult DeleteMaterial(string KeyValue)//删除物料基础信息
        {
            //不管是多个主键还是单个主键，将主键拆分出来，放在数组中
            string[] array = KeyValue.Split(',');
            
            try
            {
                var Message = "删除失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//判断删除方法是否成，0表示不成功，大于0表示成功
                
                IsOk = MyBll.Delete(array);//执行删除操作
                for (int i = 0; i < array.Length; i++)
                {
                    
                    List<BBdbR_MatFileConfig> ListData = MyBll2.GetList(array[i]);
                    string[] array2 = new string[ListData.Count] ;
                    if (ListData.Count>0)
                    {
                        for (int j = 0; j < ListData.Count; j++)
                        {
                            array2[j] = ListData[j].ConfigId;
                        }
                        MyBll2.Delete(array2);
                    }
                    List<BBdbR_PartMatConfig> PartMatData = MyBll3.GetList(array[i]);
                    string[] array3 = new string[PartMatData.Count];
                    if (PartMatData.Count > 0)
                    {
                        for (int k = 0; k < PartMatData.Count; k++)
                        {
                            array3[k] = PartMatData[k].Id;
                        }
                        MyBll3.Delete(array3);
                    }

                }
                if (IsOk > 0)//表示删除成功
                {
                    Message = "删除成功。";//修改返回信息
                }
                WriteLog(IsOk, array, Message);
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, array, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        public ActionResult DeleteMatPic(string KeyValue)//删除物料基础信息
        {
            //不管是多个主键还是单个主键，将主键拆分出来，放在数组中
            string[] array = KeyValue.Split(',');

            try
            {
                var Message = "删除失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//判断删除方法是否成，0表示不成功，大于0表示成功

                for (int i = 0; i < array.Length; i++)
                {

                    string str = "UPDATE dbo.BBdbR_MatBase SET MatImg=null WHERE MatId='" + array[i] + "'";
                    IsOk=DbHelperSQL.ExecuteSql(str);

                }
                if (IsOk > 0)//表示删除成功
                {
                    Message = "删除成功。";//修改返回信息
                }
                WriteLog(IsOk, array, Message);
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, array, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }



        public ActionResult DeleteMaterial2(string KeyValue2)//删除物料文档配置
        {
            //不管是多个主键还是单个主键，将主键拆分出来，放在数组中
            string[] array = KeyValue2.Split(',');
            try
            {
                var Message = "删除失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//判断删除方法是否成，0表示不成功，大于0表示成功
                IsOk = MyBll2.Delete(array);//执行删除操作
                if (IsOk > 0)//表示删除成
                {
                    Message = "删除成功。";//修改返回信息
                }
                WriteLog(IsOk, array, Message);
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, array, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        public ActionResult DeleteMaterial3(string KeyValue3)//删除物料文档配置
        {
            //不管是多个主键还是单个主键，将主键拆分出来，放在数组中
            string[] array = KeyValue3.Split(',');
            try
            {
                var Message = "删除失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//判断删除方法是否成，0表示不成功，大于0表示成功
                IsOk = MyBll3.Delete(array);//执行删除操作
                if (IsOk > 0)//表示删除成
                {
                    Message = "删除成功。";//修改返回信息
                }
                WriteLog(IsOk, array, Message);
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, array, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        #endregion

        #region 6.查询方法
        //查询方法，本方法为单条件查询，即根据一个条件进行查询
        //查询条件为Condition，也是数据库表_CompanyBaseInformation中的一个字段名
        //查询值为keywords，也是数据库表_CompanyBaseInformation中的字段名的字段值
        //本查询采用近似查询（like）

        public ActionResult GridPageByCondition(string ConditionMatCd, string ConditionMatNm, string ConditionMatCatg, string ConditionMatTyp, string IsScan, string WcCd, string IsPrint, string RsvFld1, JqGridParam jqgridparam)
        {
            try
            {
                #region List接收并传回前端
                //string keyword = keywords.Trim();
                //Stopwatch watch = CommonHelper.TimerStart();
                //List<BBdbR_MatBase> ListData = MyBll.GetPageListByCondition(keyword, Condition, jqgridparam);//===复制时需要修改===
                //var JsonData = new
                //{
                //    total = jqgridparam.total,
                //    page = jqgridparam.page,
                //    records = jqgridparam.records,
                //    costtime = CommonHelper.TimerEnd(watch),
                //    rows = ListData,
                //};
                //return Content(ListData.ToJson());
                #endregion

                #region datatable查询并传回前端
                Stopwatch watch = CommonHelper.TimerStart();
                StringBuilder strSql = new StringBuilder();
                List<DbParameter> parameter = new List<DbParameter>();

                strSql.Append(@"select IIF(MatImg is null,'0','1') IfMatImg,
                IIF((select count(*) from BBdbR_PartMatConfig where PartCd=BBdbR_MatBase.MatCd and Enabled = '1')>0 ,'1','0') IfPart,* 
                from BBdbR_MatBase 
                where Enabled = 1 and MatCd like @ConditionMatCd  and MatNm like @ConditionMatNm and MatCatg like @ConditionMatCatg  ");
                parameter.Add(DbFactory.CreateDbParameter("@ConditionMatCd", "%" + ConditionMatCd + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@ConditionMatNm", "%" + ConditionMatNm + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@ConditionMatCatg", "%" + ConditionMatCatg + "%"));

                if (ConditionMatTyp!=""&& ConditionMatTyp!=null)
                {
                    strSql.Append(" and MatTyp = @ConditionMatTyp");
                    parameter.Add(DbFactory.CreateDbParameter("@ConditionMatTyp", "%" + ConditionMatTyp + "%"));
                }

                //是否关重件
                if (IsScan != "" && IsScan != null)
                {
                    if (IsScan=="1")
                    {
                        strSql.Append(" and IsScan = '1'");
                    }
                    else 
                    {
                        strSql.Append(" and IsScan != '1'");
                    }
                   
                }
                //关重件工位
                if (WcCd != "" && WcCd != null)
                {
                    strSql.Append(" and WcCd like @WcCd");
                    parameter.Add(DbFactory.CreateDbParameter("@WcCd", "%" + WcCd + "%"));
                }
                //是否打印
                if (IsPrint != "" && IsPrint != null)
                {
                    if (IsPrint == "1")
                    {
                        strSql.Append(" and IsPrint = '1'");
                    }
                    else
                    {
                        strSql.Append(" and IsPrint != '1'");
                    }

                }
                //打印工位
                if (RsvFld1 != "" && RsvFld1 != null)
                {
                    strSql.Append(" and RsvFld1 like @RsvFld1");
                    parameter.Add(DbFactory.CreateDbParameter("@RsvFld1", "%" + RsvFld1 + "%"));
                }

                //排序
                strSql.Append(" order by MatCd");

                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "物料基本档案查询成功");
                return Content(JsonData.ToJson());
                #endregion


            }
            catch (Exception ex)
            {
                //CCSLog.CCSLogHelper.WriteExLog(ex, CCSLog.LogType.WebSite);
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "物料基本档案查询发生异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion
            
        #region 7.检查字段是否唯一
        /// <summary>
        /// 根据传入的字段名和字段值判断该字段是否存在
        /// </summary>
        /// <param name="KeyName">字段名</param>
        /// <param name="KeyValue">字段值</param>
        /// <returns>返回该判断结果</returns>
        public ActionResult ChectOnlyOne(string KeyName, string KeyValue)
        {
            try
            {
                int IsOk = 0;
                string Message = "该字段已经存在！";

                if (!string.IsNullOrEmpty(KeyValue))
                {
                    IsOk = MyBll.CheckCount(tableName,KeyName);
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
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        #endregion

        #endregion

        #region 8.检验图片的尺寸
        /// <summary>
        /// 校验选中的图片的像素大小是否属于合理范围
        /// </summary>
        /// <returns></returns>
        public bool CheckPhotograph()
        {
            try
            {
                HttpFileCollectionBase files = Request.Files;
                HttpPostedFileBase file = files["file"];//获取上传的文件
                Stream st = file.InputStream;
                byte[] bytes = new byte[st.Length];
                BinaryReader br = new BinaryReader((Stream)st);
                bytes = br.ReadBytes((Int32)st.Length);
                bool is_standard = CheckPictureSize(bytes);//方法在下面， 将二进制流转化为图片，从而获取该图片的像素大小，并判断图片像素是否小于标准值
                if (is_standard == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 将二进制流转化为图片，从而获取该图片的像素大小，并判断图片像素是否小于标准值
        /// </summary>
        /// <param name="pageData">图片的二进制流</param>
        /// <returns></returns>
        public bool CheckPictureSize(byte[] pageData)
        {
            //将二进制流数据转换为图片  
            Image image = Image.FromStream(new MemoryStream(pageData));
            int width = image.Width;
            int height = image.Height;
            NameValueCollection forms = Request.Form;
            int width_stadard = Convert.ToInt16(forms["width"]);
            int height_stadard = Convert.ToInt16(forms["height"]);
            if (width <= width_stadard && height <= height_stadard)//图片像素大小小于标准值，返回true
            {
                return true;
            }
            else//图片像素大小大于标准值，返回false
            {
                return false;
            }
        }
        #endregion

        #region 9.提交图片
        /// <summary>
        /// 提交物料基础信息编辑表单
        /// </summary>
        /// <param name="ProductKey">产品编码</param>
        /// <param name="productdto">产品实体</param>
        /// <returns></returns>
        public ActionResult SubmitPicture(string KeyValue)
        {
            try
            {
                string Message = "";
                byte[] photograph = null;
                string photograph_type = "";
                photograph = GetPhotographFromRequest(out photograph_type);//获取图片的二进制和图片后缀名，方法在下面
                NameValueCollection forms = Request.Form;
                int IsOk = MyBll.InsertPicture(KeyValue, photograph, photograph_type);//将实体插入数据库，插入成功返回1，失败返回0；
                if (IsOk > 0)
                {
                    Message = "插入图片成功！";
                }
                else
                {
                    Message = "插入图片失败！";
                }

                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// 提交物料基础信息编辑表单
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public ActionResult SubmitPicture1(string KeyValue, BBdbR_MatBase entity , string file)
        {
            try
            {
                BBdbR_MatBase Oldentity = repository_matbase.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                int IsOk = 0;//用于判断
                string Name = "MatCd";
                string Value = entity.MatCd;  //页面中的编号字段值                 //===复制时需要修改===
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))//编辑操作
                {
                    //===复制时需要修改===
                    entity.MatId = KeyValue;
                    entity.Modify(KeyValue);//调用扩展编辑方法
                    
                    //FORM表单中为null的部分转为空
                    if (entity.WcCd == null)
                    {
                        entity.WcCd = "";
                        entity.WcNm = "";
                        entity.WcId = "";
                    }
                    else { }
                    if (entity.RsvFld1 == null)
                    {
                        entity.RsvFld1 = "";
                    }
                    else { }
                    if (entity.MatNum == null)
                    {
                        entity.MatNum = "";
                    }
                    else { }
                    if (entity.ShortCode == null)
                    {
                        entity.ShortCode = "";
                    }
                    else { }
                    if (entity.MatCatg == null)
                    {
                        entity.MatCatg = "";
                    }
                    else { }
                    if (entity.Rem == null)
                    {
                        entity.Rem = "";
                    }
                    else { }


                    int a = MyBll.Update(entity);//将实体插入数据库，插入成功返回1，失败返回0；

                    if (a > 0 && file != "undefined" )
                    {
                        byte[] photograph = null;
                        string photograph_type = "";
                        photograph = GetPhotographFromRequest(out photograph_type);//获取图片的二进制和图片后缀名，方法在下面
                        NameValueCollection forms = Request.Form;
                        IsOk = MyBll.InsertPicture(entity.MatCd, photograph, photograph_type);//将实体插入数据库，插入成功返回1，失败返回0；
                        if (IsOk > 0)
                        {
                            Message = "编辑物料信息成功！";
                        }
                        else
                        {
                            Message = "编辑物料信息失败！";
                        }
                    }
                    if (a>0)//编辑但是没有更新图片
                    {
                        IsOk = 1;
                        Message = "编辑物料信息成功！";
                    }
                    this.WriteLog(IsOk, entity, Oldentity, KeyValue, Message);//记录日志
                }
                else//新增操作
                {
                    IsOk = MyBll.CheckCount(Name, Value);//判断页面中填写的数据的编号字段的值是否存在
                    if (IsOk > 0)//存在
                    {
                        Message = "该编号已经存在！";
                        return Content(new JsonMessage { Success = false, Code = IsOk.ToString(), Message = Message }.ToString());
                    }
                    
                    entity.Create();
                    entity.Enabled = "1";
                    int a = MyBll.Insert(entity);//将实体插入数据库，插入成功返回1，失败返回0；

                    var files = System.Web.HttpContext.Current.Request.Files;
                    if (files.Count > 0)
                    {
                        var file1 = files[0].InputStream;
                        byte[] buff = new byte[file1.Length];
                    }
                    else { }
                    if (a > 0)
                    {
                        if (files.Count > 0)
                        {
                            byte[] photograph = null;
                            string photograph_type = "";
                            photograph = GetPhotographFromRequest(out photograph_type);//获取图片的二进制和图片后缀名，方法在下面
                            NameValueCollection forms = Request.Form;
                            IsOk = MyBll.InsertPicture(entity.MatCd, photograph, photograph_type);//将实体插入数据库，插入成功返回1，失败返回0；
                        }
                        else
                        {
                            IsOk = 1;

                        }
                        
                        if (IsOk > 0)
                        {
                            Message = "新增物料信息成功！";
                        }
                        else
                        {
                            Message = "新增物料信息失败！";
                        }
                    }
                }
                this.WriteLog(IsOk, entity, null, KeyValue, Message);//记录日志
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
               
            }
            catch (Exception ex)
            {
                this.WriteLog(-1, entity, null, KeyValue, "操作失败：" + ex.Message);//记录日志SubmitForm3
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 从request中获取上传图片的二进制
        /// </summary>
        /// <param name="photograph_type">通过out修饰符返回图片后缀名</param>
        /// <returns></returns>
        public byte[] GetPhotographFromRequest(out string photograph_type)
        {
            try
            {
                HttpFileCollectionBase files = Request.Files;
                HttpPostedFileBase file = files["file"];//获取上传的文件
                photograph_type = file.FileName.Substring(file.FileName.LastIndexOf('.'));//获取上传的文件后缀名
                if (file != null && file.InputStream != null)
                {
                    Stream st = file.InputStream;//生成文件流对象
                    byte[] bytes = new byte[st.Length];
                    BinaryReader br = new BinaryReader((Stream)st);
                    bytes = br.ReadBytes((Int32)st.Length);
                    return bytes;
                }
                return null;
            }
            catch (Exception)
            {
                photograph_type = "";
                return null;
            }
        }
        #endregion

        #region 获取图片
        public ActionResult getPicture(string ID)
        {
            try
            {
                DataTable img = MyBll.getPicture(ID);
                return Content(img.ToJson());
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                return null;
            }
        }
        #endregion

        #region 编辑获取实体
        /// <summary>
        /// 获取物料信息对象返回JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SetMatForm(string KeyValue)
        {
            try
            {
                DataTable ListData = MyBll.GetMatInfor(KeyValue);
                var JsonData = new
                {
                    rows = ListData,
                };
                //var s = JsonData.ToJson();
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }

            //BBdbR_MatBase base_matbase = DataFactory.Database().FindEntity<BBdbR_MatBase>(KeyValue);
            //if (base_matbase == null)
            //{
            //    return Content("");
            //}
            ////Base_Employee base_employee = DataFactory.Database().FindEntity<Base_Employee>(KeyValue);
            //string strJson = base_matbase.ToJson();
            ////自定义
            //strJson = strJson.Insert(1, Base_FormAttributeBll.Instance.GetBuildForm(KeyValue));
            //return Content(strJson);

            //try
            //{
            //    DataTable ListData = MyBll.GetMatInfor(KeyValue);
            //    if (ListData.Rows.Count > 0)
            //    {
            //        byte[] pic = (byte[])ListData.Rows[0]["MatImg"];
            //        Image foodpic = BytesToImage(pic);
            //        ListData.Rows[0]["MatImg"] = foodpic;
            //    }

            //    return Content(ListData.ToJson());
            //}
            //catch (Exception ex)
            //{
            //    Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
            //    return null;
            //}
        }

        /// <summary>
        /// 获取物料信息对象返回JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SetPartMatForm(string KeyValue)
        {
            try
            {
                DataTable ListData = MyBll.GetPartMatInfor(KeyValue);
                var JsonData = new
                {
                    rows = ListData,
                };
                //var s = JsonData.ToJson();
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SetMatForm3(string KeyValue)
        {
            try
            {
                DataTable ListData = MyBll2.GetMatInfor(KeyValue);

                if(ListData.Rows[0]["FileTy"].ToString() == "文档")
                {
                   
                }
                var JsonData = new
                {
                    rows = ListData,
                };
                //var s = JsonData.ToJson();
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }

            
        }
        //二进制转图片
        public static Image BytesToImage(byte[] buffer)
        {
            using (MemoryStream ms = new MemoryStream(buffer))
            {
                try
                {
                    Image image = System.Drawing.Image.FromStream(ms);
                    return image;
                }
                catch (Exception ex)
                {
                    Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                    return null;
                }

            }
        }
        #endregion

        # region 物料文档配置获取实体-用于form2获取物料编号
        /// <summary>
        /// 获取物料信息对象返回JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SetMatForm2(string KeyValue)
        {
            try
            {
                DataTable ListData = MyBll.GetMatInfor(KeyValue);
                var JsonData = new
                {
                    rows = ListData,
                };
                //var s = JsonData.ToJson();
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }

           
        }

        #endregion

        #region 提交物料文档配置表单
        /// <summary>
        /// 提交物料文档配置表单
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public ActionResult SubmitPicture2(string KeyValue, BBdbR_MatFileConfig entity2, string file)//提交图片
        {
            try
            {
                int IsOk = 0;//用于判断
                string Name = "FileCd";
                string Value = entity2.FileCd;  //页面中的编号字段值                 //===复制时需要修改===
                string Message = "" ;
                    IsOk = MyBll2.CheckCount(Name, Value);//判断页面中填写的数据的编号字段的值是否存在
                    if (IsOk > 0)//存在
                    {
                        Message = "该编号已经存在！";
                        return Content(new JsonMessage { Success = false, Code = IsOk.ToString(), Message = Message }.ToString());
                    }
                //新增操作
                    entity2.Create();
                    var files = System.Web.HttpContext.Current.Request.Files;
                    var file1 = files[0].InputStream;
                    byte[] buff = new byte[file1.Length];
                    entity2.MatId = KeyValue;
                    int a = MyBll2.Insert(entity2);//将实体插入数据库，插入成功返回1，失败返回0；
                    if (a > 0)
                    {
                        byte[] photograph = null;
                        string photograph_type = "";
                        photograph = GetPhotographFromRequest(out photograph_type);//获取图片的二进制和图片后缀名，方法在下面
                        NameValueCollection forms = Request.Form;
                        IsOk = MyBll2.InsertPicture(entity2.FileCd, photograph, photograph_type);//将实体插入数据库，插入成功返回1，失败返回0；
                        if (IsOk > 0)
                        {
                            Message = "新增配置信息成功！";
                        }
                        else
                        {
                            Message = "新增配置信息失败！";
                        }
                    }
                
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());

            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }


        public ActionResult SubmitDocument(string KeyValue, string MatId, string FileCd, string FileTy, string Enabled, string Rem, string file,string path2)//提交文件
        {
            try
            {
                int IsOk = 0;//用于判断
                string Name = "FileCd";
                string Value = FileCd;  //页面中的编号字段值                 //===复制时需要修改===
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                //path2 = @"C:\Users\Administrator\Desktop";

                IsOk = MyBll2.CheckCount(Name, Value);//判断页面中填写的数据的编号字段的值是否存在
                if (IsOk > 0)//存在
                {
                    Message = "该编号已经存在！";
                    return Content(new JsonMessage { Success = false, Code = IsOk.ToString(), Message = Message }.ToString());
                }
                //新增操作
                //先将实体插入数据库
                BBdbR_MatFileConfig entity = new BBdbR_MatFileConfig();
                entity.ConfigId = System.Guid.NewGuid().ToString();
                entity.MatId = MatId;
                entity.FileCd = FileCd;
                entity.FileTy = FileTy;
                entity.Enabled = Enabled;
                entity.Rem = Rem;
                int a = MyBll2.Insert(entity);//将实体插入数据库，插入成功返回1，失败返回0；
                if (a > 0)
                {
                   // byte[] document = null;
                    byte[] document2 = null;
                    //string document_type = "";
                    document2 = DocumentToByte(path2);
                    //document = GetDocumentFromRequest(out document_type);//获取文件的二进制和文件后缀名，方法在下面
                    NameValueCollection forms = Request.Form;
                    IsOk = MyBll2.InsertDocument(FileCd, document2);//将实体插入数据库，插入成功返回1，失败返回0；
                    if (IsOk > 0)
                    {
                        Message = "新增配置信息成功！";
                    }
                    else
                    {
                        Message = "新增配置信息失败！";
                    }
                }

                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());

            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 文件转为二进制
        public byte[] GetDocumentFromRequest(out string document_type)
        {
            try
            {
                HttpFileCollectionBase files = Request.Files;
                HttpPostedFileBase file = files["file"];//获取上传的文件
                document_type = file.FileName.Substring(file.FileName.LastIndexOf('.'));//获取上传的文件后缀名
                if (file != null && file.InputStream != null)
                {
                    Stream st = file.InputStream;//生成文件流对象
                    byte[] bytes = new byte[st.Length];
                    BinaryReader br = new BinaryReader((Stream)st);
                    bytes = br.ReadBytes((Int32)st.Length);
                    return bytes;
                }
                return null;
            }
            catch (Exception)
            {
                document_type = "";
                return null;
            }
        }

      
        /// <summary>
        /// 文档文件转为二进制
        /// </summary>
        /// <param name="document_type">后缀</param>
        /// <returns></returns>
        public byte[] GetDocumentFromRequest2(string path)
        {
            try
            {
                if (!System.IO.File.Exists(path))
                {
                    return new byte[0];
                }
                FileInfo fi = new FileInfo(path);
                byte[] buff = new byte[fi.Length];
                FileStream fs = fi.OpenRead();
                fs.Read(buff, 0, Convert.ToInt32(fs.Length));
                fs.Close();
                return buff;
            }
            catch (Exception)
            {
                path = "";
                return null;
            }
        }
        #endregion


        #region 文档文件转二进制
        public byte[] DocumentToByte(string path2)
        {
            FileStream fs =new FileStream(path2, FileMode.Open);
            BinaryReader br =new BinaryReader(fs);
            Byte[] byData = br.ReadBytes((int)fs.Length);
            fs.Close();
            return byData;
        }
        #endregion


        public static byte[] Serialize(object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            bf.Serialize(stream, obj);
            byte[] datas = stream.ToArray();
            stream.Dispose();
            return datas;
        }

        #region 二进制转换成文件
        /// <summary>
        /// 将byte数组转换为文件
        /// </summary>
        /// <param name="buff">byte数组</param>
        /// <param name="savepath">保存地址</param>
        [HttpPost]
        [ValidateInput(false)]
        public  void Bytes2File(byte[] buff)
        {

            string savepath = @"D:\test\测试.docx";
            if (System.IO.File.Exists(savepath))
            {
                System.IO.File.Delete(savepath);
            }
            FileStream fs = new FileStream(savepath, FileMode.CreateNew);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(buff, 0, buff.Length);
            bw.Close();
            fs.Close();
        }
        #endregion

        #region 4.导入
        ///// <summary>
        ///// 导入Excel弹出框页面
        ///// </summary>
        ///// <returns></returns>
        //[ManagerPermission(PermissionMode.Enforce)]
        //public ActionResult ExcelImportDialog()
        //{
        //    string moduleId = DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId"));
        //    //模板主表
        //    Base_ExcelImport base_excellimport = DataFactory.Database().FindEntity<Base_ExcelImport>("ModuleId", moduleId);
        //    if (base_excellimport.ModuleId != null)
        //    {
        //        ViewBag.ModuleId = moduleId;
        //        ViewBag.ImportFileName = base_excellimport.ImportFileName;
        //        ViewBag.ImportName = base_excellimport.ImportName;
        //        ViewBag.ImportId = base_excellimport.ImportId;
        //    }
        //    else
        //    {
        //        ViewBag.ModuleId = "0";
        //    }
        //    return View();
        //}
        ///// <summary>
        ///// 导入Excell数据
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult ImportExel()
        //{
        //    int IsOk = 0;//导入状态
        //    int IsCheck = 1;//用作检验重复地址的标识
        //    DataTable Result = new DataTable();//导入错误记录表
        //    IDatabase database = DataFactory.Database();
        //    List<BBdbR_MatBase> BFacRShiftBaseList = new List<BBdbR_MatBase>();

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
        //        if (file != null)
        //        {
        //            string fullname = file.FileName;
        //            string IsXls = System.IO.Path.GetExtension(fullname).ToString().ToLower();//System.IO.Path.GetExtension获得文件的扩展名
        //            if (!IsXls.EndsWith(".xls") && !IsXls.EndsWith(".xlsx"))
        //            {
        //                IsOk = 0;
        //            }
        //            else
        //            {

        //                string filename = Guid.NewGuid().ToString() + ".xls";
        //                if (fullname.EndsWith(".xlsx"))
        //                {
        //                    filename = Guid.NewGuid().ToString() + ".xlsx";
        //                }
        //                if (file != null && file.FileName != "")
        //                {
        //                    string msg = UploadHelper.FileUpload(file, Server.MapPath("~/Resource/UploadFile/ImportExcel/"), filename);
        //                }

        //                DataTable dt = ImportExcel.ExcelToDataTable(Server.MapPath("~/Resource/UploadFile/ImportExcel/") + filename);

        //                RemoveEmpty(dt);//清除空行。???=>20210712注：方法是否真的有用？void返回对dt未生效
        //                dt.Columns.Add("rowid", typeof(System.String));//用来标识excell行ID
        //                for (int i = 0; i < dt.Rows.Count; i++)
        //                {
        //                    dt.Rows[i]["rowid"] = i + 1;
        //                }
        //                #region 物料基础信息导入
        //                //校验
        //                for (int i = 0; i < dt.Rows.Count; i++)
        //                {

        //                    IsCheck = 1;//重置标识
        //                    DataRow dr = Newdt.NewRow();

        //                    if (dt.Rows[i]["物料名称"].ToString().Trim() != "" && dt.Rows[i]["物料类别"].ToString().Trim() != "" && dt.Rows[i]["物料类型"].ToString().Trim() != "" && dt.Rows[i]["物料编号"].ToString().Trim() != "")
        //                    {
        //                        BBdbR_MatBase Entity = new BBdbR_MatBase();

        //                        Entity.MatId = System.Guid.NewGuid().ToString();//主键
        //                        Entity.MatCd = dt.Rows[i]["物料编号"].ToString().Trim();
        //                        Entity.MatNm = dt.Rows[i]["物料名称"].ToString().Trim();
        //                        Entity.MatCatg = dt.Rows[i]["物料类别"].ToString().Trim();
        //                        Entity.MatTyp = dt.Rows[i]["物料类型"].ToString().Trim();
        //                        Entity.MatSpec = dt.Rows[i]["规格型号"].ToString().Trim();
        //                        Entity.LeadTm = dt.Rows[i]["提前期"].ToString().Trim();
        //                        Entity.YieldRate = Convert.ToDecimal(dt.Rows[i]["良品率"].ToString().Trim());
        //                        Entity.Rem = dt.Rows[i]["备注"].ToString().Trim();

        //                        Entity.VersionNumber = "V1.0"; //版本号
        //                        Entity.Enabled = "1";          //有效性
        //                        Entity.CreTm = DateTime.Now.ToLocalTime(); //创建时间
        //                        Entity.CreCd = ManageProvider.Provider.Current().UserId;     //创建人编号
        //                        Entity.CreNm = ManageProvider.Provider.Current().UserName;   //创建人名称

        //                        BFacRShiftBaseList.Add(Entity);
        //                        int b = database.Insert(BFacRShiftBaseList);
        //                        if (b > 0)
        //                        {
        //                            IsOk = IsOk + b;
        //                            BFacRShiftBaseList.Clear();
        //                        }
        //                        else
        //                        {
        //                            dr = Newdt.NewRow();
        //                            dr[0] = errorNum;
        //                            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行";
        //                            dr[2] = "物料信息插入失败";
        //                            Newdt.Rows.Add(dr);
        //                            IsCheck = 0;
        //                            continue;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        dr = Newdt.NewRow();
        //                        dr[0] = errorNum;
        //                        dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行";
        //                        dr[2] = "物料信息不能为空";
        //                        Newdt.Rows.Add(dr);
        //                        errorNum++;
        //                        IsCheck = 0;
        //                        continue;
        //                    }
        //                }
        //                if (IsCheck == 0)
        //                {
        //                    IsOk = 0;
        //                }
        //                #endregion

        //            }
        //            Result = Newdt;
        //        }

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
        #endregion

        #region 5.导出模板
        ///// <summary>
        ///// 下载Excell模板
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult GetExcellTemperature(string ImportId)
        //{
        //    if (!string.IsNullOrEmpty(ImportId))
        //    {
        //        DataSet ds = new DataSet();
        //        DataTable data = new DataTable(); string DataColumn = ""; string fileName;
        //        MyBll.GetExcellTemperature(ImportId, out data, out DataColumn, out fileName);
        //        ds.Tables.Add(data);
        //        MemoryStream ms = DeriveExcel.ExportToExcel(ds, "xls", DataColumn.Split('|'));
        //        if (!fileName.EndsWith(".xls"))
        //        {
        //            fileName = fileName + ".xls";
        //        }
        //        return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
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

        #endregion

        #region 4.提交组件物料配置表单
        //entity表示页面中传入的实体，KeyValue表示传入的主键
        //不管是新增还是编辑，页面中都会传入实体（entity）
        //新增时实体是一个全新的实体
        //编辑时实体是修改后的实体
        //只有在编辑时页面中才会传入主键entity（KeyValue），该主键是需要编辑那个实体的主键
        //
        //编辑方法首先根据KeyValue是否有值判断是需要编辑还是需要新增
        //如果是新增就将该实体新增到数据库中
        //如果是编辑就将该实体更新到数据中
        //
        //不管是新增还是编辑首先判断页面输入的编号是否已经存在
        //如果已经存在就直接返回“该编号已经存在！”的信息
        //不存在再进行下一步
        public ActionResult SubmitForm3(BBdbR_PartMatConfig entity, string KeyValue)//===复制时需要修改===
        {
            try
            {
                int IsOk = 0;//用于判断
                KeyValue = entity.Id;
                string Message = entity.Id==""? "新增成功" : "编辑成功";

                #region 根据WcCd获取WcId和WcNm
                StringBuilder strSqlWc = new StringBuilder();
                strSqlWc.Append(@"select * from BBdbR_WcBase where WcCd = '" + entity.WcCd + "'  and Enabled =1");
                DataTable dt = DataFactory.Database().FindTableBySql(strSqlWc.ToString(), false);
                string WcNm = "";
                if (dt.Rows.Count > 0)
                {
                    entity.WcNm = dt.Rows[0]["WcNm"].ToString();
                    entity.WcId = dt.Rows[0]["WcId"].ToString();
                }
                else { }
                #endregion

                if (entity.Id == ""||entity.Id == null)
                {
                    IsOk = MyBll3.CheckCount2(entity.PartCd, entity.MatCd);//判断页面中填写的数据的编号字段的值是否存在
                    if (IsOk > 0)//存在
                    {
                        Message = "该编号已经存在！";
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = Message }.ToString());
                    }
                    entity.Create();
                    IsOk = MyBll3.Insert(entity);//将实体插入数据库，插入成功返回1，失败返回0；
                    if (IsOk == 0)
                    {
                        Message = "新增失败！";
                    }
                    WriteLog(IsOk, entity, null, KeyValue, Message);//记录日志
                }
                else
                {
                    BBdbR_PartMatConfig Oldentity = repository_partmatconfigbase.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    if (entity.WcCd==null)
                    {
                        entity.WcCd = " ";
                    }
                    else { }

                    if (entity.RsvFld1 == null)
                    {
                        entity.RsvFld1 = " ";
                    }
                    else { }

                    if (entity.MatNum == null)
                    {
                        entity.MatNum = " ";
                    }
                    else { }

                    if (entity.ShortCode == null)
                    {
                        entity.ShortCode = " ";
                    }
                    else { }

                    if (entity.Rem == null)
                    {
                        entity.Rem = " ";
                    }
                    else { }


                    entity.Modify();
                    IsOk = MyBll3.update(entity);//将实体插入数据库，插入成功返回1，失败返回0；
                    if (IsOk == 0)
                    {
                        Message = "编辑失败！";
                    }
                    this.WriteLog(IsOk, entity, Oldentity, KeyValue, Message);//记录日志
                }
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                this.WriteLog(-1, entity, null, KeyValue, "操作失败：" + ex.Message);//记录日志
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 写入作业日志
        /// <summary>
        /// 写入作业日志（新增、修改）
        /// </summary>
        /// <param name="IsOk">操作状态</param>
        /// <param name="entity">实体对象</param>
        /// <param name="Oldentity">之前实体对象</param>
        /// <param name="Message">备注信息</param>
        public void WriteLog(int IsOk, BBdbR_PartMatConfig entity, BBdbR_PartMatConfig Oldentity, string KeyValue, string Message = "")
        {
            if (!string.IsNullOrEmpty(KeyValue))
            {
                Base_SysLogBll.Instance.WriteLog(Oldentity, entity, OperationType.Update, IsOk.ToString(), Message);
            }
            else
            {
                Base_SysLogBll.Instance.WriteLog(entity, OperationType.Add, IsOk.ToString(), Message);
            }
        }
        #endregion

        #region 重构导出
        public ActionResult GetExcel_Data(string ConditionMatCd, string ConditionMatNm, string ConditionMatCatg,string ConditionMatTyp, string IsScan, string WcCd, string IsPrint, string RsvFld1, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = new DataTable();
                List<DbParameter> parameter = new List<DbParameter>();
                #region 查询
                StringBuilder strSql = new StringBuilder();

                strSql.Append(@"select MatCd,MatNm,WcCd,IsScan,MatNum,IsPrint,RsvFld1,
                IIF((select count(*) from BBdbR_PartMatConfig where PartCd=BBdbR_MatBase.MatCd)>0,'1','0') IfPart,RsvFld2,ShortCode,MatCatg,MatTyp,CreTm,MdfTm,Rem 
                from BBdbR_MatBase 
                where Enabled = 1 and MatCd like @ConditionMatCd  and MatNm like @ConditionMatNm and MatCatg like @ConditionMatCatg");

                parameter.Add(DbFactory.CreateDbParameter("@ConditionMatCd", "%" + ConditionMatCd + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@ConditionMatNm", "%" + ConditionMatNm + "%"));
                parameter.Add(DbFactory.CreateDbParameter("@ConditionMatCatg", "%" + ConditionMatCatg + "%"));

                if (ConditionMatTyp != "" && ConditionMatTyp != null)
                {
                    strSql.Append(" and MatTyp = @ConditionMatTyp");
                    parameter.Add(DbFactory.CreateDbParameter("@ConditionMatTyp", "%" + ConditionMatTyp + "%"));
                }

                //是否关重件
                if (IsScan != "" && IsScan != null)
                {
                    if (IsScan == "1")
                    {
                        strSql.Append(" and IsScan = '1'");
                    }
                    else
                    {
                        strSql.Append(" and IsScan != '1'");
                    }

                }
                //关重件工位
                if (WcCd != "" && WcCd != null)
                {
                    strSql.Append(" and WcCd like @WcCd");
                    parameter.Add(DbFactory.CreateDbParameter("@WcCd", "%" + WcCd + "%"));
                }
                //是否打印
                if (IsPrint != "" && IsPrint != null)
                {
                    if (IsPrint == "1")
                    {
                        strSql.Append(" and IsPrint = '1'");
                    }
                    else
                    {
                        strSql.Append(" and IsPrint != '1'");
                    }

                }
                //打印工位
                if (RsvFld1 != "" && RsvFld1 != null)
                {
                    strSql.Append(" and RsvFld1 like @RsvFld1");
                    parameter.Add(DbFactory.CreateDbParameter("@RsvFld1", "%" + RsvFld1 + "%"));
                }

                //排序
                strSql.Append(" order by MatCd");

                DataTable outputdt  = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                #endregion


                //if (dtExport.Rows.Count > 0)
                //{
                //    ListData = dtExport.DefaultView.ToTable("物料基础信息", true, "MatCd", "MatNm", "WcCd", "WcNm", "IsScan", "MatNum", "IsPrint", "ShortCode", "MatCatg", "MatSpec", "LeadTm", "YieldRate", "CreTm", "MdfTm", "Rem");//获取表中特定列
                //}

                string fileName = "物料基础信息";
                string excelType = "xls";
                MemoryStream ms = DeriveExcel.ExportExcel_MatBase(outputdt, excelType);
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "物料基本档案导出成功");
                return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "物料基本档案导出操作失败：" + ex.Message);
                return null;
            }

        }
        #endregion

        #region 导入-关重件

        
        /// <summary>
        /// 导入Excel弹出框页面
        /// </summary>
        /// <returns></returns>
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult ExcelImportDialog()
        {
            string moduleId = DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId"));
            //模板主表
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

            return View();
        }
        #region 导出模板
        /// <summary>
        /// 下载Excell模板
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
        /// 清除Datatable空行
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
        /// 导入Excell数据
        /// </summary>
        /// <returns></returns>
        public ActionResult ImportExel(string AlarmId)
        {
            int IsOk = 0;//导入状态
            int IsCheck = 1;//用作检验重复地址的标识
            DataTable Result = new DataTable();//导入错误记录表
            IDatabase database = DataFactory.Database();

            //用来存储已有物料信息
            List<BBdbR_MatBase> BBdbR_MatBaseList0 = new List<BBdbR_MatBase>();                                         //物料信息List
            //用来存储工位信息
            List<BBdbR_WcBase> BBdbR_WcBaseList0 = new List<BBdbR_WcBase>();                                            //工位信息List
            //用来存储导入数据
            List<BBdbR_MatBase> BBdbR_MatBaseList1 = new List<BBdbR_MatBase>();                                         //物料信息List
            

            //构造导入返回结果表
            DataTable Newdt = new DataTable("Result");
            Newdt.Columns.Add("rowid", typeof(System.String));                  //行号
            Newdt.Columns.Add("locate", typeof(System.String));                 //位置
            Newdt.Columns.Add("reason", typeof(System.String));                 //原因
            int errorNum = 1;
            try
            {
                string moduleId = Request["moduleId"]; //表名
                StringBuilder sb_table = new StringBuilder();
                HttpFileCollectionBase files = Request.Files;
                HttpPostedFileBase file = files["filePath"];//获取上传的文件
                string fullname = file.FileName;
                string IsXls = System.IO.Path.GetExtension(fullname).ToString().ToLower();//System.IO.Path.GetExtension获得文件的扩展名
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

                    RemoveEmpty(dt);//清除空行
                    dt.Columns.Add("rowid", typeof(System.String));//用来标识excell行ID
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["rowid"] = i + 1;
                    }
                    #region 关重件数据导入

                    #region 导入前获取所有物料信息并存入BBdbR_MatBaseList0中
                    StringBuilder strSqlMat0 = new StringBuilder();
                    DataTable dtCheckMat0 = new DataTable();
                    strSqlMat0.Append(@"select * from BBdbR_MatBase where Enabled =1");
                    dtCheckMat0 = DataFactory.Database().FindTableBySql(strSqlMat0.ToString(), false);
                    foreach (DataRow item in dtCheckMat0.Rows)
                    {
                        BBdbR_MatBase NewMatBase = new BBdbR_MatBase();
                        NewMatBase.MatId = item["MatId"].ToString(); //物料主键
                        NewMatBase.MatCd = item["MatCd"].ToString(); //物料编号
                        NewMatBase.MatNm = item["MatNm"].ToString(); //物料名称
                        NewMatBase.WcId = item["WcId"].ToString();   //工位主键
                        NewMatBase.WcCd = item["WcCd"].ToString();   //工位编号
                        NewMatBase.WcNm = item["WcNm"].ToString();   //工位名称
                        NewMatBase.IsScan = item["IsScan"].ToString();   //是否关重件
                        NewMatBase.IsPrint = item["IsPrint"].ToString(); //是否打印
                        NewMatBase.ShortCode = item["ShortCode"].ToString(); //简码
                        BBdbR_MatBaseList0.Add(NewMatBase);
                    }
                    #endregion

                    #region 导入前获取所有工位信息并存入BBdbR_WcBaseList0中
                    StringBuilder strSqlWc0 = new StringBuilder();
                    DataTable dtCheckWc0 = new DataTable();
                    strSqlWc0.Append(@"select * from BBdbR_WcBase where Enabled =1");
                    dtCheckWc0 = DataFactory.Database().FindTableBySql(strSqlWc0.ToString(), false);
                    foreach (DataRow item in dtCheckWc0.Rows)
                    {
                        BBdbR_WcBase NewWcBase = new BBdbR_WcBase();
                        NewWcBase.WcId = item["WcId"].ToString();   //工位主键
                        NewWcBase.WcCd = item["WcCd"].ToString();   //工位编号
                        NewWcBase.WcNm = item["WcNm"].ToString();   //工位名称
                        BBdbR_WcBaseList0.Add(NewWcBase);
                    }
                    #endregion

                    #region 表中数据循环录入
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        IsCheck = 1;//重置标识
                        DataRow dr = Newdt.NewRow();

                        string MatCd = dt.Rows[i]["物料编号"].ToString().Trim();
                        string WcCd = dt.Rows[i]["关重件工位编号"].ToString().Trim();
                        string WcId = "";
                        string WcNm = "";
                        string IsScan = dt.Rows[i]["是否关重件"].ToString().Trim();
                        string IsPrint = dt.Rows[i]["是否打印"].ToString().Trim();
                        string RsvFld1 = dt.Rows[i]["打印工位编号"].ToString().Trim();
                        string ShortCode = dt.Rows[i]["简码"].ToString().Trim();

                        //物料编号不能为空
                        if (dt.Rows[i]["物料编号"].ToString().Trim() != "")
                        {
                            //如果关重件字符不为空
                            if (IsScan != "" && IsScan != null)
                            {
                                #region  判断关重件信息是否符合要求
                                if (IsScan == "0" || IsScan == "1" || IsScan == "是" || IsScan == "否")
                                {
                                    if (IsScan == "0" || IsScan == "否")
                                    {
                                        IsScan = "0";
                                    }
                                    else
                                    {
                                        IsScan = "1";
                                        #region  判断关重件工位编号是否正确
                                        if (WcCd != "" && WcCd != null)
                                        {
                                            bool ifExistWc = BBdbR_WcBaseList0.Exists(t => t.WcCd == WcCd);
                                            if (ifExistWc == false)
                                            {
                                                dr = Newdt.NewRow();
                                                dr[0] = errorNum;
                                                dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行";
                                                dr[2] = "关重件工位编号不在系统中";
                                                Newdt.Rows.Add(dr);
                                                errorNum++;
                                                IsCheck = 0;
                                                continue;
                                            }
                                            else //如果存在，获取该关重件工位名称
                                            {
                                                BBdbR_WcBase thisWcBase = BBdbR_WcBaseList0.Find(t => t.WcCd == WcCd);
                                                WcId = thisWcBase.WcId;
                                                WcNm = thisWcBase.WcNm;
                                            }
                                        }
                                        else
                                        {
                                            WcId = "";
                                            WcCd = "";
                                            WcNm = "";
                                        }
                                        #endregion
                                    }
                                }
                                else
                                {
                                    dr = Newdt.NewRow();
                                    dr[0] = errorNum;
                                    dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行";
                                    dr[2] = "是否关重件输入字符不规范，请输入0或1";
                                    Newdt.Rows.Add(dr);
                                    errorNum++;
                                    IsCheck = 0;
                                    continue;
                                }
                                #endregion

                                #region  判断打印信息是否符合要求
                                if (IsPrint != "" && IsPrint != null)
                                {

                                    if (IsPrint == "0" || IsPrint == "1" || IsPrint == "是" || IsPrint == "否")
                                    {
                                        if (IsPrint == "0" || IsPrint == "否")
                                        {
                                            IsPrint = "0";
                                        }
                                        else
                                        {
                                            IsPrint = "1";
                                            #region  判断打印工位编号是否正确
                                            if (RsvFld1 != "" && RsvFld1 != null)
                                            {
                                                bool ifExistWc = BBdbR_WcBaseList0.Exists(t => t.WcCd == RsvFld1);
                                                if (ifExistWc == false)
                                                {
                                                    dr = Newdt.NewRow();
                                                    dr[0] = errorNum;
                                                    dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行";
                                                    dr[2] = "打印工位编号不在系统中";
                                                    Newdt.Rows.Add(dr);
                                                    errorNum++;
                                                    IsCheck = 0;
                                                    continue;
                                                }
                                                else { }
                                            }
                                            else
                                            {
                                                RsvFld1 = "";
                                            }
                                            #endregion
                                        }
                                    }
                                    else
                                    {
                                        dr = Newdt.NewRow();
                                        dr[0] = errorNum;
                                        dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行";
                                        dr[2] = "是否打印输入字符不规范，请输入0或1";
                                        Newdt.Rows.Add(dr);
                                        errorNum++;
                                        IsCheck = 0;
                                        continue;
                                    }

                                    
                                }
                                else
                                {
                                    IsPrint = "";
                                }
                                #endregion
                            }
                            else //如果关重件字符为空
                            {
                                IsScan = "";

                                #region  判断打印信息是否符合要求
                                if (IsPrint != "" && IsPrint != null)
                                {

                                    if (IsPrint == "0" || IsPrint == "1" || IsPrint == "是" || IsPrint == "否")
                                    {
                                        if (IsPrint == "0" || IsPrint == "否")
                                        {
                                            IsPrint = "0";
                                        }
                                        else
                                        {
                                            IsPrint = "1";
                                            #region  判断打印工位编号是否正确
                                            if (RsvFld1 != "" && RsvFld1 != null)
                                            {
                                                bool ifExistWc = BBdbR_WcBaseList0.Exists(t => t.WcCd == RsvFld1);
                                                if (ifExistWc == false)
                                                {
                                                    dr = Newdt.NewRow();
                                                    dr[0] = errorNum;
                                                    dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行";
                                                    dr[2] = "打印工位编号不在系统中";
                                                    Newdt.Rows.Add(dr);
                                                    errorNum++;
                                                    IsCheck = 0;
                                                    continue;
                                                }
                                                else { }
                                            }
                                            else
                                            {
                                                RsvFld1 = "";
                                            }
                                            #endregion
                                        }
                                    }
                                    else
                                    {
                                        dr = Newdt.NewRow();
                                        dr[0] = errorNum;
                                        dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行";
                                        dr[2] = "是否打印输入字符不规范，请输入0或1";
                                        Newdt.Rows.Add(dr);
                                        errorNum++;
                                        IsCheck = 0;
                                        continue;
                                    }


                                }
                                else
                                {
                                    IsPrint = "";
                                }
                                #endregion
                            }



                            #region 判断物料编号是否存在,存在则添加到list中，批量更新
                            bool ifExist = BBdbR_MatBaseList0.Exists(t => t.MatCd == MatCd);
                            if (ifExist==true)
                            {
                                BBdbR_MatBase thisMatBaseRecord = BBdbR_MatBaseList0.Find(t => t.MatCd == dt.Rows[i]["物料编号"].ToString().Trim());
                                #region 获取表中此行所有内容并加到BBdbR_MatBaseList1表中 
                                
                                thisMatBaseRecord.ShortCode = ShortCode;

                                #region 判断该条数据关重件信息和打印工位信息是否有效-不为空则有效
                                if (IsScan != ""&& IsScan !="0")
                                {
                                    thisMatBaseRecord.IsScan = IsScan;
                                    thisMatBaseRecord.WcCd = WcCd;
                                    thisMatBaseRecord.WcId = WcId;
                                    thisMatBaseRecord.WcNm = WcNm;
                                }
                                else { }
                                if (IsPrint != "" && IsPrint != "0")
                                {
                                    thisMatBaseRecord.IsPrint = IsPrint;
                                    thisMatBaseRecord.RsvFld1 = RsvFld1;
                                }
                                else { }
                                #endregion
                                BBdbR_MatBaseList1.Add(thisMatBaseRecord);
                                #endregion
                            }
                            else
                            {
                                dr = Newdt.NewRow();
                                dr[0] = errorNum;
                                dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行";
                                dr[2] = "物料编号不存在";
                                Newdt.Rows.Add(dr);
                                errorNum++;
                                IsCheck = 0;
                                continue;
                            }
                            #endregion
                            
                        }
                        else
                        {
                            dr = Newdt.NewRow();
                            dr[0] = errorNum;
                            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行";
                            dr[2] = "物料编号不能为空";
                            Newdt.Rows.Add(dr);
                            errorNum++;
                            IsCheck = 0;
                            continue;
                        }
                        
                    }
                    #endregion
                    
                    #region 向表中更新数据
                    //结果表
                    int b0 = database.Update(BBdbR_MatBaseList1);
                    if (b0 > 0)
                    {
                        IsOk = IsOk + b0;
                        BBdbR_MatBaseList1.Clear();
                    }
                    else
                    {

                    }
                    #endregion

                    Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "物料配置信息导入成功");

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
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "物料配置信息导入操作失败：" + ex.Message);
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

        

        #region form页面加载工位

        public ActionResult FindWc(string WcCd)
        {
            try
            {
                #region datatable查询并传回前端
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select * from BBdbR_WcBase where WcCd like '%" + WcCd + "%' and WcTyp = '一般' and Enabled =1 order by WcNm");
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                if (dt.Rows.Count > 0)
                {
                    var JsonData = new
                    {
                        rows = dt,
                    };
                    return Content(JsonData.ToJson());
                }
                return null;
                #endregion


            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region 判断关重件工位编号和打印工位编号是否存在，传回form页面
        //如果存在返回关重件工位名称
        public ActionResult IfexistWcCd(string WcCd)
        {
            try
            {
                #region datatable查询并传回前端
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select * from BBdbR_WcBase where WcCd = '" + WcCd + "'  and Enabled =1");
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                string WcNm = "";
                if (dt.Rows.Count > 0)
                {
                    WcNm = dt.Rows[0]["WcNm"].ToString();
                    return Content(dt.ToJson());
                }
                return Content("\"" + "unfind" + "\"");
                #endregion


            }
            catch (Exception ex)
            {
                return Content("\"" + "unfind" + "\"");
            }
        }
        #endregion



    }
}