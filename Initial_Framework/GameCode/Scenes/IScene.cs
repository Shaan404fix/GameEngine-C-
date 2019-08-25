using OpenTK;

namespace OpenGL_Game.Scenes
{
    enum SceneChange
    {
        Main_Window,
        Game_Window,
        GameOver_Window
    }
    interface IScene
    {
        void Render(FrameEventArgs e);
        void Update(FrameEventArgs e);
        void Close();
    }
}
