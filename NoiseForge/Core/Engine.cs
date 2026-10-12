using Emgu.CV;
using Emgu.CV.Structure;
using NoiseForge.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Diagnostics;

namespace NoiseForge.Core
{
    internal class Engine : GameWindow
    {

        private Camera camera;
        private Model<float> blockModel;


        float[] vertices = {
    // positions          // normals           // texture coords
    -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,    0.0f, 0.0f,
      0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,    1.0f, 1.0f,
     0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,   1.0f, 0.0f,
     0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,     1.0f, 1.0f,
      -0.5f, -0.5f, -0.5f,   0.0f,  0.0f, -1.0f,  0.0f, 0.0f,
      -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,     0.0f, 1.0f,

     -0.5f, -0.5f,  0.5f,  0.0f,  0.0f, 1.0f,     0.0f, 0.0f,
         0.5f, -0.5f,  0.5f,  0.0f,  0.0f, 1.0f,   1.0f, 0.0f,
      0.5f,  0.5f,  0.5f,   0.0f,  0.0f, 1.0f,   1.0f, 1.0f,
     0.5f,  0.5f,  0.5f,  0.0f,  0.0f, 1.0f,      1.0f, 1.0f,
     -0.5f,  0.5f,  0.5f,  0.0f,  0.0f, 1.0f,  0.0f, 1.0f,
      -0.5f, -0.5f,  0.5f,  0.0f,  0.0f, 1.0f,     0.0f, 0.0f,

     -0.5f,  0.5f,  0.5f,  -1.0f,  0.0f,  0.0f,    1.0f, 0.0f,
       -0.5f,  0.5f, -0.5f,   -1.0f,  0.0f,  0.0f, 1.0f, 1.0f,
     -0.5f, -0.5f, -0.5f,   -1.0f,  0.0f,  0.0f,   0.0f, 1.0f,
   -0.5f, -0.5f, -0.5f,   -1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
  -0.5f, -0.5f,  0.5f,    -1.0f,  0.0f,  0.0f,   0.0f, 0.0f,
  -0.5f,  0.5f,  0.5f,   -1.0f,  0.0f,  0.0f,    1.0f, 0.0f,

       0.5f,  0.5f,  0.5f,    1.0f,  0.0f,  0.0f,   1.0f, 0.0f,
   0.5f, -0.5f, -0.5f,    1.0f,  0.0f,  0.0f,     0.0f, 1.0f,
   0.5f,  0.5f, -0.5f,    1.0f,  0.0f,  0.0f,    1.0f, 1.0f,
   0.5f, -0.5f, -0.5f,    1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
       0.5f,  0.5f,  0.5f,    1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
   0.5f, -0.5f,  0.5f,    1.0f,  0.0f,  0.0f,   0.0f, 0.0f,

      -0.5f, -0.5f, -0.5f,   0.0f, -1.0f,  0.0f,  0.0f, 1.0f,
    0.5f, -0.5f, -0.5f,   0.0f, -1.0f,  0.0f,    1.0f, 1.0f,
     0.5f, -0.5f,  0.5f,   0.0f, -1.0f,  0.0f,  1.0f, 0.0f,
    0.5f, -0.5f,  0.5f,    0.0f, -1.0f,  0.0f,       1.0f, 0.0f,
     -0.5f, -0.5f,  0.5f,    0.0f, -1.0f,  0.0f,   0.0f, 0.0f,
    -0.5f, -0.5f, -0.5f,   0.0f, -1.0f,  0.0f,   0.0f, 1.0f,

    -0.5f,  0.5f, -0.5f,    0.0f,  1.0f,  0.0f, 0.0f, 1.0f,
      0.5f,  0.5f,  0.5f,    0.0f,  1.0f,  0.0f,  1.0f, 0.0f,
     0.5f,  0.5f, -0.5f,    0.0f,  1.0f,  0.0f,   1.0f, 1.0f,
     0.5f,  0.5f,  0.5f,   0.0f,  1.0f,  0.0f, 1.0f, 0.0f,
   -0.5f,  0.5f, -0.5f,    0.0f,  1.0f,  0.0f,  0.0f, 1.0f,
    -0.5f,  0.5f,  0.5f,   0.0f,  1.0f,  0.0f,    0.0f, 0.0f,
};


