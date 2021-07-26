#version 420
#pragma optionNV unroll all

layout(location=0)in vec3 position;
layout(location=2)in vec3 normal;

struct Light {
    vec3 position;  
    vec3 direction;  
  
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    
	float linear;
    float quadratic;

    float cutoff;
    float outerCutoff;

    mat4 matrix;
}; 

uniform mat4 matrixModel;
uniform mat4 matrixView;
uniform mat4 matrixProjection;

uniform Light[1] light;

out Light[1] lightEye;
out vec4[1] lightPos;

out vec3 positionEye;
out vec3 normalEye;


void main()
{	
    normalEye = mat3(matrixView * matrixModel) * normal;
    positionEye = (matrixView * matrixModel * vec4(position, 1.0f)).xyz;
    gl_Position = matrixProjection * vec4(positionEye, 1.0f);

    for (int l = 0; l < 1; l++)
    {
        lightEye[l].position = vec3(matrixView * vec4(light[l].position, 1.0f));
	    lightEye[l].direction = mat3(matrixView) * light[l].direction;

        lightEye[l].ambient = light[l].ambient;
        lightEye[l].diffuse = light[l].diffuse;
        lightEye[l].specular = light[l].specular;
    
	    lightEye[l].linear = light[l].linear;
        lightEye[l].quadratic = light[l].quadratic;

	    lightEye[l].cutoff = cos(light[l].cutoff);
	    lightEye[l].outerCutoff = cos(light[l].outerCutoff);

		lightPos[l] = light[l].matrix * matrixModel * vec4(position, 1.0f);
    }
}