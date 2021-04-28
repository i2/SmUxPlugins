using System.Collections;
using System.Collections.Generic;

namespace PluginSystem.Helpers
{
    public static class EnumerableHelepr
    {
        public static T First<T>(this IEnumerable<T> tEnumerable) where T : class
        {
            foreach (var t in tEnumerable)
            {
                return t;
            }

            return null;
        }

        public static T First<T>(this IEnumerable tEnumerable) where T : class
        {
            foreach (var t in tEnumerable)
            {
                return (T)t;
            }

            return null;
        }
    }
}