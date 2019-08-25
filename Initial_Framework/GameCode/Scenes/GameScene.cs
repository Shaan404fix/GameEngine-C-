using System;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenGL_Game.Components;
using OpenGL_Game.Systems;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;


namespace OpenGL_Game.Scenes
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    class GameScene : Scene 
    {
        public Matrix4 view, projection;
        public static float dt = 0;
        public static bool endGame = false;
        public static int Lives = 3;
        public static Vector3 origin = new Vector3(-0.0f, 0.0f, -40.0f);
        public static Vector3 GhostOrigin = new Vector3(0.0f, 0.0f, 10.0f);

        EntityManager entityManager;
        SystemManager systemManager;
      

        public static GameScene gameInstance;

        public GameScene(SceneManager sceneManager) : base(sceneManager)
        {
            gameInstance = this;
            entityManager = new EntityManager();
            systemManager = new SystemManager();

            // Set the title of the window
            sceneManager.Title = "Game";
            // Set the Render and Update delegates to the Update and Render methods of this class
            sceneManager.renderer = Render;
            sceneManager.updater = Update;
            // Set Keyboard events to go to a method in this class
            sceneManager.Keyboard.KeyDown += Keyboard_KeyDown;

                GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            view = Matrix4.LookAt(new Vector3(0, 40, 100), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45), 800f / 480f, 0.01f, 70000f);


            CreateEntities();
            CreateSystems();

            // TODO: Add your initialization logic here
        }
 
        private void CreateEntities()
        {
            Entity newEntity;
            
            newEntity = new Entity("Pac-man");
            newEntity.AddComponent(new ComponentPosition(-0.0f, 0.0f, -40.0f));
            newEntity.AddComponent(new ComponentVelocity(0.0f ,0.0f, 0.0f));
            newEntity.AddComponent(new ComponentScale(0.12f, 0.12f, 0.12f));
            newEntity.AddComponent(new ComponenetRotation(MathHelper.DegreesToRadians(90), 'Y'));
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentControl(sceneManager));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("Ghost");
            newEntity.AddComponent(new ComponentPosition(0.0f, 0.0f, 10.0f));
            newEntity.AddComponent(new ComponentVelocity(0.0f, 0.0f, 0.0f));
            newEntity.AddComponent(new ComponentScale(0.12f, 0.12f, 0.12f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentTexture("Textures/Ghost.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/pac_die.wav",false));
            newEntity.AddComponent(new ComponentAi());
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("Wall1");
            newEntity.AddComponent(new ComponentPosition(0.0f, 0.0f, 49.0f));
            newEntity.AddComponent(new ComponentScale(1f, 1f, 1f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/SquareGeometry.txt"));
            newEntity.AddComponent(new ComponentCollision("wall", "WallH"));
            newEntity.AddComponent(new ComponentTexture("Textures/Wall.png"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("Wall2");
            newEntity.AddComponent(new ComponentPosition(0.0f, 0.0f, -51.0f));
            newEntity.AddComponent(new ComponentScale(1f, 1f, 1f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/SquareGeometry.txt"));
            newEntity.AddComponent(new ComponentCollision("wall", "WallH"));
            newEntity.AddComponent(new ComponentTexture("Textures/Wall.png"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("Wall3");
            newEntity.AddComponent(new ComponentPosition(-51.0F, 0.0f, 0.0f));
            newEntity.AddComponent(new ComponentScale(1f, 1f, 1f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/SquareGeometry2.txt"));
            newEntity.AddComponent(new ComponentCollision("wall", "WallV"));
            newEntity.AddComponent(new ComponentTexture("Textures/Wall.png"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("Wall4");
            newEntity.AddComponent(new ComponentPosition(+49.0F, 0.0f, 0.0F));
            newEntity.AddComponent(new ComponentScale(1.0f, 1.0f, 1.0f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/SquareGeometry2.txt"));
            newEntity.AddComponent(new ComponentCollision("wall", "WallV"));
            newEntity.AddComponent(new ComponentTexture("Textures/Wall.png"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("Wall5");
            newEntity.AddComponent(new ComponentPosition(+20.0F, 0.0f, -0F));
            newEntity.AddComponent(new ComponentScale(1.0f, 1.0f, 1.0f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/SquareGeometry5.txt"));
            newEntity.AddComponent(new ComponentCollision("wall", "WallVT"));
            newEntity.AddComponent(new ComponentTexture("Textures/Wall.png"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("Wall6");
            newEntity.AddComponent(new ComponentPosition(20.0F, 0.0f, 49F));
            newEntity.AddComponent(new ComponentScale(1.0f, 1.0f, 1.0f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/SquareGeometry5.txt"));
            newEntity.AddComponent(new ComponentCollision("wall", "WallVT"));
            newEntity.AddComponent(new ComponentTexture("Textures/Wall.png"));
            entityManager.AddEntity(newEntity);


            newEntity = new Entity("Wall7");
            newEntity.AddComponent(new ComponentPosition(-20.0F, 0.0f, 49F));
            newEntity.AddComponent(new ComponentScale(1.0f, 1.0f, 1.0f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/SquareGeometry5.txt"));
            newEntity.AddComponent(new ComponentCollision("wall", "WallVT"));
            newEntity.AddComponent(new ComponentTexture("Textures/Wall.png"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("Wall8");
            newEntity.AddComponent(new ComponentPosition(-30.0F, 0.0f, 49F));
            newEntity.AddComponent(new ComponentScale(1.0f, 1.0f, 1.0f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/SquareGeometry5.txt"));
            newEntity.AddComponent(new ComponentCollision("wall", "WallVT"));
            newEntity.AddComponent(new ComponentTexture("Textures/Wall.png"));
            entityManager.AddEntity(newEntity);


            newEntity = new Entity("Wall9");
            newEntity.AddComponent(new ComponentPosition(-40.0F, 0.0f, 49F));
            newEntity.AddComponent(new ComponentScale(1.0f, 1.0f, 1.0f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/SquareGeometry5.txt"));
            newEntity.AddComponent(new ComponentCollision("wall", "WallVT"));
            newEntity.AddComponent(new ComponentTexture("Textures/Wall.png"));
            entityManager.AddEntity(newEntity);


            newEntity = new Entity("Wall10");
            newEntity.AddComponent(new ComponentPosition(30.0F, 0.0f, 49F));
            newEntity.AddComponent(new ComponentScale(1.0f, 1.0f, 1.0f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/SquareGeometry5.txt"));
            newEntity.AddComponent(new ComponentCollision("wall", "WallVT"));
            newEntity.AddComponent(new ComponentTexture("Textures/Wall.png"));
            entityManager.AddEntity(newEntity);


            newEntity = new Entity("Wall11");
            newEntity.AddComponent(new ComponentPosition(40.0F, 0.0f, 49F));
            newEntity.AddComponent(new ComponentScale(1.0f, 1.0f, 1.0f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/SquareGeometry5.txt"));
            newEntity.AddComponent(new ComponentCollision("wall", "WallVT"));
            newEntity.AddComponent(new ComponentTexture("Textures/Wall.png"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("Wall12");
            newEntity.AddComponent(new ComponentPosition(-20.0F, 0.0f, -0F));
            newEntity.AddComponent(new ComponentScale(1.0f, 1.0f, 1.0f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/SquareGeometry5.txt"));
            newEntity.AddComponent(new ComponentCollision("wall", "WallVT"));
            newEntity.AddComponent(new ComponentTexture("Textures/Wall.png"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("Wall13");
            newEntity.AddComponent(new ComponentPosition(-30.0F, 0.0f, -0F));
            newEntity.AddComponent(new ComponentScale(1.0f, 1.0f, 1.0f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/SquareGeometry5.txt"));
            newEntity.AddComponent(new ComponentCollision("wall", "WallVT"));
            newEntity.AddComponent(new ComponentTexture("Textures/Wall.png"));
            entityManager.AddEntity(newEntity);


            newEntity = new Entity("Wall14");
            newEntity.AddComponent(new ComponentPosition(-40.0F, 0.0f, -0F));
            newEntity.AddComponent(new ComponentScale(1.0f, 1.0f, 1.0f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/SquareGeometry5.txt"));
            newEntity.AddComponent(new ComponentCollision("wall", "WallVT"));
            newEntity.AddComponent(new ComponentTexture("Textures/Wall.png"));
            entityManager.AddEntity(newEntity);


            newEntity = new Entity("Wall15");
            newEntity.AddComponent(new ComponentPosition(30.0F, 0.0f, -0F));
            newEntity.AddComponent(new ComponentScale(1.0f, 1.0f, 1.0f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/SquareGeometry5.txt"));
            newEntity.AddComponent(new ComponentCollision("wall", "WallVT"));
            newEntity.AddComponent(new ComponentTexture("Textures/Wall.png"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("Wall16");
            newEntity.AddComponent(new ComponentPosition(40.0F, 0.0f, -0F));
            newEntity.AddComponent(new ComponentScale(1.0f, 1.0f, 1.0f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/SquareGeometry5.txt"));
            newEntity.AddComponent(new ComponentCollision("wall", "WallVT"));
            newEntity.AddComponent(new ComponentTexture("Textures/Wall.png"));
            entityManager.AddEntity(newEntity);

            

            //newEntity = new Entity("Center4");
            //newEntity.AddComponent(new ComponentPosition(-0.0f, 0.0f, -5.0F));
            //newEntity.AddComponent(new ComponentScale(1.0f, 1.0f, 1.0f));
            //newEntity.AddComponent(new ComponenetRotation());
            //newEntity.AddComponent(new ComponentGeometry("Geometry/SquareGeometry4.txt"));
            //newEntity.AddComponent(new ComponentCollision("wall", "WallH"));
            //newEntity.AddComponent(new ComponentTexture("Textures/Wall.png"));
            //entityManager.AddEntity(newEntity);

            


            newEntity = new Entity("Center5");
            newEntity.AddComponent(new ComponentPosition(-0.0f, 0.0f, 15.0F));
            newEntity.AddComponent(new ComponentScale(1.0f, 1.0f, 1.0f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/SquareGeometry4.txt"));
            newEntity.AddComponent(new ComponentCollision("wall", "WallH"));
            newEntity.AddComponent(new ComponentTexture("Textures/Wall.png"));
            entityManager.AddEntity(newEntity);


            newEntity = new Entity("food1");
            newEntity.AddComponent(new ComponentPosition(-45.0f, 0.0f, -10.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav",false));
            newEntity.AddComponent(new ComponentControl(sceneManager));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food2");
            newEntity.AddComponent(new ComponentPosition(-45.0f, 0.0f, -20.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food3");
            newEntity.AddComponent(new ComponentPosition(-45.0f, 0.0f, -30.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food4");
            newEntity.AddComponent(new ComponentPosition(-45.0f, 0.0f, -40.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food5");
            newEntity.AddComponent(new ComponentPosition(-35.0f, 0.0f, -10.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food6");
            newEntity.AddComponent(new ComponentPosition(-35.0f, 0.0f, -20.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food7");
            newEntity.AddComponent(new ComponentPosition(-35.0f, 0.0f, -30.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food8");
            newEntity.AddComponent(new ComponentPosition(-35.0f, 0.0f, -40.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food9");
            newEntity.AddComponent(new ComponentPosition(-25.0f, 0.0f, -10.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food10");
            newEntity.AddComponent(new ComponentPosition(-25.0f, 0.0f, -20.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food11");
            newEntity.AddComponent(new ComponentPosition(-25.0f, 0.0f, -30.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food12");
            newEntity.AddComponent(new ComponentPosition(-25.0f, 0.0f, -40.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food13");
            newEntity.AddComponent(new ComponentPosition(25.0f, 0.0f, -10.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food14");
            newEntity.AddComponent(new ComponentPosition(25.0f, 0.0f, -20.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food15");
            newEntity.AddComponent(new ComponentPosition(25.0f, 0.0f, -30.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food16");
            newEntity.AddComponent(new ComponentPosition(25.0f, 0.0f, -40.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food17");
            newEntity.AddComponent(new ComponentPosition(35.0f, 0.0f, -10.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food18");
            newEntity.AddComponent(new ComponentPosition(35.0f, 0.0f, -20.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food19");
            newEntity.AddComponent(new ComponentPosition(35.0f, 0.0f, -30.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food20");
            newEntity.AddComponent(new ComponentPosition(35.0f, 0.0f, -40.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food21");
            newEntity.AddComponent(new ComponentPosition(45.0f, 0.0f, -10.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food22");
            newEntity.AddComponent(new ComponentPosition(45.0f, 0.0f, -20.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));

            entityManager.AddEntity(newEntity);
            newEntity = new Entity("food23");
            newEntity.AddComponent(new ComponentPosition(45.0f, 0.0f, -30.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food24");
            newEntity.AddComponent(new ComponentPosition(45.0f, 0.0f, -40.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food25");
            newEntity.AddComponent(new ComponentPosition(-45.0f, 0.0f, 10.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food26");
            newEntity.AddComponent(new ComponentPosition(-45.0f, 0.0f, 20.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food27");
            newEntity.AddComponent(new ComponentPosition(-45.0f, 0.0f, 30.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food28");
            newEntity.AddComponent(new ComponentPosition(-45.0f, 0.0f, 40.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food29");
            newEntity.AddComponent(new ComponentPosition(-35.0f, 0.0f, 10.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food30");
            newEntity.AddComponent(new ComponentPosition(-35.0f, 0.0f, 20.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food31");
            newEntity.AddComponent(new ComponentPosition(-35.0f, 0.0f, 30.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food32");
            newEntity.AddComponent(new ComponentPosition(-35.0f, 0.0f, 40.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food34");
            newEntity.AddComponent(new ComponentPosition(-25.0f, 0.0f, 10.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food35");
            newEntity.AddComponent(new ComponentPosition(-25.0f, 0.0f, 20.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food36");
            newEntity.AddComponent(new ComponentPosition(-25.0f, 0.0f, 30.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food37");
            newEntity.AddComponent(new ComponentPosition(-25.0f, 0.0f, 40.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food38");
            newEntity.AddComponent(new ComponentPosition(25.0f, 0.0f, 10.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food39");
            newEntity.AddComponent(new ComponentPosition(25.0f, 0.0f, 20.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food40");
            newEntity.AddComponent(new ComponentPosition(25.0f, 0.0f, 30.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food41");
            newEntity.AddComponent(new ComponentPosition(25.0f, 0.0f, 40.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food42");
            newEntity.AddComponent(new ComponentPosition(35.0f, 0.0f, 10.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food43");
            newEntity.AddComponent(new ComponentPosition(35.0f, 0.0f, 20.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food44");
            newEntity.AddComponent(new ComponentPosition(35.0f, 0.0f, 30.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food45");
            newEntity.AddComponent(new ComponentPosition(35.0f, 0.0f, 40.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food46");
            newEntity.AddComponent(new ComponentPosition(45.0f, 0.0f, 10.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food47");
            newEntity.AddComponent(new ComponentPosition(45.0f, 0.0f, 20.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food48");
            newEntity.AddComponent(new ComponentPosition(45.0f, 0.0f, 30.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("food49");
            newEntity.AddComponent(new ComponentPosition(45.0f, 0.0f, 40.0f));
            newEntity.AddComponent(new ComponentScale(0.015f, 0.015f, 0.015f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("food"));
            newEntity.AddComponent(new ComponentTexture("Textures/skin.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/eatball.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("Sfood1");
            newEntity.AddComponent(new ComponentPosition(45.0f, 0.0f, 45.0f));
            newEntity.AddComponent(new ComponentScale(0.04f, 0.04f, 0.04f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("power"));
            newEntity.AddComponent(new ComponentTexture("Textures/Ghost.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/SFX_Powerup_35.wav", true));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("Sfood2");
            newEntity.AddComponent(new ComponentPosition(-45.0f, 0.0f, 45.0f));
            newEntity.AddComponent(new ComponentScale(0.04f, 0.04f, 0.04f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("power"));
            newEntity.AddComponent(new ComponentTexture("Textures/Ghost.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/SFX_Powerup_35.wav", true));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("Sfood3");
            newEntity.AddComponent(new ComponentPosition(45.0f, 0.0f, -45.0f));
            newEntity.AddComponent(new ComponentScale(0.04f, 0.04f, 0.04f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("power"));
            newEntity.AddComponent(new ComponentTexture("Textures/Ghost.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/SFX_Powerup_35.wav", true));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("Sfood4");
            newEntity.AddComponent(new ComponentPosition(-45.0f, 0.0f, -45.0f));
            newEntity.AddComponent(new ComponentScale(0.04f, 0.04f, 0.04f));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
            newEntity.AddComponent(new ComponentCollision("power"));
            newEntity.AddComponent(new ComponentTexture("Textures/Ghost.png"));
            newEntity.AddComponent(new ComponentAudio("Audio/SFX_Powerup_35.wav", true));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("SkyBox");
            newEntity.AddComponent(new ComponentPosition(0.0f, 0.0f, 0.0f));
            newEntity.AddComponent(new ComponentScale(1, 1, 1));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/SkyBox.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/Sky.png"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("Ground");
            newEntity.AddComponent(new ComponentPosition(0.0f, 0.0f, 0.0f));
            newEntity.AddComponent(new ComponentScale(1, 1, 1));
            newEntity.AddComponent(new ComponenetRotation());
            newEntity.AddComponent(new ComponentGeometry("Geometry/Ground.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/Ground.png"));
            entityManager.AddEntity(newEntity);




        }

        private void CreateSystems()
        {
            ISystem newSystem;

            newSystem = new SystemRender();
            systemManager.AddSystem(newSystem);

            newSystem = new SystemControl();
            systemManager.AddSystem(newSystem);

            newSystem = new SystemPhysics();
            systemManager.AddSystem(newSystem);

            newSystem = new SystemAudio();
            systemManager.AddSystem(newSystem);

            //newSystem = new SystemAi();
            //systemManager.AddSystem(newSystem);

        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="e">Provides a snapshot of timing values.</param>
        public override void Update(FrameEventArgs e)
        {
            if (GamePad.GetState(1).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Key.Escape))
                sceneManager.Exit();

            dt = (float)e.Time;

            // TODO: Add your update logic here
            if (endGame || Lives == 0)
            {
                sceneManager.changeScene(SceneChange.GameOver_Window);
                Close();
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="e">Provides a snapshot of timing values.</param>
        public override void Render(FrameEventArgs e)
        {
            GL.Viewport(0, 0, sceneManager.Width, sceneManager.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //float width = sceneManager.Width, height = sceneManager.Height, fontSize = Math.Min(width, height) / 10f;
            //GUI.Label(new Rectangle(0, (int)(fontSize / 2f), (int)width, (int)(fontSize * 2f)), "Lives = ", (int)fontSize, StringAlignment.Center);

           

            systemManager.ActionSystems(entityManager);
   
        }

        /// <summary>
        /// This is called when the game exits.
        /// </summary>
        public override void Close()
        {
            entityManager.Close();
        }

        public void Keyboard_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
               
                case Key.Up:
                    view = view * Matrix4.CreateTranslation(0.0f, 0.0f, 5);
                    break;
                case Key.Left:

                    view = view * Matrix4.CreateRotationY(-0.025f);
                    break;
                case Key.Down:
                    view = view * Matrix4.CreateTranslation(0.0f, 0.0f, -5f);
                    break;
                case Key.Right:
                    view = view * Matrix4.CreateRotationY(0.025f);
                    break;
                case Key.L:
                    view = view * Matrix4.CreateTranslation(-5, 0.0f, 0.0f);
                    break;
                case Key.J:
                    view = view * Matrix4.CreateTranslation(5, 0.0f, 0.0f);
                    break;
                case Key.I:
                    view = view * Matrix4.CreateTranslation(0.0f, 5, 0.0f);
                    break;
                case Key.K:
                    view = view * Matrix4.CreateTranslation(0.0f, -5f, 0.0f);
                    break;

                case Key.G:
                    view = view * Matrix4.CreateRotationX(0.05f);
                    break;
                case Key.V:
                    view = view * Matrix4.CreateRotationX(-0.05f);
                    break;
            
             
                    
            }
        }
    }
}
