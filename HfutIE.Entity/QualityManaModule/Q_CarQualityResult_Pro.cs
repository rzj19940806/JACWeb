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
    /// ����λ�ʼ�����¼���̱�
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.11.18 20:08</date>
    /// </author>
    /// </summary>
    [Description("����λ�ʼ�����¼���̱�")]
    [PrimaryKey("CarQualityResultId")]
    public class Q_CarQualityResult_Pro : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ����λ�ʼ�������
        /// </summary>
        /// <returns></returns>
        [DisplayName("����λ�ʼ�������")]
        public string CarQualityResultId { get; set; }
        /// <summary>
        /// �ʿص�������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ʿص�������")]
        public string QualityCheckPointGroupId { get; set; }
        /// <summary>
        /// �ʿص�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ʿص�����")]
        public string QualityCheckPointGroupCd { get; set; }
        /// <summary>
        /// �ʿص�������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ʿص�������")]
        public string QualityCheckPointGroupNm { get; set; }
        /// <summary>
        /// �ʿص�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ʿص�����")]
        public string QualityCheckPointId { get; set; }
        /// <summary>
        /// �ʿص���
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ʿص���")]
        public string QualityCheckPointCd { get; set; }
        /// <summary>
        /// �ʿص�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ʿص�����")]
        public string QualityCheckPointNm { get; set; }
        /// <summary>
        /// ¼����Ա���
        /// </summary>
        /// <returns></returns>
        [DisplayName("¼����Ա���")]
        public string StfCd { get; set; }
        /// <summary>
        /// ¼����Ա����
        /// </summary>
        /// <returns></returns>
        [DisplayName("¼����Ա����")]
        public string StfNm { get; set; }
        /// <summary>
        /// �ʼ�¼��ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ʼ�¼��ʱ��")]
        public string QualityInspectTm { get; set; }
        /// <summary>
        /// VIN
        /// </summary>
        /// <returns></returns>
        [DisplayName("VIN")]
        public string VIN { get; set; }
        /// <summary>
        /// �ʼ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ʼ���")]
        public string QualityResult { get; set; }
        /// <summary>
        /// �Ƿ�ת��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ƿ�ת��")]
        public int? isFile { get; set; }
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
            this.CarQualityResultId = CommonHelper.GetGuid;
            this.Enabled = "1";
            this.CreTm = DateTime.Now.ToString();
            this.CreCd = ManageProvider.Provider.Current().UserId;
            this.CreNm = ManageProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.CarQualityResultId = KeyValue;
            this.MdfTm = DateTime.Now.ToString();
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}