using SwinGameSDK;
using static SwinGameSDK.SwinGame;

namespace ShooterGame
{
    public class LobbyMenu : Menu
    {
        private const int WIDTH = 240;
        private const int HEIGHT = 20;
        private const int PADDING = 10;

        Button _back;
        Button _start;
        Rectangle _tempBox;

        /// <summary>
        /// Lobby menu constructor.
        /// </summary>
        public LobbyMenu()
        {
            // Setup player list
            Player.InitPlayers(8);
            Player.SetLocalPlayerIndex(0);

            // Make sure a message log exists
            if (MessageLog.Current == null)
                MessageLog.Current = new MessageLog();

            // Get coordinates for buttons
            int width = (WIDTH - PADDING) / 2;
            int height = Player.Count * (HEIGHT + PADDING);
            int x = (ScreenWidth() - WIDTH) / 2;
            int y = ((ScreenHeight() - height) / 2) + height + PADDING;

            // Create back button
            _back = new Button("Back", x, y, width, HEIGHT);

            // Create start button
            x += width + PADDING;
            _start = new Button("Start", x, y, width, HEIGHT);

            // Create temp box
            _tempBox = new Rectangle();
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

            // Setup initial format of temp box
            _tempBox.X = (ScreenWidth() - WIDTH) / 2;
            _tempBox.Y = (ScreenHeight() - ((Player.Count + 1) * (HEIGHT + PADDING))) / 2;
            _tempBox.Width = WIDTH;
            _tempBox.Height = HEIGHT;

            // Draw player names
            for (int i = 0; i < Player.Count; i++)
            {
                // Draw border
                DrawRectangle(Color.Gray, _tempBox);

                // Adjust temp box
                _tempBox.X -= 1;
                _tempBox.Y -= 1;
                _tempBox.Width -= 2;
                _tempBox.Height -= 2;

                // Get player
                Player p = Player.GetByIndex(i);

                // Draw player name
                DrawText(p.Name, p.Color, Color.White, Textbox.Font, FontAlignment.AlignCenter, _tempBox);

                // Adjust temp box
                _tempBox.X += 1;
                _tempBox.Y += 1 + HEIGHT + PADDING;
                _tempBox.Width += 2;
                _tempBox.Height += 2;
            }
        }
    }
}
