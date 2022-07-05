using HfutIE.Business;
using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HfutIE.WebApp.Areas.BaseModule.Controllers
{
    /// <summary>
    /// 班次时间配置表控制器
    /// </summary>
    public class BFacR_ShiftTimeConfigController : PublicController<BFacR_ShiftTimeConfig>
    {
        #region 全局变量定义区

        //定义本页面主要操作的表的表名，称为主表
        string tableName = "";//===复制时需要修改===
        public readonly RepositoryFactory<BFacR_ShiftTimeConfig> repository_shiftbase = new RepositoryFactory<BFacR_ShiftTimeConfig>();
        #endregion
        public virtual ActionResult ShiftTimeConfig()
        {
            return View();
        }
        #region 创建数据库操作对象区域
        //BBdbR_PushRuleBll，用以访问BBdbR_PushRuleBll中操作数据库的方法\
        BFacR_ShiftTimeConfigBll MyBll = new BFacR_ShiftTimeConfigBll();
        #endregion

        #region 2.新增、编辑
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
        ///// <summary>
        ///// 表单赋值
        ///// </summary>
        ///// <param name="KeyValue">主键值</param>
        ///// <returns></returns>
        //[HttpPost]
        //[ValidateInput(false)]
        //[LoginAuthorize]
        //public ActionResult SetForm1(string KeyValue)
        //{
        //    BFacR_ShiftTimeConfig entity = repository_shiftbase1.Repository().FindEntity(KeyValue);
        //    return Content(entity.ToJson());
        //}
        public ActionResult SubmitForm_TmConfig(BFacR_ShiftTimeConfig entity, string KeyValue, string FatherKey)//===复制时需要修改===
        {
            try
            {
                int IsOk = 0;//用于判断
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))//编辑操作
                {
                    //===复制时需要修改===
                    BFacR_ShiftTimeConfig Oldentity = repository_shiftbase.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    entity.ShiftId = Oldentity.ShiftId;
                    entity.ShiftTimeConfigId = KeyValue;//编辑保持主键不变
                    entity.Modify(KeyValue);
                    IsOk = MyBll.Update(entity);//将修改后的实体更新到数据库，插入成功返回1，失败返回0；
                }
                else//新增操作
                {
                    entity.ShiftId = FatherKey;
                    entity.ShiftTimeConfigId = Guid.NewGuid().ToString();
                    entity.Create();
                    IsOk = MyBll.Insert(entity);//将实体插入数据库，插入成功返回1，失败返回0；
                }
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion
        #region 3.删除
        /// <summary>
        /// 首先判断需要删除的信息是否绑定了其他信息
        ///     如：删除一条工厂信息先要判断该条公司信息下面是否绑定了车间信息
        ///         如果绑定了信息，则提示“当前所选有子节点数据，不能删除。”并结束
        ///  在确定没绑定数据的情况下，删除该数据
        ///     删除表示将该数据的Enable属性设为false，并不真的删除该记录
        /// </summary>
        /// <param name="KeyValue">页面中提供的需要删除的数据的主键,可能是多条数据的主键，即多个主键</param>
        /// <param name="ParentId">不需要</param>
        /// <param name="DeleteMark">不需要</param>
        /// <returns></returns>
        public override ActionResult Delete(string KeyValue, string ParentId, string DeleteMark)
        {
            //不管是多个主键还是单个主键，将主键拆分出来，放在数组中
            string[] array = KeyValue.Split(',');
            try
            {
                var Message = "删除失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//判断删除方法是否成，0表示不成功，大于0表示成功

                //直接删除
                if (array != null && array.Length > 0)
                {
                    IsOk = MyBll.Delete(array);//软删除-Enabled=0
                }
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
    }
}