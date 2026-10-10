using NoiseForge.Core;

namespace NoiseForge
{
    internal class Program
    {
        static void Main(string[] args)
        {

            using (Engine window = new Engine(1000, 600, "Game Engine Renderer"))
            {
                window.Run();
            }
        }
    }
}
