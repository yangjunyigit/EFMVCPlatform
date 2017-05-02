using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVCPlatform.IBusiness;
using System.Threading;

namespace EFMVCPlatform.Business
{
    public class SystemManagerBLL: BaseBLL,ISystemManager
    {
        public bool CheckUserInfo(string userName, string password)
        {
            return true;
        }

        public void Test()
        {
            //DataProvider.UserDAL
        }

        public DataTable TestResultData(int num)
        {
            for (int i = 0; i < num; i++)
            {
                ThreadStart ts = new ThreadStart(GetUserData);
                Thread th = new Thread(ts);
                th.Start();
                ThreadPools.ThreadList.Add(th);
            }
            DataTable dt = new DataTable();
            return dt;
        }

        public void AbordThread()
        {
            foreach (Thread item in ThreadPools.ThreadList)
            {
                if(item.IsAlive)
                {
                    item.Abort();
                }
            }
        }

        private void GetUserData()
        {
            DataTable dt = DataProvider.UserDAL.GetCustomData();
        }

        public DataTable TestResultData()
        {
            throw new NotImplementedException();
        }
    }

    public class ThreadPools
    {
        public static List<Thread> ThreadList { get; set; } = new List<Thread>();
    }
}
