using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Components
{
    [FlagsAttribute]
    enum ComponentTypes {
        COMPONENT_NONE     = 0,
	    COMPONENT_POSITION = 1 << 0,
        COMPONENT_GEOMETRY = 1 << 1,
        COMPONENT_TEXTURE  = 1 << 2,
        COMPONENT_VELOCITY = 1 << 4,
        COMPONENT_AUDIO    = 1 << 5,
        COMPONENT_SCALE    = 1 << 6,
        COMPONENT_ROTATE   = 1 << 7,
        COMPONENT_COLLISION= 1 << 8,
        COMPONENT_CONTROL  = 1 << 9,
        COMPONENT_COLLISION2 = 1 << 10,
        COMPONENT_AI       = 1 << 11,
        COMPONENT_CUBETEXTURE = 1 << 12
    }

    interface IComponent
    {
        ComponentTypes ComponentType
        {
            get;
        }
    }
}
