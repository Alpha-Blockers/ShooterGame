using SwinGameSDK;
using System.Collections.Generic;
using static SwinGameSDK.SwinGame;

namespace ShooterGame
{
    public class Button
    {
        private static Font _font = LoadFont("cour.ttf", 16);
        private static Color[] _edgeColor = new Color[]
        {
            RGBColor(0, 0, 0), // Normal border colour
            RGBColor(0, 0, 100), // Border colour when Hover is true
            RGBColor(0, 0, 255) // Border colour when both Hover and Pressed are true
        };
        private static Color[] _backColor = new Color[]
        {
            RGBColor(255, 255, 255), // Normal background colour
            RGBColor(220, 220, 255), // Background colour when Hover is true
            RGBColor(100, 100, 255) // Background colour when both Hover and Pressed are true
        };

        private Rectangle _textLocation;
        private Rectangle _location;
        private string _text;
        private bool _hover; // True if mouse is over this button
        private bool _pressed; // True if mouse button is pressed while over this button

        /// <summary>
        /// Button constructor.
        /// </summary>
        /// <param name="text">Button label/text.</param>
        /// <param name="x">Left edge of button.</param>
        /// <param name="y">Top edge of button.</param>
        /// <param name="width">Width of button.</param>
        /// <param name="height">Height of button.</param>
        public Button (string text, int x, int y, int width, int height)
        {
            _textLocation = new Rectangle();
            _location = new Rectangle
            {
                X = x,
                Y = y,
                Width = width,
                Height = height
            };
            _text = text;
            _hover = false;
            _pressed = false;
        }

        /// <summary>
        /// Check if mouse is hovering over button.
        /// </summary>
        public bool Hover { get => _hover; }

        /// <summary>
        /// Check if button has been pressed by mouse button.
        /// </summary>
        public bool Pressed { get => _pressed; }

        /// <summary>
        /// Access location of button.
        /// </summary>
        public Rectangle Location { get => _location; }

        /// <summary>
        /// Get or set button text/label.
        /// </summary>
        public string Text
        {
            get => _text;
            set { _text = value; }
        }

        /// <summary>
        /// Check for user input and run any other updates.
        /// </summary>
        /// <returns>True if button was activated.</returns>
        public bool Update()
        {
            // Check if mouse cursor is over the button
            _hover =
                (_location.Left <= MouseX()) && (MouseX() <= _location.Right) &&
                (_location.Top <= MouseY()) && (MouseY() <= _location.Bottom);

            // Check if mouse is pressed
            _pressed = _hover && MouseDown(MouseButton.LeftButton);

            // Check if button was activated
            return _hover && MouseClicked(MouseButton.LeftButton);
        }

        /// <summary>
        /// Draw the menu to the screen.
        /// </summary>
        public void Draw()
        {
            // Get button mode index
            // Used for selecting colour and so forth
            int mode = _pressed ? 2 : (_hover ? 1 : 0);

            // Draw button background
            Color bc = _backColor[mode];
            FillRectangle(bc, _location);

            // Draw button border
            DrawRectangle(_edgeColor[mode], _location);

            // Update textbox
            _textLocation.Width = _location.Width - 2;
            _textLocation.Height = _location.Height - 2;
            _textLocation.X = _location.X + 1;
            _textLocation.Y = _location.Y + 2 + ((_textLocation.Height - _font.TextHeight(_text)) / 2);

            // Draw button text
            DrawText(_text, Color.Black, bc, _font, FontAlignment.AlignCenter, _textLocation);
        }
    }
}
