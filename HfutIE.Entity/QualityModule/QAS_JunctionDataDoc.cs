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
    /// ���ӵ�����������ݵ�����
    /// <author>
    ///		<name>CHFAS</name>
    ///		<date>2021.07.20 12:00</date>
    /// </author>
    /// </summary>
    [Description("���ӵ�����������ݵ�����")]
    [PrimaryKey("RecordId")]
    public class QAS_JunctionDataDoc : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public string RecordId { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public string CarType { get; set; }
        /// <summary>
        /// ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("���")]
        public string Code { get; set; }
        /// <summary>
        /// ItemNm
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����")]
        public string ItemNm { get; set; }
        /// <summary>
        /// ��λ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��λ����")]
        public string WcNm { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������")]
        public string PartNm { get; set; }
        /// <summary>
        /// CC/SC
        /// </summary>
        /// <returns></returns>
        [DisplayName("CC/SC")]
        public string CCSC { get; set; }
        /// <summary>
        /// <summary>
        /// ͷ�߱�׼
        /// </summary>
        /// <returns></returns>
        [DisplayName("ͷ�߱�׼")]
        public string HeadHeghitSta { get; set; }
        /// <summary>
        /// ͷ����϶
        /// </summary>
        /// <returns></returns>
        [DisplayName("ͷ����϶")]
        public string HeadGapSta { get; set; }
        /// <summary>
        /// ��λ
        /// </summary>
        /// <returns></returns>
        [DisplayName("��λ")]
        public string Unit { get; set; }
        /// <summary>
        /// ���ȱ�׼
        /// </summary>
        /// <returns></returns>
        [DisplayName("���ȱ�׼")]
        public string LengthSta { get; set; }/// <summary>
        /// š��Ť��
        /// </summary>
        /// <returns></returns>
        [DisplayName("š��Ť��")]
        public string TightenTOR { get; set; }
        /// ����λ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("����λ��")]
        public string SpotLocation { get; set; }
        /// <summary>
        /// ���˱�׼ֵ
        /// </summary>
        /// <returns></returns>
        [DisplayName("���˱�׼ֵ")]
        public string SpotStaValue { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string Category { get; set; }
        /// <summary>
        /// �����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����")]
        public string BodyNo { get; set; }
        /// <summary>
        /// ���ֵ
        /// </summary>
        /// <returns></returns>
        [DisplayName("���ֵ")]
        public string CheckValue { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������")]
        public DateTime? CheckTm { get; set; }
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
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ʱ��")]
        public DateTime? UpdateTm { get; set; }
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
            this.RecordId = CommonHelper.GetGuid;
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.RecordId = KeyValue;
        }
        #endregion
    }
}