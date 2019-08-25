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
    class SystemPhysics : ISystem
    {
        private IComponent playerpos;
        private Vector3 vel;
        private static bool hit;
        private static int i;
        private static int foodCount;
        private static float timer;
        static Entity power;

        const ComponentTypes MASK = (ComponentTypes.COMPONENT_GEOMETRY | ComponentTypes.COMPONENT_COLLISION);
        const ComponentTypes MASKT = (ComponentTypes.COMPONENT_CONTROL | ComponentTypes.COMPONENT_VELOCITY);

        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK || (entity.Mask & MASKT) == MASKT)
            {
                List<IComponent> components = entity.Components;

                IComponent PositionComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });
                Vector3 objPos = ((ComponentPosition)PositionComponent).Position;

                IComponent geometryComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_GEOMETRY;
                });
                Geometry geom = ((ComponentGeometry)geometryComponent).Geometry();

                if (entity.Name == "Pac-man")
                {
                    playerpos = PositionComponent;
                    IComponent VelocityComponent = components.Find(delegate (IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_VELOCITY;
                    });
                    vel = ((ComponentVelocity)VelocityComponent).Velocity;

                    hit = false;
                    i = -1;
                    return;
                }

                else
                {
                    IComponent CollisionComponent = components.Find(delegate (IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_COLLISION;
                    });
                    string collisoionType = ((ComponentCollision)CollisionComponent).CollisionType;
                    WallCollisions wallColl = ((ComponentCollision)CollisionComponent).collisions();

                    if (collisoionType == "food" || collisoionType == "power")
                    {
                        IComponent audio = components.Find(delegate (IComponent component)
                        {
                            return component.ComponentType == ComponentTypes.COMPONENT_AUDIO;
                        });
                        food((ComponentPosition)playerpos, objPos, entity, wallColl, (ComponentAudio)audio, collisoionType);

                    }
                    else if (collisoionType == "WallH" || collisoionType == "WallV" || collisoionType == "WallVT")
                    {
                        Motion((ComponentPosition)playerpos, vel, geom, objPos, collisoionType, wallColl);
                    }
                }
            }
        }
        public string Name
        {
            get { return "SystemPhysics"; }
        }


        private void Motion(ComponentPosition positions, Vector3 velocity, Geometry geom, Vector3 objPos, string collisionType, WallCollisions wallCol)
        {
            Vector3 newpos = positions.Position;
           Vector3 newvel =  velocity * 15 ;

            newpos += newvel * GameScene.dt;
            checkCollsion(newpos, geom, objPos, collisionType, ref velocity, wallCol);


            if (hit != true && wallCol.getlength() == i)
            {
                positions.Position += (velocity * GameScene.dt);
            }
            else if (wallCol.getlength() == i)
            {
                velocity *= -1;
                positions.Position += (velocity * GameScene.dt);
                hit = false;
            }
        }

        public void checkCollsion(Vector3 newPos, Geometry geometry, Vector3 objPos, string collisionType, ref Vector3 velocity, WallCollisions wallColl)
        {
            if (wallColl.CheckCollision(newPos, geometry.bBoxMin, geometry.bBoxMax, objPos, collisionType, ref velocity, hit))
            {
                hit = true;

            }
            i++;
            return;
        }

        public void food(ComponentPosition pos, Vector3 foodPos, Entity ent, WallCollisions end, ComponentAudio audio, string foodType)
        {
            Vector3 pacPos = pos.Position;

            if(timer >= 0 )
            {
                if (timer > 0)
                {
                    timer += GameScene.dt;
                }
                if (timer >= 1000  && power.Name == ent.Name)
                {
                    timer = 0;
                    audio.Stop();
                    EntityManager.Remove(ent);
                }
            }
            if (end.overCollsion(pacPos,foodPos))
            {
                if(foodType == "power")
                {
                    if (power != ent && timer > 10)
                    {
                        return;
                    }
                    timer = 0;
                    timer += GameScene.dt;
                    power = ent;
                }
                audio.Start();
                if (foodType != "power")
                {
                    EntityManager.Remove(ent);
                    foodCount++;

                    if (end.getFood() + 1 == foodCount)
                    {

                        GameScene.endGame = true;

                    }
                }
            }
        }
    }
}
