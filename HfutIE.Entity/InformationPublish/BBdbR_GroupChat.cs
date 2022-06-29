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
    /// Ⱥ�Ļ�����
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.03.25 13:34</date>
    /// </author>
    /// </summary>
    [Description("Ⱥ�Ļ�����")]
    [PrimaryKey("GroupchatId")]
    public class BBdbR_GroupChat : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ȺID
        /// </summary>
        /// <returns></returns>
        [DisplayName("ȺID")]
        public string GroupchatId { get; set; }
        /// <summary>
        /// Ⱥ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("Ⱥ����")]
        public string GroupchatNm { get; set; }
        /// <summary>
        /// Ⱥ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("Ⱥ��")]
        public string Qrcode { get; set; }
        /// <summary>
        /// Ⱥ��ַ
        /// </summary>
        /// <returns></returns>
        [DisplayName("Ⱥ��ַ")]
        public string GroupchatAddr { get; set; }
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
            this.GroupchatId = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.GroupchatId = KeyValue;
                                            }
        #endregion
    }
}