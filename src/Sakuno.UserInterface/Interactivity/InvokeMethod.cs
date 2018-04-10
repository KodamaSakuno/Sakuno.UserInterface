using Sakuno.Reflection;
using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Sakuno.UserInterface.Interactivity
{
    [ContentProperty(nameof(Arguments))]
    public class InvokeMethod : TargetedTriggerAction
    {
        public static readonly DependencyProperty MethodProperty =
            DependencyProperty.Register(nameof(Method), typeof(string), typeof(InvokeMethod));

        public string Method
        {
            get => (string)GetValue(MethodProperty);
            set => SetValue(MethodProperty, value);
        }

        static readonly DependencyPropertyKey ArgumentsPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(Arguments), typeof(MethodArgumentCollection), typeof(InvokeMethod), null);

        public static readonly DependencyProperty ParametersProperty = ArgumentsPropertyKey.DependencyProperty;

        MethodArgumentCollection _arguments;

        public MethodArgumentCollection Arguments
        {
            get
            {
                var result = (MethodArgumentCollection)GetValue(ParametersProperty);
                if (result == null)
                {
                    _arguments = result = new MethodArgumentCollection();

                    SetValue(ArgumentsPropertyKey, _arguments);
                }

                return result;
            }
        }

        Type _targetType;
        MethodInfo _method;
        MethodInvoker _methodInvoker;

        public InvokeMethod()
        {
            BindingOperations.SetBinding(this, TargetObjectProperty, new Binding());
        }

        protected override void Invoke(object args)
        {
            var target = Target;
            if (target == null)
                return;

            var method = Method;
            if (method.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Method));

            var targetType = target.GetType();
            if (_targetType != targetType || _method == null || (_arguments != null && _arguments.IsSignatureChanged))
            {
                if (_arguments == null || _arguments.Types.Length == 0)
                    _method = targetType.GetMethod(Method, Array.Empty<Type>());
                else
                    _method = targetType.GetMethod(Method, _arguments.Types);

                if (_method == null)
                    if (_arguments == null || _arguments.Types.Length == 0)
                        throw new InvalidOperationException($"Method \"{Method}()\" does not exist on type \"{targetType.FullName}\".");
                    else
                        throw new InvalidOperationException($"Method \"{Method}({string.Join(", ", _arguments.Types.Select(r => r.Name))})\" does not exist on type \"{targetType.FullName}\".");

                if (_arguments != null)
                    _arguments.IsSignatureChanged = false;

                _targetType = targetType;
                _methodInvoker = ReflectionCache.GetMethodInvoker(_method);
            }

            _methodInvoker.Invoke(target, _arguments?.Values);
        }
    }
}
