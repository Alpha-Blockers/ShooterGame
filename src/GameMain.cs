using System;
using SwinGameSDK;
using static SwinGameSDK.SwinGame;

namespace ShooterGame
{
    public class GameMain
    {
        public static bool _shutdown;
        public static int _score;

        /// <summary>
        /// Get or set if the program should shutdown.
        /// </summary>
        public static bool Shutdown
        {
            get => _shutdown;
            set { _shutdown = value; }
        }

        /// <summary>
        /// Get or set score value.
        /// </summary>
        public static int Score
        {
            get => _score;
            set { _score = value; }
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

            int enemySpawnCounter = 0;
            Random rnd = new Random();

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

                // Set camera before drawing world
                // Also set before running update in case update methods use camera position
                PositionComponent position = player.Position;
                if (position != null)
                {
                    point.X = position.X - (ScreenWidth() / 2);
                    point.Y = position.Y - (ScreenHeight() / 2);
                    SetCameraPos(point);
                }

                // Update game
                Entity.UpdateAll();
                Menu.Current?.Update();
                NetworkController.Current?.Update();

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
                DrawText("Score: " + Score, Color.Black, 5, ScreenHeight() - 20);
                DrawFramerate(0, 0);

                // Spawn enemies
                if (enemySpawnCounter > 0)
                {
                    enemySpawnCounter -= 1;
                } else
                {
                    enemySpawnCounter = 60;
                    int px = (2 * Tile.Width) + (int)(rnd.NextDouble() * (map.Width - (4 * Tile.Width)));
                    int py = (2 * Tile.Height) + (int)(rnd.NextDouble() * (map.Height - (4 * Tile.Height)));
                    int mx = (int)(rnd.NextDouble() * 4) - 2;
                    int my = (int)(rnd.NextDouble() * 4) - 2;
                    new Entity
                    {
                        Position = new PositionComponent(map, px, py),
                        Movement = new MovementComponent(mx, my),
                        Drawable = new DrawableComponent(Color.Red, 12),
                        Collision = new CollisionComponent(8),
                        Health = new HealthComponent(1)
                    };
                }

                // Refresh screen and wait to control the frame rate and tick rate
                RefreshScreen();
                ticker.Wait();
            }
        }
    }
}
