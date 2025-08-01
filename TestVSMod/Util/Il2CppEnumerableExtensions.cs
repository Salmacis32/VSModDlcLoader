using Il2CppInterop.Runtime.InteropTypes;
using Il2Col = Il2CppSystem.Collections.Generic;

namespace TestVSMod.Util
{
    internal static class Il2CppEnumerableExtensions
    {
        /// <summary>
        /// Return as Il2CppSystem.List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static Il2Col.List<T> ToIl2CppList<T>(this IEnumerable<T> enumerable)
        {
            var il2CppList = new Il2Col.List<T>();

            using var enumerator = enumerable.GetEnumerator();
            while (enumerator.MoveNext())
                il2CppList.Add(enumerator.Current);

            return il2CppList;
        }

        /// <summary>
        /// Return as Il2CppSystem.List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppStructArray<T> ToIl2CppArray<T>(this IEnumerable<T> enumerable) where T : unmanaged
        {
            var count = enumerable.Count();
            var il2CppList = new Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppStructArray<T>(count);

            using var enumerator = enumerable.GetEnumerator();
            int i = 0;
            while (enumerator.MoveNext())
            {
                il2CppList[i] = enumerator.Current;
                i++;
            }

            return il2CppList;
        }
    }
}
