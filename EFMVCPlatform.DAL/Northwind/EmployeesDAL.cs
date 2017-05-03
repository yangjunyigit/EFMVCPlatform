using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVCPlatform.Models.Northwind;

namespace EFMVCPlatform.DAL.Northwind
{
    class EmployeesDAL
    {
        public void Test(Action<string> log)
        {
            using (NorthwindEntities ctx = new Northwind.NorthwindEntities())
            {
                var query = from a in ctx.GetSubEmployee(2)
                            select a;
                List<GetSubEmployee_Result> result = query.ToList();
                if(log!=null)
                {
                    log(query.ToString());
                }
                //ctx.Database.Log
            }
        }
    }
}
