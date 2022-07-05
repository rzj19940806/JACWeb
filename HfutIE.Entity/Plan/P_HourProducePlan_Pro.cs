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
    /// Сʱ�ƻ����չ��̱�
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.29 11:12</date>
    /// </author>
    /// </summary>
    [Description("Сʱ�ƻ����չ��̱�")]
    [PrimaryKey("HourPlanProId")]
    public class P_HourProducePlan_Pro : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// Сʱ�ƻ���������
        /// </summary>
        /// <returns></returns>
        [DisplayName("Сʱ�ƻ���������")]
        public string HourPlanProId { get; set; }
        /// <summary>
        /// �ƻ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ƻ����")]
        public string ProducePlanCd { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������")]
        public string OrderCd { get; set; }
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
        /// �����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����")]
        public string BodyNo { get; set; }
        /// <summary>
        /// �������Ϻ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������Ϻ�")]
        public string MatCd { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public string CarType { get; set; }
        /// <summary>
        /// ��ɫ
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ɫ")]
        public string CarColor1 { get; set; }
        /// <summary>
        /// Ԥ������ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("Ԥ������ʱ��")]
        public DateTime? PlanTime { get; set; }
        /// <summary>
        /// ����״̬
        /// </summary>
        /// <returns></returns>
        [DisplayName("����״̬")]
        public int? OrderStatus { get; set; }
        /// <summary>
        /// �ƻ�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ƻ�����")]
        public string PlanDsc { get; set; }
        /// <summary>
        /// �ƻ�����ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ƻ�����ʱ��")]
        public DateTime? RecieveTm { get; set; }
        /// <summary>
        /// ���������
        /// </summary>
        /// <returns></returns>
        [DisplayName("���������")]
        public int? RecieveSeq { get; set; }
        /// <summary>
        /// ���������
        /// </summary>
        /// <returns></returns>
        [DisplayName("���������")]
        public int? Seq { get; set; }
        /// <summary>
        /// �Ƿ�ת��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ƿ�ת��")]
        public int? IsFile { get; set; }
        /// <summary>
        /// ��Ч��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ч��")]
        public string Enabled { get; set; }
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
            this.HourPlanProId = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.HourPlanProId = KeyValue;
                                            }
        #endregion
    }
}