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
    /// �����Ŀ������Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.28 16:56</date>
    /// </author>
    /// </summary>
    [Description("�����Ŀ������Ϣ��")]
    [PrimaryKey("QuaCheckItemId")]
    public class BBdbR_QuaCheckItemBase : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ���������
        /// </summary>
        /// <returns></returns>
        [DisplayName("���������")]
        public string QuaCheckItemId { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������")]
        public string QuaCheckItemCd { get; set; }
        /// <summary>
        /// ���������
        /// </summary>
        /// <returns></returns>
        [DisplayName("���������")]
        public string QuaCheckItemNm { get; set; }
        /// <summary>
        /// ���������
        /// </summary>
        /// <returns></returns>
        [DisplayName("���������")]
        public string QuaCheckItemTy { get; set; }
        /// <summary>
        /// ���������
        /// </summary>
        /// <returns></returns>
        [DisplayName("���������")]
        public decimal? QuaCheckItemUpLimit { get; set; }
        /// <summary>
        /// ���������
        /// </summary>
        /// <returns></returns>
        [DisplayName("���������")]
        public decimal?  QuaCheckItemLowLimit { get; set; }
        /// <summary>
        /// ������׼ֵ
        /// </summary>
        /// <returns></returns>
        [DisplayName("������׼ֵ")]
        public decimal? QuaCheckItemValue { get; set; }
        /// <summary>
        /// ������׼ֵ
        /// </summary>
        /// <returns></returns>
        [DisplayName("����λ")]
        public string Unit { get; set; }
        /// <summary>
        /// ���������
        /// </summary>
        /// <returns></returns>
        [DisplayName("���������")]
        public string QuaCheckItemDsc { get; set; }
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
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ʱ��")]
        public DateTime? CreTm { get; set; }
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
        /// �޸�ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�޸�ʱ��")]
        public DateTime? MdfTm { get; set; }
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
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ע")]
        public string Rem { get; set; }
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
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.QuaCheckItemId = CommonHelper.GetGuid;
            this.Enabled = "1";
            this.VersionNumber = "V1.0";
            this.CreTm = DateTime.Now;
            this.CreCd = ManageProvider.Provider.Current().UserId;
            this.CreNm = ManageProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.QuaCheckItemId = KeyValue;
            this.MdfTm = DateTime.Now;
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;

        }
        #endregion
    }
}