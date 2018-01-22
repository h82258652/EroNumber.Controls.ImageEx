using System;
using System.Threading;
using System.Threading.Tasks;

namespace EroNumber.Handlers
{
    public class StringImageSourceHandler : ImageSourceHandlerBase
    {
        public override async Task GetImageSourceAsync(ImageSourceContext context, HandlerDelegate next, CancellationToken cancellationToken)
        {
            var source = context.Current as string;
            if (source == null)
            {
                await next(context, cancellationToken);
                return;
            }

            context.Current = ToUriSource(source);
            await next(context, cancellationToken);
        }

        private static Uri ToUriSource(string source)
        {
            Uri uriSource;
            if (Uri.TryCreate(source, UriKind.RelativeOrAbsolute, out uriSource))
            {
                if (!uriSource.IsAbsoluteUri)
                {
                    Uri.TryCreate("ms-appx:///" + (source.StartsWith("/") ? source.Substring(1) : source), UriKind.Absolute, out uriSource);
                }
            }

            if (uriSource == null)
            {
                throw new NotSupportedException();
            }

            return uriSource;
        }
    }
}