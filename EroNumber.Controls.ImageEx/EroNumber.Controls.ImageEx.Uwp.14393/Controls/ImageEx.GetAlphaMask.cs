using Windows.UI.Composition;

namespace EroNumber.Controls
{
    public partial class ImageEx
    {
        public CompositionBrush GetAlphaMask()
        {
            return _image?.GetAlphaMask();
        }
    }
}