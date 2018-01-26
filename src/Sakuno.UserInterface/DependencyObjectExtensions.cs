using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Sakuno.UserInterface
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class DependencyObjectExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPropertyUnset(this DependencyObject obj, DependencyProperty dp) =>
            obj.ReadLocalValue(dp) == DependencyProperty.UnsetValue;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UpdateBindingTarget(this DependencyObject obj, DependencyProperty dp) =>
            BindingOperations.GetBindingExpression(obj, dp)?.UpdateTarget();

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
    }
}
