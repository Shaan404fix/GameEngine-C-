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
    class SystemAi : ISystem
    {
        private static IComponent playerpos;
        public static bool hit;
        private static IComponent audio;
        private IComponent Ghostpos;
        private Vector3 velG;
        private static int i;

        const ComponentTypes MASK = (ComponentTypes.COMPONENT_GEOMETRY | ComponentTypes.COMPONENT_COLLISION);
        const ComponentTypes MASKT = (ComponentTypes.COMPONENT_CONTROL | ComponentTypes.COMPONENT_VELOCITY);
        const ComponentTypes MASKG = (ComponentTypes.COMPONENT_AI | ComponentTypes.COMPONENT_VELOCITY);

        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASKT) == MASKT)
            {
                List<IComponent> components = entity.Components;

                playerpos = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });

                hit = false;
                i = 0;

            }
            else if ((entity.Mask & MASKG) == MASKG)
            {
                List<IComponent> components = entity.Components;

                Ghostpos = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });
                
                IComponent VelocityComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_VELOCITY;
                });
                velG = ((ComponentVelocity)VelocityComponent).Velocity;

                audio = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_AUDIO;
                });
            }
            else if ((entity.Mask & MASK) == MASK)
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

                IComponent CollisionComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_COLLISION;
                });
                string collisoionType = ((ComponentCollision)CollisionComponent).CollisionType;
                WallCollisions wallColl = ((ComponentCollision)CollisionComponent).collisions();

                if (collisoionType == "WallH" || collisoionType == "WallV" || collisoionType == "WallVT")
                {
                    Motion2((ComponentPosition)Ghostpos, velG, geom, objPos, collisoionType, wallColl);
                }
            }
        }
        public string Name
        {
            get { return "SystemAi"; }
        }
        private void Motion2(ComponentPosition positions, Vector3 velocity, Geometry geom, Vector3 objPos, string collisionType, WallCollisions wallCol)
        {
            Vector3 player = ((ComponentPosition)playerpos).Position;
            Vector3 newpos = positions.Position;

            /*Vector3 pos = Vector3.Normalize(newpos);

            float dot = Vector3.Dot(pos, player);

            if (dot >= player.X)
            {
                velocity.X = dot;
                newpos += velocity * GameScene.dt;
                checkCollsion(newpos, geom, objPos, collisionType, ref velocity, wallCol);
            }
            if (dot >= player.Z)
            {
                velocity.Z = -10;
                newpos += velocity * GameScene.dt;
                checkCollsion(newpos, geom, objPos, collisionType, ref velocity, wallCol);

            }*/

            Vector3 playerposv = ((ComponentPosition)playerpos).Position;
            
            Vector3 newvel;
            if (playerposv.X > newpos.X)
            {
                velocity.X = 8;
            }

            if (playerposv.X < newpos.X)
            {
                velocity.X = -8;
            }

            if (playerposv.Z < newpos.Z)
            {
                velocity.Z = -8;
              
            }
            if (playerposv.Z > newpos.Z)
            {
                velocity.Z = 8;
            }

            newvel = velocity * 15;
            newpos += newvel * GameScene.dt;

            checkCollsion(newpos, geom, objPos, collisionType, ref velocity, wallCol);
                PlayerHit((ComponentPosition)Ghostpos,(ComponentPosition)playerpos, wallCol);

            if (hit != true && wallCol.getlength() == i)
            {
                positions.Position += (velocity * GameScene.dt);

            }
            else if (wallCol.getlength() == i)
            {
                velocity *= -10;
                positions.Position += (velocity * GameScene.dt);
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
        public void PlayerHit(ComponentPosition ghostpos, ComponentPosition pacPos, WallCollisions wallCol)
        {
            Vector3 pac = pacPos.Position;
            Vector3 newpos = ghostpos.Position;
            double X = Math.Round(pac.X);
            double Z = Math.Round(pac.Z);
            pac.X = (float)X;
            pac.Z = (float)Z;
            X = Math.Round(newpos.X);
            Z = Math.Round(newpos.Z);
            newpos.X = (float)X;
            newpos.Z = (float)Z;

            if (newpos == pac)
            {
                ((ComponentAudio)audio).Start();
                GameScene.Lives--;    
                pacPos.Position = GameScene.origin;
                ghostpos.Position = GameScene.GhostOrigin;
                
            }
        }
    }
}

  