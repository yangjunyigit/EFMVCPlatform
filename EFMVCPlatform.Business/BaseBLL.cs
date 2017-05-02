using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVCPlatform.Business
{
    public class BaseBLL
    {
        public bool Add<T>(T entity) where T:class
        {
            //IDAL.IBase<T> dal = new DAL.Base<T>();
            //return dal.Add(entity);
            return false;
        }
    }
}
