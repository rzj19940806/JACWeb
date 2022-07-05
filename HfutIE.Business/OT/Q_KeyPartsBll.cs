//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2014
// Software Developers @ HfutIE 2014
//=====================================================================================

using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using System.Data.Common;
using System.IO;

namespace HfutIE.Business
{
    public class Q_KeyPartsBll
    {
        public IDatabase db = DataFactory.Database();

        public int ExecuteSql(StringBuilder sql)
        {
            return db.ExecuteBySql(sql);
        }
        #region 关重件录入
        public void GetRowValue(string tableName, string select, string propName, string propValue, ref Dictionary<string, string> props)
        {
            string sql = $"select {select} from {tableName} with(nolock) where {propName} = '{propValue}' and Enabled = '1'";
            DataRow dr = db.FindTableBySql(sql)?.Rows[0];
            foreach (string item in select.Split(','))
            {
                string nm = item;
                if (item.Split(' ').Length>1)
                {
                    nm = item.Split(' ')[1];
                }
                props.Add(nm, dr[nm].ToString());
            }
        }

        //public string GetClassId(Dictionary<string, string> props)
        //{
        //    DateTime day = DateTime.Now.Date;
        //    string sql = $"select ClassId from BPdb_Dt with(nolock) where Tm = '{day}' AND OrgId='{props["PlineId"]}'";
        //    DataTable dtTable = db.FindTableBySql(sql);
        //    if (dtTable == null && dtTable.Rows.Count == 0)
        //    {
        //        sql = $"select ClassId from BPdb_Dt with(nolock) where Tm = '{day}' AND OrgId='{props["WorkshopId"]}'";
        //        dtTable = db.FindTableBySql(sql);
        //    }
        //    return dtTable.Rows[0]["ClassId"].ToString();
        //}

        //public DataTable GetShif(string ClassId)
        //{
        //    string sql = $"select ShiftCd from BFacR_ClassConfig with(nolock) where ClassId = '{ClassId}' and Enabled = '1'";
        //    return db.FindTableBySql(sql);
        //}

        //public DataTable GetShiftTime(DataTable shifts)
        //{
        //    string whereSql = null;
        //    foreach (DataRow dataRow in shifts.Rows)
        //    {
        //        whereSql += "'"+dataRow["ShiftCd"] + "',";
        //    }
        //    whereSql = whereSql.Remove(whereSql.Length - 1);
        //    string sql = $"select ShiftCd,StrtRestTm,EndRestTm from BFacR_ShiftBase with(nolock) where RestTm = '总时间' and ShiftCd in ({whereSql}) and Enabled = '1' order by EndRestTm";
        //    return db.FindTableBySql(sql);
        //}

