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
        public List<ModuleRPS> GetListModules()
        {
            string sqlString = "SELECT b.ID, NAME, [DESCRIPTION], LINKURL, PARENTID, SORTID, ISDELETED "
                + "FROM [dbo].[ROLEMODULES] a JOIN[MODULES] b ON a.MODULE_ID = b.ID "
                + "WHERE a.ROLE_ID = @Role_id; ";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Role_id", 1)
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
            UpdateSortId(module);

            return 0;
        }

        public int UpdateModule(ModuleRPS module)
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
            UpdateSortId(module);
            return 0;
        }

        private int UpdateSortId(ModuleRPS module)
        {
            string sqlString = string.Empty;
            //get list of SortId
            sqlString = "update Modules set SORTID = SORTID + 1 WHERE PARENTID = @ParentId "
                + "AND SORTID >= @SortId AND ID != @ModuleId";
            SqlParameter[] param1 = new SqlParameter[]
            {
                new SqlParameter("@ParentId", module.ParentId),
                new SqlParameter("@SortId", module.SortId),
                new SqlParameter("@ModuleId", module.ID)
            };

            ExecNonQuery(sqlString, param1);

            return 0;
        }
    }
}