using System.Threading.Tasks;
using Windows.Foundation;

namespace ArtTherapyUI.Animation
{
    public interface IArrangeAnimator
    {
        Rect Arrange(double elapsedTime, Point desiredPosition, Size desiredSize, Point currentPosition, Size currentSize);
    }
}
