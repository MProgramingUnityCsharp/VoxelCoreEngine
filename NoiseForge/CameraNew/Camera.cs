using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace NoiseForge.CameraNew
{
    internal class Camera
    {
        public Matrix4 PerspectiveMatrix { get; private set; }
        private Matrix4 viewMatrix;
        private Vector3 Position { get; set; } = new Vector3(0, 0, 5);

        private Vector3 front = -Vector3.UnitZ;
        private Vector3 up = Vector3.UnitY;
        private Vector3 right = Vector3.UnitX;
        public Vector2 lastPos;
        private bool firstMove = true;
        private float Sensitivity = 300;
        private float pitch;
        private float yaw = -90;
        private float Pitch
        {
            get
            {

                return pitch;
            }
            set
            {
                pitch = (float)Math.Clamp(value, -89, 89);
            }
        }
        private float Speed = 7.5f;

        public Camera(float fieldOfView = 60f, float aspect = 1.77f, float near = 0.1f, float far = 100)
        {
            GetPerspectiveMatrix(fieldOfView, aspect, near, far);
            GetViewMatrix();
        }

        private void GetPerspectiveMatrix(float fieldOfView, float aspect, float near, float far)
        {
            PerspectiveMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(fieldOfView), aspect, near, far);
        }

        public Matrix4 GetViewMatrix() => viewMatrix = Matrix4.LookAt(Position, Position + front, up);

        public void Update(KeyboardState input, MouseState mouse, FrameEventArgs e)
        {

            InputController(input, mouse, e);

        }

        private void InputController(KeyboardState input, MouseState mouse, FrameEventArgs e)
        {
            if (input.IsKeyDown(Keys.W))
            {
                Position += front * Speed * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.S))
            {
                Position -= front * Speed * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.D))
            {
                Position += right * Speed * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.A))
            {
                Position -= right * Speed * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.E))
            {
                Position += up * Speed * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.Q))
            {
                Position -= up * Speed * (float)e.Time;
            }
            if (mouse.IsButtonDown(MouseButton.Left))
            {
                if (firstMove)
                {
                    lastPos = new Vector2(mouse.X, mouse.Y);
                    firstMove = false;
                }
                else
                {
                    float deltaX = mouse.X - lastPos.X;
                    float deltaY = mouse.Y - lastPos.Y;
                    lastPos = new Vector2(mouse.X, mouse.Y);

                    yaw += deltaX * Sensitivity * (float)e.Time;
                    Pitch -= deltaY * Sensitivity * (float)e.Time;

                }

            }
            UpdateVectors();
        }

        private void UpdateVectors()
        {
            front.X += (float)(Math.Cos(MathHelper.DegreesToRadians(pitch)) * Math.Cos(MathHelper.DegreesToRadians(yaw)));
            front.Y += (float)Math.Sin(MathHelper.DegreesToRadians(pitch));
            front.Z += (float)(Math.Cos(MathHelper.DegreesToRadians(pitch)) * Math.Sin(MathHelper.DegreesToRadians(yaw)));
            front.Normalize();

            right = Vector3.Cross(front, Vector3.UnitY);
            right.Normalize();
            up = Vector3.Cross(right, front);
            up.Normalize();
            GetViewMatrix();

        }


    }
}
