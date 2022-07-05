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
    /// ���ϻ�����Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.11.21 21:20</date>
    /// </author>
    /// </summary>
    [Description("���ϻ�����Ϣ��")]
    [PrimaryKey("MatId")]
    public class BBdbR_MatBase : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ����ID
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ID")]
        public string MatId { get; set; }
        /// <summary>
        /// ���ϱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("���ϱ��")]
        public string MatCd { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MatNm { get; set; }
        /// <summary>
        /// WcId
        /// </summary>
        /// <returns></returns>
        [DisplayName("WcId")]
        public string WcId { get; set; }
        /// <summary>
        /// WcCd
        /// </summary>
        /// <returns></returns>
        [DisplayName("WcCd")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string WcCd { get; set; }
        /// <summary>
        /// WcNm
        /// </summary>
        /// <returns></returns>
        [DisplayName("WcNm")]
        public string WcNm { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MatCatg { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MatTyp { get; set; }
        /// <summary>
        /// ����ͺ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ͺ�")]
        public string MatSpec { get; set; }
        /// <summary>
        /// IsScan
        /// </summary>
        /// <returns></returns>
        [DisplayName("IsScan")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string IsScan { get; set; }
        /// <summary>
        /// MatNum
        /// </summary>
        /// <returns></returns>
        [DisplayName("MatNum")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MatNum { get; set; }
        /// <summary>
        /// IsPrint
        /// </summary>
        /// <returns></returns>
        [DisplayName("IsPrint")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string IsPrint { get; set; }
        /// <summary>
        /// ShortCode
        /// </summary>
        /// <returns></returns>
        [DisplayName("ShortCode")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ShortCode { get; set; }
        /// <summary>
        /// Unit
        /// </summary>
        /// <returns></returns>
        [DisplayName("Unit")]
        public string Unit { get; set; }
        /// <summary>
        /// Ĭ��ͼƬ
        /// </summary>
        /// <returns></returns>
        [DisplayName("Ĭ��ͼƬ")]
        public byte[] MatImg { get; set; }
        /// <summary>
        /// ��ǰ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ǰ��")]
        public string LeadTm { get; set; }
        /// <summary>
        /// ��Ʒ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ʒ��")]
        public decimal? YieldRate { get; set; }
        /// <summary>
        /// �汾��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�汾��")]
        public string VersionNumber { get; set; }
        /// <summary>
        /// ��Ч��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ч��")]
        public string Enabled { get; set; }
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
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Rem { get; set; }
        /// <summary>
        /// Ԥ���ֶ�1
        /// </summary>
        /// <returns></returns>
        [DisplayName("Ԥ���ֶ�1")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string RsvFld1 { get; set; }
        /// <summary>
        /// Ԥ���ֶ�2
        /// </summary>
        /// <returns></returns>
        [DisplayName("Ԥ���ֶ�2")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string RsvFld2 { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.MatId = CommonHelper.GetGuid;
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.MatId = KeyValue;
            this.MdfTm = DateTime.Now;
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}