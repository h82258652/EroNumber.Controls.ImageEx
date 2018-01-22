using System.Threading;
using System.Threading.Tasks;

namespace EroNumber.Handlers
{
    public delegate Task HandlerDelegate(ImageSourceContext context, CancellationToken cancellationToken);
}