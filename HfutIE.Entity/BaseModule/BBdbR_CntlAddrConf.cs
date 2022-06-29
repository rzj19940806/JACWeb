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
    [Description("���ɵ�ַ��Ϣ��")]
    [PrimaryKey("RecId")]
    public class BBdbR_CntlAddrConf : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public string RecId { get; set; }
       
        /// <summary>
        /// ��λId
        /// </summary>
        /// <returns></returns>
        [DisplayName("��λId")]
        public string WcId { get; set; }
        /// <summary>
        /// �豸Id
        /// </summary>
        /// <returns></returns>
        [DisplayName("�豸Id")]
        public string DvcId { get; set; }
       
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string CntlType { get; set; }
        /// <summary>
        /// ��ַ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ַ����")]
        public string SingnalNm { get; set; }
        /// <summary>
        /// ��ֵַ
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ֵַ")]
        public string CntlAddr { get; set; }
        /// <summary>
        /// ��ַ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ַ����")]
        public string CntlAddrDsc { get; set; }
        /// <summary>
        /// ��ַ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ַ��������")]
        public string CntlDateType { get; set; }
        /// <summary>
        /// ��ַ��Դ
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ַ��Դ")]
        public string SglSource { get; set; }
        /// <summary>
        /// �Ƿ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ƿ���")]
        public string IsMonitoring { get; set; }
        /// <summary>
        /// ���Ƶ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("���Ƶ��")]
        public int? MonitorRate { get; set; }
        
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
            this.RecId = CommonHelper.GetGuid;
            //this.VersionNumber = "V1.0";
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
            this.RecId = KeyValue;
            //this.VersionNumber = "V1.0";
            this.MdfTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
       
    }
}