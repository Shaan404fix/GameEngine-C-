#version 330
 
in vec3 v_TexCoord;
uniform sampler3D s_texture;

out vec4 Color;
 
void main()
{
    Color = texture3D(s_texture, v_TexCoord);
}