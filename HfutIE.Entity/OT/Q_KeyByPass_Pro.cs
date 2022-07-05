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
    /// ���ؼ��󶨹��̱�
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.26 00:49</date>
    /// </author>
    /// </summary>
    [Description("���ؼ�ByPass���̱�")]
    [PrimaryKey("KeyByPassProId")]
    public class Q_KeyByPass_Pro : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ���ؼ��󶨹�������
        /// </summary>
        /// <returns></returns>
        [DisplayName("���ؼ�ByPass��������")]
        public string KeyByPassProId { get; set; }
        /// <summary>
        /// VIN��
        /// </summary>
        /// <returns></returns>
        [DisplayName("VIN��")]
        public string VIN { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������")]
        public string OrderCd { get; set; }
        /// <summary>
        /// ��ˮ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ˮ��")]
        public string SequenceNo { get; set; }
        /// <summary>
        /// �����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����")]
        public string BodyNo { get; set; }
        /// <summary>
        /// �������ϴ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������ϴ���")]
        public string ProductMatCd { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public string CarType { get; set; }
        /// <summary>
        /// ��ɫ
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ɫ")]
        public string CarColor1 { get; set; }
        /// <summary>
        /// ���ؼ�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("���ؼ�����")]
        public string BarCode { get; set; }
        /// <summary>
        /// ���ؼ���������
        /// </summary>
        /// <returns></returns>
        [DisplayName("���ؼ���������")]
        public string MatId { get; set; }
        /// <summary>
        /// ���ؼ����ϱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("���ؼ����ϱ��")]
        public string MatCd { get; set; }
        /// <summary>
        /// ���ؼ���������
        /// </summary>
        /// <returns></returns>
        [DisplayName("���ؼ���������")]
        public string MatNm { get; set; }
        /// <summary>
        /// ���ؼ���Ӧ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("���ؼ���Ӧ������")]
        public string SupplierId { get; set; }
        /// <summary>
        /// ���ؼ���Ӧ�̱��
        /// </summary>
        /// <returns></returns>
        [DisplayName("���ؼ���Ӧ�̱��")]
        public string SupplierCd { get; set; }
        /// <summary>
        /// ���ؼ���Ӧ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("���ؼ���Ӧ������")]
        public string SupplierNm { get; set; }
        /// <summary>
        /// ��˾����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��˾����")]
        public string CompanyId { get; set; }
        /// <summary>
        /// ��˾���
        /// </summary>
        /// <returns></returns>
        [DisplayName("��˾���")]
        public string CompanyCd { get; set; }
        /// <summary>
        /// ��˾����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��˾����")]
        public string CompanyNm { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string FacId { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�������")]
        public string FacCd { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string FacNm { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string WorkshopId { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������")]
        public string WorkshopCd { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string WorkshopNm { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string WorkSectionId { get; set; }
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
        public string PlineId { get; set; }
        /// <summary>
        /// ���߱��
        /// </summary>
        /// <returns></returns>
        [DisplayName("���߱��")]
        public string PlineCd { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string PlineNm { get; set; }
        /// <summary>
        /// ��λ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��λ����")]
        public string WcId { get; set; }
        /// <summary>
        /// ��λ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("��λ���")]
        public string WcCd { get; set; }
        /// <summary>
        /// ��λ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��λ����")]
        public string WcNm { get; set; }
        /// <summary>
        /// ��λ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��λ����")]
        public string PostId { get; set; }
        /// <summary>
        /// ��λ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("��λ���")]
        public string PostCd { get; set; }
        /// <summary>
        /// ��λ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��λ����")]
        public string PostNm { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string TeamId { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������")]
        public string TeamCd { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string TeamNm { get; set; }
        /// <summary>
        /// ���鸺��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("���鸺��������")]
        public string MgrId { get; set; }
        /// <summary>
        /// ���鸺���˱��
        /// </summary>
        /// <returns></returns>
        [DisplayName("���鸺���˱��")]
        public string MgrCd { get; set; }
        /// <summary>
        /// ���鸺��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("���鸺��������")]
        public string MgrNm { get; set; }
        /// <summary>
        /// ��Ա����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ա����")]
        public string StfId { get; set; }
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
        /// �ɼ�ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ɼ�ʱ��")]
        public DateTime? Datetime { get; set; }
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
            KeyByPassProId = CommonHelper.GetGuid;
            Datetime = DateTime.Now;
            isFile = 0;
            Enabled = "1";
        }
        #endregion
    }
}