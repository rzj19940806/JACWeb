using HfutIE.Business;
using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Entity.EntityModel;
using HfutIE.Repository;
using HfutIE.Utilities;
using NPOI.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace HfutIE.WebApp.Areas.VideoModule.Controllers
{
    /// <summary>
    /// 指导文件控制器BBdbR_GuidanceFile
    /// </summary>
    public class BBdbR_GuidanceFileController : PublicController<BBdbR_GuidanceFile>
    {

        BBdbR_GuidanceFileBll MyBll = new BBdbR_GuidanceFileBll();

        #region 打开视图区域

        public virtual ActionResult File()
        {
            return View();
        }

        //管理后台预览界面
        public virtual ActionResult VideoForm()
        {
            return View();
        } 
        #endregion

        //左侧树
        public ActionResult TreeJson()
        {
            try
            {
                //DataTable dt = MyBll1.GetTree();//获取树所需数据
                DataTable dt = MyBll.GetTree();//获取树所需数据
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
        //查询
        public ActionResult GetGridPage(string ParameterJson, JqGridParam jqgridparam)
        {
            try
            {
                Condition condition = new Condition();
                condition.ParamName = "Enabled";
                condition.Operation = ConditionOperate.Equal;
                condition.ParamValue = "1";
                condition.Logic = "AND";
                Stopwatch watch = CommonHelper.TimerStart();
                List<Condition> conditions = new List<Condition>();
                conditions.Add(condition);
                List<DbParameter> parameter = new List<DbParameter>();

                if (!string.IsNullOrEmpty(ParameterJson))
                {
                    conditions.AddRange(ParameterJson.JonsToList<Condition>());
                }
                string WhereSql = ConditionBuilder.GetWhereSql(conditions, out parameter);
                DataTable dataTable = repositoryfactory.Repository().FindTableBySql("select GuidanceFileID,WcNm,GuidanceFileName,GuidanceFileType,CreTm,CreNm,CONVERT(decimal(10,3),1.0*RsvFld1/1048576) RsvFld1 from BBdbR_GuidanceFile where 1=1 " + WhereSql +" order by "+ jqgridparam.sidx +" "+ jqgridparam.sord, parameter.ToArray());
                //var dataList = repositoryfactory.Repository().FindListPage(WhereSql, parameter.ToArray(), ref jqgridparam);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dataTable,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "指导文件信息查询成功");

                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "指导文件信息查询发生异常错误：" + ex.Message + "\r\n条件：" + ParameterJson);
                return null;
            }
        }
        //上传
        public ActionResult UploadingVideo(HttpPostedFileBase file, string plineId, string wcId, string wcNm)
        {
            int state = -1;
            string msg = "上传失败";
            BBdbR_GuidanceFile videoFile = new BBdbR_GuidanceFile();
            try
            {
                
                //判断文件是否为空
                if (file != null)
                {
                    videoFile.Create();
                    videoFile.PlineId = plineId;
                    videoFile.WcId = wcId;
                    videoFile.WcNm = wcNm;

                    videoFile.GuidanceFileName = file.FileName;
                    videoFile.GuidanceFileType = file.ContentType;
                    BinaryReader r = new BinaryReader(file.InputStream);
                    byte[] Content = r.ReadBytes(file.ContentLength);

                   
                    state = repositoryfactory.Repository().Insert(videoFile);

                    StringBuilder sql = new StringBuilder();
                    sql.Append($@"update BBdbR_GuidanceFile set RsvFld1='{file.ContentLength}', Content = @content where GuidanceFileID = '" + videoFile.GuidanceFileID + "'");
                    var dbparameter = new SqlParameter("@content", Content);
                    state += repositoryfactory.Repository().ExecuteBySql(sql, new[] { dbparameter });

                    if (state == 2)
                    {
                        msg = "上传成功";
                    }
                    this.WriteLog(state, videoFile, null, "新增成功", msg);
                    //}
                    //else
                    //{ msg = "文件格式错误！";}
                }
                else
                { msg = "没有找到该文件！"; }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                this.WriteLog(state, videoFile, null, "新增失败", msg);
            }
            return Json(new { Code = state, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Deduplicate()
        {
            string str = "SELECT DISTINCT(a.GuidanceFileName) FROM dbo.BBdbR_GuidanceFile A,dbo.BBdbR_GuidanceFile B WHERE A.GuidanceFileName=B.GuidanceFileName AND a.WcNm=b.WcNm AND a.GuidanceFileID!=b.GuidanceFileID";
            DataTable dt = DbHelperSQL.OpenTable(str);
            ArrayList list = new ArrayList();
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                string name = dt.Rows[i][0].ToString();
                string str1 = "SELECT GuidanceFileID  FROM dbo.BBdbR_GuidanceFile WHERE GuidanceFileName='" + name + "'";
                DataTable dt1 = DbHelperSQL.OpenTable(str1);
                for(int j = 1; j < dt1.Rows.Count; j++)
                {
                    string str2 = "DELETE dbo.BBdbR_GuidanceFile WHERE GuidanceFileID='" + dt1.Rows[j][0] + "'";
                    list.Add(str2);
                }
            }
            DbHelperSQL.ExecuteSqlTran(list);
            return Content(new JsonMessage { Success = true, Code = "1", Message = "成功" }.ToString());
        }
        //删除
        public ActionResult DeleteVideo(string KeyValue)
        {
            try
            {
                if (repositoryfactory.Repository().Delete(KeyValue.Split(',')) > 0)
                {
                    Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "指导文件删除成功");
                    return Json(new { Message = "删除成功", Code = 1 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "指导文件删除失败");
                    return Json(new { Message = "删除失败", Code = -1 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "指导文件删除异常");
                return Json(new { Message = "删除异常:"+ ex.Message}, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult DownLoadFile(String path)
        {
            string contentType = "";
            string fileName = path.Substring(path.LastIndexOf('/') + 1);
            string fileExtension = System.IO.Path.GetExtension(fileName);
            switch (fileExtension)
            {
                case ".mp4":
                    contentType = "video/mpeg4";
                    break;
                default:
                    break;
            }
            path = Server.MapPath(path);
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(fileName));
            return File(fileStream, contentType);
        }

        //下载
        public FileContentResult download(string GuidanceFileID)
        {
            try
            {
                DataRow fileRow = DbHelperSQL.OpenTable($"select top 1 * from BBdbR_GuidanceFile where GuidanceFileID='{GuidanceFileID}'").Rows[0];
                string fileType = fileRow["GuidanceFileType"].ToString();
                string fileName = fileRow["GuidanceFileName"].ToString();
                byte[] file = (byte[])fileRow["Content"];
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "指导文件下载成功");
                return File(file, fileType, fileName);
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "指导文件下载操作失败：" + ex.Message);
                return null;
            }
            
        }

        //AVI及关重件获取图片
        public ActionResult GetPicture(string name)
        {
            byte[] file;
            DataTable fileTable = DbHelperSQL.OpenTable($"select top 1 * from BBdbR_GuidanceFile where GuidanceFileName like '{name}.%' and WcNm='车身照片' ");
            if (fileTable.Rows.Count>0)
            {
                DataRow fileRow = fileTable.Rows[0];
                return Content(new { Success = true, Code = "1", name = fileRow["GuidanceFileName"].ToString(), file = (byte[])fileRow["Content"] }.ToJson());
            }
            else
            {
                return GetPicture("车身默认照片");
            }
        }


        //视频预览
        public void GetVideo(string GuidanceFileID)
        {
            try
            {
                DataRow fileRow = DbHelperSQL.OpenTable($"select * from BBdbR_GuidanceFile where GuidanceFileID='{GuidanceFileID}'").Rows[0];
                byte[] bytes = (byte[])fileRow["Content"];
                //Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(fileInfo.Name));
                Response.AddHeader("Content-Length", bytes.Length.ToString());
                Response.AddHeader("Content-Transfer-Encoding", "binary");
                Response.ContentType = "application/octet-stream";
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "指导文件视频预览成功");
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "指导文件视频预览操作失败：" + ex.Message);
            }
            
        }
    }
}
