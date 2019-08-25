using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenGL_Game.Managers;
using OpenTK.Graphics.OpenGL;

namespace OpenGL_Game.Components
{
    class ComponentCubeTexture : IComponent
    {
        int texture;

        public ComponentCubeTexture(string[] textureName)
        {
            texture = ResourceManager.LoadTexture(textureName);
        }

        public int Texture
        {
            get { return texture; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_CUBETEXTURE; }
        }
        public void Close()
        {
            GL.DeleteTexture(texture);
        }
    }
}
