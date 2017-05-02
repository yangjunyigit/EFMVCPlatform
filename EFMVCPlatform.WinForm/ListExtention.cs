using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVCPlatform.WinForm
{
    public static class ListExtention
    {
        public static void Sort<T,TKey>(this List<T> list,Func<T, TKey> condition)
        {
            //string.Compare()
        }
    }
}
