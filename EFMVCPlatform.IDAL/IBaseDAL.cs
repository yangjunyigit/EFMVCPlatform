using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EFMVCPlatform.IDAL
{
    public interface IBaseDAL<T>
    {
        bool Add(T entity);
        int AddList(List<T> entityList);
        bool Delete(T entity);
        int DeleteList(List<T> entityList);
        int GetDataCount(Expression<Func<T, bool>> predicate);
        T GetModel(Expression<Func<T, bool>> predicate, params string[] paras);
        List<T> GetModelList(Expression<Func<T, bool>> predicate, params string[] paras);
        List<T> GetModelList<Tkey>(Expression<Func<T, bool>> predicate, Expression<Func<T, Tkey>> keySelector, string sort, int index, int count, params string[] paras);
        bool Update(T entity);
        int UpdateList(List<T> entityList);
    }
}
