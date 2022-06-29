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
    /// ��˾����
    /// <author>
    ///		<name>she</name>
    ///		<date>2014.08.06 10:59</date>
    /// </author>
    /// </summary>
    [Description("��������")]
    [PrimaryKey("ID")]
    public class A_OrderBase : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public int? ID { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        [DisplayName("�������")]
        public string OrderCode { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string OrderName { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string Description { get; set; }
        /// <summary>
        /// �ͻ�ID
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ͻ�ID")]
        public int? CustomerID { get; set; }
        /// <summary>
        /// �ͻ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ͻ����")]
        public string CustomerCode { get; set; }
        /// <summary>
        /// �ͻ�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ͻ�����")]
        public string CustomerName { get; set; }
        /// <summary>
        /// ����״̬
        /// </summary>
        /// <returns></returns>
        [DisplayName("����״̬")]
        public int? State { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public int? OrderType { get; set; }
        /// <summary>
        /// ���ȼ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("���ȼ�")]
        public int? Priority { get; set; }
        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        [DisplayName("����������")]
        public string TechnicalDirector { get; set; }
        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        [DisplayName("����������")]
        public string ManufacturingDirector { get; set; }
        /// <summary>
        /// ��Ч��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ч��")]
        public int? IsAvailable { get; set; }
        
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ʱ��")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������")]
        public string CreatorID { get; set; }
        /// <summary>
        /// �ϴ��޸�ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ϴ��޸�ʱ��")]
        public DateTime? LastModifiedTime { get; set; }
        /// <summary>
        /// �޸���
        /// </summary>
        /// <returns></returns>
        [DisplayName("�޸���")]
        public string ModifierID { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ע")]
        public string Remarks { get; set; }
        /// <summary>
        /// Ԥ���ֶ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("Ԥ���ֶ�")]
        public string Reserve1 { get; set; }
        /// <summary>
        /// Ԥ���ֶ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("Ԥ���ֶ�")]
        public string Reserve2 { get; set; }
        /// <summary>
        /// Ԥ���ֶ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("Ԥ���ֶ�")]
        public string Reserve3 { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            //this.CompanyId = CommonHelper.GetGuid;
            this.CreateTime = DateTime.Now;
            this.CreatorID = ManageProvider.Provider.Current().UserId;
            //this.CreateUserName = ManageProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// �༭����
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