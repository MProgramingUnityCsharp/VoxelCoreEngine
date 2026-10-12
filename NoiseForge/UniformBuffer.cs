using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace NoiseForge
{
    internal class UniformBuffer
    {
        public int _uniformBufferObject;
        public UniformBuffer(int size, int bindingPoint)
        {
            _uniformBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.UniformBuffer, _uniformBufferObject);
            GL.BufferData(BufferTarget.UniformBuffer, size, IntPtr.Zero, BufferUsageHint.StaticDraw);
            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, bindingPoint, _uniformBufferObject);
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);
        }
        public void SetMatrixBuffer(Matrix4 matrix4, int offset)
        {
            GL.BindBuffer(BufferTarget.UniformBuffer, _uniformBufferObject);
            GL.BufferSubData(BufferTarget.UniformBuffer, offset, Vector4.SizeInBytes * 4, ref matrix4);
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);

        }
        public void SetVector3Buffer(Vector3 vector, int offset)
        {
            GL.BindBuffer(BufferTarget.UniformBuffer, _uniformBufferObject);
            GL.BufferSubData(BufferTarget.UniformBuffer, offset, Vector4.SizeInBytes, ref vector);
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);

        }
        public void BindUniformBuffer()
        {
            GL.BindBuffer(BufferTarget.UniformBuffer, _uniformBufferObject);
        }

    }
}
