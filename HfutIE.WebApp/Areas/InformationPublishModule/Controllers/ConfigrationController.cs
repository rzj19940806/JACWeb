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
using System.Web.Script.Serialization;

namespace HfutIE.WebApp.Areas.InformationPublishModule.Controllers
{
    /// <summary>
    /// 信息推送对象配置表控制器
    /// </summary>
    public class ConfigrationController : PublicController<BBdbR_PushObjectConf>
    {
        #region
        #endregion

        #region 全局变量定义区
        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BBdbR_PushObjectConf";//===复制时需要修改===
        public readonly RepositoryFactory<BBdbR_PushInfor> repositoryfactory_pushinfor = new RepositoryFactory<BBdbR_PushInfor>();
        public readonly RepositoryFactory<BBdbR_StfBase> repositoryfactory_StfBase = new RepositoryFactory<BBdbR_StfBase>();
        public readonly RepositoryFactory<BBdbR_PushObjectConf> repositoryfactory_PushObConf = new RepositoryFactory<BBdbR_PushObjectConf>();
        public readonly RepositoryFactory<Base_Roles3> repositoryfactory_RoleObConf = new RepositoryFactory<Base_Roles3>();
        public readonly RepositoryFactory<BBdbR_GroupChat> repositoryfactory_GroupObConf = new RepositoryFactory<BBdbR_GroupChat>();
        #endregion

        #region 打开视图区域
        public ActionResult UserForm()//打开推送信息用户配置页面
        {
            return View();
        }
        public ActionResult AddInfor()//推送信息
        {
            return View();
        }
        public ActionResult GroupForm()//打开推送信息群聊配置页面
        {
            return View();
        }
        public ActionResult RoleForm()//打开推送信息角色配置页面
        {
            return View();
        }
        #endregion

        #region 创建数据库操作对象区域
        //创建BBdbR_PushObjectConfBll对象，用以访问BBdbR_PushObjectConfBll中操作数据库的方法
        BBdbR_PushObjectConfBll MyBll = new BBdbR_PushObjectConfBll();
        BBdbR_PushInforBll PushInforBll = new BBdbR_PushInforBll();
        BBdbR_StfBaseBll StfbaseBll = new BBdbR_StfBaseBll();
        Base_Roles3Bll RolesBll = new Base_Roles3Bll();
        BBdbR_GroupChatBll GroupBll = new BBdbR_GroupChatBll();
        PushObject_StfBase_VBll Object_StfBase_VBll = new PushObject_StfBase_VBll();
        PushObject_BaseRoles_VBll Object_BaseRoles_VBll = new PushObject_BaseRoles_VBll();
        PushObject_Groupchat_VBll Object_Groupchat_VBll = new PushObject_Groupchat_VBll();
        #endregion

        #region 方法区 

