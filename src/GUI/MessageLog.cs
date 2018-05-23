using System.Collections.Generic;
using SwinGameSDK;
using static SwinGameSDK.SwinGame;

namespace ShooterGame
{
    public class MessageLog
    {
        private const int MAX_MESSAGES = 10;
        private const int MENU_WIDTH = 300;
        private const int MENU_HEIGHT = 116;
        private const int EDGE_PADDING = 5;

        public static MessageLog _current = new MessageLog();

        private List<ChatPacket> _messages;
        private Textbox _textbox;
        private Rectangle _location;
        private Rectangle _locationBackground;
        private Rectangle _tempBox;

        /// <summary>
        /// Access current global message log.
        /// </summary>
        public static MessageLog Current
        {
            get => _current;
            set { _current = value; }
        }

        /// <summary>
        /// Message log constructor.
        /// </summary>
        public MessageLog()
        {
            // Define location
            _location = new Rectangle
            {
                X = EDGE_PADDING,
                Y = ScreenHeight() - EDGE_PADDING - MENU_HEIGHT,
                Width = MENU_WIDTH,
                Height = MENU_HEIGHT
            };

            // Setup message list
            _messages = new List<ChatPacket>();

            // Get height of text box
            float TextboxHeight = Textbox.FontSize + 2;

            // Setup location of background (excludes textbox space)
            _locationBackground = new Rectangle
            {
                X = Location.X + 1,
                Y = Location.Y + 1,
                Width = Location.Width - 2,
                Height = Location.Height - 2 - TextboxHeight,
            };

            // Setup textbox
            _textbox = new Textbox(
                _locationBackground.X,
                _locationBackground.Y + _locationBackground.Height,
                _locationBackground.Width,
                TextboxHeight,
                128);

            // Create temp rectangle box
            _tempBox = new Rectangle();
        }

        /// <summary>
        /// Get location of message log
        /// </summary>
        public Rectangle Location { get => _location; }
        
        /// <summary>
        /// Get internal message list.
        /// </summary>
        public List<ChatPacket> Messages { get => _messages; }

        /// <summary>
        /// Add a message to the local message list. Does not relay to network in any form.
        /// </summary>
        /// <param name="m">The message to add</param>
        public void Add(ChatPacket m)
        {
            // Remove a message if the maximum number of messages has been reached
            if (_messages.Count >= MAX_MESSAGES)
                _messages.RemoveAt(0);

            // Add new message
            _messages.Add(m);
        }

        /// <summary>
        /// Add a message to the local message list. Does not relay to network in any form.
        /// This version of add is intended for debugging only.
        /// </summary>
        /// <param name="s">String to be added to the message list.</param>
        public void Add(string s)
        {
            Add(new ChatPacket(null, s));
        }

        /// <summary>
        /// Get maximum number of messages a message log can store.
        /// </summary>
        public static int CountMax { get => MAX_MESSAGES; }

        /// <summary>
        /// Get number of messages within the message log.
        /// </summary>
        public int Count { get { return _messages.Count; } }

        /// <summary>
        /// Clear all messages from the message log.
        /// </summary>
        public void Clear()
        {
            _messages.Clear();
        }

        /// <summary>
        /// Check for user input and run any other updates.
        /// </summary>
        public void Update()
        {
            // Update internal textbox
            if (_textbox.Update())
            {
                // Get message to be sent
                string text = _textbox.Text.Trim();
                _textbox.Text = "";

                // Reselect textbox
                _textbox.MakeActive();

                // Check if there was anything to send
                if ((text != null) && (text != ""))
                {
                    // Create chat packet
                    ChatPacket packet = new ChatPacket(Player.LocalPlayer, text);

                    // Check if network exists and add packet to correct list
                    if (NetworkController.Current != null)
                        NetworkController.Current.Send(packet);
                    else
                        Add(packet);
                }
            }
        }

        /// <summary>
        /// Draw the menu to the screen.
        /// </summary>
        public void Draw()
        {
            // Get message box background colour
            Color backColor = RGBAColor(50, 50, 50, 100);

            // Draw background
            FillRectangle(backColor, _locationBackground);

            // Draw border
            DrawRectangle(Color.Black, _location);

            // Initial setup of tempBox
            _tempBox.X = _locationBackground.X;
            _tempBox.Y = _locationBackground.Bottom - Textbox.FontSize;
            _tempBox.Width = _locationBackground.Width;
            _tempBox.Height = Textbox.FontSize;

            // Draw message log
            for (int i = Count - 1; i >= 0; i--)
            {
                // Get chat packet
                ChatPacket chat = _messages[i];

                // Form string and get colour
                string text = "> " + chat.ToString();
                Color color = (chat.Player != null) ? chat.Player.Color : Color.Black;

                // Draw text
                DrawText(text, color, RGBAColor(0, 0, 0, 0), Textbox.Font, FontAlignment.AlignLeft, _tempBox);

                // Move line
                _tempBox.Y -= _tempBox.Height;
                if (_tempBox.Y < _location.Y) break;
            }

            // Draw textbox
            _textbox.Draw();
        }
    }
}
