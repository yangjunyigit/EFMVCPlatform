using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EFMVCPlatform.Models;
using EFMVCPlatform.IDAL;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace EFMVCPlatform.DAL.UserRights
{
    public class UserDAL : BaseDAL<UserRightsEntities, Models.UserRights.AdminUser>, IUserDAL
    {
        public DataTable GetCustomData()
        {
            DataTable dt = new DataTable();
            try
            {

                for (int i = 0; i < 1; i++)
                {
                    //TestCreateConnect();
                    Test();
                }

                //UserRightsEntities ctx = new UserRightsEntities();
                //ctx.Database.SqlQuery<string>("");

                //SqlParameter[] paras = new SqlParameter[] {
                //    null,
                //    new SqlParameter("@UserName","wcx"),
                //    new SqlParameter("@RoleName","ww")
                //};
                //string strSql = "select * from AdminUser where UserName = @UserName select * from Roles where RoleName like @RoleName+'%' ";
                //DataSet ds = ExecuteQuery(strSql, paras);
                //dt = ds.Tables[0];               
            }
            catch (Exception ex)
            {

                //throw;
            }
            return dt;
        }

        public void Test()
        {
            //Method1();

            using (UserRightsEntities ctx = new UserRightsEntities())
            {
                ctx.Database.Log = (msg) => { Debug.Write(msg); };

                //ctx.AdminUser.
            }
        }

        private static void Method1()
        {
            Models.UserRights.AdminUser user = new Models.UserRights.AdminUser();
            user.UserName = "a";
            user.RealName = "";

            using (UserRightsEntities ctx = new UserRights.UserRightsEntities())
            {

                Func<Models.UserRights.AdminUser, bool> func = (m) =>
                {
                    bool folg = true;

                    if (!string.IsNullOrEmpty(user.RealName))
                    {
                        folg = folg && m.RealName.Contains(user.RealName);
                    }
                    if (!string.IsNullOrEmpty(user.UserName))
                    {
                        folg = folg && m.UserName.Contains(user.UserName);
                    }

                    return folg;
                };
                if (ctx != null)
                {
                    List<Models.UserRights.AdminUser> userList = ctx.AdminUser.Where(m => (string.IsNullOrEmpty(user.UserName) ? m.UserName.Contains(user.UserName) : true) && (string.IsNullOrEmpty(user.RealName) ? m.RealName.Contains(user.RealName) : true)).ToList();
                    //List<Models.UserRights.AdminUser> userList = GetModelList(m => (!string.IsNullOrEmpty(user.UserName) ? m.UserName.Contains(user.UserName) : true) && (!string.IsNullOrEmpty(user.RealName) ? m.RealName.Contains(user.RealName) : true));
                    Debug.WriteLine(userList.Count.ToString());

                    //ctx.Dispose();
                }
            }
        }

        //UserRightsEntities ctx = null;
        public void TestCreateConnect()
        {
            //ctx = new UserRights.UserRightsEntities();
            //ctx.Database.Log = (m) => { Debug.WriteLine(m); };
        }

        /// <summary>  
        /// 查询的数据  
        /// </summary>  
        /// <param name="YKTEntities">数据访问的上下文</param>  
        /// <param name="order">排序字段</param>  
        /// <param name="sort">升序asc（默认）还是降序desc</param>  
        /// <param name="search">查询条件</param>   
        /// <param name="listQuery">额外的参数</param>  
        /// <returns></returns>        
        public IQueryable<Models.UserRights.AdminUser> DaoChuData(string order, string sort, string search, params object[] listQuery)
        {
            //UserRightsEntities db = new UserRightsEntities();
            //string where = string.Empty;
            //int flagWhere = 0;

            //Dictionary<string, string> queryDic = ValueConvert.StringToDictionary(search.GetString());
            //if (queryDic != null && queryDic.Count > 0)
            //{
            //    foreach (var item in queryDic)
            //    {
            //        if (flagWhere != 0)
            //        {
            //            where += " and ";
            //        }
            //        flagWhere++;

            //        if (!string.IsNullOrWhiteSpace(item.Key) && !string.IsNullOrWhiteSpace(item.Value) && item.Key.Equals("STATUS")) //需要查询的列名  
            //        {
            //            where += "it." + item.Key + " = '" + item.Value + "'";
            //            continue;
            //        }
            //        where += "it." + item.Key + " like '%" + item.Value + "%'";
            //    }
            //}
            //return db.AdminUser
            //         .Where(string.IsNullOrEmpty(where) ? "true" : where)
            //         .OrderBy("it." + sort.GetString() + " " + order.GetString())
            //         .AsQueryable();
            return null;
        }
    }
}
