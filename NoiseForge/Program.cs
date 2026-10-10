using NoiseForge.Core;

namespace NoiseForge
{
    internal class Program
    {
        static void Main(string[] args)
        {

            using (Engine window = new Engine(800, 600, "Game Engine Renderer"))
            {
                window.Run();
            }
        }
    }
}
