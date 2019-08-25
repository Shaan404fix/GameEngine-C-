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
    class SystemCollision : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_VELOCITY);
        const ComponentTypes MASKT = (ComponentTypes.COMPONENT_POSITION);

        protected static Entity player;
        protected static Vector3 pos;


        public void OnAction(Entity entity)
        {
          
                if ((entity.Mask & MASKT) == MASKT)
                {
                    List<IComponent> components = entity.Components;

                    IComponent positionComponent = components.Find(delegate (IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                    });

                    pos = ((ComponentPosition)positionComponent).Position;

                }
            
            else if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> components = entity.Components;

                IComponent positionComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });

                IComponent openListComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_AI;
                });
                List<Vector3> openlist = ((ComponentAi)openListComponent).GetPoints;

                IComponent VelocityComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_VELOCITY;
                });
                Vector3 vel = ((ComponentVelocity)VelocityComponent).Velocity;


                //MoveAi(pos, openlist, vel);
                MoveAi(pos, (ComponentPosition)positionComponent, vel);
            }

        }

        public string Name
        {
            get { return "SystemCollsion"; }
        }


        private void MoveAi(Vector3 playerPos, List<Vector3> openlist, Vector3 vel)
        {
            List<Vector3> closedList = new List<Vector3>();
            Vector3[] open = openlist.ToArray();

            if (openlist[1] == playerPos)
            {
                return;
            }
        }

        private void MoveAi(Vector3 playerPos, ComponentPosition ghostPos, Vector3 vel)
        {
            if (ghostPos.Position.Xz != playerPos.Xz)
            {

                if (ghostPos.Position.Z < playerPos.Z)
                {
                    vel.Z = playerPos.Z * -1;
                }
                else
                {
                    vel.Z = playerPos.Z;
                }
                ghostPos.Position += vel * GameScene.dt;
            }

        }
    }
}

