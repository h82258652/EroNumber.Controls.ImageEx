using System;

namespace EroNumber.Controls
{
    public class ImageFailedEventArgs : ExceptionEventArgs
    {
        public ImageFailedEventArgs(object source, Exception failedException) : base(failedException)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            Source = source;
        }

        public object Source { get; }
    }
}