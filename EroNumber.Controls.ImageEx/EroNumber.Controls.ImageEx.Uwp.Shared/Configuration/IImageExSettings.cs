using System;
using System.Net.Http;
using EroNumber.Cache;
using EroNumber.Handlers;

namespace EroNumber.Configuration
{
    public interface IImageExSettings
    {
        IImageExSettings UseDiskCache(Func<IDiskCache> diskCache);

        IImageExSettings UseHttpHandler(Func<HttpMessageHandler> httpHandler);

        IImageExSettings UseSourceHandler(Func<HandlerDelegate, HandlerDelegate> handler);

        IImageExSettings UseSourceHandler<T>() where T : IImageSourceHandler;
    }
}