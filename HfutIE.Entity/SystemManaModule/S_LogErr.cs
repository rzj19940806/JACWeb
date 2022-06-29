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
    /// ϵͳ�쳣��־��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.04.03 09:50</date>
    /// </author>
    /// </summary>
    [Description("ϵͳ�쳣��־��")]
    [PrimaryKey("LogErrId")]
    public class S_LogErr : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public string LogErrId { get; set; }
        /// <summary>
        /// ��¼ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��¼ʱ��")]
        public DateTime? RecTm { get; set; }
        /// <summary>
        /// ������=>�쳣��ջ
        /// </summary>
        /// <returns></returns>
        [DisplayName("������=>�쳣��ջ")]
        public string MethNm { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������Դ")]
        public string ErrFrom { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string ErrDetl { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ע")]
        public string Rem { get; set; }        
        /// <summary>
        /// Ԥ���ֶ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("Ԥ���ֶ�")]
        public string RsvFld1 { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.LogErrId = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.LogErrId = KeyValue;
                                            }
        #endregion
    }
}