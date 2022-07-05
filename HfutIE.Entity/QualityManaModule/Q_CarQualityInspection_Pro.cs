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
    /// ����λ�ʼ���̱�
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.11.08 20:38</date>
    /// </author>
    /// </summary>
    [Description("����λ�ʼ���̱�")]
    [PrimaryKey("CarQualityInspectionId")]
    public class Q_CarQualityInspection_Pro : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ����λ�ʼ�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����λ�ʼ�����")]
        public string CarQualityInspectionId { get; set; }
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
        /// �����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����")]
        public string BodyNo { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public string CarType { get; set; }
        /// <summary>
        /// �������Ϻ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������Ϻ�")]
        public string MatCd { get; set; }
        /// <summary>
        /// ����������ɫ
        /// </summary>
        /// <returns></returns>
        [DisplayName("����������ɫ")]
        public string CarColor1 { get; set; }
        /// <summary>
        /// �����ֹ���ɫ
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����ֹ���ɫ")]
        public string CarColor2 { get; set; }
        /// <summary>
        /// VIN��
        /// </summary>
        /// <returns></returns>
        [DisplayName("VIN��")]
        public string VIN { get; set; }
        /// <summary>
        /// ��ˮ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ˮ��")]
        public string SequenceNo { get; set; }
        /// <summary>
        /// Ϳװ�����ƻ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("Ϳװ�����ƻ����")]
        public string ProducePlanCd { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������")]
        public string OrderCd { get; set; }
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
        /// �ʼ�¼��ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ʼ�¼��ʱ��")]
        public string QualityInspectTm { get; set; }
        /// <summary>
        /// �ʼ�¼����Ա���
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ʼ�¼����Ա���")]
        public string StfCd { get; set; }
        /// <summary>
        /// �ʼ�¼����Ա����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ʼ�¼����Ա����")]
        public string StfNm { get; set; }
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
            this.CarQualityInspectionId = CommonHelper.GetGuid;
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.CarQualityInspectionId = KeyValue;
        }
        #endregion
    }
}