        #region 1.推送信息树
        public ActionResult TreeJson()
        {
            try
            {
                DataTable dt = PushInforBll.GetTree();
                List<TreeJsonEntity> TreeList = new List<TreeJsonEntity>();
                if (DataHelper.IsExistRows(dt))
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string ID = row["ID"].ToString();
                        bool hasChildren = false;
                        DataTable childnode = DataHelper.GetNewDataTable(dt, "parentid='" + ID + "'");
                        if (childnode.Rows.Count > 0)
                        {
                            hasChildren = true;
                        }
                        TreeJsonEntity tree = new TreeJsonEntity();
                        tree.id = ID;
                        tree.text = row["name"].ToString();
                        tree.parentId = row["parentid"].ToString();
                        tree.Attribute = "Type";
                        tree.AttributeValue = row["sort"].ToString();
                        tree.isexpand = true;
                        tree.complete = true;
                        tree.hasChildren = hasChildren;
                        tree.img = "/Content/Images/Icon16/group.png";

                        TreeList.Add(tree);
                    }
                }
                return Content(TreeList.TreeToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 2.根据树节点加载推送信息表
        public ActionResult GridTreepush(string InforTypID) //根据树节点加载表格
        {
            try
            {
                DataTable ListData = PushInforBll.GetInforList(InforTypID);
                var JsonData = new
                {                                     
                    rows = ListData,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 3.获取用户配置
        public ActionResult Getpushuserinfor(string selectTycd) //根据树节点加载表格
        {
            try
            {
                DataTable ListData = Object_StfBase_VBll.GetConfigUserList(selectTycd);
                var JsonData = new
                {
                    rows = ListData,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 3.获取角色配置
        public ActionResult Getpushroleinfor(string selectTycd) //根据树节点加载表格
        {
            try
            {
                DataTable ListData = Object_BaseRoles_VBll.GetConfigRoleList(selectTycd);
                var JsonData = new
                {
                    rows = ListData,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 3.获取群聊配置
        public ActionResult Getpushgroupinfor(string selectTycd) //根据树节点加载表格
        {
            try
            {
                DataTable ListData = Object_Groupchat_VBll.GetConfigGroupList(selectTycd);
                var JsonData = new
                {
                    rows = ListData,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 加载推送信息
        public ActionResult GridPushPageJson() //加载推送信息
        {
            try
            {
                DataTable ListData = PushInforBll.GetInforPage();
                var JsonData = new
                {
                    rows = ListData,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 4.编辑推送信息
        public ActionResult SetPushForm(string keyvalue) //主键
        {
            try
            {
                DataTable ListData = PushInforBll.SetPushinfor(keyvalue);
                var JsonData = new
                {
                    rows = ListData,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 5.保存推送信息
        public ActionResult SubmitPushInfor(string RecId,string InforTyp,string InforTypID, BBdbR_PushInfor entity)
        {
            try
            {
                string Message = InforTyp == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(RecId))
                {
                    repositoryfactory_pushinfor.Repository().Update(entity);
                }
                else
                {
                    entity.RecId = Guid.NewGuid().ToString();
                    entity.Enabled = "1";
                    entity.InforTypCd = InforTypID;
                    entity.InforTyp = InforTyp;
                    entity.CreTm = DateTime.Now;
                    repositoryfactory_pushinfor.Repository().Insert(entity);
                }
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());                            
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region Index界面搜索
        public ActionResult GridPushInfor(string condition, string keywords,  string ParameterJson, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = PushInforBll.SelectPushIn(condition, keywords,  ParameterJson, jqgridparam);
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
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        public ActionResult GridUserConfig(string condition, string keywords, string ParameterJson, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = Object_StfBase_VBll.SelectUserConIn(condition, keywords,  ParameterJson, jqgridparam);
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
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        public ActionResult GridRoleConfig(string condition, string keywords, string ParameterJson, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = Object_BaseRoles_VBll.SelectRoleConIn(condition, keywords, ParameterJson, jqgridparam);
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
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        public ActionResult GridGroupConfig(string condition, string keywords, string ParameterJson, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = Object_Groupchat_VBll.SelectGroupConIn(condition, keywords, ParameterJson, jqgridparam);
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
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 删除推送信息
        /// <summary>
        ///移除推送信息
        /// </summary>
        /// <param name="KeyValueuser"></param>
        /// <returns></returns>
        public ActionResult DeleteInfor(string KeyValueuser)
        {
            string[] array = KeyValueuser.Split(',');
            try
            {
                var Message = "删除失败。";
                //直接删除
                if (array != null && array.Length > 0)
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        repositoryfactory_pushinfor.Repository().Delete(array[i]);
                    }
                    Message = "删除成功。";
                }
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 获取未配置用户信息
        public ActionResult GridNoConList(string Tycd, string ParameterJson, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = StfbaseBll.GetNoConList(Tycd, ParameterJson, jqgridparam);
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
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 获取已配置用户
        public ActionResult GridConUserList(string Tycd, string Rank, string ParameterJson, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = StfbaseBll.GetConUserList(Tycd, Rank, ParameterJson, jqgridparam);
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
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 查询未配置用户信息       
        public ActionResult GridAllUserList(string condition, string keywords, string Tycd, string ParameterJson, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = StfbaseBll.GetAllUserList(condition, keywords, Tycd, ParameterJson, jqgridparam);
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
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 提交配置用户
        public ActionResult SubmitUserCon(string Tycd, string Type, string Rank,decimal Time,string Unit,string conuser)
        {
            int IsOk = 0;
            try
            {
                string Message = "配置失败";
                //删除之前的配置
                int deleteisok = 0;
                deleteisok = StfbaseBll.DeleteConUser(Tycd, Type, Rank);
                JavaScriptSerializer js = new JavaScriptSerializer();
                List<BBdbR_StfBase> conuserlist = new List<BBdbR_StfBase>();
                List<BBdbR_PushObjectConf> list = new List<BBdbR_PushObjectConf>();
                if (conuser != "" && conuser != null )
                {
                    conuserlist = js.Deserialize<List<BBdbR_StfBase>>(conuser);//反序列化
                    foreach (var item in conuserlist)
                    {
                        BBdbR_PushObjectConf mail_Push = new BBdbR_PushObjectConf();
                        mail_Push.RecId = Guid.NewGuid().ToString();
                        mail_Push.ObjectTyp = "1";//1:用户,2:角色,3:群聊
                        mail_Push.ObjectId = item.StfId;//人员主键
                        mail_Push.InforTypCd = Tycd;
                        mail_Push.InforTyp = Type;
                        mail_Push.PushRank = Rank;
                        mail_Push.IntvlTm = Time;
                        mail_Push.TmUnit = Unit;
                        mail_Push.Enabled = "1";
                        mail_Push.CreTm = DateTime.Now;
                        list.Add(mail_Push);
                    }
                    IsOk = repositoryfactory_PushObConf.Repository().Insert(list);
                }
                if (IsOk > 0)
                {
                    Message = "配置成功";
                }
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());

            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }

        }
        #endregion

        #region 获取未配置角色信息
        public ActionResult GridNoRoleConList(string Tycd, string ParameterJson, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = RolesBll.GetNoConList(Tycd, ParameterJson, jqgridparam);
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
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 查询未配置角色信息       
        public ActionResult GridNoConRoleList(string condition, string keywords, string Tycd, string ParameterJson, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = RolesBll.GetNoConRoleList(condition, keywords, Tycd, ParameterJson, jqgridparam);
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
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 获取已配置角色
        public ActionResult GridRoleConList(string Tycd, string Rank, string ParameterJson, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = RolesBll.GetConRoleList(Tycd, Rank, ParameterJson, jqgridparam);
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
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 提交配置角色
        public ActionResult SubmitRoleCon(string Tycd, string Type, string Rank, decimal Time, string Unit, string conuser)
        {
            int IsOk = 0;
            try
            {
                string Message = "配置失败";
                //删除之前的配置
                int deleteisok = 0;
                deleteisok = RolesBll.DeleteConRole(Tycd, Type, Rank);
                JavaScriptSerializer js = new JavaScriptSerializer();
                List<Base_Roles> conuserlist = new List<Base_Roles>();
                List<BBdbR_PushObjectConf> list = new List<BBdbR_PushObjectConf>();
                if (conuser != "" && conuser != null)
                {
                    conuserlist = js.Deserialize<List<Base_Roles>>(conuser);//反序列化
                    foreach (var item in conuserlist)
                    {
                        BBdbR_PushObjectConf mail_Push = new BBdbR_PushObjectConf();
                        mail_Push.RecId = Guid.NewGuid().ToString();
                        mail_Push.ObjectTyp = "2";//1:用户,2:角色,3:群聊
                        mail_Push.ObjectId = item.RoleId;//角色主键
                        mail_Push.InforTypCd = Tycd;
                        mail_Push.InforTyp = Type;
                        mail_Push.PushRank = Rank;
                        mail_Push.IntvlTm = Time;
                        mail_Push.TmUnit = Unit;
                        mail_Push.Enabled = "1";
                        mail_Push.CreTm = DateTime.Now;
                        list.Add(mail_Push);
                    }
                    IsOk = repositoryfactory_PushObConf.Repository().Insert(list);
                }
                if (IsOk > 0)
                {
                    Message = "配置成功";
                }
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());

            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }

        }
        #endregion

        #region 获取未配置群聊信息
        public ActionResult GridNoGroupConList(string Tycd, string ParameterJson, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = GroupBll.GetNoConList(Tycd, ParameterJson, jqgridparam);
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
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 查询未配置群聊信息       
        public ActionResult GridNoCGroupList(string condition, string keywords, string Tycd, string ParameterJson, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = GroupBll.GetNoConGroupList(condition, keywords, Tycd, ParameterJson, jqgridparam);
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
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 获取已配置群聊
        public ActionResult GridConGroupList(string Tycd, string Rank, string ParameterJson, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = GroupBll.GetConGroupList(Tycd, Rank, ParameterJson, jqgridparam);
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
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 提交配置群聊
        public ActionResult SubmitGroupCon(string Tycd, string Type, string Rank, decimal Time, string Unit, string conuser)
        {
            int IsOk = 0;
            try
            {
                string Message = "配置失败";
                //删除之前的配置
                int deleteisok = 0;
                deleteisok = GroupBll.DeleteConGroup(Tycd, Type, Rank);
                JavaScriptSerializer js = new JavaScriptSerializer();
                List<BBdbR_GroupChat> conuserlist = new List<BBdbR_GroupChat>();
                List<BBdbR_PushObjectConf> list = new List<BBdbR_PushObjectConf>();
                if (conuser != "" && conuser != null)
                {
                    conuserlist = js.Deserialize<List<BBdbR_GroupChat>>(conuser);//反序列化
                    foreach (var item in conuserlist)
                    {
                        BBdbR_PushObjectConf mail_Push = new BBdbR_PushObjectConf();
                        mail_Push.RecId = Guid.NewGuid().ToString();
                        mail_Push.ObjectTyp = "3";//1:用户,2:角色,3:群聊
                        mail_Push.ObjectId = item.GroupchatId;//群聊主键
                        mail_Push.InforTypCd = Tycd;
                        mail_Push.InforTyp = Type;
                        mail_Push.PushRank = Rank;
                        mail_Push.IntvlTm = Time;
                        mail_Push.TmUnit = Unit;
                        mail_Push.Enabled = "1";
                        mail_Push.CreTm = DateTime.Now;
                        list.Add(mail_Push);
                    }
                    IsOk = repositoryfactory_PushObConf.Repository().Insert(list);
                }
                if (IsOk > 0)
                {
                    Message = "配置成功";
                }
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());

            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }

        }
        #endregion

        #region 删除配置
        /// <summary>
        ///移除推送信息
        /// </summary>
        /// <param name="KeyValueuser"></param>
        /// <returns></returns>
        public ActionResult Deleteconfig1(string KeyValuetable1)
        {
            string[] array = KeyValuetable1.Split(',');
            try
            {
                var Message = "删除失败。";
                //直接删除
                if (array != null && array.Length > 0)
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        repositoryfactory_PushObConf.Repository().Delete(array[i]);
                    }
                    Message = "删除成功。";
                }
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        public ActionResult Deleteconfig2(string KeyValuetable2)
        {
            string[] array = KeyValuetable2.Split(',');
            try
            {
                var Message = "删除失败。";
                //直接删除
                if (array != null && array.Length > 0)
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        repositoryfactory_PushObConf.Repository().Delete(array[i]);
                    }
                    Message = "删除成功。";
                }
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        public ActionResult Deleteconfig3(string KeyValuetable3)
        {
            string[] array = KeyValuetable3.Split(',');
            try
            {
                var Message = "删除失败。";
                //直接删除
                if (array != null && array.Length > 0)
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        repositoryfactory_PushObConf.Repository().Delete(array[i]);
                    }
                    Message = "删除成功。";
                }
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #endregion
    }
}