using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGL_Game.Objects
{
    public class Geometry
    {
        List<float> vertices = new List<float>();

        List<Vector3> vertexObj = new List<Vector3>();
        List<Vector2> texture = new List<Vector2>();
        List<Vector3> normal = new List<Vector3>();
        List<int> indicies = new List<int>();
        float[] verticesArray = null;
        float[] normalArray = null;
        float[] textureArray = null;
        int[] indiciesArray = null;

        public Vector3 bBoxMin;
        public Vector3 bBoxMax;

        int numberOfTriangles;
        int numberOfQuads;
        // Graphics
        private int vao_Handle;
        private int vbo_verts;
        private int[] vbo_vert = new int[2];

        private static bool once = true;

        public Geometry()
        {
        }

        public void LoadObject(string filename)
        {
            string line;

            try
            {
                FileStream fin = File.OpenRead(filename);
                StreamReader sr = new StreamReader(fin);

                GL.GenVertexArrays(1, out vao_Handle);
                GL.BindVertexArray(vao_Handle);
                GL.GenBuffers(1, out vbo_verts);

                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    string[] values = line.Split(',');

                    if (values[0].StartsWith("NUM_OF_TRIANGLES"))
                    {
                        numberOfTriangles = int.Parse(values[0].Remove(0, "NUM_OF_TRIANGLES".Length));
                        continue;
                    }

                    if (values[0].StartsWith("NUM_OF_Quads"))
                    {
                        numberOfQuads = int.Parse(values[0].Remove(0, "NUM_OF_Quads".Length));
                        continue;
                    }

                    if (values[0].StartsWith("//") || values.Length < 5) continue;

                    vertices.Add(float.Parse(values[0]));
                    vertices.Add(float.Parse(values[1]));
                    vertices.Add(float.Parse(values[2]));
                    vertices.Add(float.Parse(values[3]));
                    vertices.Add(float.Parse(values[4]));
                    if(once)
                    {
                        bBoxMin.X = float.Parse(values[0]);
                        bBoxMin.Y = float.Parse(values[1]);
                        bBoxMin.Z = float.Parse(values[2]);

                        bBoxMax.X = float.Parse(values[0]);
                        bBoxMax.Y = float.Parse(values[1]);
                        bBoxMax.Z = float.Parse(values[2]);
                        once = false;
                    }
                    if (bBoxMin.X > float.Parse(values[0]))
                    {
                        bBoxMin.X = float.Parse(values[0]);
                    }
                    if (bBoxMin.Y > float.Parse(values[1]))
                    {
                        bBoxMin.Y = float.Parse(values[1]);
                    }
                    if (bBoxMin.Z > float.Parse(values[2]))
                    {
                        bBoxMin.Z = float.Parse(values[2]);
                    }

                    if (bBoxMax.X < float.Parse(values[0]))
                    {
                        bBoxMax.X = float.Parse(values[0]);
                    }
                    if (bBoxMax.Y < float.Parse(values[1]))
                    {
                        bBoxMax.Y = float.Parse(values[1]);
                    }
                    if (bBoxMax.Z < float.Parse(values[2]))
                    {
                        bBoxMax.Z = float.Parse(values[2]);
                    }
                }
                once = true;

                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_verts);
                GL.BufferData<float>(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Count * 4), vertices.ToArray<float>(), BufferUsageHint.StaticDraw);

                // Positions
                GL.EnableVertexAttribArray(0);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

                // Tex Coords
                if (numberOfTriangles != 0)
                {
                    GL.EnableVertexAttribArray(1);
                    GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
                }
                else
                {
                    GL.EnableVertexAttribArray(1);
                    GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3);
                }
                GL.BindVertexArray(0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }


           public void LoadObj(string filename)
           {
               try
               {
                   FileStream fin = File.OpenRead(filename);
                   StreamReader sr = new StreamReader(fin);
                   string line = "";
                   int size;
                   GL.GenVertexArrays(1, out vao_Handle);
                   GL.BindVertexArray(vao_Handle);
                   GL.GenBuffers(2, vbo_vert);

                   while (!sr.EndOfStream)
                   {
                       line = sr.ReadLine(); // vertex format 
                       string[] currentLine = line.Split(' ');
                       if (currentLine[0] == "v")
                       {
                           Vector3 vertex = new Vector3(float.Parse(currentLine[1]),
                               float.Parse(currentLine[2]), float.Parse(currentLine[3]));
                           vertexObj.Add(vertex);
                       }

                       else if (currentLine[0] == "vt")
                       {
                           Vector2 textures = new Vector2(float.Parse(currentLine[1]),
                               float.Parse(currentLine[2]));
                           texture.Add(textures);
                       }

                       else if (currentLine[0] == "vn")
                       {
                           Vector3 normals = new Vector3(float.Parse(currentLine[1]),
                               float.Parse(currentLine[2]), float.Parse(currentLine[3]));
                           normal.Add(normals);

                       }
                       else if (currentLine[0] == "f")
                       {
                           textureArray = new float[vertexObj.Count * 2];
                           normalArray = new float[vertexObj.Count * 3];
                           break;
                       }
                   }
                   while (line != null)
                   {
                       string[] current = line.Split(' ');
                       if (current[0] != "f")
                       {
                           line = sr.ReadLine();
                           continue;
                       }
                       string[] currentLine = line.Split(' ');
                       string[] vertex1 = currentLine[1].Split('/');
                       string[] vertex2 = currentLine[2].Split('/');
                       string[] vertex3 = currentLine[3].Split('/');

                       processVertex(vertex1, indicies, texture, normal, textureArray, normalArray);
                       processVertex(vertex2, indicies, texture, normal, textureArray, normalArray);
                       processVertex(vertex3, indicies, texture, normal, textureArray, normalArray);
                       line = sr.ReadLine();
                   }

                   sr.Close();

                   verticesArray = new float[vertexObj.Count * 3];
                   indiciesArray = new int[indicies.Count];

                   int vertexPoiinter = 0;
                   bBoxMin = vertexObj.First<Vector3>();
                   foreach (Vector3 vertex in vertexObj)
                   {
                       verticesArray[vertexPoiinter++] = vertex.X;
                       verticesArray[vertexPoiinter++] = vertex.Y;
                       verticesArray[vertexPoiinter++] = vertex.Z;

                       if(bBoxMin.X > vertex.X)
                       {
                           bBoxMin.X = vertex.X;
                       }
                       if (bBoxMin.Y > vertex.Y)
                       {
                           bBoxMin.Y = vertex.Y;
                       }
                       if (bBoxMin.Z > vertex.Z)
                       {
                           bBoxMin.Z = vertex.Z;
                       }

                       if (bBoxMax.X < vertex.X)
                       {
                           bBoxMax.X = vertex.X;
                       }
                       if (bBoxMax.Y < vertex.Y)
                       {
                           bBoxMax.Y = vertex.Y;
                       }
                       if (bBoxMax.Z < vertex.Z)
                       {
                           bBoxMax.Z = vertex.Z;
                       }
                   }

                   int[] ind = indicies.ToArray();

                   GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_vert[0]);
                   GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(verticesArray.Length * sizeof(float)), verticesArray, BufferUsageHint.StaticDraw);

                   GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
                   if (verticesArray.Length * sizeof(float) != size)
                   {
                       throw new ApplicationException("Vertex data not loaded onto graphics card correctly");
                   }

                   GL.BindBuffer(BufferTarget.ElementArrayBuffer, vbo_vert[1]);
                   GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(ind.Length * sizeof(float)), ind, BufferUsageHint.StaticDraw);

                   GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out size);
                   if (ind.Length * sizeof(float) != size)
                   {
                       throw new ApplicationException("Index data not loaded onto graphics card correctly");
                   }

                   // Positions
                   GL.EnableVertexAttribArray(0);
                   GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);


                   // Tex Coords
                   GL.EnableVertexAttribArray(1);
                   GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, true, 3 * sizeof(float), 0);

                   GL.BindVertexArray(0);
               }
               catch(Exception e)
               {
                   throw new Exception("Something went wrong: " + e);
               }
           }
           public static void processVertex(string[] vertexData, List<int> indices, List<Vector2> textures, List<Vector3> normals, float[] textureArray, float[] normalArray)
           {
               int currentVectorPointer = int.Parse(vertexData[0]) - 1;
               indices.Add(currentVectorPointer);

               Vector2[] textureA = textures.ToArray();
               Vector2 currentTex = textureA[int.Parse(vertexData[1]) - 1];

               textureArray[currentVectorPointer * 2] = currentTex.X;
               textureArray[currentVectorPointer * 2 + 1] = 1 - currentTex.Y;

               Vector3[] norm = normals.ToArray();
               Vector3 currentNorm = norm[int.Parse(vertexData[2]) - 1];
               normalArray[currentVectorPointer * 3] = currentNorm.X;
               normalArray[currentVectorPointer * 3 + 1] = currentNorm.Y;
               normalArray[currentVectorPointer * 3 + 2] = currentNorm.Z;
           }

        public void Render()
        {
            GL.BindVertexArray(vao_Handle);
            if (numberOfTriangles != 0)
            {
                GL.DrawArrays(PrimitiveType.Triangles, 0, numberOfTriangles * 3);
            }
            else if(numberOfQuads != 0)
            {
                GL.DrawArrays(PrimitiveType.Quads, 0, numberOfQuads * 4);
            }
            else
            {
                GL.DrawElements(PrimitiveType.Triangles, indicies.Count, DrawElementsType.UnsignedInt, 0);

            }
        }
    }
}