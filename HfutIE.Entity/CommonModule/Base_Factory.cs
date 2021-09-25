//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2016
// Software Developers @ HfutIE 2016
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
    /// Base_Factory
    /// <author>
    ///		<name>she</name>
    ///		<date>2016.10.04 15:06</date>
    /// </author>
    /// </summary>
    [Description("Base_Factory")]
    [PrimaryKey("FactoryId")]
    public class Base_Factory : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// FactoryId
        /// </summary>
        /// <returns></returns>
        [DisplayName("FactoryId")]
        public string FactoryId { get; set; }
        /// <summary>
        /// FactoryCode
        /// </summary>
        /// <returns></returns>
        [DisplayName("FactoryCode")]
        public string FactoryCode { get; set; }
        /// <summary>
        /// FactoryName
        /// </summary>
        /// <returns></returns>
        [DisplayName("FactoryName")]
        public string FactoryName { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        /// <returns></returns>
        [DisplayName("Description")]
        public string Description { get; set; }
        /// <summary>
        /// ParentId
        /// </summary>
        /// <returns></returns>
        [DisplayName("ParentId")]
        public string ParentId { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.FactoryId = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.FactoryId = KeyValue;
                                            }
        #endregion
    }
}