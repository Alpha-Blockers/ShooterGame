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

            // Setup tick-rate controller
            TickRateController ticker = new TickRateController(60);
            ticker.Reset();

            // Run the game loop
            while ((false == WindowCloseRequested()) && !Shutdown)
            {
                // Fetch the next batch of UI interaction
                ProcessEvents();
                if (KeyTyped(KeyCode.EscapeKey)) Shutdown = true;

                // Clear the screen
                ClearScreen(Color.White);

                // Update game
                Menu.Current?.Update();
                NetworkController.Current?.Update();

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