        private Shader _modelShader;
        private Shader _lightShader;
        private Shader _grassShader;
        private Texture texture0;
        private Texture texture1;
        private bool canRotate;

        private int depth = 10;
        private int col = 30;
        private int row = 30;
        private Matrix4[] modelMatrices;
        private List<Matrix4> translationMatrixModelList = new List<Matrix4>();
        private Vector3 lightDir;
        private Matrix4 lightTranslationMat4;

        private UniformBuffer _uniformBuffer;
        private readonly float TERRAIN_ROUGHNESS = 7;

        public Engine(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = new OpenTK.Mathematics.Vector2i(width, height), Title = title })
        {


        }
        protected override void OnLoad()
        {
            Random randomTreeHeightGenerator = new Random();
            Random randomTreeLeavesGenerator = new Random();
            Random randomTreeGenerator = new Random();
            lightDir = new Vector3(col / 2, 1.5f * 20, row / 2);


            Image<Gray, byte> Image = CvInvoke.Imread("perlin_output.png").ToImage<Gray, byte>();
            lightTranslationMat4 = Matrix4.CreateTranslation(lightDir);

            _modelShader = new Shader("Shaders\\Default.vert", "Shaders\\Default.frag");
            _lightShader = new Shader("Shaders\\LightShader.vert", "Shaders\\LightShader.frag");
            _grassShader = new Shader("Shaders\\Default.vert", "Shaders\\Default.frag");
            _modelShader.UniformFloat("texture0", 0);

            texture0 = new Texture("container.jpg");
            texture1 = new Texture("azalea_leaves.png");




            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(TriangleFace.Back);

            for (int i = 0; i < col; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    int column = Image.Height / this.col;
                    int row = Image.Width / this.row;
                    double depth = (int)(Image[j * row, i * column].Intensity / 255 * TERRAIN_ROUGHNESS);
                    Matrix4 createdTree = Matrix4.CreateScale(1);
                    for (int k = 0; k < this.depth; k++)
                    {
                        #region for TreeGeneration
                        /*
                                                 if (depth + k < this.depth - 1)
                                                {
                                                    int randomHeight = randomTreeHeightGenerator.Next(5, 10);
                                                    int TreeCreatingPermisson = randomTreeGenerator.Next(0, 255);
                                                    if (TreeCreatingPermisson <= 3)
                                                    {

                                                        for (int x = 0; x < randomHeight; x++)
                                                        {
                                                            createdTree = Matrix4.CreateScale(1f) * Matrix4.CreateTranslation(Vector3.UnitX * i) * Matrix4.CreateTranslation(Vector3.UnitZ * j) * Matrix4.CreateTranslation(Vector3.UnitY * (float)(k + x + depth));
                                                            translationMatrixModelList.Add(createdTree);
                                                        }
                                                        for (int y = 0; y < 3; y++)
                                                        {
                                                            for (int z = 0; z < 3; z++)
                                                            {

                                                                translationMatrixModelList.Add(Matrix4.CreateScale(1f) * Matrix4.CreateTranslation(-Vector3.UnitX) * Matrix4.CreateTranslation(-Vector3.UnitZ) * Matrix4.CreateTranslation(Vector3.UnitX * (i + z)) * Matrix4.CreateTranslation(Vector3.UnitZ * (j + y)) * Matrix4.CreateTranslation(Vector3.UnitY * (float)(k + randomHeight + depth)));
                                                            }
                                                        }

                                                        continue;
                                                    }
                                                }   
                          */
                        #endregion

                        translationMatrixModelList.Add(Matrix4.CreateScale(1f) * Matrix4.CreateTranslation(Vector3.UnitX * i) * Matrix4.CreateTranslation(Vector3.UnitZ * j) * Matrix4.CreateTranslation(Vector3.UnitY * (float)(k + depth)));
                        #region for optimization

                        //Matrix4[] matrix4 = translationMatrixModelList.Where(item => item == translationMatrixModelList[translationMatrixModelList.Count - 1]).ToArray();


                        //if (matrix4.Length > 1)
                        //{
                        //    for (int l = 0; l < matrix4.Length; l++)
                        //    {

                        //        if (l == 0)
                        //        {
                        //            continue;
                        //        }
                        //        translationMatrixModelList.Remove(matrix4[l]);
                        //    }
                        //}
                        #endregion

                    }
                }
            }
            modelMatrices = translationMatrixModelList.ToArray();
            camera = new Camera();
            blockModel = new Model<float>(vertices, 0, 3, "Vertices", 8 * sizeof(float));
            blockModel.SetAttrib(1, 3, sizeof(float) * 8, sizeof(float) * 3);
            blockModel.SetAttrib(2, 3, sizeof(float) * 8, sizeof(float) * 6);




