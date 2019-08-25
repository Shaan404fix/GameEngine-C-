using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenGL_Game.Managers;
using OpenGL_Game.Scenes;
namespace OpenGL_Game.Objects
{
    class WallCollisions
    {
        static int[] walls;
        static int[] food;
        static int[] Power;
        private static Vector3 dir;
        private static int i, j,s;
      public void ResetArray()
        {
            walls = new int[0];
            food  = new int[0];
        }
        public void addtoWall()
        {
            walls = new int[i];
            i++;
        }
        public void addtoFood()
        {
            food = new int[j];
            j++;
        }
        public void addPower()
        {
            Power = new int[s];
            s++;
        }

        public  int getlength()
        {
            return walls.Length;
        }
        public  int getFood()
        {
            return food.Length;
        }

        public bool CheckCollision(Vector3 position, Vector3 bMin, Vector3 bmax, Vector3 objPos, string Collision, ref Vector3 vel, bool hit)
        {
            double X = Math.Round(position.X);
            double Z = Math.Round(position.Z);
            position.X = (float)X;
            position.Z = (float)Z;
            if(!hit)
            {
                dir = vel;
            }
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
                return true;
            }
            return false;
        }

        public bool overCollsion(Vector3 pacPos, Vector3 ObjPos)
        {
            double X = Math.Round(pacPos.X);
            double Z = Math.Round(pacPos.Z);
            pacPos.X = (float)X;
            pacPos.Z = (float)Z;

            if (pacPos == ObjPos)
            {
                return true;
            }
            return false;
        }
    }
}
