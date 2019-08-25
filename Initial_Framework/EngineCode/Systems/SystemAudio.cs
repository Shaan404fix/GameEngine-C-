using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.Components;
using OpenGL_Game.Objects;
using OpenGL_Game.Scenes;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;

namespace OpenGL_Game.Systems
{
    class SystemAudio : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_AUDIO);


        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {

                List<IComponent> components = entity.Components;

                IComponent positionComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });
                IComponent AudioComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_AUDIO;
                });

                Vector3 position = ((ComponentPosition)positionComponent).Position;

                Playback((ComponentAudio)AudioComponent, position);
            }
        }
        public string Name
        {
            get { return "SystemAudio"; }
        }
        private void Playback(ComponentAudio AudioComponent, Vector3 position)
        {
            AudioComponent.SetPosition(position);
        }
    }
}

