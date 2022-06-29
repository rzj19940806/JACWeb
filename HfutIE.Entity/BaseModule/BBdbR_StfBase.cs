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
    /// ��Ա������Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.26 15:42</date>
    /// </author>
    /// </summary>
    [Description("��Ա������Ϣ��")]
    [PrimaryKey("StfId")]
    public class BBdbR_StfBase : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ��Ա����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ա����")]
        public string StfId { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string DepartmentID { get; set; }
        /// <summary>
        /// ��Ա���
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ա���")]
        public string StfCd { get; set; }
        /// <summary>
        /// ��Ա����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ա����")]
        public string StfNm { get; set; }
        /// <summary>
        /// �Ա�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ա�")]
        public string StfGndr { get; set; }
        /// <summary>
        /// �ֻ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ֻ���")]
        public string Phn { get; set; }
        /// <summary>
        /// ��ҵ΢�ź�
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ҵ΢�ź�")]
        public string Wechat { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public string Email { get; set; }
        /// <summary>
        /// ��Ա����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ա����")]
        public string StfTyp { get; set; }
        /// <summary>
        /// ��Աְλ
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Աְλ")]
        public string StfPosn { get; set; }
        /// <summary>
        /// ��Աְ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Աְ��")]
        public string StfTitl { get; set; }
        /// <summary>
        /// ��ԱͼƬ
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ԱͼƬ")]
        public string StfImg { get; set; }
        /// <summary>
        /// ͼƬ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("ͼƬ����")]
        public string ImgTyp { get; set; }
        /// <summary>
        /// �˺�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�˺�")]
        public string Account { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public string Pssw { get; set; }
        /// <summary>
        /// ��Կ
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Կ")]
        public string Sctkey { get; set; }
        /// <summary>
        /// �޸�����ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�޸�����ʱ��")]
        public DateTime? ChangePasswordDate { get; set; }
        /// <summary>
        /// ��¼����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��¼����")]
        public int? LogOnCount { get; set; }
        /// <summary>
        /// ���֮ǰ��¼ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("���֮ǰ��¼ʱ��")]
        public DateTime? PreviousVisit { get; set; }
        /// <summary>
        /// ����¼ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("����¼ʱ��")]
        public DateTime? LastVisit { get; set; }
        /// <summary>
        /// ����¼ip
        /// </summary>
        /// <returns></returns>
        [DisplayName("����¼ip")]
        public string LastLoginIp { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������")]
        public int? Online { get; set; }
        /// <summary>
        /// �汾��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�汾��")]
        public string VersionNumber { get; set; }
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
            this.StfId = CommonHelper.GetGuid;
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
            //this.StfId = KeyValue;
            this.VersionNumber = "V1.0";
            this.MdfTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}