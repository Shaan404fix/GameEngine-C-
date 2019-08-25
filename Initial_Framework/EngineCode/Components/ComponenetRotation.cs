using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Components
{
    class ComponenetRotation : IComponent
    {
        float RotDeg;
        char dir;

        public ComponenetRotation()
        {
            RotDeg = 0.0f;
            dir = 'X';
        }

        public ComponenetRotation(float Rot, char direction)
        {
            RotDeg = Rot;
            dir = direction;
        }

        public float Rot
        {
            get { return RotDeg; }
            set { RotDeg = value; }
        }
        public char direction
        {
            get { return dir; }
            set { dir = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_ROTATE; }
        }
    }
}
