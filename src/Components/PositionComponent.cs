
namespace ShooterGame
{
    class PositionComponent : IComponent
    {
        private Entity _entity;
        private int _x;
        private int _y;
        private Chunk _chunk;

        /// <summary>
        /// Get or set parent entity.
        /// </summary>
        public Entity Entity
        {
            get => _entity;
            set
            {
                if (_entity != value)
                {
                    if (_entity != null) _entity.Position = null;
                    _entity = value;
                    if (_entity != null) _entity.Position = this;
                }
            }
        }

        /// <summary>
        /// Position component constructor.
        /// </summary>
        /// <param name="map">Parent map within which this position is located.</param>
        /// <param name="x">X-coordinate of this position.</param>
        /// <param name="y">Y-coordiate of this position.</param>
        public PositionComponent(Map map, int x, int y)
        {
            _x = x;
            _y = y;
            _chunk = map.NearestChunk(x, y);
            _chunk.Entities.Add(this);
        }

        /// <summary>
        /// Get chunk which contains this position.
        /// </summary>
        public Chunk Chunk { get => _chunk; }

        /// <summary>
        /// Get or set X location of this position.
        /// </summary>
        public int X
        {
            get { return _x; }
            set
            {
                _x = value;
                UpdateParentChunk();
            }
        }

        /// <summary>
        /// Get or set X location of this position.
        /// </summary>
        public int Y
        {
            get { return _y; }
            set
            {
                _y = value;
                UpdateParentChunk();
            }
        }

        /// <summary>
        /// Update parent chunk. Check if this position is still within the parent chunk, and update as needed.
        /// </summary>
        private void UpdateParentChunk()
        {
            if (!_chunk.BoundCheck(X, Y))
            {
                // Save map on which this position is located
                Map m = _chunk.Map;

                // Remove from current chunk
                _chunk?.Entities.Remove(this);

                // Add to new chunk
                _chunk = m.NearestChunk(X, Y);
                _chunk.Entities.Add(this);
            }
        }

        /// <summary>
        /// Output string of position.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "(" + X + ", " + Y + ")";
        }

        /// <summary>
        /// Clear all component data and attempt to unlink from any external data.
        /// </summary>
        public void Destroy()
        {
            if (_chunk != null)
            {
                _chunk.Entities.Remove(this);
                _chunk = null;
            }
        }
    }
}
