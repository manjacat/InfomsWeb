using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InfomsWeb.Models;
using System.Data.SqlClient;
using System.Data;

namespace InfomsWeb.DataContext
{
    public class ModuleDataContext : RPSSQL
    {
        //For Admin to edit Modules
        public List<ModuleRPS> GetAllModules()
        {
            string sqlString = "SELECT b.ID, NAME, [DESCRIPTION], LINKURL, PARENTID, SORTID, ISDELETED "
                + "FROM [dbo].[ROLEMODULES] a JOIN [MODULES] b ON a.MODULE_ID = b.ID "
                + "WHERE ISDELETED = 0; ";
            DataTable dt = QueryTable(sqlString);

            List<ModuleRPS> list = new List<ModuleRPS>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ModuleRPS m = new ModuleRPS
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        Name = dr["Name"].ToString(),
                        Description = dr["Description"].ToString(),
                        LinkURL = dr["LinkURL"].ToString(),
                        ParentId = Convert.ToInt32(dr["ParentId"]),
                        SortId = Convert.ToInt32(dr["SortId"])
                    };
                    list.Add(m);
                }
            }
            return list;
        }

        public List<ModuleRPS> GetListModules(int roleid)
        {
            string sqlString = "SELECT b.ID, NAME, [DESCRIPTION], LINKURL, PARENTID, SORTID, ISDELETED "
                + "FROM [dbo].[ROLEMODULES] a JOIN [MODULES] b ON a.MODULE_ID = b.ID "
                + "WHERE a.ROLE_ID = @Role_id; ";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Role_id", roleid)
            };
            DataTable dt = QueryTable(sqlString, param);

            List<ModuleRPS> list = new List<ModuleRPS>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ModuleRPS m = new ModuleRPS
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        Name = dr["Name"].ToString(),
                        Description = dr["Description"].ToString(),
                        LinkURL = dr["LinkURL"].ToString(),
                        ParentId = Convert.ToInt32(dr["ParentId"]),
                        SortId = Convert.ToInt32(dr["SortId"])
                    };
                    list.Add(m);
                }
            }
            return list;
        }

        public int CreateModule(ModuleRPS module)
        {
            if (string.IsNullOrEmpty(module.Description))
            {
                module.Description = string.Empty;
            }
            string sqlString = "INSERT INTO MODULES (NAME, [DESCRIPTION], LINKURL, PARENTID, SORTID, ISDELETED) "
                + "VALUES(@Name, @Description, @LinkUrl, @ParentId, @SortId, 0); ";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Name", module.Name),
                new SqlParameter("@Description", module.Description),
                new SqlParameter("@LinkUrl", module.LinkURL),
                new SqlParameter("@ParentId", module.ParentId),
                new SqlParameter("@SortId", module.SortId)
            };

            ExecNonQuery(sqlString, param);

            ////TODO: if SortId is updated, change all sortId for that parent
            UpdateSortId(module, 0);

            return 0;
        }

        public int UpdateModule(ModuleRPS module, int oldSortId)
        {
            if (string.IsNullOrEmpty(module.Description))
            {
                module.Description = string.Empty;
            }
            string sqlString = "UPDATE MODULES SET NAME = @Name, DESCRIPTION = @Description,  "
                + "LINKURL = @LinkUrl, PARENTID = @ParentId, SORTID = @SortId "
                + "WHERE ID = @ModuleID ";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Name", module.Name),
                new SqlParameter("@Description", module.Description),
                new SqlParameter("@LinkUrl", module.LinkURL),
                new SqlParameter("@ParentId", module.ParentId),
                new SqlParameter("@SortId", module.SortId),
                new SqlParameter("@ModuleID", module.ID)
            };

            ExecNonQuery(sqlString, param);

            //TODO: if SortId is updated, change all sortId for that parent
            UpdateSortId(module, oldSortId);
            return 0;
        }

        private int UpdateSortId(ModuleRPS module, int oldSortId)
        {
            string sqlString = string.Empty;
            if (oldSortId == 0)
            {
                oldSortId = 999;
            }
            //get list of SortId
            //Belom Jadi Lagi
            if (module.SortId < oldSortId)
            {
                //naik
                sqlString = "update Modules set SORTID = SORTID + 1 WHERE PARENTID = @ParentId "
                    + "AND SORTID > 0 AND SORTID <= @SortId AND ID != @ModuleId";
            }
            else
            {
                //turun
                sqlString = "update Modules set SORTID = SORTID - 1 WHERE PARENTID = @ParentId "
                    + "AND SORTID > @SortId AND ID != @ModuleId";
            }
            SqlParameter[] param1 = new SqlParameter[]
            {
                new SqlParameter("@ParentId", module.ParentId),
                new SqlParameter("@SortId", oldSortId),
                new SqlParameter("@ModuleId", module.ID)
            };
            return ExecNonQuery(sqlString, param1);

            //List<ModuleRPS> list = GetModuleByParentId(module.ParentId);
            //if (list.Count > 0)
            //{
            //    int counter = 0;
            //    foreach (ModuleRPS m in list)
            //    {
            //        counter++;
            //        sqlString += string.Format("update MODULES Set SORTID = {0} WHERE ID = {1}; ", counter, m.ID);
            //    }
            //    ExecNonQuery(sqlString);
            //}
        }
    }
}