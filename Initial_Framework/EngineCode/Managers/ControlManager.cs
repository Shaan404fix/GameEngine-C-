using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Input;
using OpenGL_Game.Objects;
namespace OpenGL_Game.Managers
{
    class ControlManager
    {
        private static List<Key> keysDown;
        private static List<Key> keysDownLast;


        public static Control Initialize(GameWindow game)
        {
            Control control = new Control();
            control.Initialize(game);
            keysDown = new List<Key>();
            keysDownLast = new List<Key>();
            game.KeyDown += game_KeyDown;
            game.KeyUp += game_KeyUp;
            return control;
        }
        static void game_KeyDown(object sender, KeyboardKeyEventArgs e)
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
        public static bool KeyDown(Key key)
        {
            return (keysDown.Contains(key));
        }
    }
}
