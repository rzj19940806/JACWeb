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
    /// Andon���ݲɼ����̱�
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.11 15:17</date>
    /// </author>
    /// </summary>
    [Description("Andon���ݲɼ����̱�")]
    [PrimaryKey("RecId")]
    public class BBdbR_DataAcqPro : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ��¼����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��¼����")]
        public string RecId { get; set; }
        /// <summary>
        /// ͣ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("ͣ������")]
        public string BPIERecId { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string PlineId { get; set; }
        /// <summary>
        /// ���߱��
        /// </summary>
        /// <returns></returns>
        [DisplayName("���߱��")]
        public string PlineCd { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string PlineNm { get; set; }
        /// <summary>
        /// ��λ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��λ����")]
        public string WcId { get; set; }
        /// <summary>
        /// ��λ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("��λ����")]
        public string WcCd { get; set; }
        /// <summary>
        /// ��λ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��λ����")]
        public string WcNm { get; set; }
        /// <summary>
        /// ��λ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��λ����")]
        public string PostId { get; set; }
        /// <summary>
        /// KepSId
        /// </summary>
        /// <returns></returns>
        [DisplayName("KepSId")]
        public string KepServerId { get; set; }
        /// <summary>
        /// KepSName
        /// </summary>
        /// <returns></returns>
        [DisplayName("KepSName")]
        public string KepServerNm { get; set; }
        /// <summary>
        /// KepSNode(IP)
        /// </summary>
        /// <returns></returns>
        [DisplayName("KepSNode(IP)")]
        public string KepServerNd { get; set; }
        /// <summary>
        /// KepServer��Դ
        /// </summary>
        /// <returns></returns>
        [DisplayName("KepServer��Դ")]
        public string KepServerSource { get; set; }
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
        public string MonitoringRate { get; set; }
        /// <summary>
        /// ���Ƶ�ַ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("���Ƶ�ַ����")]
        public string CntlAddrDsc { get; set; }
        
        /// <summary>
        /// �ź����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ź����")]
        public string SignalTyp { get; set; }
        /// <summary>
        /// �ź���Դ
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ź���Դ")]
        public string SignalSource { get; set; }
        /// <summary>
        /// �ź�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ź�����")]
        public string SignalDetail { get; set; }
        /// <summary>
        /// ��ǰֵ
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ǰֵ")]
        public string CurValue { get; set; }
        /// <summary>
        /// �ɼ�ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ɼ�ʱ��")]
        public string AcqTm { get; set; }
        /// <summary>
        /// ����״̬
        /// </summary>
        /// <returns></returns>
        [DisplayName("����״̬")]
        public string HandleSts { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ʱ��")]
        public string EndTm { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ʱ��")]
        public string HandleTm { get; set; }
        /// <summary>
        /// �����˱��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����˱��")]
        public string HandlerCode { get; set; }
        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        [DisplayName("����������")]
        public string HandlerName { get; set; }
        /// <summary>
        /// �쳣����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�쳣����")]
        public string ExceptionType { get; set; }
        /// <summary>
        /// �쳣����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�쳣����")]
        public string ExceptionDes { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������")]
        public string HandleResult { get; set; }
        /// <summary>
        /// ��Ч��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ч��")]
        public string Enabled { get; set; }
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
            this.AcqTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.Enabled = "1";
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.RecId = KeyValue;
            this.MdfTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}