using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace NoiseForge.Graphics
{
    internal class Texture
    {
        public int imageLocation;

        public Texture(string filePath, TextureUnit unit = TextureUnit.Texture0)
        {

            imageLocation = GL.GenTexture();
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, imageLocation);
            StbImage.stbi_set_flip_vertically_on_load(1);

            // Load the image.
            ImageResult image = ImageResult.FromStream(File.OpenRead(filePath), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);


            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

        }
        public void UseTexture(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, imageLocation);
        }
    }
}
