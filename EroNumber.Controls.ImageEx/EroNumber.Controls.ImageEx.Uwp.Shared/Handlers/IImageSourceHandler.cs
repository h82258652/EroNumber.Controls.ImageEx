using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace EroNumber.Handlers
{
    public interface IImageSourceHandler : IDisposable
    {
        Task GetImageSourceAsync([NotNull] ImageSourceContext context, [NotNull] HandlerDelegate next, CancellationToken cancellationToken);
    }
}