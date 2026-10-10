using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace NoiseForge.Graphics
{
    internal class Camera
    {
        public Matrix4 projectionMatrix;
        private Matrix4 viewMatrix;
        private Vector3 position = new Vector3(0, 0, 5);
        private Vector3 front = -Vector3.UnitZ;
        private Vector3 up = Vector3.UnitY;
        private Vector3 right = Vector3.UnitX;
        private float Speed { get; set; } = 10;

        public Camera(float fieldOfView = 60, float aspect = 1, float near = .1f, float far = 1)
        {
            projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(fieldOfView), aspect, near, far);
            GetViewMatrix();
        }

        public Matrix4 GetViewMatrix() => viewMatrix = Matrix4.LookAt(position, position + front, up);
        public Matrix4 GetPerspective()
        {
            return projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(60), 1, .1f, 100);

        }
        public void Update(KeyboardState input, MouseState mouse, FrameEventArgs e)
        {
            if (input.IsKeyDown(Keys.W))
            {
                position += front * Speed * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.S))
            {
                position -= front * Speed * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.D))
            {
                position += right * Speed * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.A))
            {
                position -= right * Speed * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.E))
            {
                position += up * Speed * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.Q))
            {
                position -= up * Speed * (float)e.Time;
            }


        }
    }
}
