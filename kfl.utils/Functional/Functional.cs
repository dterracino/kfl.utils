using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kfl.Utils.Functional
{
    public static class Functional
    {
        public static T Min<T>(T x, T y) where T : IComparable, IComparable<T>
        {
            return x.CompareTo(y) > 0 ? y : x;
        }

        public static T Max<T>(T x, T y) where T : IComparable, IComparable<T>
        {
            return x.CompareTo(y) > 0 ? x : y;
        }
    }
}
