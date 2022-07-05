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
    /// ���������̱�
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.23 11:20</date>
    /// </author>
    /// </summary>
    [Description("���������̱�")]
    [PrimaryKey("CarPastProId")]
    public class P_CarPastInfo_Pro : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ���������Ϣ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("���������Ϣ����")]
        public string CarPastProId { get; set; }
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
        /// ������ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("������ʱ��")]
        public DateTime? ArrivalTm { get; set; }
        /// <summary>
        /// �����ƻ�У�˽��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����ƻ�У�˽��")]
        public int? PlanCheckResult { get; set; }
        /// <summary>
        /// �������У�˽��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������У�˽��")]
        public int? BodyCheckResult { get; set; }
        /// <summary>
        /// ʧ��ԭ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("ʧ��ԭ��")]
        public string FaultReason { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public int? CarPatType { get; set; }
        /// <summary>
        /// �������״̬
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������״̬")]
        public int? CarSchedulStatus { get; set; }
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
        /// ����ȥ��AVI����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ȥ��AVI����")]
        public string ToAVIId { get; set; }
        /// <summary>
        /// ����ȥ��AVI���
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ȥ��AVI���")]
        public string ToAVICd { get; set; }
        /// <summary>
        /// ����ȥ��AVI����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ȥ��AVI����")]
        public string ToAVINm { get; set; }
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
        /// �Ƿ�д��PLC
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ƿ�д��PLC")]
        public int? IsWrite { get; set; }
        /// <summary>
        /// д��PLCʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("д��PLCʱ��")]
        public DateTime? WriteTime { get; set; }
        /// <summary>
        /// �Ƿ�ת��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ƿ�ת��")]
        public int? IsFile { get; set; }
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
            this.CarPastProId = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.CarPastProId = KeyValue;
                                            }
        #endregion
    }
}