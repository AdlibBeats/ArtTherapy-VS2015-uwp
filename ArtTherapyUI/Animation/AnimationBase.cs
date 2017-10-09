using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ArtTherapyUI.Animation
{
    public static class AnimationBase
    {
        public static DependencyProperty CurrentPositionProperty = DependencyProperty.RegisterAttached("CurrentPosition", typeof(Point), typeof(AnimationBase), new PropertyMetadata(new Point()));
        public static DependencyProperty CurrentSizeProperty = DependencyProperty.RegisterAttached("CurrentSize", typeof(Size), typeof(AnimationBase), new PropertyMetadata(new Size()));

        public static DependencyProperty OverrideArrangeProperty = DependencyProperty.RegisterAttached("OverrideArrange", typeof(bool), typeof(AnimationBase), new PropertyMetadata(false));
        public static DependencyProperty OverridePositionProperty = DependencyProperty.RegisterAttached("OverridePosition", typeof(Point), typeof(AnimationBase), new PropertyMetadata(new Point()));
        public static DependencyProperty OverrideSizeProperty = DependencyProperty.RegisterAttached("OverrideSize", typeof(Size), typeof(AnimationBase), new PropertyMetadata(new Size()));


        public static DispatcherTimer CreateAnimationTimer(FrameworkElement element, TimeSpan animationInterval)
        {
            DispatcherTimer animationTimer = new DispatcherTimer();
            animationTimer.Interval = animationInterval;
            animationTimer.Tick += (s, e) => { element.InvalidateArrange(); };

            element.SizeChanged += (s, e) =>
            {
                animationTimer.Start();
            };

            return animationTimer;
        }

        public static void Arrange(double elapsedTime, FrameworkElement owner, UIElementCollection elements, IArrangeAnimator animator)
        {
            foreach (var element in elements)
            {
                Point desiredPosition = new Point();
                if ((bool)element.GetValue(AnimationBase.OverrideArrangeProperty))
                    desiredPosition = (Point)element.GetValue(AnimationBase.OverridePositionProperty);
                else
                {
                    GeneralTransform generalTransform = element.TransformToVisual(owner);
                    desiredPosition = generalTransform.TransformPoint(new Point());
                }
                Size desiredSize = new Size();
                if ((bool)element.GetValue(AnimationBase.OverrideArrangeProperty))
                    desiredSize = (Size)element.GetValue(AnimationBase.OverrideSizeProperty);
                else
                    desiredSize = element.DesiredSize;

                Point currentPosition = (Point)element.GetValue(AnimationBase.CurrentPositionProperty);
                Size currentSize = (Size)element.GetValue(AnimationBase.CurrentSizeProperty);
                Rect rect = animator.Arrange(elapsedTime, desiredPosition, desiredSize, currentPosition, currentSize);

                element.SetValue(AnimationBase.CurrentPositionProperty, new Point(rect.Left, rect.Top));
                element.SetValue(AnimationBase.CurrentSizeProperty, new Size(rect.Width, rect.Height));

                element.Arrange(rect);
            }
        }
    }
}
