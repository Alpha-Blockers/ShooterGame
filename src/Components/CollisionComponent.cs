using static System.Math;

namespace ShooterGame
{
    public class CollisionComponent : IComponent
    {
        private Entity _entity;
        private int _radius;
        private bool _enabled;

        public CollisionComponent(int radius)
        {
            _radius = radius;
            Enabled = true;
        }

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
                    if (_entity != null) _entity.Collision = null;
                    _entity = value;
                    if (_entity != null) _entity.Collision = this;
                }
            }
        }

        /// <summary>
        /// Get or set the enabled status of this collision component.
        /// </summary>
        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    if (_enabled) Entity?.QueueForUpdate();
                }
            }
        }

        /// <summary>
        /// Check if this class should continue to be updated.
        /// </summary>
        public bool ShouldUpdate { get { return _enabled && (Entity != null); } }

        /// <summary>
        /// Update movement data.
        /// </summary>
        public void Update()
        {
            // Get required component
            PositionComponent position = Entity?.Position;
            MovementComponent movement = Entity?.Movement;
            if ((position == null) || (movement == null)) return;

            // Get parent chunk
            Chunk chunk = position.Chunk;
            if (chunk == null) return;

            // Get parent map
            Map map = chunk.Map;
            if (map == null) return;

            // Get index range of nearby chunks
            int xMinChunk = Max(0, chunk.XIndex - 1);
            int yMinChunk = Max(0, chunk.YIndex - 1);
            int xMaxChunk = Min(chunk.XIndex + 1, map.ChunksX - 1);
            int yMaxChunk = Min(chunk.YIndex + 1, map.ChunksY - 1);

            // Get size limits
            int edgeLeft = position.X - _radius;
            int edgeRight = position.X + _radius;
            int edgeTop = position.Y - _radius;
            int edgeBottom = position.Y + _radius;

            // Loop through nearby entities and check for collisions
            for (int x = xMinChunk; x <= xMaxChunk; x++)
            {
                for (int y = yMinChunk; y <= yMaxChunk; y++)
                {
                    // Create shorthand value for accessing nearby chunk
                    Chunk nearChunk = map.ChunkByIndex(x, y);

                    // Bound-check chunk to see if this entity overlaps it
                    if ((edgeRight >= nearChunk.Left) && (edgeLeft <= nearChunk.Right) &&
                        (edgeBottom >= nearChunk.Top) && (edgeTop <= nearChunk.Bottom))
                    {
                        // Check if tiles overlap the entity in the x-direction
                        for (int s=0; s<Chunk.TILES_PER_CHUNK; s++)
                        {
                            int tileLeft = nearChunk.Left + (s * Tile.Width);
                            int tileRight = tileLeft + Tile.Width;
                            if ((edgeRight >= tileLeft) && (edgeLeft <= tileRight))
                            {
                                // Check if tiles overlap the entity in the y-direction
                                for (int t = 0; t < Chunk.TILES_PER_CHUNK; t++)
                                {
                                    int tileTop = nearChunk.Top + (t * Tile.Height);
                                    int tileBottom = tileTop + Tile.Height;
                                    if ((edgeBottom >= tileTop) && (edgeTop <= tileBottom))
                                    {
                                        // Check if the overlapped tile is not passable
                                        if (nearChunk.TileByIndex(s, t).Passable == false)
                                        {
                                            // Get middle of tile
                                            int midTileX = (tileLeft + tileRight) / 2;
                                            int midTileY = (tileTop + tileBottom) / 2;

                                            // Check if the x or y direction has pushed further into the tile
                                            if (Abs(midTileX - position.X) > Abs(midTileY - position.Y))
                                            {
                                                // Push entity away from tile in x-direction
                                                if (midTileX < position.X)
                                                    movement.X = 1 + _radius + (Tile.Width / 2) - (position.X - midTileX);
                                                else
                                                    movement.X = -1 + (midTileX - position.X) - _radius - (Tile.Width / 2);
                                            }
                                            else
                                            {
                                                // Push entity away from tile in y-direction
                                                if (midTileY < position.Y)
                                                    movement.Y = 1 + _radius + (Tile.Height / 2) - (position.Y - midTileY);
                                                else
                                                    movement.Y = -1 + (midTileY - position.Y) - _radius - (Tile.Height / 2);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // Loop through entities in chunk
                    foreach (PositionComponent otherPosition in nearChunk.Entities)
                    {
                        // Make sure not to collide with self
                        if (position == otherPosition) continue;

                        // Get collision component of other entity
                        CollisionComponent otherCollision = otherPosition.Entity?.Collision;
                        if (otherCollision == null) continue;

                        // Get minimum clear distance
                        int clearDistance = _radius + otherCollision._radius;

                        // Get distance apart
                        int xDistance = position.X - otherPosition.X;
                        int yDistance = position.Y - otherPosition.Y;

                        // Check for collision
                        if (((xDistance * xDistance) + (yDistance * yDistance)) <= (clearDistance * clearDistance))
                        {
                            if (OnCollide(Entity, otherPosition.Entity) == false)
                            {
                                // OnCollide returned false, so skip further checking
                                x = xMaxChunk;
                                y = yMaxChunk;
                                break;
                            }
                        }
                    }
                }
            }
        }
        
        public bool OnCollide(Entity e1, Entity e2)
        {
            //MessageLog.Add("entity " + e1.ToString() + " collided with " + e2.ToString());
            return true;
        }

        /// <summary>
        /// Clear all component data and attempt to unlink from any external data.
        /// </summary>
        public void Destroy()
        {
            Enabled = false;
        }
    }
}
