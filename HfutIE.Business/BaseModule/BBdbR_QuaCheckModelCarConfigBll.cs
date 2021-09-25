//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2021
// Software Developers @ HfutIE 2021
//=====================================================================================

using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// 检查模板车身配置信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.26 17:21</date>
    /// </author>
    /// </summary>
    public class BBdbR_QuaCheckModelCarConfigBll : RepositoryFactory<BBdbR_QuaCheckModelCarConfig>
    {
        #region 全局变量定义区
        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BBdbR_QuaCheckModelCarConfig";//===复制时需要修改===
        #endregion
        #region 1.查询已配置物料
        /// <summary>
        /// 联合查询，展示页面表格
        /// </summary>
        /// <param name="CheckId"></param>
        /// <returns></returns>
        public List<BBdbR_QuaCheckModelCarConfig> GetPlanList(string QualityCheckModelId)
        {
            StringBuilder strSql = new StringBuilder();
            List<BBdbR_QuaCheckModelCarConfig> dt;
            if (QualityCheckModelId == "")
            {
                strSql.Append(@"SELECT  * FROM  BBdbR_QuaCheckModelCarConfig where Enabled=1 ");
                dt = Repository().FindListBySql(strSql.ToString());
                
            }
            else
            {
                strSql.Append(@"SELECT  * FROM  BBdbR_QuaCheckModelCarConfig where Enabled=1 and QualityCheckModelId='" + QualityCheckModelId + "'");
                dt = Repository().FindListBySql(strSql.ToString());
                
            }

            return dt;
        }

        #endregion
        #region 2.已配置检查模板产品配置信息
        /// <summary>
        /// 已配置检查模板产品配置信息
        /// </summary>
        /// <param name="CheckId"></param>
        /// <returns></returns>
        public List<BBdbR_QuaCheckModelCarConfig> GetMatList(string QualityCheckModelId, string MatId)
        {
            StringBuilder strSql = new StringBuilder();
            List<BBdbR_QuaCheckModelCarConfig> dt;
            if (QualityCheckModelId != "" && MatId != "")
            {
                strSql.Append(@"SELECT  * FROM  BBdbR_QuaCheckModelCarConfig where Enabled=1 and QualityCheckModelId='" + QualityCheckModelId + "'and MatId='" + MatId + "'");
                dt = Repository().FindListBySql(strSql.ToString());
            }
            else
            {
                if (QualityCheckModelId != "" )
                {
                    strSql.Append(@"SELECT  * FROM  BBdbR_QuaCheckModelCarConfig where Enabled=1 and QualityCheckModelId='" + QualityCheckModelId + "'");
                    dt = Repository().FindListBySql(strSql.ToString());
                }
                else if (MatId != "")
                {
                    strSql.Append(@"SELECT  * FROM  BBdbR_QuaCheckModelCarConfig where Enabled=1 and MatId='" + MatId + "'");
                    dt = Repository().FindListBySql(strSql.ToString());
                }
                else
                {
                    strSql.Append(@"SELECT  * FROM  BBdbR_QuaCheckModelCarConfig where Enabled=1 ");
                    dt = Repository().FindListBySql(strSql.ToString());
                }
            }
            return dt;
        }
        #endregion
        #region 3.检查某个字段的字段值是否存在
        /// <summary>
        ///   Enabled = 1的数据中某个字段（KeyName）的字段值（KeyValue）是否存在
        /// </summary>
        /// <param name="KeyName">字段名</param>
        /// <param name="KeyValue">字段值</param>
        /// <returns>返回的是搜索的表中包含该字段值的记录条数</returns>
        public int CheckCount(string KeyName, string KeyValue)
        {
            string sql = @"select * from " + tableName + " where Enabled = '1' and " + KeyName + "  = '" + KeyValue + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }
        #endregion
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
        public int Insert(BBdbR_MatBase entity, string QualityCheckModelId) //===复制时需要修改===
        {
            StringBuilder strSql = new StringBuilder();
            if (entity.MatId.ToString() != "")
            {
                BBdbR_QuaCheckModelCarConfig Configentity = new BBdbR_QuaCheckModelCarConfig();
                Configentity.QuaCheckModelCarConfigId = System.Guid.NewGuid().ToString();
                Configentity.QualityCheckModelId = QualityCheckModelId;
                Configentity.MatId = entity.MatId.ToString();
                Configentity.Enabled = "1";
                Configentity.VersionNumber = "V1.0";
                Configentity.CreTm = DateTime.Now;
                Configentity.CreCd = ManageProvider.Provider.Current().UserId;
                Configentity.CreNm = ManageProvider.Provider.Current().UserName;
                Repository().Insert(Configentity);
            }
            return 1;
        }
        #endregion
        #region 5.删除方法
        //array 需要删除的信息的主键的数组
        //删除表中某一条数据表示将表中该条数据的Enabled设置为0，并不是真的删除该数据
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Delete(List<BBdbR_QuaCheckModelCarConfig> TeamStfList)
        {
            //创建一个表格的list，用于存储通过主键查询到的信息
            List<BBdbR_QuaCheckModelCarConfig> listEntity = new List<BBdbR_QuaCheckModelCarConfig>(); //===复制时需要修改===
            for (int i = 0; i < TeamStfList.Count; i++)
            {
                TeamStfList[i].Enabled = "0";//将该实体的IsAvailable属性改为false
                TeamStfList[i].Modify(TeamStfList[i].QuaCheckModelCarConfigId);
                listEntity.Add(TeamStfList[i]);
            }
            return Repository().Update(listEntity);//修改数据库
        }
        #endregion
    }
}