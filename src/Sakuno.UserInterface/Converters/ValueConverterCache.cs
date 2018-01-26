using System;
using System.Collections.Concurrent;
using System.Windows.Data;

namespace Sakuno.UserInterface.Converters
{
    public static class ValueConverterCache
    {
        static ConcurrentDictionary<Type, IValueConverter> _vcCache = new ConcurrentDictionary<Type, IValueConverter>();
        static ConcurrentDictionary<Type, IMultiValueConverter> _mvcCache = new ConcurrentDictionary<Type, IMultiValueConverter>();

        public static T GetValueConverter<T>() where T : IValueConverter, new() => Single<T>.Instance;
        public static T GetMultiValueConverter<T>() where T : IMultiValueConverter, new() => Multiple<T>.Instance;

        public static IValueConverter GetValueConverter(Type type)
        {
            if (type.IsInterface || type.IsAbstract)
                throw new ArgumentException(nameof(type));

            var isSingleValueConverter = type.IsAssignableTo<IValueConverter>();
            if (!isSingleValueConverter)
                throw new ArgumentException(nameof(type));

            if (_vcCache.TryGetValue(type, out var result))
                return result;

            return (IValueConverter)InitializeValueConverter(type, isSingleValueConverter, type.IsAssignableTo<IMultiValueConverter>());
        }
        public static IMultiValueConverter GetMultiValueConverter(Type type)
        {
            if (type.IsInterface || type.IsAbstract)
                throw new ArgumentException(nameof(type));

            var isMultipleValueConverter = type.IsAssignableTo<IMultiValueConverter>();
            if (!isMultipleValueConverter)
                throw new ArgumentException(nameof(type));

            if (_mvcCache.TryGetValue(type, out var result))
                return result;

            return (IMultiValueConverter)InitializeValueConverter(type, type.IsAssignableTo<IValueConverter>(), isMultipleValueConverter);
        }

        static object InitializeValueConverter(Type type, bool isSingleValueConverter, bool isMultipleValueConverter)
        {
            var result = Activator.CreateInstance(type);

            if (isSingleValueConverter)
                _vcCache.TryAdd(type, (IValueConverter)result);

            if (isMultipleValueConverter)
                _mvcCache.TryAdd(type, (IMultiValueConverter)result);

            return result;
        }

        static class Single<T> where T : IValueConverter, new()
        {
            public static readonly T Instance = (T)GetValueConverter(typeof(T));
        }
        static class Multiple<T> where T : IMultiValueConverter, new()
        {
            public static readonly T Instance = (T)GetMultiValueConverter(typeof(T));
        }
    }
}
