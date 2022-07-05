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
    /// ��������¼��Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.23 11:01</date>
    /// </author>
    /// </summary>
    [Description("��������¼��Ϣ��")]
    [PrimaryKey("CarPastRecordId")]
    public class P_CarPastRecordInfo : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ������Ϣ��¼����
        /// </summary>
        /// <returns></returns>
        [DisplayName("������Ϣ��¼����")]
        public string CarPastRecordId { get; set; }
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
        /// AVIվ�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("AVIվ�����")]
        public string AviCatg { get; set; }
        /// <summary>
        /// AVIվ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("AVIվ������")]
        public string AviType { get; set; }
        /// <summary>
        /// �����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����")]
        public string BodyNo { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public string CarType { get; set; }
        /// <summary>
        /// �������Ϻ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������Ϻ�")]
        public string MatCd { get; set; }
        /// <summary>
        /// ����������ɫ
        /// </summary>
        /// <returns></returns>
        [DisplayName("����������ɫ")]
        public string CarColor1 { get; set; }
        /// <summary>
        /// �����ֹ���ɫ
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����ֹ���ɫ")]
        public string CarColor2 { get; set; }
        /// <summary>
        /// VIN��
        /// </summary>
        /// <returns></returns>
        [DisplayName("VIN��")]
        public string VIN { get; set; }
        /// <summary>
        /// ��ˮ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ˮ��")]
        public string SequenceNo { get; set; }
        
        /// <summary>
        /// ��װ�����ƻ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��װ�����ƻ����")]
        public string ProducePlanCd { get; set; }
        /// <summary>
        /// ����ȥ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ȥ��")]
        public int? CarRoute { get; set; }
        /// <summary>
        /// ����ȥ���������
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ȥ���������")]
        public string PlineId { get; set; }
        /// <summary>
        /// ����ȥ����߱��
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ȥ����߱��")]
        public string PlineCd { get; set; }
        /// <summary>
        /// ����ȥ���������
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ȥ���������")]
        public string PlineNm { get; set; }
        
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public int? PastType { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string PastDate { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ʱ��")]
        public DateTime? PastTime { get; set; }
        /// <summary>
        /// ����˳��
        /// </summary>
        /// <returns></returns>
        [DisplayName("����˳��")]
        public string PastNo { get; set; }
        /// <summary>
        /// ¼������
        /// </summary>
        /// <returns></returns>
        [DisplayName("¼������")]
        public int? RecordType { get; set; }
        /// <summary>
        /// �����ʶ
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����ʶ")]
        public int? SpecialTag { get; set; }
        /// <summary>
        /// ¼��ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("¼��ʱ��")]
        public DateTime? CreTm { get; set; }
        /// <summary>
        /// ¼����Ա
        /// </summary>
        /// <returns></returns>
        [DisplayName("¼����Ա")]
        public string CreStaff { get; set; }
        /// <summary>
        /// �Ƿ���������FASϵͳ
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ƿ���������FASϵͳ")]
        public int? IsBack { get; set; }
        
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

        //[DisplayName("ȥ��AVIId")]
        //public string ToAVIId { get; set; }

        //[DisplayName("ȥ��AVICd")]
        //public string ToAVICd { get; set; }

        //[DisplayName("ȥ��AVINm")]
        //public string ToAVINm { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.CarPastRecordId = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.CarPastRecordId = KeyValue;
                                            }
        #endregion
    }
}