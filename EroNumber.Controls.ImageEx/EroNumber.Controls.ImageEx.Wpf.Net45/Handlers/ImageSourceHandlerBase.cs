using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace EroNumber.Handlers
{
    public abstract class ImageSourceHandlerBase : IImageSourceHandler
    {
        protected bool IsInDesignMode => (bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue;

        public virtual void Dispose()
        {
        }

        public abstract Task GetImageSourceAsync(ImageSourceContext context, HandlerDelegate next, CancellationToken cancellationToken);
    }
}