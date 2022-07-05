//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2022
// Software Developers @ HfutIE 2022
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
    /// ���ϼ�ע���ñ�
    /// <author>
    ///		<name>she</name>
    ///		<date>2022.05.26 15:13</date>
    /// </author>
    /// </summary>
    [Description("���ϼ�ע���ñ�")]
    [PrimaryKey("Id")]
    public class BBDBR_MatJZConfig : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ���ϼ�ע��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("���ϼ�ע��������")]
        public string Id { get; set; }
        /// <summary>
        /// ���ϱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("���ϱ��")]
        public string MatCd { get; set; }
        /// <summary>
        /// ��ע����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ע����")]
        public string JZType { get; set; }
        /// <summary>
        /// ��ע��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ע��")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public decimal? JZNumber { get; set; }
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
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Rem { get; set; }
        /// <summary>
        /// �����ֶ�1
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����ֶ�1")]
        public string RsvFld1 { get; set; }
        /// <summary>
        /// �����ֶ�2
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����ֶ�2")]
        public string RsvFld2 { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.Id = CommonHelper.GetGuid;
            this.Enabled = "1";
            this.CreTm = DateTime.Now;
            this.CreCd = ManageProvider.Provider.Current().UserId;
            this.CreNm = ManageProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.Id = KeyValue;
            this.MdfTm = DateTime.Now;
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}