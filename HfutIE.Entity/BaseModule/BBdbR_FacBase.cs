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
    /// ����������Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.26 15:22</date>
    /// </author>
    /// </summary>
    [Description("����������Ϣ��")]
    [PrimaryKey("FacId")]
    public class BBdbR_FacBase : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string FacId { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������")]
        public string FacCd { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string FacNm { get; set; }
        /// <summary>
        /// ��˾����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��˾����")]
        public string CompanyId { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string FacTyp { get; set; }
        /// <summary>
        /// ������ַ
        /// </summary>
        /// <returns></returns>
        [DisplayName("������ַ")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Addr { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Dsc { get; set; }
        /// <summary>
        /// ������Ա����
        /// </summary>
        /// <returns></returns>
        [DisplayName("������Ա����")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string StfId { get; set; }
        /// <summary>
        /// ������Ա���
        /// </summary>
        /// <returns></returns>
        [DisplayName("������Ա���")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string StfCd { get; set; }
        /// <summary>
        /// ������Ա����
        /// </summary>
        /// <returns></returns>
        [DisplayName("������Ա����")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string StfNm { get; set; }
        /// <summary>
        /// �����ֻ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����ֻ���")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Phn { get; set; }
        /// <summary>
        /// ��ϵ�绰
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ϵ�绰")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string FacTelephone { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string FacFax { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string FacEmail { get; set; }
        /// <summary>
        /// �汾��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�汾��")]
        public string VersionNumber { get; set; }
        /// <summary>
        /// ˳���
        /// </summary>
        /// <returns></returns>
        [DisplayName("˳���")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string sort { get; set; }
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
        public string CreTm { get; set; }
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
        public string MdfTm { get; set; }
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
            this.FacId = CommonHelper.GetGuid;
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
            this.FacId = KeyValue;
            this.MdfTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}