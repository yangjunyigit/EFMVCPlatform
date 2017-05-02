using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVCPlatform.IBusiness
{
    public interface ISystemManager
    {
        bool CheckUserInfo(string userName, string password);
        DataTable TestResultData();
    }
}
