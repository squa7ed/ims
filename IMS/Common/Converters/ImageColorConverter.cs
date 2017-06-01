using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace IMS.Common.Converters
{
    public class ImageColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bitmap = (BitmapImage)value;
            if (bitmap == null)
            {
                return null;
            }
            var img = new WriteableBitmap(bitmap);
            int pixelWidth = img.PixelWidth;
            int pixelHeight = img.PixelHeight;
            int Stride = pixelWidth * 4;
            byte[] pixels = new byte[pixelHeight * Stride];
            img.CopyPixels(pixels, Stride, 0);
            byte TransparentByte = byte.Parse("0");
            byte colorByte = byte.Parse("255");
            int N = pixelWidth * pixelHeight;

            //Operate the pixels directly
            for (int i = 0; i < N; i++)
            {
                byte r = pixels[i * 4];
                byte g = pixels[i * 4 + 1];
                byte b = pixels[i * 4 + 2];
                byte a = pixels[i * 4 + 3];
                if (a != TransparentByte)
                {
                    pixels[i * 4] = colorByte;
                    pixels[i * 4 + 1] = colorByte;
                    pixels[i * 4 + 2] = colorByte;
                }
            }
            img.WritePixels(new System.Windows.Int32Rect(0, 0, pixelWidth, pixelHeight), pixels, Stride, 0);
            return img;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
