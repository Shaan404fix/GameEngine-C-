using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.Components;
using OpenGL_Game.Objects;
using OpenGL_Game.Scenes;

namespace OpenGL_Game.Systems
{
    class SystemTransform : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_SCALE);

        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> components = entity.Components;

                IComponent positionComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });
                Vector3 position = ((ComponentPosition)positionComponent).Position;
                Matrix4 trans = Matrix4.CreateTranslation(position);

                IComponent ScaleComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_SCALE;
                });
                Vector3 Scale = ((ComponentScale)ScaleComponent).Scale;
                Matrix4 scaler = Matrix4.CreateScale(Scale);

                Transform(trans, scaler);
            }
        }
        public string Name
        {
            get { return "SystemRender"; }
        }

        public void Transform(Matrix4 pos, Matrix4 scale)
        {
            Matrix4 transform = pos * scale;
        }
    }
}
