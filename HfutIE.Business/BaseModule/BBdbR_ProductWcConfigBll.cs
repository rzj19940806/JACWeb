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
    /// ��λ��������
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.06 20:03</date>
    /// </author>
    /// </summary>
    public class BBdbR_ProductWcConfigBll : RepositoryFactory<BBdbR_ProductWcConfig>
    {
        #region 1.��ȡ������Ҫ�޸�sql���
        public DataTable GetTree(string productMatId)
        {
            StringBuilder strSql = new StringBuilder();
            //===����ʱ��Ҫ�޸�===
            strSql.Append($@"select PlineId as keys,PlineCd as code,PlineNm as name,'0'as parentId,'0'as sort from BBdbR_PlineBase A with(nolock) where Enabled=1 and exists(select 1 from BBdbR_WcBase with(nolock) where PlineId=A.PlineId) and PlineCd like 'Line-%' union select WcId as keys, WcCd as code,WcNm as name,B.PlineId as parentId,'1' as sort from BBdbR_WcBase A with(nolock),BBdbR_PlineBase B with(nolock) where B.PlineId=A.PlineId and A.Enabled=1 and A.WcId in (select ISNULL(C.WcId,D.WcId) from BBdbR_ProductWcMatConfig C with(nolock) join BBdbR_MatBase D with(nolock) on C.MatId=D.MatId and C.ProductId='{productMatId}') order by code asc");
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion

        public DataTable GridPageJson(JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
                strSql.Append($"select MatId,MatCd,MatNm,CarType,CarColor1 from BBdbR_ProductBase with(nolock) where Enabled=1 order by {jqgridparam.sidx} {jqgridparam.sord}");
            return Repository().FindTableBySql(strSql.ToString(),false);
        }
        public DataTable GridPageJson1(string VIN, string ProductCd, string ProductNm,JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append($"select MatId,MatCd,MatNm,CarType,CarColor1 from BBdbR_ProductBase with(nolock) where Enabled = 1 and(MatCd in (select distinct MatCd from P_ProducePlan_Pro where @VIN1 <> '' and Enabled = 1 and VIN like @VIN2) or @VIN1 = '') and MatCd like @ProductCd and MatNm like @ProductNm order by {jqgridparam.sidx} {jqgridparam.sord}");

            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@VIN1", VIN ));
            parameter.Add(DbFactory.CreateDbParameter("@VIN2", "%" + VIN + "%"));
            parameter.Add(DbFactory.CreateDbParameter("@ProductCd", "%" + ProductCd + "%"));
            parameter.Add(DbFactory.CreateDbParameter("@ProductNm", "%" + ProductNm + "%"));

            return DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
        }
        #region 2.�����չʾ�����Ҫ�޸�sql���
        /// <summary>
        ///     ���ݴ���Ĳ�����ͬд��ͬ��sql�����в�ѯ
        ///             
        /// </summary>
        /// <param name="areaId">����Ľڵ������</param>
        /// <param name="sort">����Ľڵ�Ĳ㼶���</param>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public DataTable GetList(string areaId, string text, string value, string sort, ref JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            if (sort != "0")
            {
                strSql.Append(@"select ProductClassId,MatId,MatCd,MatNm,FileNm,FileCd,LockStationNum,Rem from BBdbR_ProductWcConfig with(nolock) where  WcId='" + areaId + "' and Enabled=1");
            }
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion

        #region 3.����δ���ò�Ʒ
        public DataTable GetNotConfigProduct(ref JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select *from BBdbR_MatBase where MatId not in(select distinct MatId  from BBdbR_ProductWcConfig where Enabled=1 ) and MatCatg='��Ʒ' and Enabled=1");
            return Repository().FindTableBySql(strSql.ToString(), false);
        }
        #endregion

        #region 3.����չʾδ���ò�Ʒ
        public DataTable GetNotConfigProductbycondition(string keywords, string Condition, ref JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            if (Condition == "all")
            {
                strSql.Append(@"select *from BBdbR_MatBase where MatId not in(select distinct MatId  from BBdbR_ProductWcConfig where Enabled=1 ) and MatCatg='��Ʒ' and Enabled=1");
            }
            else
            {
                strSql.Append(@"select *from BBdbR_MatBase where MatId not in(select distinct MatId  from BBdbR_ProductWcConfig where Enabled=1 ) and MatCatg='��Ʒ' and " + Condition + " like '%" + keywords + "%' and Enabled=1");
            }
            return Repository().FindTableBySql(strSql.ToString(), false);
        }
        #endregion

        #region 4.���������ò�Ʒ�ı�û�õ���
        public DataTable GetProductList(string array)
        {
            string[] str1 = array.Split(',');
            string str2 = "";
            for (int i = 0; i < str1.Length - 1; i++)
            {
                if (i == str1.Length - 2)
                {
                    str2 += "'" + str1[i] + "'";
                }
                else
                {
                    str2 += "'" + str1[i] + "'" + ",";
                }
            }
            str2 = "(" + str2 + ")";
            string str = $"select  *from BBdbR_MatBase where MatId in{str2}";
            return Repository().FindTableBySql(str, false);
        }
        #endregion

        #region 5.�ύ������Ʒ��
        #region 5.1��ȡѡ�в����µ����й�λ
        public DataTable GetWcdt(string PlineId)
        {
            string sql = $"select WcId,WcCd,WcNm from BBdbR_WcBase where PlineId='{PlineId}' and Enabled=1";
            return Repository().FindTableBySql(sql, false);
        }
        #endregion

        #region 5.2���뵽��Ʒ��λ���ñ���
        public int Insert(BBdbR_ProductWcConfig entity)
        {
            Repository().Insert(entity);
            return 1;
        }
        #endregion
        #region

        #endregion
        #endregion

        #region 6.�༭ͼƬ��
        #region 6.1ͼƬ���������
        public DataTable GetFaileCd(string matid)
        {
            string sql = "select * from BBdbR_MatFileConfig where FileTy='0' and Enabled='1' and MatId='" + matid + "'";
            return Repository().FindTableBySql(sql, false);
        }
        #endregion
        #region 6.2���ݱ�ż���
        public DataTable GetFaileData(string filecd, string matid)
        {
            string sql = "select * from BBdbR_MatFileConfig where FileTy='0' and Enabled='1' and FileCd='" + filecd + "' and MatId='" + matid + "'";
            return Repository().FindTableBySql(sql, false);
        }
        #endregion
        #region 6.3����ͼƬ��Ϣ�������ݿ�
        public int SubmitPicture(string newfilecd, string newfilenm, string newconfigid, string productclassid)
        {
            StringBuilder strsql = new StringBuilder();
            strsql.Append(@"update BBdbR_ProductWcConfig set FileNm='" + newfilenm + "',FileCd='" + newfilecd + "',ConfigId='" + newconfigid + "' where ProductClassId='" + productclassid + "'");
            //string sql = $"updata BBdbR_ProductWcConfig set FileNm='{newfilenm}',FileCd='{newfilecd}',ConfigId='{newconfigid}' where ProductClassId='{productclassid}'";
            int isok = Repository().ExecuteBySql(strsql);
            return isok;
        }
        #endregion
        #endregion

        //7.�������ü�BBdbR_ProductWcMatConfigBll(MyBll2)

        #region 8.ɾ������
        public int ConfigMatDelete(string MatKeyValue)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"update BBdbR_ProductWcMatConfig set Enabled=0 where ProductClassMatId='" + MatKeyValue + "'");
            int isok = Repository().ExecuteBySql(sql);
            return isok;
        }

        #endregion

        #region 9.��ӡ
        public DataTable Getprintdata1(string productMatId, string PlineCd)//�õ�ѡ�еĲ�Ʒ���ڵĲ���
        {
            string sql;
            if (PlineCd == "Line-12" || PlineCd == "Line-23")//����A��������
            {
                string PlineCd2;
                if (PlineCd == "Line-12") PlineCd2 = "Line-13";
                else PlineCd2 = "Line-21";

                sql = $"select b.MatCd,b.MatNm,b.MatNum,b.RsvFld1 WcCd from produceBom b join BBdbR_WcBase w on b.RsvFld1=w.WcCd join BBdbR_PlineBase p on w.PlineId=p.PlineId where ProductId = '{productMatId}' and(p.PlineCd = '{PlineCd}' or p.PlineCd = '{PlineCd2}') and IsPrint = 1 order by b.RsvFld1";
            }
            else if (PlineCd == "Line-22")//����
            {
                sql = $"select b.MatCd,b.MatNm,b.MatNum,b.RsvFld1 WcCd from produceBom b join BBdbR_WcBase w on b.RsvFld1 = w.WcCd join BBdbR_PlineBase p on w.PlineId = p.PlineId where ProductId = '{productMatId}' and(p.PlineCd = '{PlineCd}'  or b.RsvFld1 = 'C-14') and IsPrint = 1 order by b.RsvFld1";
            }
            else if(PlineCd == "Line-10" || PlineCd == "Line-11" || PlineCd == "Line-14" || PlineCd == "Line-20")//����AB����װ������
            {
                sql = $"select b.MatCd,b.MatNm,b.MatNum,b.RsvFld1 WcCd from produceBom b join BBdbR_WcBase w on b.RsvFld1=w.WcCd join BBdbR_PlineBase p on w.PlineId=p.PlineId where ProductId='{productMatId}' and  p.PlineCd = '{PlineCd}' and IsPrint=1 order by b.RsvFld1";
            }
            else return null;

            //sql = $"select PlineTyp,PlineNm,MatCd,MatNm from produceBom A with(nolock) join BBdbR_WcBase B with(nolock) on A.RsvFld1=B.WcCd and IsPrint=1 and ProductId='{productMatId}' join BBdbR_PlineBase C with(nolock) on B.PlineId = C.PlineId and C.PlineCd = '{PlineCd}' order by B.WcCd";
            return Repository().FindTableBySql(sql, false);
        }

        public DataTable Getprintdata2(string productMatId, string PlineCd)//�õ�ѡ�еĲ�Ʒ���ڵĲ���
        {

            string NoNumSql = "";
            if (PlineCd == "Line-23")//������
            {
                NoNumSql = $"select b.MatCd,b.MatNm,b.MatNum,b.WcCd from produceBom b join BBdbR_WcBase w on b.WcId=w.WcId join BBdbR_PlineBase p on w.PlineId=p.PlineId where ProductId='{productMatId}' and p.PlineCd in ('{PlineCd}','Line-21') and IsScan=1 and MatNum>0 order by b.WcCd";
            }
            else if (PlineCd == "Line-22")//����
            {
                NoNumSql = $"select b.MatCd,b.MatNm,b.MatNum,b.WcCd from produceBom b join BBdbR_WcBase w on b.WcId=w.WcId join BBdbR_PlineBase p on w.PlineId=p.PlineId where ProductId='{productMatId}' and  (p.PlineCd = '{PlineCd}' or b.WcId='53') and IsScan=1 and MatNum>0 order by b.WcCd";
            }
            else if (PlineCd == "Line-20")//����
            {
                NoNumSql = $"select b.MatCd,b.MatNm,b.MatNum,b.WcCd from produceBom b join BBdbR_WcBase w on b.WcId=w.WcId join BBdbR_PlineBase p on w.PlineId=p.PlineId where ProductId='{productMatId}' and p.PlineCd = '{PlineCd}' and IsScan=1 and MatNum>0 order by b.WcCd";
            }
            else return null;
            DataTable NoNumscanData = Repository().FindTableBySql(NoNumSql, false);//��ʼɨ�뵥
            DataTable scanData = NoNumscanData.Clone();//����У׼ɨ�뵥
            for (int i = 0; i < NoNumscanData.Rows.Count; i++)
            {
                if (int.Parse((NoNumscanData.Rows[i]["MatNum"].ToString())) > 0)
                {
                    int num = int.Parse((NoNumscanData.Rows[i]["MatNum"].ToString()));
                    NoNumscanData.Rows[i]["MatNum"] = 0;
                    for (int j = 0; j < num; j++)
                    {
                        scanData.ImportRow(NoNumscanData.Rows[i]);
                    }
                }
            }
            return scanData;
        }
        public DataTable GetPlineData(string plinecd)
        {
            return Repository().FindTableBySql($"select top 1 * from BBdbR_PlineBase where Enabled='1' and PlineCd='{plinecd}'",false);
        }

        #endregion
    }
}