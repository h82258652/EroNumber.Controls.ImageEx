using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace EroNumber.Handlers
{
    public class ByteArrayImageSourceHandler : ImageSourceHandlerBase
    {
        public override async Task GetImageSourceAsync(ImageSourceContext context, HandlerDelegate next, CancellationToken cancellationToken)
        {
            var bytes = context.Current as byte[];
            if (bytes == null)
            {
                await next(context, cancellationToken);
                return;
            }

            context.Current = new MemoryStream(bytes);
            await next(context, cancellationToken);
        }
    }
}