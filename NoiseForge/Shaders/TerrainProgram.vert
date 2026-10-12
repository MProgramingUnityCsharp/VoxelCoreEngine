#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormals;
layout (location = 2) in vec2 atexCoords;
layout (location = 3) in mat4 model;
layout (std140) uniform  Matrices
{
mat4 projection;
mat4 view;
vec3 lightPos;
vec3 viewPos;
};
layout (std140) uniform Lights
{
vec3 lightColorRay;
};
out vec3 lightDir;
out vec3 Normal;
out vec3 texCoords;
out vec3 objectPosition;
out vec3 viewLocation;
out vec3 lightColor;
void main()
{
	gl_Position = projection * view * model * vec4(aPos,1);
	texCoords = aPos;
	lightDir = lightPos;
	Normal =  aNormals;
	objectPosition = vec3(model * vec4(aPos, 1.0));
	lightColor = lightColorRay;

}