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
    /// ��ӡ������Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2022.04.22 10:42</date>
    /// </author>
    /// </summary>
    [Description("��ӡ������Ϣ��")]
    [PrimaryKey("Id")]
    public class BBdbR_Print : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public string Id { get; set; }
        /// <summary>
        /// ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("���")]
        public string PrintCd { get; set; }
        /// <summary>
        /// ���͡�1�����õ���2��ɨ�뵥
        /// </summary>
        /// <returns></returns>
        [DisplayName("���͡�1�����õ���2��ɨ�뵥")]
        public int? PrintType { get; set; }
        /// <summary>
        /// ��߾�
        /// </summary>
        /// <returns></returns>
        [DisplayName("��߾�")]
        public int? LeftMargin { get; set; }
        /// <summary>
        /// �ұ߾�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ұ߾�")]
        public int? RightMargin { get; set; }
        /// <summary>
        /// �ϱ߾�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ϱ߾�")]
        public int? UpMargin { get; set; }
        /// <summary>
        /// �±߾�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�±߾�")]
        public int? DownMargin { get; set; }
        /// <summary>
        /// �߿���
        /// </summary>
        /// <returns></returns>
        [DisplayName("�߿���")]
        public int? Width { get; set; }
        /// <summary>
        /// �߿�߶�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�߿�߶�")]
        public int? Height { get; set; }
        /// <summary>
        /// �߿��ϸ
        /// </summary>
        /// <returns></returns>
        [DisplayName("�߿��ϸ")]
        public int? Board { get; set; }
        /// <summary>
        /// ��ӡ����1������2������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ӡ����1������2������")]
        public int? Orientation { get; set; }
        /// <summary>
        /// ��Ч�ԡ�0��ɾ����1�����ã�2��ͣ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ч�ԡ�0��ɾ����1�����ã�2��ͣ��")]
        public string Enabled { get; set; }
        /// <summary>
        /// CreTm
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreTm")]
        public DateTime? CreTm { get; set; }
        /// <summary>
        /// CreCd
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreCd")]
        public string CreCd { get; set; }
        /// <summary>
        /// CreNm
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreNm")]
        public string CreNm { get; set; }
        /// <summary>
        /// MdfTm
        /// </summary>
        /// <returns></returns>
        [DisplayName("MdfTm")]
        public DateTime? MdfTm { get; set; }
        /// <summary>
        /// MdfCd
        /// </summary>
        /// <returns></returns>
        [DisplayName("MdfCd")]
        public string MdfCd { get; set; }
        /// <summary>
        /// MdfNm
        /// </summary>
        /// <returns></returns>
        [DisplayName("MdfNm")]
        public string MdfNm { get; set; }
        /// <summary>
        /// Rem
        /// </summary>
        /// <returns></returns>
        [DisplayName("Rem")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Rem { get; set; }
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