#version 330 core
uniform vec3 lightColor;
out vec4 FragColor;
uniform sampler2D texture0;

uniform vec3 objectColor;
in vec3 viewLocation;
in vec3 lightDir;
in vec2 texCoords;
in vec3 Normal;
in vec3 objectPosition;
void main()
{
	vec3 norm = normalize(Normal);

	    float ambientStrength = 0.1;
    vec3 ambient = ambientStrength * lightColor;

	
	vec3 light = normalize(lightDir-objectPosition);
	
	float diff = max(dot(norm,light),0.0);
		
vec3 diffuse = diff * lightColor;

vec3 viewDir = normalize(viewLocation - objectPosition);

vec3 reflectDir = reflect(-light, norm); 
	float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
	float specularStrength = 0.5f;
vec3 specular = specularStrength * spec * lightColor; 

vec3 result = (ambient+diffuse+specular)*objectColor;
vec4 FragTex = texture(texture0,texCoords);
if(FragTex.w < 0.4f)
	{
	discard;
	}
	FragColor = vec4(result,1.0) * FragTex;
}