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
    /// AVIȥ��������Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.31 12:49</date>
    /// </author>
    /// </summary>
    [Description("AVIȥ��������Ϣ��")]
    [PrimaryKey("AVIWhereId")]
    public class BBdbR_AVIWhereaboutsConfig : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public string AVIWhereId { get; set; }
        /// <summary>
        /// AVI����
        /// </summary>
        /// <returns></returns>
        [DisplayName("AVI����")]
        public string AviId { get; set; }
        /// <summary>
        /// ȥ��AVIվ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("ȥ��AVIվ������")]
        public string ToAVIId { get; set; }
        /// <summary>
        /// ȥ��AVIվ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("ȥ��AVIվ����")]
        public string ToAVICd { get; set; }
        /// <summary>
        /// ȥ��AVIվ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("ȥ��AVIվ������")]
        public string ToAVINm { get; set; }
        /// <summary>
        /// AVIվ���Ƿ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("AVIվ���Ƿ����")]
        public int? IsIndependence { get; set; }
        /// <summary>
        /// ȥ��AVIվ��˳��
        /// </summary>
        /// <returns></returns>
        [DisplayName("ȥ��AVIվ��˳��")]
        public int? ToAVISequence { get; set; }
        /// <summary>
        /// ȥ���������
        /// </summary>
        /// <returns></returns>
        [DisplayName("ȥ���������")]
        public string PlineId { get; set; }
        /// <summary>
        /// ȥ����߱�ʶ
        /// </summary>
        /// <returns></returns>
        [DisplayName("ȥ����߱�ʶ")]
        public string PlineMark { get; set; }
        /// <summary>
        /// ���в�
        /// </summary>
        /// <returns></returns>
        [DisplayName("���в�")]
        public string Queue { get; set; }
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
            this.AVIWhereId = CommonHelper.GetGuid;
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
            this.AVIWhereId = KeyValue;
            this.VersionNumber = "V1.0";
            this.Enabled = "1";
            this.MdfTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}