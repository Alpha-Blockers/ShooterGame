using SwinGameSDK;
using System.Collections.Generic;
using static System.Math;

namespace ShooterGame
{
    /// <summary>
    /// A map chunk is a small sub-section of the map which knows its size, location and parent map.
    /// Objects within the map are grouped by chunk to allow for fast collision checking.
    /// Objetcs cannot be larger than a single chunk in size.
    /// </summary>
    public class Chunk
    {
        /// <summary>
        /// Get the size of a chunk, measured in tiles.
        /// </summary>
        public const int TILES_PER_CHUNK = 8;

        private Map _map;
        private int _xIndex, _yIndex;
        private Tile[,] _tile;
        private List<PositionComponent> _entities;
        
        /// <summary>
        /// Get parent map.
        /// </summary>
        public Map Map { get => _map; }

        /// <summary>
        /// Get X index of this chunk (useful if used with paremt map).
        /// </summary>
        public int XIndex { get => _xIndex; }

        /// <summary>
        /// Get Y index of this chunk (useful if used with paremt map).
        /// </summary>
        public int YIndex { get => _yIndex; }

        /// <summary>
        /// Get width of chunk (size in x-direction).
        /// </summary>
        public static int Width { get { return Tile.Width * TILES_PER_CHUNK; } }

        /// <summary>
        /// Get height of chunk (size in y-direction).
        /// </summary>
        public static int Height { get { return Tile.Height * TILES_PER_CHUNK; } }

        /// <summary>
        /// Get location of the tile's left edge.
        /// </summary>
        public int Left { get { return _xIndex * Width; } }

        /// <summary>
        /// Get location of the tile's right edge.
        /// </summary>
        public int Right { get { return Left + Width; } }

        /// <summary>
        /// Get location of the tile's top edge.
        /// </summary>
        public int Top { get { return _yIndex * Height; } }

        /// <summary>
        /// Get location of the tile's bottom edge.
        /// </summary>
        public int Bottom { get { return Top + Height; } }

        /// <summary>
        /// Get whole location of the chunk.
        /// </summary>
        public Rectangle Location
        {
            get
            {
                Rectangle res = new Rectangle();
                res.X = Left;
                res.Y = Top;
                res.Width = Width;
                res.Height = Height;
                return res;
            }
        }

        /// <summary>
        /// Get tile using tile index.
        /// </summary>
        /// <param name="x">X index of tile.</param>
        /// <param name="y">Y index of tile.</param>
        /// <returns>Tile at the coordinate.</returns>
        public Tile TileByIndex(int x, int y) { return _tile[x, y]; }

        /// <summary>
        /// Find nearest tile using an x,y coordinate.
        /// </summary>
        /// <param name="x">X-coordinate of the point.</param>
        /// <param name="y">Y-coordinate of the point.</param>
        /// <returns>The tile either directly under the point, or the nearest tile to the point.</returns>
        public Tile NearestTile(int x, int y)
        {
            return _tile[
                Max(0, Min(TILES_PER_CHUNK - 1, x / Tile.Width)),
                Max(0, Min(TILES_PER_CHUNK - 1, y / Tile.Height))];
        }

        /// <summary>
        /// Map tile constructor.
        /// </summary>
        /// <param name="parent">Parent map.</param>
        /// <param name="xIndex">X-index of this chunk within the parent map's chunk array.</param>
        /// <param name="yIndex">Y-index of this chunk within the parent map's chunk array.</param>
        public Chunk(Map parent, int xIndex, int yIndex)
        {
            _map = parent;
            _xIndex = xIndex;
            _yIndex = yIndex;
            _tile = new Tile[TILES_PER_CHUNK, TILES_PER_CHUNK];
            _entities = new List<PositionComponent>();
            for (int x = 0; x < TILES_PER_CHUNK; x++)
            {
                for (int y = 0; y < TILES_PER_CHUNK; y++)
                {
                    _tile[x, y] = new Tile();
                }
            }
        }

        /// <summary>
        /// Draw background objects such as Tiles to screen.
        /// </summary>
        public void DrawBackground()
        {
            // Draw tiles within this chunk
            for (int x=0; x<TILES_PER_CHUNK; x++)
            {
                for (int y = 0; y < TILES_PER_CHUNK; y++)
                {
                    TileByIndex(x, y).Draw(Left + (x * Tile.Width), Top + (y * Tile.Height));
                }
            }

            // Draw chunk boundary (for debug)
            SwinGame.DrawRectangle(Color.Gray, Left, Top, Width, Height);
        }

        /// <summary>
        /// Draw foreground objects to screen.
        /// </summary>
        public void DrawForeground()
        {
            foreach (PositionComponent p in _entities)
            {
                p?.Entity?.Drawable?.Draw();
            }
        }

        /// <summary>
        /// Check x is within the bounds of this chunk.
        /// </summary>
        /// <param name="x">X-coordinate to be bound-checked.</param>
        /// <returns>True if x is over the chunk, or False if not.</returns>
        public bool BoundCheckX(float x) { return (Left <= x) && (x <= Right); }

        /// <summary>
        /// Check y is within the bounds of this chunk.
        /// </summary>
        /// <param name="y">Y-coordinate to be bound-checked.</param>
        /// <returns>True if y is over the chunk, or False if not.</returns>
        public bool BoundCheckY(float y) { return (Top <= y) && (y <= Bottom); }

        /// <summary>
        /// Check if the point is within the bounds of this chunk.
        /// </summary>
        /// <param name="x">X-coordinate to be bound-checked.</param>
        /// <param name="y">Y-coordinate to be bound-checked.</param>
        /// <returns>True if the point is within the chunk, or False if not.</returns>
        public bool BoundCheck(float x, float y) { return BoundCheckX(x) && BoundCheckY(y); }

        /// <summary>
        /// Access list of position components which are over this tile.
        /// </summary>
        public List<PositionComponent> Entities { get => _entities; }
    }
}
