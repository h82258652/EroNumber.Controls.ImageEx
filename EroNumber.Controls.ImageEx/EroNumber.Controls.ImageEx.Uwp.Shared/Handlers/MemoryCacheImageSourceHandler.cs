using System;
using System.Threading;
using System.Threading.Tasks;
using Weakly;
using Windows.UI.Xaml.Media;

namespace EroNumber.Handlers
{
    public class MemoryCacheImageSourceHandler : ImageSourceHandlerBase
    {
        private static readonly WeakValueDictionary<object, ImageSource> CacheImageSources = new WeakValueDictionary<object, ImageSource>();

        public override async Task GetImageSourceAsync(ImageSourceContext context, HandlerDelegate next, CancellationToken cancellationToken)
        {
            if (IsInDesignMode)
            {
                await next(context, cancellationToken);
                return;
            }

            var source = context.Current;
            if (source is string || source is Uri)
            {
                if (CacheImageSources.TryGetValue(source, out var cacheImageSource))
                {
                    context.Result = cacheImageSource;
                    return;
                }

                await next(context, cancellationToken);

                var result = context.Result;
                if (result != null)
                {
                    CacheImageSources[source] = result;
                }
            }
            else
            {
                await next(context, cancellationToken);
            }
        }
    }
}