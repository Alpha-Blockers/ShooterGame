using System.Collections.Generic;
using SwinGameSDK;

namespace ShooterGame
{
    /// <summary>
    /// The Player class contains player data such as name and colour.
    /// This class also contains static methods for accessing and managing a static player list.
    /// </summary>
    public class Player
    {
        private static List<Player> _players = new List<Player>();
        private static Player _localPlayer;
        private static string _localPlayerName;

        private int _index;
        private string _name;
        private Color _color;
        private bool _slotUsed;

        /// <summary>
        /// Check if this player is the local player.
        /// </summary>
        public bool IsLocal { get { return this == _localPlayer; } }

        /// <summary>
        /// Get the local player (this machine in a network sense).
        /// </summary>
        public static Player LocalPlayer { get => _localPlayer; }

        /// <summary>
        /// Get or set the local players name.
        /// </summary>
        public static string LocalPlayerName
        {
            get => _localPlayerName;
            set
            {
                string name = value?.Trim();
                if ((name != null) && (name != "") && (_localPlayerName != name))
                {
                    _localPlayerName = name;
                    if ((NetworkController.Current != null) && (_localPlayer != null) && (name != ""))
                        NetworkController.Current.Send(new PlayerNamePacket(_localPlayer, name));
                }
            }
        }

        /// <summary>
        /// Access player index (location within the NetworkController player list).
        /// </summary>
        /// <remarks>This should only be changed by the NetworkController.</remarks>
        public int Index
        {
            get => _index;
        }

        /// <summary>
        /// Access player name.
        /// </summary>
        /// <remarks>This should only be changed by a network module.</remarks>
        public string Name
        {
            get => _name;
            set { _name = value; }
        }

        /// <summary>
        /// Access player colour.
        /// </summary>
        /// <remarks>This should only be changed by a network module.</remarks>
        public Color Color
        {
            get => _color;
            set { _color = value;}
        }

        /// <summary>
        /// Setup the player list.
        /// </summary>
        /// <param name="amount">Amount of players the list should contain.</param>
        public static void InitPlayers(int amount)
        {
            // Stupidity check
            if (_players.Count > 0) throw new System.ArgumentException("cannot create new player list while one already exists");

            // Setup players list
            for (int i=0; i<amount; i++)
            {
                Player p = new Player();
                p._index = i;
                p._name = "Player " + (i + 1);
                p._color = SwinGame.RandomRGBColor(255);
                _players.Add(p);
            }
        }

        /// <summary>
        /// Clear the player list.
        /// </summary>
        public static void TerminatePlayers()
        {
            _players.Clear();
        }

        /// <summary>
        /// Get the number of players in the static player list.
        /// </summary>
        public static int Count
        {
            get
            {
                return _players.Count;
            }
        }

        /// <summary>
        /// Set the index of the local player.
        /// </summary>
        /// <param name="i">Array index if the local player.</param>
        public static void SetLocalPlayerIndex(int i)
        {
            Player p = GetByIndex(i);
            if ((p != _localPlayer) && (p?._slotUsed == false))
            {
                if (_localPlayer != null) _localPlayer._slotUsed= false;
                _localPlayer = p;
                if (_localPlayer != null)_localPlayer._slotUsed = true;
            }
        }

        /// <summary>
        /// Get player using list index.
        /// </summary>
        /// <param name="i">Array index if the player.</param>
        /// <returns>Player at index, or null if out-of-range.</returns>
        public static Player GetByIndex(int i)
        {
            if ((0 <= i) && (i < _players.Count))
                return _players[i];
            else
                return null;
        }

        /// <summary>
        /// Find a free player slot and return it, if one exists.
        /// </summary>
        /// <returns>The newly allocated player slot, or null if none were available.</returns>
        public static Player AllocatePlayer()
        {
            foreach (Player p in _players)
            {
                if (!p._slotUsed)
                {
                    p._slotUsed = true;
                    return p;
                }
            }
            return null;
        }

        /// <summary>
        /// Release a previously allocated player slot. This is used if a player leaves the game.
        /// </summary>
        public void ReleasePlayer()
        {
            _slotUsed = false;
        }
    }
}
