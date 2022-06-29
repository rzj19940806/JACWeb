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
    /// ���նλ�����Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.24 17:14</date>
    /// </author>
    /// </summary>
    [Description("���նλ�����Ϣ��")]
    [PrimaryKey("WorkSectionId")]
    public class BBdbR_WorkSectionBase : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string WorkSectionId { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string WorkshopId { get; set; }
        /// <summary>
        /// ���α��
        /// </summary>
        /// <returns></returns>
        [DisplayName("���α��")]
        public string WorkSectionCd { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string WorkSectionNm { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string WorkSectionTy { get; set; }
        /// <summary>
        /// JPH
        /// </summary>
        /// <returns></returns>
        [DisplayName("JPH")]
        public int? WspJPH { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public int? CacheQantity { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public int? CacheLimit { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������")]
        public int? HighestQantity { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������")]
        public int? LowestQantity { get; set; }
        /// <summary>
        /// ������Ա����
        /// </summary>
        /// <returns></returns>
        [DisplayName("������Ա����")]
        public string StfId { get; set; }
        /// <summary>
        /// ������Ա���
        /// </summary>
        /// <returns></returns>
        [DisplayName("������Ա���")]
        public string StfCd { get; set; }
        /// <summary>
        /// ������Ա����
        /// </summary>
        /// <returns></returns>
        [DisplayName("������Ա����")]
        public string StfNm { get; set; }
        /// <summary>
        /// �������ֻ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������ֻ���")]
        public string Phn { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string WorkSecDsc { get; set; }
        /// <summary>
        /// ��Ч��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ч��")]
        public string Enabled { get; set; }
        /// <summary>
        /// �汾��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�汾��")]
        public string VersionNumber { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ʱ��")]
        public string CreTm { get; set; }
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
        public string MdfTm { get; set; }
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
            this.WorkSectionId = CommonHelper.GetGuid;
            this.VersionNumber = "V1.0";
            this.CreTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.Enabled = "1";
            this.CreCd = ManageProvider.Provider.Current().UserId;
            this.CreNm = ManageProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            //this.ConfigId = KeyValue;
            this.VersionNumber = "V1.0";
            this.MdfTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}