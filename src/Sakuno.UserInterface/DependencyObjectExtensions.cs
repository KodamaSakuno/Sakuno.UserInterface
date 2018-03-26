using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Sakuno.UserInterface
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class DependencyObjectExtensions
    {
        static IDictionary<Type, Node> _dpCache = new Dictionary<Type, Node>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPropertyUnset(this DependencyObject obj, DependencyProperty dp) =>
            obj.ReadLocalValue(dp) == DependencyProperty.UnsetValue;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UpdateBindingTarget(this DependencyObject obj, DependencyProperty dp) =>
            BindingOperations.GetBindingExpression(obj, dp)?.UpdateTarget();

        public static void UpdateBindingTargetsOfAllProperties(this DependencyObject target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            var node = GetDependencyProperties(target.GetType());

            do
            {
                if (node.Properties != null)
                    foreach (var property in node.Properties)
                        target.UpdateBindingTarget(property);

                node = node.Base;
            } while (node != null);
        }

        static Node GetDependencyProperties(Type type)
        {
            if (_dpCache.TryGetValue(type, out var result))
                return result;

            _dpCache[type] = result = new Node();

            var currentNode = result;
            var list = new List<DependencyProperty>();

            while (type != typeof(DependencyObject))
            {
                list.Clear();

                var fields = type.GetTypeInfo().DeclaredFields;

                foreach (var field in fields)
                {
                    if (!field.IsPublic || !field.IsStatic || field.FieldType != typeof(DependencyProperty))
                        continue;

                    if (field.GetValue(null) is DependencyProperty dependencyProperty)
                        list.Add(dependencyProperty);
                }

                if (list.Count > 0)
                    currentNode.Properties = list.ToArray();

                type = type.BaseType;

                if (!_dpCache.TryGetValue(type, out var baseNode))
                {
                    currentNode.Base = new Node();
                    _dpCache[type] = currentNode = currentNode.Base;
                }
                else
                {
                    currentNode.Base = baseNode;
                    break;
                }
            }

            return result;
        }

        public static IEnumerable<DependencyObject> EnumerateSelfAndAncestors(this DependencyObject obj)
        {
            while (obj != null)
            {
                yield return obj;

                obj = VisualTreeHelper.GetParent(obj);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetParent<T>(this DependencyObject obj) where T : DependencyObject => VisualTreeHelper.GetParent(obj) as T;

        class Node
        {
            public DependencyProperty[] Properties;

            public Node Base;
        }
    }
}
