using static SwinGameSDK.SwinGame;

namespace ShooterGame
{
    public class LobbyMenu : Menu
    {
        private const int WIDTH = 200;
        private const int HEIGHT = 20;

        Button _back;

        /// <summary>
        /// Lobby menu constructor.
        /// </summary>
        public LobbyMenu()
        {
            // Get coordinates for buttons
            int x = (ScreenWidth() - WIDTH) / 2;
            int y = 50;

            // Create back button
            _back = new Button("Back", x, y, WIDTH, HEIGHT);
        }

        /// <summary>
        /// Check for user input and run any other updates.
        /// </summary>
        public override void Update()
        {
            // Check if going back to main menu
            if (_back.Update()) Current = new MainMenu();
        }

        /// <summary>
        /// Draw the menu to the screen.
        /// </summary>
        public override void Draw()
        {
            _back.Draw();
        }
    }
}
