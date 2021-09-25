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
    /// Base_Product
    /// <author>
    ///		<name>she</name>
    ///		<date>2016.10.04 09:35</date>
    /// </author>
    /// </summary>
    [Description("Base_Product")]
    [PrimaryKey("ProductId")]
    public class Base_Product : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// ProductId
        /// </summary>
        /// <returns></returns>
        [DisplayName("ProductId")]
        public string ProductId { get; set; }
        /// <summary>
        /// ProductName
        /// </summary>
        /// <returns></returns>
        [DisplayName("ProductName")]
        public string ProductName { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ProductId = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.ProductId = KeyValue;
                                            }
        #endregion
    }
}