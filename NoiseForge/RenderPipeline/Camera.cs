using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;

namespace VoxelCoreEngine.RenderPipeline
{
    internal class Camera
    {
        private float SPEED = 10f;
        private float SENSITIVITY;
        private float _fieldOfView;
        private float _aspect;
        public float _near;
        public float _far;
        private float _pitch = -30;
        private float yaw = 30;

        private float Pitch { get => Math.Clamp(_pitch, -89, 89); set => _pitch = value; }

        private bool firstMove = true;

        private Face _nearFace;
        private Face _farFace;
        private Face _rightFace;
        private Face _leftFace;
        private Face _topFace;
        private Face _bottomFace;

        public Matrix4 ViewMatrix { get; private set; }
        public Matrix4 PerspectiveMatrix { get; private set; }


        public Vector3 _front = new Vector3(0.75f, -.5f, .43f);
        private Vector3 _right = Vector3.UnitX;
        private Vector3 _up = Vector3.UnitY;
        public Vector3 Position { get; private set; }
        public bool CameraMovingState { get; internal set; }

        public Vector2 lastPos;
        private float farDot;
        private float nearDot;
        private float rightDot;
        private float leftDot;
        private float topDot;
        private float bottomDot;

        public Camera(float fieldOfView = 60, float aspect = 1.77f, float near = 0.1f, float far = 250)
        {
            _fieldOfView = fieldOfView;
            _aspect = aspect;
            _near = near;
            _far = far;


            Position = new Vector3(0, 20, 5);
            PerspectiveMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(fieldOfView), aspect, near, far);
            GetViewMatrix();

            CreateFrustumFromCamera();
        }

        public Matrix4 GetViewMatrix()
        {
            ViewMatrix = Matrix4.LookAt(Position, Position + _front, _up);
            return ViewMatrix;
        }
        public void InputController(KeyboardState input, MouseState mouse, FrameEventArgs e)
        {
            SENSITIVITY = (float)(0.5f / (e.Time));
            Debug.WriteLine($"Sensetivity : {SENSITIVITY}");
            UpdateVectors();

            if (input.IsKeyDown(Keys.W) || input.IsKeyDown(Keys.A) || input.IsKeyDown(Keys.D) || input.IsKeyDown(Keys.S) || input.IsKeyDown(Keys.E) || input.IsKeyDown(Keys.Q) || mouse.IsButtonDown(MouseButton.Left))
            {
                CameraMovingState = true;
            }
            else
            {
                CameraMovingState = false;
            }

            if (CameraMovingState)
            {
                CreateFrustumFromCamera();
            }

            if (input.IsKeyDown(Keys.W))
            {
                Position += _front * SPEED * (float)e.Time;

            }
            if (input.IsKeyDown(Keys.A))
            {
                Position -= _right * SPEED * (float)e.Time;

            }
            if (input.IsKeyDown(Keys.D))
            {
                Position += _right * SPEED * (float)e.Time;


            }
            if (input.IsKeyDown(Keys.S))
            {
                Position -= _front * SPEED * (float)e.Time;


            }
            if (input.IsKeyDown(Keys.E))
            {
                Position += _up * SPEED * (float)e.Time;

            }
            if (input.IsKeyDown(Keys.Q))
            {
                Position -= _up * SPEED * (float)e.Time;

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



                    float xDelta = mouse.X - lastPos.X;
                    float yDelta = mouse.Y - lastPos.Y;
                    lastPos = new Vector2(mouse.X, mouse.Y);


                    yaw += xDelta * SENSITIVITY * (float)e.Time;
                    Pitch -= yDelta * SENSITIVITY * (float)e.Time;

                }


            }





        }

        private void UpdateVectors()
        {

            _front.X = (float)(Math.Cos(MathHelper.DegreesToRadians(Pitch)) * Math.Cos(MathHelper.DegreesToRadians(yaw)));
            _front.Y = (float)(Math.Sin(MathHelper.DegreesToRadians(Pitch)));
            _front.Z = (float)(Math.Cos(MathHelper.DegreesToRadians(Pitch)) * Math.Sin(MathHelper.DegreesToRadians(yaw)));

            _front.Normalize();

            _right = Vector3.Cross(_front, Vector3.UnitY);
            _right.Normalize();
            _up = Vector3.Cross(_right, _front);
            _up.Normalize();
        }

        public void Update(KeyboardState input, MouseState mouse, FrameEventArgs e)
        {
            InputController(input, mouse, e);
        }
        void CreateFrustumFromCamera()
        {

            float halfVSide = _far * (float)Math.Tan(MathHelper.DegreesToRadians(_fieldOfView) * .5f);
            float halfHSide = halfVSide * _aspect;
            Vector3 frontMultFar = _far * _front;

            _nearFace = new Face() { Point = (Position + _near * _front), Normal = _front };
            _farFace = new Face()
            {
                Point = Position + frontMultFar,
                Normal = -_front
            };

            _rightFace = new Face()
            {
                Point = Position,
                Normal = Vector3.Cross(frontMultFar - _right * halfHSide, _up)
            };
            _leftFace = new Face()
            {
                Point = Position,
                Normal = Vector3.Cross(_up, frontMultFar + _right * halfHSide)
            };
            _topFace = new Face()
            {
                Point = Position,
                Normal = Vector3.Cross(_right, frontMultFar - _up * halfVSide)
            };
            _bottomFace = new Face()
            {
                Point = Position,
                Normal = Vector3.Cross(frontMultFar + _up * halfVSide, _right)
            };


        }

        public bool CheckFrustomView(Vector3 objectTranslation)
        {
            farDot = Vector3.Dot(_farFace.Normal, objectTranslation - _farFace.Point);
            nearDot = Vector3.Dot(_nearFace.Normal, objectTranslation - _nearFace.Point);
            rightDot = Vector3.Dot(_rightFace.Normal, objectTranslation - _rightFace.Point);
            leftDot = Vector3.Dot(_leftFace.Normal, objectTranslation - _leftFace.Point);
            topDot = Vector3.Dot(_topFace.Normal, objectTranslation - _topFace.Point);
            bottomDot = Vector3.Dot(_bottomFace.Normal, objectTranslation - _bottomFace.Point);
            if (farDot > 0 && nearDot > 0 && rightDot > 0 && leftDot > 0 && topDot > 0 && bottomDot > 0)
            {
                return true;
            }


            return false;
        }
    }
}
