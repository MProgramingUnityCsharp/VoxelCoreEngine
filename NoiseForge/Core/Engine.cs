using Emgu.CV;
using Emgu.CV.Structure;
using NoiseForge.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace NoiseForge.Core
{
    internal class Engine : GameWindow
    {
        private int VAOTriangle;
        private Camera camera;
        private int VBOTriangle;
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
        // Back face
        // Bottom-left
        // top-right
        // bottom-right         
        // top-right
        // bottom-left
        // top-left
        // Front face
        // bottom-left
        // bottom-right
        // top-right
        // top-right
        // top-left
        // bottom-left
        // Left face
        // top-right
        // top-left
        // bottom-left
        // bottom-left
        // bottom-right
        // top-right
        // Right face
        // top-left
        // bottom-right
        // top-right         
        // bottom-right
        // top-left
        // bottom-left     
        // Bottom face
        // top-right
        // top-left
        // bottom-left
        // bottom-left
        // bottom-right
        // top-right
        // Top face
        // top-left
        // bottom-right
        // top-right     
        // bottom-right
        // top-left
        // bottom-left        



        private Vector3 position = new Vector3(0, 0, -5);
        private Vector3 modelPosition = new Vector3(1, 0, 1);
        private Vector3 target = Vector3.UnitZ;
        private Shader _modelShader;
        private Texture texture0;
        private bool canRotate;
        private Vector3[] cubePositions =
        {
 new Vector3(1, 0, 1),
 new Vector3(2, 0, 1),
 new Vector3(3, 0, 1),
 new Vector3(4, 0, 1),
 new Vector3(5, 0, 1),
 new Vector3(1, 0, 2),
 new Vector3(2, 0, 2),
 new Vector3(3, 0, 2),
 new Vector3(4, 0, 2),
 new Vector3(5, 0, 2),
 new Vector3(1, 0, 3),
 new Vector3(2, 0, 3),
 new Vector3(3, 0, 3),
 new Vector3(4, 0, 3),
 new Vector3(5, 0, 3),
 new Vector3(1, 0, 4),
 new Vector3(2, 0, 4),
 new Vector3(3, 0, 4),
 new Vector3(4, 0, 4),
 new Vector3(5, 0, 4),
 new Vector3(1, 0, 5),
 new Vector3(2, 0, 5),
 new Vector3(3, 0, 5),
 new Vector3(4, 0, 5),
 new Vector3(5, 0, 5),
        };
        private int ubObject;
        private int depth = 10;
        private int col = 50;
        private int row = 50;
        private Matrix4[] modelMatrices;
        private List<Matrix4> translationMatrixModelList = new List<Matrix4>();

        public Engine(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = new OpenTK.Mathematics.Vector2i(width, height), Title = title })
        {


        }
        protected override void OnLoad()
        {

            Image<Gray, byte> Image = CvInvoke.Imread("perlin_output.png").ToImage<Gray, byte>();
            int indexer = 0;
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(TriangleFace.Back);

            for (int i = 0; i < col; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    int column = Image.Height / this.col;
                    int row = Image.Width / this.row;
                    double depth = (int)Image[j * row, i * column].Intensity / 40;

                    for (int k = 0; k < this.depth; k++)
                    {
                        //if (depth + k < this.depth)
                        //{
                        //    continue;
                        //}
                        translationMatrixModelList.Add(Matrix4.CreateScale(1f) * Matrix4.CreateTranslation(Vector3.UnitX * i) * Matrix4.CreateTranslation(Vector3.UnitZ * j) * Matrix4.CreateTranslation(Vector3.UnitY * (float)(k + depth)));
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
                    }
                }
            }
            modelMatrices = translationMatrixModelList.ToArray();
            camera = new Camera();
            VBOTriangle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOTriangle);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.StaticDraw);
            VAOTriangle = GL.GenVertexArray();
            GL.BindVertexArray(VAOTriangle);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
            GL.EnableVertexAttribArray(2);


            _modelShader = new Shader("Shaders\\Default.vert", "Shaders\\Default.frag");
            texture0 = new Texture("container.jpg");
            _modelShader.UniformFloat("texture0", 0);

            int Indexx = GL.GetUniformBlockIndex(_modelShader.programLocation, "Matrices");
            GL.UniformBlockBinding(_modelShader.programLocation, Indexx, 0);

            ubObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.UniformBuffer, ubObject);
            GL.BufferData(BufferTarget.UniformBuffer, (4 * Vector4.SizeInBytes * 2) + Vector4.SizeInBytes, IntPtr.Zero, BufferUsageHint.StaticDraw);
            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, 0, ubObject);
            Matrix4 projection = camera.PerspectiveMatrix;
            GL.BindBuffer(BufferTarget.UniformBuffer, ubObject);
            GL.BufferSubData(BufferTarget.UniformBuffer, 0, Vector4.SizeInBytes * 4, ref projection);
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);
            Matrix4 viewMatrix = camera.GetViewMatrix();
            GL.BindBuffer(BufferTarget.UniformBuffer, ubObject);
            GL.BufferSubData(BufferTarget.UniformBuffer, Vector4.SizeInBytes * 4, Vector4.SizeInBytes * 4, ref viewMatrix);
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);
            Vector3 lightDir = Vector3.UnitY * 20;

            GL.BindBuffer(BufferTarget.UniformBuffer, ubObject);
            GL.BufferSubData(BufferTarget.UniformBuffer, 2 * Vector4.SizeInBytes * 4, Vector4.SizeInBytes, ref lightDir);
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);

            int bufferMatrices = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferMatrices);
            GL.BufferData(BufferTarget.ArrayBuffer, 4 * Vector4.SizeInBytes * modelMatrices.Length, modelMatrices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(3, 4, VertexAttribPointerType.Float, false, 4 * Vector4.SizeInBytes, 0);
            GL.EnableVertexAttribArray(3);
            GL.VertexAttribPointer(4, 4, VertexAttribPointerType.Float, false, 4 * Vector4.SizeInBytes, Vector4.SizeInBytes * 1);
            GL.EnableVertexAttribArray(4);
            GL.VertexAttribPointer(5, 4, VertexAttribPointerType.Float, false, 4 * Vector4.SizeInBytes, Vector4.SizeInBytes * 2);
            GL.EnableVertexAttribArray(5);
            GL.VertexAttribPointer(6, 4, VertexAttribPointerType.Float, false, 4 * Vector4.SizeInBytes, Vector4.SizeInBytes * 3);
            GL.EnableVertexAttribArray(6);


            GL.VertexAttribDivisor(3, 1);
            GL.VertexAttribDivisor(4, 1);
            GL.VertexAttribDivisor(5, 1);
            GL.VertexAttribDivisor(6, 1);

        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(.3f, .3f, 0, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


            GL.BindVertexArray(VAOTriangle);
            _modelShader.Use();
            texture0.UseTexture();

            _modelShader.UniformVector("lightDir", new Vector3(0, 20, 0));
            _modelShader.UniformVector("objectColor", new Vector3(.25f, .25f, .25f));
            _modelShader.UniformVector("lightColor", new Vector3(1f, 1f, 1));

            Matrix4 viewMatrix = camera.GetViewMatrix();
            GL.BindBuffer(BufferTarget.UniformBuffer, ubObject);
            GL.BufferSubData(BufferTarget.UniformBuffer, Vector4.SizeInBytes * 4, Vector4.SizeInBytes * 4, ref viewMatrix);
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);

            int indexer = 0;

            GL.DrawArraysInstanced(PrimitiveType.Triangles, 0, vertices.Length, modelMatrices.Length);
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);





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

        }
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            camera.lastPos = new Vector2(MousePosition.X, MousePosition.Y);
        }


    }
}
