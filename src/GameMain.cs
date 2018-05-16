using System;
using SwinGameSDK;
using static SwinGameSDK.SwinGame;

namespace ShooterGame
{
    public class GameMain
    {
        public static bool _shutdown;

        /// <summary>
        /// Get or set if the program should shutdown.
        /// </summary>
        public static bool Shutdown
        {
            get => _shutdown;
            set { _shutdown = value; }
        }

        /// <summary>
        /// Game main method.
        /// </summary>
        public static void Main()
        {
            // Open the game window
            OpenGraphicsWindow("GameMain", 800, 600);

            // Declare variables
            Map map = new Map(4, 3);
            Point2D point = new Point2D();
            TickRateController ticker = new TickRateController(60);

            Player.LocalPlayerName = "TestPlayerName";

            // Create test entity
            Entity player = new Entity
            {
                Position = new PositionComponent(map, 100, 100),
                Drawable = new DrawableComponent(Color.Blue, 12),
                Movement = new MovementComponent(),
                Controller = new PlayerControllerComponent(),
                Collision = new CollisionComponent(12)
            };

            // Run the game loop
            ticker.Reset();
            while ((false == WindowCloseRequested()) && !Shutdown)
            {
                // Fetch the next batch of UI interaction
                ProcessEvents();
                if (KeyTyped(KeyCode.EscapeKey)) Shutdown = true;

                // Clear the screen
                ClearScreen(Color.White);

                // Update game
                Entity.UpdateAll();
                Menu.Current?.Update();
                NetworkController.Current?.Update();
                
                // Set camera before drawing world
                PositionComponent position = player.Position;
                if (position != null)
                {
                    point.X = position.X - (ScreenWidth() / 2);
                    point.Y = position.Y - (ScreenHeight() / 2);
                    SetCameraPos(point);
                }

                // Draw world
                map.Draw();

                // Reset camera before drawing interface
                point.X = 0;
                point.Y = 0;
                SetCameraPos(point);

                // Draw message log
                int y = 50;
                foreach (ChatPacket c in MessageLog.Messages)
                {
                    DrawText("> " + c.ToString(), (c.Player != null) ? c.Player.Color : Color.Black, 10, y);
                    y += 20;
                }

                // Draw interface
                Menu.Current?.Draw();
                DrawFramerate(0, 0);

                // Refresh screen and wait to control the frame rate and tick rate
                RefreshScreen();
                ticker.Wait();
            }
        }
    }
}
