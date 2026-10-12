#version 330 core

in vec3 lightColor;
out vec4 FragColor;
uniform samplerCube texture0;
in vec3 viewLocation;
in vec3 lightDir;
in vec3 texCoords;
in vec3 Normal;
in vec3 objectPosition;
void main()
{
	vec3 norm = normalize(Normal);

	    float ambientStrength = 0.1;
    vec3 ambient = ambientStrength * lightColor;

	
	vec3 light = normalize(lightDir);
	
	float diff = max(dot(norm,light),0.0);
		
vec3 diffuse = diff * lightColor;

vec3 viewDir = normalize(viewLocation - objectPosition);

vec3 reflectDir = reflect(-light, norm); 
	float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
	float specularStrength = 0.5f;
vec3 specular = specularStrength * spec * lightColor; 

vec3 result = (ambient+diffuse+specular);
vec4 FragTex = texture(texture0,texCoords);
if(FragTex.w < 0.4f)
	{
	discard;
	}
	FragColor = vec4(result,1.0) * FragTex;
}