#version 420
#pragma optionNV unroll all

uniform sampler2DArrayShadow shadowTex;

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

in Light[1] lightEye;
in vec4[1] lightPos;

in vec3 positionEye;
in vec3 normalEye;
in vec3 colourEye;

float ShadowTest(vec4 positionLight, int layer) 
{
    // move so we get rid of "texture" in shadows = shadow map acne
	// if offset too big -> some objects wont have a shadow
	vec3 shadowCoords = (positionLight.xyz-vec3(0,0,0.005))/positionLight.w;
	shadowCoords = shadowCoords* 0.5 + 0.5;

	vec3 size = textureSize(shadowTex, 0) / 3.1;

	// need every time different angle
	float c = cos(positionLight.length*100);
	float s = sin(positionLight.length*100);
	mat3 rotZ = mat3(c, -s, 0, s, c, 0, 0, 0, 1);

	vec3 poissonDisk[8];
	poissonDisk[0] = rotZ * vec3(-0.613392 / size.x, 0.617481 / size.y, 0);
	poissonDisk[1] = rotZ * vec3(0.170019 / size.x, -0.040254 / size.y, 0);
	poissonDisk[2] = rotZ * vec3(-0.299417 / size.x, 0.791925 / size.y, 0);
	poissonDisk[3] = rotZ * vec3(0.645680 / size.x, 0.493210 / size.y, 0);
	poissonDisk[4] = rotZ * vec3(-0.651784 / size.x, 0.717887 / size.y, 0);
	poissonDisk[5] = rotZ * vec3(0.421003 / size.x, 0.027070 / size.y, 0);
	poissonDisk[6] = rotZ * vec3(-0.817194 / size.x, -0.271096 / size.y, 0);
	poissonDisk[7] = rotZ * vec3(-0.705374 / size.x, -0.668203 / size.y, 0);

	// want to rotate the samples

	// returns distance from light compared to neighbors, looks at 4 neighbors
	// now using poissonDisk sampling
	float lightDepth = 0;
	for (int s = 0; s < poissonDisk.length;s++) {
		vec4 coord;
		coord.xyw = shadowCoords.xyz + poissonDisk[s];
		coord.z = layer;
		lightDepth += texture(shadowTex, coord);
	}
	
	return lightDepth/poissonDisk.length;
}

void main() 
{    
	vec3 color = vec3(0);

    // specular setup
    float shininess = 32;
    vec3 viewDirection = normalize(-positionEye);

    // diffuse setup
    // apply normal map
    vec3 normalEyeUnit = normalize(normalEye);

    for(int l = 0; l < 1; l++)
    {
        vec3 lightDirection = normalize(lightEye[l].position - positionEye);
        float theta = dot(lightDirection, normalize(-lightEye[l].direction));
        float epsilon   = lightEye[l].cutoff - lightEye[l].outerCutoff;
        float intensity = clamp((theta - lightEye[l].outerCutoff) / epsilon, 0.0, 1.0);

        // ambient contribution
        // just the albedo texture * ambient multiplier
        vec3 ambient = lightEye[l].ambient; // * texture(albedo, texCoords).rgb;

        // diffuse contribution
        // contribution is calculated as the dot product between the light direction and fragment position
        // it will approach 1 for fragments facing the light
        float diffuseContribution = max(dot(normalEyeUnit, lightDirection), 0.0);
        vec3 diffuse = lightEye[l].diffuse * diffuseContribution; // * texture(albedo, texCoords).rgb;

        // specular contribution
        // it is view dependent and based on the angle under which a ray reflects
        // blinn-phong!
        vec3 halfwayDir = normalize(lightDirection + viewDirection);
        float specularContribution = pow(max(dot(normalEyeUnit, halfwayDir), 0.0), shininess);
        vec3 specular = lightEye[l].specular * specularContribution; //* texture(specular, texCoords).rgb;

        // attenuation
        // light falloff over distance
        float d = length(lightEye[l].position - positionEye);
        float attenuation = 1.0 / (1.0 + lightEye[l].linear * d + lightEye[l].quadratic * (d * d));    
        ambient  *= attenuation;
        diffuse  *= attenuation;
        specular *= attenuation;

		if (theta > lightEye[l].outerCutoff) {
            // spotlight fade
            diffuse  *= intensity;
            specular *= intensity;

            float shadow = 1.0;
            shadow = ShadowTest(lightPos[l], l);
            color += ambient + (diffuse + specular) * shadow;

		} else {
            // residual light from the spotlight
            color += ambient;
		}   
    }

    gl_FragColor = vec4(colourEye, 1.0f);
}