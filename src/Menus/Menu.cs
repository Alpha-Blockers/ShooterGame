using SwinGameSDK;

namespace ShooterGame
{
    public abstract class Menu
    {
        private static Menu _current = new MainMenu();
        private static Font _font = SwinGame.LoadFontNamed("menu", "cour.ttf", 16);

        /// <summary>
        /// Access current active menu.
        /// </summary>
        public static Menu Current
        {
            get => _current;
            set { _current = value; }
        }

        /// <summary>
        /// Check for user input and run any other updates.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Draw the menu to the screen.
        /// </summary>
        public abstract void Draw();
    }
}
