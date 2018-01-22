using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EroNumber.Cache;
using EroNumber.Handlers;

namespace EroNumber.Configuration
{
    public class ImageExSettings : IImageExSettings
    {
        private readonly IList<Func<HandlerDelegate, HandlerDelegate>> _sourceHandlers = new List<Func<HandlerDelegate, HandlerDelegate>>();

        internal Func<IDiskCache> DiskCache
        {
            get;
            private set;
        }

        internal Func<HttpMessageHandler> HttpHandler
        {
            get;
            private set;
        }

        internal HandlerDelegate SourceHandler
        {
            get;
            private set;
        }

        public IImageExSettings UseDiskCache(Func<IDiskCache> diskCache)
        {
            if (diskCache == null)
            {
                throw new ArgumentNullException(nameof(diskCache));
            }

            DiskCache = diskCache;
            return this;
        }

        public IImageExSettings UseHttpHandler(Func<HttpMessageHandler> httpHandler)
        {
            if (httpHandler == null)
            {
                throw new ArgumentNullException(nameof(httpHandler));
            }

            HttpHandler = httpHandler;
            return this;
        }

        public IImageExSettings UseSourceHandler(Func<HandlerDelegate, HandlerDelegate> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            _sourceHandlers.Add(handler);
            return this;
        }

        public IImageExSettings UseSourceHandler<T>() where T : IImageSourceHandler
        {
            return UseSourceHandler(next =>
            {
                return (context, cancellationToken) =>
                {
                    var handler = CreateSourceHandler<T>(context);
                    try
                    {
                        return handler.GetImageSourceAsync(context, next, cancellationToken);
                    }
                    finally
                    {
                        handler.Dispose();
                    }
                };
            });
        }

        internal void Build()
        {
            HandlerDelegate end = (context, cancellationToken) =>
            {
                if (context.Result == null)
                {
                    throw new NotSupportedException();
                }

                return Task.FromResult<object>(null);
            };

            foreach (var handler in _sourceHandlers.Reverse())
            {
                end = handler(end);
            }

            SourceHandler = end;
        }

        private IImageSourceHandler CreateSourceHandler<T>(ImageSourceContext context) where T : IImageSourceHandler
        {
            var type = typeof(T);

            var constructorInfo = type.GetConstructors().Single();
            var parameterInfos = constructorInfo.GetParameters();
            var parameterCount = parameterInfos.Length;
            var parameters = new object[parameterCount];
            for (var i = 0; i < parameterCount; i++)
            {
                var parameterInfo = parameterInfos[i];
                var parameterType = parameterInfo.ParameterType;
                if (typeof(IImageExSettings).IsAssignableFrom(parameterType))
                {
                    parameters[i] = this;
                }
                else if (typeof(HttpMessageHandler).IsAssignableFrom(parameterType))
                {
                    parameters[i] = HttpHandler();
                }
                else if (typeof(IDiskCache).IsAssignableFrom(parameterType))
                {
                    parameters[i] = context.DiskCache;
                }
                else
                {
                    throw new NotSupportedException();
                }
            }

            return (IImageSourceHandler)constructorInfo.Invoke(parameters);
        }
    }
}