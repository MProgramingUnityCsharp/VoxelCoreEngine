
using Emgu.CV;
using Emgu.CV.Structure;
using NoiseForge.Graphics;
using NoiseForge.StoragePipeline;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using static System.Net.Mime.MediaTypeNames;

namespace NoiseForge.Core
{
    internal class Engine : GameWindow
    {
        float[] _vertices = {
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


        private Camera _camera;
        private Model<float> _blockTerrainModel;
        private Model<float> _blockLeavesModel;
        private Shader _modelShader;
        private Shader _groundlShader;
        private Shader _lightShader;
        private Shader _grassShader;
        private Shader _treeBodyShader;
        private Texture _terrainTexture;
        private Texture _groundTexture;
        private Texture _leavesTexture;
        private Texture _treeBodyTexture;
        private int _col = 75;
        private int _row = 75;

        private List<Matrix4> _blockTranslationList = new List<Matrix4>();
        private List<Matrix4> _blockLeavesList = new List<Matrix4>();
        private List<Matrix4> _blockTreeBodiesList = new List<Matrix4>();
        private List<Matrix4> _groundTranslationList = new List<Matrix4>();

        private Matrix4[] _blockTranslations;
        private Matrix4 _lightTranslation;

        private Vector3 _lightDir;
        private UniformBuffer _lightBuffer;
        private UniformBuffer _projectionBuffer;
        private Model<float> _blockTreeBodiesModel;
        private readonly float TERRAIN_ROUGHNESS = 13;
        private string[] cubeFaces =
            {
    "Textures\\right.jpg",
     "Textures\\left.jpg",
    "Textures\\top.jpg",
    "Textures\\bottom.jpg",
    "Textures\\front.jpg",
    "Textures\\back.jpg"
};
        private Model<float> _skyBoxBlock;
        private Model<float> _groundTerrainModel;

        public Engine(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = new OpenTK.Mathematics.Vector2i(width, height), Title = title })
        {
            _lightDir = new Vector3(_col / 2, 1.5f * 20, _row / 2);


            _lightTranslation = Matrix4.CreateTranslation(_lightDir);

            _modelShader = new Shader("Shaders\\DefaultProgram.vert", "Shaders\\DefaultProgram.frag");
            _groundlShader = new Shader("Shaders\\TerrainProgram.vert", "Shaders\\TerrainProgram.frag");
            _lightShader = new Shader("Shaders\\LightProgram.vert", "Shaders\\LightProgram.frag");
            _grassShader = new Shader("Shaders\\DefaultProgram.vert", "Shaders\\DefaultProgram.frag");
            _treeBodyShader = new Shader("Shaders\\DefaultProgram.vert", "Shaders\\DefaultProgram.frag");

            _terrainTexture = new Texture(cubeFaces);
            _groundTexture = new Texture("Textures\\bottom.jpg");
            _leavesTexture = new Texture("Textures\\azalea_leaves.png");
            _treeBodyTexture = new Texture("Textures\\TreeBody.png");
        }
        protected override void OnLoad()
        {
            if (!File.Exists("Textures\\perlin_output.png"))
            {
                ImageLoader imageInstance = new ImageLoader("Textures\\perlin_output.png", 1024, 1024, .01f);

            }

            Random randomTreeHeightGenerator = new Random();
            Random randomTreeLeavesGenerator = new Random();
            Random randomTreeGenerator = new Random();

            Image<Gray, byte> Image = CvInvoke.Imread("Textures\\perlin_output.png").ToImage<Gray, byte>();

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(TriangleFace.Back);

            for (int i = 0; i < _col; i++)
            {
                for (int j = 0; j < _row; j++)
                {
                    int column = Image.Height / this._col;
                    int row = Image.Width / this._row;

                    int surfaceHeight = (int)((Image[j * row, i * column].Intensity / 255.0) * TERRAIN_ROUGHNESS);

                    for (int k = 0; k <= surfaceHeight - 1; k++)
                    {
                        Matrix4 terrainMatrix = Matrix4.CreateScale(1f) *
                                                Matrix4.CreateTranslation(new Vector3(i, k, j));
                        _blockTranslationList.Add(terrainMatrix);
                    }

                    Matrix4 groundMatrix = Matrix4.CreateScale(1f) *
                                           Matrix4.CreateTranslation(new Vector3(i, surfaceHeight, j));
                    _groundTranslationList.Add(groundMatrix);

                    bool canSpawnTree = (i % 7 == 0 && j % 7 == 0);
                    int treeCreatingProbability = randomTreeGenerator.Next(0, 10);

                    if (canSpawnTree && treeCreatingProbability <= 4)
                    {
                        int minHeight = 3;
                        int maxHeight = Math.Clamp(4 + (int)(TERRAIN_ROUGHNESS * 0.1f), 4, 9);

                        if (maxHeight <= minHeight) maxHeight = minHeight + 1;

                        int randomHeight = randomTreeHeightGenerator.Next(minHeight, maxHeight);


                        for (int x = 0; x < randomHeight; x++)
                        {
                            int trunkY = surfaceHeight + 1 + x;

                            Matrix4 trunkMatrix = Matrix4.CreateScale(1f) *
                                                  Matrix4.CreateTranslation(new Vector3(i, trunkY, j));
                            _blockTreeBodiesList.Add(trunkMatrix);
                        }

                        int leavesStartY = surfaceHeight + 1 + randomHeight;

                        for (int ly = 0; ly < randomTreeLeavesGenerator.Next(1, 4); ly++)
                        {
                            for (int lx = -1; lx <= 1; lx++)
                            {
                                for (int lz = -1; lz <= 1; lz++)
                                {
                                    Matrix4 leafMatrix = Matrix4.CreateScale(1f) *
                                                         Matrix4.CreateTranslation(new Vector3(i + lx, leavesStartY + ly, j + lz));
                                    _blockLeavesList.Add(leafMatrix);
                                }
                            }
                        }
                    }
                }
            }
            _blockTranslations = _blockTranslationList.ToArray();
            _camera = new Camera();
            _blockTerrainModel = new Model<float>(_vertices, 0, 3, "Vertices", 8 * sizeof(float));
            _blockTerrainModel.SetAttrib(1, 3, sizeof(float) * 8, sizeof(float) * 3);
            _blockTerrainModel.SetAttrib(2, 3, sizeof(float) * 8, sizeof(float) * 6);
            _blockLeavesModel = new Model<float>(_vertices, 0, 3, "Vertices", 8 * sizeof(float));
            _blockLeavesModel.SetAttrib(1, 3, sizeof(float) * 8, sizeof(float) * 3);
            _blockLeavesModel.SetAttrib(2, 3, sizeof(float) * 8, sizeof(float) * 6);

            _blockTreeBodiesModel = new Model<float>(_vertices, 0, 3, "Vertices", 8 * sizeof(float));
            _blockTreeBodiesModel.SetAttrib(1, 3, sizeof(float) * 8, sizeof(float) * 3);
            _blockTreeBodiesModel.SetAttrib(2, 3, sizeof(float) * 8, sizeof(float) * 6);
            _groundTerrainModel = new Model<float>(_vertices, 0, 3, "Vertices", 8 * sizeof(float));
            _groundTerrainModel.SetAttrib(1, 3, sizeof(float) * 8, sizeof(float) * 3);
            _groundTerrainModel.SetAttrib(2, 3, sizeof(float) * 8, sizeof(float) * 6);




            _modelShader.SetBindingPoint("Matrices", 0);
            _groundlShader.SetBindingPoint("Matrices", 0);
            _lightShader.SetBindingPoint("Matrices", 0);
            _grassShader.SetBindingPoint("Matrices", 0);
            _treeBodyShader.SetBindingPoint("Matrices", 0);

            _modelShader.SetBindingPoint("Lights", 1);
            _grassShader.SetBindingPoint("Lights", 1);
            _groundlShader.SetBindingPoint("Lights", 1);
            _treeBodyShader.SetBindingPoint("Lights", 1);
            _groundlShader.SetBindingPoint("Lights", 1);
            _projectionBuffer = new UniformBuffer((4 * Vector4.SizeInBytes * 2) + Vector4.SizeInBytes * 2, 0);
            _lightBuffer = new UniformBuffer(Vector4.SizeInBytes, 1);
            _lightBuffer.BindUniformBuffer();
            _lightBuffer.SetVector3Buffer(new Vector3(2f), 0);

            Matrix4 projection = _camera.PerspectiveMatrix;
            Matrix4 viewMatrix = _camera.GetViewMatrix();
            _projectionBuffer.BindUniformBuffer();
            _projectionBuffer.SetMatrixBuffer(projection, 0);
            _projectionBuffer.SetMatrixBuffer(viewMatrix, Vector4.SizeInBytes * 4);

            _blockTerrainModel.BindVAO();
            _blockTerrainModel.SetMatrixBuffer(_blockTranslations, 4, 3, 4, 1, "ModelMatrices", 4 * Vector4.SizeInBytes, Vector4.SizeInBytes);
            _blockLeavesModel.BindVAO();
            _blockLeavesModel.SetMatrixBuffer(_blockLeavesList.ToArray(), 4, 3, 4, 1, "ModelMatrices", 4 * Vector4.SizeInBytes, Vector4.SizeInBytes);
            _blockTreeBodiesModel.BindVAO();
            _blockTreeBodiesModel.SetMatrixBuffer(_blockTreeBodiesList.ToArray(), 4, 3, 4, 1, "ModelMatrices", 4 * Vector4.SizeInBytes, Vector4.SizeInBytes);
            _groundTerrainModel.BindVAO();
            _groundTerrainModel.SetMatrixBuffer(_groundTranslationList.ToArray(), 4, 3, 4, 1, "ModelMatrices", 4 * Vector4.SizeInBytes, Vector4.SizeInBytes);

        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(.78f, .86f, 1.0f, 0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Vector3 cameraPosition = _camera.Position;
            Matrix4 viewMatrix = _camera.GetViewMatrix();

            _blockTerrainModel.BindVAO();
            _modelShader.Use();
            _groundTexture.UseTexture2D();
            _projectionBuffer.SetMatrixBuffer(viewMatrix, Vector4.SizeInBytes * 4);
            _projectionBuffer.SetVector3Buffer(_lightDir, Vector4.SizeInBytes * 8);
            _projectionBuffer.SetVector3Buffer(cameraPosition, Vector4.SizeInBytes * 9);



            GL.DrawArraysInstanced(PrimitiveType.Triangles, 0, _vertices.Length, _blockTranslations.Length);
            _groundTerrainModel.BindVAO();
            _terrainTexture.UseCubemap();
            _groundlShader.Use();
            GL.DrawArraysInstanced(PrimitiveType.Triangles, 0, _vertices.Length, _groundTranslationList.Count);


            _blockLeavesModel.BindVAO();
            _grassShader.Use();
            _leavesTexture.UseTexture2D();

            GL.DrawArraysInstanced(PrimitiveType.Triangles, 0, _vertices.Length, _blockLeavesList.Count);
            _blockTreeBodiesModel.BindVAO();
            _treeBodyShader.Use();

            _treeBodyTexture.UseTexture2D();
            GL.DrawArraysInstanced(PrimitiveType.Triangles, 0, _vertices.Length, _blockTreeBodiesList.Count);
            _lightTranslation = Matrix4.CreateTranslation(_lightDir);

            _lightShader.Use();

            _lightShader.UniformMatrix4("model", _lightTranslation);
            GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Length);


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

            _camera.Update(keyboardInput, mouseInput, args);

            if (keyboardInput.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Up))
            {
                _lightDir += Vector3.UnitZ * .01f;
            }
            if (keyboardInput.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Down))
            {
                _lightDir -= Vector3.UnitZ * .01f;
            }
            if (keyboardInput.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Left))
            {
                _lightDir += Vector3.UnitX * .01f;
            }
            if (keyboardInput.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Right))
            {
                _lightDir -= Vector3.UnitX * .01f;
            }


        }
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            _camera.lastPos = new Vector2(MousePosition.X, MousePosition.Y);
        }


    }
}
