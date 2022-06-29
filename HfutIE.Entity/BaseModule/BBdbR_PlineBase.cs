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
    /// ���߻�����Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.27 09:17</date>
    /// </author>
    /// </summary>
    [Description("���߻�����Ϣ��")]
    [PrimaryKey("PlineId")]
    public class BBdbR_PlineBase : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string PlineId { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string WorkSectionId { get; set; }
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
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string PlineTyp { get; set; }
        /// <summary>
        /// ��λ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��λ����")]
        public int? WcQuantity { get; set; }
        /// <summary>
        /// ��λĬ�ϳ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("��λĬ�ϳ���")]
        public decimal? WcLength { get; set; }
        /// <summary>
        /// ��λĬ�Ͻؾ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("��λĬ�Ͻؾ�")]
        public decimal? WcIntercept { get; set; }
        /// <summary>
        /// JPH
        /// </summary>
        /// <returns></returns>
        [DisplayName("JPH")]
        public decimal? WspJPH { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public int? CacheQantity { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public int? CacheLimit { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������")]
        public int? HighestQantity { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������")]
        public int? LowestQantity { get; set; }
        /// <summary>
        /// Ԥ��λ
        /// </summary>
        /// <returns></returns>
        [DisplayName("Ԥ��λ")]
        public int? PreAlarmPoint { get; set; }
        /// <summary>
        /// ֹͣλ
        /// </summary>
        /// <returns></returns>
        [DisplayName("ֹͣλ")]
        public int? EndPoint { get; set; }
        /// <summary>
        /// ����ģʽ
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ģʽ")]
        public string RunningMode { get; set; }
        /// <summary>
        /// ��ʼ
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ʼ")]
        public decimal? StationBegion { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public decimal? StationEnd { get; set; }
        /// <summary>
        /// ǰ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("ǰ��������")]
        public int? HighestFrontCache { get; set; }
        /// <summary>
        /// ǰ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("ǰ��������")]
        public int? LowestFrontCache { get; set; }
        /// <summary>
        /// �󻺴�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�󻺴�����")]
        public int? HighestPostCache { get; set; }
        /// <summary>
        /// �󻺴�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�󻺴�����")]
        public int? LowestPostCache { get; set; }
        /// <summary>
        /// ������Ա����
        /// </summary>
        /// <returns></returns>
        [DisplayName("������Ա����")]
        public string StfId { get; set; }
        /// <summary>
        /// ������Ա���
        /// </summary>
        /// <returns></returns>
        [DisplayName("������Ա���")]
        public string StfCd { get; set; }
        /// <summary>
        /// ������Ա����
        /// </summary>
        /// <returns></returns>
        [DisplayName("������Ա����")]
        public string StfNm { get; set; }
        /// <summary>
        /// �������ֻ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������ֻ���")]
        public string Phn { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
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
            this.PlineId = CommonHelper.GetGuid;
            this.Enabled = "1";
            this.VersionNumber = "V1.0";
            this.CreTm = DateTime.Now;
            this.CreCd = ManageProvider.Provider.Current().UserId;
            this.CreNm = ManageProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.PlineId = KeyValue;
            this.MdfTm = DateTime.Now;
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}