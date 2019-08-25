using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenGL_Game.Components;
using OpenGL_Game.Objects;
using OpenGL_Game.Scenes;



namespace OpenGL_Game.Managers
{
    class CollisionManager
    {    
        public static WallCollisions load(string type)
        {
            WallCollisions collisions = new WallCollisions();
            switch(type)
            {
                case "food":
                    collisions.addtoFood();
                    break;
                case "wall":
                    collisions.addtoWall();
                    break;
                case "Power":
                    collisions.addPower();
                    break;
            }
            return collisions;
        }

        public static bool CheckCollision(Vector3 position, Vector3 bMin, Vector3 bmax, Vector3 objPos, string Collision, ref Vector3 vel)
        {

            double X = Math.Round(position.X);
            double Z = Math.Round(position.Z);
            position.X = (float)X;
            position.Z = (float)Z;

            switch (Collision)
            {
                case "WallV":
                    bmax.X *= objPos.X;
                    bMin.X *= objPos.X;

                    break;
                case "WallVT":
                    bmax.X *= objPos.X;
                    bMin.X *= objPos.X;

                    bMin.Z += objPos.Z;
                    bmax.Z += objPos.Z;
                    break;
                case "WallH":
                    bmax.Z *= objPos.Z;
                    bMin.Z *= objPos.Z;
                    break;

            }
            if (position.X >= bMin.X && position.X <= bmax.X &&
               position.Z >= bMin.Z && position.Z <= bmax.Z)
            {
                vel.Xz *= 0;

                return true;
            }
            return false;
        }
    }
}
