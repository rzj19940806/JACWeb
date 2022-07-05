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
using System.Drawing;
using System.Web.UI.WebControls;

namespace HfutIE.Entity
{
    /// <summary>
    /// ���ϻ�����Ϣ
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.04.02 16:27</date>
    /// </author>
    /// </summary>
    [Description("����")]
    [PrimaryKey("Id")]
    public class BBdbR_PartMatConfig : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public string Id { get; set; }
        /// <summary>
        /// ���ID
        /// </summary>
        /// <returns></returns>
        [DisplayName("���ID")]
        public string PartId { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������")]
        public string PartCd { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PartNm { get; set; }
        /// <summary>
        /// ���ϱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("���ϱ��")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MatCd { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MatNm { get; set; }
       
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������")]
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
            this.Id = CommonHelper.GetGuid;
            this.VersionNumber = "V1.0";
            this.CreTm = DateTime.Now;
            this.Enabled = "1";
            this.CreCd = ManageProvider.Provider.Current().Code;
            this.CreNm = ManageProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public void Modify()
        {
            this.VersionNumber = "V1.0";
            this.MdfTm = DateTime.Now;
            this.MdfCd = ManageProvider.Provider.Current().Code;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}