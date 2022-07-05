//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2021
// Software Developers @ HfutIE 2021
//=====================================================================================

using HfutIE.DataAccess;
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
    /// 合装线编码器数值记录表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.12.12 16:22</date>
    /// </author>
    /// </summary>
    public class P_FitEnCodeInfoBll : RepositoryFactory<P_FitEnCodeInfo>
    {
        #region 全局变量定义区
        //定义本页面主要操作的表的表名，称为主表
        string tableName = "P_FitEnCodeInfo";
        public readonly RepositoryFactory<P_FitEnCodeInfo> repositoryFactory = new RepositoryFactory<P_FitEnCodeInfo>();
        #endregion
        

        #region 3.展示表格
        
        /// <summary>
        /// 返回填充编辑界面数据源
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public DataTable GetList(string PlineNm) //===复制时需要修改===
        {
            string sql = $"select FitEnCodeInfoId,TB.PlineQueID,TA.PlineNm,TA.No,TA.WcNm,TA.VIN,TB.VIN VIN2,IIF(TA.VIN=TB.VIN,0,1) YN,TA.CodeValue,StartPoint,EndPoint from(select ROW_NUMBER()over(partition by A.PlineCd order by CodeValue) No,FitEnCodeInfoId,VIN,CodeValue,PlineNm,WcNm,A.PlineId,C.StartPoint,C.EndPoint from P_FitEnCodeInfo A with(nolock) join BBdbR_PlineBase B with(nolock) on A.PlineId = B.PlineId left join BBdbR_WcBase C with(nolock) on CodeValue> C.StartPoint and CodeValue<C.EndPoint and A.PlineId= C.PlineId) TA join(select ROW_NUMBER()over(partition by PlineCd order by CarQuene desc) No,PlineQueID, VIN, PlineCd, CarQuene, PlineId from P_LineProductionQueue_Pro with (nolock) where PlineCd in ('Line-10', 'Line-11', 'Line-12', 'Line-13', 'Line-14') and AVICd<>'FIT-2') TB on TA.No = TB.No and TA.PlineId = TB.PlineId where PlineNm = '{PlineNm}' or '{PlineNm}'=''";     //===复制时需要修改===
            DataTable dt = Repository().FindTableBySql(sql.ToString(), false);
            return dt;
        }
        #endregion



        #region VIN校准
        public string VINToOk(string PlineNm) //===复制时需要修改===
        {
            //StringBuilder stringBuilder = new StringBuilder();
            //stringBuilder.Append( $"update P_FitEnCodeInfo set VIN=A.VIN from (select VIN, ROW_NUMBER()over(order by CarQuene desc) ID from P_LineProductionQueue_Pro with(nolock) where PlineNm = '{PlineNm}') A join (select FitEnCodeInfoId, VIN, ROW_NUMBER() over(order by CodeValue) ID from P_FitEnCodeInfo with(nolock) where PlineCd= (select PlineCd from BBdbR_PlineBase where PlineNm = '{PlineNm}')) B on A.ID = B.ID where P_FitEnCodeInfo.FitEnCodeInfoId = B.FitEnCodeInfoId");     //===复制时需要修改===
            ArrayList sqllist = new ArrayList();
            string vintonull = $"update P_FitEnCodeInfo  set VIN = '9999' where FitEnCodeInfoId in (select FitEnCodeInfoId from P_FitEnCodeInfo a left join BBdbR_PlineBase b on a.PlineCd = b.PlineCd where PlineNm = '{PlineNm}' )";
            string vintook = $"update P_FitEnCodeInfo set VIN=A.VIN from (select VIN, ROW_NUMBER()over(order by CarQuene desc) ID from P_LineProductionQueue_Pro with(nolock) where PlineNm = '{PlineNm}') A join (select FitEnCodeInfoId, VIN, ROW_NUMBER() over(order by CodeValue) ID from P_FitEnCodeInfo with(nolock) where PlineCd= (select PlineCd from BBdbR_PlineBase where PlineNm = '{PlineNm}')) B on A.ID = B.ID where P_FitEnCodeInfo.FitEnCodeInfoId = B.FitEnCodeInfoId";
            //sqllist.Add(sqlupdateoldqueue);
            sqllist.Add(vintonull);
            sqllist.Add(vintook);
            string res =  DbHelperSQL.ExecuteSqlTran1(sqllist); //同时执行，实现事务回滚
            return res;
                
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
        public int Update(P_FitEnCodeInfo entity) //===复制时需要修改===
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



        #region 5.删除方法
        //array 需要删除的信息的主键的数组
        //删除表中某一条数据表示将表中该条数据的Enabled设置为0，并不是真的删除该数据
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Delete(string[] array)
        {

            int count = 0;
            foreach (string keyValue in array)
            {
                StringBuilder del = new StringBuilder();
                del.Append($"delete from P_LineProductionQueue_Pro where PlineQueID = '{keyValue}'");

                count += DbHelperSQL.ExecuteSql(del.ToString());//修改数据库
                                                                //===复制时需要修改===

            }
            return count;
        }
        #endregion

    }
}