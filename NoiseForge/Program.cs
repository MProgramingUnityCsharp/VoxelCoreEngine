using NoiseForge.Core;
using NoiseForge.StoragePipeline;

namespace NoiseForge
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random seedRandomizer = new Random();
            int seedInteger = seedRandomizer.Next(0, 255);
            Console.WriteLine("do you wanna generate new terrain?? : [Yy]es | [Nn]o");
            string terrainGeneratingAnwer = Console.ReadLine();
            if (terrainGeneratingAnwer.ToLower() == "yes")
            {
                ImageLoader imageInstance = new ImageLoader("Textures\\perlin_output.png", 1024, 1024, 0.01, seedInteger);
            }
            using (Engine window = new Engine(1000, 600, "Game Engine Renderer"))
            {
                window.Run();
            }
        }
    }
}
