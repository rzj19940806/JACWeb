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
    /// AVIվ�������Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.03.26 10:57</date>
    /// </author>
    /// </summary>
    [Description("AVIվ�������Ϣ��")]
    [PrimaryKey("AviId")]
    public class BBdbR_AVIBase : BaseEntity
    {
        public string OP_CODE { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string OP_NAME { get; set; }
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// AVIվ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("AVIվ������")]
        public string AviId { get; set; }
        /// <summary>
        /// AVIվ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("AVIվ����")]
        public string AviCd { get; set; }
        /// <summary>
        /// AVIվ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("AVIվ������")]
        public string AviNm { get; set; }
       
        /// <summary>
        /// AVIվ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("AVIվ������")]
        public string AviType { get; set; }
        /// <summary>
        /// AVI����
        /// </summary>
        /// <returns></returns>
        [DisplayName("AVI����")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Dsc { get; set; }
        /// <summary>
        /// IsMonitor
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ƿ���Ҫ�ؼ���Ƶ���")]
        public string IsMonitor { get; set; }
        /// <summary>
        /// �Ƿ������ظ�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ƿ������ظ�����")]
        public string IsRePeated { get; set; }
        /// <summary>
        /// AVIվ���Ƿ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("AVIվ���Ƿ����")]
        public string IsIndependence { get; set; }
        /// <summary>
        /// ����Id
        /// </summary>
        /// <returns></returns>
        [DisplayName("����Id")]
        public string PlineId { get; set; }
        /// <summary>
        /// �Ƿ񱨹�
        /// </summary>
        /// <returns></returns>
         [DisplayName("AVIվ��˳��")]
        public int AVISequence { get; set; }
        /// <summary>
        /// �Ƿ񱨹�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ƿ񱨹�")]
        public string IsReport { get; set; }
        /// <summary>
        /// �汾��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ƿ���������")]
        public int IsStranded { get; set; }
        /// <summary>
        /// �汾��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����������")]
        public string StrandedCategory { get; set; }
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
            this.AviId = CommonHelper.GetGuid;
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
            this.AviId = KeyValue;
            this.VersionNumber = "V1.0";
            this.MdfTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
       
    }
}