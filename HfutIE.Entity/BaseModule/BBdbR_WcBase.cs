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
    /// ��λ������Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.27 16:34</date>
    /// </author>
    /// </summary>
    [Description("��λ������Ϣ��")]
    [PrimaryKey("WcId")]
    public class BBdbR_WcBase : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ��λ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��λ����")]
        public string WcId { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string PlineId { get; set; }
        /// <summary>
        /// ��λ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("��λ���")]
        public string WcCd { get; set; }
        /// <summary>
        /// ��λ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��λ����")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string WcNm { get; set; }
        /// <summary>
        /// ��λ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��λ����")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string WcTyp { get; set; }
        /// <summary>
        /// ��λ˳��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��λ˳��")]
        public int? WcQueue { get; set; }
        /// <summary>
        /// ��λ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��λ����")]
        public string WcLength { get; set; }
        /// <summary>
        /// ��λ��ʼ
        /// </summary>
        /// <returns></returns>
        [DisplayName("��λ��ʼ")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? StartPoint { get; set; }
        /// <summary>
        /// Ԥ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("Ԥ��")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? PreAlarmPoint { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? EndPoint { get; set; }
        /// <summary>
        /// ֹͣ
        /// </summary>
        /// <returns></returns>
        [DisplayName("ֹͣ")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? StopPoint { get; set; }
        /// <summary>
        /// �����
        /// </summary>
        /// <returns></returns>
        [DisplayName("Pass����")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? Seq { get; set; }
        /// <summary>
        /// ��λ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��λ����")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Dsc { get; set; }
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
            this.WcId = CommonHelper.GetGuid;
            this.VersionNumber = "V1.0";
            this.Enabled = "1";
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
            this.WcId = KeyValue;
            this.VersionNumber = "V1.0";
            this.Enabled = "1";
            this.MdfTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}