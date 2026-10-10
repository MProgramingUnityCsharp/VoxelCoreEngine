#version 330 core
layout (location = 0) in vec2 aPos;

void main()
{
	gl_Position = vec4(vec3(aPos,0),1);
}