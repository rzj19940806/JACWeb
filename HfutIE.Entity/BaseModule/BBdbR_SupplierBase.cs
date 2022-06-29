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
    /// ��Ӧ�̻�����Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.10 22:44</date>
    /// </author>
    /// </summary>
    [Description("��Ӧ�̻�����Ϣ��")]
    [PrimaryKey("SupplierId")]
    public class BBdbR_SupplierBase : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ��Ӧ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ӧ������")]
        public string SupplierId { get; set; }
        /// <summary>
        /// ��Ӧ�̴���
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ӧ�̴���")]
        public string SupplierCd { get; set; }
        /// <summary>
        /// ��Ӧ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ӧ������")]
        public string SupplierNm { get; set; }
        /// <summary>
        /// ��Ӧ����ϵ�绰
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ӧ����ϵ�绰")]
        public string SupplierTeleph { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������")]
        public string Mgr { get; set; }
        /// <summary>
        /// ��Ӧ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ӧ������")]
        public string SupplierCatg { get; set; }
        /// <summary>
        /// ��Ӧ�̵ȼ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ӧ�̵ȼ�")]
        public string SupplierGrade { get; set; }
        /// <summary>
        /// ��Ӧ�̵�ַ
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ӧ�̵�ַ")]
        public string SupplierAddress { get; set; }
        /// <summary>
        /// ��Ӧ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ӧ������")]
        public string SupplierEmail { get; set; }
        /// <summary>
        /// ��Ӧ����ַ
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ӧ����ַ")]
        public string SupplierWebsite { get; set; }
        /// <summary>
        /// ��Ӧ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ӧ������")]
        public string Description { get; set; }
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
        public string Enabled { get; set; }
        /// <summary>
        /// �汾��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�汾��")]
        public string VersionNumber { get; set; }
        /// <summary>
        /// �����˱��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����˱��")]
        public string CreCd { get; set; }
        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        [DisplayName("����������")]
        public string CreNm { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ʱ��")]
        public string CreTm { get; set; }
        /// <summary>
        /// �޸��˱��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�޸��˱��")]
        public string MdfCd { get; set; }
        /// <summary>
        /// �޸�������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�޸�������")]
        public string MdfNm { get; set; }
        /// <summary>
        /// �޸�ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�޸�ʱ��")]
        public string MdfTm { get; set; }
        /// <summary>
        /// Ԥ���ֶ�1
        /// </summary>
        /// <returns></returns>
        [DisplayName("Ԥ���ֶ�1")]
        public string RsvFld1 { get; set; }
        /// <summary>
        /// Ԥ���ֶ�2
        /// </summary>
        /// <returns></returns>
        [DisplayName("Ԥ���ֶ�2")]
        public string RsvFld2 { get; set; }
        /// <summary>
        /// Ԥ���ֶ�3
        /// </summary>
        /// <returns></returns>
        [DisplayName("Ԥ���ֶ�3")]
        public string RsvFld3 { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.SupplierId = CommonHelper.GetGuid;
            this.Enabled = "1";
            this.VersionNumber = "V1.0";
            this.CreTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.CreCd = ManageProvider.Provider.Current().UserId;
            this.CreNm = ManageProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.SupplierId = KeyValue;
            this.MdfTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}