        //public void GetTeam(string shiftCd,ref Dictionary<string, string> props)
        //{
        //    //获取班组ID
        //    string teamIdSql = $"select TeamId from BFacR_TeamOrg where TeamId in(select TeamId from BFacR_ClassTeamConfig with(nolock) where ShiftId in (select ShiftId from BFacR_ShiftBase with(nolock) where shiftCd = '{shiftCd}') and Enabled = '1')and PlineId ='{props["PlineId"]}' and Enabled = '1'";
        //    DataTable teamIds= db.FindTableBySql(teamIdSql);
        //    string TeamId = null;
        //    foreach (DataRow dataRow in teamIds.Rows)
        //    {
        //        TeamId += "'" + dataRow["TeamId"] + "',";
        //    }
        //    TeamId = TeamId.Remove(TeamId.Length - 1);
        //    //根据班组ID获取班组信息，可能多个班组。
        //    string teamSql = $"select TeamId,TeamCd,TeamNm from BFacR_TeamBase with(nolock) where TeamId in ({TeamId}) and Enabled = '1'";
        //    DataTable teams= db.FindTableBySql(teamSql);
        //    string TeamCd=null, TeamNm = null, MgrId=null;
        //    foreach (DataRow dataRow in teams.Rows)
        //    {
        //        MgrId += "'" +db.FindTableBySql($"select top 1 StfId from BFacR_TeamStfConfig with(nolock) where TeamId = '" +dataRow["TeamId"]+ "' and Enabled = '1' and IsMgr='是'").Rows[0]["StfId"] + "',";
        //        TeamCd += "'" + dataRow["TeamCd"] + "',";
        //        TeamNm += "'" + dataRow["TeamNm"] + "',";
        //    }
        //    props.Add("TeamId", TeamId);
        //    props.Add("TeamCd", TeamCd.Remove(TeamCd.Length - 1));
        //    props.Add("TeamNm", TeamNm.Remove(TeamNm.Length - 1));
        //    //根据班组长信息获取班组负责人信息
        //    string mgrSql= $"select StfCd,StfNm from BBdbR_StfBase with(nolock) where StfId in ({MgrId.Remove(MgrId.Length - 1)}) and Enabled = '1'";
        //    DataTable mgrs = db.FindTableBySql(mgrSql);
        //    string MgrCd = null, MgrNm = null;
        //    foreach (DataRow dataRow in mgrs.Rows)
        //    {
        //        MgrCd += "'" + dataRow["StfCd"] + "',";
        //        MgrNm += "'" + dataRow["StfNm"] + "',";
        //    }
        //    props.Add("MgrId", MgrId.Remove(MgrId.Length - 1));
        //    props.Add("MgrCd", MgrCd.Remove(MgrCd.Length - 1));
        //    props.Add("MgrNm", MgrNm.Remove(MgrNm.Length - 1));
        //}
        public DataTable GetBarCode(string WcId)
        {
            string sql = $@"declare @table table(RsvFld1 varchar(50),BarLength varchar(50),CombineNm varchar(50),CombineStart	int,CombineLength int)
insert into @table select C.RsvFld1,C.BarLength,A.CombineNm,A.CombineStart,A.CombineLength
from BBdbR_DecodeRule A with(nolock) join BBdbR_CodeWcConfig B with(nolock) on WcId = '{WcId}' and A.BarId = B.CodeId and A.Enabled = '1' and BarCd = 'MatBarCode' 
join BBdbR_BarCodeBase C with(nolock) on A.BarId = C.BarId and C.Enabled = '1'
select A.RsvFld1,A.BarLength,A.CombineStart AStart,A.CombineLength ALength,B.CombineStart BStart,B.CombineLength BLength,C.CombineStart CStart,C.CombineLength CLength
from @table A join @table B on A.RsvFld1=B.RsvFld1 and A.CombineNm='批次号' and B.CombineNm='供应商' join @table C on A.RsvFld1=C.RsvFld1 and C.CombineNm='图号'
order by A.RsvFld1";
            return db.FindTableBySql(sql);
        }
        public DataTable getvininfo(string WcId, string VIN)
        {
            string sql = $@"select W.WcCd,W.WcNm,isnull(F.VIN,'9999') VIN,CarType,CarColor1,MatCd,SequenceNo  
                from BBdbR_WcBase W  with(nolock)
                left join P_FitEnCodeInfo F  with(nolock)
                on W.PlineId=F.PlineId and W.StartPoint<F.CodeValue and W.EndPoint>F.CodeValue 
                Left join P_LineProductionQueue_Pro L with(nolock) on F.VIN=L.VIN
                where W.Enabled=1 and W.WcId='{WcId}' and isnull(F.VIN,'9999') <>'{VIN}'";
            return db.FindTableBySql(sql);
        }
        public DataTable GetVINInfoByStf(string VIN)
        {
            string sql = $"select *,MatCd as ProductMatCd from P_LineProductionQueue_Pro with(nolock) where VIN like '%{VIN}'";
            return db.FindTableBySql(sql); //获取当前车身队列中的信息
        }

        public DataTable GetProductMat(string MatId)
        {
            string sql = $"select * from BBdbR_MatBase with(nolock) where MatId ='{MatId}' and Enabled='1'";
            return db.FindTableBySql(sql);
        }

