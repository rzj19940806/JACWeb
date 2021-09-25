//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2021
// Software Developers @ HfutIE 2021
//=====================================================================================

using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// 班组组织机构配置表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.03.30 19:32</date>
    /// </author>
    /// </summary>
    public class BFacR_TeamOrgBll : RepositoryFactory<BFacR_TeamOrg>
    {
        #region 全局变量定义区
        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BFacR_TeamOrg";//===复制时需要修改===
        #endregion

        //#region 1.查询班组组织机构对应机构id
        ///// <summary>
        ///// 联合查询，展示页面表格
        ///// </summary>
        ///// <param name="CheckId"></param>
        ///// <returns></returns>
        //public List<BFacR_TeamOrg> GetStaffList(string OrgNm)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    List<BFacR_TeamOrg> dt;
        //    if (OrgNm != "")
        //    {
        //        strSql.Append(@"SELECT  * FROM  BFacR_TeamOrg where Enabled=1 and OrgNm='" + OrgNm + "'");
        //        dt = Repository().FindListBySql(strSql.ToString());
        //    }
        //    else
        //    {
        //        strSql.Append(@"SELECT  * FROM  BFacR_TeamOrg where Enabled=1 ");
        //        dt = Repository().FindListBySql(strSql.ToString());
        //    }
        //    return dt;
        //}
        //#endregion

        #region 1.查询班组组织机构对应机构id
        /// <summary>
        /// 联合查询，展示页面表格
        /// </summary>
        /// <param name="CheckId"></param>
        /// <returns></returns>
        public List<BFacR_TeamOrg> GetOrgList(string TeamId)
        {
            StringBuilder strSql = new StringBuilder();
            List<BFacR_TeamOrg> dt=new List<BFacR_TeamOrg>  ();
            if (TeamId != "")
            {
                strSql.Append(@"SELECT  * FROM  BFacR_TeamOrg where Enabled='1' and TeamId='" + TeamId + "'");
                dt = Repository().FindListBySql(strSql.ToString());
                if (dt.Count > 0)
                {
                    for (int i = 0; i < dt.Count; i++)
                    {
                        StringBuilder strSql1 = new StringBuilder();
                        strSql1.Append(@"SELECT  * FROM  BFacR_TeamBase where  TeamId='" + dt[i].TeamId + "'");
                        DataTable dt1 = Repository().FindTableBySql(strSql1.ToString());
                        if (dt1.Rows.Count > 0)
                        {
                            dt[i].RsvFld1 = dt1.Rows[0]["TeamNm"].ToString();
                        }
                    }
                }
            }

            return dt;
        }

        /// <summary>
        /// 联合查询，展示页面表格
        /// </summary>
        /// <param name="CheckId"></param>
        /// <returns></returns>
        public List<BFacR_TeamOrg> GetPlineList(string TeamId, string PlineId)
        {
            StringBuilder strSql = new StringBuilder();
            List<BFacR_TeamOrg> dt;
            if (TeamId != "" && PlineId != "")
            {
                strSql.Append(@"SELECT  * FROM  BFacR_TeamOrg where Enabled='1' and TeamId='" + TeamId + "'and PlineId='" + PlineId + "'");
                dt = Repository().FindListBySql(strSql.ToString());
            }
            else
            {
                if (PlineId == "")
                {
                    strSql.Append(@"SELECT  * FROM  BFacR_TeamOrg where Enabled='1' and TeamId='" + TeamId + "'");
                    dt = Repository().FindListBySql(strSql.ToString());
                }
                else
                {
                    strSql.Append(@"SELECT  * FROM  BFacR_TeamOrg where Enabled='1' ");
                    dt = Repository().FindListBySql(strSql.ToString());
                }
            }
            return dt;
        }

        /// <summary>
        /// 联合查询，展示页面表格
        /// </summary>
        /// <param name="ClassId">查询值</param>
        /// <returns></returns>
        public List<BFacR_TeamOrg> GetClassList(string TeamId, string PlineId) //===复制时需要修改===
        {
            string sql = "";
            if (TeamId != "")
            {
                if (PlineId == "")
                {
                    //根据条件查询
                    sql = @"select * from " + tableName + " where  TeamId='" + TeamId + "' and Enabled=1";
                    return Repository().FindListBySql(sql.ToString());
                }
                else
                {
                    //根据条件查询
                    sql = @"select * from " + tableName + " where  TeamId='" + TeamId + "' and PlineId= '" + PlineId + "' and Enabled=1";
                    return Repository().FindListBySql(sql.ToString());
                }
            }
            else
            {
                return null;
            }
        }
        #endregion

        public List<BBdbR_PlineBase> GetPlineList(List<BBdbR_PlineBase> ListData)
        {
            string strSql1 = $"SELECT  * FROM  BFacR_TeamOrg where Enabled='1' ";

            return null;
        }

        #region 4.新增方法
        //entity实体中的数据是从页面中传来的，它是用户填写的数据
        //entity实体中只有部分字段有值，因为页面中只提供给部分字段赋值
        //将页面中填写的数据以实体（entity）的方式新增到数据库
        //其中，实体中的IsAvailable字段没有在页面中填写
        //IsAvailable字段的作用是做假删除，即数据库中的某一条数据的IsAvailable字段的字段值为true表示该数据存在
        //字段值为false表示该数据被删除
        //在删除数据库中的某一条数据时只要修改IsAvailable字段的字段值为false，并不删除该条数据
        //在新增时将实体的IsAvailable字段的值修改为true
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Insert(BBdbR_PlineBase entity, string TeamId) //===复制时需要修改===
        {
            StringBuilder strSql = new StringBuilder();
            if (entity.PlineId.ToString() != "")
            {
                BFacR_TeamOrg TeamStaffentity = new BFacR_TeamOrg();
                TeamStaffentity.TeamId = TeamId;
                TeamStaffentity.PlineId = entity.PlineId.ToString();
                TeamStaffentity.Create();
                Repository().Insert(TeamStaffentity);
            }
            return 1;
        }
        #endregion
        #region 5.删除方法
        //array 需要删除的信息的主键的数组
        //删除表中某一条数据表示将表中该条数据的Enabled设置为0，并不是真的删除该数据
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Delete(List<BFacR_TeamOrg> TeamStfList)
        {
            //创建一个表格的list，用于存储通过主键查询到的信息
            List<BFacR_TeamOrg> listEntity = new List<BFacR_TeamOrg>(); //===复制时需要修改===
            for (int i = 0; i < TeamStfList.Count; i++)
            {
                TeamStfList[i].Enabled = "0";//将该实体的IsAvailable属性改为false
                listEntity.Add(TeamStfList[i]);
            }
            return Repository().Update(listEntity);//修改数据库
        }
        #endregion
    }
}