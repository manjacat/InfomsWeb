using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InfomsWeb.ViewModel;
using System.Data.SqlClient;
using System.Data;
using InfomsWeb.Models;

namespace InfomsWeb.DataContext
{
    public class RoleModuleDataContext : RPSSQL
    {
        public RoleModule GetRoleModule(int roleId)
        {
            string sqlString = string.Empty;
            sqlString += "SELECT ID, NAME, ISDEFAULT, ISACTIVE FROM [ROLES] WHERE ID = @RoleId; ";
            sqlString += "select b.ID, b.[Name], b.DESCRIPTION, b.LINKURL, b.PARENTID, b.SORTID, "
                + "(select Count(MODULE_ID) from ROLEMODULES WHERE ROLE_ID = @RoleId AND MODULE_ID = b.ID) as IsAuth "
                + "FROM MODULES b WHERE ISDELETED = 0;";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@RoleId", roleId)
            };
            DataSet ds = QuerySet(sqlString, param);
            //table[0] = Role
            //table[1] = Module
            RoleModule roleModule = new RoleModule();

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                RoleRPS rle = new RoleRPS
                {
                    ID = Convert.ToInt32(dr["ID"]),
                    Name = dr["NAME"].ToString(),
                    IsActive = Convert.ToBoolean(dr["ISACTIVE"]),
                    IsDefault = Convert.ToBoolean(dr["ISDEFAULT"])
                };
                roleModule.Role = rle;
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    ModuleRPS mod = new ModuleRPS
                    {
                        ID = Convert.ToInt32(dr["id"]),
                        Name = dr["Name"].ToString(),
                        Description = dr["DESCRIPTION"].ToString(),
                        LinkURL = dr["LINKURL"].ToString(),
                        ParentId = Convert.ToInt32(dr["PARENTID"]),
                        SortId = Convert.ToInt32(dr["SORTID"]),
                        IsAuthorized = Convert.ToInt32(dr["IsAuth"]) > 0 ? true : false
                    };
                    roleModule.Module.Add(mod);
                }
            }
            return roleModule;
        }

        public int UpdateRoleModule(List<int> moduleIdList, int roleId)
        {
            string sqlString = string.Empty;

            //delete existing role/module
            sqlString = "DELETE FROM ROLEMODULES WHERE ROLE_ID = @RoleId; ";
            if (moduleIdList.Count > 0)
            {
                //insert new RoleModule pair
                sqlString += "INSERT INTO ROLEMODULES (ROLE_ID, MODULE_ID) VALUES ";
                List<string> roleModules = new List<string>();
                foreach (int moduleId in moduleIdList)
                {
                    string insertStr = string.Format("({0},{1})", roleId, moduleId);
                    roleModules.Add(insertStr);
                }
                string strJoin = string.Join(",", roleModules.ToArray());
                sqlString += strJoin;
                sqlString += ";";
            }

            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@RoleId", roleId)
            };
            return ExecNonQuery(sqlString, param);
        }
    }
}