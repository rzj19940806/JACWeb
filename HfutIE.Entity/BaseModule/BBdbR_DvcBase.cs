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
    /// �豸������Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.29 15:54</date>
    /// </author>
    /// </summary>
    [Description("�豸������Ϣ��")]
    [PrimaryKey("DvcId")]
    public class BBdbR_DvcBase : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// �豸����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�豸����")]
        public string DvcId { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Class { get; set; }
        /// <summary>
        /// ����ID
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ID")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ClassId { get; set; }
        /// <summary>
        /// �豸λ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�豸λ��")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string DvcLocatn { get; set; }
        /// <summary>
        /// �豸���
        /// </summary>
        /// <returns></returns>
        [DisplayName("�豸���")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string DvcCd { get; set; }
        /// <summary>
        /// �豸����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�豸����")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string DvcNm { get; set; }
        /// <summary>
        /// �豸���
        /// </summary>
        /// <returns></returns>
        [DisplayName("�豸���")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string DvcCatg { get; set; }
        /// <summary>
        /// �豸����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�豸����")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string DvcTyp { get; set; }
        /// <summary>
        /// IP��ַ
        /// </summary>
        /// <returns></returns>
        [DisplayName("IP��ַ")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string IPAddr { get; set; }
        /// <summary>
        /// �˿�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�˿�")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Port { get; set; }
        /// <summary>
        /// �豸�ͺ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�豸�ͺ�")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string DvcMdl { get; set; }
        /// <summary>
        /// �豸����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�豸����")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string DvcMaker { get; set; }
        /// <summary>
        /// �豸����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�豸����")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string DvcLife { get; set; }
        /// <summary>
        /// �豸��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�豸��������")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string DvcMDt { get; set; }
        /// <summary>
        /// ά������(��)
        /// </summary>
        /// <returns></returns>
        [DisplayName("ά������(��)")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MaintCycle { get; set; }
        /// <summary>
        /// ��ǰ�ڣ��죩
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ǰ�ڣ��죩")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LeadTm { get; set; }
        /// <summary>
        /// �豸����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�豸����")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Dsc { get; set; }
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
            this.DvcId = CommonHelper.GetGuid;
            this.VersionNumber = "V1.0";
            this.Enabled = "1";
            this.CreTm = DateTime.Now.ToString();
            this.CreCd = ManageProvider.Provider.Current().UserId;
            this.CreNm = ManageProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.DvcId = KeyValue;
            this.MdfTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}