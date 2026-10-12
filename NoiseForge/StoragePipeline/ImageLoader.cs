using Emgu.CV;
using Emgu.CV.Structure;

namespace NoiseForge.StoragePipeline
{
    internal class ImageLoader
    {
        public ImageLoader(string filePath, int width, int height, double scale, int seed = 123)
        {


            Image<Gray, byte> noiseImage = new Image<Gray, byte>(width, height);

            PerlinNoise perlin = new PerlinNoise(seed);



            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    double nx = x * scale;
                    double ny = y * scale;

                    double n = (perlin.Compute(nx, ny) + 1.0) * 0.5;

                    byte pixelValue = (byte)(n * 255);

                    noiseImage[y, x] = new Gray(pixelValue);
                }
            }

            noiseImage.Save(filePath);

        }
    }
}
