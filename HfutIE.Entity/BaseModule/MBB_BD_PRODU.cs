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
    /// ������Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.14 16:56</date>
    /// </author>
    /// </summary>
    [Description("������Ϣ��")]
    [PrimaryKey("GID")]
    public class MBB_BD_PRODU : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public string GID { get; set; }
        /// <summary>
        /// ��Ʒ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ʒ���")]
        public string CODE { get; set; }
        /// <summary>
        /// ��Ʒ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ʒ����")]
        public string NAME { get; set; }
        /// <summary>
        /// �����ͺ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����ͺ�")]
        public string UDA19 { get; set; }
        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        [DisplayName("����������")]
        public string UDA2 { get; set; }
        /// <summary>
        /// �������ͺ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������ͺ�")]
        public string UDA1 { get; set; }
        /// <summary>
        /// ��������󾻹���
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������󾻹���")]
        public string UDA3 { get; set; }
        /// <summary>
        /// ��󾻹���ת��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��󾻹���ת��")]
        public string MAX_POWER_SPEED { get; set; }
        /// <summary>
        /// �������������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������������")]
        public Single? TOTAL_WEIGHT { get; set; }
        /// <summary>
        /// ���������ͺ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("���������ͺ�")]
        public string UDA4 { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public Single? CAPACITY { get; set; }
        /// <summary>
        /// �������ͺ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������ͺ�")]
        public string UDA16 { get; set; }
        /// <summary>
        /// �����ͺ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����ͺ�")]
        public string UDA21 { get; set; }
        /// <summary>
        /// ���⳵������
        /// </summary>
        /// <returns></returns>
        [DisplayName("���⳵������")]
        public string UDA9 { get; set; }
        /// <summary>
        /// ��;
        /// </summary>
        /// <returns></returns>
        [DisplayName("��;")]
        public string UDA10 { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public DateTime? PLANNED_WORK_ORDER { get; set; }
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