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
    /// �����������������̱�
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.12.25 10:08</date>
    /// </author>
    /// </summary>
    [Description("�����������������̱�")]
    [PrimaryKey("CarInspectionOutputId")]
    public class Q_CarQualityOutput_Pro : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// �����ʼ���������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����ʼ���������")]
        public string CarInspectionOutputId { get; set; }
        /// <summary>
        /// ����λ�ʼ�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����λ�ʼ�����")]
        public string CarQualityInspectionId { get; set; }
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
        /// �����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����")]
        public string VIN { get; set; }
        /// <summary>
        /// �����������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����������")]
        public string CarComponentId { get; set; }
        /// <summary>
        /// ����λ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����λ����")]
        public string CarPositionId { get; set; }
        /// <summary>
        /// ����λ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("����λ��������")]
        public string CarPositionGroupId { get; set; }
        /// <summary>
        /// ����������ձ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("����������ձ���")]
        public string CarComponentCd { get; set; }
        /// <summary>
        /// �����������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����������")]
        public string CarComponentNm { get; set; }
        /// <summary>
        /// �����������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����������")]
        public string CarComponentSimpleCd { get; set; }
        /// <summary>
        /// ȱ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("ȱ������")]
        public string DefectId { get; set; }
        /// <summary>
        /// ȱ����������
        /// </summary>
        /// <returns></returns>
        [DisplayName("ȱ����������")]
        public string DefectCatgId { get; set; }
        /// <summary>
        /// ȱ�����ͷ�������
        /// </summary>
        /// <returns></returns>
        [DisplayName("ȱ�����ͷ�������")]
        public string DefectCatgGroupId { get; set; }
        /// <summary>
        /// ȱ�ݱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("ȱ�ݱ��")]
        public string DefectCd { get; set; }
        /// <summary>
        /// ȱ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("ȱ������")]
        public string DefectNm { get; set; }
        /// <summary>
        /// ȱ�ݷ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("ȱ�ݷ���")]
        public string DefectAnalysis { get; set; }
        /// <summary>
        /// �����Ա���
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����Ա���")]
        public string StfCd { get; set; }
        /// <summary>
        /// �����Ա����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����Ա����")]
        public string StfNm { get; set; }
        /// <summary>
        /// �ʼ�¼��ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ʼ�¼��ʱ��")]
        public string QualityInspectTm { get; set; }
        /// <summary>
        /// ά����Ա���
        /// </summary>
        /// <returns></returns>
        [DisplayName("ά����Ա���")]
        public string WStfCd { get; set; }
        /// <summary>
        /// ά����Ա����
        /// </summary>
        /// <returns></returns>
        [DisplayName("ά����Ա����")]
        public string WStfNm { get; set; }
        /// <summary>
        /// ά��ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("ά��ʱ��")]
        public string WxTm { get; set; }
        /// <summary>
        /// ������������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������������")]
        public decimal? OutputValue { get; set; }
        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        [DisplayName("����������")]
        public string OutputResult { get; set; }
        /// <summary>
        /// ������Ա���
        /// </summary>
        /// <returns></returns>
        [DisplayName("������Ա���")]
        public string XStfCd { get; set; }
        /// <summary>
        /// ������Ա����
        /// </summary>
        /// <returns></returns>
        [DisplayName("������Ա����")]
        public string XStfNm { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ʱ��")]
        public string OutputTime { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������")]
        public int? ReinspectionNumber { get; set; }
        /// <summary>
        /// ��ʷ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ʷ����")]
        public string HistoryId { get; set; }
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
            this.CarInspectionOutputId = CommonHelper.GetGuid;
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
            this.CarInspectionOutputId = KeyValue;
            this.MdfTm = DateTime.Now.ToString();
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}