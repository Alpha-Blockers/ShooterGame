using SwinGameSDK;

namespace ShooterGame
{
    /// <summary>
    /// Tiles are locations within a Map. They have a size and location, and can contain objects.
    /// </summary>
    class Tile
    {
        private const int TILE_SIZE = 20; // Used within property Width and Height

        /// <summary>
        /// Get tile size in the X-direction.
        /// </summary>
        public static int Width { get { return TILE_SIZE; } }

        /// <summary>
        /// Get tile size in the Y-direction
        /// </summary>
        public static int Height { get { return TILE_SIZE; } }

        /// <summary>
        /// Map tile constructor.
        /// </summary>
        public Tile()
        {
        }

        /// <summary>
        /// Draw tile at position.
        /// </summary>
        /// <param name="x">X-coordinate where tile is drawn.</param>
        /// <param name="y">Y-coordinate where tile is drawn.</param>
        public void Draw(int x, int y)
        {
            SwinGame.DrawRectangle(Color.LightGray, x, y, Width, Height);
        }
    }
}
