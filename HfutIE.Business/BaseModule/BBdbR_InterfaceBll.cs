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
    /// 接口管理表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.12.12 16:22</date>
    /// </author>
    /// </summary>
    public class BBdbR_InterfaceBll : RepositoryFactory<BBdbR_Interface>
    {
        #region 全局变量定义区
        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BBdbR_Interface";
        public readonly RepositoryFactory<BBdbR_Interface> repositoryFactory = new RepositoryFactory<BBdbR_Interface>();
        #endregion
        

        #region 3.展示表格
        
        /// <summary>
        /// 返回填充编辑界面数据源
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public DataTable GetList() //===复制时需要修改===
        {
            string sql = "";
            DataTable dt = new DataTable();
            sql = "select * from " + tableName + " where Enabled = '1'";     //===复制时需要修改===
            dt = Repository().FindTableBySql(sql.ToString(), false);
            return dt;
        }
        #endregion

        #region 4.新增编辑方法
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
        public int Update(BBdbR_Interface entity) //===复制时需要修改===
        {
            return Repository().Update(entity); //将修改后的实体跟新到数据库中
        }
        public int CheckCount(string tableName, string KeyName, string KeyValue)
        {
            //string sql = @"select * from " + tableName + " where Enabled = 1 and " + KeyName + " = '" + KeyValue + "'";
            string sql = @"select * from " + tableName + " where Enabled = '1' and " + KeyName + " = '" + KeyValue + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }
       
        #endregion
        
    }
}