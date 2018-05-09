using System;
using SwinGameSDK;
using static SwinGameSDK.SwinGame; // requires mcs version 4+,
// using SwinGameSDK.SwinGame; // requires mcs version 4+,

namespace ShooterGame
{
    public class GameMain
    {
        public static void Main()
        {
            // Open the game window
            OpenGraphicsWindow("GameMain", 800, 600);

            // Declare variables
            Map map = new Map(6, 6);
            Point2D point = new Point2D();
            TickRateController ticker = new TickRateController(60);
            //NetworkServer server = new NetworkServer();
            //NetworkClient client = new NetworkClient("localhost");

            // Create test entity
            Entity player = new Entity
            {
                Position = new PositionComponent(map, 100, 100),
                Drawable = new DrawableComponent(),
                Movement = new MovementComponent(),
                Controller = new PlayerControllerComponent()
            };

            // Run the game loop
            ticker.Reset();
            while ((false == WindowCloseRequested()) && !KeyTyped(KeyCode.EscapeKey))
            {
                // Fetch the next batch of UI interaction
                ProcessEvents();

                // Clear the screen
                ClearScreen(Color.White);

                /*
                int y = 50;
                foreach (ChatPacket c in MessageLog.Messages)
                {
                    DrawText("> " + c.ToString(), Color.Black, 30, y);
                    y += 20;
                }
                */


                // Update game
                UpdateController.Flush();
                
                //server.Update();
                //client.Update();

                /*
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
                */
                // Draw interface
                DrawFramerate(0, 0);

                // Refresh screen and wait to control the frame rate and tick rate
                RefreshScreen();
                ticker.Wait();
            }
        }
    }
}
