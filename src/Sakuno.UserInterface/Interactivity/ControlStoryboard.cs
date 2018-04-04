using System.Windows;

namespace Sakuno.UserInterface.Interactivity
{
    public class ControlStoryboard : StoryboardAction
    {
        public static readonly DependencyProperty OptionProperty =
            DependencyProperty.Register(nameof(Option), typeof(ControlStoryboardOption), typeof(ControlStoryboard),
                new PropertyMetadata(EnumUtil.GetBoxed(ControlStoryboardOption.Begin)));

        public ControlStoryboardOption Option
        {
            get => (ControlStoryboardOption)GetValue(OptionProperty);
            set => SetValue(OptionProperty, EnumUtil.GetBoxed(value));
        }

        protected override void Invoke(object args)
        {
            if (_associatedObject == null)
                return;

            var storyboard = Storyboard;
            if (storyboard == null)
                return;

            switch (Option)
            {
                case ControlStoryboardOption.Begin:
                    foreach (var child in storyboard.Children)
                        child.UpdateBindingTargetsOfAllProperties();

                    storyboard.Begin();
                    break;

                case ControlStoryboardOption.Stop:
                    storyboard.Stop();
                    break;

                case ControlStoryboardOption.TogglePlayPause:
                    break;

                case ControlStoryboardOption.Pause:
                    storyboard.Pause();
                    break;

                case ControlStoryboardOption.Resume:
                    storyboard.Resume();
                    break;

                case ControlStoryboardOption.SkipToFill:
                    storyboard.SkipToFill();
                    break;
            }
        }
    }
}
