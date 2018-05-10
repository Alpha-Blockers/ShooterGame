using static System.Math;

namespace ShooterGame
{
    class Map
    {

        private readonly int _chunksX;
        private readonly int _chunksY;
        private Chunk[,] _chunk;

        /// <summary>
        /// Map class constructor.
        /// </summary>
        /// <param name="chunksX">Size of map in X direction, measured in chunks.</param>
        /// <param name="chunksY">Size of map in Y direction, measured in chunks.</param>
        public Map(int chunksX, int chunksY)
        {
            // Save map size
            _chunksX = chunksX;
            _chunksY = chunksY;

            // Generate chunks
            _chunk = new Chunk[ChunksX, ChunksY];
            for (int x = 0; x < ChunksX; x++)
            {
                for (int y = 0; y < ChunksY; y++)
                {
                    _chunk[x, y] = new Chunk(this, x, y);
                }
            }

            // Set edge chunks and not passable
            for (int x = 0; x < ChunksX; x++)
            {
                Chunk upper = _chunk[x, 0];
                Chunk lower = _chunk[x, chunksY - 1];
                for (int s = 0; s < Chunk.TILES_PER_CHUNK; s++)
                {
                    upper.TileByIndex(s, 0).Passable = false;
                    lower.TileByIndex(s, Chunk.TILES_PER_CHUNK - 1).Passable = false;
                }
            }
            for (int y = 0; y < ChunksY; y++)
            {
                Chunk left = _chunk[0, y];
                Chunk right = _chunk[chunksX - 1, y];
                for (int t = 0; t < Chunk.TILES_PER_CHUNK; t++)
                {
                    left.TileByIndex(0, t).Passable = false;
                    right.TileByIndex(Chunk.TILES_PER_CHUNK - 1, t).Passable = false;
                }
            }
        }

        /// <summary>
        /// Get width of map (size in x-direction).
        /// </summary>
        public int Width { get { return Chunk.Width * ChunksX; } }

        /// <summary>
        /// Get height of map (size in y-direction).
        /// </summary>
        public int Height { get { return Chunk.Height * ChunksY; } }

        /// <summary>
        /// Get number of chunks in the X-direction.
        /// </summary>
        public int ChunksX { get => _chunksX; }

        /// <summary>
        /// Get number of chunks in the Y-direction.
        /// </summary>
        public int ChunksY { get => _chunksY; }

        /// <summary>
        /// Get tile using tile index coordinates.
        /// </summary>
        /// <param name="x">X index of tile.</param>
        /// <param name="y">Y index of tile.</param>
        /// <returns>Tile at the coordinate.</returns>
        public Chunk ChunkByIndex(int x, int y) { return _chunk[x, y]; }

        /// <summary>
        /// Find nearest chunk using an x,y coordinate.
        /// </summary>
        /// <param name="x">X-coordinate of the point.</param>
        /// <param name="y">Y-coordinate of the point.</param>
        /// <returns>The chunk either directly under the point, or the nearest chunk to the point.</returns>
        public Chunk NearestChunk(int x, int y)
        {
            return _chunk[
                Max(0, Min(ChunksX - 1, (int)(x / Chunk.Width))),
                Max(0, Min(ChunksY - 1, (int)(y / Chunk.Height)))];
        }


        /// <summary>
        /// Draw map to screen.
        /// </summary>
        public void Draw()
        {

            // Draw map background
            for (int x = 0; x < ChunksX; x++)
            {
                for (int y = 0; y < ChunksY; y++)
                {
                    _chunk[x, y].DrawBackground();
                }

            }

            // Draw map foreground
            for (int x = 0; x < ChunksX; x++)
            {
                for (int y = 0; y < ChunksY; y++)
                {
                    _chunk[x, y].DrawForeground();
                }

            }
        }
    }
}
