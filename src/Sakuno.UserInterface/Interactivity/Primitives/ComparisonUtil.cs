using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Sakuno.UserInterface.Interactivity.Primitives
{
    static class ComparisonUtil
    {
        static IDictionary<Type, TypeConverter> _typeConverters = new ConcurrentDictionary<Type, TypeConverter>();

        public static bool Evaluate(object left, object right, ComparisonType comparison)
        {
            if (left != null && right is string rightString)
            {
                if (left is string leftString)
                {
                    var result = string.Compare(leftString, rightString, StringComparison.OrdinalIgnoreCase);

                    return EvaluateCore(result, comparison);
                }

                var leftType = left.GetType();

                if (!_typeConverters.TryGetValue(leftType, out var typeConverter))
                {
                    typeConverter = TypeDescriptor.GetConverter(leftType);
                    _typeConverters[leftType] = typeConverter;
                }

                if (typeConverter != null)
                    right = typeConverter.ConvertFromInvariantString(rightString);
            }

            if (left is IComparable leftComparable && right is IComparable)
            {
                right = Convert.ChangeType(right, left.GetType(), CultureInfo.InvariantCulture);

                var result = leftComparable.CompareTo((IComparable)right);

                return EvaluateCore(result, comparison);
            }

            switch (comparison)
            {
                case ComparisonType.Equal:
                    return Equals(left, right);

                case ComparisonType.NotEqual:
                    return !Equals(left, right);

                case ComparisonType.LessThan:
                case ComparisonType.LessThanOrEqual:
                case ComparisonType.GreaterThan:
                case ComparisonType.GreaterThanOrEqual:
                    throw new ArgumentException();

                default: throw new ArgumentException(nameof(comparison));
            }
        }

        static bool EvaluateCore(int result, ComparisonType comparison)
        {
            switch (comparison)
            {
                case ComparisonType.Equal:
                    return result == 0;

                case ComparisonType.NotEqual:
                    return result != 0;

                case ComparisonType.LessThan:
                    return result < 0;

                case ComparisonType.LessThanOrEqual:
                    return result <= 0;

                case ComparisonType.GreaterThan:
                    return result > 0;

                case ComparisonType.GreaterThanOrEqual:
                    return result >= 0;

                default: throw new ArgumentException(nameof(comparison));
            }
        }
    }
}
