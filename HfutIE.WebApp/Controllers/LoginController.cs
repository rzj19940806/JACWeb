using HfutIE.Business;
using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HfutIE.WebApp.Controllers
{
    /// <summary>
    /// 登录控制器
    /// </summary>
    public class LoginController : Controller
    {
        public string _P_account;
        public static string P_account1;
        public string P_account
        {
            get
            {
                _P_account = P_account1;
                return _P_account;
            }
        }
        public string _P_user_name;
        public string P_user_name
        {
            get
            {
                _P_account = user_name;
                return _P_account;
            }
        }
        /// <summary>
        /// 调试日志
        /// </summary>
        public static HfutIE.Utilities.LogHelper log = HfutIE.Utilities.LogFactory.GetLogger("LoginController");
        public static string user;//获取登录账号，新增时自动录入
        public static string user_name;//获取登录者的姓名
        Base_UserBll base_userbll = new Base_UserBll();
        Base_ObjectUserRelationBll base_objectuserrelationbll = new Base_ObjectUserRelationBll();
        /// <summary>
        /// 默认页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Default()
        {
            return View();
        }
        /// <summary>
        /// 登录视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //addImport("Base_DataScopePermission");
            return View();
        }
        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult VerifyCode()
        {
            int codeW = 80;
            int codeH = 22;
            int fontSize = 16;
            string chkCode = string.Empty;
            //颜色列表，用于验证码、噪线、噪点 
            Color[] color = { Color.Black, Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown, Color.Brown, Color.DarkBlue };
            //字体列表，用于验证码 
            string[] font = { "Times New Roman", "Verdana", "Arial", "Gungsuh", "Impact" };
            //验证码的字符集，去掉了一些容易混淆的字符 
            char[] character = { '2', '3', '4', '5', '6', '8', '9', 'a', 'b', 'd', 'e', 'f', 'h', 'k', 'm', 'n', 'r', 'x', 'y', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'R', 'S', 'T', 'W', 'X', 'Y' };
            Random rnd = new Random();
            //生成验证码字符串 
            for (int i = 0; i < 4; i++)
            {
                chkCode += character[rnd.Next(character.Length)];
            }
            //写入Session、验证码加密
            Session["session_verifycode"] = Md5Helper.MD5(chkCode.ToLower(), 16);
            //创建画布
            Bitmap bmp = new Bitmap(codeW, codeH);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            //画噪线 
            for (int i = 0; i < 1; i++)
            {
                int x1 = rnd.Next(codeW);
                int y1 = rnd.Next(codeH);
                int x2 = rnd.Next(codeW);
                int y2 = rnd.Next(codeH);
                Color clr = color[rnd.Next(color.Length)];
                g.DrawLine(new Pen(clr), x1, y1, x2, y2);
            }
            //画验证码字符串 
            for (int i = 0; i < chkCode.Length; i++)
            {
                string fnt = font[rnd.Next(font.Length)];
                Font ft = new Font(fnt, fontSize);
                Color clr = color[rnd.Next(color.Length)];
                g.DrawString(chkCode[i].ToString(), ft, new SolidBrush(clr), (float)i * 18 + 2, (float)0);
            }
            //将验证码图片写入内存流，并将其以 "image/Png" 格式输出 
            MemoryStream ms = new MemoryStream();
            try
            {
                bmp.Save(ms, ImageFormat.Png);
                return File(ms.ToArray(), @"image/Gif");
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                g.Dispose();
                bmp.Dispose();
            }
        }

        ///// <summary>
        ///// 获取本机IP地址
        ///// </summary>
        ///// <returns></returns>
        //public static string GetLocalIpAddress()
        //{
        //    string hostinfo = System.Net.Dns.GetHostName();
        //    System.Net.IPAddress addr = new System.Net.IPAddress(System.Net.Dns.GetHostByName(hostinfo).AddressList[0].Address);
        //    return addr.ToString();
        //}

        /// <summary>
        /// 获取本机IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIpAddress()
        {
            string hostinfo = System.Net.Dns.GetHostName();
            System.Net.IPAddress addr = Dns.GetHostEntry(hostinfo).AddressList.Last();
            return addr.ToString();
        }
        public ActionResult GetIpAddress()
        {
            string IPAddress = NetHelper.GetIPAddress();
            //IPAddress = "10.138.13.48";
            return Content(IPAddress);
        }
        #region 通过设备IP获取设备类型
        public ActionResult GetDvcTypeByIP()
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                DataTable dt = new DataTable();
                string IPAddress = NetHelper.GetIPAddress();
                //IPAddress = "10.138.13.48";
                strSql.Append(@"select DvcTyp from BBdbR_DvcBase where IPAddr = '" + IPAddress + "' and Enabled =1");
                dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                if (dt.Rows.Count == 1)
                {
                    string DvcTyp = dt.Rows[0]["DvcTyp"].ToString();
                    return Content(DvcTyp);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return Content("error");
            }
        }
        #endregion
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="Account">账户</param>
        /// <param name="Password">密码</param>
        /// <returns></returns>
        public ActionResult CheckLogin(string Account, string Password, string Token)
        {
            string maccode = "";
            string name = "";
            P_account1 = Account;
            string Msg = "";
            try
            {
                //AD_MAC_INFOBll macbll = new AD_MAC_INFOBll();
                //maccode = getMac();//MAC码
                //maccode=maccode.Replace(":","-").ToString();
                //DataTable dt_mac = macbll.Getdt_mac(maccode);
                //if (dt_mac == null || dt_mac.Rows.Count == 0)
                //{
                //    Msg = "5";//未配置该电脑的MAC码
                //}
                //if (dt_mac != null && dt_mac.Rows.Count > 0)
                //{
                //string enable = dt_mac.Rows[0]["enable"].ToString();
                //if (enable == "0")
                //{
                //    Msg = "6";//该电脑的MAC码无权限登录
                //}
                //else
                //{
                IPScanerHelper objScan = new IPScanerHelper();
                string IPAddress = NetHelper.GetIPAddress();
                objScan.IP = IPAddress;
                objScan.DataPath = Server.MapPath("~/Resource/IPScaner/QQWry.Dat");
                string IPAddressName = objScan.IPLocation();
                string outmsg = "";
                VerifyIPAddress(Account, IPAddress, IPAddressName, Token);
                //系统管理
                if (Account == ConfigHelper.AppSettings("CurrentUserName"))
                {
                    if (ConfigHelper.AppSettings("CurrentPassword") == Password)
                    {
                        IManageUser imanageuser = new IManageUser();
                        imanageuser.UserId = "System";
                        user = "System";
                        imanageuser.Account = "System";
                        imanageuser.UserName = "超级管理员";
                        imanageuser.Gender = "男";
                        imanageuser.Code = "System";
                        imanageuser.LogTime = DateTime.Now;
                        imanageuser.CompanyId = "系统";
                        imanageuser.DepartmentId = "系统";
                        imanageuser.IPAddress = IPAddress;
                        imanageuser.IPAddressName = IPAddressName;
                        imanageuser.IsSystem = true;
                        ManageProvider.Provider.AddCurrent(imanageuser);
                        //对在线人数全局变量进行加1处理
                        HttpContext rq = System.Web.HttpContext.Current;
                        rq.Application["OnLineCount"] = (int)rq.Application["OnLineCount"] + 1;
                        Msg = "3";//验证成功
                        Base_SysLogBll.Instance.WriteLog(Account, OperationType.Login, "1", "登陆成功、IP所在城市：" + IPAddressName);
                    }
                    else
                    {
                        return Content("4");
                    }
                }
                else
                {
                    Base_User base_user = base_userbll.UserLogin(Account, Password, out outmsg);
                    switch (outmsg)
                    {
                        case "-1":      //账户不存在
                            Msg = "-1";
                            Base_SysLogBll.Instance.WriteLog(Account, OperationType.Login, "-1", "账户不存在、IP所在城市：" + IPAddressName);
                            break;
                        case "lock":    //账户锁定
                            Msg = "2";
                            Base_SysLogBll.Instance.WriteLog(Account, OperationType.Login, "-1", "账户锁定、IP所在城市：" + IPAddressName);
                            break;
                        case "error":   //密码错误
                            Msg = "4";
                            Base_SysLogBll.Instance.WriteLog(Account, OperationType.Login, "-1", "密码错误、IP所在城市：" + IPAddressName);
                            break;
                        case "succeed": //验证成功
                            
                            

                            IManageUser imanageuser = new IManageUser();
                            user = base_user.Account;//系统操作室自动获取用户名
                            imanageuser.UserId = base_user.UserId;
                            imanageuser.Account = base_user.Account;
                            
                            imanageuser.UserName = base_user.RealName;
                            imanageuser.Gender = base_user.Gender;
                            imanageuser.Password = base_user.Password;
                            imanageuser.Code = base_user.Code;
                            imanageuser.Secretkey = base_user.Secretkey;
                            imanageuser.LogTime = DateTime.Now;
                            imanageuser.CompanyId = base_user.CompanyId;
                            //imanageuser.CompanyName = DataFactory.Database().FindEntity<BBdbR_CompanyBase>(base_user.CompanyId)?.CompanyNm;
                            imanageuser.DepartmentId = base_user.DepartmentId;
                            //imanageuser.DepartmentName = DataFactory.Database().FindEntity<Base_Department>(base_user.DepartmentId).FullName;

                            //imanageuser.UserName = base_user.RealName;
                            //user_name = base_user.RealName;
                            //imanageuser.Gender = base_user.Gender;
                            //imanageuser.Password = base_user.Password;
                            //imanageuser.Code = base_user.Code;
                            //imanageuser.Secretkey = base_user.Secretkey;
                            //imanageuser.LogTime = DateTime.Now;
                            //imanageuser.CompanyId = base_user.CompanyId;
                            //imanageuser.CompanyName = DataFactory.Database().FindEntity<BBdbR_CompanyBase>(base_user.CompanyId)?.CompanyNm;
                            //imanageuser.DepartmentId = base_user.DepartmentId;
                            //imanageuser.DepartmentName = DataFactory.Database().FindEntity<Base_Department>(base_user.DepartmentId).FullName;
                            imanageuser.ObjectId = base_objectuserrelationbll.GetObjectId(imanageuser.UserId);
                            imanageuser.IPAddress = IPAddress;
                            imanageuser.IPAddressName = IPAddressName;
                            imanageuser.IsSystem = false;
                            imanageuser.PwdRank = base_user.PwdRank;//2021.12.20新增
                            ManageProvider.Provider.AddCurrent(imanageuser);

                            #region 判断是否需要修改密码
                            if (Password == "4a7d1ed414474e4033ac29ccb8653d9b" || base_user.LastPwdModfyTm == null)//密码为默认密码0000/从未修改过密码
                            {
                                Msg = "7";
                                break;
                            }
                            else if (base_user.LastPwdModfyTm != null)
                            {
                                var span = (DateTime.Now - DateTime.Parse(base_user.LastPwdModfyTm.ToString())).TotalDays;
                                if (span >= 60)//距离上次修改密码时间超过两个月
                                {
                                    Msg = "7";
                                    break;
                                }
                            }
                            #endregion

                            //对在线人数全局变量进行加1处理
                            HttpContext rq = System.Web.HttpContext.Current;
                            rq.Application["OnLineCount"] = (int)rq.Application["OnLineCount"] + 1;
                            Msg = "3";//验证成功
                            Base_SysLogBll.Instance.WriteLog(Account, OperationType.Login, "1", "登陆成功、IP所在城市：" + IPAddressName);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Msg = ex.Message;
            }
            return Content(Msg);
        }

        /// <summary>
        /// 获取MAC地址
        /// </summary>
        public static string getMac()
        {
            try
            {
                //获取网卡硬件地址 
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        mac = mo["MacAddress"].ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                return mac;
            }
            catch
            {
                return "no";
            }
            finally
            {
            }
        }


        /// <summary>
        /// 验证强迫退出 下线
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckForceOutLogin()
        {
            return null;
        }
        /// <summary>
        /// 同账号不允许重复登录
        /// </summary>
        /// <param name="Account">账户</param>
        /// <returns></returns>
        public bool CheckOnLine(string Account)
        {
            if (!CommonHelper.GetBool(ConfigHelper.AppSettings("CheckOnLine")))
            {
                return true;
            }
            ArrayList list = Session["OnLineList"] as ArrayList;
            if (list == null)
            {
                list = new ArrayList();
            }
            for (int i = 0; i < list.Count; i++)
            {
                if (Account == (list[i] as string))
                {
                    //已经登录了，提示错误信息 
                    return false;
                }
            }
            Session.Add("OnLineList", list.Add(Account));
            return true;
        }
        /// <summary>
        /// 安全退出
        /// </summary>
        /// <returns></returns>
        public ActionResult OutLogin()
        {
            string UserId = ManageProvider.Provider.Current().UserId;
            //更改数据库用户表在线状态
            Base_User entity = new Base_User();
            entity.UserId = UserId; entity.Online = 0;
            base_userbll.Repository().Update(entity);
            //清空当前登录用户信息
            ManageProvider.Provider.EmptyCurrent();
            Session.Abandon();  //取消当前会话
            Session.Clear();    //清除当前浏览器所有Session
            return Content("1");
        }
        /// <summary>
        /// IP限制验证
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="IPAddress"></param>
        /// <param name="IPAddressName"></param>
        /// <param name="OpenId"></param>
        public void VerifyIPAddress(string Account, string IPAddress, string IPAddressName, string OpenId)
        {
            if (ConfigHelper.AppSettings("VerifyIPAddress") == "true")
            {
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@IPAddress", IPAddress));
                parameter.Add(DbFactory.CreateDbParameter("@IPAddressName", IPAddressName));
                parameter.Add(DbFactory.CreateDbParameter("@OpenId", DESEncrypt.Decrypt(OpenId)));
                int IsOk = DataFactory.Database().ExecuteByProc("[Login].dbo.[PROC_verify_IPAddress]", parameter.ToArray());
            }
        }

        #region 自定义分页测试
        public ActionResult testdata()
        {
            return View();
        }
        #endregion
    }
}
