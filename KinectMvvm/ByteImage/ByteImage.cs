﻿using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace KinectMvvm.ByteImage
{
    [TemplatePart(Name = "PART_image", Type = typeof(Image))]
    public class ByteImage : Control
    {
        private Image image;
        private WriteableBitmap imageOutput;

        public ByteImage()
        {
            DefaultStyleKey = typeof (ByteImage);
        }

        public static readonly DependencyProperty ByteSourceProperty 
            = DependencyProperty.Register("ByteSource", typeof (byte[]), typeof (ByteImage), new PropertyMetadata(new byte[0], SourceUpdated));

        public byte[] ByteSource
        {
            get
            {
                return (byte[]) GetValue(ByteSourceProperty);
            }
            set
            {
                SetValue(ByteSourceProperty, value);
            }
        }

        public static readonly DependencyProperty FrameWidthProperty
            = DependencyProperty.Register("FrameWidth", typeof(int), typeof(ByteImage), new PropertyMetadata(0));

        public int FrameWidth
        {
            get
            {
                return (int)GetValue(FrameWidthProperty);
            }

            set
            {
                SetValue(FrameWidthProperty, value);
            }
        }

        public static readonly DependencyProperty FrameHeightProperty
            = DependencyProperty.Register("FrameHeight", typeof(int), typeof(ByteImage), new PropertyMetadata(0));

        public int FrameHeight
        {
            get
            {
                return (int)GetValue(FrameHeightProperty);
            }

            set
            {
                SetValue(FrameHeightProperty, value);
            }
        }

        protected override void OnApplyTemplate()
        {
            this.image = (Image) GetTemplateChild("PART_image");
            imageOutput = new WriteableBitmap(FrameWidth, FrameHeight);
            this.image.Source = imageOutput;
        }

        private static async void SourceUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ByteImage instance = (ByteImage) d;

            if (e.OldValue == e.NewValue || instance.image == null)
            {
                return;
            }

            await instance.CreateSource();
        }

        private async Task CreateSource()
        {
            using (Stream stream = this.imageOutput.PixelBuffer.AsStream())
            {
                await stream.WriteAsync(ByteSource, 0, ByteSource.Length - 1);
                this.imageOutput.Invalidate();
            }
        }
    }
}
