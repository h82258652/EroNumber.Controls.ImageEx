using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using EroNumber.Utils;

namespace EroNumber.Handlers
{
    public class UriImageSourceHandler : ImageSourceHandlerBase
    {
        private readonly HttpMessageHandler _httpHandler;

        public UriImageSourceHandler(HttpMessageHandler httpHandler)
        {
            _httpHandler = httpHandler;
        }

        public override async Task GetImageSourceAsync(ImageSourceContext context, HandlerDelegate next, CancellationToken cancellationToken)
        {
            var uri = context.Current as Uri;
            if (uri == null)
            {
                await next(context, cancellationToken);
                return;
            }

            if (IsInDesignMode)
            {
                context.Result = new BitmapImage(uri);
                return;
            }

            if (!UriHelper.IsHttpUri(uri))
            {
                context.Result = await Task.Run(() =>
                {
                    var asyncBitmap = new BitmapImage();
                    asyncBitmap.BeginInit();
                    asyncBitmap.UriSource = uri;
                    asyncBitmap.EndInit();
                    asyncBitmap.Freeze();
                    return asyncBitmap;
                }, cancellationToken);
                return;
            }

            using (var client = new HttpClient(_httpHandler))
            {
                var response = await client.GetAsync(uri, cancellationToken);
                response.EnsureSuccessStatusCode();
                var bytes = await response.Content.ReadAsByteArrayAsync();
                context.Current = bytes;
                await next(context, cancellationToken);
                var cacheControl = response.Headers.CacheControl;
                if (cacheControl != null && !cacheControl.NoCache)
                {
                    context.HttpContentBytes = bytes;
                }
            }
        }
    }
}