using SwinGameSDK;
using static SwinGameSDK.SwinGame;

namespace ShooterGame
{
    public class LobbyMenu : Menu
    {
        public const int PLAYER_COUNT = 4;

        private const int WIDTH = 240;
        private const int HEIGHT = 20;
        private const int PADDING = 10;

        bool _failure;
        Button _back;
        Button _start;

        /// <summary>
        /// Lobby menu constructor.
        /// </summary>
        public LobbyMenu(string serverAddress)
        {
            // Set failure trigger
            _failure = false;

            // Make sure a message log exists
            if (MessageLog.Current == null)
                MessageLog.Current = new MessageLog();

            // Get coordinates for buttons
            int width = (WIDTH - PADDING) / 2;
            int x = (ScreenWidth() - WIDTH) / 2;
            int y = (ScreenHeight() - HEIGHT) / 2;

            // Create back button
            _back = new Button("Back", x, y, width, HEIGHT);

            // Create start button
            x += width + PADDING;
            _start = new Button("Start", x, y, width, HEIGHT);

            // Setup network
            // This will also setup the player list
            try
            {
                if (serverAddress != null)
                {
                    System.Console.WriteLine("joining " + serverAddress);
                    NetworkController.Current = new NetworkClient(serverAddress);
                    System.Console.WriteLine("joined " + serverAddress);
                }
                else
                {
                    System.Console.WriteLine("hosting");
                    NetworkController.Current = new NetworkServer(PLAYER_COUNT);
                    Player.LocalPlayerName = "Host";
                }
                Player.SetLocalPlayerIndex(0);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Lobby error: " + e);
                _failure = true;
            }
        }

        /// <summary>
        /// Go back to the main menu.
        /// </summary>
        private void GoBack()
        {
            NetworkController.Current = null;
            Player.TerminatePlayers();
            Current = new MainMenu();
        }

        /// <summary>
        /// Check for user input and run any other updates.
        /// </summary>
        public override void Update()
        {
            // Check for failure
            if (_failure) GoBack();

            // Check if going back to main menu
            if (_back.Update()) GoBack();

            // Game should start
            if (_start.Update()) Current = new InGameMenu();

            // Update message log
            MessageLog.Current?.Update();
        }

        /// <summary>
        /// Draw the menu to the screen.
        /// </summary>
        public override void Draw()
        {
            // Draw buttons
            _back.Draw();
            _start.Draw();

            // Draw message box
            MessageLog.Current?.Draw();
        }
    }
}
