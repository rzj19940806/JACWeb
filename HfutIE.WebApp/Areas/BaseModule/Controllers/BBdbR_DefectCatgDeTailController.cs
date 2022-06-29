using HfutIE.Business;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace HfutIE.WebApp.Areas.BaseModule.Controllers
{
    /// <summary>
    /// ȱ�����������
    /// </summary>
    public class BBdbR_DefectCatgDeTailController : PublicController<BBdbR_DefectCatgDeTail>
    {
        #region �������ݿ������������
        //�������ݿ���ʶ������Է������в������ݿ�ķ���
        BBdbR_DefectCatgDeTailBll MyBll = new BBdbR_DefectCatgDeTailBll(); //===����ʱ��Ҫ�޸�===
        public readonly RepositoryFactory<BBdbR_DefectCatgDeTail> repository_DefectCatgDeTail = new RepositoryFactory<BBdbR_DefectCatgDeTail>();
        #endregion
        #region ������   

        #region 1.��ȡ��
        public ActionResult TreeJson()
        {
            try
            {
                DataTable dt = MyBll.GetTree();//��ȡ����������
                List<TreeJsonEntity> TreeList = new List<TreeJsonEntity>();
                if (DataHelper.IsExistRows(dt))
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string area_key = row["keys"].ToString();
                        bool hasChildren = false;
                        DataTable childnode = DataHelper.GetNewDataTable(dt, "parentid='" + area_key + "'");
                        if (childnode.Rows.Count > 0)
                        {
                            hasChildren = true;
                        }
                        TreeJsonEntity tree = new TreeJsonEntity();
                        tree.id = area_key;
                        tree.text = row["name"].ToString();
                        tree.value = row["code"].ToString();
                        tree.parentId = row["parentId"].ToString();
                        tree.Attribute = "Type";
                        tree.AttributeValue = row["sort"].ToString();
                        tree.isexpand = true;
                        tree.complete = true;
                        tree.hasChildren = hasChildren;
                        if (row["parentid"].ToString() == "0")
                        {

                            tree.img = "/Content/Images/Icon16/defect.png";
                        }
                        TreeList.Add(tree);
                    }
                }
                return Content(TreeList.TreeToJson());
            }
            catch (Exception ex)
            {
                this.WriteLog(-1, null, null, null, "����ʧ��" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message }.ToString());
            }
        }
        #endregion

        //#region 9.�༭����������
        //public override ActionResult SetForm(string KeyValue)
        //{
        //    try
        //    {
        //        BBdbR_PlineBase Plineentity = MyBll.GetPageListByCondition(KeyValue);

        //        return Content(Plineentity.ToJson());
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(new JsonMessage { Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message }.ToString());
        //    }
        //}

        //public ActionResult GridPageJson2(string PlineCd)
        //{
        //    try
        //    {
        //        List<BBdbR_PlineBase> ListData = MyBll.GetPlineList2(PlineCd);
        //        var JsonData = new
        //        {
        //            rows = ListData,
        //        };
        //        string a = JsonData.ToJson();
        //        return Content(JsonData.ToJson());
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(new JsonMessage { Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message }.ToString());
        //    }
        //}
        //#endregion
        #endregion
    }
}