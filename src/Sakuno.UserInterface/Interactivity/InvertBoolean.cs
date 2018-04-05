using Sakuno.Reflection;

namespace Sakuno.UserInterface.Interactivity
{
    public sealed class InvertBoolean : PropertyOperationAction
    {
        protected override void Invoke(PropertyAccessor propertyAccessor, object target)
        {
            var value = (bool)propertyAccessor.GetValue(target);

            propertyAccessor.SetValue(target, BooleanUtil.GetBoxed(!value));
        }
    }
}
