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
    /// INTERFACE_C
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.06.05 08:47</date>
    /// </author>
    /// </summary>
    [Description("INTERFACE_C")]
    [PrimaryKey("interface_key")]
    public class INTERFACE_C : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// interface_key
        /// </summary>
        /// <returns></returns>
        [DisplayName("interface_key")]
        public string interface_key { get; set; }
        /// <summary>
        /// interface_code
        /// </summary>
        /// <returns></returns>
        [DisplayName("interface_code")]
        public string interface_code { get; set; }
        /// <summary>
        /// interface_name
        /// </summary>
        /// <returns></returns>
        [DisplayName("interface_name")]
        public string interface_name { get; set; }
        /// <summary>
        /// interface_type
        /// </summary>
        /// <returns></returns>
        [DisplayName("interface_type")]
        public string interface_type { get; set; }
        /// <summary>
        /// interface_ip
        /// </summary>
        /// <returns></returns>
        [DisplayName("interface_ip")]
        public string interface_ip { get; set; }
        /// <summary>
        /// database_name
        /// </summary>
        /// <returns></returns>
        [DisplayName("database_name")]
        public string database_name { get; set; }
        /// <summary>
        /// user_name
        /// </summary>
        /// <returns></returns>
        [DisplayName("user_name")]
        public string user_name { get; set; }
        /// <summary>
        /// password
        /// </summary>
        /// <returns></returns>
        [DisplayName("password")]
        public string password { get; set; }
        /// <summary>
        /// start_time
        /// </summary>
        /// <returns></returns>
        [DisplayName("start_time")]
        public string start_time { get; set; }
        /// <summary>
        /// frequency
        /// </summary>
        /// <returns></returns>
        [DisplayName("frequency")]
        public Single? frequency { get; set; }
        /// <summary>
        /// remarks
        /// </summary>
        /// <returns></returns>
        [DisplayName("remarks")]
        public string remarks { get; set; }
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
        /// <summary>
        /// from_table_name
        /// </summary>
        /// <returns></returns>
        [DisplayName("from_table_name")]
        public string from_table_name { get; set; }
        /// <summary>
        /// rx_mode
        /// </summary>
        /// <returns></returns>
        [DisplayName("rx_mode")]
        public string rx_mode { get; set; }
        /// <summary>
        /// period
        /// </summary>
        /// <returns></returns>
        [DisplayName("period")]
        public string period { get; set; }
        /// <summary>
        /// period_unit
        /// </summary>
        /// <returns></returns>
        [DisplayName("period_unit")]
        public string period_unit { get; set; }
        /// <summary>
        /// to_table_name
        /// </summary>
        /// <returns></returns>
        [DisplayName("to_table_name")]
        public string to_table_name { get; set; }
        /// <summary>
        /// reserve06
        /// </summary>
        /// <returns></returns>
        [DisplayName("reserve06")]
        public string reserve06 { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.interface_key = CommonHelper.GetGuid;
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
            this.interface_key = KeyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = ManageProvider.Provider.Current().UserId;
            this.ModifyUserName = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}