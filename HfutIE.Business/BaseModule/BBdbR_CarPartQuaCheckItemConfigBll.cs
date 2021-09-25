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
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// 车身部位检查项配置表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.28 17:21</date>
    /// </author>
    /// </summary>
    public class BBdbR_CarPartQuaCheckItemConfigBll : RepositoryFactory<BBdbR_CarPartQuaCheckItemConfig>
    {
        #region 全局变量定义区
        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BBdbR_CarPartQuaCheckItemConfig";//===复制时需要修改===
        #endregion
        #region 1.查询已配置检查项目
        /// <summary>
        /// 联合查询，展示页面表格
        /// </summary>
        /// <param name="CheckId"></param>
        /// <returns></returns>
        public List<BBdbR_CarPartQuaCheckItemConfig> GetPlanList(string CarPartId, JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            List<BBdbR_CarPartQuaCheckItemConfig> dt;
            if (CarPartId == "")
            {
                strSql.Append(@"SELECT  * FROM  BBdbR_CarPartQuaCheckItemConfig where Enabled=1 ");
                dt = Repository().FindListPageBySql(strSql.ToString(),ref jqgridparam);

            }
            else
            {
                strSql.Append(@"SELECT  * FROM  BBdbR_CarPartQuaCheckItemConfig where Enabled=1 and CarPartId='" + CarPartId + "'");
                dt = Repository().FindListPageBySql(strSql.ToString(),ref jqgridparam);

            }

            return dt;
        }

        #endregion
        #region 2.已配置车身部位检查项目配置信息
        /// <summary>
        /// 已配置车身部位检查项目配置信息
        /// </summary>
        /// <param name="CheckId"></param>
        /// <returns></returns>
        public List<BBdbR_CarPartQuaCheckItemConfig> GetCheckList(string CarPartId, string QuaCheckItemId)
        {
            StringBuilder strSql = new StringBuilder();
            List<BBdbR_CarPartQuaCheckItemConfig> dt;
            if (CarPartId != "" && QuaCheckItemId != "")
            {
                strSql.Append(@"SELECT  * FROM  BBdbR_CarPartQuaCheckItemConfig where Enabled=1 and CarPartId='" + CarPartId + "'and QuaCheckItemId='" + QuaCheckItemId + "'");
                dt = Repository().FindListBySql(strSql.ToString());
            }
            else
            {
                if (CarPartId != "")
                {
                    strSql.Append(@"SELECT  * FROM  BBdbR_CarPartQuaCheckItemConfig where Enabled=1 and CarPartId='" + CarPartId + "'");
                    dt = Repository().FindListBySql(strSql.ToString());
                }
                else if (QuaCheckItemId != "")
                {
                    strSql.Append(@"SELECT  * FROM  BBdbR_CarPartQuaCheckItemConfig where Enabled=1 and QuaCheckItemId='" + QuaCheckItemId + "'");
                    dt = Repository().FindListBySql(strSql.ToString());
                }
                else
                {
                    strSql.Append(@"SELECT  * FROM  BBdbR_CarPartQuaCheckItemConfig where Enabled=1 ");
                    dt = Repository().FindListBySql(strSql.ToString());
                }
            }
            return dt;
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
        public int Insert(BBdbR_QuaCheckItemBase entity, string CarPartId) //===复制时需要修改===
        {
            StringBuilder strSql = new StringBuilder();
            if (entity.QuaCheckItemId.ToString() != "")
            {
                BBdbR_CarPartQuaCheckItemConfig Configentity = new BBdbR_CarPartQuaCheckItemConfig();
                Configentity.CarPartQuaCheckItemConfigId = System.Guid.NewGuid().ToString();
                Configentity.CarPartId = CarPartId;
                Configentity.QuaCheckItemId = entity.QuaCheckItemId.ToString();
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
        public int Delete(List<BBdbR_CarPartQuaCheckItemConfig> TeamStfList)
        {
            //创建一个表格的list，用于存储通过主键查询到的信息
            List<BBdbR_CarPartQuaCheckItemConfig> listEntity = new List<BBdbR_CarPartQuaCheckItemConfig>(); //===复制时需要修改===
            for (int i = 0; i < TeamStfList.Count; i++)
            {
                TeamStfList[i].Enabled = "0";//将该实体的IsAvailable属性改为false
                TeamStfList[i].Modify(TeamStfList[i].CarPartQuaCheckItemConfigId);
                listEntity.Add(TeamStfList[i]);
            }
            return Repository().Update(listEntity);//修改数据库
        }
        #endregion
    }
}