        public DataTable GetProduct(string MatCd)//参数M002
        {
            string sql = $"select * from BBdbR_MatBase with(nolock) where MatCd ='{MatCd}' and Enabled='1'";
            return db.FindTableBySql(sql);
        }
        public DataTable GetWcProduct(string MatId,string WcId)
        {
            string sql = $"select * from BBdbR_ProductWcConfig with(nolock) where MatId ='{MatId}' and WcId='{WcId}' and Enabled='1'";
            return db.FindTableBySql(sql);
        }
        public DataTable GetFileImg(string ConfigId)
        {
            string sql = $"select * from BBdbR_MatFileConfig with(nolock) where ConfigId ='{ConfigId}' and Enabled='1'";
            return db.FindTableBySql(sql);
        }
        public DataTable GetParts(string productId,string wcId)
        {
            string sql = $"select * from produceBom with(nolock) where ProductId='{productId}' and WcId='{wcId}'";
            DataTable Parts = db.FindTableBySql(sql);
            return Parts;
        }
        public DataTable GetBondParts(string VIN, string wcId)
        {
            string sql = $"select matid,matcd from Q_KeyPartsBind_Pro with(nolock) where VIN='{VIN}' and WcId='{wcId}' order by matcd";
            DataTable PartBond = db.FindTableBySql(sql);

            StringBuilder sb = new StringBuilder();
            int num = 1;
            for (int i = 0; i < PartBond.Rows.Count; i++)
            {
                if (i==0 || PartBond.Rows[i]["matcd"].ToString()!= PartBond.Rows[i-1]["matcd"].ToString())
                {
                    num = 1; 
                }
                else
                {
                    num++;
                }
                sb.Append($"update Q_ScanStatus_Timely set ScanStatus='ByPass' where WcId='{wcId}' and MatId='{PartBond.Rows[i]["matid"]}' and MatNo=(select max(MatNo) from Q_ScanStatus_Timely with(nolock) where WcId='{wcId}' and MatId='{PartBond.Rows[i]["matid"]}')-convert(int,'{num}')+1");
            }
            if (PartBond.Rows.Count>0)
            {
                db.ExecuteBySql(sb);
            }
            return PartBond;
        }
        //顺便刷新实时扫码表
        public DataTable GetPartImgs(DataTable Parts,string PlineId, string WcId,string VIN,bool del,string plineType)
        {
            StringBuilder sbSql = new StringBuilder();
            DataTable partimgs = Parts.Clone();
            string table = plineType == "主线" ? "Q_ScanStatus_Timely" : "Q_ScanStatus_Assist_Timely";
            sbSql.Append($"delete {table} where WcId = '{WcId}'");
            foreach (DataRow item in Parts.Rows)
            {
                if (item["IsScan"]!=System.DBNull.Value&&item["IsScan"].ToString() == "1")
                {
                    int num = item["MatNum"] != DBNull.Value ? (int)item["MatNum"] : 0;
                    for (int i = 1; i < num + 1; i++)
                    {
                        sbSql.Append($"INSERT INTO {table} VALUES('{PlineId}', '{WcId}', '{VIN}','{item["MatId"]}','未扫码',1,{i}); ");
                    }
                }
                if (item["MatImg"] != System.DBNull.Value)
                {
                    partimgs.Rows.Add(item.ItemArray);
                }
            }
            if (Parts.Rows.Count == 0 && table== "Q_ScanStatus_Assist_Timely")
            {
                sbSql.Append($"INSERT INTO Q_ScanStatus_Assist_Timely VALUES('{PlineId}', '{WcId}', '{VIN}','','ByPass',1,1); ");
            }
            if (del)
            {
                db.ExecuteBySql(sbSql);
            }
            return partimgs;
        }
        //顺便刷新实时扫码表
        public void GetCharPart(DataTable Parts, string PlineId, string WcId, string VIN, string oledVIN, bool del)
        {
            if (!del)
            {
                return;
            }
            StringBuilder sbSql = new StringBuilder();
            DataTable partimgs = Parts.Clone();
            sbSql.Append($"delete Q_ScanStatus_Assist_Timely where WcId = '{WcId}' and VIN='{oledVIN}'");
            foreach (DataRow item in Parts.Rows)
            {
                if (item["IsScan"] != System.DBNull.Value && item["IsScan"].ToString() == "1")
                {
                    int num = item["MatNum"] != DBNull.Value ? (int)item["MatNum"] : 0;
                    for (int i = 1; i < num + 1; i++)
                    {
                        sbSql.Append($"INSERT INTO Q_ScanStatus_Assist_Timely VALUES('{PlineId}', '{WcId}', '{VIN}','{item["MatId"]}','未扫码',1,{i}); ");
                    }
                }
            }
            if (Parts.Rows.Count==0)
            {
                sbSql.Append($"INSERT INTO Q_ScanStatus_Assist_Timely VALUES('{PlineId}', '{WcId}', '{VIN}','','ByPass',1,1); ");
            }
            db.ExecuteBySql(sbSql);
        }

