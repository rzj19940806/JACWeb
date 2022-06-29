//=====================================================================================
// All Rights Reserved , Copyright @ Learun 2017
// Software Developers @ Learun 2017
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
    /// AD_MAC_INFO������Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.11.18 15:09</date>
    /// </author>
    /// </summary>
    [Description("AD_MAC_INFO")]
    [PrimaryKey("MAC_KEY")]
    public class AD_MAC_INFO : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("MAC_KEY����")]
        public string mac_key { get; set; }
        /// <summary>
        /// PAD���
        /// </summary>
        /// <returns></returns>
        [DisplayName("PAD���")]
        public string mac_code { get; set; }
        /// <summary>
        /// PAD����
        /// </summary>
        /// <returns></returns>
        [DisplayName("PAD����")]
        public string mac_name { get; set; }
        /// <summary>
        /// ��Ա���
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Աcode")]
        public string user_code { get; set; }
        /// <summary>
        /// ��Ա����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Աname")]
        public string user_name { get; set; }
        /// <summary>
        /// ��Ч
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ч")]
        public int? enable { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ʱ��")]
        public DateTime? creation_time { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ע")]
        public string mark { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.mac_key = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.mac_key = KeyValue;
                                            }
        #endregion
    }
}