using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
using OpenGL_Game.Managers;
using OpenTK;
using OpenTK.Graphics.OpenGL;
namespace OpenGL_Game.Components
{
    class ComponentTexture : IComponent
    {
        int texture;

        public ComponentTexture(string textureName)
        {
            texture = ResourceManager.LoadTexture(textureName);
        }

        public int Texture
        {
            get { return texture; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_TEXTURE; }
        }
        public void Close()
        {
            GL.DeleteTexture(texture);
        }
    }
}
