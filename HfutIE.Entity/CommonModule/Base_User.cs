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
    /// �û�����
    /// <author>
    ///		<name>she</name>
    ///		<date>2014.08.11 15:45</date>
    /// </author>
    /// </summary>
    [Description("�û�����")]
    [PrimaryKey("UserId")]
    public class Base_User : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// �û�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�û�����")]
        public string UserId { get; set; }
        /// <summary>
        /// ��˾����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��˾����")]
        public string CompanyId { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string DepartmentId { get; set; }
        /// <summary>
        /// �ڲ��û�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ڲ��û�")]
        public int? InnerUser { get; set; }
        /// <summary>
        /// �û�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�û�����")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Code { get; set; }
        /// <summary>
        /// ��¼�˻�
        /// </summary>
        /// <returns></returns>
        [DisplayName("��¼�˻�")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Account { get; set; }
        /// <summary>
        /// ��¼����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��¼����")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Password { get; set; }
        /// <summary>
        /// ������Կ
        /// </summary>
        /// <returns></returns>
        [DisplayName("������Կ")]
        public string Secretkey { get; set; }
        /// <summary>
        /// ������Եȼ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("������Եȼ�")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PwdRank { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string RealName { get; set; }
        /// <summary>
        /// ����ƴ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ƴ��")]
        public string Spell { get; set; }
        /// <summary>
        /// �Ա�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ա�")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Gender { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// �ֻ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ֻ�")]
        public string Mobile { get; set; }
        /// <summary>
        /// �绰
        /// </summary>
        /// <returns></returns>
        [DisplayName("�绰")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Telephone { get; set; }
        /// <summary>
        /// OICQ
        /// </summary>
        /// <returns></returns>
        [DisplayName("OICQ")]
        public string OICQ { get; set; }
        /// <summary>
        /// �����ʼ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����ʼ�")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Email { get; set; }
        /// <summary>
        /// ����޸���������
        /// </summary>
        /// <returns></returns>
        [DisplayName("����޸���������")]
        public DateTime? ChangePasswordDate { get; set; }
        /// <summary>
        /// �����¼��ʶ
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����¼��ʶ")]
        public int? OpenId { get; set; }
        /// <summary>
        /// ��¼����
        /// </summary>
        /// <returns></returns>
        [DisplayName("��¼����")]
        public int? LogOnCount { get; set; }
        /// <summary>
        /// ��һ�η���ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��һ�η���ʱ��")]
        public DateTime? FirstVisit { get; set; }
        /// <summary>
        /// ��һ�η���ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��һ�η���ʱ��")]
        public DateTime? PreviousVisit { get; set; }
        /// <summary>
        /// ������ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("������ʱ��")]
        public DateTime? LastVisit { get; set; }
        /// <summary>
        /// ���״̬
        /// </summary>
        /// <returns></returns>
        [DisplayName("���״̬")]
        public string AuditStatus { get; set; }
        /// <summary>
        /// ���Ա����
        /// </summary>
        /// <returns></returns>
        [DisplayName("���Ա����")]
        public string AuditUserId { get; set; }
        /// <summary>
        /// ���Ա
        /// </summary>
        /// <returns></returns>
        [DisplayName("���Ա")]
        public string AuditUserName { get; set; }
        /// <summary>
        /// ���ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("���ʱ��")]
        public DateTime? AuditDateTime { get; set; }
        /// <summary>
        /// �Ƿ�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ƿ�����")]
        public int? Online { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ע")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Remark { get; set; }
        /// <summary>
        /// ��Ч
        /// </summary>
        /// <returns></returns>
        [DisplayName("��Ч")]
        public string Enabled { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������")]
        public int? SortCode { get; set; }
        /// <summary>
        /// ɾ�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("ɾ�����")]
        public int? DeleteMark { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ʱ��")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// �����û�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����û�����")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// �����û�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����û�")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// �޸�ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�޸�ʱ��")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// �޸��û�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�޸��û�����")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// �޸��û�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�޸��û�")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// ��һ���޸�����ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("��һ���޸�����ʱ��")]
        public DateTime? LastPwdModfyTm { get; set; }
        /// <summary>
        /// IP��ַ
        /// </summary>
        /// <returns></returns>
        [DisplayName("IP��ַ")]
        public string IPAddress { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.UserId = CommonHelper.GetGuid;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = ManageProvider.Provider.Current().UserId;
            this.CreateUserName = ManageProvider.Provider.Current().UserName;
            this.Secretkey = Md5Helper.MD5(CommonHelper.CreateNo(), 16).ToLower();
            this.Password = Md5Helper.MD5(DESEncrypt.Encrypt(Md5Helper.MD5(this.Password, 32).ToLower(), this.Secretkey).ToLower(), 32).ToLower();
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.UserId = KeyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = ManageProvider.Provider.Current().UserId;
            this.ModifyUserName = ManageProvider.Provider.Current().UserName;
            this.Password = null;
        }
        #endregion
    }
}