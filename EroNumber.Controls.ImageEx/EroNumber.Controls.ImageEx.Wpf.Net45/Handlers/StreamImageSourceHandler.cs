using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

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

            context.Result = await Task.Run(() =>
            {
                var asyncBitmap = new BitmapImage();
                asyncBitmap.BeginInit();
                asyncBitmap.StreamSource = stream;
                asyncBitmap.EndInit();
                asyncBitmap.Freeze();
                return asyncBitmap;
            }, cancellationToken);
        }
    }
}