using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace Sakuno.UserInterface.Interactivity.Primitives
{
    [ContentProperty(nameof(Storyboard))]
    public abstract class StoryboardAction : TriggerAction
    {
        public static readonly DependencyProperty StoryboardProperty =
            DependencyProperty.Register(nameof(Storyboard), typeof(Storyboard), typeof(StoryboardAction),
                new PropertyMetadata(OnStoryboardChanged));

        static void OnStoryboardChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            ((StoryboardAction)d).OnStoryboardChanged(e);
        protected virtual void OnStoryboardChanged(DependencyPropertyChangedEventArgs e) { }

        [TypeConverter(typeof(NameReferenceConverter))]
        public Storyboard Storyboard
        {
            get => (Storyboard)GetValue(StoryboardProperty);
            set => SetValue(StoryboardProperty, value);
        }
    }
}
