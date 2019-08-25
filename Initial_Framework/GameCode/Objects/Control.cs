using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Input;

namespace OpenGL_Game.Objects
{
    class Control
    {
        private static List<Key> keysDown;
        private static List<Key> keysDownLast;

        public void Initialize(GameWindow game)
        {
            keysDown = new List<Key>();
            keysDownLast = new List<Key>();
            game.KeyDown += game_KeyDown;
            game.KeyUp += game_KeyUp;
           
            
        }
        public void game_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (!keysDown.Contains(e.Key))
                keysDown.Add(e.Key);
        }
        static void game_KeyUp(object sender, KeyboardKeyEventArgs e)
        {
            while (keysDown.Contains(e.Key))
                keysDown.Remove(e.Key);
        }

        public static bool KeyPress(Key key)
        {
            return (keysDown.Contains(key) && !keysDownLast.Contains(key));
        }
        public static bool KeyRelease(Key key)
        {
            return (!keysDown.Contains(key) && keysDownLast.Contains(key));
        }
        public bool KeyDown(Key key)
        {
            return (keysDown.Contains(key));
        }
    }
}

