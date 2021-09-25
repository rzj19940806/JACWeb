//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2017
// Software Developers @ HfutIE 2017
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
    /// USER_PHOTOGRAPH
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.08.21 21:56</date>
    /// </author>
    /// </summary>
    [Description("USER_PHOTOGRAPH")]
    [PrimaryKey("user_photograph_key")]
    public class USER_PHOTOGRAPH : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// user_photograph_key
        /// </summary>
        /// <returns></returns>
        [DisplayName("user_photograph_key")]
        public string user_photograph_key { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        /// <returns></returns>
        [DisplayName("UserId")]
        public string UserId { get; set; }
        /// <summary>
        /// Code
        /// </summary>
        /// <returns></returns>
        [DisplayName("Code")]
        public string Code { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        /// <returns></returns>
        [DisplayName("姓名")]
        public string RealName { get; set; }
        /// <summary>
        /// photograph
        /// </summary>
        /// <returns></returns>
        [DisplayName("photograph")]
        public byte[] photograph { get; set; }
        /// <summary>
        /// type
        /// </summary>
        /// <returns></returns>
        [DisplayName("type")]
        public string type { get; set; }
        /// <summary>
        /// reserve01
        /// </summary>
        /// <returns></returns>
        [DisplayName("reserve01")]
        public string reserve01 { get; set; }
        /// <summary>
        /// reserve02
        /// </summary>
        /// <returns></returns>
        [DisplayName("reserve02")]
        public string reserve02 { get; set; }
        /// <summary>
        /// reserve03
        /// </summary>
        /// <returns></returns>
        [DisplayName("reserve03")]
        public string reserve03 { get; set; }
        /// <summary>
        /// reserve04
        /// </summary>
        /// <returns></returns>
        [DisplayName("reserve04")]
        public string reserve04 { get; set; }
        /// <summary>
        /// reserve05
        /// </summary>
        /// <returns></returns>
        [DisplayName("reserve05")]
        public string reserve05 { get; set; }
        /// <summary>
        /// CreateDate
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreateDate")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// CreateUserId
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreateUserId")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// CreateUserName
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreateUserName")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// ModifyDate
        /// </summary>
        /// <returns></returns>
        [DisplayName("ModifyDate")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// ModifyUserId
        /// </summary>
        /// <returns></returns>
        [DisplayName("ModifyUserId")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// ModifyUserName
        /// </summary>
        /// <returns></returns>
        [DisplayName("ModifyUserName")]
        public string ModifyUserName { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.user_photograph_key = CommonHelper.GetGuid;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = ManageProvider.Provider.Current().UserId;
            this.CreateUserName = ManageProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.user_photograph_key = KeyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = ManageProvider.Provider.Current().UserId;
            this.ModifyUserName = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}