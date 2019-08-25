using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.Components;
using OpenGL_Game.Objects;
using OpenGL_Game.Scenes;


namespace OpenGL_Game.Components
{
    class ComponentScale : IComponent
    {
        Vector3 ScaleObj;

        public ComponentScale(float x, float y, float z)
        {
            ScaleObj = new Vector3(x, y, z);
        }

        public ComponentScale(Vector3 scale)
        {
            ScaleObj = scale;
            
        }

        public Vector3 Scale
        {
            get { return ScaleObj; }
            set { ScaleObj = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_SCALE; }
        }
    }
}
