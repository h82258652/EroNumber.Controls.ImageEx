using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace EroNumber.Handlers
{
    public class DirectImageSourceHandler : ImageSourceHandlerBase
    {
        public override async Task GetImageSourceAsync(ImageSourceContext context, HandlerDelegate next, CancellationToken cancellationToken)
        {
            if (context.Current is ImageSource imageSource)
            {
                context.Result = imageSource;
                return;
            }

            await next(context, cancellationToken);
        }
    }
}