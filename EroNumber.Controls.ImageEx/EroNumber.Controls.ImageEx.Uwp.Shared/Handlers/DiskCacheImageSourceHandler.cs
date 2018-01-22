using System;
using System.Threading;
using System.Threading.Tasks;
using EroNumber.Cache;
using EroNumber.Utils;

namespace EroNumber.Handlers
{
    public class DiskCacheImageSourceHandler : ImageSourceHandlerBase
    {
        private readonly IDiskCache _diskCache;

        public DiskCacheImageSourceHandler(IDiskCache diskCache)
        {
            _diskCache = diskCache;
        }

        public override async Task GetImageSourceAsync(ImageSourceContext context, HandlerDelegate next, CancellationToken cancellationToken)
        {
            if (IsInDesignMode)
            {
                await next(context, cancellationToken);
                return;
            }

            var uri = context.Current as Uri;
            if (uri == null || !UriHelper.IsHttpUri(uri))
            {
                await next(context, cancellationToken);
                return;
            }

            var cacheKey = uri.AbsoluteUri;

            if (await _diskCache.IsExistAsync(cacheKey))
            {
                context.Current = await _diskCache.GetAsync(cacheKey);
                try
                {
                    await next(context, cancellationToken);
                }
                catch
                {
                    async void AsyncAction()
                    {
                        await _diskCache.DeleteAsync(cacheKey);
                    }
                    AsyncAction();
                    throw;
                }
                return;
            }

            await next(context, cancellationToken);

            {
                async void AsyncAction()
                {
                    var bytes = context.HttpContentBytes;
                    if (bytes != null)
                    {
                        await _diskCache.SetAsync(cacheKey, bytes);
                    }
                }
                AsyncAction();
            }
        }
    }
}