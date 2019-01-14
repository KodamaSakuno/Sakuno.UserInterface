using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Sakuno.UserInterface.Media.Effects
{
    public class GrayscaleEffect : ShaderEffect
    {
        static PixelShader _pixelShader = new PixelShader()
        {
            UriSource = new Uri(@"pack://application:,,,/Sakuno.UserInterface;component/Media/Effects/GrayscaleEffect.ps")
        };

        public static readonly DependencyProperty InputProperty =
            RegisterPixelShaderSamplerProperty(nameof(Input), typeof(GrayscaleEffect), 0);

        public Brush Input
        {
            get => (Brush)GetValue(InputProperty);
            set => SetValue(InputProperty, value);
        }

        public static readonly DependencyProperty DesaturationFactorProperty =
            DependencyProperty.Register(nameof(DesaturationFactor), typeof(double), typeof(GrayscaleEffect),
                new UIPropertyMetadata(BoxedConstants.Double.Zero, PixelShaderConstantCallback(0)), IsDesaturationFactorValid);

        static bool IsDesaturationFactorValid(object boxedValue)
        {
            var value = (double)boxedValue;

            return value >= .0 && value <= 1.0;
        }

        public double DesaturationFactor
        {
            get => (double)GetValue(DesaturationFactorProperty);
            set => SetValue(DesaturationFactorProperty, value);
        }

        public GrayscaleEffect()
        {
            PixelShader = _pixelShader;

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(DesaturationFactorProperty);
        }
    }
}
