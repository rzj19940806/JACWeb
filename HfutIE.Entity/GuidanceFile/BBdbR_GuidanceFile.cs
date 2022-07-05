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
    /// ָ���ļ�
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.09 11:15</date>
    /// </author>
    /// </summary>
    [Description("ָ���ļ�")]
    [PrimaryKey("GuidanceFileID")]
    public class BBdbR_GuidanceFile : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ָ���ļ�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("ָ���ļ�����")]
        public string GuidanceFileID { get; set; }
        /// <summary>
        /// ����ID
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ID")]
        public string PlineId { get; set; }
        /// <summary>
        /// ��λID
        /// </summary>
        /// <returns></returns>
        [DisplayName("��λID")]
        public string WcId { get; set; }
        /// <summary>
        /// ��λ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��λ����")]
        public string WcNm { get; set; }
        /// <summary>
        /// ָ���ļ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("ָ���ļ����")]
        public string GuidanceFileCode { get; set; }
        /// <summary>
        /// ָ���ļ�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("ָ���ļ�����")]
        public string GuidanceFileName { get; set; }
        /// <summary>
        /// ָ���ļ�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("ָ���ļ�����")]
        public string GuidanceFileType { get; set; }
        /// <summary>
        /// ָ���ļ�·��
        /// </summary>
        /// <returns></returns>
        [DisplayName("ָ���ļ�·��")]
        public string GuidanceFilePath { get; set; }
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
            this.GuidanceFileID = CommonHelper.GetGuid;
            this.CreTm = DateTime.Now.ToLocalTime();
            this.CreCd = ManageProvider.Provider.Current().UserId;
            this.CreNm = ManageProvider.Provider.Current().UserName;
            Enabled = "1";
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.GuidanceFileID = KeyValue;
                                            }
        #endregion
    }
}