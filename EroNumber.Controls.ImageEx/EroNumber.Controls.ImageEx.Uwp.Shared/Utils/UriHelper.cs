using System;

namespace EroNumber.Utils
{
    internal static class UriHelper
    {
        private const string HttpScheme = "http";
        private const string HttpsScheme = "https";

        internal static bool IsHttpUri(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            var scheme = uri.Scheme;
            return uri.IsAbsoluteUri && (string.Equals(scheme, HttpScheme, StringComparison.OrdinalIgnoreCase)
                                         || string.Equals(scheme, HttpsScheme, StringComparison.OrdinalIgnoreCase));
        }
    }
}