        //绑定验证
        public int PartVerify(Q_KeyPartsBind_Pro KeyPartsBind)
        {
            string sql = $"select IIF((select top 1 mattyp from BBdbR_MatBase with(nolock) where MatCd='{KeyPartsBind.MatCd}' and Enabled=1)='1',(select count(*) from Q_KeyPartsBind_Pro where BarCode='{KeyPartsBind.BarCode}'),0)";
            return db.FindCountBySql(sql);
        }

        //绑定记录
        public int PartBind(Q_KeyPartsBind_Pro KeyPartsBind,int MatNo,string tableName)
        {
            string sql = $"select * from BBdbR_SupplierBase with(nolock) where SupplierCd = '{KeyPartsBind.SupplierCd}'";
            DataTable supplier = DataFactory.Database().FindTableBySql(sql);
            if (supplier.Rows.Count>0)
            {
                KeyPartsBind.SupplierId = supplier.Rows[0]["SupplierId"] == DBNull.Value ? null : supplier.Rows[0]["SupplierId"].ToString();
                KeyPartsBind.SupplierCd = supplier.Rows[0]["SupplierCd"] == DBNull.Value ? null : supplier.Rows[0]["SupplierCd"].ToString();
                KeyPartsBind.SupplierNm = supplier.Rows[0]["SupplierNm"] == DBNull.Value ? null : supplier.Rows[0]["SupplierNm"].ToString();
            }
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            db.Insert(KeyPartsBind, isOpenTrans);
            int num = ScanStatus(KeyPartsBind.WcId, KeyPartsBind.VIN, KeyPartsBind.MatId, KeyPartsBind.BarCode, MatNo, tableName);
            if (num > 0)
                database.Commit();
            else
                database.Rollback();
            return num;
        }             
        //绑定记录
        public int PartPass(Q_KeyByPass_Pro KeyByPass, int MatNo, string tableName)
        {
            int num = ScanStatus(KeyByPass.WcId, KeyByPass.VIN, KeyByPass.MatId, KeyByPass.BarCode, MatNo, tableName);
            if (num > 0)
                DataFactory.Database().Insert(KeyByPass);
            return num;
        }
        //刷新实时扫码表
        public int ScanStatus(string wcId,string Vin,string matId,string status, int MatNo, string tableName)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append($"UPDATE {tableName} SET ScanStatus = '{status}' WHERE WcId = '{wcId}' and VIN = '{Vin}' and MatId = '{matId}' and MatNo = '{MatNo}' and ScanStatus='未扫码'; ");
            return db.ExecuteBySql(sql);
        }

