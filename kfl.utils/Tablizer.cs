namespace kfl.utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Reflection;
    using System.Linq.Expressions;
    using System.Collections.Concurrent;

    public class Tablizer
    {
        private static readonly ConcurrentDictionary<Type, TypeInfo[]> TypeRegister = new ConcurrentDictionary<Type, TypeInfo[]>();

        private static TypeInfo[] GetOrRegister<T>()
        {
            TypeInfo[] info;
            if (TypeRegister.TryGetValue(typeof(T), out info)) return info;

            MemberInfo[] members = typeof(T).GetMembers(System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.Instance
                | System.Reflection.BindingFlags.GetField
                | System.Reflection.BindingFlags.GetProperty)
                .Where(m => m is FieldInfo || m is PropertyInfo)
                .ToArray();

            var para = Expression.Parameter(typeof(object), "obj");
            var typed = Expression.Convert(para, typeof(T));

            Func<MemberInfo, Func<object, object>> getGetter = m => Expression.Lambda<Func<object, object>>(
                Expression.Convert(
                    Expression.PropertyOrField(typed, m.Name),
                    typeof(object)),
                para).Compile();

            info = members.Select(m => new TypeInfo { Name = m.Name, Getter = getGetter(m) }).ToArray();
            TypeRegister.TryAdd(typeof(T), info);
            return info;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs"></param>
        /// <param name="columeOptions">column options. an option begins with the field/prop name. if trailed with '-' means left aligned</param>
        /// <returns></returns>
        public static IEnumerable<string> Tablize<T>(IEnumerable<T> objs, params string[] columeOptions)
        {
            var info = GetOrRegister<T>();

            // TODO: TypeRegister can cache a TableFormatter. But need to make a base non-generic TableFormatter
            Func<TypeInfo, string> getHeader = _ => columeOptions.FirstOrDefault(opt => opt.StartsWith(_.Name)) ?? _.Name;
            var formatter = new TableFormatter<T>()
                .SetHeaders(info.Select(getHeader).ToArray())
                .SetColumns(info.Select(_ => (Func<T, object>)(t => _.Getter(t))).ToArray());
            return formatter.Format(objs);
        }

        class TypeInfo
        {
            public string Name;
            public Func<object, object> Getter;
        }

        class TableFormatter<T>
        {
            public string[] Names { get; private set; }
            public Func<T, object>[] Getters { get; private set; }
            public string[] Alignments { get; private set; }
            
            /// <remarks>
            /// if the last char of a colume header is '-', the colume is left aligned instead
            /// </remarks>
            public TableFormatter<T> SetHeaders(params string[] names)
            {
                Names = names;

                if (Getters != null && Names != null && Getters.Length != Names.Length)
                {
                    throw new ArgumentException($"Number of columns ({Names.Length}) does not match number of column getters ({Getters.Length})");
                }

                Alignments = names.Select(_ =>
                {
                    switch (_[_.Length - 1])
                    {
                        case '-':
                            return "-";
                        default:
                            return "";
                    }
                }).ToArray();

                return this;
            }

            public TableFormatter<T> SetColumns(params Func<T, object>[] columns)
            {
                Getters = columns;

                if (Getters != null && Names != null && Getters.Length != Names.Length)
                {
                    throw new ArgumentException($"Number of columns ({Names.Length}) does not match number of column getters ({Getters.Length})");
                }

                return this;
            }

            static readonly Func<int, int, int> max = (x, y) => x > y ? x : y;

            public IEnumerable<string> Format(IEnumerable<T> input)
            {
                var cached = input.ToList();
                int[] widths = cached.Aggregate(
                    Names.Select(_ => _.Length).ToArray(), // seed
                    (accu, current) => Enumerable.Zip(
                        accu,
                        Getters.Select(_ => _(current).ToString().Length),
                        max).ToArray());

                string template = string.Join("", Enumerable.Range(0, widths.Length).Select(i => string.Format("{{{0},{2}{1}}}", i, (int)(widths[i] * 2), Alignments[i])));

                yield return string.Format(template, Names);
                foreach (var obj in cached)
                {
                    yield return string.Format(template, Getters.Select(getCol => getCol(obj)).ToArray());
                }
            }
        }
    }
}
