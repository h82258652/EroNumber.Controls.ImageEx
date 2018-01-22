using EroNumber.Cache;
using EroNumber.Configuration;
using Windows.UI.Xaml.Media;

namespace EroNumber.Handlers
{
    public class ImageSourceContext
    {
        private readonly ImageExSettings _settings;
        private IDiskCache _diskCache;

        internal ImageSourceContext(object source, ImageExSettings settings)
        {
            Current = source;
            _settings = settings;
            OriginSource = source;
        }

        public object Current { get; set; }

        public byte[] HttpContentBytes { get; set; }

        public object OriginSource { get; }

        public ImageSource Result { get; set; }

        internal IDiskCache DiskCache
        {
            get
            {
                _diskCache = _diskCache ?? _settings.DiskCache();
                return _diskCache;
            }
        }
    }
}