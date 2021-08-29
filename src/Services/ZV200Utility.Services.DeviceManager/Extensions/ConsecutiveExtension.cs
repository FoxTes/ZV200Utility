using System.Collections.Generic;
using System.Linq;
using MemoryPools.Collections;

namespace ZV200Utility.Services.DeviceManager.Extensions
{
    /// <summary>
    /// Расширение, предоставляющие методы для разделения на группы последовательности чисел.
    /// </summary>
    public static class ConsecutiveExtension
    {
        /// <summary>
        /// Разделяет последовательность на группы.
        /// </summary>
        /// <param name="source">Последовательность чисел.</param>
        /// <returns>Перечислитель, с начальным и конечным числом группы.</returns>
        public static IEnumerable<IEnumerable<int>> GroupConsecutive(this IEnumerable<int> source)
        {
            using var e = source.GetEnumerator();

            for (var more = e.MoveNext(); more;)
            {
                int first = e.Current, last = first, next;
                while ((more = e.MoveNext()) && (next = e.Current) > last && next - last == 1)
                    last = next;
                yield return Enumerable.Range(first, last - first + 1);
            }
        }

        /// <summary>
        /// Разделяет последовательность на группы.
        /// </summary>
        /// <param name="source">Последовательность чисел.</param>
        /// <returns>Кортеж, с начальным числом группы и её длинной.</returns>
        public static IEnumerable<(ushort First, ushort Count)> ConsecutiveRanges(this IPoolingEnumerable<ushort> source)
        {
            using var e = source.GetEnumerator();

            for (var more = e.MoveNext(); more;)
            {
                if (e.Current == 0)
                    break;
                ushort first = e.Current, last = first, next;
                while ((more = e.MoveNext()) && (next = e.Current) > last && next - last == 1)
                    last = next;
                yield return (first, (ushort)(last - first + 1));
            }
        }
    }
}