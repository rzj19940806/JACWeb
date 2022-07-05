//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2014
// Software Developers @ HfutIE 2014
//=====================================================================================

using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// ������ťȨ�ޱ�
    /// <author>
    ///		<name>she</name>
    ///		<date>2014.08.18 16:02</date>
    /// </author>
    /// </summary>
    public class Base_ButtonPermissionBll : RepositoryFactory<Base_ButtonPermission>
    {
        /// <summary>
        /// ��ťȨ���б�������Ȩ��ʱʹ��
        /// </summary>
        /// <param name="ObjectId">��������</param>
        /// <param name="Category">�������:1-����2-��ɫ3-��λ4-Ⱥ��</param>
        /// <returns></returns>
        public DataTable GetList(string ObjectId, string Category)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"select * from(SELECT  b.ButtonId ,			
                                    b.ModuleId ,			
                                    b.Code ,					
                                    b.FullName ,				
                                    b.Category ,				
                                    b.Icon ,					
                                    b.SortCode ,				
                                    bp.ObjectId,
                                    ROW_NUMBER() OVER(PARTITION BY b.ButtonId ORDER BY b.SortCode ASC) as RepeatNum		
                            FROM    Base_Button b
                                    LEFT JOIN Base_ButtonPermission bp ON bp.ModuleButtonId = b.ButtonId
                                                                          AND bp.ObjectId  IN ('" + ObjectId.Replace(",", "','") + "')) a ");//@ObjectId����2021.11.29�޸�
            //if (!ManageProvider.Provider.Current().IsSystem)
            //{
            //    strSql.Append(@"SELECT  b.ButtonId ,				
            //                            b.ModuleId ,				
            //                            b.Code ,					
            //                            b.FullName ,				
            //                            b.Category ,				
            //                            b.Icon ,					
            //                            b.SortCode ,				
            //                            cp.ModuleButtonId AS ObjectId					
            //                    FROM    Base_Button b INNER JOIN ( SELECT DISTINCT ModuleButtonId  FROM   Base_ButtonPermission");
            //    strSql.Append(" WHERE  ObjectId IN ('" + ObjectId.Replace(",", "','") + "')) bp ON B.ButtonId = bp.ModuleButtonId");
            //    strSql.Append(" LEFT JOIN ( SELECT DISTINCT ModuleButtonId  FROM  Base_ButtonPermission");
            //    strSql.Append(" WHERE  ObjectId IN ('" + ObjectId.Replace(",", "','") + "') ) cp ON cp.ModuleButtonId = b.ButtonId");//= @ObjectId����2021.11.29�޸�
            //}
            //else
            //{
                
            //}
            //2021.11.28����
            strSql.Append(@" where   RepeatNum = '1'");//2021.11.28����  Enabled = '1' and 

            strSql.Append(" order by SortCode ASC");
            parameter.Add(DbFactory.CreateDbParameter("@ObjectId", ObjectId));
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }
        /// <summary>
        /// ���ذ�ťȨ�ޡ����û���¼ģ�����ʱ����
        /// </summary>
        /// <param name="ObjectId">��������</param>
        /// <param name="ModuleId"ģ������</param>
        /// <returns></returns>
        public List<Base_Button> GetButtonList(string ObjectId, string ModuleId)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            if (!ManageProvider.Provider.Current().IsSystem)
            {
                strSql.Append(@"SELECT DISTINCT B.* FROM Base_Button B");
                strSql.Append(" INNER JOIN Base_ButtonPermission BP ON B.ButtonId = BP.ModuleButtonId WHERE ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "')");
            }
            else
            {
                strSql.Append(@"SELECT * FROM Base_Button B WHERE 1=1 ");
            }
            strSql.Append(" AND B.ModuleId = @ModuleId");
            strSql.Append(" ORDER BY B.SortCode ASC ");
            parameter.Add(DbFactory.CreateDbParameter("@ModuleId", ModuleId));
            return DataFactory.Database().FindListBySql<Base_Button>(strSql.ToString(), parameter.ToArray());
        }
        /// <summary>
        /// �����ն˽��水ťȨ�ޡ���2021.11.30�����ն˽���ʹ��
        /// </summary>
        /// <param name="ObjectId">��������</param>
        /// <param name="ModuleId"ģ������</param>
        /// <returns></returns>
        public List<Base_Button> GetButtonList1(string ObjectId, string ModuleId)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT DISTINCT B.* FROM Base_Button B");
            strSql.Append(" INNER JOIN Base_ButtonPermission BP ON B.ButtonId = BP.ModuleButtonId WHERE ObjectId IN ('" + ObjectId.Replace(",", "','") + "')");
            strSql.Append(" AND B.ModuleId = @ModuleId");
            strSql.Append(" ORDER BY B.SortCode ASC ");
            parameter.Add(DbFactory.CreateDbParameter("@ModuleId", ModuleId));
            return DataFactory.Database().FindListBySql<Base_Button>(strSql.ToString(), parameter.ToArray());
        }
        /// <summary>
        /// ���ݶ���Id��ȡģ�鰴ťȨ���б����û���¼��������ʱ����
        /// </summary>
        /// <param name="ObjectId">����ID</param>
        /// <returns></returns>
        public DataTable GetButtonePermission(string ObjectId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  *
                            FROM    ( SELECT    m.ModuleId AS ID ,
                                                m.ParentId ,
                                                m.FullName ,
                                                m.Icon ,
                                                m.SortCode,
                                                m.Category,
                                                'ģ��' AS Sort
                                      FROM      Base_Module m
                                                LEFT JOIN Base_ModulePermission mp ON mp.ModuleId = m.ModuleId");
            strSql.Append(@" WHERE     mp.ObjectId IN ('" + ObjectId.Replace(",", "','") + "')");
            strSql.Append(@" UNION     SELECT    b.ButtonId AS ID ,
                                                b.ModuleId AS ParentId ,
                                                b.FullName ,
                                                b.Icon ,
                                                b.SortCode,
                                                b.Category,
                                                '��ť' AS Sort
                                      FROM      Base_Button b
                                                LEFT JOIN Base_ButtonPermission bp ON bp.ModuleButtonId = b.ButtonId");
            strSql.Append(" WHERE     bp.ObjectId IN ('" + ObjectId.Replace(",", "','") + "')) A");
            strSql.Append(" ORDER BY SortCode ASC ");
            return Repository().FindTableBySql(strSql.ToString());
        }
    }
}