
using Emgu.CV;
using Emgu.CV.Structure;
using NoiseForge.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Diagnostics;
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
        private Shader _lightShader;
        private Shader _grassShader;
        private Shader _treeBodyShader;
        private Texture _texture0;
        private Texture _texture1;
        private Texture _texture2;

        private int _depth = 10;
        private int _col = 30;
        private int _row = 30;

        private List<Matrix4> _blockTranslationList = new List<Matrix4>();
        private List<Matrix4> _blockLeavesList = new List<Matrix4>();
        private List<Matrix4> _blockTreeBodiesList = new List<Matrix4>();

        private Matrix4[] _blockTranslations;
        private Matrix4 _lightTranslation;

        private Vector3 _lightDir;

        private UniformBuffer _uniformBuffer;
        private Model<float> _blockTreeBodiesModel;
        private readonly float TERRAIN_ROUGHNESS = 7;

        public Engine(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = new OpenTK.Mathematics.Vector2i(width, height), Title = title })
        {
            _lightDir = new Vector3(_col / 2, 1.5f * 20, _row / 2);


            _lightTranslation = Matrix4.CreateTranslation(_lightDir);

            _modelShader = new Shader("Shaders\\Default.vert", "Shaders\\Default.frag");
            _lightShader = new Shader("Shaders\\LightShader.vert", "Shaders\\LightShader.frag");
            _grassShader = new Shader("Shaders\\Default.vert", "Shaders\\Default.frag");
            _treeBodyShader = new Shader("Shaders\\Default.vert", "Shaders\\Default.frag");

            _texture0 = new Texture("container.jpg");
            _texture1 = new Texture("azalea_leaves.png");
            _texture2 = new Texture("TreeBody.png");

        }
        protected override void OnLoad()
        {
            Random randomTreeHeightGenerator = new Random();
            Random randomTreeLeavesGenerator = new Random();
            Random randomTreeGenerator = new Random();

            Image<Gray, byte> Image = CvInvoke.Imread("perlin_output.png").ToImage<Gray, byte>();

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(TriangleFace.Back);

            for (int i = 0; i < _col; i++)
            {
                for (int j = 0; j < _row; j++)
                {
                    int column = Image.Height / this._col;
                    int row = Image.Width / this._row;
                    double depth = (int)(Image[j * row, i * column].Intensity / 255 * TERRAIN_ROUGHNESS);
                    Matrix4 createdTree = Matrix4.CreateScale(1);
                    for (int k = 0; k < this._depth; k++)
                    {

                        if (depth + k > this._depth)
                        {
                            int randomHeight = randomTreeHeightGenerator.Next(3, (int)Math.Clamp((TERRAIN_ROUGHNESS * _depth + TERRAIN_ROUGHNESS) / _depth, 5, 12));
                            int TreeCreatingPermisson = randomTreeGenerator.Next(0, 255);
                            if (TreeCreatingPermisson <= 5)
                            {
                                for (int x = 0; x < randomHeight; x++)
                                {
                                    createdTree = Matrix4.CreateScale(1f) * Matrix4.CreateTranslation(Vector3.UnitX * i) * Matrix4.CreateTranslation(Vector3.UnitZ * j) * Matrix4.CreateTranslation(Vector3.UnitY * (float)((k + depth) + x));
                                    _blockTreeBodiesList.Add(createdTree);
                                }
                                for (int y = 0; y < 3; y++)
                                {
                                    for (int z = 0; z < 3; z++)
                                    {

                                        _blockLeavesList.Add(Matrix4.CreateScale(1f) * Matrix4.CreateTranslation(-Vector3.UnitX) * Matrix4.CreateTranslation(-Vector3.UnitZ) * Matrix4.CreateTranslation(Vector3.UnitX * (i + z)) * Matrix4.CreateTranslation(Vector3.UnitZ * (j + y)) * Matrix4.CreateTranslation(Vector3.UnitY * (float)(randomHeight + (k + depth))));
                                    }
                                }

                                continue;
                            }
                        }


                        _blockTranslationList.Add(Matrix4.CreateScale(1f) * Matrix4.CreateTranslation(Vector3.UnitX * i) * Matrix4.CreateTranslation(Vector3.UnitZ * j) * Matrix4.CreateTranslation(Vector3.UnitY * (float)(k + depth)));
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




            _modelShader.SetBindingPoint("Matrices", 0);
            _lightShader.SetBindingPoint("Matrices", 0);
            _grassShader.SetBindingPoint("Matrices", 0);
            _treeBodyShader.SetBindingPoint("Matrices", 0);
            _uniformBuffer = new UniformBuffer((4 * Vector4.SizeInBytes * 2) + Vector4.SizeInBytes * 2, 0);

            Matrix4 projection = _camera.PerspectiveMatrix;
            Matrix4 viewMatrix = _camera.GetViewMatrix();
            _uniformBuffer.BindUniformBuffer();
            _uniformBuffer.SetMatrixBuffer(projection, 0);
            _uniformBuffer.SetMatrixBuffer(viewMatrix, Vector4.SizeInBytes * 4);

            _blockTerrainModel.BindVAO();
            _blockTerrainModel.SetMatrixBuffer(_blockTranslations, 4, 3, 4, 1, "ModelMatrices", 4 * Vector4.SizeInBytes, Vector4.SizeInBytes);
            _blockLeavesModel.BindVAO();
            _blockLeavesModel.SetMatrixBuffer(_blockLeavesList.ToArray(), 4, 3, 4, 1, "ModelMatrices", 4 * Vector4.SizeInBytes, Vector4.SizeInBytes);
            _blockTreeBodiesModel.BindVAO();
            _blockTreeBodiesModel.SetMatrixBuffer(_blockTreeBodiesList.ToArray(), 4, 3, 4, 1, "ModelMatrices", 4 * Vector4.SizeInBytes, Vector4.SizeInBytes);

        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(.3f, .3f, 0, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Vector3 cameraPosition = _camera.Position;
            Matrix4 viewMatrix = _camera.GetViewMatrix();


            _blockTerrainModel.BindVAO();
            _modelShader.Use();
            _texture0.UseTexture2D();

            _modelShader.UniformVector("objectColor", new Vector3(1f));
            _modelShader.UniformVector("lightColor", new Vector3(1.2f));

            _uniformBuffer.SetMatrixBuffer(viewMatrix, Vector4.SizeInBytes * 4);
            _uniformBuffer.SetVector3Buffer(_lightDir, Vector4.SizeInBytes * 8);
            _uniformBuffer.SetVector3Buffer(cameraPosition, Vector4.SizeInBytes * 9);



            GL.DrawArraysInstanced(PrimitiveType.Triangles, 0, _vertices.Length, _blockTranslations.Length);
            _blockLeavesModel.BindVAO();
            _grassShader.Use();
            _texture1.UseTexture2D();
            _grassShader.UniformVector("objectColor", new Vector3(1f));
            _grassShader.UniformVector("lightColor", new Vector3(1.2f));

            GL.DrawArraysInstanced(PrimitiveType.Triangles, 0, _vertices.Length, _blockLeavesList.Count);
            _blockTreeBodiesModel.BindVAO();
            _treeBodyShader.Use();
            _treeBodyShader.UniformVector("objectColor", new Vector3(1f));
            _treeBodyShader.UniformVector("lightColor", new Vector3(1.2f));
            _texture2.UseTexture2D();
            GL.DrawArraysInstanced(PrimitiveType.Triangles, 0, _vertices.Length, _blockTreeBodiesList.Count);
            _lightTranslation = Matrix4.CreateTranslation(_lightDir);

            _lightShader.Use();
            _lightShader.UniformMatrix4("model", _lightTranslation);
            GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Length);


            Debug.WriteLine(_lightDir);
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
