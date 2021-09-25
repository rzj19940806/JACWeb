using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HfutIE.WebApp.App_Start
{
    public class PublicClass
    {
        #region 班组配置信息表
        /// <summary>
        /// 班组配置人员、产线信息
        /// </summary>
        public class BFacR_TeamBase_Add
        {
            #region 班组基本信息
            public string TeamId { get; set; }//班组主键
            public string TeamCd { get; set; }//班组编号
            public string TeamNm { get; set; }//班组名称
            public string Enabled { get; set; }//有效性
            #endregion
            #region 班组人员配置信息
            public string StfId { get; set; }//人员id
            public string StfCd { get; set; }//人员编号
            public string StfNm { get; set; }//人员姓名
            #endregion
            #region 班组组织机构配置信息
            public string OrgId { get; set; }//组织机构id
            public string OrgRank { get; set; }//组织机构级别
            public string OrgNm { get; set; }//组织机构名称
            #endregion
        }
        #endregion
    }
}