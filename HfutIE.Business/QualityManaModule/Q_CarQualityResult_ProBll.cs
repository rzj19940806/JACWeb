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
using System.Linq;
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// 车身质量检查销项过程表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.12.25 10:08</date>
    /// </author>
    /// </summary>
    public class Q_CarQualityResult_ProBll : RepositoryFactory<Q_CarQualityResult_Pro>
    {

        #region 删除方法
        //array 需要删除的信息的主键的数组
        //删除表中某一条数据表示将表中该条数据的Enabled设置为0，并不是真的删除该数据
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Delete(string KeyValue)
        {

            ////创建一个表格的list，用于存储通过主键查询到的信息
            List<Q_CarQualityResult_Pro> listEntity = new List<Q_CarQualityResult_Pro>(); //===复制时需要修改===
            //===复制时需要修改===
            Q_CarQualityResult_Pro entity = Repository().FindEntity(KeyValue);//根据主键（keyValue）在数据库中查找实体 //===复制时需要修改===
            entity.Enabled = "0";//将该实体的IsAvailable属性改为false
            entity.Modify(KeyValue);
            listEntity.Add(entity);
            return Repository().Update(listEntity);//修改数据库
        }
        #endregion

    }
}