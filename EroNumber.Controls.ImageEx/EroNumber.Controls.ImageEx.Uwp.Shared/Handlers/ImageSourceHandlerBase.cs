using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace EroNumber.Handlers
{
    public abstract class ImageSourceHandlerBase : IImageSourceHandler
    {
        protected bool IsInDesignMode => DesignMode.DesignModeEnabled;

        public virtual void Dispose()
        {
        }

        public abstract Task GetImageSourceAsync(ImageSourceContext context, HandlerDelegate next, CancellationToken cancellationToken);
    }
}