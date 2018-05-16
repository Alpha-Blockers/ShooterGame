
namespace ShooterGame
{
    class MovementComponent : IComponent
    {
        private Entity _entity;
        private int _x;
        private int _y;

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
                    if (_entity != null) _entity.Movement = null;
                    _entity = value;
                    if (_entity != null) _entity.Movement = this;
                }
            }
        }

        /// <summary>
        /// Get or set speed in x-direction
        /// </summary>
        public int X
        {
            get => _x;
            set
            {
                bool previouslyActive = Active;
                _x = value;
                if (!previouslyActive && Active) Entity?.QueueForUpdate();
            }
        }

        /// <summary>
        /// Get or set speed in y-direction.
        /// </summary>
        public int Y
        {
            get => _y;
            set
            {
                bool previouslyActive = Active;
                _y = value;
                if (!previouslyActive && Active) Entity?.QueueForUpdate();
            }
        }

        /// <summary>
        /// Check if the moveent movement component is currently active.
        /// </summary>
        public bool Active
        {
            get
            {
                return (_x != 0) || (_y != 0);
            }
        }

        /// <summary>
        /// Clear all component data and attempt to unlink from any external data.
        /// </summary>
        public void Destroy()
        {
            // Set values such that the Active method returns false
            _x = 0;
            _y = 0;
        }

        /// <summary>
        /// Update movement data.
        /// </summary>
        public void Update()
        {
            // Get position component
            PositionComponent position = Entity?.Position;
            if (position == null)
                throw new System.FormatException("Entities with a movement component must also have a position component");

            // Update related position component
            position.X = position.X + _x;
            position.Y = position.Y + _y;
        }
    }
}
