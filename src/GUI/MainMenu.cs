using static SwinGameSDK.SwinGame;

namespace ShooterGame
{
    public class MainMenu : Menu
    {
        private const int WIDTH = 200;
        private const int HEIGHT = 20;

        private Button _host;
        private Button _join;
        private Button _exit;

        public MainMenu()
        {
            // Get x-coordinates for buttons
            int x = (ScreenWidth() - WIDTH) / 2;

            // Create host button
            int y = 50;
            _host = new Button("Host Game", x, y, WIDTH, HEIGHT);

            // Create join button
            y += HEIGHT + 2;
            _join = new Button("Join Game", x, y, WIDTH, HEIGHT);

            // Create exit button
            y += HEIGHT + 10;
            _exit = new Button("Exit", x, y, WIDTH, HEIGHT);
        }

        /// <summary>
        /// Check for user input and run any other updates.
        /// </summary>
        public override void Update()
        {
            // Check if hosting a game
            if (_host.Update()) Current = new LobbyMenu(null);

            // Check if joining a game
            if (_join.Update()) Current = new JoinMenu();

            // Check if program should close
            if (_exit.Update()) GameMain.Shutdown = true;
        }

        /// <summary>
        /// Draw the menu to the screen.
        /// </summary>
        public override void Draw()
        {
            _host.Draw();
            _join.Draw();
            _exit.Draw();
        }
    }
}
