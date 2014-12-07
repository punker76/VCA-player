using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace Rina.Controls
{
    public class AnimatedContentControl : ContentControl
    {
        #region Generated static constructor
        static AnimatedContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnimatedContentControl), new FrameworkPropertyMetadata(typeof(AnimatedContentControl)));
        }
        #endregion

        /// <summary>
        /// This gets called when the template has been applied and we have our visual tree
        /// </summary>
        public override void OnApplyTemplate()
        {
            this.RenderTransform = new TranslateTransform();

            base.OnApplyTemplate();
        }

        /// <summary>
        /// This gets called when the content we're displaying has changed
        /// </summary>
        /// <param name="oldContent">The content that was previously displayed</param>
        /// <param name="newContent">The new content that is displayed</param>
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            BeginAnimateContentReplacement();
            base.OnContentChanged(oldContent, newContent);
        }

        /// <summary>
        /// Starts the animation for the new content
        /// </summary>
        private void BeginAnimateContentReplacement()
        {

            this.BeginAnimation(UIElement.OpacityProperty, CreateAnimation(0, 1, TimeSpan.FromSeconds(0.5)));
            this.RenderTransform.BeginAnimation(TranslateTransform.XProperty, CreateAnimation(this.ActualWidth / 5, 0, TimeSpan.FromSeconds(0.5),
                (s, e) =>
                {
                    this.Visibility = System.Windows.Visibility.Hidden;
                    this.Visibility = System.Windows.Visibility.Visible;
                }));
        }

        /// <summary>
        /// Creates the animation that moves content in or out of view.
        /// </summary>
        /// <param name="from">The starting value of the animation.</param>
        /// <param name="to">The end value of the animation.</param>
        /// <param name="whenDone">(optional) A callback that will be called when the animation has completed.</param>
        private static AnimationTimeline CreateAnimation(double from, double to, TimeSpan timeDuration, EventHandler whenDone = null)
        {
            var duration = new Duration(timeDuration);
            var anim = new DoubleAnimation(from, to, duration);// { EasingFunction = ease };
            if (whenDone != null)
                anim.Completed += whenDone;
            anim.Freeze();

            return anim;
        }
    }
}
