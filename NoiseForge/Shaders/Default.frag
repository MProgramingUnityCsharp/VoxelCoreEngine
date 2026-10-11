#version 330 core
uniform vec3 lightColor;
out vec4 FragColor;
uniform sampler2D texture0;

uniform vec3 objectColor;

in vec3 lightDir;
in vec2 texCoords;
in vec3 Normal;
void main()
{
	vec3 light = normalize(lightDir);
	float diff = max(dot(light,Normal),0.0);
	FragColor = (diff +vec4(objectColor,1.0)*vec4(lightColor,1.0)) *texture(texture0,texCoords);
}