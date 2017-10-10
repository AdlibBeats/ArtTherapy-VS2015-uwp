using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtTherapyUI.Animation;
using ArtTherapyUI.Animation.Animators;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace ArtTherapyUI.Controls
{
    public class AnimatedWrapPanel : Microsoft.Toolkit.Uwp.UI.Controls.WrapPanel.WrapPanel
    {
        private DispatcherTimer animationTimer;
        private DateTime lastArrange = DateTime.MinValue;

        public IArrangeAnimator Animator { get; set; }

        public AnimatedWrapPanel()
            : this(new FractionDistanceAnimator(0.4), TimeSpan.FromSeconds(0.1))
        {
        }

        public AnimatedWrapPanel(IArrangeAnimator animator, TimeSpan animationInterval)
        {
            animationTimer = AnimationBase.CreateAnimationTimer(this, animationInterval);
            Animator = animator;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            finalSize = base.ArrangeOverride(finalSize);

            AnimationBase.Arrange(Math.Max(0, (DateTime.Now - lastArrange).TotalSeconds), this, Children, Animator);
            lastArrange = DateTime.Now;

            return finalSize;
        }
    }
}
