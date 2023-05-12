using System;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Toolbox
{
    public static class EnumUtil
    {
        public static IEnumerable<T> GetEnumValues<T>() where T : Enum 
        {
                return Enum.GetValues(typeof(T)).Cast<T>();
        }

    }
}