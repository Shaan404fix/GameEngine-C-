using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using OpenTK;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using OpenTK.Graphics.OpenGL;
namespace OpenGL_Game.Components
{
    class ComponentCollision : IComponent
    {
        WallCollisions collision;
        string typecol;

        public ComponentCollision(string type)
        {
            this.collision = CollisionManager.load(type);
            typecol = type;
        }

        public ComponentCollision(string Typecollision, string type)
        {
            this.collision = CollisionManager.load(Typecollision);
            typecol = type;
        }
        public string CollisionType
        {
            get { return typecol; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_COLLISION; }
        }
        public WallCollisions collisions()
        {
            return collision;
        }
    }
}