            _modelShader.SetBindingPoint("Matrices", 0);
            _lightShader.SetBindingPoint("Matrices", 0);

            _uniformBuffer = new UniformBuffer((4 * Vector4.SizeInBytes * 2) + Vector4.SizeInBytes * 2, 0);

            Matrix4 projection = camera.PerspectiveMatrix;
            Matrix4 viewMatrix = camera.GetViewMatrix();
            _uniformBuffer.BindUniformBuffer();
            _uniformBuffer.SetMatrixBuffer(projection, 0);
            _uniformBuffer.SetMatrixBuffer(viewMatrix, Vector4.SizeInBytes * 4);

            blockModel.BindVAO();
            blockModel.SetMatrixBuffer(modelMatrices, 4, 3, 4, 1, "ModelMatrices", 4 * Vector4.SizeInBytes, Vector4.SizeInBytes);


        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(.3f, .3f, 0, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Vector3 cameraPosition = camera.Position;
            Matrix4 viewMatrix = camera.GetViewMatrix();


            blockModel.BindVAO();
            _modelShader.Use();
            texture0.UseTexture2D();

            _modelShader.UniformVector("objectColor", new Vector3(1f));
            _modelShader.UniformVector("lightColor", new Vector3(1.2f));

            _uniformBuffer.SetMatrixBuffer(viewMatrix, Vector4.SizeInBytes * 4);
            _uniformBuffer.SetVector3Buffer(lightDir, Vector4.SizeInBytes * 8);
            _uniformBuffer.SetVector3Buffer(cameraPosition, Vector4.SizeInBytes * 9);



            GL.DrawArraysInstanced(PrimitiveType.Triangles, 0, vertices.Length, modelMatrices.Length);

            lightTranslationMat4 = Matrix4.CreateTranslation(lightDir);

            _lightShader.Use();
            _lightShader.UniformMatrix4("model", lightTranslationMat4);
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);


            Debug.WriteLine(lightDir);
            SwapBuffers();

        }
        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            GL.ClearColor(.3f, .3f, 0, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Viewport(0, 0, e.Width, e.Height);
        }
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            var keyboardInput = KeyboardState;
            var mouseInput = MouseState;

            camera.Update(keyboardInput, mouseInput, args);

            if (keyboardInput.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Up))
            {
                lightDir += Vector3.UnitZ * .01f;
            }
            if (keyboardInput.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Down))
            {
                lightDir -= Vector3.UnitZ * .01f;
            }
            if (keyboardInput.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Left))
            {
                lightDir += Vector3.UnitX * .01f;
            }
            if (keyboardInput.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Right))
            {
                lightDir -= Vector3.UnitX * .01f;
            }


        }
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            camera.lastPos = new Vector2(MousePosition.X, MousePosition.Y);
        }


    }
}
