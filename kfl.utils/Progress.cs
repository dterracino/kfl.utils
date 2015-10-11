namespace kfl.utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class Progress
    {
        public static IEnumerable<T> ShowProgress<T>(this IEnumerable<T> source, int total, Action<string> print)
        {
            int i = 0;
            foreach (var item in source)
            {
                string output = string.Format("{0:10}/{1} ({2:P2})", i + 1, total, (i + 1) / (double)total);
                if (print != null) print(output);
                yield return item;
            }
        }
    }
}
