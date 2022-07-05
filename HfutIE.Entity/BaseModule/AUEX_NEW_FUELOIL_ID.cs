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
    /// ȼ�ͱ�ʶ��Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.14 17:00</date>
    /// </author>
    /// </summary>
    [Description("ȼ�ͱ�ʶ��Ϣ��")]
    [PrimaryKey("GID")]
    public class AUEX_NEW_FUELOIL_ID : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public string GID { get; set; }
        /// <summary>
        /// ��ʶ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ʶ����")]
        public string IDCODE { get; set; }
        /// <summary>
        /// ������ҵ
        /// </summary>
        /// <returns></returns>
        [DisplayName("������ҵ")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MANUFACTURER { get; set; }
        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        [DisplayName("����������")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string TRANSMISSION_TYPE { get; set; }
        /// <summary>
        /// ��Դ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Դ����")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string FUEL_TYPE { get; set; }
        /// <summary>
        /// ������������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������������")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ENTIRE_QUALITY { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string DISPLACEMENT { get; set; }
        /// <summary>
        /// ������������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������������")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string DESIGN_QUALITY { get; set; }
        /// <summary>
        /// ��󾻹���
        /// </summary>
        /// <returns></returns>
        [DisplayName("��󾻹���")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MAX_POWERS { get; set; }
        /// <summary>
        /// ������ʽ
        /// </summary>
        /// <returns></returns>
        [DisplayName("������ʽ")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string DRIVE_TYPE { get; set; }
        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <returns></returns>
        [DisplayName("������Ϣ")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ADDTIONAL_INFORMATION { get; set; }
        /// <summary>
        /// ���������ֵ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("���������ֵ����")]
        public string POWER_DRIVE_MOTOR { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string URBAN_OPERATING_MODE { get; set; }
        /// <summary>
        /// �ۺϹ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ۺϹ���")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string INTEGRATED_OPERATING_MODE { get; set; }
        /// <summary>
        /// �ۺϹ�������������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ۺϹ�������������")]
        public string INTEGRATED_CONSUMPTION { get; set; }
        /// <summary>
        /// ���ܵ���ȼ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("���ܵ���ȼ��������")]
        public string ELECTRI_CONSUMPTION { get; set; }
        /// <summary>
        /// �н�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�н�����")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string SUBURBAN_OPERATING_MODE { get; set; }
        /// <summary>
        /// ����ֵ
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ֵ")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LEADVALUE { get; set; }
        /// <summary>
        /// ��ֵ
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ֵ")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LIMIT { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string RECORD_NUMBER { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string OPENING_DATE { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������")]
        public string GB_NO { get; set; }
        /// <summary>
        /// �����ͺ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����ͺ�")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PRODUCT_VEHICLE_CODE { get; set; }
        /// <summary>
        /// �������ͺ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������ͺ�")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ENGINE_MODEL { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������")]
        public string RANGE { get; set; }
        /// <summary>
        /// ��Ч��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ч��")]
        public string Enabled { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������")]
        public string CREATE_ID { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ʱ��")]
        public DateTime? CREATE_DATE { get; set; }
        /// <summary>
        /// �޸���
        /// </summary>
        /// <returns></returns>
        [DisplayName("�޸���")]
        public string MODIFY_ID { get; set; }
        /// <summary>
        /// �޸�ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�޸�ʱ��")]
        public DateTime? MODIFY_DATE { get; set; }
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
            this.CREATE_DATE = DateTime.Now;
            this.CREATE_ID = ManageProvider.Provider.Current().UserId;
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.MODIFY_DATE = DateTime.Now;
            this.MODIFY_ID = ManageProvider.Provider.Current().UserId;

        }
        #endregion
    }
}