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

        /// <summary>
        /// Useful to make things fluent
        /// </summary>
        /// <remarks>
        /// <![CDATA[
        /// This code:
        /// 
        ///     var thing = new Thing();
        ///     thing.Config.X = 1;
        ///     
        /// becomes:
        /// 
        ///     var thing = new Thing().With(_ => _.Config.X = 1);
        /// ]]>
        /// </remarks>
        public static T With<T>(this T obj, Action<T> action)
        {
            action(obj);
            return obj;
        }
    }
}
