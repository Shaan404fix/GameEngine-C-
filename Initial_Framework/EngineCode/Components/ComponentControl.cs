using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Input;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using OpenTK;

namespace OpenGL_Game.Components
{
    class ComponentControl : IComponent
    {
        Control control;
       
        public ComponentControl(SceneManager e)
        {
           
            this.control = ControlManager.Initialize(e);
        }
        public Control controls()
        {
            return control;
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_CONTROL; }
        }
    }
}
