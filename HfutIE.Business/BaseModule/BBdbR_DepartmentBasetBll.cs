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
    /// ���Ż�����Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.03.22 16:10</date>
    /// </author>
    /// </summary>
    public class DepartmentBll : RepositoryFactory<BBdbR_DepartmentBase>
    {
        #region ȫ�ֱ���������
        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BBdbR_DepartmentBase";
        public readonly RepositoryFactory<BBdbR_DepartmentBase> repositoryFactory = new RepositoryFactory<BBdbR_DepartmentBase>();
        #endregion

        #region 1.��ȡ������Ҫ�޸�sql���
        public DataTable GetTree()
        {
            StringBuilder strSql = new StringBuilder();
            //===����ʱ��Ҫ�޸�===
            strSql.Append(@"select    
                              CompanyId AS keys,
                              CompanyCd AS code,
                              CompanyNm AS name,
                              Enabled As IsAvailable,
                              '0' AS parentId,
                              '1' as sort 
                             from BBdbR_CompanyBase where Enabled = '1' ");
            strSql.Append(@" union select  FacId AS keys,     
                             FacCd AS code,
                             FacNm AS name,
                             Enabled As IsAvailable,
                             CompanyId as parentId,  
                            '1' as sort    
                            from BBdbR_FacBase where Enabled = '1' ");         
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion

        #region 2.�����չʾ�����Ҫ�޸�sql���
        /// <summary>
        /// ������Ϣ�������š�   --����-->   �������� 
        ///     ���ݴ���Ĳ�����ͬд��ͬ��sql�����в�ѯ           
        /// </summary>
        /// <param name="areaId">����Ľڵ������</param>
        /// <param name="parentId">�ڵ�ĸ�������</param>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public DataTable GetList(string areaId, string parentId, ref JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            DataTable dt = new DataTable();
            if (string.IsNullOrEmpty(areaId) && string.IsNullOrEmpty(parentId))
            {
                sql = "select a.*,b.FacCd as FacCd,b.FacNm as FacNm,c.DepartmentName as ParentDepartmentName,c.DepartmentCode as ParentDepartmentCode from " + tableName + " a join BBdbR_FacBase b on a.FacId=b.FacId left join BBdbR_DepartmentBase c on  a.ParentDepartmentID=c.DepartmentID where a.Enabled=1 and b.Enabled=1";     //===����ʱ��Ҫ�޸�===
                dt = Repository().FindTableBySql(sql.ToString(), false);
            }
            else
            {
                if (parentId != "0")//�������
                {
                    sql = "select a.*,b.FacCd as FacCd,b.FacNm as FacNm,c.DepartmentName as ParentDepartmentName,c.DepartmentCode as ParentDepartmentCode from " + tableName + " a join BBdbR_FacBase b on a.FacId=b.FacId left join BBdbR_DepartmentBase c on  a.ParentDepartmentID=c.DepartmentID  where a.Enabled=1 and b.Enabled=1 and a.FacId='" + areaId + "'";     //===����ʱ��Ҫ�޸�===
                    dt = Repository().FindTableBySql(sql.ToString(), false);
                }
                else//�����˾
                {
                    sql = "select a.*,b.FacCd as FacCd,b.FacNm as FacNm,d.DepartmentName as ParentDepartmentName,d.DepartmentCode as ParentDepartmentCode  from " + tableName + " a join BBdbR_FacBase b on a.FacId=b.FacId join BBdbR_CompanyBase c on b.CompanyId=c.CompanyId left join BBdbR_DepartmentBase d on  a.ParentDepartmentID=d.DepartmentID  where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and c.CompanyId='" + areaId + "'";     //===����ʱ��Ҫ�޸�===
                    dt = Repository().FindTableBySql(sql.ToString(), false);
                }
            }
            return dt;           
        }

        #endregion

        #region 3.չʾ���
        /// <summary>
        /// �������������Enabled = true������, ��Ϊ��Ч�Ĺ�����ϢGetDepartmentBase
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns>����������������</returns>
        public List<BBdbR_DepartmentBase> GetPageList(JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  BBdbR_DepartmentBase where 1=1");
            List<BBdbR_DepartmentBase> dt = Repository().FindListBySql(strSql.ToString());
            for (int i = 0; i < dt.Count; i++)
            {
                string sql = "select * from BBdbR_FacBase where FacId='" + dt[i].FacId + "'";
                DataTable dt1 = Repository().FindTableBySql(sql.ToString());
                if (dt1.Rows.Count > 0)
                {
                    dt[i].FacId = dt1.Rows[0]["FacNm"].ToString();
                }
            }
            return dt;
        }

       /// <summary>
       /// �������༭��������Դ
       /// </summary>
       /// <param name="KeyValue"></param>
       /// <returns></returns>
        public DataTable GetDepartmentBase(string KeyValue) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            DataTable dt = new DataTable();
            if (KeyValue != "")
            {
                sql = "select a.*,b.FacCd as FacCd,b.FacNm as FacNm from " + tableName + " a join BBdbR_FacBase b on a.FacId=b.FacId where a.Enabled=1 and b.Enabled=1 and a.DepartmentID='"+ KeyValue+"'";     //===����ʱ��Ҫ�޸�===
                dt = Repository().FindTableBySql(sql.ToString(), false);
                return dt;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 4.�����༭����
        //entityʵ���е������Ǵ�ҳ���д����ģ������û���д������
        //entityʵ����ֻ�в����ֶ���ֵ����Ϊҳ����ֻ�ṩ�������ֶθ�ֵ
        //��ҳ������д��������ʵ�壨entity���ķ�ʽ���������ݿ�
        //���У�ʵ���е�IsAvailable�ֶ�û����ҳ������д
        //IsAvailable�ֶε�����������ɾ���������ݿ��е�ĳһ�����ݵ�IsAvailable�ֶε��ֶ�ֵΪtrue��ʾ�����ݴ���
        //�ֶ�ֵΪfalse��ʾ�����ݱ�ɾ��
        //��ɾ�����ݿ��е�ĳһ������ʱֻҪ�޸�IsAvailable�ֶε��ֶ�ֵΪfalse������ɾ����������
        //������ʱ��ʵ���IsAvailable�ֶε�ֵ�޸�Ϊtrue
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Update(BBdbR_DepartmentBase entity) //===����ʱ��Ҫ�޸�===
        {
            return Repository().Update(entity); //���޸ĺ��ʵ����µ����ݿ���
        }
        public int CheckCount(string tableName, string KeyName, string KeyValue)
        {
            //string sql = @"select * from " + tableName + " where Enabled = 1 and " + KeyName + " = '" + KeyValue + "'";
            string sql = @"select * from " + tableName + " where  " + KeyName + " = '" + KeyValue + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Insert(BBdbR_DepartmentBase entity) //===����ʱ��Ҫ�޸�===
        {      
            return Repository().Insert(entity);
        }
        #endregion

        #region 5.ɾ������
        public int Delete(string[] array)
        {
            //����һ������list�����ڴ洢ͨ��������ѯ������Ϣ
            List<BBdbR_DepartmentBase> listEntity = new List<BBdbR_DepartmentBase>(); //===����ʱ��Ҫ�޸�===
            foreach (string keyValue in array)
            { 
                //===����ʱ��Ҫ�޸�===
                BBdbR_DepartmentBase entity = Repository().FindEntity(keyValue);//����������keyValue�������ݿ��в���ʵ�� //===����ʱ��Ҫ�޸�===
                entity.Enabled = 0;          
                listEntity.Add(entity);
            }
            return Repository().Update(listEntity);//�޸����ݿ�
        }
        #endregion

        #region 6.��ѯ��������Ҫ�޸�sql���
        /// <summary>
        ///     ��ѯʱ�ṩ�������ؼ��֣�һ����Condition����һ����keywords 
        ///     Condition�ǹؼ��֣�����ѯ��������Ӧ���ݿ��е�һ���ֶ�
        ///     keywords�ǲ�ѯֵ������ѯ�����ľ���ֵ����Ӧ���ݿ��в�ѯ�����ֶε�ֵ
        ///     ��ѯ��ʱ�򴫵���Condition��keywords
        /// 
        /// </summary>
        /// <param name="keywords">��ѯֵ</param>
        /// <param name="Condition">�ؼ��֣���ѯ������</param>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public DataTable GetPageListByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            DataTable dt = new DataTable();
            if (Condition == "all")
            {
                sql = "select a.*,b.FacCd as FacCd,b.FacNm as FacNm,c.DepartmentName as ParentDepartmentName,c.DepartmentCode as ParentDepartmentCode from " + tableName + " a join BBdbR_FacBase b on a.FacId=b.FacId left join BBdbR_DepartmentBase c on  a.ParentDepartmentID=c.DepartmentID where a.Enabled=1 and b.Enabled=1";     //===����ʱ��Ҫ�޸�===
            }
            else
            {
                if (keywords == "all")
                {
                    sql = "select a.*,b.FacCd as FacCd,b.FacNm as FacNm,c.DepartmentName as ParentDepartmentName,c.DepartmentCode as ParentDepartmentCode from " + tableName + " a join BBdbR_FacBase b on a.FacId=b.FacId left join BBdbR_DepartmentBase c on  a.ParentDepartmentID=c.DepartmentID where a.Enabled=1 and b.Enabled=1";     //===����ʱ��Ҫ�޸�===
                }
                else
                {
                    sql = "select a.*,b.FacCd as FacCd,b.FacNm as FacNm,c.DepartmentName as ParentDepartmentName,c.DepartmentCode as ParentDepartmentCode from " + tableName + " a join BBdbR_FacBase b on a.FacId=b.FacId left join BBdbR_DepartmentBase c on  a.ParentDepartmentID=c.DepartmentID where a.Enabled=1 and b.Enabled=1 and a." + Condition + " like  '%" + keywords + "%'";      //===����ʱ��Ҫ�޸�===
                }
            }
            return Repository().FindTableBySql(sql.ToString(), false);      
        }

        #endregion

        #region 7.ɾ��ʱʹ�ã����ұ�����ĳһ�����������Ƿ������������е�����
        //�ж�Ҫɾ�������������Ƿ�����������е�����
        //�磺��˾��Ϣ����һ����˾��Ϣ����һ���󶨶���������Ϣ
        //    ��ʾ��Щ���������ڸù�˾�ġ�
        //    ��ô��ɾ������һ����˾��Ϣ֮ǰ����Ҫ�жϸù�˾��Ϣ�����Ƿ���˹�����Ϣ
        //
        //�����������þ����жϸù�˾�����Ƿ��й�����Ϣ
        //Condition��ʾҪ��ѯ�ı��е��ֶ����������б�ʾ����������Ϣ���еĹ�˾����
        //keyValue��ʾҪ��ѯ�ı����ֶε�ֵ��
        //����ֵ��ʾ��ѯ���˶���������
        public int FindChildCount(string tableName, string Condition, string keyValue)
        {
            string sql = "select * from " + tableName + " where " + Condition + " = '" + keyValue + "'";
            int count = Repository().FindCountBySql(sql);
            return count;
        }

        /// �༭����������Դ
        /// </summary>
        /// <returns></returns>
        public BBdbR_DepartmentBase GetPlanList(string KeyValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  BBdbR_DepartmentBase where DepartmentID='" + KeyValue + "'");
            List<BBdbR_DepartmentBase> dt = Repository().FindListBySql(strSql.ToString());
            BBdbR_DepartmentBase Dvcentity = new BBdbR_DepartmentBase();
            Dvcentity = dt[0];
            return Dvcentity;
        }
        #endregion

        #region 8.��������Ա
        /// <summary>
        /// ��ȡ������Ա
        /// </summary>
        /// <returns></returns>
        public DataTable GetPlineNm()
        {
            string sql = @"select StfId as id, stfnm from BBdbR_StfBase where Enabled='1' and StfPosn='����������'";
            return Repository().FindTableBySql(sql, false);
        }
        /// <summary>
        /// ��ȡ��Ա��Ϣ
        /// </summary>
        /// <param name="StfId"></param>
        /// <returns></returns>
        public DataTable GetPlineNm2(string StfId)
        {
            string sql = @"select stfnm,phn,email from BBdbR_StfBase where Enabled='1' and StfId=" + "'" + StfId + "'";
            return Repository().FindTableBySql(sql, false);
        }

        /// <summary>
        /// ��ȡ���и�����
        /// </summary>
        /// <returns></returns>
        public DataTable GetPDepartMent()
        {
            string sql = @"select DepartmentID,DepartmentName from BBdbR_DepartmentBase where Enabled='1' and DepartmentType='������'";
            return Repository().FindTableBySql(sql, false);
        }
        #endregion

    }
}




