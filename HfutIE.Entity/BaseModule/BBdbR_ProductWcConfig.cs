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
    /// ��Ʒ��λ���ñ�
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.13 10:18</date>
    /// </author>
    /// </summary>
    [Description("��Ʒ��λ���ñ�")]
    [PrimaryKey("ProductClassId")]
    public class BBdbR_ProductWcConfig : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ��Ʒ��λ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ʒ��λ����")]
        public string ProductClassId { get; set; }
        /// <summary>
        /// ��Ʒ����������������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ʒ����������������")]
        public string MatId { get; set; }
        /// <summary>
        /// ��Ʒ��š����ϱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ʒ��š����ϱ��")]
        public string MatCd { get; set; }
        /// <summary>
        /// ��Ʒ���ơ���������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ʒ���ơ���������")]
        public string MatNm { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string PlineId { get; set; }
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
        /// ͼƬ���ơ��ĵ�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("ͼƬ���ơ��ĵ�����")]
        public string FileNm { get; set; }
        /// <summary>
        /// �ĵ�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ĵ�����")]
        public string ConfigId { get; set; }
        /// <summary>
        /// ����ͼƬ�������ĵ���ͼƬ��ţ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ͼƬ�������ĵ���ͼƬ��ţ�")]
        public string FileCd { get; set; }
        /// <summary>
        /// ����λ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("����λ��")]
        public int? LockStationNum { get; set; }
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
            this.ProductClassId = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.ProductClassId = KeyValue;
                                            }
        #endregion
    }
}