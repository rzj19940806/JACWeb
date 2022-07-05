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
    /// ��װ�߱�������ֵ��¼��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.12.12 16:22</date>
    /// </author>
    /// </summary>
    public class P_FitEnCodeInfoBll : RepositoryFactory<P_FitEnCodeInfo>
    {
        #region ȫ�ֱ���������
        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "P_FitEnCodeInfo";
        public readonly RepositoryFactory<P_FitEnCodeInfo> repositoryFactory = new RepositoryFactory<P_FitEnCodeInfo>();
        #endregion
        

        #region 3.չʾ���
        
        /// <summary>
        /// �������༭��������Դ
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public DataTable GetList(string PlineNm) //===����ʱ��Ҫ�޸�===
        {
            string sql = $"select FitEnCodeInfoId,TB.PlineQueID,TA.PlineNm,TA.No,TA.WcNm,TA.VIN,TB.VIN VIN2,IIF(TA.VIN=TB.VIN,0,1) YN,TA.CodeValue,StartPoint,EndPoint from(select ROW_NUMBER()over(partition by A.PlineCd order by CodeValue) No,FitEnCodeInfoId,VIN,CodeValue,PlineNm,WcNm,A.PlineId,C.StartPoint,C.EndPoint from P_FitEnCodeInfo A with(nolock) join BBdbR_PlineBase B with(nolock) on A.PlineId = B.PlineId left join BBdbR_WcBase C with(nolock) on CodeValue> C.StartPoint and CodeValue<C.EndPoint and A.PlineId= C.PlineId) TA join(select ROW_NUMBER()over(partition by PlineCd order by CarQuene desc) No,PlineQueID, VIN, PlineCd, CarQuene, PlineId from P_LineProductionQueue_Pro with (nolock) where PlineCd in ('Line-10', 'Line-11', 'Line-12', 'Line-13', 'Line-14') and AVICd<>'FIT-2') TB on TA.No = TB.No and TA.PlineId = TB.PlineId where PlineNm = '{PlineNm}' or '{PlineNm}'=''";     //===����ʱ��Ҫ�޸�===
            DataTable dt = Repository().FindTableBySql(sql.ToString(), false);
            return dt;
        }
        #endregion



        #region VINУ׼
        public string VINToOk(string PlineNm) //===����ʱ��Ҫ�޸�===
        {
            //StringBuilder stringBuilder = new StringBuilder();
            //stringBuilder.Append( $"update P_FitEnCodeInfo set VIN=A.VIN from (select VIN, ROW_NUMBER()over(order by CarQuene desc) ID from P_LineProductionQueue_Pro with(nolock) where PlineNm = '{PlineNm}') A join (select FitEnCodeInfoId, VIN, ROW_NUMBER() over(order by CodeValue) ID from P_FitEnCodeInfo with(nolock) where PlineCd= (select PlineCd from BBdbR_PlineBase where PlineNm = '{PlineNm}')) B on A.ID = B.ID where P_FitEnCodeInfo.FitEnCodeInfoId = B.FitEnCodeInfoId");     //===����ʱ��Ҫ�޸�===
            ArrayList sqllist = new ArrayList();
            string vintonull = $"update P_FitEnCodeInfo  set VIN = '9999' where FitEnCodeInfoId in (select FitEnCodeInfoId from P_FitEnCodeInfo a left join BBdbR_PlineBase b on a.PlineCd = b.PlineCd where PlineNm = '{PlineNm}' )";
            string vintook = $"update P_FitEnCodeInfo set VIN=A.VIN from (select VIN, ROW_NUMBER()over(order by CarQuene desc) ID from P_LineProductionQueue_Pro with(nolock) where PlineNm = '{PlineNm}') A join (select FitEnCodeInfoId, VIN, ROW_NUMBER() over(order by CodeValue) ID from P_FitEnCodeInfo with(nolock) where PlineCd= (select PlineCd from BBdbR_PlineBase where PlineNm = '{PlineNm}')) B on A.ID = B.ID where P_FitEnCodeInfo.FitEnCodeInfoId = B.FitEnCodeInfoId";
            //sqllist.Add(sqlupdateoldqueue);
            sqllist.Add(vintonull);
            sqllist.Add(vintook);
            string res =  DbHelperSQL.ExecuteSqlTran1(sqllist); //ͬʱִ�У�ʵ������ع�
            return res;
                
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
        public int Update(P_FitEnCodeInfo entity) //===����ʱ��Ҫ�޸�===
        {
            return Repository().Update(entity); //���޸ĺ��ʵ����µ����ݿ���
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



        #region 5.ɾ������
        //array ��Ҫɾ������Ϣ������������
        //ɾ������ĳһ�����ݱ�ʾ�����и������ݵ�Enabled����Ϊ0�����������ɾ��������
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Delete(string[] array)
        {

            int count = 0;
            foreach (string keyValue in array)
            {
                StringBuilder del = new StringBuilder();
                del.Append($"delete from P_LineProductionQueue_Pro where PlineQueID = '{keyValue}'");

                count += DbHelperSQL.ExecuteSql(del.ToString());//�޸����ݿ�
                                                                //===����ʱ��Ҫ�޸�===

            }
            return count;
        }
        #endregion

    }
}