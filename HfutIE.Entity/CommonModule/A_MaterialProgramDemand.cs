//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2014
// Software Developers @ HfutIE 2014
//=====================================================================================

using HfutIE.DataAccess.Attributes;
using HfutIE.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HfutIE.Entity
{
    /// <summary>
    /// 物料程序齐套校核表
    /// <author>
    ///		<name>she</name>
    ///		<date>2014.08.06 10:59</date>
    /// </author>
    /// </summary>
    [Description("物料程序齐套校核表")]
    [PrimaryKey("ID")]
    public class A_MaterialProgramDemand : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("主键")]
        public int? ID { get; set; }
        /// <summary>
        /// 项目主键
        /// </summary>
        [DisplayName("项目主键")]
        public int? ProjectID { get; set; }
        /// <summary>
        /// 项目编号
        /// </summary>
        [DisplayName("项目编号")]
        public string ProjectCode { get; set; }
        /// <summary>
        /// 计划ID
        /// </summary>
        /// <returns></returns>
        [DisplayName("计划ID")]
        public int? PlanID { get; set; }
        /// <summary>
        /// 计划编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("计划编号")]
        public string PlanCode { get; set; }
        /// <summary>
        /// 需求数量
        /// </summary>
        /// <returns></returns>
        [DisplayName("需求数量")]
        public int? DemandNum { get; set; }
        /// <summary>
        /// 优先级
        /// </summary>
        /// <returns></returns>
        [DisplayName("优先级")]
        public int? Priority { get; set; }
        /// <summary>
        /// 齐套任务编码
        /// </summary>
        /// <returns></returns>
        [DisplayName("齐套任务编码")]
        public string TaskCode { get; set; }
        /// <summary>
        /// 对应工序编码
        /// </summary>
        /// <returns></returns>
        [DisplayName("对应工序编码")]
        public string ProcedureCode { get; set; }
        /// <summary>
        /// 齐套状态
        /// </summary>
        /// <returns></returns>
        [DisplayName("齐套状态")]
        public int? Cstate { get; set; }
        /// <summary>
        /// 预警提前期
        /// </summary>
        /// <returns></returns>
        [DisplayName("预警提前期")]
        public DateTime? ProLeadTime { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("类型")]
        public int? Type { get; set; }
        /// <summary>
        /// 有效性
        /// </summary>
        /// <returns></returns>
        [DisplayName("有效性")]
        public int? IsAvailable { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建时间")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建人")]
        public string CreatorID { get; set; }
        /// <summary>
        /// 上次修改时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("上次修改时间")]
        public DateTime? LastModifiedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改人")]
        public string ModifierID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [DisplayName("备注")]
        public string Remarks { get; set; }
        /// <summary>
        /// 预留字段
        /// </summary>
        /// <returns></returns>
        [DisplayName("预留字段")]
        public string Reserve1 { get; set; }
        /// <summary>
        /// 预留字段
        /// </summary>
        /// <returns></returns>
        [DisplayName("预留字段")]
        public string Reserve2 { get; set; }
        /// <summary>
        /// 预留字段
        /// </summary>
        /// <returns></returns>
        [DisplayName("预留字段")]
        public string Reserve3 { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            //this.CompanyId = CommonHelper.GetGuid;
            this.CreateTime = DateTime.Now;
            this.CreatorID = ManageProvider.Provider.Current().UserId;
            //this.CreateUserName = ManageProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            //this.CompanyId = KeyValue;
            this.LastModifiedTime = DateTime.Now;
            this.ModifierID = ManageProvider.Provider.Current().UserId;
            //this.ModifyUserName = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}