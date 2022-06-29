//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2014
// Software Developers @ HfutIE 2014
//=====================================================================================

using HfutIE.DataAccess.Attributes;
using HfutIE.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HfutIE.Entity
{
    /// <summary>
    /// ���ϳ�������У�˱�
    /// <author>
    ///		<name>she</name>
    ///		<date>2014.08.06 10:59</date>
    /// </author>
    /// </summary>
    [Description("���ϳ�������У�˱�")]
    [PrimaryKey("ID")]
    public class A_MaterialProgramDemand : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public int? ID { get; set; }
        /// <summary>
        /// ��Ŀ����
        /// </summary>
        [DisplayName("��Ŀ����")]
        public int? ProjectID { get; set; }
        /// <summary>
        /// ��Ŀ���
        /// </summary>
        [DisplayName("��Ŀ���")]
        public string ProjectCode { get; set; }
        /// <summary>
        /// �ƻ�ID
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ƻ�ID")]
        public int? PlanID { get; set; }
        /// <summary>
        /// �ƻ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ƻ����")]
        public string PlanCode { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public int? DemandNum { get; set; }
        /// <summary>
        /// ���ȼ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("���ȼ�")]
        public int? Priority { get; set; }
        /// <summary>
        /// �����������
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����������")]
        public string TaskCode { get; set; }
        /// <summary>
        /// ��Ӧ�������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ӧ�������")]
        public string ProcedureCode { get; set; }
        /// <summary>
        /// ����״̬
        /// </summary>
        /// <returns></returns>
        [DisplayName("����״̬")]
        public int? Cstate { get; set; }
        /// <summary>
        /// Ԥ����ǰ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("Ԥ����ǰ��")]
        public DateTime? ProLeadTime { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public int? Type { get; set; }
        /// <summary>
        /// ��Ч��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ч��")]
        public int? IsAvailable { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ʱ��")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������")]
        public string CreatorID { get; set; }
        /// <summary>
        /// �ϴ��޸�ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ϴ��޸�ʱ��")]
        public DateTime? LastModifiedTime { get; set; }
        /// <summary>
        /// �޸���
        /// </summary>
        /// <returns></returns>
        [DisplayName("�޸���")]
        public string ModifierID { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ע")]
        public string Remarks { get; set; }
        /// <summary>
        /// Ԥ���ֶ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("Ԥ���ֶ�")]
        public string Reserve1 { get; set; }
        /// <summary>
        /// Ԥ���ֶ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("Ԥ���ֶ�")]
        public string Reserve2 { get; set; }
        /// <summary>
        /// Ԥ���ֶ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("Ԥ���ֶ�")]
        public string Reserve3 { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            //this.CompanyId = CommonHelper.GetGuid;
            this.CreateTime = DateTime.Now;
            this.CreatorID = ManageProvider.Provider.Current().UserId;
            //this.CreateUserName = ManageProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            //this.CompanyId = KeyValue;
            this.LastModifiedTime = DateTime.Now;
            this.ModifierID = ManageProvider.Provider.Current().UserId;
            //this.ModifyUserName = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}