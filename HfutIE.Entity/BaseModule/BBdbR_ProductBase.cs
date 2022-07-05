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
    [Description("��Ʒ������Ϣ")]
    [PrimaryKey("MatId")]
    public class BBdbR_ProductBase : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ������������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������������")]
        public string MatId { get; set; }
        /// <summary>
        /// ��Ʒ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ʒ���")]
        public string MatCd { get; set; }
        /// <summary>
        /// ��Ʒ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ʒ����")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MatNm { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CarType { get; set; }
        /// <summary>
        /// ��ɫ
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ɫ")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CarColor1 { get; set; }
        /// <summary>
        /// �����ͺ�(�����)
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����ͺ�(�����)")]
        public string Notification { get; set; }
        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        [DisplayName("����������")]
        public string EngineOut { get; set; }
        /// <summary>
        /// �������ͺ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������ͺ�")]
        public string EngineModel { get; set; }
        /// <summary>
        /// ��������󾻹���/ת��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������󾻹���/ת��")]
        public string EngineMaxPower { get; set; }
        /// <summary>
        /// �������������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������������")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string TOTAL_WEIGHT { get; set; }
        /// <summary>
        /// �ؿ�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ؿ�����")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CAPACITY { get; set; }
        /// <summary>
        /// ������������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������������")]
        public string BodyWeight { get; set; }
        /// <summary>
        /// ��;
        /// </summary>
        /// <returns></returns>
        [DisplayName("��;")]
        public string Purpose { get; set; }
        /// <summary>
        /// ���⳵������
        /// </summary>
        /// <returns></returns>
        [DisplayName("���⳵������")]
        public string SpecialCarNm { get; set; }
        /// <summary>
        /// �����������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����������")]
        public string MAX_POWER_SPEED { get; set; }
        /// <summary>
        /// ȼ�ͱ�ʶ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("ȼ�ͱ�ʶ����")]
        public string Oil { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string DIS { get; set; }
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
            this.MatId = CommonHelper.GetGuid;
            this.VersionNumber = "V1.0";
            this.CreTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.Enabled = "1";
            this.CreCd = ManageProvider.Provider.Current().UserId;
            this.CreNm = ManageProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.MatId = KeyValue;
            this.VersionNumber = "V1.0";
            this.MdfTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}