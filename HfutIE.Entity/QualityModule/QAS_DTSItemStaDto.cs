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
    /// DTS����������Ϣ��
    /// <author>
    ///		<name>CHFAS</name>
    ///		<date>2021.07.06 12:00</date>
    /// </author>
    /// </summary>
    [Description("DTS����������Ϣ��")]
    [PrimaryKey("RecordId")]
    public class QAS_DTSItemStaDto : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public string RecordId { get; set; }
        /// <summary>
        /// ����ID
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ID")]
        public string ParId { get; set; }
        /// <summary>
        /// ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("���")]
        public string SerialNumber { get; set; }
        /// <summary>
        /// ��׼ֵ
        /// </summary>
        /// <returns></returns>
        [DisplayName("��׼ֵ")]
        public string BaseValue { get; set; }
        /// <summary>
        /// ����ֵ
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ֵ")]
        public string TolValue { get; set; }
        /// <summary>
        /// TOL+
        /// </summary>
        /// <returns></returns>
        [DisplayName("TOL+")]
        public string TOLUp { get; set; }
        /// <summary>
        /// TOL-
        /// </summary>
        /// <returns></returns>
        [DisplayName("TOL-")]
        public string TOLDown { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public string Direction { get; set; }
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