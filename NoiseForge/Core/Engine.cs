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
        private CameraNew.Camera camera;
        private int VBOTriangle;
        float[] vertices = {
    -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
     0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
    -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,

    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
     0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
    -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,

    -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
    -0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
    -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
     0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
     0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
     0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
     0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
     0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
     0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,

    -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
    -0.5f,  0.5f,  0.5f,  0.0f, 0.0f,
    -0.5f,  0.5f, -0.5f,  0.0f, 1.0f
};

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

        public Engine(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = new OpenTK.Mathematics.Vector2i(width, height), Title = title })
        {




        }
        protected override void OnLoad()
        {
            GL.Enable(EnableCap.DepthTest);

            camera = new CameraNew.Camera();
            VBOTriangle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOTriangle);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.StaticDraw);
            VAOTriangle = GL.GenVertexArray();
            GL.BindVertexArray(VAOTriangle);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);


            _modelShader = new Shader("Shaders\\Default.vert", "Shaders\\Default.frag");
            texture0 = new Texture("container.jpg");
            _modelShader.UniformFloat("texture0", 0);

            int Indexx = GL.GetUniformBlockIndex(_modelShader.programLocation, "Matrices");
            GL.UniformBlockBinding(_modelShader.programLocation, Indexx, 0);

            ubObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.UniformBuffer, ubObject);
            GL.BufferData(BufferTarget.UniformBuffer, sizeof(float) * 2 * 16, IntPtr.Zero, BufferUsageHint.StaticDraw);
            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, 0, ubObject);
            Matrix4 perpectiveMatrix = camera.PerspectiveMatrix;
            GL.BufferSubData(BufferTarget.UniformBuffer, 0, Vector4.SizeInBytes * 4, ref perpectiveMatrix);
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);



        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(.3f, .3f, 0, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


            GL.BindVertexArray(VAOTriangle);
            _modelShader.Use();
            texture0.UseTexture();
            _modelShader.UniformMatrix4("view", camera.GetViewMatrix());
            GL.BindBuffer(BufferTarget.UniformBuffer, ubObject);
            Matrix4 viewMatrix = camera.GetViewMatrix();
            GL.BufferSubData(BufferTarget.UniformBuffer, Vector4.SizeInBytes * 4, Vector4.SizeInBytes * 4, ref viewMatrix);
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);

            for (int i = 0; i < cubePositions.Length; i++)
            {
                for (int j = 0; j < cubePositions.Length; j++)
                {
                    _modelShader.UniformMatrix4("model", Matrix4.CreateScale(1f) * Matrix4.CreateTranslation(cubePositions[i]) * Matrix4.CreateTranslation(cubePositions[j]));
                    GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);

                }
            }


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
