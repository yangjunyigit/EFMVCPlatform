using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVCPlatform.Models;
using System.Data;

namespace EFMVCPlatform.IDAL
{
    public interface IUserDAL:IBaseDAL<Models.UserRights.AdminUser>
    {
        void Test();
        DataTable GetCustomData();
    }
}
