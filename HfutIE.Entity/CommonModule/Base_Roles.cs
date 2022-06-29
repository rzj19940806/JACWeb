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
    /// ��ɫ�����
    /// <author>
    ///		<name>she</name>
    ///		<date>2014.08.07 15:34</date>
    /// </author>
    /// </summary>
    [Description("��ɫ�����")]
    [PrimaryKey("RoleId")]
    public class Base_Roles : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public string RoleId { get; set; }
        /// <summary>
        /// ��˾ID
        /// </summary>
        /// <returns></returns>
        [DisplayName("��˾ID")]
        public string CompanyId { get; set; }
        /// <summary>
        /// ��ɫ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ɫ���")]
        public string Category { get; set; }
        /// <summary>
        /// ��ɫ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ɫ���")]
        public string Code { get; set; }
        /// <summary>
        /// ��ɫ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ɫ����")]
        public string FullName { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ע")]
        public string Remark { get; set; }
        /// <summary>
        /// ��Ч��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ч��")]
        public int? Enabled { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������")]
        public int? SortCode { get; set; }
        /// <summary>
        /// DeleteMark
        /// </summary>
        /// <returns></returns>
        [DisplayName("DeleteMark")]
        public int? DeleteMark { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ʱ��")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// ������ID
        /// </summary>
        /// <returns></returns>
        [DisplayName("������ID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        [DisplayName("����������")]
        public string CreateUserName { get; set; }

        /// <summary>
        /// �ϴ��޸�ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ϴ��޸�ʱ��")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// �޸���ID
        /// </summary>
        /// <returns></returns>
        [DisplayName("�޸���ID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// �޸�������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�޸�������")]
        public string ModifyUserName { get; set; }

        ///// <summary>
        ///// Ԥ���ֶ�
        ///// </summary>
        ///// <returns></returns>
        //[DisplayName("Ԥ���ֶ�")]
        //public string Reserve1 { get; set; }
        ///// <summary>
        ///// Ԥ���ֶ�
        ///// </summary>
        ///// <returns></returns>
        //[DisplayName("Ԥ���ֶ�")]
        //public string Reserve2 { get; set; }
        ///// <summary>
        ///// Ԥ���ֶ�
        ///// </summary>
        ///// <returns></returns>
        //[DisplayName("Ԥ���ֶ�")]
        //public string Reserve3 { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.RoleId = CommonHelper.GetGuid;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = ManageProvider.Provider.Current().UserId;
            this.CreateUserName = ManageProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.RoleId = KeyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = ManageProvider.Provider.Current().UserId;
            this.ModifyUserName = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}