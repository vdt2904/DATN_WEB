using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;
using ZXing.QrCode;
using ZXing;

namespace DATNWEB.Models
{
    public class QRCodeGenerator
    {
        public static Bitmap GenerateQRCode(string data, int width, int height)
        {
            var qrWriter = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = height,
                    Width = width
                }
            };

            var pixelData = qrWriter.Write(data);

            using (var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb))
            {
                var bitmapData = bitmap.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height),
                                                  ImageLockMode.WriteOnly,
                                                  PixelFormat.Format32bppRgb);

                try
                {
                    Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                    return new Bitmap(bitmap);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }
            }
        }
    }
}
