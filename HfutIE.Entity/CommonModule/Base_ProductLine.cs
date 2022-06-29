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
    /// Base_ProductLine
    /// <author>
    ///		<name>she</name>
    ///		<date>2016.10.04 15:10</date>
    /// </author>
    /// </summary>
    [Description("Base_ProductLine")]
    [PrimaryKey("ProductLineId")]
    public class Base_ProductLine : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ProductLineId
        /// </summary>
        /// <returns></returns>
        [DisplayName("ProductLineId")]
        public string ProductLineId { get; set; }
        /// <summary>
        /// FactoryId
        /// </summary>
        /// <returns></returns>
        [DisplayName("FactoryId")]
        public string FactoryId { get; set; }
        /// <summary>
        /// ProductLineName
        /// </summary>
        /// <returns></returns>
        [DisplayName("ProductLineName")]
        public string ProductLineName { get; set; }
        /// <summary>
        /// ProdcutLineCode
        /// </summary>
        /// <returns></returns>
        [DisplayName("ProdcutLineCode")]
        public string ProdcutLineCode { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        /// <returns></returns>
        [DisplayName("Description")]
        public string Description { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.ProductLineId = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.ProductLineId = KeyValue;
                                            }
        #endregion
    }
}