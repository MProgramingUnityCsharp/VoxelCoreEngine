using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;

namespace NoiseForge.Graphics
{
    internal class Camera
    {

        private float SPEED = 8f;
        private float SENSITIVITY = 250;

        public Vector3 front = new Vector3(0.75f, -.5f, .43f);
        private Vector3 right = Vector3.UnitX;
        private Vector3 up = Vector3.UnitY;


        public Camera(float fieldOfView = 60, float aspect = 1.77f, float near = 0.1f, float far = 100)
        {
            Position = new Vector3(0, 20, 5);
            PerspectiveMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(fieldOfView), aspect, near, far);
            GetViewMatrix();
        }

        public Vector3 Position { get; private set; }
        private float Pitch { get => Math.Clamp(_pitch, -89, 89); set => _pitch = value; }
        private float _pitch = -30;
        private float yaw = 30;
        private bool firstMove = true;
        public Vector2 lastPos;
        public Matrix4 ViewMatrix { get; private set; }
        public Matrix4 PerspectiveMatrix { get; private set; }
        public Matrix4 GetViewMatrix()
        {
            ViewMatrix = Matrix4.LookAt(Position, Position + front, up);
            Debug.WriteLine("front : " + front);
            return ViewMatrix;
        }
        public void InputController(KeyboardState input, MouseState mouse, FrameEventArgs e)
        {
            if (input.IsKeyDown(Keys.W))
            {
                Position += front * SPEED * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.A))
            {
                Position -= right * SPEED * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.D))
            {
                Position += right * SPEED * (float)e.Time;

            }
            if (input.IsKeyDown(Keys.S))
            {
                Position -= front * SPEED * (float)e.Time;

            }
            if (input.IsKeyDown(Keys.E))
            {
                Position += up * SPEED * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.Q))
            {
                Position -= up * SPEED * (float)e.Time;
            }
            if (mouse.IsButtonDown(MouseButton.Left))
            {

                if (firstMove)
                {
                    UpdateVectors();
                    lastPos = new Vector2(mouse.X, mouse.Y);
                    firstMove = false;
                }
                else
                {



                    float xDelta = mouse.X - lastPos.X;
                    float yDelta = mouse.Y - lastPos.Y;
                    lastPos = new Vector2(mouse.X, mouse.Y);


                    yaw += xDelta * SENSITIVITY * (float)e.Time;
                    Pitch -= yDelta * SENSITIVITY * (float)e.Time;

                }

                UpdateVectors();

            }



        }

        private void UpdateVectors()
        {
            front.X += (float)(Math.Cos(MathHelper.DegreesToRadians(Pitch)) * Math.Cos(MathHelper.DegreesToRadians(yaw)));
            front.Y += (float)(Math.Sin(MathHelper.DegreesToRadians(Pitch)));
            front.Z += (float)(Math.Cos(MathHelper.DegreesToRadians(Pitch)) * Math.Sin(MathHelper.DegreesToRadians(yaw)));

            front.Normalize();

            right = Vector3.Cross(front, Vector3.UnitY);
            right.Normalize();
            up = Vector3.Cross(right, front);
            up.Normalize();

        }

        public void Update(KeyboardState input, MouseState mouse, FrameEventArgs e)
        {
            InputController(input, mouse, e);
        }

    }
}
