using InfomsWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InfomsWeb.DataContext
{
    public class UserDataContext : RPSSQL
    {
        public UserLogin GetUserByUsername(string username)
        {
            string sqlQuery = "Select u.ID, u.LOGINNAME, u.PASSWORD, u.STAFFID, u.FULLNAME, u.EMAIL, u.ISDEFAULT, u.ISACTIVE , r.[NAME] as USERROLE " +
                "From ROLES r, USERROLES ur, USERS u Where u.LOGINNAME = @Username and u.ID = ur.[USER_ID] " +
                "and ur.[ROLE_ID] = r.[ID]";

            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Username", username)
            };

            DataTable dt = QueryTable(sqlQuery, param);

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                UserLogin u = new UserLogin
                {
                    ID = Convert.ToInt32(dr["ID"]),
                    LoginName = dr["LOGINNAME"].ToString(),
                    Fullname = dr["FULLNAME"].ToString(),
                    IsDefault = Convert.ToBoolean(dr["ISDEFAULT"]),
                    IsActive = Convert.ToBoolean(dr["ISACTIVE"]),
                    UserRoles = dr["USERROLE"].ToString()
                };
                return u;
            }
            return null;
        }

        public List<User> GetAllUser()
        {
            List<User> list = new List<User>();

            string sqlString = "Select ID, StaffID, Fullname, Email, ContactNo, Addr, City, Postcode, States, LoginName, IsDefault, IsActive From Users";
            DataTable dt = QueryTable(sqlString);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    User usr = new User
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        StaffID = dr["StaffID"].ToString(),
                        FullName = dr["Fullname"].ToString(),
                        Email = dr["Email"].ToString(),
                        ContactNo = dr["ContactNo"].ToString(),
                        Address = dr["Addr"].ToString(),
                        City = dr["City"].ToString(),
                        Postcode = dr["Postcode"].ToString(),
                        State = dr["States"].ToString(),
                        LoginName = dr["LoginName"].ToString(),
                        IsActive = Convert.ToBoolean(dr["IsActive"]),
                        IsDefault = Convert.ToBoolean(dr["IsDefault"])
                    };
                    list.Add(usr);
                }
            }
            return list;
        }
        
        public int CreateUser(User usr)
        {
            //TODO: Encrypt the password
            string sqlString = "INSERT INTO [USERS] ([STAFFID],[FULLNAME],[LOGINNAME],[PASSWORD],[EMAIL],[ISDEFAULT],[ISACTIVE], " +
                "[CONTACTNO],[ADDR],[CITY],[STATES],[POSTCODE]) " +
                "VALUES(@STAFFID, @FULLNAME, @LOGINNAME, @PASSWORD, @EMAIL, @ISDEFAULT, @ISACTIVE, " +
                "@CONTACTNO, @ADDR, @CITY, @POSTCODE, @STATES)";

            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@STAFFID", usr.StaffID),
                new SqlParameter("@FULLNAME", usr.FullName),
                new SqlParameter("@LOGINNAME", usr.LoginName),
                new SqlParameter("@PASSWORD", usr.Password),
                new SqlParameter("@EMAIL", (object) usr.Email ?? DBNull.Value),
                new SqlParameter("@ISDEFAULT", usr.IsDefault),
                new SqlParameter("@ISACTIVE", usr.IsActive),
                new SqlParameter("@CONTACTNO", (object) usr.ContactNo ?? DBNull.Value),
                new SqlParameter("@ADDR", (object) usr.Address ?? DBNull.Value),
                new SqlParameter("@CITY", (object) usr.City ?? DBNull.Value),
                new SqlParameter("@POSTCODE", (object) usr.Postcode ?? DBNull.Value),
                new SqlParameter("@STATES", (object) usr.State ?? DBNull.Value)
            };
            return ExecNonQuery(sqlString, param);
        }

        public int SaveUserRoleAssign(AssignUserRole usr)
        {
            //TODO: Check for Insert or Update
            string sqlString = string.Empty;
            object result = CheckUserRole(usr.UserID);

            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@USERID", usr.UserID),
                new SqlParameter("@ROLEID", usr.RoleID)
            };

            if (result != null)
            {
                //run update
                sqlString = "UPDATE [dbo].[USERROLES] SET [USER_ID] = @USERID, [ROLE_ID] = @ROLEID WHERE [ID] = @ID";
                Array.Resize(ref param, 3);
                param[2] = new SqlParameter("@ID", result);
            }
            else
            {
                //run insert
                sqlString = "INSERT INTO [USERROLES] ([USER_ID],[ROLE_ID]) VALUES (@USERID, @ROLEID)";
            }

            return ExecNonQuery(sqlString, param);
        }

        private object CheckUserRole(int userID)
        {
            string sqlString = "SELECT [ID] from [USERROLES] WHERE [USER_ID] = @USERID";

            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@USERID", userID)
            };
            //return no. of rows affected
            return ExecScalar(sqlString, param);
        }

        public List<AssignUserRole> GetListActiveUser()
        {
            List<AssignUserRole> list = new List<AssignUserRole>();

            string sqlString = "Select u.ID, u.FULLNAME, IsNull(ur.ROLE_ID,0) as ROLE_ID From USERS u " +
                "Left Join USERROLES ur ON ur.USER_ID = u.ID " +
                "WHERE u.ISACTIVE = 1";
            DataTable dt = QueryTable(sqlString);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    AssignUserRole aur = new AssignUserRole
                    {
                        UserID = Convert.ToInt32(dr[0]),
                        Fullname = dr[1].ToString(),
                        RoleID = Convert.ToInt32(dr[2])
                    };
                    list.Add(aur);
                }
            }

            return list;
        }
    }
}