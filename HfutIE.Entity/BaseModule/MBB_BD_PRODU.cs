//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2021
// Software Developers @ HfutIE 2021
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
    /// 铭牌信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.14 16:56</date>
    /// </author>
    /// </summary>
    [Description("铭牌信息表")]
    [PrimaryKey("GID")]
    public class MBB_BD_PRODU : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("主键")]
        public string GID { get; set; }
        /// <summary>
        /// 产品编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("产品编号")]
        public string CODE { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("产品名称")]
        public string NAME { get; set; }
        /// <summary>
        /// 整车型号
        /// </summary>
        /// <returns></returns>
        [DisplayName("整车型号")]
        public string UDA19 { get; set; }
        /// <summary>
        /// 发动机排量
        /// </summary>
        /// <returns></returns>
        [DisplayName("发动机排量")]
        public string UDA2 { get; set; }
        /// <summary>
        /// 发动机型号
        /// </summary>
        /// <returns></returns>
        [DisplayName("发动机型号")]
        public string UDA1 { get; set; }
        /// <summary>
        /// 发动机最大净功率
        /// </summary>
        /// <returns></returns>
        [DisplayName("发动机最大净功率")]
        public string UDA3 { get; set; }
        /// <summary>
        /// 最大净功率转速
        /// </summary>
        /// <returns></returns>
        [DisplayName("最大净功率转速")]
        public string MAX_POWER_SPEED { get; set; }
        /// <summary>
        /// 最大允许总质量
        /// </summary>
        /// <returns></returns>
        [DisplayName("最大允许总质量")]
        public Single? TOTAL_WEIGHT { get; set; }
        /// <summary>
        /// 整车整备型号
        /// </summary>
        /// <returns></returns>
        [DisplayName("整车整备型号")]
        public string UDA4 { get; set; }
        /// <summary>
        /// 容量
        /// </summary>
        /// <returns></returns>
        [DisplayName("容量")]
        public Single? CAPACITY { get; set; }
        /// <summary>
        /// 变速器型号
        /// </summary>
        /// <returns></returns>
        [DisplayName("变速器型号")]
        public string UDA16 { get; set; }
        /// <summary>
        /// 后桥型号
        /// </summary>
        /// <returns></returns>
        [DisplayName("后桥型号")]
        public string UDA21 { get; set; }
        /// <summary>
        /// 特殊车辆名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("特殊车辆名称")]
        public string UDA9 { get; set; }
        /// <summary>
        /// 用途
        /// </summary>
        /// <returns></returns>
        [DisplayName("用途")]
        public string UDA10 { get; set; }
        /// <summary>
        /// 制造日期
        /// </summary>
        /// <returns></returns>
        [DisplayName("制造日期")]
        public DateTime? PLANNED_WORK_ORDER { get; set; }
        /// <summary>
        /// 有效性
        /// </summary>
        /// <returns></returns>
        [DisplayName("有效性")]
        public string Enabled { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建人")]
        public string CREATE_ID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建时间")]
        public DateTime? CREATE_DATE { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改人")]
        public string MODIFY_ID { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改时间")]
        public DateTime? MODIFY_DATE { get; set; }
        /// <summary>
        /// 预留字段1
        /// </summary>
        /// <returns></returns>
        [DisplayName("预留字段1")]
        public string RsvFld1 { get; set; }
        /// <summary>
        /// 预留字段2
        /// </summary>
        /// <returns></returns>
        [DisplayName("预留字段2")]
        public string RsvFld2 { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.CREATE_DATE = DateTime.Now;
            this.CREATE_ID = ManageProvider.Provider.Current().UserId;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.MODIFY_DATE = DateTime.Now;
            this.MODIFY_ID = ManageProvider.Provider.Current().UserId;

        }
        #endregion
    }
}