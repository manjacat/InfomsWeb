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
            string sqlString = "SELECT b.ID, NAME, [DESCRIPTION], LINKURL, PARENTID, SORTID, ISDELETED, CODE, ICON "
                + "FROM [MODULES] b "
                + "WHERE ISDELETED = 0; ";
            DataTable dt = QueryTable(sqlString);

            List<ModuleRPS> list = new List<ModuleRPS>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ModuleRPS m = BindDataRow(dr);
                    list.Add(m);
                }
            }
            return list;
        }

        public List<ModuleRPS> GetListModules(int roleid)
        {
            string sqlString = "SELECT b.ID, NAME, [DESCRIPTION], LINKURL, PARENTID, SORTID, ISDELETED, CODE, ICON  "
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
                    ModuleRPS m = BindDataRow(dr);
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
            string sqlString = "INSERT INTO MODULES "
                + " (NAME, [DESCRIPTION], LINKURL, PARENTID, SORTID, ISDELETED, CODE, ICON ) "
                + "VALUES(@Name, @Description, @LinkUrl, @ParentId, @SortId, 0, @Code, @Icon); ";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Name", module.Name),
                new SqlParameter("@Description", module.Description),
                new SqlParameter("@LinkUrl", module.LinkURL),
                new SqlParameter("@ParentId", module.ParentId),
                new SqlParameter("@SortId", module.SortId),
                new SqlParameter("@Code", module.Code),
                new SqlParameter("@Icon", module.Icon)
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
                + "LINKURL = @LinkUrl, PARENTID = @ParentId, SORTID = @SortId, "
                + "CODE = @Code, ICON = @Icon "
                + "WHERE ID = @ModuleID ";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Name", module.Name),
                new SqlParameter("@Description", module.Description),
                new SqlParameter("@LinkUrl", string.IsNullOrEmpty(module.LinkURL) ? string.Empty : module.LinkURL ),
                new SqlParameter("@ParentId", module.ParentId),
                new SqlParameter("@SortId", module.SortId),
                new SqlParameter("@Code", module.Code),
                new SqlParameter("@Icon", module.Icon),
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

            //Belom Jadi Lagi
            sqlString = "update Modules set SORTID = SORTID + 1 WHERE PARENTID = @ParentId "
                    + "AND SORTID >= @SortId AND ID != @ModuleId";

            SqlParameter[] param1 = new SqlParameter[]
            {
                new SqlParameter("@ParentId", module.ParentId),
                new SqlParameter("@SortId", module.SortId),
                new SqlParameter("@ModuleId", module.ID)
            };
            return ExecNonQuery(sqlString, param1);
        }

        private ModuleRPS BindDataRow(DataRow dr)
        {
            ModuleRPS m = new ModuleRPS
            {
                ID = Convert.ToInt32(dr["ID"]),
                Name = dr["Name"].ToString(),
                Description = dr["Description"].ToString(),
                LinkURL = dr["LinkURL"].ToString(),
                ParentId = Convert.ToInt32(dr["ParentId"]),
                SortId = Convert.ToInt32(dr["SortId"]),
                Code = dr["CODE"].ToString(),
                Icon = dr["ICON"].ToString()
            };
            //IsAuthorized is only used in RoleModule
            try
            {
                m.IsAuthorized = Convert.ToInt32(dr["IsAuth"]) > 0 ? true : false;
            }
            catch
            {
                m.IsAuthorized = true;
            }
            //m.Icon = GetRandomCSS();

            return m;
        }

        private string GetRandomCSS()
        {
            Random r = new Random();
            int randomNumber = r.Next(1, 5);
            string cssString = string.Empty;
            switch (randomNumber)
            {
                case (1):
                    cssString = "fa fa-free-code-camp";
                    break;
                case (2):
                    cssString = "fa fa-envelope-open";
                    break;
                case (3):
                    cssString = "fa fa-window-restore";
                    break;
                case (4):
                    cssString = "fa fa-trash-o";
                    break;
                case (5):
                    cssString = "fa fa-battery-2";
                    break;
                case (6):
                    cssString = "fa fa-bookmark-o";
                    break;
                case (7):
                    cssString = "fa fa-cubes";
                    break;
                case (8):
                    cssString = "fa fa-group";
                    break;
                case (9):
                    cssString = "fa fa-paper-plane-o";
                    break;
            }
            return cssString;
        }
    }
}