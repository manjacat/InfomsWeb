using InfomsWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InfomsWeb.DataContext
{
    public class RoleDataContext : RPSSQL
    {
        public List<RoleRPS> GetRoles()
        {
            List<RoleRPS> list = new List<RoleRPS>();

            string sqlString = "SELECT ID, NAME, ISDEFAULT, ISACTIVE FROM [ROLES]";
            DataTable dt = QueryTable(sqlString);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    RoleRPS rle = new RoleRPS
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        Name = dr["NAME"].ToString(),
                        IsActive = Convert.ToBoolean(dr["ISACTIVE"]),
                        IsDefault = Convert.ToBoolean(dr["ISDEFAULT"]),
                        Modules = ModuleTree.BuildTree()
                    };
                    list.Add(rle);
                }
            }
            return list;
        }

        public RoleRPS GetRole(int Id)
        {
            string sqlString = "SELECT NAME, ISDEFAULT, ISACTIVE FROM [ROLES] WHERE ID = @RoleId";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@RoleId", Id)
            };
            DataTable dt = QueryTable(sqlString, param);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                RoleRPS rle = new Models.RoleRPS
                {
                    ID = Id,
                    Name = dr["NAME"].ToString(),
                    IsActive = Convert.ToBoolean(dr["ISACTIVE"]),
                    IsDefault = Convert.ToBoolean(dr["ISDEFAULT"]),
                    Modules = ModuleTree.BuildTree()
                };
                return rle;
            }
            return null;
        }

        public int CreateRole(RoleRPS rle)
        {
            string sqlString = " INSERT INTO [ROLES] (NAME, ISDEFAULT, ISACTIVE) VALUES (@RoleName,1,1);";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@RoleName", rle.Name)
            };
            return ExecNonQuery(sqlString, param);
        }

        public int UpdateRole(RoleRPS rle)
        {
            string sqlString = "UPDATE ROLES SET NAME = @RoleName, ISACTIVE = @IsActive WHERE ID = @Id;";
            SqlParameter[] param = new SqlParameter[]
            {
                   new SqlParameter("@RoleName", rle.Name),
                   new SqlParameter("@IsActive", rle.IsActive),
                   new SqlParameter("@Id", rle.ID)
            };
            return ExecNonQuery(sqlString, param);
        }

        public RoleRPS GetRoleByUsername(string user)
        {
            string sqlString = "Select r.[ID],r.ISACTIVE,r.ISDEFAULT, r.[NAME] From ROLES r, USERROLES ur, USERS u "+
                "Where u.LOGINNAME = @Username and u.ID = ur.[USER_ID] and ur.[ROLE_ID] = r.[ID];";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Username", user)
            };
            DataTable dt = QueryTable(sqlString, param);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                RoleRPS rle = new Models.RoleRPS
                {
                    ID = Convert.ToInt32(dr["ID"]),
                    Name = dr["NAME"].ToString(),
                    IsActive = Convert.ToBoolean(dr["ISACTIVE"]),
                    IsDefault = Convert.ToBoolean(dr["ISDEFAULT"])
                    //Modules = new ModuleTree()
                };
                return rle;
            }
            return null;
        }
    }
}