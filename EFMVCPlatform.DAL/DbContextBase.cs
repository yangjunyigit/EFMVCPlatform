using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVCPlatform.DAL
{
    public class DbContextBase<T>:T where T: DbContext
    {
    }
}
