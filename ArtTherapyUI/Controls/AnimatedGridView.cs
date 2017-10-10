using ArtTherapyUI.Animation;
using ArtTherapyUI.Animation.Animators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace ArtTherapyUI.Controls
{
    public class AnimatedGridView : Microsoft.Toolkit.Uwp.UI.Controls.AdaptiveGridView
    {
        private DispatcherTimer animationTimer;
        private DateTime lastArrange = DateTime.MinValue;

        public IArrangeAnimator Animator { get; set; }

        public AnimatedGridView()
            : this(new FractionDistanceAnimator(0.3), TimeSpan.FromSeconds(0.05))
        {
        }

        public AnimatedGridView(IArrangeAnimator animator, TimeSpan animationInterval)
        {
            animationTimer = AnimationBase.CreateAnimationTimer(this, animationInterval);
            Animator = animator;
        }

        public bool IsLoad { get; set; }

        protected override Size ArrangeOverride(Size finalSize)
        {
            finalSize = base.ArrangeOverride(finalSize);

            AnimationBase.Arrange(Math.Max(0, (DateTime.Now - lastArrange).TotalSeconds), this, ItemsPanelRoot?.Children, Animator);
            lastArrange = DateTime.Now;

            return finalSize;
        }
    }
}
