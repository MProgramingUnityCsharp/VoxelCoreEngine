using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace NoiseForge.Core
{
    internal class Engine : GameWindow
    {
        private int VAOTriangle;
        private int VBOTriangle;
        private float[] vertices =
            {
            0,0.5f,
            -.5f,-0.5f,
            .5f,-0.5f,
            };
        private int shaderProgram;
        private int vertexShader;
        private int fragmentShader;

        public Engine(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = new OpenTK.Mathematics.Vector2i(width, height), Title = title })
        {




        }
        protected override void OnLoad()
        {
            VBOTriangle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOTriangle);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.StaticDraw);
            VAOTriangle = GL.GenVertexArray();
            GL.BindVertexArray(VAOTriangle);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);


            vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, File.ReadAllText("Shaders\\Default.vert"));
            GL.CompileShader(vertexShader);
            fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, File.ReadAllText("Shaders\\Default.frag"));
            GL.CompileShader(fragmentShader);

            shaderProgram = GL.CreateProgram();
            GL.AttachShader(shaderProgram, vertexShader);
            GL.AttachShader(shaderProgram, fragmentShader);
            GL.LinkProgram(shaderProgram);

        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(.3f, .3f, 0, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit);


            GL.BindVertexArray(VAOTriangle);
            GL.UseProgram(shaderProgram);
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);


            SwapBuffers();

        }
        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            GL.ClearColor(.3f, .3f, 0, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Viewport(0, 0, e.Width, e.Height);
        }



    }
}
