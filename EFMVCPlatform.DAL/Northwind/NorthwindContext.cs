using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVCPlatform.DAL.Northwind
{
    public partial class NorthwindEntities
    {
        public NorthwindEntities(Action<string> writeLog)
        {
            this.Database.Log = writeLog;
        }
    }
}
