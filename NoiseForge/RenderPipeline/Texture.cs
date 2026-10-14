using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace VoxelCoreEngine.RenderPipeline
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
        public Texture(string[] filePath, TextureUnit unit = TextureUnit.Texture0)
        {

            imageLocation = GL.GenTexture();
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.TextureCubeMap, imageLocation);
            for (int i = 0; i < filePath.Length; i++)
            {


                StbImage.stbi_set_flip_vertically_on_load(0);
                using (FileStream imageStream = File.OpenRead(filePath[i]))
                {
                    ImageResult textureLoadResult = ImageResult.FromStream(imageStream, ColorComponents.RedGreenBlueAlpha);
                    GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgba, textureLoadResult.Width, textureLoadResult.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, textureLoadResult.Data);
                }
            }
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);

        }
        public void UseTexture2D(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, imageLocation);
        }
        public void UseCubemap(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.TextureCubeMap, imageLocation);
        }
    }
}
