//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2021
// Software Developers @ HfutIE 2021
//=====================================================================================

using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// ��λ������Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.03.23 21:51</date>
    /// </author>
    /// </summary>
    public class BBdbR_QualityCheckPointCarComponentConfigBll : RepositoryFactory<BBdbR_QualityCheckPointCarComponentConfig>
    {
        #region ȫ�ֱ���������

        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BBdbR_QualityCheckPointCarComponentConfig";//===����ʱ��Ҫ�޸�===
        string tableName1 = "BBdbR_WcBase";//===����ʱ��Ҫ�޸�===
        #endregion

        #region 1.��ȡ������Ҫ�޸�sql���
        /// <summary>
        /// ������Ϣ������λ��   --����-->   �����ߡ�  --����-->  �����Ρ�  --����-->  �����䡿
        /// </summary>
        /// <returns></returns>
        public DataTable GetTreeQuality()
        {
            StringBuilder strSql = new StringBuilder();
            //===����ʱ��Ҫ�޸�===����������
            //����ڵ�
            strSql.Append(@"select c.WorkshopId As keys,
                                   c.WorkshopCd As code,
                                   c.WorkshopNm As name,
                                   c.Enabled As IsAvailable,
                                   '0' as parentId,
                                   '0' as sort             
                                   from BBdbR_WorkshopBase c where c.Enabled = '1' and c.WorkshopCd='ZLCJXN01'  ");
            //���νڵ�
            strSql.Append(@"union select 
                                    b.WorkSectionId AS keys,
                                    b.WorkSectionCd AS code, 
                                    b.WorkSectionNm AS name, 
                                    b.Enabled As IsAvailable,
                                    b.WorkshopId as parentId,
                                    '1' as sort 
                                    from BBdbR_WorkSectionBase b,BBdbR_WorkshopBase c where b.WorkshopId = c.WorkshopId and b.Enabled= '1'  ");
            //���߽ڵ�
            strSql.Append(@" union select    
                                    a.PlineId AS keys,
                                    a.PlineCd AS code,
                                    a.PlineNm AS name,
                                    a.Enabled As IsAvailable,
                                    a.WorkSectionId AS parentId,
                                    '2' as sort 
                             from  BBdbR_PlineBase a,BBdbR_WorkSectionBase b 
                             where a.WorkSectionId=b.WorkSectionId and a.Enabled = '1' and a.Enabled = '1' order by code asc");
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion

        #region 2.�����չʾ�����Ҫ�޸�sql���
        /// <summary>
        /// ������Ϣ��
        ///     ���ݴ���Ĳ�����ͬд��ͬ��sql�����в�ѯ           
        /// </summary>
        /// <param name="areaId">����Ľڵ������</param>
        /// <param name="parentId">�ڵ�ĸ�������</param>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public DataTable GetListQuality(string areaId, string parentId, string sort, ref JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            DataTable dt = new DataTable();
            var a = string.IsNullOrEmpty(areaId) && string.IsNullOrEmpty(parentId);
            if (areaId == "''" && parentId == "''")
            {
                sql = "select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName1 + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId join BBdbR_WorkshopBase d on d.WorkshopId=c.WorkshopId where a.Enabled=1 and b.Enabled=1 and d.WorkshopCd='ZLCJXN01' order by a.sort asc";     //===����ʱ��Ҫ�޸�===
            }
            else
            {
                if (parentId != "0")
                {
                    if (sort == "0")
                    {
                        sql = "select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName1 + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and c.WorkSectionId='" + areaId + "' order by a.sort asc";     //===����ʱ��Ҫ�޸�===
                    }
                    else if (sort == "1")
                    {
                        sql = "select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName1 + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and c.WorkSectionId='" + areaId + "' order by a.sort asc";     //===����ʱ��Ҫ�޸�===
                    }
                    else
                    {
                        sql = "select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName1 + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId where a.Enabled=1 and b.Enabled=1 and a.PlineId='" + areaId + "' order by a.sort asc";     //===����ʱ��Ҫ�޸�===Ҫ�޸�===
                    }
                }
                else
                {
                    sql = "select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName1 + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId join BBdbR_WorkshopBase d on c.WorkshopId=d.WorkshopId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and d.Enabled=1 and d.WorkshopId='" + areaId + "' order by a.sort asc";     //===����ʱ��Ҫ�޸�===
                }
            }
            return Repository().FindTableBySql(sql.ToString(), false);
        }
        #endregion

        #region 3.����ҳ�����Ѿ����ù��������Ϣ

        public DataTable GetMatList(string KeyValue, string isCH)//����ҳ�����Ѿ����ù��������Ϣ
        {
            string sql = "";
            if (isCH == "CH")
            {
                sql = $"select a.*,c.CarPositionNm,c.CarPositionCd from {tableName} a left join BBdbR_QualityCarPositionBase_Add c on a.CarPositionId=c.CarPositionId where a.Enabled=1 and a.QualityCheckPointId= '{KeyValue}' and c.Type='CH'  ";

            }
            else if (isCH == "TZ")
            {
                sql = $"select a.*,c.CarPositionNm,c.CarPositionCd from {tableName} a left join BBdbR_QualityCarPositionBase_Add c on a.CarPositionId=c.CarPositionId where a.Enabled=1 and a.QualityCheckPointId= '{KeyValue}' and c.Type='TZ' ";
            }
            else
            {
                sql = $"select a.*,c.CarPositionNm,c.CarPositionCd from {tableName} a left join BBdbR_QualityCarPositionBase c on a.CarPositionId=c.CarPositionId where a.Enabled=1 and a.QualityCheckPointId= '{KeyValue}' ";
            }
            return Repository().FindTableBySql(sql, false);
        }
        #endregion

        #region 4.�ڱ��м��ؼ�����û���ù�������嵥CarPositionInsert
        public DataTable GetNotConfig(string WcId, string Condition, string keywords, string isCH, ref JqGridParam jqgridparam)//�ڱ��м���û���ù�������嵥
        {
            StringBuilder strSql = new StringBuilder();
            if (isCH == "CH")
            {
                if (Condition == "all" || Condition == "")
                {
                    strSql.Append(@"select CarPositionId,CarPositionCd,CarPositionNm,Enabled,Rem from BBdbR_QualityCarPositionBase_Add where CarPositionId not in(select distinct CarPositionId from " + tableName + " where Enabled=1 and QualityCheckPointId='" + WcId + "' ) and Enabled=1  and Type='CH'" );
                }
                else
                {
                    strSql.Append(@"select CarPositionId,CarPositionCd,CarPositionNm,Enabled,Rem from BBdbR_QualityCarPositionBase_Add where CarPositionId not in(select distinct CarPositionId from " + tableName + " where Enabled=1 and QualityCheckPointId='" + WcId + "' ) and " + Condition + " like '%" + keywords + "%' and Enabled=1  and Type='CH'");
                }
            }
            else if(isCH == "TZ")
            {
                if (Condition == "all" || Condition == "")
                {
                    strSql.Append(@"select CarPositionId,CarPositionCd,CarPositionNm,Enabled,Rem from BBdbR_QualityCarPositionBase_Add where CarPositionId not in(select distinct CarPositionId from " + tableName + " where Enabled=1 and QualityCheckPointId='" + WcId + "' ) and Enabled=1  and Type='TZ'");
                }
                else
                {
                    strSql.Append(@"select CarPositionId,CarPositionCd,CarPositionNm,Enabled,Rem from BBdbR_QualityCarPositionBase_Add where CarPositionId not in(select distinct CarPositionId from " + tableName + " where Enabled=1 and QualityCheckPointId='" + WcId + "' ) and " + Condition + " like '%" + keywords + "%' and Enabled=1  and Type='TZ'");
                }
            }
            else
            {
                if (Condition == "all" || Condition == "")
                {
                    strSql.Append(@"select CarPositionId,CarPositionCd,CarPositionNm,Enabled,Rem from BBdbR_QualityCarPositionBase where CarPositionId not in(select distinct CarPositionId from " + tableName + " where Enabled=1 and QualityCheckPointId='" + WcId + "' ) and Enabled=1");
                }
                else
                {
                    strSql.Append(@"select CarPositionId,CarPositionCd,CarPositionNm,Enabled,Rem from BBdbR_QualityCarPositionBase where CarPositionId not in(select distinct CarPositionId from " + tableName + " where Enabled=1 and QualityCheckPointId='" + WcId + "' ) and " + Condition + " like '%" + keywords + "%' and Enabled=1");
                }
            }
            return Repository().FindTableBySql(strSql.ToString(), false);
        }


        #endregion

        #region 5.�ύ����
        public int CarPositionInsert(BBdbR_QualityCheckPointCarComponentConfig entity)
        {
            return Repository().Insert(entity);
            //return 1;
        }
        #endregion

        #region 6.ɾ������
        public int ConfigDelete(string KeyValue)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"update "+tableName+ " set Enabled=0 where ConfigId='" + KeyValue + "'");
            int isok = Repository().ExecuteBySql(sql);
            return isok;
        }

        #endregion

        #region 7.��ѯ��������Ҫ�޸�sql���
        /// <summary>
        ///     ��ѯʱ�ṩ�������ؼ��֣�һ����Condition����һ����keywords
        ///     
        ///     Condition�ǹؼ��֣�����ѯ��������Ӧ���ݿ��е�һ���ֶ�
        ///     keywords�ǲ�ѯֵ������ѯ�����ľ���ֵ����Ӧ���ݿ��в�ѯ�����ֶε�ֵ
        ///     ��ѯ��ʱ�򴫵���Condition��keywords
        /// 
        /// </summary>
        /// <param name="keywords">��ѯֵ</param>
        /// <param name="Condition">�ؼ��֣���ѯ������</param>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns>��ѯ�����ݣ��б�</returns>
        public DataTable GetPageListByConditionQuality(string keywords, string Condition, JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            if (Condition == "all")
            {
                sql = @"select * from " + tableName1 + " where Enabled = 1 order by sort asc";
                return Repository().FindTableBySql(sql,false);
            }
            else
            {
                if (keywords != "all")
                {
                    //����������ѯ
                    sql = @"select * from " + tableName1 + " where  " + Condition + " like  '%" + keywords + "%' and Enabled = 1 order by sort asc";
                    return Repository().FindTableBySql(sql.ToString(), false);
                }
                else
                {
                    sql = @"select * from " + tableName1 + " where Enabled = 1 order by sort asc";
                    return Repository().FindTableBySql(sql.ToString(), false);
                }
            }
        }
        #endregion
    }
}