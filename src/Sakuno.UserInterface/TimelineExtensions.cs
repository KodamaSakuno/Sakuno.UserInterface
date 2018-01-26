using System;
using System.ComponentModel;
using System.Windows.Media.Animation;

namespace Sakuno.UserInterface
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class TimelineExtensions
    {
        public static void OnCompleted(this Timeline timeline, Action continuation)
        {
            timeline.Completed += Handler;

            void Handler(object sender, EventArgs e)
            {
                timeline.Completed -= Handler;

                continuation();
            }
        }
    }
}
