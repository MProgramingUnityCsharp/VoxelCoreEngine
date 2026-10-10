public class PerlinNoise
{
    private readonly int[] p = new int[512];

    public PerlinNoise(int seed = 0)
    {
        Random rand = new Random(seed);
        int[] permutation = new int[256];
        for (int i = 0; i < 256; i++) permutation[i] = i;

        // به‌هم‌ریختگی جدول با توجه به Seed
        for (int i = 0; i < 256; i++)
        {
            int j = rand.Next(256);
            int temp = permutation[i];
            permutation[i] = permutation[j];
            permutation[j] = temp;
        }

        for (int i = 0; i < 256; i++)
        {
            p[i] = permutation[i];
            p[256 + i] = permutation[i];
        }
    }

    private double Fade(double t) => t * t * t * (t * (t * 6 - 15) + 10);
    private double Lerp(double t, double a, double b) => a + t * (b - a);

    private double Grad(int hash, double x, double y)
    {
        int h = hash & 7;
        double u = h < 4 ? x : y;
        double v = h < 4 ? y : x;
        return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
    }

    public double Compute(double x, double y)
    {
        int X = (int)Math.Floor(x) & 255;
        int Y = (int)Math.Floor(y) & 255;

        x -= Math.Floor(x);
        y -= Math.Floor(y);

        double u = Fade(x);
        double v = Fade(y);

        int A = p[X] + Y;
        int B = p[X + 1] + Y;

        return Lerp(v, Lerp(u, Grad(p[A], x, y), Grad(p[B], x - 1, y)),
                       Lerp(u, Grad(p[A + 1], x, y - 1), Grad(p[B + 1], x - 1, y - 1)));
    }
}