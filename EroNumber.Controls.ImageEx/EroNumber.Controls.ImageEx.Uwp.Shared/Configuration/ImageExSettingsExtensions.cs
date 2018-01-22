using System;
using System.Net.Http;
using EroNumber.Cache;
using EroNumber.Handlers;

namespace EroNumber.Configuration
{
    public static class ImageExSettingsExtensions
    {
        public static IImageExSettings UseDefault(this IImageExSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            return settings
                .UseSourceHandler<DirectImageSourceHandler>()
                .UseSourceHandler<MemoryCacheImageSourceHandler>()
                .UseSourceHandler<StringImageSourceHandler>()
                .UseSourceHandler<DiskCacheImageSourceHandler>()
                .UseSourceHandler<UriImageSourceHandler>()
                .UseSourceHandler<ByteArrayImageSourceHandler>()
                .UseSourceHandler<StreamImageSourceHandler>()
                .UseDefaultHttpHandler()
                .UseDefaultDiskCache();
        }

        public static IImageExSettings UseDefaultDiskCache(this IImageExSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            return settings.UseDiskCache(() => new DefaultDiskCache());
        }

        public static IImageExSettings UseDefaultHttpHandler(this IImageExSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            return settings.UseHttpHandler(() => new HttpClientHandler());
        }
    }
}