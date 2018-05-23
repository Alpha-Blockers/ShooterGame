using static SwinGameSDK.SwinGame;

namespace ShooterGame
{
    public class LobbyMenu : Menu
    {
        private const int WIDTH = 200;
        private const int HEIGHT = 20;
        private const int PADDING = 10;

        Button _back;
        Button _start;

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

            // Create start button
            x = ScreenWidth() - PADDING - WIDTH;
            _start = new Button("Start", x, y, WIDTH, HEIGHT);
        }

        /// <summary>
        /// Check for user input and run any other updates.
        /// </summary>
        public override void Update()
        {
            // Check if going back to main menu
            if (_back.Update()) Current = new MainMenu();

            // Game should start
            if (_start.Update()) Current = new MainMenu();
        }

        /// <summary>
        /// Draw the menu to the screen.
        /// </summary>
        public override void Draw()
        {
            _back.Draw();
            _start.Draw();
        }
    }
}
