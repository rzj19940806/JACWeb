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
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// �����ĵ�����
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.04.02 16:27</date>
    /// </author>
    /// </summary>
    public class BBdbR_MatFileConfigBll : RepositoryFactory<BBdbR_MatFileConfig>
    {
        #region ȫ�ֱ���������
        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BBdbR_MatFileConfig";//===����ʱ��Ҫ�޸�===
        #endregion
        #region 1.ҳ����
        /// <summary>
        /// ���ϲ�ѯ��չʾҳ����
        /// </summary>
        /// <param name="CheckId"></param>
        /// <returns></returns>
        public List<BBdbR_MatFileConfig> GetList(string KeyValue)
        {
            StringBuilder strSql = new StringBuilder();
            List<BBdbR_MatFileConfig> dt;
            strSql.Append(@"SELECT  * FROM  BBdbR_MatFileConfig where  MatId='" + KeyValue + "' and Enabled='1' ");
            dt = Repository().FindListBySql(strSql.ToString());
            return dt;
        }
        #endregion
        #region 3.չʾ���
        /// <summary>
        /// �������������IsAvailable = true������
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns>����������������</returns>
        public List<BBdbR_MatFileConfig> GetPageList(JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            return Repository().FindList("Enabled", "1");
        }
        #endregion
        #region 4.�༭����
        //���޸ĺ��ʵ����µ����ݿ���
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Update(BBdbR_MatFileConfig entity) //===����ʱ��Ҫ�޸�===
        {
            return Repository().Update(entity); //���޸ĺ��ʵ����µ����ݿ���
        }
        #endregion
        #region 5.���ĳ���ֶε��ֶ�ֵ�Ƿ����
        /// <summary>
        ///   Enabled = 1��������ĳ���ֶΣ�KeyName�����ֶ�ֵ��KeyValue���Ƿ����
        /// </summary>
        /// <param name="KeyName">�ֶ���</param>
        /// <param name="KeyValue">�ֶ�ֵ</param>
        /// <returns>���ص��������ı��а������ֶ�ֵ�ļ�¼����</returns>
        public int CheckCount(string KeyName, string KeyValue)
        {
            string sql = @"select * from " + "BBdbR_MatFileConfig" + " where Enabled = '1' and " + KeyName + "  = '" + KeyValue + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }
        #endregion
        #region 6.��������
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
        public int Insert(BBdbR_MatFileConfig entity) //===����ʱ��Ҫ�޸�===
        {
            //entity.Enabled = "1";

            return Repository().Insert(entity);
        }
        #endregion

        #region 7.ɾ������
        //array ��Ҫɾ������Ϣ������������
        //ɾ������ĳһ�����ݱ�ʾ�����и������ݵ�Enabled����Ϊ0�����������ɾ��������
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Delete(string[] array)
        {
            StringBuilder sql = new StringBuilder();
            int isok = 0;
            List<BBdbR_MatFileConfig> listEntity = new List<BBdbR_MatFileConfig>();
            //����һ������list�����ڴ洢ͨ��������ѯ������Ϣ
            //List<BBdbR_CarPartBase> listEntity = new List<BBdbR_CarPartBase>(); //===����ʱ��Ҫ�޸�===
            foreach (string keyValue in array)
            {

                //===����ʱ��Ҫ�޸�===
                BBdbR_MatFileConfig entity = Repository().FindEntity(keyValue);//����������keyValue�������ݿ��в���ʵ�� //===����ʱ��Ҫ�޸�===
                                                                         //entity.Enabled = "0";//����ʵ���IsAvailable���Ը�Ϊfalse
                                                                         //entity.Modify(keyValue);
                if (entity.FileContent == null)
                {
                    entity.Enabled = "0";
                    listEntity.Add(entity);

                }
                else
                {
                    sql.Append(@"update " + tableName + " set FileContent = @content,Enabled = '0' where ConfigId = '" + keyValue + "'");
                    var dbparameter = new SqlParameter("@content", entity.FileContent);
                    isok += Repository().ExecuteBySql(sql, new[] { dbparameter });
                    return isok;//��ɾ���ݿ�
                }

            }
            return Repository().Update(listEntity);//�޸����ݿ�

        }

        #endregion
        #region 8.��ѯ��������Ҫ�޸�sql���
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
        public List<BBdbR_MatFileConfig> GetPageListByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            List<BBdbR_MatFileConfig> dt;
            if (Condition == "all")
            {
                sql = @"select * from " + tableName + " where 1 = 1 and Enabled='1'";
                dt = Repository().FindListBySql(sql.ToString());
            }
            else
            {
                //����������ѯ
                sql = @"select * from " + tableName + " where  " + Condition + " like  '%" + keywords + "%'and Enabled='1'";
                dt = Repository().FindListBySql(sql.ToString());
            }
            return dt;
        }
        #endregion

        #region 8.����ͼƬ
        public int InsertPicture(string FileCd, byte[] image, string name) //===����ʱ��Ҫ�޸�===
        {

            //long longKeyValue = Convert.ToInt64(KeyValue);
            StringBuilder sql = new StringBuilder();
            byte[] img = image;
            Image images = Image.FromStream(new MemoryStream(img));
            int width = images.Width;
            int height = images.Height;
            string Name = "";
            if (name != null && !String.IsNullOrEmpty(name))
            {
                Name = name.Substring(1, name.Length - 1);
            }

            sql.Append(@"update " + tableName + " set FileContent = @content where FileCd = '" + FileCd + "'");
            var dbparameter = new SqlParameter("@content", image);
            return Repository().ExecuteBySql(sql, new[] { dbparameter });
        }
        #endregion

        #region ��ȡͼƬ
        public DataTable getPicture(string strID)
        {
            DataTable dt = new DataTable();
            if (strID != null && !String.IsNullOrEmpty(strID))
            {
                Stopwatch watch = CommonHelper.TimerStart();
                string sql = "select MatImg,MdfCd,MdfNm,Rem from BBdbR_MatFileConfig where MatId = " + strID + "";
                //string sql = "select Image,Reserve1 from A_ProductBase where ID = " + ID + "";
                dt = Repository().FindTableBySql(sql);
                string costtime = CommonHelper.TimerEnd(watch);
            }
            return dt;
        }
        #endregion

        #region �����ļ�
        public int InsertDocument(string FileCd, byte[] document) //===����ʱ��Ҫ�޸�===
        {

            //long longKeyValue = Convert.ToInt64(KeyValue);
            StringBuilder sql = new StringBuilder();
            //byte[] docu = document;
            //Image duocus = Image.FromStream(new MemoryStream(docu));
            //string Name = "";
            //if (name != null && !String.IsNullOrEmpty(name))
            //{
            //    Name = name.Substring(1, name.Length - 1);
            //}

            sql.Append(@"update " + tableName + " set FileContent = @content where FileCd = '" + FileCd + "'");
            var dbparameter = new SqlParameter("@content", document);
            return Repository().ExecuteBySql(sql, new[] { dbparameter });
        }
        #endregion

        #region �༭_0628
        public DataTable GetMatInfor(string KeyValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from BBdbR_MatFileConfig where ConfigId='" + KeyValue + "'");
            return Repository().FindTableBySql(strSql.ToString(),false);

        }
        #endregion
    }
}