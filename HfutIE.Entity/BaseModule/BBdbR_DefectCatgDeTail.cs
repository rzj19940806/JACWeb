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
    /// ȱ????ϸ??
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.31 16:55</date>
    /// </author>
    /// </summary>
    [Description("ȱ????ϸ??")]
    [PrimaryKey("DefectId")]
    public class BBdbR_DefectCatgDeTail : BaseEntity
    {
        #region ??ȡ/???? ?ֶ?ֵ
        /// <summary>
        /// ȱ??????
        /// </summary>
        /// <returns></returns>
        [DisplayName("ȱ??????")]
        public string DefectId { get; set; }
        /// <summary>
        /// ȱ??????????
        /// </summary>
        /// <returns></returns>
        [DisplayName("ȱ??????????")]
        public string DefectCatgId { get; set; }
        /// <summary>
        /// ȱ?ݱ???
        /// </summary>
        /// <returns></returns>
        [DisplayName("ȱ?ݱ???")]
        public string DefectCd { get; set; }
        /// <summary>
        /// ȱ??????
        /// </summary>
        /// <returns></returns>
        [DisplayName("ȱ??????")]
        public string DefectNm { get; set; }
        /// <summary>
        /// ȱ?ݼ???
        /// </summary>
        /// <returns></returns>
        [DisplayName("ȱ?ݼ???")]
        public int? DefecLevel { get; set; }
        /// <summary>
        /// ????
        /// </summary>
        /// <returns></returns>
        [DisplayName("????")]
        public string Dsc { get; set; }
        /// <summary>
        /// ??Ч??
        /// </summary>
        /// <returns></returns>
        [DisplayName("??Ч??")]
        public string Enabled { get; set; }
        /// <summary>
        /// ?汾??
        /// </summary>
        /// <returns></returns>
        [DisplayName("?汾??")]
        public string VersionNumber { get; set; }
        /// <summary>
        /// ????ʱ??
        /// </summary>
        /// <returns></returns>
        [DisplayName("????ʱ??")]
        public string CreTm { get; set; }
        /// <summary>
        /// ?????˱???
        /// </summary>
        /// <returns></returns>
        [DisplayName("?????˱???")]
        public string CreCd { get; set; }
        /// <summary>
        /// ??????????
        /// </summary>
        /// <returns></returns>
        [DisplayName("??????????")]
        public string CreNm { get; set; }
        /// <summary>
        /// ?޸?ʱ??
        /// </summary>
        /// <returns></returns>
        [DisplayName("?޸?ʱ??")]
        public string MdfTm { get; set; }
        /// <summary>
        /// ?޸??˱???
        /// </summary>
        /// <returns></returns>
        [DisplayName("?޸??˱???")]
        public string MdfCd { get; set; }
        /// <summary>
        /// ?޸???????
        /// </summary>
        /// <returns></returns>
        [DisplayName("?޸???????")]
        public string MdfNm { get; set; }
        /// <summary>
        /// ??ע
        /// </summary>
        /// <returns></returns>
        [DisplayName("??ע")]
        public string Rem { get; set; }
        #endregion

        #region ??չ????
        /// <summary>
        /// ????????
        /// </summary>
        public override void Create()
        {
            this.DefectId = CommonHelper.GetGuid;
            this.VersionNumber = "V1.0";
            this.CreTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.Enabled = "1";
            this.CreCd = ManageProvider.Provider.Current().UserId;
            this.CreNm = ManageProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// ?༭????
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.DefectId = KeyValue;
            this.VersionNumber = "V1.0";
            this.MdfTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}