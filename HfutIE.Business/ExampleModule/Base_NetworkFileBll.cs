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
    /// 网络硬盘文件表
    /// <author>
    ///		<name>she</name>
    ///		<date>2014.10.16 10:54</date>
    /// </author>
    /// </summary>
    public class Base_NetworkFileBll : RepositoryFactory<Base_NetworkFile>
    {
        /// <summary>
        /// 根据文件夹Id 查询所有子文件夹（递归）
        /// </summary>
        /// <param name="FolderId">文件夹主键</param>
        /// <returns></returns>
        public List<Base_NetworkFolder> GetChildrenNodeList(string FolderId)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"WITH  cte
                                  AS ( SELECT   a.FolderId ,
                                                a.ParentId
                                       FROM     Base_NetworkFolder a
                                       WHERE    FolderId = @FolderId
                                       UNION ALL
                                       SELECT   k.FolderId ,
                                                k.ParentId
                                       FROM     dbo.Base_NetworkFolder k
                                                INNER JOIN cte c ON c.FolderId = k.ParentId
                                     )
                            SELECT  *
                            FROM    Base_NetworkFolder
                            WHERE   FolderId IN ( SELECT FolderId FROM cte )");
            parameter.Add(DbFactory.CreateDbParameter("@FolderId", FolderId));
            return DataFactory.Database().FindListBySql<Base_NetworkFolder>(strSql.ToString(), parameter.ToArray());
        }
        /// <summary>
        /// 获取文件、文件夹 列表
        /// </summary>
        /// <param name="keywords">文件名搜索条件</param>
        /// <param name="FolderId">文件夹ID</param>
        /// <param name="DepartmentId">用户ID</param>
        /// <returns></returns>
        public DataTable GetList(string keywords, string FolderId, string UserId)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            if (string.IsNullOrEmpty(keywords))
            {
                strSql.Append(@"SELECT  *
                                FROM    ( SELECT    NetworkFileId AS Id ,
                                                    FolderId ,			
                                                    filename AS fullname,
                                                    FILENAME ,			
                                                    FileSize ,			
                                                    FileType ,			
                                                    Icon ,				
                                                    CreateDate ,		
                                                    CreateUserId,		
                                                    CreateUserName ,	
                                                    2 AS IsPublic ,		  
                                                    0 AS SortCode ,		  
                                                    0 AS Sort			
                                          FROM      Base_NetworkFile
                                          UNION
                                          SELECT    FolderId AS Id ,	
                                                    ParentId AS FolderId ,
                                                    FolderName AS fullname,
                                                    FolderName ,		
                                                    '' AS FileSize ,	
                                                    '文件夹' AS FileType ,
                                                    '-1' AS Icon ,		
                                                    CreateDate ,		
                                                    CreateUserId,		
                                                    CreateUserName ,	
                                                    IsPublic ,			
                                                    SortCode ,			
                                                    1 AS Sort			
                                          FROM      Base_NetworkFolder
                                        ) A WHERE 1=1 AND FolderId = @FolderId ");
            }
            else
            {
                strSql.Append(@" WITH   cte
                                      AS ( SELECT   a.FolderId ,
                                                    a.ParentId
                                           FROM     Base_NetworkFolder a
                                           WHERE    FolderId = @FolderId
                                           UNION ALL
                                           SELECT   k.FolderId ,
                                                    k.ParentId
                                           FROM     dbo.Base_NetworkFolder k
                                                    INNER JOIN cte c ON c.FolderId = k.ParentId
                                         )
                                SELECT  NetworkFileId AS Id ,
                                        FolderId ,			
                                        filename AS fullname ,
                                        FILENAME ,			
                                        FileSize ,			
                                        FileType ,			
                                        Icon ,				
                                        CreateDate ,		
                                        CreateUserId ,		
                                        CreateUserName ,	
                                        2 AS IsPublic ,		  
                                        0 AS SortCode ,		  
                                        0 AS Sort			
                                FROM    Base_NetworkFile
                                WHERE   FileName LIKE @FileName
                                        AND FolderId IN ( SELECT    FolderId FROM cte )");
                parameter.Add(DbFactory.CreateDbParameter("@FileName", "%" + keywords + "%"));
            }
            parameter.Add(DbFactory.CreateDbParameter("@FolderId", FolderId));
            if (!string.IsNullOrEmpty(UserId))
            {
                strSql.Append(" AND CreateUserId = @UserId");
                parameter.Add(DbFactory.CreateDbParameter("@UserId", UserId));
            }
            strSql.Append(" ORDER BY Sort DESC , SortCode ASC,CreateDate DESC");
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }
    }
}