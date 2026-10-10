#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec2 atexCoords;
layout (location = 2) in mat4 model;
layout (std140) uniform  Matrices
{
mat4 projection;
mat4 view;
};

out vec2 texCoords;
void main()
{
	gl_Position = projection * view * model * vec4(aPos,1);
	texCoords = atexCoords;
}