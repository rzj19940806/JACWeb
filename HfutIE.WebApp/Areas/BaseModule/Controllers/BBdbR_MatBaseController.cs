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

        #region 创建数据库操作对象区域
        BBdbR_MatBaseBll MyBll = new BBdbR_MatBaseBll(); //===复制时需要修改===
        BBdbR_MatFileConfigBll MyBll2 = new BBdbR_MatFileConfigBll();
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
                List<BBdbR_MatBase> ListData = MyBll.GetPlanList("");
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

        #endregion

        #region 6.查询方法
        //查询方法，本方法为单条件查询，即根据一个条件进行查询
        //查询条件为Condition，也是数据库表_CompanyBaseInformation中的一个字段名
        //查询值为keywords，也是数据库表_CompanyBaseInformation中的字段名的字段值
        //本查询采用近似查询（like）

        public ActionResult GridPageByCondition(string keywords, string Condition, JqGridParam jqgridparam)
        {
            try
            {
                string keyword = keywords.Trim();
                Stopwatch watch = CommonHelper.TimerStart();
                List<BBdbR_MatBase> ListData = MyBll.GetPageListByCondition(keyword, Condition, jqgridparam);//===复制时需要修改===
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
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
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
                int IsOk = 0;//用于判断
                string Name = "MatCd";
                string Value = entity.MatCd;  //页面中的编号字段值                 //===复制时需要修改===
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))//编辑操作
                {
                    //===复制时需要修改===
                    entity.MatId = KeyValue;
                    entity.Modify(KeyValue);//调用扩展编辑方法
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
                    var files = System.Web.HttpContext.Current.Request.Files;
                    var file1 = files[0].InputStream;
                    byte[] buff = new byte[file1.Length];
                    int a = MyBll.Insert(entity);//将实体插入数据库，插入成功返回1，失败返回0；
                    if (a > 0)
                    {
                        byte[] photograph = null;
                        string photograph_type = "";
                        photograph = GetPhotographFromRequest(out photograph_type);//获取图片的二进制和图片后缀名，方法在下面
                        NameValueCollection forms = Request.Form;
                        IsOk = MyBll.InsertPicture(entity.MatCd, photograph, photograph_type);//将实体插入数据库，插入成功返回1，失败返回0；
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
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
               
            }
            catch (Exception ex)
            {
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

       
        #region 10.导入
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
            //ViewBag.ID = Request.Params["ID"];
            //ID1 = ViewBag.ID;
            //ViewBag.OrderID = Request.Params["OrderID"];
            //OrderID1 = ViewBag.OrderID;
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
            List<BBdbR_MatBase> DeviceList = new List<BBdbR_MatBase>();

            //构造导入返回结果表
            DataTable Newdt = new DataTable("Result");
            Newdt.Columns.Add("rowid", typeof(System.String));                 //行号
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
                    #region 物料基础信息导入
                    //校验
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        IsCheck = 1;//重置标识
                        DataRow dr = Newdt.NewRow();
                        string Enabled = "";
                        string MatCatg = "";
                        string MatTyp = "";
                        
                        //switch (dt.Rows[i]["有效性"].ToString())
                        //{
                        //    case "有效":
                        //        Enabled = "1"; break;
                        //    case "无效":
                        //        Enabled = "0"; break;
                        //    default:
                        //        dr = Newdt.NewRow();
                        //        dr[0] = errorNum;
                        //        dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[有效性]";
                        //        dr[2] = "数字格式不正确请重新输入";
                        //        Newdt.Rows.Add(dr);
                        //        errorNum++;
                        //        IsCheck = 0;
                        //        break;
                        //}
                        switch (dt.Rows[i]["物料类别"].ToString())
                        {
                            case "产品":
                                MatCatg = "产品"; break;
                            case "零部件":
                                MatCatg = "零部件"; break;
                            default:
                                dr = Newdt.NewRow();
                                dr[0] = errorNum;
                                dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[物料类别]";
                                dr[2] = "数字格式不正确请重新输入";
                                Newdt.Rows.Add(dr);
                                errorNum++;
                                IsCheck = 0;
                                break;
                        }
                        switch (dt.Rows[i]["物料类型"].ToString())
                        {
                            case "正常车身":
                                MatTyp = "正常车身"; break;
                            case "试制车":
                                MatTyp = "试制车"; break;
                            case "备件车":
                                MatTyp = "备件车"; break;
                            default:
                                dr = Newdt.NewRow();
                                dr[0] = errorNum;
                                dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[物料类型]";
                                dr[2] = "数字格式不正确请重新输入";
                                Newdt.Rows.Add(dr);
                                errorNum++;
                                IsCheck = 0;
                                break;
                        }
                       
                        if (dt.Rows[i]["物料编号"].ToString().Trim() != "")
                        {
                            int DeviceCount = MyBll.CheckCount("MatCd", dt.Rows[i]["物料编号"].ToString());//是否有相同设备编码
                            if (DeviceCount > 0)
                            {
                                dr = Newdt.NewRow();
                                dr[0] = errorNum;
                                dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[物料编号]";
                                dr[2] = "在系统中已存在不能重复插入";
                                Newdt.Rows.Add(dr);
                                errorNum++;
                                IsCheck = 0;
                                continue;
                            }
                            else
                            {
                                BBdbR_MatBase Device = new BBdbR_MatBase();
                                Device.MatId = System.Guid.NewGuid().ToString();
                                Device.MatCd = dt.Rows[i]["物料编号"].ToString().Trim();
                                Device.MatNm = dt.Rows[i]["物料名称"].ToString().Trim();
                                Device.MatCatg = MatCatg;
                                Device.MatTyp = MatTyp;
                                Device.MatSpec = dt.Rows[i]["规格型号"].ToString().Trim();
                                //Device.MatImg = dt.Rows[i]["默认图片"].ToString().Trim();
                                Device.Enabled = "1";
                               
                                DeviceList.Add(Device);
                                int b = database.Insert(DeviceList);
                                if (b > 0)
                                {
                                    IsOk = IsOk + b;
                                    DeviceList.Clear();
                                }
                                else
                                {
                                    dr = Newdt.NewRow();
                                    dr[0] = errorNum;
                                    dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行";
                                    dr[2] = "物料基础信息插入失败";
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
                            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行";
                            dr[2] = "物料编号不能为空";
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
                Base_SysLogBll.Instance.WriteLog("", OperationType.Add, "-1", "异常错误：" + ex.Message);
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
       




    }
}