using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
namespace VoxelCoreEngine.RenderPipeline
{
    internal class Shader
    {
        public int programLocation;

        public Shader(string vertexShaderPath, string fragmentShaderPath)
        {

            int vertexShaderLocation = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShaderLocation, ReadShaderSource(vertexShaderPath));
            GL.CompileShader(vertexShaderLocation);
            int fragmentShaderLocation = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShaderLocation, ReadShaderSource(fragmentShaderPath));

            programLocation = GL.CreateProgram();
            GL.AttachShader(programLocation, vertexShaderLocation);
            GL.AttachShader(programLocation, fragmentShaderLocation);
            GL.LinkProgram(programLocation);
            Use();



            GL.DetachShader(programLocation, vertexShaderLocation);
            GL.DetachShader(programLocation, fragmentShaderLocation);
            GL.DeleteShader(fragmentShaderLocation);
            GL.DeleteShader(vertexShaderLocation);

        }
        public void UniformMatrix4(string name, Matrix4 matrix)
        {
            int index = ExtractLocation(name);
            GL.UniformMatrix4(index, false, ref matrix);
        }

        private int ExtractLocation(string name)
        {
            return GL.GetUniformLocation(programLocation, name);
        }

        public void UniformFloat(string name, float magnitude)
        {
            int index = ExtractLocation(name);
            GL.Uniform1(index, magnitude);
        }
        private string ReadShaderSource(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        internal void Use()
        {
            GL.UseProgram(programLocation);
        }

        internal void UniformVector(string name, Vector3 vector3)
        {
            int index = ExtractLocation(name);
            GL.Uniform3(index, ref vector3);
        }
        public void SetBindingPoint(string uniformMemoryName, int bindingPoint)
        {
            int index_modelShader = GL.GetUniformBlockIndex(programLocation, uniformMemoryName);
            GL.UniformBlockBinding(programLocation, index_modelShader, bindingPoint);

        }
    }
}
