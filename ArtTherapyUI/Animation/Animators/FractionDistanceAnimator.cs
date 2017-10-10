using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace ArtTherapyUI.Animation.Animators
{
    public class FractionDistanceAnimator : DependencyObject, IArrangeAnimator
    {
        public double Fraction { get; set; }

        public FractionDistanceAnimator(double fraction)
        {
            Fraction = fraction;
        }

        public Rect Arrange(double elapsedTime, Point desiredPosition, Size desiredSize, Point currentPosition, Size currentSize)
        {
            double deltaX = (desiredPosition.X - currentPosition.X) * Fraction;
            double deltaY = (desiredPosition.Y - currentPosition.Y) * Fraction;
            double deltaW = (desiredSize.Width - currentSize.Width) * Fraction;
            double deltaH = (desiredSize.Height - currentSize.Height) * Fraction;

            return new Rect(currentPosition.X + deltaX, currentPosition.Y + deltaY, currentSize.Width + deltaW, currentSize.Height + deltaH);
        }
    }
}
