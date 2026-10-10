using Emgu.CV;
using Emgu.CV.Structure;

namespace NoiseForge.Graphics
{
    internal class ImageLoader
    {
        public ImageLoader(int width, int height, double scale)
        {

            // ایجاد یک تصویر خاکستری در Emgu CV
            Image<Gray, byte> noiseImage = new Image<Gray, byte>(width, height);

            // ساخت شیء پرلین نویز با یک Seed دلخواه
            PerlinNoise perlin = new PerlinNoise(123);

            // مقیاس (Frequency) برای کنترل زبری یا نرمی نویز


            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    double nx = x * scale;
                    double ny = y * scale;

                    // تابع پرلین مقداری بین 1- تا 1 برمی‌گرداند؛ آن را به بازه 0 تا 1 نگاشت می‌کنیم
                    double n = (perlin.Compute(nx, ny) + 1.0) * 0.5;

                    // تبدیل مقدار به بازه بایت (0 تا 255)
                    byte pixelValue = (byte)(n * 255);

                    noiseImage[y, x] = new Gray(pixelValue);
                }
            }

            // نمایش تصویر با استفاده از پنجره Emgu CV
            CvInvoke.Imshow("Emgu CV - Perlin Noise", noiseImage);
            CvInvoke.WaitKey(0);
            CvInvoke.DestroyAllWindows();
            noiseImage.Save("perlin_output.png");

            // ذخیره تصویر خروجی (اختیاری)
        }
    }
}
