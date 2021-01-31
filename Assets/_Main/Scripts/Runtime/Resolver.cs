#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Game
{
    public static class Resolver
    {
        static readonly Dictionary<Type, object> k_Dict = new Dictionary<Type, object>();

        public static T Get<T>() where T : class
        {
            if (k_Dict.TryGetValue(typeof(T), out var value))
            {
                return (T)value;
            }
            throw new InvalidOperationException($"[{nameof(Resolver)}] 未登録のクラス '{typeof(T).Name}' を要求した");
        }

        public static void Register<T>(in T instance) where T : class
            => k_Dict.Add(typeof(T), instance);

        public static bool TryGet<T>([NotNullWhen(true)] out T? instance) where T : class
        {
            if (k_Dict.TryGetValue(typeof(T), out var value))
            {
                instance = (T)value;
                return true;
            }
            instance = null;
            return false;
        }

        public static void Unregister<T>()
        {
            var key = typeof(T);
            if (k_Dict.TryGetValue(key, out var value))
            {
                if (value is IDisposable disposable)
                {
                    disposable.Dispose();
                }
                k_Dict.Remove(key);
            }
        }

        public static void UnregisterAll()
        {
            foreach (var value in k_Dict.Values)
            {
                if (value is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
            k_Dict.Clear();
        }
    }
}