        public DataTable getScaned(string table,string WcId)
        {
            string sql = $"select * from {table} with(nolock) where WcId='{WcId}' and ScanStatus != '未扫码'";
            return db.FindTableBySql(sql);
        }
        public DataTable getAllScan(string table, string WcId)
        {
            string sql = $"select * from {table} with(nolock) where WcId='{WcId}'";
            return db.FindTableBySql(sql);
        }
        public DataTable getNewCar()
        {
            string sql = $"select Top 1 * from P_LineProductionQueue_Pro with(nolock) where len(VIN)=17 order by SequenceNo";
            return db.FindTableBySql(sql);
        }
        public DataTable getNewCar(string VIN)
        {
            string sql = $"declare @vininfo table(rowid int, vin varchar(50)) insert into @vininfo select row_number() over(order by plinecd, carquene desc) rowid,VIN from P_LineProductionQueue_Pro with(nolock) where plinecd <> '' and PlineNm not like 'PBS%' and len(vin)= 17 select Top 1 * from @vininfo where rowid < (select rowid from @vininfo where vin = '{VIN}') order by rowid desc";
            return db.FindTableBySql(sql);
        }


        //座椅指示
        public DataTable getCharCar(string AviId)
        {
            string sql = $"select Top 2 * from P_LineProductionQueue_Pro with(nolock) where AVIId='{AviId}' and len(VIN)=17 order by CarQuene";
            return db.FindTableBySql(sql);
        }
        public DataTable getCharCar(string VIN1, string VIN2)
        {
            string sql = $"declare @vininfo table(rowid int, vin varchar(50)) insert into @vininfo select row_number() over(order by plinecd, carquene desc) rowid,VIN from P_LineProductionQueue_Pro with(nolock) where plinecd <> '' and PlineNm not like 'PBS%' and len(vin)= 17 select Top 2 * from @vininfo where rowid < (select min(rowid) from @vininfo where vin in ('{VIN1}', '{VIN2}')) order by rowid desc";
            return db.FindTableBySql(sql);
        }

        //电视机
        public DataTable GetMatImgs(DataTable VINInfo,string WcId)
        {
            string sql = $"select top 2 * from produceBom where ProductCd='{VINInfo.Rows[0]["MatCd"]}' and WcId='{WcId}' and MatImg is not null";
            return db.FindTableBySql(sql);
        }
        #endregion

        #region 打印--AVI使用
        public string GetPlineType(string PlinId)//得到产线的类别
        {
            string sql = $"select PlineTyp from BBdbR_PlineBase where PlineId ='{PlinId}' and Enabled=1 ";
            return db.FindTableBySql(sql, false).Rows[0]["PlineTyp"].ToString();
        }
        public DataTable GetPlineTypeByIp(string IP)//得到产线的类别
        {
            string sql = $"select * from BBdbR_PlineBase where PlineId=(select top 1 PlineId from BBdbR_WcBase where WcId =(select top 1 ClassId from BBdbR_DvcBase where IPAddr='{IP}')) ";
            return db.FindTableBySql(sql);
        }

        public DataTable GetPrintData(string plineId, string productId,string condition)//得到该产线上该产品所有配置的物料
        {
            string Sql = $"select B.MatCd,B.MatNm,A.MatNum,A.WcCd from(select * from BBdbR_ProductWcMatConfig where ProductClassId in (select ProductClassId From BBdbR_ProductWcConfig where PlineId = '{plineId}' and MatId = '{productId}' and  Enabled = 1)) AS A Left join BBdbR_MatBase AS B on A.MatId = B.MatId where B." + condition;
            return db.FindTableBySql(Sql, false);
        }
        #endregion

        #region 日志记录
        public void WriteLog(string path,string fileName, string text)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (!File.Exists(fileName))
                {
                    FileStream fs1 = new FileStream(fileName, FileMode.Create, FileAccess.Write);//创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1);
                    sw.WriteLine("——————————————" + DateTime.Now.ToString("HH:mm:ss") + "-————————————————");
                    sw.WriteLine(text);
                    sw.WriteLine();
                    sw.Close();
                    fs1.Close();
                }
                else
                {
                    StreamWriter sr = File.AppendText(fileName);
                    sr.WriteLine("——————————————" + DateTime.Now.ToString("HH:mm:ss") + "-————————————————");
                    sr.WriteLine(text);
                    sr.WriteLine();
                    sr.Close();
                }
            }
            catch (Exception ex)
            {

            }

        }
        #endregion
    }
}