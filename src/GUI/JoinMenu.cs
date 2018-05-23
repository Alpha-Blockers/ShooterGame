using SwinGameSDK;
using static SwinGameSDK.SwinGame;

namespace ShooterGame
{
    public class JoinMenu : Menu
    {
        private const int WIDTH = 200;
        private const int HEIGHT = 20;

        Textbox _address;
        Button _join;
        Button _back;

        /// <summary>
        /// Join menu constructor.
        /// </summary>
        public JoinMenu()
        {
            // Get x-coordinates for buttons
            int x = (ScreenWidth() - WIDTH) / 2;

            // Create host address textbox
            int y = 50;
            _address = new Textbox(x, y, WIDTH, HEIGHT, 120, "Address");

            // Create join button
            y += HEIGHT + 2;
            _join = new Button("Join", x, y, WIDTH, HEIGHT);

            // Create back button
            y += HEIGHT + 10;
            _back = new Button("Back", x, y, WIDTH, HEIGHT);
        }

        /// <summary>
        /// Check for user input and run any other updates.
        /// </summary>
        public override void Update()
        {
            // Check for join attempt
            // Update both the address textbox and the join button
            // Using '|' is the following line is correct (not a typo)
            if (_address.Update() | _join.Update()) Current = new LobbyMenu(_address.Text);

            // Check if going back to main menu
            if (_back.Update()) Current = new MainMenu();
        }

        /// <summary>
        /// Draw the menu to the screen.
        /// </summary>
        public override void Draw()
        {
            // Draw host text
            string s = "Host:";
            DrawText(
                s,
                Color.Black,
                Textbox.Font,
                _address.Location.X - Textbox.Font.TextWidth(s),
                _address.Location.Y + ((_address.Location.Height - Textbox.FontSize) / 2));

            // Draw text box
            _address.Draw();

            // Draw buttons
            _join.Draw();
            _back.Draw();
        }
    }
}
