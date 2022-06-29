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
    /// �ɼ��������Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.27 12:20</date>
    /// </author>
    /// </summary>
    [Description("�ɼ��������Ϣ��")]
    [PrimaryKey("GetItemId")]
    public class BBdbR_GetItemBase : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// �ɼ�������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ɼ�������")]
        public string GetItemId { get; set; }
        /// <summary>
        /// �ɼ�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ɼ�����")]
        public string GetItemCd { get; set; }
        /// <summary>
        /// �ɼ�������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ɼ�������")]
        public string GetItemNm { get; set; }
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
        /// �ɼ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ɼ������")]
        public string GetItemType { get; set; }
        /// <summary>
        /// �ɼ�������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ɼ�������")]
        public decimal? GetItemUpLimit { get; set; }
        /// <summary>
        /// �ɼ�������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ɼ�������")]
        public decimal? GetItemLowLimit { get; set; }
        /// <summary>
        /// �ɼ����׼ֵ
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ɼ����׼ֵ")]
        public decimal? GetItemValue { get; set; }
        /// <summary>
        /// �ɼ��������λ
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ɼ��������λ")]
        public string GetItemUnit { get; set; }
        /// <summary>
        /// �ɼ���˵��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ɼ���˵��")]
        public string GetItemExplain { get; set; }
        /// <summary>
        /// �ɼ���ʽ
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ɼ���ʽ")]
        public string GetItemTy { get; set; }
        /// <summary>
        /// �ɼ�Ƶ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ɼ�Ƶ��")]
        public string GetItemFq { get; set; }
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
            this.GetItemId = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.GetItemId = KeyValue;
                                            }
        #endregion
    }
}