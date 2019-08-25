
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using OpenGL_Game.Objects;
using OpenGL_Game.Components;

namespace OpenGL_Game.Managers
{ 
    class EntityManager
    {
        List<Entity> entityList;
        static List<Entity> newEntityList;

        public EntityManager()
        {
            entityList = new List<Entity>();
            newEntityList = new List<Entity>();
        }

        public void AddEntity(Entity entity)
        {
            Entity result = FindEntity(entity.Name);
            //Debug.Assert(result != null, "Entity '" + entity.Name + "' already exists");
            entityList.Add(entity);
        }

        private Entity FindEntity(string name)
        {
            return entityList.Find(delegate(Entity e)
            {
                return e.Name == name;
            }
            );
        }
      

        public List<Entity> Entities()
        {
            return entityList;
        }

        public void Close()
        {
           foreach(Entity entity in entityList)
            {
                List<IComponent> components = entity.Components;
                foreach(IComponent com in components)
                {
                    if (com.ComponentType == ComponentTypes.COMPONENT_AUDIO)
                    {
                        ((ComponentAudio)com).Close();
                    }
                    else if (com.ComponentType == ComponentTypes.COMPONENT_TEXTURE)
                    {
                        ((ComponentTexture)com).Close();
                    }
                }
            }
        }

        public static void Remove(Entity entity)
        {
            newEntityList.Add(entity);
        }
        public bool isRemoved(Entity entity)
        {
            if(newEntityList.Contains(entity))
            {
                return true;
            }
            return false;
        }
    }
}
