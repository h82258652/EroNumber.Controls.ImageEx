using System;
using System.Threading;
using EroNumber.Controls;
using EroNumber.Handlers;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace EroNumber.Media
{
    [Obsolete("未完成", true)]
    public class ImageBrushEx : XamlCompositionBrushBase
    {
        public static readonly DependencyProperty AlignmentXProperty = DependencyProperty.Register(nameof(AlignmentX), typeof(AlignmentX), typeof(ImageBrushEx), new PropertyMetadata(AlignmentX.Center, OnAlignmentXChanged));
        public static readonly DependencyProperty AlignmentYProperty = DependencyProperty.Register(nameof(AlignmentY), typeof(AlignmentY), typeof(ImageBrushEx), new PropertyMetadata(AlignmentY.Center, OnAlignmentYChanged));
        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(nameof(ImageSource), typeof(object), typeof(ImageBrushEx), new PropertyMetadata(default(object), OnImageSourceChanged));
        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(nameof(Stretch), typeof(Stretch), typeof(ImageBrushEx), new PropertyMetadata(Stretch.Uniform, OnStretchChanged));

        private CancellationTokenSource _loadSourceCancellationTokenSource = new CancellationTokenSource();

        public event EventHandler<ImageFailedEventArgs> ImageFailed;

        public event EventHandler ImageOpened;

        public AlignmentX AlignmentX
        {
            get => (AlignmentX)GetValue(AlignmentXProperty);
            set => SetValue(AlignmentXProperty, value);
        }

        public AlignmentY AlignmentY
        {
            get => (AlignmentY)GetValue(AlignmentYProperty);
            set => SetValue(AlignmentYProperty, value);
        }

        public object ImageSource
        {
            get => GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public Stretch Stretch
        {
            get => (Stretch)GetValue(StretchProperty);
            set => SetValue(StretchProperty, value);
        }

        protected override void OnConnected()
        {
            base.OnConnected();

            SetImageSource(ImageSource);
        }

        protected override void OnDisconnected()
        {
            base.OnDisconnected();

            DisposeCompositionBrush();
        }

        private static void OnAlignmentXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (ImageBrushEx)d;
            var value = (AlignmentX)e.NewValue;

            if (!Enum.IsDefined(typeof(AlignmentX), value))
            {
                throw new ArgumentOutOfRangeException(nameof(AlignmentX));
            }

            var brush = (CompositionSurfaceBrush)obj.CompositionBrush;
            brush.HorizontalAlignmentRatio = (float)value * 0.5f;
        }

        private static void OnAlignmentYChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (ImageBrushEx)d;
            var value = (AlignmentY)e.NewValue;

            if (!Enum.IsDefined(typeof(AlignmentY), value))
            {
                throw new ArgumentOutOfRangeException(nameof(AlignmentY));
            }

            var brush = (CompositionSurfaceBrush)obj.CompositionBrush;
            brush.VerticalAlignmentRatio = (float)value * 0.5f;
        }

        private static void OnImageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (ImageBrushEx)d;
            var value = e.NewValue;

            obj.SetImageSource(value);
        }

        private static void OnStretchChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (ImageBrushEx)d;
            var value = (Stretch)e.NewValue;

            if (Enum.IsDefined(typeof(Stretch), value))
            {
                throw new ArgumentOutOfRangeException(nameof(Stretch));
            }

            var brush = (CompositionSurfaceBrush)obj.CompositionBrush;
            brush.Stretch = (CompositionStretch)value;
        }

        private void DisposeCompositionBrush()
        {
            if (CompositionBrush != null)
            {
                CompositionBrush.Dispose();
                CompositionBrush = null;
            }
        }

        private async void SetImageSource(object imageSource)
        {
            var compositor = Window.Current.Compositor;

            if (imageSource == null)
            {
                DisposeCompositionBrush();
                return;
            }

            _loadSourceCancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _loadSourceCancellationTokenSource.Token;
            try
            {
                var context = new ImageSourceContext(imageSource, null);
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    DisposeCompositionBrush();
                    ImageFailed?.Invoke(this, new ImageFailedEventArgs(imageSource, ex));
                }
            }
        }
    }
}