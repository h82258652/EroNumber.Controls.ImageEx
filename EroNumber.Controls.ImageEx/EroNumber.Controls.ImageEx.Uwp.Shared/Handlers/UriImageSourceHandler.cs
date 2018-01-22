using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EroNumber.Utils;
using Windows.UI.Xaml.Media.Imaging;

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
                var bitmapImage = new BitmapImage()
                {
                    UriSource = uri
                };
                context.Result = bitmapImage;
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