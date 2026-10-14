using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
namespace VoxelCoreEngine.RenderPipeline
{
    internal class Model<T> where T : struct
    {
        private int _vao;
        private Dictionary<string, int> _vbos = new Dictionary<string, int>();
        public int VertexArrayObject => _vao;
        private T[] _vertexes;
        private int _dimension;
        private int _sizeOfJumpToNextPerMember;


        public Model(T[] vertices, int index, int dimensionOfVector, string BufferName, int stride = 0, int offset = 0)
        {
            _vertexes = vertices;
            _dimension = dimensionOfVector;
            _sizeOfJumpToNextPerMember = stride;
            _offsetToFirstDimension = offset;


            _vao = GL.GenVertexArray();
            GL.BindVertexArray(_vao);
            _vbos.Add(BufferName, 0);
            _vbos[BufferName] = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbos[BufferName]);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * _vertexes.Length, _vertexes, BufferUsageHint.StreamDraw);
            GL.VertexAttribPointer(index, _dimension, VertexAttribPointerType.Float, false, _sizeOfJumpToNextPerMember, _offsetToFirstDimension);
            GL.EnableVertexAttribArray(index);

        }
        public void SetNewData<T2>(T2[] vertices, int index, int dimensionOfVector, int divisorValue, string BufferName, int stride = 0, int offset = 0) where T2 : struct
        {
            if (!_vbos.ContainsKey(BufferName))
            {
                _vbos.Add(BufferName, 0);
                _vbos[BufferName] = GL.GenBuffer();
            }
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbos[BufferName]);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.StreamDraw);
            GL.BindVertexArray(_vao);

            SetAttrib(index, dimensionOfVector, stride, offset);

        }
        public void SetMatrixBuffer(Matrix4[] vertices, int divisorLoop, int index, int dimensionOfVector, int divisorValue, string BufferName, int stride = 0, int offset = 0)
        {
            if (!_vbos.ContainsKey(BufferName))
            {
                _vbos.Add(BufferName, 0);
                _vbos[BufferName] = GL.GenBuffer();
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbos[BufferName]);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * Vector4.SizeInBytes * 4, vertices, BufferUsageHint.StreamDraw);
            GL.BindVertexArray(_vao);
            for (int i = 0; i < divisorLoop; i++)
            {
                SetAttrib(index + i, dimensionOfVector, stride, offset * i);
                SetDivisor(index + i, divisorValue);

            }

        }

        public void SetAttrib(int index, int dimensionOfVector, int stride, int offset)
        {
            GL.VertexAttribPointer(index, dimensionOfVector, VertexAttribPointerType.Float, false, stride, offset);
            GL.EnableVertexAttribArray(index);
        }

        public void SetDivisor(int index, int divisor)
        {
            GL.BindVertexArray(_vao);
            GL.VertexAttribDivisor(index, divisor);
        }
        public void BindVAO()
        {
            GL.BindVertexArray(_vao);
        }
        private nint _offsetToFirstDimension; public int GetVertexBufferObject(string name) => _vbos[name];

    }
}
