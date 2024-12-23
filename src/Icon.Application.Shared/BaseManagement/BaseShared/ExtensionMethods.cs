using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Icon.BaseManagement.BaseShared
{
    public static class ExtensionMethods
    {
        public static T Apply<T>(this T obj, Action<T> action)
        {
            action(obj);
            return obj;
        }
    }

}