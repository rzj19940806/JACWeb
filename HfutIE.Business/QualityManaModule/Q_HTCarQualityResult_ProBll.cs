//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2022
// Software Developers @ HfutIE 2022
//=====================================================================================

using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// �庸Ϳװ�ʼ�����¼���̱�
    /// <author>
    ///		<name>she</name>
    ///		<date>2022.04.25 09:54</date>
    /// </author>
    /// </summary>
    public class Q_HTCarQualityResult_ProBll : RepositoryFactory<Q_HTCarQualityResult_Pro>
    {
        #region �����̺ź͹��β�ѯ
        public List<Q_HTCarQualityResult_Pro> GetListByChassisNumber(string ChassisNumber,string QualityCheckPointGroupNm) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            List<Q_HTCarQualityResult_Pro> dt;
            sql = @"select * from Q_HTCarQualityResult_Pro where Enabled = '1' and VIN like '%" + ChassisNumber + "' and QualityCheckPointGroupNm = '" + QualityCheckPointGroupNm + "'";
            dt = Repository().FindListBySql(sql.ToString());
            return dt;
        }

        #endregion


        #region ɾ������
        //array ��Ҫɾ������Ϣ������������
        //ɾ������ĳһ�����ݱ�ʾ�����и������ݵ�Enabled����Ϊ0�����������ɾ��������
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Delete(string KeyValue)
        {

            ////����һ������list�����ڴ洢ͨ��������ѯ������Ϣ
            List<Q_HTCarQualityResult_Pro> listEntity = new List<Q_HTCarQualityResult_Pro>(); //===����ʱ��Ҫ�޸�===
            //===����ʱ��Ҫ�޸�===
            Q_HTCarQualityResult_Pro entity = Repository().FindEntity(KeyValue);//����������keyValue�������ݿ��в���ʵ�� //===����ʱ��Ҫ�޸�===
            entity.Enabled = "0";//����ʵ���IsAvailable���Ը�Ϊfalse
            entity.Modify(KeyValue);
            listEntity.Add(entity);
            return Repository().Update(listEntity);//�޸����ݿ�
        }
        #endregion
    }
}