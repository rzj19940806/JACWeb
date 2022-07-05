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
    /// 指导文件
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.09 11:15</date>
    /// </author>
    /// </summary>
    public class BBdbR_GuidanceFileBll : RepositoryFactory<BBdbR_GuidanceFile>
    {
        public DataTable GetTree()
        {
            StringBuilder strSql = new StringBuilder();
            //===复制时需要修改===（三级数）
            //车间节点
            strSql.Append(@"WITH Tab (keys,code,name,IsAvailable,parentId,sort) AS
                (
                    SELECT DataDictionaryId keys,code,FullName name,Enabled IsAvailable,convert(varchar(50),'0') parentId,0 AS sort FROM Base_DataDictionary WHERE Code='File' and DeleteMark='0'
                    UNION ALL
                    SELECT d.DataDictionaryId,d.code,d.FullName,d.Enabled,d.parentId,Tab.sort + 1 FROM Base_DataDictionary d
                    JOIN Tab
                    ON
                      d.ParentId=Tab.keys 
                  )
                select * from Tab");
            return Repository().FindTableBySql(strSql.ToString());
        }

    }
}