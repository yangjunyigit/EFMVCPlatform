using EFMVCPlatform.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EFMVCPlatform.IDAL;
using System.Data;
using System.Data.SqlClient;

namespace EFMVCPlatform.DAL
{
    public class BaseDAL<TContext, T> : IDAL.IBaseDAL<T> where TContext : DbContext,new() where T : class
    {
        private TContext GetContext()
        {
            TContext ctx = new TContext();
            ctx.Database.Log = (msg) => { string sql = msg; };
            return ctx;
        }
        ~BaseDAL()
        {

        }
        /// <summary>
        /// 获取单个数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="paras">要连接的类名 格式为User.Role，可选参数</param>
        /// <returns></returns>
        T IDAL.IBaseDAL<T>.GetModel(Expression<Func<T, bool>> predicate, params string[] paras)
        {
            try
            {
                using (TContext context = GetContext())
                {
                    DbQuery<T> dbq = context.Set<T>();
                    for (int i = 0; i < paras.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(paras[i]))
                        {
                            dbq = dbq.Include(paras[i]);
                        }
                    }
                    T query = dbq.FirstOrDefault(predicate);
                    if (query != null)
                    {
                        return query;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("sql", "");
                throw ex;
            }
        }
        
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="paras">要连接的类名 格式为User.Role，可选参数</param>
        /// <returns></returns>
        public List<T> GetModelList(Expression<Func<T, bool>> predicate, params string[] paras)
        {
            using (TContext context = System.Activator.CreateInstance<TContext>())
            {
                DbQuery<T> dbq = context.Set<T>();
                for (int i = 0; i < paras.Length; i++)
                {
                    if (!string.IsNullOrEmpty(paras[i]))
                    {
                        dbq = dbq.Include(paras[i]);
                    }
                }
                IEnumerable<T> query = dbq.Where(predicate);
                if (query != null)
                {
                    List<T> list = query.ToList<T>();
                    return list;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 按条件获取数据条数
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public int GetDataCount(Expression<Func<T, bool>> predicate)

        {
            int page = 0;
            using (TContext context = System.Activator.CreateInstance<TContext>())
            {
                DbQuery<T> dbq = context.Set<T>();
                page = dbq.Count(predicate);
                return page;
            }
        }
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <param name="keySelector">排序字段</param>
        /// <param name="sort">"ASC":顺序，"DESC":倒序</param>
        /// <param name="index">页码</param>
        /// <param name="count">每页条数</param>
        /// <param name="paras">要连接的类名 格式为User.Role，可选参数</param>
        /// <returns></returns>
        public List<T> GetModelList<Tkey>(Expression<Func<T, bool>> predicate,Expression<Func<T,Tkey>> keySelector,string sort, int index, int count, params string[] paras)
        {
            using (TContext context = System.Activator.CreateInstance<TContext>())
            {
                //context.Database.Log = Common.LogUtil.WriteLog;
                DbQuery<T> dbq = context.Set<T>();
                for (int i = 0; i < paras.Length; i++)
                {
                    if (!string.IsNullOrEmpty(paras[i]))
                    {
                        dbq = dbq.Include(paras[i]);
                    }
                }
                IQueryable<T> query;
                if (sort == "DESC")
                {
                    query = dbq.Where(predicate).OrderByDescending(keySelector).Skip((index - 1) * count).Take(count); 
                }
                else
                {
                    query = dbq.Where(predicate).OrderBy(keySelector).Skip((index - 1) * count).Take(count);
                }
                if (query != null)
                {
                    List<T> list = query.ToList<T>();
                    return list;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 添加单个数据
        /// </summary>
        /// <param name="entity">要添加的对象</param>
        /// <returns></returns>
        public virtual bool Add(T entity)
        {
            using (TContext context = System.Activator.CreateInstance<TContext>())
            {
                context.Set<T>().Add(entity);

                int nCount = context.SaveChanges();

                if (nCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 添加数据列表
        /// </summary>
        /// <param name="entityList">要填加的对象列表</param>
        /// <returns></returns>
        public virtual int AddList(List<T> entityList)
        {
            using (TContext context = System.Activator.CreateInstance<TContext>())
            {
                context.Set<T>().AddRange(entityList);

                int nCount = context.SaveChanges();

                return nCount;
            }
        }

        /// <summary>
        /// 更新单个数据
        /// </summary>
        /// <param name="entity">要更新的对象</param>
        /// <returns></returns>
        public virtual bool Update(T entity)
        {
            using (TContext context = System.Activator.CreateInstance<TContext>())
            {
                int nCount = 0;
                if (context.Entry<T>(entity).State == EntityState.Detached)
                {
                    context.Set<T>().Attach(entity);
                    context.Entry<T>(entity).State = EntityState.Modified;
                }

                nCount = context.SaveChanges();


                if (nCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 更新数据列表
        /// </summary>
        /// <param name="entityList">要更新的对象列表</param>
        /// <returns></returns>
        public virtual int UpdateList(List<T> entityList)
        {
            using (TContext context = System.Activator.CreateInstance<TContext>())
            {
                foreach (T entity in entityList)
                {
                    if (context.Entry<T>(entity).State == EntityState.Detached)
                    {
                        context.Set<T>().Attach(entity);
                        context.Entry<T>(entity).State = EntityState.Modified;
                    }
                }
                int nCount = context.SaveChanges();

                return nCount;
            }
        }

        /// <summary>
        /// 删除单个数据
        /// </summary>
        /// <param name="entity">要删除的对象</param>
        /// <returns></returns>
        public virtual bool Delete(T entity)
        {
            using (TContext context = System.Activator.CreateInstance<TContext>())
            {
                if (context.Entry<T>(entity).State == EntityState.Detached)
                {
                    context.Set<T>().Attach(entity);
                    context.Entry<T>(entity).State = EntityState.Deleted;
                }

                int nCount = context.SaveChanges();

                if (nCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 删除数据列表
        /// </summary>
        /// <param name="entityList">要删除的对象列表</param>
        /// <returns></returns>
        public virtual int DeleteList(List<T> entityList)
        {
            using (TContext context = System.Activator.CreateInstance<TContext>())
            {
                foreach (T entity in entityList)
                {
                    if (context.Entry<T>(entity).State == EntityState.Detached)
                    {
                        context.Set<T>().Attach(entity);
                        context.Entry<T>(entity).State = EntityState.Deleted;
                    }
                }

                int nCount = context.SaveChanges();

                return nCount;
            }
        }

        public DataSet ExecuteQuery(string sql,params SqlParameter[] paras)
        {
            using (TContext ctx = new TContext())
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand(sql, (SqlConnection)ctx.Database.Connection);
                cmd.Parameters.AddRange(paras);

                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
               
                adapt.Fill(ds);

                return ds;
            }
        }
        
    }
}
