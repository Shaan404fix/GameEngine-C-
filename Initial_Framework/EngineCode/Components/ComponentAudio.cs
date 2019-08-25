using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Components
{
    class ComponentAudio : IComponent
    {
        int mySource;
        Vector3 listenerPosition;
        Vector3 listenerDirection;
        Vector3 listenerUp;
       static AudioContext audioContext;

        public ComponentAudio(string audioName, bool loop)
        {

             listenerUp = Vector3.UnitY;

            if (audioContext == null)
            {
                audioContext = new AudioContext();
            }
            int myBuffer = ResourceManager.LoadWav(audioName);

            // Create a sounds source using the audio clip
            mySource = AL.GenSource(); // gen a Source Handle
            AL.Source(mySource, ALSourcei.Buffer, myBuffer); // attach the buffer to a source
            AL.Source(mySource, ALSourceb.Looping, loop); // source loops infinitely
        }
        public void SetPosition(Vector3 emitterPosition)
        {
            AL.Source(mySource, ALSource3f.Position, ref emitterPosition);

            AL.Listener(ALListener3f.Position, ref emitterPosition);
            AL.Listener(ALListenerfv.Orientation, ref emitterPosition, ref listenerUp);
        }

        public void Start()
        {
            AL.SourcePlay(mySource);
        }

        public void Stop()
        {
            AL.SourceStop(mySource);
        }

        public int Audio
        {
            get { return mySource; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_AUDIO; }
        }

        public void Close()
        {
            Stop();
            AL.DeleteSource(mySource);
        }

    }
}