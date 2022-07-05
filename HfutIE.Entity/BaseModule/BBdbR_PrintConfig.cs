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
    /// ��ӡ���ñ�
    /// <author>
    ///		<name>she</name>
    ///		<date>2022.04.22 10:43</date>
    /// </author>
    /// </summary>
    [Description("��ӡ���ñ�")]
    [PrimaryKey("Id")]
    public class BBdbR_PrintConfig : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// Id
        /// </summary>
        /// <returns></returns>
        [DisplayName("Id")]
        public string Id { get; set; }
        /// <summary>
        /// PridId
        /// </summary>
        /// <returns></returns>
        [DisplayName("PridId")]
        public string PridId { get; set; }
        /// <summary>
        /// ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("���")]
        public string PrintConfigCd { get; set; }
        /// <summary>
        /// ����0����ͨ��1�����1������
        /// </summary>
        /// <returns></returns>
        [DisplayName("����0����ͨ��1�����1������")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? Type { get; set; }
        /// <summary>
        /// ������(��ż���������ڱ����������ڱ߿�)
        /// </summary>
        /// <returns></returns>
        [DisplayName("������(��ż���������ڱ����������ڱ߿�)")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? X { get; set; }
        /// <summary>
        /// ������(��ż���������ڱ����������ڱ߿�)
        /// </summary>
        /// <returns></returns>
        [DisplayName("������(��ż���������ڱ����������ڱ߿�)")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MX { get; set; }
        /// <summary>
        /// ��������(��ż���������ڱ����������ڱ߿�)
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������(��ż���������ڱ����������ڱ߿�)")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? Y { get; set; }
        /// <summary>
        /// ��������(��ż���������ڱ����������ڱ߿�)
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������(��ż���������ڱ����������ڱ߿�)")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MY { get; set; }
        /// <summary>
        /// ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("���")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? Width { get; set; }
        /// <summary>
        /// ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("���")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MWidth { get; set; }
        /// <summary>
        /// �߶�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�߶�")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? Height { get; set; }
        /// <summary>
        /// �߶�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�߶�")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MHeight { get; set; }
        /// <summary>
        /// �߿��ϸ
        /// </summary>
        /// <returns></returns>
        [DisplayName("�߿��ϸ")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? Board { get; set; }

        /// <summary>
        /// �Զ�������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Զ�������")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Context { get; set; }

        /// <summary>
        /// �ֺ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ֺ�")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? FontSize { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string FontFamily { get; set; }
        /// <summary>
        /// ���Ρ�0����ͨ��1���Ӵ֣�2����б��4���»��ߣ�8��ɾ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("���Ρ�0����ͨ��1���Ӵ֣�2����б��4���»��ߣ�8��ɾ����")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? FontStyle { get; set; }
        /// <summary>
        /// ARGB
        /// </summary>
        /// <returns></returns>
        [DisplayName("ARGB")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ARGB { get; set; }
        /// <summary>
        /// ˮƽ���� 0������1�����У�2�����ң�������Ϊ������Ϊ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("ˮƽ���� 0������1�����У�2�����ң�������Ϊ������Ϊ������")]
        public int? Alignment { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        /// <summary>
        /// ��ֱ���� 0�����ϣ�1�����У�2������,������Ϊ������Ϊ����߶�
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ֱ���� 0�����ϣ�1�����У�2������,������Ϊ������Ϊ����߶�")]
        public int? LineAlignment { get; set; }
        /// <summary>
        /// ����߶�
        /// </summary>
        /// <returns></returns>
        [DisplayName("����߶�")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CodeHeight { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CodeWidth { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? RowNum { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? ColNum { get; set; }
        /// <summary>
        /// �м��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�м��")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? RowHeight { get; set; }
        /// <summary>
        /// �м��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�м��")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? ColWidth { get; set; }
        /// <summary>
        /// Rem
        /// </summary>
        /// <returns></returns>
        [DisplayName("Rem")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Rem { get; set; }
        /// <summary>
        /// Enabled
        /// </summary>
        /// <returns></returns>
        [DisplayName("Enabled")]
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