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
    /// ͣ����־��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.12.06 15:17 ckl</date>
    /// </author>
    /// </summary>
    [Description("ͣ����־��")]
    [PrimaryKey("RecId")]
    public class BPIE_SLDoc : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ��¼����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��¼����")]
        public string RecId { get; set; }
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
        public string PlineNm { get; set; }
        /// <summary>
        /// ͣ��״̬
        /// </summary>
        /// <returns></returns>
        [DisplayName("ͣ��״̬")]
        public string State { get; set; }
        /// <summary>
        /// ͣ����Դ
        /// </summary>
        /// <returns></returns>
        [DisplayName("ͣ����Դ")]
        public string SLSource { get; set; }
        /// <summary>
        /// ͣ�߿�ʼʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("ͣ�߿�ʼʱ��")]
        public DateTime? SLStrtTm { get; set; }
        /// <summary>
        /// ͣ�߽���ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("ͣ�߽���ʱ��")]
        public DateTime? SLCmplTm { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ʱ��")]
        public Decimal? SLContTm { get; set; }
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
            this.RecId = CommonHelper.GetGuid;
            this.Enabled = "1";
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