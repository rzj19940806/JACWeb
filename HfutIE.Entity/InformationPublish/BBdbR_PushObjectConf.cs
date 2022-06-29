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
    /// ��Ϣ���Ͷ������ñ�
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.03.24 09:38</date>
    /// </author>
    /// </summary>
    [Description("��Ϣ���Ͷ������ñ�")]
    [PrimaryKey("RecId")]
    public class BBdbR_PushObjectConf : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public string RecId { get; set; }
        /// <summary>
        /// �������ͱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������ͱ��")]
        public string InforTypCd { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string InforTyp { get; set; }
        /// <summary>
        /// ���Ͷ�������
        /// </summary>
        /// <returns></returns>
        [DisplayName("���Ͷ�������")]
        public string ObjectTyp { get; set; }
        /// <summary>
        /// ���Ͷ���Id
        /// </summary>
        /// <returns></returns>
        [DisplayName("���Ͷ���Id")]
        public string ObjectId { get; set; }
        /// <summary>
        /// ���͵ȼ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("���͵ȼ�")]
        public string PushRank { get; set; }
        /// <summary>
        /// ���ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("���ʱ��")]
        public decimal? IntvlTm { get; set; }
        /// <summary>
        /// ʱ����λ
        /// </summary>
        /// <returns></returns>
        [DisplayName("ʱ����λ")]
        public string TmUnit { get; set; }
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
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.RecId = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.RecId = KeyValue;
                                            }
        #endregion
    }
}