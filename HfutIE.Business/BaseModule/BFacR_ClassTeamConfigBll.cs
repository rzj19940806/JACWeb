//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2021
// Software Developers @ HfutIE 2021
//=====================================================================================

using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// 班次班组配置表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.29 22:27</date>
    /// </author>
    /// </summary>
    public class BFacR_ClassTeamConfigBll : RepositoryFactory<BFacR_ClassTeamConfig>
    {
        #region 全局变量定义区
        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BFacR_ClassTeamConfig";//===复制时需要修改===
        #endregion

        #region 1.搜索
        /// <summary>
        /// 联合查询，展示页面表格
        /// </summary>
        /// <param name="ClassId">查询值</param>
        /// <returns></returns>
        public List<BFacR_ClassTeamConfig> GetPageListByCondition(string ClassId) //===复制时需要修改===
        {
            string sql = "";
            if (ClassId != "")
            {
                //根据条件查询
                sql = @"select * from " + tableName + " where  ClassId='" + ClassId + "' and Enabled=1";
                return Repository().FindListBySql(sql.ToString());
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 2.搜索
        /// <summary>
        /// 联合查询，展示页面表格
        /// </summary>
        /// <param name="ClassId">查询值</param>
        /// <returns></returns>
        public List<BFacR_ClassTeamConfig> GetClassList(string ShiftId, string TeamId) //===复制时需要修改===
        {
            string sql = "";
            if (ShiftId != "")
            {
                if (TeamId == "")
                {
                    //根据条件查询
                    sql = @"select * from " + tableName + " where  ShiftId='" + ShiftId + "' and Enabled=1";
                    return Repository().FindListBySql(sql.ToString());
                }
                else
                {
                    //根据条件查询
                    sql = @"select * from " + tableName + " where  ShiftId='" + ShiftId + "' and TeamId= '" + TeamId + "' and Enabled=1";
                    return Repository().FindListBySql(sql.ToString());
                }
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 3.删除
        //array 需要删除的信息的主键的数组
        //删除表中某一条数据表示将表中该条数据的Enabled设置为0，并不是真的删除该数据
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Delete(List<BFacR_ClassTeamConfig> ClassConfigList)
        {
            //创建一个表格的list，用于存储通过主键查询到的信息
            List<BFacR_ClassTeamConfig> listEntity = new List<BFacR_ClassTeamConfig>(); //===复制时需要修改===
            for (int i = 0; i < ClassConfigList.Count; i++)
            {
                ClassConfigList[i].Enabled = "0";//将该实体的IsAvailable属性改为false
                listEntity.Add(ClassConfigList[i]);
            }
            return Repository().Update(listEntity);//修改数据库
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
        public int InsertClassConfig(string ShiftId, BFacR_TeamBase entity) //===复制时需要修改===
        {
            StringBuilder strSql = new StringBuilder();
            int Isok = 0;
            if (entity.TeamId.ToString() != "")
            {
                BFacR_ClassTeamConfig Classentity = new BFacR_ClassTeamConfig();
                Classentity.ShiftId = ShiftId;
                Classentity.TeamId = entity.TeamId.ToString();
                Classentity.Create();
                Repository().Insert(Classentity);
                Isok = 1;
            }
            return Isok;
        }
        #endregion
    }
}