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
    /// ȱ����ϸ��Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.10.09 19:51</date>
    /// </author>
    /// </summary>
    [Description("ȱ����ϸ��Ϣ��")]
    [PrimaryKey("DefectId")]
    public class BBdbR_DefectCatgDeTail : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
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
        /// DefectCatgGroupId
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
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public string Dsc { get; set; }
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
        /// RsvFld1
        /// </summary>
        /// <returns></returns>
        [DisplayName("RsvFld1")]
        public string RsvFld1 { get; set; }
        /// <summary>
        /// RsvFld2
        /// </summary>
        /// <returns></returns>
        [DisplayName("RsvFld2")]
        public string RsvFld2 { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.DefectId = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.DefectId = KeyValue;
                                            }
        #endregion
    }
}