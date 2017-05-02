using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVCPlatform.Business;
using EFMVCPlatform.IBusiness;

namespace EFMVCPlatform.WinForm
{
    internal class DataProvider
    {
        private static ISystemManager systemManagerBll;
        public static ISystemManager SystemManagerBLL
        {
            get
            {
                if (systemManagerBll == null)
                {
                    systemManagerBll = new SystemManagerBLL();
                }
                return systemManagerBll;
            }
        }
    }
}
