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
    /// �ӿڻ�����Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.12.14 21:17</date>
    /// </author>
    /// </summary>
    [Description("�ӿڻ�����Ϣ��")]
    [PrimaryKey("InterfaceId")]
    public class BBdbR_Interface : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public string InterfaceId { get; set; }
        /// <summary>
        /// �ӿڱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ӿڱ��")]
        public string InterfaceCd { get; set; }
        /// <summary>
        /// �ӿ�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ӿ�����")]
        public string InterfaceNm { get; set; }
        /// <summary>
        /// �ӿ�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ӿ�����")]
        public string InterfaceType { get; set; }
        /// <summary>
        /// ���ͷ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("���ͷ�")]
        public string Sender { get; set; }
        /// <summary>
        /// ���շ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("���շ�")]
        public string Receiver { get; set; }
        /// <summary>
        /// ִ�з�ʽ
        /// </summary>
        /// <returns></returns>
        [DisplayName("ִ�з�ʽ")]
        public string ExecutionMode { get; set; }
        /// <summary>
        /// ִ��ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("ִ��ʱ��")]
        public DateTime? ExecutionTime { get; set; }
        /// <summary>
        /// ִ�м��(����)
        /// </summary>
        /// <returns></returns>
        [DisplayName("ִ�м��(����)")]
        public int InterfaceTime { get; set; }
        /// <summary>
        /// �ϴ�ִ��ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ϴ�ִ��ʱ��")]
        public DateTime? LastTime { get; set; }
        /// <summary>
        /// �´�ִ��ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�´�ִ��ʱ��")]
        public DateTime? NextTime { get; set; }
        /// <summary>
        /// ״̬
        /// </summary>
        /// <returns></returns>
        [DisplayName("״̬")]
        public string Status { get; set; }
        /// <summary>
        /// SQL���
        /// </summary>
        /// <returns></returns>
        [DisplayName("SQL���")]
        public string Sql { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ע")]
        public string Rem { get; set; }
        /// <summary>
        /// ��Ч��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ч��")]
        public string Enabled { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.InterfaceId = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.InterfaceId = KeyValue;
                                            }
        #endregion
    }
}