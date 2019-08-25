using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.Components;
using OpenGL_Game.Objects;
using OpenGL_Game.Scenes;
using OpenTK.Input;
using OpenGL_Game.Managers;

namespace OpenGL_Game.Systems
{
    class SystemControl : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_CONTROL | ComponentTypes.COMPONENT_VELOCITY);

        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> components = entity.Components;

                IComponent controlComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_CONTROL;
                });
                Control e = ((ComponentControl)controlComponent).controls();

                IComponent VelocityComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_VELOCITY;
                });
                Motion((ComponentVelocity)VelocityComponent, e);

            }
        }
        public string Name
        {
            get { return "SystemControl"; }
        }
        private void Motion(ComponentVelocity velocity, Control e)
        {
            if (e.KeyDown(Key.W))
            {
                velocity.Velocity = new Vector3(0.0f, 0.0f, -10.0f);
            }
            else if (e.KeyDown(Key.A))
            {
                velocity.Velocity = new Vector3(-10.0f, 0.0f, 0.0f);
            }
            else if (e.KeyDown(Key.S))
            {
                velocity.Velocity = new Vector3(0.0f, 0.0f, 10.0f);
            }
            else if (e.KeyDown(Key.D))
            {
                velocity.Velocity = new Vector3(10.0f, 0.0f, 0.0f);
            }
            else
            {
                velocity.Velocity = new Vector3(0.0f, 0.0f, 0.0f);
            }
        }
    }
}