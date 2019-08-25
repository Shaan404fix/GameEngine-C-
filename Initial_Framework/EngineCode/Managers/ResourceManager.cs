using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.Objects;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;

namespace OpenGL_Game.Managers
{
    static class ResourceManager
    {
        static Dictionary<string, Geometry> geometryDictionary = new Dictionary<string, Geometry>();
        static Dictionary<string, int> textureDictionary = new Dictionary<string, int>();
        static Dictionary<string, int> AudioDictionary = new Dictionary<string, int>();

        public static Geometry LoadGeometry(string filename)
        {
            Geometry geometry;
            geometryDictionary.TryGetValue(filename, out geometry);
            if (geometry == null)
            {
                geometry = new Geometry();
                if (filename.Contains("txt"))
                {
                    geometry.LoadObject(filename);
                }
                else
                {
                    geometry.LoadObj(filename);
                }
                geometryDictionary.Add(filename, geometry);
            }

            return geometry;
        }

        public static int LoadTexture(string filename)
        {
            if (String.IsNullOrEmpty(filename))
                throw new ArgumentException(filename);

            int texture;
            textureDictionary.TryGetValue(filename, out texture);
            if (texture == 0)
            {
                texture = GL.GenTexture();
                textureDictionary.Add(filename, texture);
                GL.BindTexture(TextureTarget.Texture2D, texture);

                // We will not upload mipmaps, so disable mipmapping (otherwise the texture will not appear).
                // We can use GL.GenerateMipmaps() or GL.Ext.GenerateMipmaps() to create
                // mipmaps automatically. In that case, use TextureMinFilter.LinearMipmapLinear to enable them.
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                Bitmap bmp = new Bitmap(filename);
                BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

                bmp.UnlockBits(bmp_data);
            }

            return texture;
        }

        public static int LoadTexture(string[] filename)
        { 
            int texture = 0;
            for (int i = 0; i < filename.Length; i++)
            {
                textureDictionary.TryGetValue(filename[i], out texture);
                if (texture == 0)
                {
                    texture = GL.GenTexture();
                    textureDictionary.Add(filename[i], texture);
                    GL.BindTexture(TextureTarget.TextureCubeMap, texture);

                    Bitmap bmp = new Bitmap(filename[i]);
                    BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    
                    GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
                        OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);


                    GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                    GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                    GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                    GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
                    GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);


                    bmp.UnlockBits(bmp_data);
                }
            }

            return texture;
        }
        public static int LoadWav(string fileName)
        {
            int audiobuf;
            AudioDictionary.TryGetValue(fileName, out audiobuf);

            if (audiobuf == 0)
            {
                audiobuf = AL.GenBuffer();
                AudioDictionary.Add(fileName, audiobuf);

                int channels, bit_per_sample, sample_rate;
                byte[] sound_data = LoadWave(File.Open(fileName, FileMode.Open), out channels, out bit_per_sample, out sample_rate);

                ALFormat soundFormat =
                   channels == 1 && bit_per_sample == 8 ? ALFormat.Mono8 :
                   channels == 1 && bit_per_sample == 16 ? ALFormat.Mono16 :
                   channels == 2 && bit_per_sample == 8 ? ALFormat.Stereo8 :
                   channels == 2 && bit_per_sample == 16 ? ALFormat.Stereo16 :
                   (ALFormat)0;

                AL.BufferData(audiobuf, soundFormat, sound_data, sound_data.Length, sample_rate);
                if (AL.GetError() != ALError.NoError)
                {

                }
            }
            return audiobuf;
        }
        private static byte[] LoadWave(Stream stream, out int channels, out int bits, out int rate)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            using (BinaryReader reader = new BinaryReader(stream))
            {
                string signiture = new string(reader.ReadChars(4));
                if (signiture != "RIFF")
                {
                    throw new NotSupportedException("specific stream is not a WAVE file");
                }
                int riff_chunk_size = reader.ReadInt32();

                string format = new string(reader.ReadChars(4));
                if (format != "WAVE")
                {
                    throw new NotSupportedException("Specific streeam is not a WAVE file");
                }
                string format_signiture = new string(reader.ReadChars(4));
                if (format_signiture != "fmt ")
                {
                    throw new NotSupportedException("specified WAVE fileis not supported");
                }
                int format_chunk_size = reader.ReadInt32();
                int audio_format = reader.ReadInt16();
                int num_channels = reader.ReadInt16();
                int sample_rate = reader.ReadInt32();
                int byte_rate = reader.ReadInt32();
                int block_align = reader.ReadInt16();
                int bits_per_sample = reader.ReadInt16();

                string data_signiture = new string(reader.ReadChars(4));
                if (data_signiture != "data")
                {
                    throw new NotSupportedException("specified WAVE file is not supported");
                }

                int data_chunk_size = reader.ReadInt32();

                channels = num_channels;
                bits = bits_per_sample;
                rate = sample_rate;

                return reader.ReadBytes((int)reader.BaseStream.Length);
            }
        }
    }
}
