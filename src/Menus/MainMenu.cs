﻿
namespace ShooterGame
{
    public class MainMenu : Menu
    {
        Button _host = new Button("Host Game", 10, 100, 200, 20);
        Button _join = new Button("Join Game", 10, 125, 200, 20);
        Button _exit = new Button("Exit", 10, 150, 200, 20);

        /// <summary>
        /// Check for user input and run any other updates.
        /// </summary>
        public override void Update()
        {
            _host.Update();
            _join.Update();
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
