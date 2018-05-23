using SwinGameSDK;
using static SwinGameSDK.SwinGame;

namespace ShooterGame
{
    class Textbox
    {
        private const int TEXT_SIZE = 16;

        private static Textbox _currentActive = null;
        private static Font _font = LoadFont("cour.ttf", TEXT_SIZE);
        private static Color[] _backColor = new Color[]
        {
            RGBColor(255, 255, 255), // Normal background colour
            RGBColor(220, 220, 255), // Background colour when Hover is true
        };

        private Rectangle _textLocation;
        private Rectangle _location;
        private string _text;
        private string _ghostText;
        private int _maxLength;
        private bool _hover; // True if mouse is over this textbox

        /// <summary>
        /// Get font used with textboxes.
        /// </summary>
        public static Font Font { get => _font; }

        /// <summary>
        /// Size value used to load the font.
        /// </summary>
        public static int FontSize { get => TEXT_SIZE; }

        /// <summary>
        /// Get the current active textbox, if any
        /// </summary>
        public static Textbox CurrentActive { get => _currentActive; }

        /// <summary>
        /// Textbox constructor.
        /// </summary>
        /// <param name="x">Left edge of textbox.</param>
        /// <param name="y">Top edge of textbox.</param>
        /// <param name="width">Width of textbox.</param>
        /// <param name="height">Height of textbox.</param>
        /// <param name="maxLength">Maximum number of characters the textbox can accept.</param>
        /// <param name="ghostText">Text to be shown when no text has been entered.</param>
        /// <param name="text">Initial text within the textbox.</param>
        public Textbox(float x, float y, float width, float height, int maxLength, string ghostText = "", string text = "")
        {
            _textLocation = new Rectangle();
            _location = new Rectangle
            {
                X = x,
                Y = y,
                Width = width,
                Height = height
            };
            _hover = false;
            _text = text;
            _ghostText = ghostText;
            _maxLength = 32;
        }

        /// <summary>
        /// Check if mouse is hovering over textbox.
        /// </summary>
        public bool Hover { get => _hover; }

        /// <summary>
        /// Access location of textbox.
        /// </summary>
        public Rectangle Location { get => _location; }

        /// <summary>
        /// Get or set text.
        /// </summary>
        public string Text
        {
            get => _text;
            set { _text = value; }
        }

        /// <summary>
        /// Make this textbox inactive so that it no longer records text being entered.
        /// </summary>
        public void MakeInactive()
        {
            if (_currentActive == this)
            {
                _currentActive = null;
                _text = EndReadingText();
            }
        }

        /// <summary>
        /// Make this textbox active so that it records any text being entered.
        /// </summary>
        public void MakeActive()
        {
            if (_currentActive != this)
            {
                _currentActive?.MakeInactive();
                _currentActive = this;
                StartReadingTextWithText(_text, Color.Black, _maxLength, _font, _textLocation);
            }
        }

        /// <summary>
        /// Check for user input and run any other updates.
        /// </summary>
        /// <returns>True if textbox was activated.</returns>
        public bool Update()
        {
            // Check if mouse cursor is over the textbox
            _hover =
                (_location.Left <= MouseX()) && (MouseX() <= _location.Right) &&
                (_location.Top <= MouseY()) && (MouseY() <= _location.Bottom);

            // Check if mouse button was clicked
            // Check which textbox should be marked as the current active, if any
            if (MouseClicked(MouseButton.LeftButton))
            {
                if (_hover)
                    MakeActive();
                else
                    MakeInactive();
            }

            // Check if this textbox is currently active, but SwinGame is no longer reading text
            if ((_currentActive == this) && !ReadingText())
            {
                // Mark this textbox as inactive
                _currentActive = null;

                // Check if text entry was cancelled
                if (!TextEntryCancelled())
                {
                    // Not cancelled, so record result and return
                    _text = EndReadingText();
                    return true;
                }
            }

            // If here then nothing interesting happened
            return false;
        }

        /// <summary>
        /// Draw the menu to the screen.
        /// </summary>
        public void Draw()
        {
            // Draw textbox background
            Color bc = _backColor[_hover ? 1 : 0];
            FillRectangle(bc, _location);

            // Update textbox
            _textLocation.Width = _location.Width - 2;
            _textLocation.Height = _location.Height - 2;
            _textLocation.X = _location.X + 1;
            _textLocation.Y = _location.Y + 1 + ((_textLocation.Height - TEXT_SIZE) / 2);

            // Draw text, but only if not selected
            // If selected then SwinGame will draw the text
            if (_currentActive != this)
            {
                if ((_ghostText != null) && (_ghostText != "") && ((_text == null) || (_text == "")))
                    DrawText(_ghostText, Color.Gray, bc, _font, FontAlignment.AlignLeft, _textLocation);
                else
                    DrawText(_text, Color.Black, bc, _font, FontAlignment.AlignLeft, _textLocation);
            }
        }
    }
}
