using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace OpenGL_Game.Components
{
    class ComponentAi : IComponent
    {
        List<Vector3> openlist = new List<Vector3>();

        public ComponentAi()
        {
            
        }
        public List<Vector3> GetPoints
        {
            get { return openlist; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_AI; }
        }
    }
}
