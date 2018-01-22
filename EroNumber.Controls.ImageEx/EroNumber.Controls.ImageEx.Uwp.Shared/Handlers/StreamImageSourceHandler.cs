using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace EroNumber.Handlers
{
    public class StreamImageSourceHandler : ImageSourceHandlerBase
    {
        public override async Task GetImageSourceAsync(ImageSourceContext context, HandlerDelegate next, CancellationToken cancellationToken)
        {
            var stream = context.Current as Stream;
            if (stream == null)
            {
                await next(context, cancellationToken);
                return;
            }

            var bitmapImage = new BitmapImage();
            await bitmapImage.SetSourceAsync(stream.AsRandomAccessStream());
            context.Result = bitmapImage;
        }
    }
}