using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.Components;
using OpenGL_Game.Objects;
using OpenGL_Game.Scenes;

namespace OpenGL_Game.Systems
{
    class SystemRender : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_GEOMETRY | ComponentTypes.COMPONENT_TEXTURE);

        protected int pgmID;
        protected int vsID;
        protected int fsID;
        protected int attribute_vtex;
        protected int attribute_vpos;
        protected int uniform_stex;
        protected int uniform_mview;

        public SystemRender()
        {
            pgmID = GL.CreateProgram();
            LoadShader("Shaders/vs.glsl", ShaderType.VertexShader, pgmID, out vsID);
            LoadShader("Shaders/fs.glsl", ShaderType.FragmentShader, pgmID, out fsID);
            GL.LinkProgram(pgmID);
            Console.WriteLine(GL.GetProgramInfoLog(pgmID));

            attribute_vpos = GL.GetAttribLocation(pgmID, "a_Position");
            attribute_vtex = GL.GetAttribLocation(pgmID, "a_TexCoord");
            uniform_mview = GL.GetUniformLocation(pgmID, "WorldViewProj");
            uniform_stex = GL.GetUniformLocation(pgmID, "s_texture");

            if (attribute_vpos == -1 || attribute_vtex == -1 || uniform_stex == -1 || uniform_mview == -1)
            {
                Console.WriteLine("Error binding attributes");
            }
        }

        void LoadShader(String filename, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);
            using (StreamReader sr = new StreamReader(filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
        }

        public string Name
        {
            get { return "SystemRender"; }
        }

        Geometry geometry;
        int texture;
        Matrix4 tranform;
        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> components = entity.Components;

                IComponent geometryComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_GEOMETRY;
                });
                geometry = ((ComponentGeometry)geometryComponent).Geometry();

                IComponent textureComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_TEXTURE;
                });
                texture = ((ComponentTexture)textureComponent).Texture;

                IComponent positionComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });
                Vector3 position = ((ComponentPosition)positionComponent).Position;
                Matrix4 world = Matrix4.CreateTranslation(position);

                IComponent scaleComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_SCALE;
                });
                Vector3 Scale = ((ComponentScale)scaleComponent).Scale;
                Matrix4 Scle = Matrix4.CreateScale(Scale);

                IComponent rotateComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_ROTATE;
                });
                float Rot = ((ComponenetRotation)rotateComponent).Rot;
                char dir = ((ComponenetRotation)rotateComponent).direction;
                Matrix4 Rotation = Matrix4.CreateRotationX(Rot);
                switch (dir)
                {
                    case 'Y':
                        Rotation = Matrix4.CreateRotationY(Rot);
                        break;
                    case 'Z':
                        Rotation = Matrix4.CreateRotationZ(Rot);
                        break;
                }

                Matrix4 tranform1 = Scle * Rotation;
                tranform = tranform1 * world;

                Draw(tranform, geometry, texture);
            }
        }

        public void Draw(Matrix4 world, Geometry geometry, int texture)
        {
            GL.UseProgram(pgmID);

            GL.Uniform1(uniform_stex, 0);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.Enable(EnableCap.Texture2D);

            Matrix4 worldViewProjection = world * GameScene.gameInstance.view * GameScene.gameInstance.projection;
            GL.UniformMatrix4(uniform_mview, false, ref worldViewProjection);

            geometry.Render();

            GL.BindVertexArray(0);
            GL.UseProgram(0);
        }
    }
}
