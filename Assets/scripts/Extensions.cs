using System;

namespace DefaultNamespace
{
    public static class Extentions
    {
        public static void Invoke<T>(this T obj, Action<T> action) => action(obj);

        public static void Times(this int count, Action<int> action)
        {
            for (var i = 1; i <= count; i++)
                action(i);
        }
    }
}
